using System;
using System.Collections.Generic;
using System.Linq;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.UserPurseReq;
using PXin.Model;
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
    public class PurseFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 获取转账原因图标列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PurseHisTypeLogoDto> GetPurseHisTypeLogo(Reqbase req)
        {
            List<PurseHisTypeLogoDto> dtos = new List<PurseHisTypeLogoDto>();
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 0 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = -1 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 1 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 2 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 3 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 4 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 5 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 6 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 7 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 2000 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 2001 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 3000 });
            dtos.Add(new PurseHisTypeLogoDto() { TypeID = 3001 });
            return dtos;
        }

        /// <summary>
        /// 钱包列表
        /// </summary>
        public Respbase<List<PurseDto>> GetPurses(Reqbase req)
        {
            var query = (from purseConfig in db.TnetPurseConfigSet
                         join purse in db.TblcUserPurseSet on new { Pursetype = purseConfig.Pursetype, Subid = purseConfig.Subid, Currencytype = purseConfig.Currencytype } equals new { Pursetype = purse.Pursetype, Subid = purse.Subid, Currencytype = purse.Currencytype }
                         join currency in db.TblcCurrencySet on purseConfig.Currencytype equals currency.Currencyid
                         join currency2 in db.TblcCurrencySet on purseConfig.Showunit equals currency2.Currencyid
                         where purse.Ownertype == 1 && purse.Ownerid == req.Nodeid && purseConfig.Isshow >= 1
                         orderby purseConfig.Isshow descending, purseConfig.Infoid descending
                         select new PurseDto
                         {
                             Id = purseConfig.Infoid,
                             Purseid = purse.Purseid,
                             Pursename = purseConfig.Showname,
                             Balance = (purse.Balance - purse.Freezevalue) * currency2.ExchangeRate / currency.ExchangeRate,
                             Freeze = (purse.Freezevalue - purse.Minvalue) * currency2.ExchangeRate / currency.ExchangeRate,
                             Purseunit = purseConfig.Showunit,
                             Purseunitname = purseConfig.Showunitname,
                             IconUrl = purseConfig.Picurl,
                             Note = purseConfig.Note,
                             IsShow = purseConfig.Isshow,
                             PurseType = purseConfig.Pursetype,
                             Subid = purseConfig.Subid,
                             BgPic = purseConfig.Bgpic
                         });
            List<PurseDto> purselist = query.ToList();
            //var tpxinUserInfo = db.TpxinUserinfoSet.FirstOrDefault(w => w.Nodeid == req.Nodeid);
            var vp = new VPHelper();
            var tpxinUserInfo= vp.GetTpxinUserinfo(req.Nodeid);
            var VBalance = (tpxinUserInfo == null ? 0 : tpxinUserInfo.VDianBalance);
            var PBalance = (tpxinUserInfo == null ? 0 : tpxinUserInfo.PDianBalance);
            query = db.TnetPurseConfigSet.Where(w => w.Infoid < 0 && w.Isshow == 1).Select(s => new PurseDto()
            {
                Id = s.Infoid,
                Purseid = s.Infoid,
                Pursename = s.Showname,
                Balance = s.Infoid == -1 ? VBalance : PBalance,
                Freeze = 0,
                Purseunit = s.Showunit,
                Purseunitname = s.Showunitname,
                IconUrl = s.Picurl,
                Note = s.Note,
                IsShow = s.Isshow,
                PurseType = s.Pursetype,
                Subid = s.Subid,
                BgPic = s.Bgpic
            });
            query.ToList().ForEach(item => purselist.Add(item));
            purselist.ForEach(item =>
            {
                item.Balance = Math.Truncate(item.Balance * 100) / 100.0M;
                item.Freeze = Math.Truncate(item.Freeze * 100) / 100.0M;
                //item.DetailUrl = string.Format("http://client.p.cn/html/balanceinfo.aspx?purseid={0}&title={1}收支明细", item.Purseid, item.Pursename);
            });
            return new Respbase<List<PurseDto>>
            {
                Data = purselist.OrderByDescending(p => p.Balance).ToList()
            };
        }

        /// <summary>
        /// 获取首页钱包数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<List<List<Purse2Dto>>> GetPurses2(Reqbase req)
        {
            List<List<Purse2Dto>> dto = new List<List<Purse2Dto>>();
            var pxinuser = db.TpxinUserinfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            var vp = new VPHelper();
            var tpxinUserInfo = vp.GetTpxinUserinfo(req.Nodeid);
            var parm = $"nodeid={req.Nodeid}&sid={req.Sid}&tm={req.Tm}&sign={req.Sign}#/";
            var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            List<Purse2Dto> dto1 = new List<Purse2Dto>
            {
                new Purse2Dto
                {
                    Balance = db.TblcCentcardSet.Where(c => c.Usenodeid == req.Nodeid && c.Status == 1 && c.Areaid == "1").Count().ToString(),
                    Name = "充值码",
                    ClickUrl = baseUrl+"/App/Believe/index.html?"+parm+"RechargeCode"
                },

                new Purse2Dto
                {
                    Balance = db.Ttqm2InfoSet.Where(a => a.Status == 0 && a.Nodeid == req.Nodeid).Count().ToString(),
                    Name = "提取码",
                    ClickUrl = AppConfig.PMDomain+"/App/Extracted/Index.html?"+parm
                },

                new Purse2Dto
                {
                    Balance = pxinuser.Apoint.ToString("0.00"),
                    Name = "A点",
                    ClickUrl = baseUrl+"/App/Believe/index.html?"+parm+"auction"
                },

                new Purse2Dto
                {
                    Balance = GetBalance(65, req.Nodeid, 0, 8).ToString("0.00"),
                    Name = "专户DOS",
                    ClickUrl =baseUrl+"/App/Believe/index.html?"+parm+"exchange"
                }
            };
            dto.Add(dto1);

            List<Purse2Dto> dto2 = new List<Purse2Dto>
            {
                new Purse2Dto
                {
                    Balance = tpxinUserInfo.PDianBalance.ToString("0.00"),
                    Name = "P点",
                    ClickUrl = "-2"
                },

                new Purse2Dto
                {
                    Balance = tpxinUserInfo.VDianBalance.ToString("0.00"),
                    Name = "V点",
                    ClickUrl = "-1"
                },

                new Purse2Dto
                {
                    Balance = GetBalance(4, req.Nodeid, 0, 2).ToString("0.00"),
                    Name = "SV",
                    ClickUrl = "4"
                },

                //new Purse2Dto
                //{
                //     Balance=GetBalance(3,req.Nodeid,11,4).ToString("0.00"),
                //     Name="UV",
                //     ClickUrl="3"
                //}
            };

            dto.Add(dto2);
            return new Respbase<List<List<Purse2Dto>>>
            {
                Data = dto
            };
        }

        /// <summary>
        /// 获取首页钱包数据3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<List<List<Purse3Dto>>> GetPurses3(Reqbase req)
        {
            List<List<Purse3Dto>> dto = new List<List<Purse3Dto>>();
            var pxinuser = db.TpxinUserinfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            var vp = new VPHelper();
            var tpxinUserInfo = vp.GetTpxinUserinfo(req.Nodeid);
            var parm = $"nodeid={req.Nodeid}&sid={req.Sid}&tm={req.Tm}&sign={req.Sign}#/";
            var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            List<Purse3Dto> dto1 = new List<Purse3Dto>
            {
                new Purse3Dto
                {
                    Balance = db.TblcCentcardSet.Where(c => c.Usenodeid == req.Nodeid && c.Status == 1 && c.Areaid == "1").Count().ToString(),
                    Name = "充值码",
                    ClickUrl = baseUrl+"/App/Believe/index.html?"+parm+"RechargeCode"
                },

                new Purse3Dto
                {
                    Balance = db.Ttqm2InfoSet.Where(a => a.Status == 0 && a.Nodeid == req.Nodeid).Count().ToString(),
                    Name = "提取码",
                    ClickUrl = AppConfig.PMDomain+"/App/Extracted/Index.html?"+parm
                },

                new Purse3Dto
                {
                    Balance = pxinuser.Apoint.ToString("0.00"),
                    Name = "A点",
                    ClickUrl = baseUrl+"/App/Believe/index.html?"+parm+"auction"
                },

                new Purse3Dto
                {
                    Balance = GetBalance(65, req.Nodeid, 0, 8).ToString("0.00"),
                    Name = "专户DOS",
                    ClickUrl =baseUrl+"/App/Believe/index.html?"+parm+"exchange"
                }
            };
            dto.Add(dto1);

            List<Purse3Dto> dto2 = new List<Purse3Dto>
            {
                new Purse3Dto
                {
                    Balance = tpxinUserInfo.PDianBalance.ToString("0.00"),
                    Name = "P点",
                    PurseType=-2,
                    Subid=0
                },

                new Purse3Dto
                {
                    Balance = tpxinUserInfo.VDianBalance.ToString("0.00"),
                    Name = "V点",
                    PurseType=-1,
                    Subid=0
                },

                new Purse3Dto
                {
                    Balance = GetBalance(4, req.Nodeid, 0, 2).ToString("0.00"),
                    Name = "SV",
                    PurseType=4,
                    Subid=0
                },

                new Purse3Dto
                {
                     Balance=GetBalance(3,req.Nodeid,11,4).ToString("0.00"),
                     Name="UV",
                     PurseType=3,
                     Subid=11
                }
            };

            dto.Add(dto2);
            return new Respbase<List<List<Purse3Dto>>>
            {
                Data = dto
            };
        }

        /// <summary>
        /// 获取指定钱包账单记录
        /// </summary>
        public Respbase<List<UserPurseHisDto>> GetUserPurseHis(PurseHisReq req)
        {
            List<UserPurseHisDto> hisList = null;
            if (req.Purseid > 0)
            {
                //string resultStr = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.WsxServiceAPIHost}/api/Cache/GetUserPurseHis", req);
                //HttpResult<List<UserPurseHisDto>> result = JsonConvert.DeserializeObject<HttpResult<List<UserPurseHisDto>>>(resultStr);
                //if (result.Result != 1)
                //{
                //    log.Info($"获取指定钱包账单记录失败,req={JsonConvert.SerializeObject(req)},{result.Message}");
                //}
                //hisList = result.Result == 1 ? result.Data : null;
                //if (hisList != null)
                //{
                //    hisList.ForEach(item => item.TypeID = item.Amount > 0 ? 0 : -1);
                //}
                hisList =(from tph in db.TblcUserPurseHisSet.Where(p => p.Purseid == req.Purseid)
                         join tp in db.TblcUserPurseSet on tph.Purseid equals tp.Purseid
                         join tcg in db.TnetPurseConfigSet on new { tp.Pursetype,tp.Subid} equals new { tcg.Pursetype,tcg.Subid}
                         join tc in db.TblcCurrencySet on tcg.Currencytype equals tc.Currencyid
                         join tc2 in db.TblcCurrencySet on tcg.Showunit equals tc2.Currencyid
                         select new UserPurseHisDto
                        {
                            Amount = tph.Amount,
                             _balanceafter = (tph.BalanceAfter*tc2.ExchangeRate/tc.ExchangeRate),
                            CreateTime = tph.Createtime,
                            Remark = tph.Remarks,
                            CurrType=tp.Currencytype
                        }).OrderByDescending(p => p.CreateTime).Skip((req.PageIndex - 1) * req.PageSize).Take(req.PageSize).ToList();
                        hisList.ForEach(item =>
                        {
                            item.TypeID = item.Amount > 0 ? 0 : -1;
                            item.Amount = Math.Round(UnitConversion(item.Amount, item.CurrType), 2);
                        });
            }
            if (req.Purseid < 0)
            {
                var purseConfig = db.TnetPurseConfigSet.FirstOrDefault(f => f.Infoid == req.Purseid);
                if (purseConfig != null)
                {
                    //V点，P点钱包配置里面为负数
                    int purseType = Math.Abs(purseConfig.Pursetype);
                    hisList = db.TpxinAmountChangeHisSet
                        .Where(w => w.Typeid == purseType && w.Nodeid == req.Nodeid)
                        .OrderByDescending(o => o.Createtime)
                        .Skip((req.PageIndex - 1) * req.PageSize)
                        .Take(req.PageSize)
                        .Select(s => new UserPurseHisDto()
                        {
                            TypeID = s.Reason,
                            Amount = s.Amount,
                            _balanceafter = s.Amountafter,
                            CreateTime = s.Createtime,
                            Remark = s.Remarks
                        }).ToList();
                }
            }
            return this.Ok("获取指定钱包账单记录成功", hisList);
        }

        /// <summary>
        /// 将资金回收到系统
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<RecoveryDto> Recovery(PurseRecoveryReq req)
        {
            TnetReginfo tnet_reginfo = null;
            if (req.Nodeid > 0)
            {
                tnet_reginfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { KeyType = 0, RegInfoKey = req.Nodeid.ToString() });
            }
            else
            {
                tnet_reginfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { KeyType = 1, RegInfoKey = req.Nodecode.ToString() });
            }
            if (!UserPwd.Check(req.Paypwd, tnet_reginfo.UserpwdBak, tnet_reginfo.Nodeid, tnet_reginfo.Nodecode))
            {
                return this.Fail<RecoveryDto>("支付密码错误");
            }
            if (tnet_reginfo == null)
            {
                log.Info($"Recovery接口,获取用户信息失败,UE账号={req.Nodecode}");
                return this.Fail<RecoveryDto>("获取用户信息失败");
            }

            if (req.Reason <= 0)
            {
                log.Info($"Recovery接口,参数错误,Reason={req.Reason}");
                return this.Fail<RecoveryDto>("参数错误");
            }

            //检查typeid
            TnetPurseConfig purse_config = db.TnetPurseConfigSet.Where(c => c.Infoid == req.Pursetype).FirstOrDefault();
            if (purse_config == null)
            {
                log.Info($"Recovery接口,获取钱包信息失败,Infoid={req.Pursetype}");
                return this.Fail<RecoveryDto>("获取钱包信息失败");
            }

            //检查完毕,开始转账
            BalanceWcfProxy wcfProxy = new BalanceWcfProxy(db.NewTradeId);
            if (!wcfProxy.OpenSession())
            {
                return this.Fail<RecoveryDto>($"不能建立会话,{wcfProxy.PromptInfo}");
            }
            if (!wcfProxy.BeginTransaction())
            {
                wcfProxy.CloseSession();
                return this.Fail<RecoveryDto>($"事务开启失败,{wcfProxy.PromptInfo}");
            }
            PurseFactory purseFactory = new PurseFactory(wcfProxy);
            Purse fromPurse = new Purse((OwnerType)1, tnet_reginfo.Nodeid, (PurseType)purse_config.Pursetype, purse_config.Subid, new CurrencyType(purse_config.Currencytype), wcfProxy);
            Purse toPurse = purseFactory.SystemPurseRand(tnet_reginfo.Nodeid);//到系统钱包
            Currency currency = new Currency(CurrencyType.RMB, req.Amount);
            if (req.currencytype > 0)
            {
                currency = new Currency(new CurrencyType(req.currencytype), req.Amount);
            }
            //转账
            log.Info($"Recovery接口,开始回收,frompurseid={fromPurse.PurseId},到钱包ID={toPurse.PurseId},金额={currency.Amount},单位={currency.CurrencyUnit}");
            TransferResult transferResult = wcfProxy.Transfer(fromPurse, toPurse, currency, req.Reason, req.Remarks);
            if (!transferResult.IsSuccess)
            {
                log.Info("Recovery接口,转账失败:" + transferResult.Message);
                wcfProxy.Rollback();
                wcfProxy.CloseSession();
                return this.Fail<RecoveryDto>(transferResult.Message);
            }
            log.Info($"Recovery接口,回收成功,账号={tnet_reginfo.Nodecode},类型={req.Pursetype},金额={currency.Amount},单位={currency.CurrencyUnit},备注={req.Remarks}");
            if (!wcfProxy.Commit())
            {
                wcfProxy.CloseSession();
                return this.Fail<RecoveryDto>("转账提交失败");
            }
            return this.Ok("操作成功", new RecoveryDto { TransferId = transferResult.TransferId.ToString() });
        }

        /// <summary>
        /// 第三方支付
        /// </summary>
        public Respbase<ThridPartyPayDto> ThridPartyPay(ThridPartyPayReq req)
        {
            BeginTransfer();
            var result = ThridPartyPay_Pro(req);
            if (result.Result <= 0)
            {
                EndTransfer(false);
                return result;
            }

            EndTransfer(true);
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<ThridPartyPayDto> ThridPartyPay_Pro(ThridPartyPayReq req)
        {
            if (!CheckSign(req.ReqTime, req.Sign))
            {
                return new Respbase<ThridPartyPayDto> { Result = -1, Message = PromptInfo.Message, Data = null };
            }

            TssoOpenUser openUser = db.TssoOpenUserSet.Where(c => c.Openid == req.Nodecode && c.Opentype == 4).FirstOrDefault();
            if (openUser == null)
            {
                return new Respbase<ThridPartyPayDto> { Result = -1, Message = "没有绑定PCN账号", Data = null };
            }

            PurseFactory purseFactory = new PurseFactory(wcfProxy);
            Purse fromPurse = new Purse(OwnerType.个人钱包, openUser.Nodeid, PurseType.现金钱包, CurrencyType.RMB, wcfProxy);
            Purse toPurse = purseFactory.SystemPurseRand(openUser.Nodeid);
            Currency currency = new Currency(CurrencyType.RMB, req.Amount);
            if (fromPurse.UsableBalance < currency.ConvertTo(fromPurse.CurrencyType).Amount)
            {
                log.Info("支付转账失败：余额不足," + fromPurse.UsableBalance);
                return new Respbase<ThridPartyPayDto> { Result = -1, Message = "余额不足", Data = null };
            }
            int reasonid = 33180;//广州豪盾游戏充值
            TransferResult transferResult;
            transferResult = wcfProxy.Transfer(fromPurse, toPurse, currency, reasonid, req.Subject);
            if (!transferResult.IsSuccess)
            {
                log.Info("支付转账失败：" + transferResult.Message);
                return new Respbase<ThridPartyPayDto> { Result = -1, Message = "转账失败", Data = null };
            }

            if (PromptInfo.Result <= 0)
            {
                return new Respbase<ThridPartyPayDto> { Result = -1, Message = PromptInfo.Message };
            }
            return new Respbase<ThridPartyPayDto> { Result = 1, Message = "成功", Data = new ThridPartyPayDto { TransferId = transferResult.TransferId } };

        }

        /// <summary>
        /// 获取UV充值记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<List<UVChargeHisDto>> GetUVChargeHis(UVRechargeHisReq req)
        {
            string sql = string.Format(@"select Transid,Amount,Createtime,BalanceAfter1,ShowName from (
        select t1.*,t2.balance_after/100 BalanceAfter1,t1.remarks ShowName,ROWNUM AS rowno from Tfin_Billtrans t1 
        left join tblc_user_purse_his t2 on t1.transferids=t2.transferid
       where t1.paytype=4 and t1.status=1 and t1.chargetype=20 and t1.nodeid={0} and t2.ownerid={0}  AND ROWNUM <= {2} order by t1.createtime desc)ttt WHERE ttt.rowno > {1}", req.Nodeid, (req.PageIndex - 1) * req.PageCount, req.PageIndex * req.PageCount);
            var result = db.Database.SqlQuery<UVChargeHisDto>(sql).ToList();

            return new Respbase<List<UVChargeHisDto>>() { Data = result };
        }

        private bool CheckSign(string reqTime, string sign)
        {
            if (!DateTime.TryParse(reqTime, out DateTime dateTime))
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
            var tsign = Md5.SignString(reqTime + AppConfig.AppSecurityString);
            if (tsign != sign)
            {
                Alert("签名错误");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取对应钱包余额
        /// </summary>
        /// <param name="pursetype"></param>
        /// <param name="ownerid"></param>
        /// <param name="subid"></param>
        /// <param name="currencytype"></param>
        /// <returns></returns>
        private decimal GetBalance(int pursetype, int ownerid, int subid, int currencytype)
        {
            var Purse = db.TblcUserPurseSet.FirstOrDefault(x => x.Pursetype == pursetype && x.Ownertype == (int)OwnerType.个人钱包 && x.Ownerid == ownerid && x.Subid == subid && x.Currencytype == currencytype);
            if (Purse != null)
            {
                return UnitConversion(Purse.Balance - Purse.Freezevalue, currencytype);
            }
            return 0;
        }

        /// <summary>
        /// 单位换算
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currencytype"></param>
        /// <returns></returns>
        private decimal UnitConversion(decimal amount, int currencytype)
        {
            if (currencytype == 8 || currencytype == 2)
            {
                return Math.Floor((amount) * 100) / 100M;
            }
            else
            {
                return Math.Truncate(amount) / 100.0M;
            }
        }
    }
}
