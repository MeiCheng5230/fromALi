using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Common.UEPay;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Enum;
using PXin.Facade.Models.UserPurseReq;
using PXin.Model;
using Winner.CU.Balance.Entities;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;
using Winner.EncodeDecode;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// </summary>
    public class CZMFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 获取充值码配置
        /// tblc_centcard_config
        /// </summary>
        /// <returns></returns>
        public List<ConfigList> GetSvcConfig(Reqbase req)
        {
            var query = from tc in db.TblcCentcardConfigSet
                        group tc.Areaid by new
                        {
                            tc.Price,
                            tc.Areaid,
                            tc.Showname,
                            tc.Configid
                        } into t
                        select new
                        {
                            Price = t.Key.Price,
                            Typeid = t.Key.Areaid,
                            Showname = t.Key.Showname,
                            Configid = t.Key.Configid
                        };
            var result = query.OrderBy(c => c.Typeid).ThenBy(c => c.Configid).ToList();
            List<ConfigList> list = new List<ConfigList>();
            ConfigList dto = new ConfigList();
            int id = 0;
            foreach (var item in result)
            {
                if (id == item.Typeid)
                {
                    CardConfigDto card = new CardConfigDto();
                    card.Showname = item.Showname;
                    card.Configid = item.Configid;
                    card.Price = item.Price;
                    dto.List.Add(card);
                }
                else
                {
                    if (id != 0)
                    {
                        list.Add(dto);
                    }
                    dto = new ConfigList();
                    CardConfigDto card = new CardConfigDto();
                    card.Showname = item.Showname;
                    card.Configid = item.Configid;
                    card.Price = item.Price;
                    dto.TypeName = GetCardConfigName(item.Typeid);
                    dto.Stocknum = Stocknum(req.Nodeid, item.Typeid);
                    dto.List.Add(card);
                }
                id = item.Typeid;
            }

            if (id != 0)
            {
                list.Add(dto);
            }


            return list;
        }

        /// <summary>
        /// 微信调用返回
        /// </summary>
        private Pingpp.Models.Charge Charge { get; set; }
        private string Guid1;

        /// <summary>
        /// 购买Svc充值码,暂时只有微信
        /// 1、tblc_centcard生成充值码
        /// 2、tblc_centcard_his写充值码历史
        /// 3、如果是立即充值，再走充值流程 SvcToSv
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public OpenInfUeoDto BuySvc(BuyReq req)
        {
            if (!Check_BuyCZM(req))
            {
                return null;
            }

            db.BeginTransaction();
            if (req.PayType == 1)
            {
                if (!Pay_WX(req))
                {
                    db.Rollback();
                    return null;
                }
            }
            else if (req.PayType == 2)
            {
                BeginTransfer();
                if (!Pay_UV(req))
                {
                    db.Rollback();
                    EndTransfer(false);
                    return null;
                }

                Alert("购买成功", 1);
                EndTransfer(true);
                db.Commit();
                return null;
            }
            else
            {
                Alert("不支持的支付方式");
                db.Rollback();
                return null;
            }

            db.Commit();
            var charge = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Charge)));

            return new OpenInfUeoDto
            {
                ChargeStr = charge,
                Sign = Common.Mvc.Md5.SignString(charge + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = Guid1
            };
        }

        /// <summary>
        /// 微信回调
        /// </summary>
        /// <param name="guid">充值唯一编号</param>
        /// <returns></returns>
        public bool PingppPaySuccess_SVC(string guid)
        {
            TblcBtcChargeLog cz = db.TblcBtcChargeLogSet.Where(c => c.Guidstr == guid).FirstOrDefault();
            if (cz == null)
            {
                Alert("订单不存在");
                log.Info("订单不存在:" + guid);
                return false;
            }
            if (cz.State != 0)
            {
                Alert("订单状态错误");
                log.Info("订单状态错误:" + guid);
                return false;
            }

            var lst = cz.Remarks.Split(',');
            int count = cz.Counts;
            if (count <= 0)
            {
                Alert("购买数量不能等于或者小于0");
                return false;
            }

            db.BeginTransaction();
            //1、tblc_centcard生成充值码
            //2、tblc_centcard_his写充值码历史
            int num = count;
            decimal amount = cz.Amount;
            string cards = "";
            for (int i = 0; i < num; i++)
            {
                TblcCentcard tc = new TblcCentcard
                {
                    Cardno = PXinContext.GetCentcard(db),
                    Cardpwd = "0",
                    Ispwdrequired = 0,
                    Amount = amount,
                    Expiredtime = DateTime.Now.AddYears(10),
                    Createdtime = DateTime.Now,
                    Areaid = "1",
                    Status = 1,
                    Usenodeid = cz.Nodeid,
                    Remarks = "微信购买SVC充值码",
                    Fromid = 0,
                    Period = "0",
                    Productid = cz.Logid,
                };

                db.TblcCentcardSet.Add(tc);
                if (db.SaveChanges() <= 0)
                {
                    db.Rollback();
                    Alert("生成SVC充值码失败");
                    return false;
                }

                TblcCentcardHis his = new TblcCentcardHis
                {
                    Idno = tc.Idno,
                    Nodeid = cz.Nodeid,
                    Typeid = 0,
                    Note = "微信购买",
                    Createtime = DateTime.Now,
                    Opnodeid = cz.Nodeid,
                    Remarks = "生成充值码"
                };
                db.TblcCentcardHisSet.Add(his);
                if (db.SaveChanges() <= 0)
                {
                    db.Rollback();
                    Alert("生成SVC充值码记录失败");
                    return false;
                }
                cards += tc.Cardno + ",";
            }

            //修改订单状态
            cz.State = 1;
            cz.NoticeTime = DateTime.Now;
            if (db.SaveChanges() <= 0)
            {
                db.Rollback();
                Alert("修改订单状态失败");
                return false;
            }
            db.Commit();

            //立即使用
            if (lst[1] == "1")
            {
                db.BeginTransaction();
                BeginTransfer();
                //失败的情况下不返回false，只回滚事务
                if (SVC_Pro(cz.Nodeid, cards.TrimEnd(','), cz.Nodeid, 1) <= 0)
                {
                    db.Rollback();
                    EndTransfer(false);
                }
                else
                {
                    db.Commit();
                    EndTransfer(true);
                }

            }

            return true;
        }

        /// <summary>
        /// 微信购买UV-回调
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool PingppPaySuccess_UV(string orderNo)
        {
            int transid = Convert.ToInt32(orderNo);
            var entity = db.TfinBilltransSet.FirstOrDefault(x => x.Transid == transid);
            if (entity == null)
            {
                return Fail("没有找到该订单,订单号:" + transid);
            }

            if (entity.Status != 0)
            {
                return Fail("该笔账单已失效,订单号:" + transid);
            }
            db.BeginTransaction();
            BeginTransfer();
            Purse fromPurse = purseFactory.CenterBankPurse;
            Purse toPurse = null;
            if (entity.Paytype == 4)
            {
                toPurse = purseFactory.UserPVPurse(entity.Nodeid, 11); //UV钱包
            }
            else
            {
                EndTransfer(false);
                db.Rollback();
                return Fail("无效购买类型");
            }
            Currency currency = new Currency(CurrencyType.RMB, entity.Amount);
            int reason = 31;
            if (entity.Chargetype == 20)
            {
                //微信UV充值(APP)
                reason = 33112;
            }
            else
            {
                EndTransfer(false);
                db.Rollback();
                return Fail("无效支付类型");
            }
            TransferResult result = wcfProxy.Transfer(fromPurse, toPurse, currency, reason, entity.Remarks);
            if (!result.IsSuccess)
            {
                EndTransfer(false);
                db.Rollback();
                return Fail(result.Message);
            }
            entity.Status = 1;
            entity.Modifytime = DateTime.Now;
            entity.Transferids = result.TransferId.ToString();
            if (db.SaveChanges() <= 0)
            {
                EndTransfer(false);
                db.Rollback();
                return Fail("更新订单状态失败");
            }
            if (!EndTransfer(true))
            {
                db.Rollback();
                return Fail("转账提交失败");
            }
            if (!db.Commit())
            {
                return Fail("订单事务提交失败");
            }
            return true;
        }
        /// <summary>
        /// 购买UV，暂时只支持微信支付
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Pingpp.Models.Charge BuyUV(RechargeReq req)
        {
            //db.BeginTransaction();
            if (req.Channel.Equals("wx", StringComparison.OrdinalIgnoreCase))
            {
                if (!Pay_WX(req))
                {
                    db.Rollback();
                    return null;
                }
            }
            else
            {
                Alert("不支持的支付方式");
                db.Rollback();
                return null;
            }

            //db.Commit();
            //var charge = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Charge)));
            return Charge;

        }

        /// <summary>
        /// 零售SVC充值码，充值商代理人
        /// tbl_user_jxs.stocknum2减数量
        /// tblc_centcard生成充值码
        /// tblc_centcard_his写充值码历史
        /// tbl_user_jxs_stockhis2写转让历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SaleSvc(SaleReq req)
        {
            //参数检查
            (var jxs, var user) = Check_SaleSvc(req);
            if (jxs == null)
            {
                return false;
            }

            db.BeginTransaction();

            decimal sumprice = 0;
            var lst = req.Specifications.Split(',');
            for (int j = 0; j < lst.Length; j++)
            {
                var configlst = Array.ConvertAll(lst[j].Split('|'), int.Parse);
                int configid = configlst[0];
                var amount = db.TblcCentcardConfigSet.Where(c => c.Configid == configid).FirstOrDefault()?.Price;
                if (amount == null)
                {
                    Alert("传入的规格不正确");
                    return false;
                }
                for (int i = 0; i < configlst[1]; i++)
                {
                    //生成充值码
                    string cardno = PXinContext.GetCentcard(db);
                    TblcCentcard card = new TblcCentcard
                    {
                        Cardno = cardno,
                        Cardpwd = "0",
                        Ispwdrequired = 0,
                        Amount = amount,
                        Expiredtime = DateTime.Now.AddYears(10),
                        Createdtime = DateTime.Now,
                        Areaid = "1",
                        Status = 1,
                        Usenodeid = user.Nodeid,
                        Remarks = "零售SVC充值码",
                        Fromid = 1,
                        Assignnodeid = req.Nodeid,
                        Period = "0"
                    };
                    db.TblcCentcardSet.Add(card);

                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        db.Rollback();
                        return false;
                    }
                    sumprice += (decimal)amount;
                    //写历史记录
                    TblcCentcardHis his = new TblcCentcardHis
                    {
                        Idno = card.Idno,
                        Nodeid = user.Nodeid,
                        Typeid = 1,
                        Note = "零售",
                        Createtime = DateTime.Now,
                        Opnodeid = req.Nodeid,
                        Remarks = "生成充值码"
                    };
                    db.TblcCentcardHisSet.Add(his);

                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        db.Rollback();
                        return false;
                    }

                }

                if (configlst[1] > 0)
                {
                    //写零售历史
                    TblUserJxsStockhis2 stockhis2 = new TblUserJxsStockhis2
                    {
                        Infoid = jxs.Infoid,
                        Typeid = 0,
                        Nodeid = user.Nodeid,
                        Opnodeid = req.Nodeid,
                        Amount = (decimal)amount,
                        Num = configlst[1],
                        Createtime = DateTime.Now,
                        Totalamount = configlst[1] * (decimal)amount,
                        Remarks = "零售"
                    };
                    db.TblUserJxsStockhis2Set.Add(stockhis2);
                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        db.Rollback();
                        return false;
                    }
                }

            }

            //再次检查库存
            if (jxs.Stocknum2 < sumprice)
            {
                Alert("库存不足");
                db.Rollback();
                return false;
            }

            //修改库存
            jxs.Stocknum2 -= (int)sumprice;
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                db.Rollback();
                return false;
            }

            db.Commit();
            return true;
        }

        /// <summary>
        /// 回收SVC,界面显示转让，SVC只能充值商(tbl_user_jxs.typeid=4)回收，用户线下交易
        /// tblc_centcard修改状态status=3
        /// tblc_centcard_his写充值码历史
        /// tbl_user_jxs.stocknum2增加库存
        /// tbl_user_jxs_stockhis2写回收历史
        /// </summary>
        /// <returns></returns>
        public bool RecoverySvc(RecoveryReq req)
        {

            var lst = req.Cards.Split(',');
            int num = lst.Length;
            //参数检查
            (var jxs, var user) = Check_RecoverySvc(req);
            if (jxs == null)
            {
                return false;
            }

            db.BeginTransaction();
            decimal sumprice = 0;
            decimal amount = 0;
            foreach (var item in lst)
            {
                TblcCentcard card = db.TblcCentcardSet.Where(c => c.Cardno == item && c.Usenodeid == req.Nodeid).FirstOrDefault();
                card.Status = 3;
                card.Usenodeid = user.Nodeid;
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    db.Rollback();
                    return false;
                }
                sumprice += (decimal)card.Amount;
                amount = (decimal)card.Amount;

                //写历史记录
                TblcCentcardHis his = new TblcCentcardHis
                {
                    Idno = card.Idno,
                    Nodeid = user.Nodeid,
                    Typeid = 6,
                    Note = "转让",
                    Createtime = DateTime.Now,
                    Opnodeid = req.Nodeid,
                    Remarks = "转让回收充值码"
                };
                db.TblcCentcardHisSet.Add(his);

                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    db.Rollback();
                    return false;
                }
            }

            //增加库存
            jxs.Stocknum2 += (int)sumprice;
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                db.Rollback();
                return false;
            }

            //写转让回收历史
            TblUserJxsStockhis2 stockhis2 = new TblUserJxsStockhis2
            {
                Infoid = jxs.Infoid,
                Typeid = 1,
                Nodeid = user.Nodeid,
                Opnodeid = req.Nodeid,
                Amount = amount,
                Num = num,
                Createtime = DateTime.Now,
                Totalamount = sumprice,
                Remarks = "回收"
            };
            db.TblUserJxsStockhis2Set.Add(stockhis2);
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                db.Rollback();
                return false;
            }

            db.Commit();
            return true;
        }

        /// <summary>
        /// Svc充值码 充值 SV
        /// 默认用户自己的信息，即给用户自己给自己充值，可以更改账户给别人充值
        /// 更改tblc_centcard状态
        /// tblc_centcard_his写充值码历史
        /// SV钱包加钱，
        /// 送B指数给充值码拥有人 tpcn_index_user tblc_centcard_config.bnum
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SvcToSvByOwner(UseByOwnerReq req)
        {
            int nodeid = req.Nodeid;
            //传入nodecode时即表示为他人充值
            if (!string.IsNullOrEmpty(req.NodeCode))
            {
                var user = db.TnetReginfoSet.Where(c => c.Nodecode == req.NodeCode).FirstOrDefault();
                if (user == null)
                {
                    Alert("用户id不正确");
                    return false;
                }
                nodeid = user.Nodeid;
            }

            //检查支付密码
            if (!CheckPwd(req.Paypwd))
            {
                return false;
            }

            db.BeginTransaction();
            BeginTransfer();
            //使用充值码流程 返回充值总金额
            var sumamount = SVC_Pro(nodeid, req.Cards, req.Nodeid, 1);
            if (sumamount <= 0)
            {
                db.Rollback();
                EndTransfer(false);
                return false;
            }

            db.Commit();
            EndTransfer(true);

            if (req.IsVDNow == 1)
            {
                var user = db.TnetReginfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
                FriFacade facade = new FriFacade();
                if (!facade.ChargeVDian_SVPay(user, sumamount * 10, "", false))
                {
                    log.Info("使用充值码成功，但充值V点失败");
                    Alert("使用充值码成功，但充值V点失败，您可以选择进行手动充值V点", 1);
                }
            }
            return true;
        }
        /// <summary>
        /// 根据面额统计获取svc充值码
        /// </summary>
        public List<SvcByGroupbyAmountDto> GetSvcByGroupbyAmount(SvcByGroupbyAmountReq req)
        {
            var regInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodecode, KeyType = 1 });
            if (regInfo == null)
            {
                Alert("找不到您绑定的相信帐号,请重新绑定!");
                return null;
            }
            return db.TblcCentcardSet.Where(p => p.Usenodeid == regInfo.Nodeid && p.Status == 1)
                .GroupBy(p => p.Amount).Select(p => new SvcByGroupbyAmountDto
                {
                    Amount = p.Key ?? 0,
                    Number = p.Count()
                }).ToList();
        }
        /// <summary>
        /// 冻结Svc充值码
        /// </summary>
        /// <returns></returns>
        public bool FrozenSvc(FrozenSvcReq req)
        {
            var regInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodecode, KeyType = 1 });
            if (regInfo == null)
            {
                Alert("找不到您绑定的相信帐号,请重新绑定!");
                return false;
            }
            foreach (var item in req.FrozenSvcInfos)
            {
                var centcards = db.TblcCentcardSet.Where(p => p.Usenodeid == regInfo.Nodeid && p.Amount == item.Amount && p.Status == 1).ToList();
                if (centcards.Count() < item.Number)
                {
                    Alert($"面额为{item.Amount}的充值码张数不够{item.Number}张");
                    return false;
                }
                else
                {
                    var data = new List<TblcCentcard>();
                    for (int i = 0; i < item.Number; i++)
                    {
                        data.Add(centcards[i]);
                    }
                    if (item.PmInfoids.Count != item.Number)
                    {
                        Alert("出售失败");
                        return false;
                    }
                    var index = 0;
                    Array.ForEach(data.ToArray(), p =>
                    {
                        p.Status = 4;
                        var hisPk = db.GetPrimaryKeyValue<TblcCentcardHis>();
                        db.TblcCentcardHisSet.Add(new TblcCentcardHis()
                        {
                            Hisid = hisPk,
                            Idno = p.Idno,
                            Nodeid = regInfo.Nodeid,
                            Typeid = 12,
                            Note = "码库CNV出售充值码(SVC)",
                            Createtime = DateTime.Now,
                            Opnodeid = regInfo.Nodeid,
                            Remarks = "码库CNV出售充值码(SVC)"
                        });
                        db.TblcCentcardAuctionHisSet.Add(new TblcCentcardAuctionHis
                        {
                            CentcardHisid = hisPk,
                            PmInfoid = item.PmInfoids[index],
                            Nodeid = regInfo.Nodeid,
                            Typeid = 1,
                            Remarks = "码库CNV出售充值码(SVC)",
                            Batch = req.Batch
                        });
                        index++;
                    });
                }
            }
            var result = db.SaveChanges() > 0;
            if (result)
            {
                return result;
            }
            else
            {
                Alert("冻结SVC充值码失败");
                return false;
            }
        }
        /// <summary>
        /// 购买SVC充值码
        /// </summary>
        /// <returns></returns>
        public bool BuySvcCode(BuySvcCodeReq req)
        {
            var regInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodecode, KeyType = 1 });
            if (regInfo == null)
            {
                Alert("找不到您绑定的相信帐号,请重新绑定!");
                return false;
            }
            var sellRegInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.SellNodecode, KeyType = 1 });
            if (sellRegInfo == null)
            {
                Alert("找不到卖家绑定的相信帐号,请联系管理员!");
                return false;
            }
            var centcards = db.TblcCentcardSet.FirstOrDefault(p => p.Usenodeid == sellRegInfo.Nodeid && p.Amount == req.Amount && p.Status == 4);
            if (centcards == null)
            {
                Alert($"购买失败,请联系管理员");
                return false;
            }
            centcards.Cardno = PXinContext.GetCentcard(db);
            centcards.Usenodeid = regInfo.Nodeid;
            centcards.Status = 1;

            var hisPk = db.GetPrimaryKeyValue<TblcCentcardHis>();
            var centcardHis = new TblcCentcardHis()
            {
                Hisid = hisPk,
                Idno = centcards.Idno,
                Nodeid = regInfo.Nodeid,
                Typeid = 10,
                Note = "码库CNV购买充值码(SVC)",
                Createtime = DateTime.Now,
                Opnodeid = regInfo.Nodeid,
                Remarks = "码库CNV购买充值码(SVC)"
            };
            db.TblcCentcardHisSet.Add(centcardHis);

            db.TblcCentcardAuctionHisSet.Add(new TblcCentcardAuctionHis
            {
                CentcardHisid = hisPk,
                PmInfoid = req.PmInfoid,
                Nodeid = regInfo.Nodeid,
                Typeid = 2,
                Remarks = "码库CNV购买充值码(SVC)",
                Batch = req.Batch
            });

            var result = db.SaveChanges() > 0;
            if (result)
            {
                return result;
            }
            else
            {
                Alert("冻结SVC充值码失败");
                return false;
            }
        }
        /// <summary>
        /// 取消发布(解冻)
        /// </summary>
        /// <returns></returns>
        public bool CancelRelease(CancelReleaseReq req)
        {
            var regInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodecode, KeyType = 1 });
            if (regInfo == null)
            {
                Alert("找不到您绑定的相信帐号,请重新绑定!");
                return false;
            }
            var centcard = db.TblcCentcardSet.FirstOrDefault(p => p.Usenodeid == regInfo.Nodeid && p.Amount == req.Amount && p.Status == 4);
            if (centcard == null)
            {
                Alert("操作失败,请联系管理员");
                return false;
            }
            centcard.Status = 1;

            var hisPk = db.GetPrimaryKeyValue<TblcCentcardHis>();
            var centcardHis = new TblcCentcardHis()
            {
                Hisid = hisPk,
                Idno = centcard.Idno,
                Nodeid = regInfo.Nodeid,
                Typeid = 13,
                Note = "码库取消发布充值码(SVC)",
                Createtime = DateTime.Now,
                Opnodeid = regInfo.Nodeid,
                Remarks = "码库取消发布充值码(SVC)"
            };
            db.TblcCentcardHisSet.Add(centcardHis);

            db.TblcCentcardAuctionHisSet.Add(new TblcCentcardAuctionHis
            {
                CentcardHisid = hisPk,
                PmInfoid = req.PmInfoid,
                Nodeid = regInfo.Nodeid,
                Typeid = 3,
                Remarks = "码库取消发布充值码(SVC)"
            });

            var result = db.SaveChanges() > 0;
            if (!result)
                Alert("操作失败,请联系管理员");
            return result;
        }

        private int _transferid;
        /// <summary>
        /// 使用svc码流程
        /// </summary>
        /// <param name="nodeid">充值目标用户</param>
        /// <param name="cards">卡号</param>
        /// <param name="pnodeid">充值卡拥有人</param>
        /// <param name="type">1=有主的svc码，2=无主的svc码</param>
        /// <returns></returns>
        private decimal SVC_Pro(int nodeid, string cards, int pnodeid, int type)
        {
            var lst = cards.Split(',');
            decimal sumamount = 0M;
            foreach (var item in lst)
            {
                TblcCentcard card = null;
                if (type == 1)
                {
                    //卡号存在并且是当前人拥有
                    card = db.TblcCentcardSet.Where(c => c.Cardno == item && c.Usenodeid == pnodeid).FirstOrDefault();
                }
                else
                {
                    //无主卡无需验证用户
                    card = db.TblcCentcardSet.Where(c => c.Cardno == item && (c.Usenodeid == 0 || c.Usenodeid == null)).FirstOrDefault();
                }
                if (card == null)
                {
                    Alert("请确认卡号是否正确");
                    return -1;
                }
                if (card.Status != 1)
                {
                    Alert("充值卡已使用或失效");
                    return -1;
                }

                //b指数增加对象id   使用无主卡时对象为当前使用对象，有主卡的时候对象为卡的拥有人
                int id = (card.Usenodeid == 0 || card.Usenodeid == null) ? nodeid : (int)card.Usenodeid;

                card.Status = 2;
                card.Usenodeid = nodeid;
                card.Usedate = DateTime.Now;
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    return -1;
                }
                sumamount += (decimal)card.Amount;

                //充值SV
                if (!TransferBySvc(nodeid, (decimal)card.Amount))
                {
                    return -1;
                }
                //写历史记录
                TblcCentcardHis his = new TblcCentcardHis
                {
                    Idno = card.Idno,
                    Nodeid = nodeid,
                    Typeid = 5,
                    Note = "使用",
                    Createtime = DateTime.Now,
                    Opnodeid = pnodeid,
                    Remarks = _transferid.ToString()
                };
                db.TblcCentcardHisSet.Add(his);
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    return -1;
                }

                //获取B指数配置信息
                var config = db.TblcCentcardConfigSet.Where(c => c.Price == card.Amount).FirstOrDefault();
                if (config != null)
                {
                    //添加B指数
                    TpcnIndexUser index = new TpcnIndexUser
                    {
                        Nodeid = id,
                        Num = config.Bnum,
                        Periods = 1,
                        Fromid = 1,
                        Pkid = 0,
                        Createtime = DateTime.Now,
                        Remarks = "使用SVC充值码",
                        Typeid = 1,
                        Status = 1,
                        Price = (decimal)card.Amount
                    };
                    db.TpcnIndexUserSet.Add(index);
                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        return -1;
                    }
                }
            }
            return sumamount;
        }

        /// <summary>
        /// 无主Svc充值码 充值 SV
        /// 更改tblc_centcard状态
        /// tblc_centcard_his写充值码历史
        /// SV钱包加钱，
        /// 送B指数给使用人 tpcn_index_user tblc_centcard_config.bnum
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SvcToSvByCard(UseByCardReq req)
        {
            db.BeginTransaction();
            BeginTransfer();
            //使用充值码流程
            if (SVC_Pro(req.Nodeid, req.Cards, req.Nodeid, 2) <= 0)
            {
                db.Rollback();
                EndTransfer(false);
                return false;
            }

            db.Commit();
            EndTransfer(true);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SvToSvcCard(SvToSvcCardReq req)
        {
            var result = Check_SvToSvc(req.Nodeid, req.Paypwd, req.Config);
            if (!result.Item1)
            {
                return false;
            }

            db.BeginTransaction();
            BeginTransfer();
            //sv生成svc充值码流程
            if (!SvToSvc_Pro(req.Nodeid, result.Item2))
            {
                db.Rollback();
                EndTransfer(false);
                return false;
            }

            db.Commit();
            EndTransfer(true);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public SvcStatisDto GetMySvcStatis(Reqbase req)
        {

            var lst = db.TblcCentcardSet.Where(c => c.Usenodeid == req.Nodeid && c.Status == 1 && c.Areaid == "1");
            int count = lst.Count();
            decimal amount = count == 0 ? 0 : (decimal)lst.Sum(c => c.Amount);
            int issale = GetIsZys(req.Nodeid, 2) == null ? 0 : 1;
            var agree = db.TnetUserAgreementSet.Where(c => c.Nodeid == req.Nodeid && c.Type == 4).FirstOrDefault() == null ? 0 : 1;

            return new SvcStatisDto { Count = count, Amount = amount, Issale = issale, IsAgreement = agree };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public SVUserInfoDto GetUserInfo(SVUserInfoReq req)
        {
            int.TryParse(req.Nodecode, out int id);
            TnetReginfo user = null;
            if (req.IsPhoneEmail == 0)
            {
                user = db.TnetReginfoSet.Where(c => c.Nodecode == req.Nodecode).FirstOrDefault();
            }
            else
            {
                user = db.TnetReginfoSet.Where(c => c.Nodecode == req.Nodecode || c.Nodeid == id || c.Mobileno == req.Nodecode || c.Email == req.Nodecode).FirstOrDefault();
            }

            if (user == null)
            {
                Alert("用户不存在");
                return null;
            }
            //var zys = db.TblUserJxsSet.Where(c => c.Nodeid == user.Nodeid && c.Status !=2 && c.Status2 == 0).FirstOrDefault();
            SVUserInfoDto userInfo = new SVUserInfoDto();
            userInfo.NodeCode = user.Nodecode;
            string nodeNameFilter = Helper.FilterChar(user.Nodename);
            userInfo.Name = nodeNameFilter.Length > 0 ? nodeNameFilter.Substring(0, 1) + "**" : "";
            userInfo.Phone = user.Mobileno == null ? "" : user.Mobileno.Substring(0, 3) + "***" + user.Mobileno.Substring(8, 3);
            userInfo.IsSelf = user.Nodeid == req.Nodeid ? 1 : 0;
            userInfo.IsZYS = GetIsZys(user.Nodeid, 1) == null ? 0 : 1;

            return userInfo;
        }

        private bool SvToSvc_Pro(int nodeid, List<SVCConfig> list)
        {
            if (!CreateSVCCard(nodeid, list, out decimal sumprice))
            {
                return false;
            }

            if (!TransferBySvcToSelf(nodeid, sumprice))
            {
                return false;
            }

            var user = db.TnetReginfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            decimal free = sumprice * 0.05M;
            if (!Recovery(free, user.Nodeid, user.Nodecode, 6, "SV生成充值码手续费", 10087))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 生成充值码及历史记录
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="list"></param>
        /// <param name="sumprice"></param>
        /// <returns></returns>
        private bool CreateSVCCard(int nodeid, List<SVCConfig> list, out decimal sumprice)
        {
            sumprice = 0M;
            int count = 0;
            DateTime time = DateTime.Now.Date;
            var sum = db.TblcCentcardSet.Where(c => c.Fromid == 6 && c.Createdtime > time && c.Assignnodeid == nodeid).ToList().Sum(c => c.Amount);
            foreach (var item in list)
            {
                count += item.Num;
                for (int i = 0; i < item.Num; i++)
                {
                    //生成充值码
                    string cardno = PXinContext.GetCentcard(db);
                    DateTime dt = DateTime.Now.AddYears(10);
                    TblcCentcard card = new TblcCentcard
                    {
                        Cardno = cardno,
                        Cardpwd = "0",
                        Ispwdrequired = 0,
                        Amount = item.Amount,
                        Expiredtime = dt,
                        Createdtime = DateTime.Now,
                        Areaid = "1",
                        Status = 1,
                        Usenodeid = nodeid,
                        Remarks = "SV生成充值码",
                        Fromid = 6,
                        Assignnodeid = nodeid,
                        Period = "0"
                    };
                    db.TblcCentcardSet.Add(card);
                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        return false;
                    }

                    sumprice += item.Amount;

                    //写历史记录
                    TblcCentcardHis his = new TblcCentcardHis
                    {
                        Idno = card.Idno,
                        Nodeid = nodeid,
                        Typeid = 8,
                        Note = "SV生成充值码",
                        Createtime = DateTime.Now,
                        Opnodeid = nodeid,
                        Remarks = "SV生成充值码"
                    };
                    db.TblcCentcardHisSet.Add(his);
                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        return false;
                    }
                }
            }

            if (!AddSVCLimit(nodeid, (decimal)sum, sumprice))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 增加sv生成充值码限制
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="sum"></param>
        /// <param name="sumprice"></param>
        /// <returns></returns>
        private bool AddSVCLimit(int nodeid, decimal sum, decimal sumprice)
        {

            var total = db.TpxinSvcLimitSet.Where(c => c.Nodeid == nodeid && c.Fromtime <= DateTime.Now && c.Endtime >= DateTime.Now).FirstOrDefault();
            if (total == null)
            {
                return true;
            }
            if (sum >= 10000)
            {
                if (total.Totalamount - (total.Localamount1 + sumprice) >= 0)
                {
                    total.Localamount1 += sumprice;

                    if (db.SaveChanges() <= 0)
                    {
                        Alert("操作失败");
                        return false;
                    }
                }
                else
                {
                    Alert("超出可生成上限");
                    return false;
                }

            }
            else
            {
                if (total.Totalamount + 10000 - (total.Localamount1 + sumprice + sum) >= 0)
                {
                    if (sumprice + sum > 10000)
                    {
                        total.Localamount1 += (sumprice - 10000 + sum);

                        if (db.SaveChanges() <= 0)
                        {
                            Alert("操作失败");
                            return false;
                        }
                    }

                }
                else
                {
                    Alert("超出可生成上限");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 参数检查
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pwd">支付密码</param>
        /// <param name="config">规格列表</param>
        /// <returns></returns>
        private ValueTuple<bool, List<SVCConfig>> Check_SvToSvc(int nodeid, string pwd, string config)
        {
            //检查支付密码
            if (!CheckPwd(pwd))
            {
                return (false, null);
            }

            if (GetIsZys(nodeid, 1) == null)
            {
                Alert("当前没有操作权限，请联系客服人员");
                return (false, null);
            }

            int flag = 0;
            List<SVCConfig> list = new List<SVCConfig>();
            try
            {
                var configList = config.Split(',');
                foreach (var item in configList)
                {
                    SVCConfig config1 = new SVCConfig();
                    int[] numlist = Array.ConvertAll(item.Split('|'), int.Parse);
                    config1.Amount = numlist[0];
                    config1.Num = numlist[1];
                    if (config1.Amount != 1000 && config1.Amount != 5000 && config1.Amount != 10000)
                    {
                        Alert("传入的规格不正确");
                        flag = 1;
                        break;
                    }
                    list.Add(config1);
                }

            }
            catch (Exception)
            {
                Alert("传入的规格不正确");
                return (false, null);
            }

            if (flag == 1)
            {
                return (false, null);
            }

            //11月1日后充值商每天最多可生成10000
            if (DateTime.Now > DateTime.Parse(AppConfig.CZMCreateTime).Date)
            {
                DateTime time = DateTime.Now.Date;
                var sum = db.TblcCentcardSet.Where(c => c.Fromid == 6 && c.Createdtime > time && c.Assignnodeid == nodeid).ToList().Sum(c => c.Amount);
                var now = list.Sum(c => c.Amount * c.Num);
                var total = db.TpxinSvcLimitSet.Where(c => c.Nodeid == nodeid && c.Fromtime <= DateTime.Now && c.Endtime >= DateTime.Now).Select(c => c.Totalamount - c.Localamount1).FirstOrDefault();
                if (sum >= 10000)
                {
                    if (now > total)
                    {
                        Alert("超过每日可生成充值码上限");
                        return (false, null);
                    }
                }
                else
                {
                    if (sum + now > 10000 + total)
                    {
                        Alert("超过每日可生成充值码上限");
                        return (false, null);
                    }
                }


            }



            return (true, list);
        }

        /// <summary>
        /// 扣UEDOS
        /// </summary>
        /// <param name="amount">金额</param>
        /// <param name="nodeid">nodeid</param>
        /// <param name="nodecode">用户账号</param>
        /// <param name="typeid">账号类型</param>
        /// <param name="remarks">备注</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        public bool Recovery(decimal amount, int nodeid, string nodecode, int typeid, string remarks, int reason)
        {
            TpcnUepayconfig ueConfig = db.TpcnUepayconfigSet.FirstOrDefault(a => a.Typeid == 2);
            if (ueConfig == null || ueConfig.Id <= 0)
            {
                Alert("获取码库支付配置失败");
                return false;
            }
            var feeResult = UeApi.Recovery(ueConfig.Accesskeyid, ueConfig.Accesssecret, nodecode, typeid, 1, amount, CurrencyType.DOS_矿沙.CurrencyId, remarks, reason);
            if (feeResult == null || feeResult?.Result < 0)
            {
                log.Info("调用UE回收接口异常:" + (feeResult == null ? string.Empty : feeResult.Message));
                Alert("码库手续费扣取失败，" + (feeResult == null ? string.Empty : feeResult.Message));
                return false;
            }
            return true;


        }

        private bool Pay_WX(RechargeReq req)
        {
            if (req.Amount < (decimal)0.001 || req.Amount > 99999999)
            {
                Alert("请选择或输入充值金额");
                return false;
            }
            if (req.Paytype != 4)
            {
                Alert("无效充值类型");
                return false;
            }
            int chargeType = 20;
            string remarks = "微信UV充值(相信APP)";
            TfinBilltrans billtrans = new TfinBilltrans
            {
                Amount = req.Amount,
                Status = 0,
                Nodeid = req.Nodeid,
                Paytype = req.Paytype,
                Chargetype = chargeType,
                Remarks = remarks
            };
            db.TfinBilltransSet.Add(billtrans);
            if (db.SaveChanges() <= 0)
            {
                Alert("创建订单失败");
                return false;
            }
            decimal amount = billtrans.Amount * 100M;
            if (billtrans.Nodeid == 3434909)
            {
                amount = 1;
            }
            Dictionary<string, object> param = CreatePingppObject(req.Sid, "wx", billtrans.Transid.ToString(), amount, "uvcharge", "UV充值");
            if (param == null)
            {
                return false;
            }
            Charge = Pingpp.Models.Charge.Create(param);
            return true;

        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <returns></returns>
        private bool Pay_WX(BuyReq req)
        {
            var config = db.TblcCentcardConfigSet.Where(c => c.Price == req.Amount).FirstOrDefault();
            if (config == null)
            {
                Alert("金额类型不存在");
                return false;
            }
            string remarks = "购买Svc充值码," + req.IsUseNow;
            TblcBtcChargeLog chargeLog = new TblcBtcChargeLog
            {
                Nodeid = req.Nodeid,
                Amount = req.Amount,
                Counts = req.Num,
                CreateTime = DateTime.Now,
                Remarks = remarks,
                Guidstr = MakeOrderId(req.Nodeid),
                State = 0,
                Payment = 0,
                Typeid = 0,
                Appstoreid = req.Sid
            };

            db.TblcBtcChargeLogSet.Add(chargeLog);

            if (db.SaveChanges() <= 0)
            {
                Alert("生成订单失败");
                return false;
            }
            Guid1 = chargeLog.Guidstr;
            Dictionary<string, object> param = CreatePingppObject(req.Sid, chargeLog);
            if (param == null)
            {
                return false;
            }

            Charge = Pingpp.Models.Charge.Create(param);

            return true;
        }

        /// <summary>
        /// 零售参数检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private ValueTuple<TblUserJxs, TnetReginfo> Check_SaleSvc(SaleReq req)
        {
            if (string.IsNullOrEmpty(req.Specifications))
            {
                Alert("参数错误");
                return (null, null);
            }

            var jxs = GetIsZys(req.Nodeid, 2);
            if (jxs == null)
            {
                Alert("您没有操作权限");
                return (null, null);
            }
            var user = db.TnetReginfoSet.Where(c => c.Nodecode == req.NodeCode).FirstOrDefault();
            if (user == null)
            {
                Alert("用户id不正确");
                return (null, null);
            }
            //检查支付密码
            if (!CheckPwd(req.Paypwd))
            {
                return (null, null);
            }


            return (jxs, user);
        }

        /// <summary>
        /// 回收参数检查
        /// </summary>
        /// <returns></returns>
        private ValueTuple<TblUserJxs, TnetReginfo> Check_RecoverySvc(RecoveryReq req)
        {
            if (string.IsNullOrEmpty(req.Cards))
            {
                Alert("卡号不能为空");
                return (null, null);
            }
            var user = db.TnetReginfoSet.Where(c => c.Nodecode == req.NodeCode).FirstOrDefault();
            if (user == null)
            {
                Alert("用户id不正确");
                return (null, null);
            }

            var jxs = GetIsZys(user.Nodeid, 1);
            if (jxs == null)
            {
                Alert("只能转让给充值商");
                return (null, null);
            }
            if (jxs.Status + jxs.Status2 != 1)
            {
                if (jxs.Status == 2)
                {
                    Alert($"该用户充值商审核未通过");
                    return (null, null);
                }
            }
            if (jxs.Status2 != 0)
            {
                Alert($"该用户充值商冻结");
                return (null, null);
            }
            if (jxs.Chgtypedate.AddDays(30) < DateTime.Now && jxs.Endtime < DateTime.Now)
            {
                Alert("该用户充值商已过期");
                return (null, null);
            }
            //检查支付密码
            if (!CheckPwd(req.Paypwd))
            {
                return (null, null);
            }

            return (jxs, user);
        }

        /// <summary>
        /// sv转账
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool TransferBySvc(int nodeid, decimal amount)
        {
            Purse from = purseFactory.SystemPurseRand(nodeid);//从系统钱包
            Purse to = new Purse(OwnerType.个人钱包, nodeid, PurseType.现金钱包, CurrencyType.RMB, wcfProxy);//到用户的SV钱包
            Currency currency = new Currency(CurrencyType.RMB, amount);

            TransferResult tResult = wcfProxy.Transfer(from, to, currency, 13, "使用充值码(SVC)");
            if (!tResult.IsSuccess)
            {
                Alert("使用充值码(SVC)失败");
                return false;
            }
            _transferid = tResult.TransferId;
            return true;
        }

        /// <summary>
        /// sv转账到系统
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool TransferBySvcToSelf(int nodeid, decimal amount)
        {
            Purse to = purseFactory.SystemPurseRand(nodeid);//从系统钱包
            Purse from = new Purse(OwnerType.个人钱包, nodeid, PurseType.现金钱包, CurrencyType.RMB, wcfProxy);//到用户的SV钱包
            Currency currency = new Currency(CurrencyType.RMB, amount);

            TransferResult tResult = wcfProxy.Transfer(from, to, currency, 33179, "SV生成充值码");
            if (!tResult.IsSuccess)
            {
                Alert(tResult.Message);
                return false;
            }
            _transferid = tResult.TransferId;
            return true;
        }

        /// <summary>
        /// 扣除sv，充值v点
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool TransferByVD(int nodeid, decimal amount)
        {
            Purse to = purseFactory.SystemPurseRand(nodeid);//到系统钱包
            Purse from = new Purse(OwnerType.个人钱包, nodeid, PurseType.现金钱包, CurrencyType.RMB, wcfProxy);//从用户的SV钱包
            Currency currency = new Currency(CurrencyType.RMB, amount);

            TransferResult tResult = wcfProxy.Transfer(from, to, currency, 12, "SV充值V点");
            if (!tResult.IsSuccess)
            {
                Alert("充值失败");
                return false;
            }

            TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == nodeid);
            if (userinfo == null)
            {
                userinfo = new TpxinUserinfo
                {
                    Nodeid = nodeid,
                    Backpic = "",
                    Createtime = DateTime.Now,
                    Up = 0,
                    Down = 0,
                    P = 0,
                    V = 0,
                    Remarks = ""
                };
                db.TpxinUserinfoSet.Add(userinfo);
            }

            //兑换比例1:10
            decimal vd = amount * 10;
            //1.添加 信友圈付费V点历史表 TPXIN_PAYHIS
            TpxinPayhis payhis = new TpxinPayhis
            {
                Createtime = DateTime.Now,
                Infoid = 0,
                Typeid = 1,
                Nodeid = nodeid,
                Tonodeid = 0,
                Price = vd,
                Remarks = "使用充值码(SVC)直接充值V点"
            };
            //2.修改用户信息表的v点
            db.TpxinPayhisSet.Add(payhis);
            var amountBefore = userinfo.V;
            userinfo.V += vd;
            //3.添加金额变化记录
            var amountChangeHis = new TpxinAmountChangeHis()
            {
                Nodeid = nodeid,
                Typeid = 1,
                Amount = vd,
                Reason = (int)AmountChangeReason.ChargeVDian,
                Transferid = Guid.NewGuid().ToString(),
                Createtime = DateTime.Now,
                Remarks = "使用充值码(SVC)直接充值V点",
                Amountbefore = amountBefore,
                Amountafter = userinfo.V
            };
            db.TpxinAmountChangeHisSet.Add(amountChangeHis);
            if (db.SaveChanges() <= 0)
            {
                Alert("充值V点失败");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查支付密码
        /// </summary>
        /// <param name="Paypwd"></param>
        /// <returns></returns>
        private bool CheckPwd(string Paypwd)
        {
            if (string.IsNullOrEmpty(Paypwd))
            {
                Alert("支付密码错误");
                return false;
            }
            var tnet_reginfo = HttpContext.Current.GetRegInfo();
            //支付密码
            if (string.IsNullOrEmpty(tnet_reginfo.UserpwdBak))
            {
                Alert("未设置支付密码");
                return false;
            }
            if (!UserPwd.Check(Paypwd, tnet_reginfo.UserpwdBak, tnet_reginfo.Nodeid, tnet_reginfo.Nodecode))
            {
                Alert("支付密码错误");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取当前分类名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetCardConfigName(int id)
        {
            switch (id)
            {
                case 1:
                    return "SV";
                default:
                    break;
            }
            return "";
        }

        /// <summary>
        /// 获取当前类别库存
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private decimal Stocknum(int nodeid, int id)
        {
            var jxs = db.TblUserJxsSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            switch (id)
            {
                case 1:
                    return jxs == null ? 0 : jxs.Stocknum2;
                default:
                    break;
            }
            return 0;
        }

        /// <summary>
        /// 获取钱包余额
        /// </summary>
        /// <param name="nodeid">用户id</param>
        /// <param name="subid">subid</param>
        /// <param name="pursetype">钱包类型</param>
        /// <param name="ownertype">账户类型</param>
        /// <param name="currencytype">单位类型</param>
        /// <returns></returns>
        private decimal GetBalance(int nodeid, int subid, int pursetype, int ownertype, int currencytype)
        {
            //获取余额
            var balance = db.TblcUserPurseSet.Where(c => c.Ownerid == nodeid && c.Subid == subid && c.Pursetype == pursetype && c.Ownertype == ownertype && c.Currencytype == currencytype).FirstOrDefault();
            if (balance == null)
            {
                return 0;
            }
            return GetFloor(balance.Balance - balance.Freezevalue);
        }

        /// <summary>
        /// 小数向下取整(保留两位小数)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private decimal GetFloor(decimal num)
        {
            return Math.Truncate(num * 100) / 100;
        }

        /// <summary>
        /// 判断当前是不是专营商或者充值商
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="typeid">1=充值商 2=专营商或者充值商</param>
        /// <returns></returns>
        private TblUserJxs GetIsZys(int nodeid, int typeid)
        {
            var jxs = db.TblUserJxsSet.Where(c => c.Nodeid == nodeid && c.Status != 2 && c.Status2 == 0).FirstOrDefault();

            if (jxs == null)
            {
                return null;
            }

            //只能是充值商
            if (typeid == 1)
            {
                if (jxs.Typeid != 4)
                {
                    return null;
                }
            }
            else
            {
                if (jxs.Typeid != 4 && jxs.Typeid != 5)
                {
                    return null;
                }
            }

            if (jxs.Status != 1)
            {
                if (jxs.Chgtypedate < DateTime.Now.AddDays(-30))
                {
                    Alert("充值商未审核已过期");
                    return null;
                }
            }

            return jxs;
        }

        /// <summary>
        /// uv购买充值码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool Pay_UV(BuyReq req)
        {
            if (!CreateCZMCards(req.Nodeid, req.Num, req.Amount, 8, 11, "UV购买SVC充值码", out string cards))
            {
                Alert("操作失败，请稍后再试");
                return false;
            }

            if (!TransferByUV(req.Nodeid, req.Num * req.Amount))
            {
                return false;
            }

            //立即使用
            if (req.IsUseNow == 1)
            {
                if (SVC_Pro(req.Nodeid, cards.TrimEnd(','), req.Nodeid, 1) <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// uv购买充值码支付
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private bool TransferByUV(int nodeid, decimal amount)
        {
            Purse from = new Purse(OwnerType.个人钱包, nodeid, PurseType.电子代币, 11, CurrencyType.RMB_人民币分, wcfProxy);//从用户的UV钱包
            Purse to = purseFactory.SystemPurseRand(nodeid);//到系统钱包
            Currency currency = new Currency(CurrencyType.RMB, amount);

            TransferResult tResult = wcfProxy.Transfer(from, to, currency, 33181, "UV购买SVC充值码");
            if (!tResult.IsSuccess)
            {
                Alert("购买失败:" + tResult.Message);
                return false;
            }
            _transferid = tResult.TransferId;
            return true;
        }

        /// <summary>
        /// 生成充值码及历史
        /// </summary>
        /// <param name="nodeid">用户id</param>
        /// <param name="num">张数</param>
        /// <param name="amount">金额</param>
        /// <param name="fromid">生成类型id</param>
        /// <param name="typeid">历史类型id</param>
        /// <param name="remarks">备注</param>
        /// <param name="cards">返回参数</param>
        /// <returns></returns>
        private bool CreateCZMCards(int nodeid, int num, decimal amount, int fromid, int typeid, string remarks, out string cards)
        {
            cards = "";
            for (int i = 0; i < num; i++)
            {
                //生成充值码
                string cardno = PXinContext.GetCentcard(db);
                DateTime dt = DateTime.Now.AddYears(10);
                TblcCentcard card = new TblcCentcard
                {
                    Cardno = cardno,
                    Cardpwd = "0",
                    Ispwdrequired = 0,
                    Amount = amount,
                    Expiredtime = dt,
                    Createdtime = DateTime.Now,
                    Areaid = "1",
                    Status = 1,
                    Usenodeid = nodeid,
                    Remarks = remarks,
                    Fromid = fromid,
                    Assignnodeid = nodeid,
                    Period = "0"
                };
                db.TblcCentcardSet.Add(card);
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    return false;
                }

                cards += cardno + ",";
                //写历史记录
                TblcCentcardHis his = new TblcCentcardHis
                {
                    Idno = card.Idno,
                    Nodeid = nodeid,
                    Typeid = typeid,
                    Note = remarks,
                    Createtime = DateTime.Now,
                    Opnodeid = nodeid,
                    Remarks = remarks
                };
                db.TblcCentcardHisSet.Add(his);
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 购买充值码参数验证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool Check_BuyCZM(BuyReq req)
        {
            if (req.Num <= 0)
            {
                Alert("参数错误");
                return false;
            }
            if (req.Amount != 1000 && req.Amount != 5000 && req.Amount != 10000)
            {
                Alert("参数错误");
                return false;
            }

            if (req.PayType != 1)
            {
                if (!CheckPwd(req.Pwd))
                {
                    return false;
                }
            }

            return true;
        }

        #region【微信支付调用】
        private string MakeOrderId(int nodeid)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random(nodeid).Next(10000, 99999);
        }
        private Dictionary<string, object> CreatePingppObject(int sid, TblcBtcChargeLog his)
        {
            string channel = string.Empty;
            if (his.Payment == 0)
            {
                channel = "wx"; //微信
            }
            else if (his.Payment == 3)
            {
                channel = "upacp";//很行卡
            }
            else
            {
                Alert("不支持的支付方式");
                return null;
            }
            decimal amount = his.Amount * his.Counts * 100;
            if (his.Nodeid == 3434909)
            {
                amount = 1;
            }
            return CreatePingppObject(sid, channel, his.Guidstr, amount, "svc", "购买Svc充值码");
        }

        private Dictionary<string, object> CreatePingppObject(int sid, string channel, string orderid, decimal amount, string extraType, string body)
        {
            string key = ConfigurationManager.AppSettings["ApiKey"];
            Pingpp.Pingpp.SetApiKey(key);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Dictionary<string, object> extra = new Dictionary<string, object>();
            if (channel.Equals("alipay_wap"))
            {
                extra.Add("success_url", ConfigurationManager.AppSettings["Alipay_url"]); //支付宝支付成功回调地址
                //extra.Add("cancel_url", "http://www.yourdomain.com/cancel");
            }
            else if (channel.Equals("wx_pub"))
            {
                extra.Add("open_id", ConfigurationManager.AppSettings["Open_id"]);
            }
            else if (channel.Equals("wx"))
            {
                //extra.Add("success_url", ConfigurationManager.AppSettings["WxNotice_url"]);
            }
            //else if (channel.Equals("upacp_wap"))
            //{
            //    extra.Add("result_url", "http://www.yourdomain.com/result");
            //}
            //else if (channel.Equals("upmp_wap"))
            //{
            //    extra.Add("result_url", "http://www.yourdomain.com/result?code=");
            //}
            //else if (channel.Equals("bfb_wap"))
            //{
            //    extra.Add("result_url", "http://www.yourdomain.com/result");
            //    extra.Add("bfb_login", true);
            //}
            //else if (channel.Equals("wx_pub_qr"))
            //{
            //    extra.Add("product_id", "asdfsadfadsf");
            //}
            //else if (channel.Equals("yeepay_wap"))
            //{
            //    extra.Add("product_category", "1");
            //    extra.Add("identity_id", "sadfsdaf");
            //    extra.Add("identity_type", 1);
            //    extra.Add("terminal_type", 1);
            //    extra.Add("terminal_id", "sadfsadf");
            //    extra.Add("user_ua", "sadfsdaf");
            //    extra.Add("result_url", "http://www.yourdomain.com/result");
            //}
            //else if (channel.Equals("jdpay_wap"))
            //{
            //    extra.Add("success_url", "http://www.yourdomain.com/success");
            //    extra.Add("fail_url", "http://www.yourdomain.com/fail");
            //    extra.Add("token", "fjdilkkydoqlpiunchdysiqkanczxude");//32 位字符串
            //}

            Dictionary<string, string> app = new Dictionary<string, string>();

            string appid = ConfigurationManager.AppSettings["AppId"];
            app.Add("id", appid);

            Dictionary<string, object> param = new Dictionary<string, object>
            {
                { "order_no", orderid },
                { "channel", channel },
                { "currency", "cny" },
                { "amount", amount },
                { "subject", body },
                { "body", body }
            };

            IPHostEntry myEntry = Dns.GetHostEntry(Dns.GetHostName());
            string ipaddress = myEntry.AddressList.FirstOrDefault<IPAddress>(c => c.AddressFamily.ToString().Equals("InterNetwork")).ToString();
            param.Add("client_ip", ipaddress);
            param.Add("app", app);
            param.Add("extra", extra);

            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "type", extraType }
            };
            param.Add("metadata", metadata);

            return param;
        }
        #endregion
    }
}
