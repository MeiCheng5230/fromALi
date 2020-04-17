using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Facade.Models.UserPurseReq;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Winner.CU.Balance.Entities;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.Balance.PurseFactory;
using Winner.CU.BalanceWcfClient;
using Winner.EncodeDecode;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class ThridPayFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 第三方获取Pcn账号校验
        /// </summary>
        public Respbase<ThirdPartyVerifyDto> ThirdPartyVerify(ThirdPartyVerifyReq req)
        {
            var entity = db.TpcnThirdPartnerSet.FirstOrDefault(x => x.Accesskeyid == req.SecretKey);
            if (entity == null)
            {
                return new Respbase<ThirdPartyVerifyDto> { Result = -1, Message = "secretkey不存在" };
            }
            ThirdPartyVerifyDto dto = new ThirdPartyVerifyDto
            {
                Logo = entity.Logo,
                Storename = entity.Storename
            };

            return new Respbase<ThirdPartyVerifyDto> { Result = 1, Message = "验证成功", Data = dto };
        }

        /// <summary>
        /// 第三方支付接口
        /// </summary>
        public Respbase<ThridPayDto> ThridPartyPay(ThridPayReq req)
        {
            TnetReginfo reginfo = HttpContext.Current.GetRegInfo();
            if (string.IsNullOrEmpty(reginfo.UserpwdBak))
            {
                log.Info("未设置支付密码,nodeid=" + reginfo.Nodeid);
                return new Respbase<ThridPayDto> { Result = -5, Message = "支付密码未设置" };
            }

            var partner = db.TpcnThirdPartnerSet.FirstOrDefault(x => x.Accesskeyid == req.SecretKey);
            if (partner == null)
            {
                return new Respbase<ThridPayDto> { Result = -2, Message = "secretkey不存在" };
            }
            byte[] c = Convert.FromBase64String(req.Pwd);
            string pwd = System.Text.Encoding.UTF8.GetString(c);
            if (!UserPwd.Check(pwd, reginfo.UserpwdBak, reginfo.Nodeid, reginfo.Nodecode))
            {
                log.Info("支付密码不正确,nodeid=" + reginfo.Nodeid);
                return new Respbase<ThridPayDto> { Result = -5, Message = "支付密码不正确" };
            }

            var payhis = db.TpcnThirdPayhisSet.FirstOrDefault(x => x.Orderno == req.Orderno);
            if (payhis != null && payhis.Hisid > 0)
            {
                log.Info("订单号已存在，orderno：" + req.Orderno);
                return new Respbase<ThridPayDto> { Result = -6, Message = "订单号已存在，orderno：" + req.Orderno };
            }
            db.BeginTransaction();
            //添加历史
            payhis = InitPcnThirdPayhis();
            payhis.Nodeid = reginfo.Nodeid;
            payhis.Partnerid = partner.Id;//-------------------------
            payhis.Amount = req.Amount;//人民币
            payhis.Orderno = req.Orderno;
            payhis.Paytype = req.PayType;
            payhis.Body = req.Body;
            payhis.Subject = req.Subject;
            payhis.Notifyurl = req.Noticeurl;
            payhis.Transferids = "0";//transferids.ToString();
            db.TpcnThirdPayhisSet.Add(payhis);
            if (db.SaveChanges() <= 0)
            {
                log.Info("写支付历史失败：" + db.Message + ",payhis:" + JsonConvert.SerializeObject(payhis));
                db.Rollback();
                return new Respbase<ThridPayDto> { Result = -8, Message = "写支付历史失败" };
            }
            int transferids = 0;

            //开启转账
            BeginTransfer();
            if (req.PayType == 3000)
            {
                //SV-相信
                if (!Transfer_SV(reginfo.Nodecode, req.Amount, req.Subject))
                {
                    db.Rollback();
                    EndTransfer(false);
                    return new Respbase<ThridPayDto> { Result = -1, Message = PromptInfo.Message };
                }
                //string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //string sign = Md5.SignString(time + AppConfig.AppSecurityString);
                //PurseFacade facade = new PurseFacade();
                //var ret = facade.ThridPartyPay_Pro(new ThridPartyPayReq { Nodecode = reginfo.Nodecode, Amount = req.Amount, Subject = req.Subject, ReqTime = time, Sign = sign });
                //if (ret.Result <= 0)
                //{
                //    db.Rollback();
                //    EndTransfer(false);
                //    return new Respbase<ThridPayDto> { Result = -1, Message = ret.Message };
                //}
                else
                {
                    transferids = TransferId;
                    //transferids = ret.Data.TransferId;
                }
            }else if (req.PayType == 3001)
            {
                //SV-V点
                if (!Transfer_VD(reginfo.Nodecode, req.Amount, req.Subject))
                {
                    db.Rollback();
                    EndTransfer(false);
                    return new Respbase<ThridPayDto> { Result = -1, Message = PromptInfo.Message };
                }
                else
                {
                    transferids = TransferId;
                }

            }
            else
            {
                db.Rollback();
                EndTransfer(false);
                return new Respbase<ThridPayDto> { Result = -1, Message = "参数错误" };
            }

            payhis.Transferids = transferids.ToString();
            if (db.SaveChanges() <= 0)
            {
                log.Info("修改支付历史失败：" + db.Message + ",payhis:" + JsonConvert.SerializeObject(payhis));
                db.Rollback();
                EndTransfer(false);
                return new Respbase<ThridPayDto> { Result = -8, Message = "修改支付历史失败" };
            }

            EndTransfer();
            db.Commit();
            ThridPayDto dto = new ThridPayDto
            {
                Orderno = req.Orderno,
                OrderPcn = payhis.Hisid.ToString()
            };
            PxinSerivce.EnqueueNotice(payhis.Hisid);

            return new Respbase<ThridPayDto> { Result = 1, Message = "支付成功", Data = dto };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<GetThridPayhisDto> GetThridPayhis(GetThridPayhisReq req)
        {
            TpcnThirdPartner partner = db.TpcnThirdPartnerSet.FirstOrDefault(x => x.Accesskeyid == req.SecretKey);
            if (partner == null)
            {
                return new Respbase<GetThridPayhisDto> { Result = -1, Message = "secretkey不存在" };
            }
            string Signature = Md5.SignString(req.SecretKey + req.Orderno + req.Timestamp + partner.Accesssecret);
            if (Signature != req.Signature)
            {
                return new Respbase<GetThridPayhisDto> { Result = -1, Message = "签名错误" };
            }
            var payhis = db.TpcnThirdPayhisSet.FirstOrDefault(x => x.Orderno == req.Orderno);
            if (payhis != null && payhis.Hisid > 0)
            {
                log.Info("订单号已存在，orderno：" + req.Orderno);
                return new Respbase<GetThridPayhisDto> { Result = -6, Message = "订单号已存在，orderno：" + req.Orderno };
            }
            var payHis = new GetThridPayhisDto { amount = payhis.Amount, body = payhis.Body, orderpcn = payhis.Hisid, createtime = payhis.Createtime, orderno = payhis.Orderno, paystatus = payhis.Paystatus, paytype = payhis.Paytype, subject = payhis.Subject };

            return new Respbase<GetThridPayhisDto> { Result = 1, Message = "验证成功",Data= payHis }; 
        }

        private TpcnThirdPayhis InitPcnThirdPayhis()
        {
            TpcnThirdPayhis payhis = new TpcnThirdPayhis();
            payhis.Createtime = DateTime.Now;
            payhis.Paystatus = 1;
            payhis.Storequest = 0;
            payhis.Nextnotifytime = DateTime.Now;
            return payhis;
        }

        private bool CheckSign()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sign = Md5.SignString(time + AppConfig.AppSecurityString);

            if (!DateTime.TryParse(time, out DateTime dateTime))
            {
                Alert("时间参数错误");
                return false;
            }
            DateTime now = DateTime.Now;
            if (now > dateTime.AddHours(1) || now < dateTime.AddHours(-1))
            {
                Alert("时间参数错误");
                return false;
            }
            var tsign = Md5.SignString(time + AppConfig.AppSecurityString);
            if (tsign != sign)
            {
                Alert("签名错误");
                return false;
            }
            return true;
        }

        private int TransferId;
        /// <summary>
        /// SV扣除
        /// </summary>
        /// <param name="nodecode">用户账号</param>
        /// <param name="amount">金额</param>
        /// <param name="subject">商品名</param>
        /// <returns></returns>
        private bool Transfer_SV(string nodecode,decimal amount,string subject)
        {
            if (!CheckSign())
            {
                return false;
            }

            //TssoOpenUser openUser = db.TssoOpenUserSet.Where(d => d.Openid == nodecode && d.Opentype == 4).FirstOrDefault();
            //if (openUser == null)
            //{
            //    Alert("没有绑定PCN账号");
            //    return false;
            //}
            TnetReginfo reginfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == nodecode || c.Mobileno == nodecode);
            if(reginfo == null)
            {
                Alert("用户账号不存在");
                return false;
            }
            PurseFactory purseFactory = new PurseFactory(wcfProxy);
            Purse fromPurse = new Purse(OwnerType.个人钱包, reginfo.Nodeid, PurseType.现金钱包, CurrencyType.RMB, wcfProxy);
            Purse toPurse = purseFactory.SystemPurseRand(reginfo.Nodeid);
            Currency currency = new Currency(CurrencyType.RMB, amount);
            if (fromPurse.UsableBalance < currency.ConvertTo(fromPurse.CurrencyType).Amount)
            {
                Alert("余额不足");
                log.Info("支付转账失败：余额不足," + fromPurse.UsableBalance);
                return false;
            }
            int reasonid = 33180;//广州豪盾游戏充值
            TransferResult transferResult;
            transferResult = wcfProxy.Transfer(fromPurse, toPurse, currency, reasonid, subject);
            if (!transferResult.IsSuccess)
            {
                Alert("转账失败");
                log.Info("支付转账失败：" + transferResult.Message);
                return false;
            }
            TransferId = transferResult.TransferId;

            return true;
        }

        /// <summary>
        /// V点扣除
        /// </summary>
        /// <param name="nodecode">用户账号</param>
        /// <param name="amount">金额</param>
        /// <param name="subject">商品名</param>
        /// <returns></returns>
        private bool Transfer_VD(string nodecode, decimal amount, string subject)
        {
            if (!CheckSign())
            {
                return false;
            }

            //TssoOpenUser openUser = db.TssoOpenUserSet.Where(d => d.Openid == nodecode && d.Opentype == 4).FirstOrDefault();
            //if (openUser == null)
            //{
            //    Alert("没有绑定PCN账号");
            //    return false;
            //}
            TnetReginfo reginfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == nodecode || c.Mobileno == nodecode);
            if (reginfo == null)
            {
                Alert("用户账号不存在");
                return false;
            }
            var user = db.TpxinUserinfoSet.Where(c => c.Nodeid == reginfo.Nodeid).FirstOrDefault();
            if (user == null)
            {
                Alert("用户不存在");
                return false;
            }

            //if (user.V < amount * 10)
            //{
            //    Alert("余额不足");
            //    return false;
            //}

            //比例1:10
            //decimal before = user.V;
            //user.V -= amount * 10;
            ////添加金额变化记录
            //var amountChangeHis = new TpxinAmountChangeHis()
            //{
            //    Nodeid = user.Nodeid,
            //    Typeid = 1, 
            //    Amount = -amount * 10,
            //    Reason = 9,
            //    Transferid = Guid.NewGuid().ToString(),
            //    Createtime = DateTime.Now,
            //    Remarks = subject,
            //    Amountbefore = before,
            //    Amountafter = user.V
            //};
            //db.TpxinAmountChangeHisSet.Add(amountChangeHis);
            //if (db.SaveChanges() <= 0)
            //{
            //    Alert("V点失败");
            //    return false;
            //}

            //TransferId = amountChangeHis.Hisid;
            #region 调用VP接口扣除V点
            var id = db.GetPrimaryKeyValue<TpxinAmountChangeHis>();
            var req= new VPChargeVDian()
            {
                Amount=-amount*10,
                Nodeid=user.Nodeid,
                Reason=9,
                Remark=subject,
                Transferid=id.ToString(),
            };
            var vp = new VPHelper();
            var result= vp.SetV(req);
            if (result.Result <= 0)
            {
                Alert(result.Message);
                log.Error($"Transfer_VD扣减V点失败,Nodeid:{user.Nodeid};原因:{result.Message}");
                return false;
            }
            TransferId = id;
            #endregion
            return true;
        }

    }
}
