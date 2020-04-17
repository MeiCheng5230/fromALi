using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Common.UEPay;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.Models;
using PXin.Facade.UEPay;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Winner.CU.Balance.Entities;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;
using Winner.EncodeDecode;
using Newtonsoft.Json.Linq;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 兑换
    /// </summary>
    public class ExchangeFacade : FacadeBase<PXinContext>
    {
        #region 获取基本数据
        /// <summary>
        /// 获取用户信息及专户dos余额
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public DosInfoDto GetDosInfo(int nodeid)
        {
            var now = DateTime.Now;
            var data1 = (from u in db.TnetReginfoSet
                         join p in db.TnetUserphotoSet on u.Nodeid equals p.Nodeid
                         join b in db.TblcUserPurseSet.Where(w => w.Ownerid == nodeid && w.Ownertype == 1 && w.Pursetype == 65 && w.Subid == 0 && w.Currencytype == 8) on u.Nodeid equals b.Ownerid into bb
                         from leftp in bb.DefaultIfEmpty()
                         join o in db.TnetOpenInfoSet.Where(w => now >= w.Fromtime && now <= w.Endtime && w.Typeid == 20001 && w.Nodeid == nodeid) on u.Nodeid equals o.Nodeid into op
                         from open in op.DefaultIfEmpty()
                         where u.Nodeid == nodeid
                         select new DosInfoDto
                         {
                             NodeName = u.Nodename,
                             ImgUrl = p.Appphoto,
                             Dos = leftp == null ? 0 : leftp.Balance-leftp.Freezevalue,
                             NodeCode = u.Nodecode,
                             IsOpenInfo = open == null ? 0 : 1,
                         });
            var data = data1.FirstOrDefault();
            if (data != null)
            {
                if (string.IsNullOrWhiteSpace(data.ImgUrl))
                {
                    data.ImgUrl = "/pic/icon/defaulthead.png";
                }
                data.ImgUrl = data.ImgUrl.Trim().StartsWith("http") ? data.ImgUrl : AppConfig.Userphoto + data.ImgUrl;
                data.Dos = data.Dos.Todecimal2();
            }
            return data;
        }
        /// <summary>
        /// 获取精品兑换列表
        /// </summary>
        public List<ChargeProductDto> GetChargeProductList()
        {
            var data = db.TpxinChargeProductSet.Where(w => w.Isdel == 0).OrderBy(o => o.Seqno).Select(s => new ChargeProductDto
            {
                Id = s.Id,
                Name = s.Name,
                Pdtvalue = s.Pdtvalue,
                Pic = s.Pic,
                Price = s.Price,
                Priceunit = s.Priceunit,
                Note = s.Note,
                TypeId = s.Typeid,
            }).ToList();
            return data;
        }
        /// <summary>
        /// 获取兑换商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TpxinChargeProduct GetChargeProducDetails(int id)
        {
            var data = db.TpxinChargeProductSet.FirstOrDefault(f => f.Id == id);
            return data;
        }
        #endregion

        #region 兑换商品
        /// <summary>
        /// 兑换商品
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ProductRecharge(ReqProductRecharge req)
        {
            var product = GetChargeProducDetails(req.ProductId);
            if (product == null)
            {
                Alert("查无此商品");
                return false;
            }
            TnetReginfo user = new TnetReginfo();
            if (!CheckPwd(req.Nodeid, req.PayPwd, ref user))
            {
                return false;
            }
            var result = false;
            switch (product.Typeid)
            {
                case 1:
                    result = SvRecharge(req, product);
                    break;
                case 2:
                    result = SvcCardRecharge(req, product);
                    break;
                case 3:
                    result = YGVipRecharge(req, product, user.Nodecode);
                    break;
                case 4:
                    result = PCNCertificationRecharge(req, product, user.Nodecode);
                    break;
                default:
                    Alert("不存在其他兑换");
                    break;
            }
            return result;
        }
        /// <summary>
        /// 兑换svc相信充值码
        /// </summary>
        /// <param name="req"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool SvcCardRecharge(ReqProductRecharge req, TpxinChargeProduct product)
        {
            try
            {
                var now = DateTime.Now;
                BeginTransfer();
                var result = CheckDataTransfer(product, req.Nodeid, req.Num, 2, "兑换svc充值码");
                if (!result)
                {
                    return false;
                }
                db.BeginTransaction();
                for (int i = 0; i < req.Num; i++)
                {
                    var cardNo = db.Database.SqlQuery<string>("select GetCZMNum czm from dual").FirstOrDefault();
                    var cardId = db.GetPrimaryKeyValue<TblcCentcard>();
                    db.TblcCentcardSet.Add(new TblcCentcard
                    {
                        Idno = cardId,
                        Cardno = cardNo,
                        Cardpwd = "0",
                        Ispwdrequired = 0,
                        Amount = product.Pdtvalue,
                        Expiredtime = now.AddYears(1),
                        Createdtime = now,
                        Areaid = "1",
                        Status = 1,
                        Usenodeid = req.Nodeid,
                        Productid = null,
                        Remarks = "svc相信充值码",
                        Period = "1",
                        Fromid = 3,
                    });
                    db.TblcCentcardHisSet.Add(new TblcCentcardHis
                    {
                        Idno = cardId,
                        Createtime = now,
                        Nodeid = req.Nodeid,
                        Note = "专户DOS兑换",
                        Opnodeid = req.Nodeid,
                        Typeid = 3,
                        Remarks = "新增svc相信充值码"
                    });

                }
                db.TpxinChargeHisSet.Add(new TpxinChargeHis
                {
                    Fkid = product.Id,
                    Amount = product.Price * req.Num,
                    Createtime = now,
                    Nodeid = req.Nodeid,
                    Note = " ",
                    Num = req.Num,
                    Price = product.Price,
                    Purseconfigid = product.Purseconfigid,
                    Status = 0,
                    Typeid = 2,
                    Outnodeid = req.Nodeid,
                    Remarks = $"兑换{product.Name}({product.Pdtvalue}面额)",

                });
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    EndTransfer(false);
                    db.Rollback();
                    Alert("提交失败");
                    return false;
                }
                EndTransfer(true);
                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                EndTransfer(false);
                db.Rollback();
                log.Error("兑换svc相信充值码异常：" + e);
                Alert("兑换失败");
                return false;
            }

        }

        /// <summary>
        /// 兑换优谷vip码
        /// </summary>
        /// <param name="req"></param>
        /// <param name="product"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public bool YGVipRecharge(ReqProductRecharge req, TpxinChargeProduct product, string nodeCode)
        {
            try
            {
                //验证优谷账号
                var ygUser = GetYGUserInfo(req.PnodeCode);
                if (ygUser == null)
                {
                    return false;
                }
                BeginTransfer();
                var result = CheckDataTransfer(product, req.Nodeid, req.Num, 4, "兑换优谷vip码");
                if (!result)
                {
                    return false;
                }
                db.BeginTransaction();
                db.TpxinChargeHisSet.Add(new TpxinChargeHis
                {
                    Fkid = product.Id,
                    Amount = product.Price * req.Num,
                    Createtime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Note = $"优谷账号：{req.PnodeCode}",
                    Num = req.Num,
                    Price = product.Price,
                    Purseconfigid = product.Purseconfigid,
                    Status = 0,
                    Typeid = 3,
                    Outnodeid = ygUser.NodeId,
                    Remarks = $"兑换{product.Name}",

                });
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    EndTransfer(false);
                    db.Rollback();
                    Alert("提交失败");
                    return false;
                }
                EndTransfer(true);
                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                EndTransfer(false);
                db.Rollback();
                log.Error("兑换优谷vip码异常：" + e);
                Alert("兑换失败");
                return false;
            }

        }

        /// <summary>
        /// 兑换p客认证码
        /// </summary>
        /// <param name="req"></param>
        /// <param name="product"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        public bool PCNCertificationRecharge(ReqProductRecharge req, TpxinChargeProduct product, string nodeCode)
        {
            try
            {
                //验证pcn账号
                var pcnUser = GetPCNUserInfo(req.PnodeCode,req.Nodeid);
                if (pcnUser == null)
                {
                    return false;
                }
                BeginTransfer();
                var result = CheckDataTransfer(product, req.Nodeid, req.Num, 5, "兑换pcn认证码");
                if (!result)
                {
                    return false;
                }
                db.BeginTransaction();
                db.TpxinChargeHisSet.Add(new TpxinChargeHis
                {
                    Fkid = product.Id,
                    Amount = product.Price * req.Num,
                    Createtime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Note = $"PCN账号：{req.PnodeCode}",
                    Num = req.Num,
                    Price = product.Price,
                    Purseconfigid = product.Purseconfigid,
                    Status = 0,
                    Typeid = 4,
                    Outnodeid = pcnUser.NodeId,
                    Remarks = $"兑换{product.Name}",

                });
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    EndTransfer(false);
                    db.Rollback();
                    Alert("提交失败");
                    return false;
                }
                EndTransfer(true);
                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                EndTransfer(false);
                db.Rollback();
                log.Error("兑换pcn认证码异常：" + e);
                Alert("兑换失败");
                return false;
            }
        }

        /// <summary>
        /// 兑换sv
        /// </summary>
        /// <param name="req"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool SvRecharge(ReqProductRecharge req, TpxinChargeProduct product)
        {
            try
            {
                BeginTransfer();
                //扣除专户dos
                var result = CheckDataTransfer(product, req.Nodeid, req.Num, 3, "兑换sv");
                if (!result)
                {
                    return false;
                }
                //增加sv
                Purse fromPurse = purseFactory.SystemPurseRand(req.Nodeid);
                Purse toPurse = new Purse(OwnerType.个人钱包, req.Nodeid, (PurseType)4, 0, new CurrencyType(2), wcfProxy);//sv用户钱包
                Currency currency = new Currency(toPurse.CurrencyType, req.Num * product.Pdtvalue);
                if (!ChargeTransfer(fromPurse, toPurse, currency, 3, "兑换sv"))
                {
                    return false;
                }
                db.BeginTransaction();
                db.TpxinChargeHisSet.Add(new TpxinChargeHis
                {
                    Fkid = product.Id,
                    Amount = product.Price * req.Num,
                    Createtime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Note = " ",
                    Num = req.Num,
                    Price = product.Price,
                    Purseconfigid = product.Purseconfigid,
                    Status = 0,
                    Typeid = 1,
                    Outnodeid = req.Nodeid,
                    Remarks = $"兑换{product.Name}",

                });
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    EndTransfer(false);
                    db.Rollback();
                    Alert("提交失败");
                    return false;
                }
                EndTransfer(true);
                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                EndTransfer(false);
                db.Rollback();
                log.Error("兑换sv异常：" + e);
                Alert("兑换失败");
                return false;

            }
        }
        /// <summary>
        /// 兑换历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<RechargeHisDto> GetTpxinChargeHisList(GetByPageBase req)
        {
            return db.TpxinChargeHisSet.Where(w => w.Nodeid == req.Nodeid).OrderByDescending(o => o.Createtime).Select(s => new RechargeHisDto
            {
                Hisid = s.Hisid,
                Amount = s.Amount,
                Createtime = s.Createtime,
                Num = s.Num,
                ProductName = s.Remarks,
                Remarks = s.Note,
                Typeid = s.Typeid,
            }).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
        }
        #endregion

        #region 转入  
        /// <summary>
        /// 是否开通专属账号
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public bool IsOpenInfo(int nodeId)
        {
            var now = DateTime.Now;
            return db.TnetOpenInfoSet.Where(w => w.Nodeid == nodeId && w.Typeid == 20001 && now >= w.Fromtime && now <= w.Endtime).Any();
        }
        /// <summary>
        /// 获取ue账号信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="payType"></param>
        /// <param name="resultCode">-5表示未绑定 -1异常</param>
        /// <returns></returns>
        public ChargeUserInfoDto GetUeInfo(int nodeId, int payType,out int resultCode)
        {
            try
            {
                resultCode = 1;
                var payConfig = GetPayConfig(payType);
                if (payConfig == null)
                {
                    return null;
                }
                var user = PxinCache.GetRegInfo(nodeId);
                if (user == null)
                {
                    Alert("获取用户基本信息失败");
                    return null;
                }
                var result = UeApi.GetUeUserInfo(payConfig.Accesskeyid, payConfig.Accesssecret, "6", user.Nodecode);
                log.Info("ue返回的用户信息：" + JsonConvert.SerializeObject(result));
               
                //在外层判断成功失败
                if (result.Result > 0)
                {
                    var data = new ChargeUserInfoDto();
                    data.NodeName= GetBlurryNodeName(result.Data.Nodename);
                    data.Phone = Regex.Replace(result.Data.Mobileno ?? "", "(\\d{3})(\\d{4})(\\d{4})", "$1****$3");
                    data.UeBalance = result.Data.Balance.Todecimal2();
                    data.NodeId = nodeId;
                    data.NodeCode = result.Data.Nodecode;
                    return data;
                }
                Alert("此账号未绑定ue");
                resultCode = -5;//ue返回的失败都算未绑定
                return null;
            }
            catch (Exception e)
            {
                log.Error("获取ue用户基本信息失败：" + e);
                Alert("获取ue用户基本信息失败");
                resultCode = -1;
                return null;
            }
        }
        /// <summary>
        /// 绑定ue账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public UeUserInfoDto BindingUeAccount(ReqBindingUe req)
        {
            try
            {
                var payConfig = GetPayConfig();
                if (payConfig == null)
                {
                    return null;
                }
                var user = PxinCache.GetRegInfo(req.Nodeid);
                if (user == null)
                {
                    Alert("获取用户基本信息失败");
                    return null;
                }
                var pwd= Convert.ToBase64String(Encoding.UTF8.GetBytes(req.UeNodePwd));
                var loginResult = UeApi.UeLogin(payConfig.Accesskeyid, payConfig.Accesssecret, req.UeNodeCode, pwd, 0);
                if (loginResult.Result <= 0)
                {
                    Alert(loginResult.Message);
                    return null;
                }
                var bingResult = UeApi.BindUE(payConfig.Accesskeyid, payConfig.Accesssecret, loginResult.Data.Nodeid, user.Nodecode, 6);
                if (bingResult.Result <= 0)
                {
                    Alert(bingResult.Message);
                    return null;
                }
                return new UeUserInfoDto
                {
                    NodeId = loginResult.Data.Nodeid,
                    NodeCode = loginResult.Data.Nodecode,
                    NodeName = GetBlurryNodeName(loginResult.Data.Nodename),
                };
            }
            catch (Exception e)
            {
                log.Error("绑定ue异常：" + e);
                Alert("绑定失败");
                return null;
            }
        }
        /// <summary>
        /// 开通专属账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OpenInfUeoDto OpenInfo(Reqbase req)
        {
            var userInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (userInfo == null)
            {
                Alert("获取用户基本信息失败");
                return null;
            }
            if (IsOpenInfo(req.Nodeid))
            {
                Alert("已经开通了专户dos账号");
                return null;
            }
            var result = UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, 50, 20004, req.Sid.ToString(), "相信app开通专属账号", "开通专属账号").Result;
            if (!result.IsSuccess)
            {
                Alert(result.Message);
                return null;
            }
            return new OpenInfUeoDto
            {
                ChargeStr = result.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(result.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = result.OrderNo
            };
        }
        /// <summary>
        /// 开通专属账号-支付回调
        /// </summary>
        /// <param name="uepayhis"></param>
        /// <returns></returns>
        public bool OpenInfo_Notice(TnetUepayhis uepayhis)
        {
            var userInfo = PxinCache.GetRegInfo(uepayhis.Nodeid);
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            try
            {
                var now = DateTime.Now;
                var _businessID = db.GetPrimaryKeyValue<TnetOpenInfo>();
                db.TnetOpenInfoSet.Add(new TnetOpenInfo
                {
                    Infoid = _businessID,
                    Amount = uepayhis.Amount,
                    Createtime = now,
                    Endtime = now.AddYears(1),
                    Fromtime = now,
                    Nodeid = uepayhis.Nodeid,
                    Payment = 1,
                    Remarks = "开通专户DOS",
                    Typeid = 20001,
                });
                uepayhis.BusinessId = _businessID;
                uepayhis.Status = 2;
                db.Entry(uepayhis).State = System.Data.Entity.EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                log.Error("开通专属账号-支付回调异常：" + e);
                Alert("开通专属账号-支付回调异常：" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 转入（UEDOS转入到专户DOS）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OpenInfUeoDto UeTransferInDos(ReqUeTransferIn req)
        {
            #region 直接调用ue接口
            //TnetReginfo info = new TnetReginfo();
            //if (!CheckPwd(nodeId, pwd, ref info))
            //{
            //    return false;
            //}
            //try
            //{
            //    BeginTransfer();
            //    //增加dos
            //    Purse fromPurse = purseFactory.SystemPurseRand(nodeId);
            //    Purse toPurse = purseFactory.DosDoubleBase(nodeId);//用户专户DOS
            //    Currency currency = new Currency(toPurse.CurrencyType, amount);
            //    if (!ChargeTransfer(fromPurse, toPurse, currency, 7, "UEDOS转入到专户DOS"))
            //    {
            //        return false;
            //    }
            //    var payConfig = GetPayConfig();
            //    if (payConfig == null)
            //    {
            //        return false;
            //    }
            //    //从ue扣除dos
            //    var result = UeApi.Recovery(payConfig.Accesskeyid, payConfig.Accesssecret, info.Nodecode, 6, 1, amount, toPurse.CurrencyType.CurrencyId, "UEDOS转入到相信专户DOS", 10071);
            //    if (result == null || result.Result <= 0)
            //    {
            //        //失败
            //        EndTransfer(false);
            //        var error = "UEDOS转入到相信专户DOS失败：" + result?.Message;
            //        log.Info(error);
            //        Alert(error);
            //        return false;
            //    }
            //    EndTransfer(true);
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    EndTransfer(false);
            //    log.Error("UEDOS转入到专户DOS异常：" + e);
            //    Alert("转入失败");
            //    return false;

            //}
            #endregion

            //转账到绑定的ue账号
            int resultcode = 0;
            var ueuser = GetUeInfo(req.Nodeid, 2, out resultcode);
            if (ueuser == null)
            {
                Alert("获取ue用户信息失败");
                return null;
            }
            var user = new TnetReginfo { Nodeid = ueuser.NodeId };
            var result = UePayHelper.DosWithUePay(db, user, 1, CurrencyType.DOS_矿沙, req.Amount, 20006, req.Sid.ToString(), "UEDOS转入到专户DOS", "UEDOS转入到相信专户DOS").Result;
            if (!result.IsSuccess)
            {
                Alert(result.Message);
                return null;
            }
            return new OpenInfUeoDto
            {
                ChargeStr = result.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(result.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = result.OrderNo
            };
        }
        /// <summary>
        /// 转入（UEDOS转入到专户DOS）-支付回调
        /// </summary>
        /// <param name="uepayhis"></param>
        /// <returns></returns>
        public bool UeTransferInDos_Notice(TnetUepayhis uepayhis)
        {
            var userInfo = PxinCache.GetRegInfo(uepayhis.Nodeid);
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            try
            {
                db.BeginTransaction();
                BeginTransfer();
                //增加dos
                Purse fromPurse = purseFactory.SystemPurseRand(uepayhis.Nodeid);
                Purse toPurse = purseFactory.DosDoubleBase(uepayhis.Nodeid);//用户专户DOS
                Currency currency = new Currency(toPurse.CurrencyType, uepayhis.Amount);
                (bool result,int businessID) = ChargeTransferReturnId(fromPurse, toPurse, currency, 7, "UEDOS转入到专户DOS");
               
                if (!result)
                {
                    db.Rollback();
                    return false;
                }
                uepayhis.BusinessId = businessID;
                uepayhis.Status = 2;
                db.Entry(uepayhis).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() <= 0)
                {
                    EndTransfer(false);
                    db.Rollback();
                    return false;
                }
                EndTransfer(true);
                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                log.Error("转入-支付回调异常：" + e);
                Alert("转入-支付回调失败");
                EndTransfer(false);
                db.Rollback();
                return false;
            }

        }

        /// <summary>
        /// 转出（专户DOS转出到UEDOS）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="amount"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool DosTransferOutUe(int nodeId, decimal amount, string pwd)
        {
            TnetReginfo info = new TnetReginfo();
            if (!CheckPwd(nodeId, pwd, ref info))
            {
                return false;
            }
            try
            {
               
                BeginTransfer();
                //扣除dos
                Purse toPurse = purseFactory.SystemPurseRand(nodeId);
                Purse fromPurse = purseFactory.DosDoubleBase(nodeId);//用户专户DOS
                Currency currency = new Currency(fromPurse.CurrencyType, amount);
                if (!ChargeTransfer(fromPurse, toPurse, currency, 6, "专户DOS转出到UEDOS"))
                {
                    return false;
                }
                var payConfig = GetPayConfig();
                if (payConfig == null)
                {
                    return false;
                }
                //转账到绑定的ue账号
                int resultcode = 0;
                var ueuser = GetUeInfo(nodeId,2, out resultcode);
                if (ueuser == null)
                {
                    Alert("获取ue用户信息失败");
                    return false;
                }
                //增加ue dos 扣除50%手续费
                var result = UeApi.Recharge(payConfig.Accesskeyid, payConfig.Accesssecret, ueuser.NodeCode, 1, (amount / 2), fromPurse.CurrencyType.CurrencyId, "6", ueuser.NodeCode, "相信专户DOS转入到UEDOS", 10070);
                if (result == null || result.Result <= 0)
                {
                    //失败
                    EndTransfer(false);
                    var error = "相信专户DOS转入到UEDOS：" + result?.Message;
                    log.Info(error);
                    Alert(error);
                    return false;
                }
                EndTransfer(true);
                return true;
            }
            catch (Exception e)
            {
                EndTransfer(false);
                log.Error("专户DOS转出到UEDOS异常：" + e);
                Alert("转出失败");
                return false;

            }
        }
        #endregion

        #region 转账及验证数据

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pwd"></param>
        /// <param name="regInfo"></param>
        /// <returns></returns>
        public bool CheckPwd(int nodeid, string pwd, ref TnetReginfo regInfo)
        {
            regInfo = PxinCache.GetRegInfo(nodeid);
            if (regInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            if (!CheckPayPwd(regInfo, pwd, false))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取支付配置
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        private TpcnUepayconfig GetPayConfig(int payType = 1)
        {
            var payConfig = db.TpcnUepayconfigSet.Where(w => w.Typeid == payType).FirstOrDefault();
            if (payConfig == null)
            {
                log.Info($"没有查询到此支付配置,payType:{payType}");
                Alert("支付配置不正确");
                return null;
            }
            return payConfig;
        }
        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="fromPurse"></param>
        /// <param name="toPurse"></param>
        /// <param name="currency"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool ChargeTransfer(Purse fromPurse, Purse toPurse, Currency currency, int reason, string remarks)
        {
            var transferResult = Transfer(fromPurse, toPurse, currency, reason, remarks);
            if (!transferResult.IsSuccess)
            {
                EndTransfer(false);
                log.Error($"转账失败;{remarks}:{transferResult.Message}");
                Alert("转账失败:" + transferResult.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 转账-返回转账id
        /// </summary>
        /// <param name="fromPurse"></param>
        /// <param name="toPurse"></param>
        /// <param name="currency"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private (bool,int) ChargeTransferReturnId(Purse fromPurse, Purse toPurse, Currency currency, int reason, string remarks)
        {
            var transferResult = Transfer(fromPurse, toPurse, currency, reason, remarks);
            if (!transferResult.IsSuccess)
            {
                EndTransfer(false);
                log.Error($"转账失败;{remarks}:{transferResult.Message}");
                Alert("转账失败:" + transferResult.Message);
                return (false,0);
            }
            return (true, transferResult.TransferId);
        }
        /// <summary>
        /// 验证数据+转账（转给系统）
        /// </summary>
        /// <param name="product"></param>
        /// <param name="nodeId"></param>
        /// <param name="num"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool CheckDataTransfer(TpxinChargeProduct product, int nodeId, int num, int reason, string remarks)
        {
            var purse = db.TnetPurseConfigSet.FirstOrDefault(f => f.Infoid == product.Purseconfigid);
            if (purse == null)
            {
                log.Info($"钱包配置为空；infoid：{product.Purseconfigid}；nodeid：{nodeId}");
                Alert("钱包配置为空");
                return false;
            }
            return ChargeTransfer(nodeId, purse, product.Price * num, reason, remarks);
        }
        /// <summary>
        /// 转账-扣款（转给系统）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="purse"></param>
        /// <param name="amount"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool ChargeTransfer(int nodeId, TnetPurseConfig purse, decimal amount, int reason, string remarks)
        {
            Purse toPurse = purseFactory.SystemPurseRand(nodeId);
            Purse fromPurse = new Purse(OwnerType.个人钱包, nodeId, (PurseType)purse.Pursetype, purse.Subid, new CurrencyType(purse.Currencytype), wcfProxy);
            Currency currency = new Currency(fromPurse.CurrencyType, amount);
            var result = ChargeTransfer(fromPurse, toPurse, currency, reason, remarks);
            return result;
        }
        /// <summary>
        ///  验证数据
        /// </summary>
        /// <param name="product"></param>
        /// <param name="nodeId"></param>
        /// <param name="num"></param>
        /// <param name="purse"></param>
        /// <returns></returns>
        private bool CheckData(TpxinChargeProduct product, int nodeId, int num, ref TnetPurseConfig purse)
        {
            purse = db.TnetPurseConfigSet.FirstOrDefault(f => f.Infoid == product.Purseconfigid);
            if (purse == null)
            {
                log.Info($"钱包配置为空；infoid：{product.Purseconfigid}；nodeid：{nodeId}");
                Alert("钱包配置为空");
                return false;
            }
            return true;
        }
        #endregion

        #region 获取优谷或pcn的用户信息
        /// <summary>
        /// 获取pcn用户信息
        /// </summary>
        /// <param name="nodeCode"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public ChargeUserInfoDto GetPCNUserInfo(string nodeCode,int nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeCode))
            {
               var ssoUser= db.TssoOpenUserSet.Where(w => w.Opentype == 4 && w.Nodeid == nodeId).FirstOrDefault();
                if (ssoUser == null)
                {
                    log.Info($"查询TssoOpenUser表无数据,Opentype:4,Nodeid:{nodeId}");
                    Alert("获取pcn用户失败");
                    return null;
                }
                nodeCode = ssoUser.Openid;
            }
            var data = GetUserInfo(nodeCode, AppConfig.PCNUserInfoUrl, ConfigurationManager.AppSettings["PcnAuthString"], 3);
            if (data == null)
            {
                return null;
            }
            return new ChargeUserInfoDto
            {
                NodeId = data.Nodeid,
                NodeName = GetBlurryNodeName(data.Nodename),
                Phone = Regex.Replace(data.Mobileno ?? "", "(\\d{3})(\\d{4})(\\d{4})", "$1****$3"),
                NodeCode = data.Nodecode,
            };

        }
        /// <summary>
        /// 获取优谷用户信息
        /// </summary>
        /// <param name="nodeCode"></param>
        public ChargeUserInfoDto GetYGUserInfo(string nodeCode)
        {
            if (string.IsNullOrWhiteSpace(nodeCode))
            {
                Alert("参数验证失败：nodecode");
                return null;
            }
            var data = GetUserInfo(nodeCode, AppConfig.YGUserInfoUrl, ConfigurationManager.AppSettings["PcnAuthString"], 4);
            if (data == null)
            {
                return null;
            }
            return new ChargeUserInfoDto
            {
                NodeId = data.Nodeid,
                NodeName = GetBlurryNodeName(data.Nodename),
                Phone = Regex.Replace(data.Mobileno ?? "", "(\\d{3})(\\d{4})(\\d{4})", "$1****$3"),
                NodeCode = data.Nodecode,
            };

        }
        /// <summary>
        /// 通过接口获取用户信息
        /// </summary>
        /// <param name="nodeCode"></param>
        /// <param name="url"></param>
        /// <param name="signStr">签名秘钥</param>
        /// <param name="typeid">类型 3：pcn 4：优谷</param>
        /// <returns></returns>
        private TnetReginfo GetUserInfo(string nodeCode, string url, string signStr, int typeid)
        {
            var http = HttpSimulation.Instance;
            var tm = DateTime.Now.ToString("yyyyMMddhhmmss");
            var sid = 81123;
            var sign = Helper.GetSign(3434909, sid, tm, signStr);
            var queryString = http.GetQueryString(new
            {
                nodecode = nodeCode,
                nodeid = 3434909,
                sid,
                tm,
                sign,
            }, false);
            try
            {
                log.Info($"通过接口获取用户信息：url：{url}；参数：{queryString}");
                var result = http.Request(url, queryString);
                log.Info($"通过接口返回用户信息：{result}");
                var info = new Respbase<TnetReginfo>();
                if (result != null)
                {
                    //兼容老版pcn
                    if (url.Contains("AuthApi.aspx"))
                    {
                        var obj = JsonConvert.DeserializeObject<Respbase<string>>(result);
                        if (obj.Result == 1)
                        {
                            var user = JsonConvert.DeserializeObject<TnetReginfo>(obj.Data);
                            return user;
                        }
                        else
                        {
                            Alert($"要兑换认证码的PCN账号\\手机号[{nodeCode}]不存在");
                            return null;
                        }
                    }
                    else
                    {
                        var resResult = JsonConvert.DeserializeObject<Respbase<TnetReginfo>>(result);
                        if (resResult.Result <= 0)
                        {
                            var message = typeid == 3 ? "要兑换认证码的PCN账号\\手机号" : "要兑换VIP的优谷账号\\手机号";
                            Alert($"{message}[{nodeCode}]不存在");
                            return null;
                        }
                        return resResult.Data;
                    }
                }
                else
                {
                    log.Error($"通过接口获取用户信息：返回信息为空");
                    Alert("获取用户信息失败");
                    return null;
                }

            }
            catch (Exception e)
            {
                log.Error("通过接口获取用户信息异常：" + e);
                Alert("获取用户信息失败");
                return null;
            }
        }
        private string GetBlurryNodeName(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
            {
                return "";
            }
            nodeName = Helper.FilterChar(nodeName);
            return nodeName.Substring(0, 1) + "**";
        }
        #endregion
    }
}
