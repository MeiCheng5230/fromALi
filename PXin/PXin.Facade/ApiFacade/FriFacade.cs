using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.Models;
using Common.UEPay;
using PXin.DB;
using PXin.Facade.Models;
using PXin.Facade.Models.Enum;
using PXin.Model;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class FriFacade : FacadeBase<PXinContext>
    {

        #region 充值V点
        /// <summary>
        /// 充值V点
        /// </summary>
        /// <param name="chargeVDian"></param>
        /// <returns></returns>
        public bool ChargeVDian(ReqChargeVDian chargeVDian)
        {
            log.Info($"ChargeVDian,充值V点,nodeid={chargeVDian.Nodeid},充值数量={chargeVDian.Price}");
            TnetReginfo regInfo = PxinCache.GetRegInfo(chargeVDian.Nodeid);
            if (chargeVDian.PayType == 0)
            {
                //调用ue支付
                return ChargeVDian_DosUEPrepare(regInfo, chargeVDian.Price);
            }
            else
            {
                return ChargeVDian_SVPay(regInfo, chargeVDian.Price, chargeVDian.PayPwd);
            }
        }


        /// <summary>
        /// 调用ue支付
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        private bool ChargeVDian_DosUEPrepare(TnetReginfo regInfo, decimal Price)
        {
            TpcnUepayconfig ueConfig = db.TpcnUepayconfigSet.FirstOrDefault(a => a.Typeid == 2);
            if (ueConfig == null || ueConfig.Id <= 0)
            {
                Alert("获取UE支付配置失败");
                return false;
            }
            //获取配置DOS兑换V点比例
            TappConfig config = db.TappConfigSet.FirstOrDefault(a => a.Propertyname == "chargerate");
            decimal chargerate = config == null ? 0.01M : decimal.Parse(config.Propertyvalue);
            decimal amount = decimal.Parse(Price.ToString()) * chargerate;
            if (amount < 0.01M)
            {
                amount = 0.01M;
            }
            Currency currency = new Currency(CurrencyType.DOS_矿沙, amount);
            decimal total = currency.Amount;
            int unit = currency.Type.CurrencyId;
            TnetUepayhis uePayHis = new TnetUepayhis { Typeid = 20001, Nodeid = regInfo.Nodeid, BusinessParams = Price.ToString(), Amount = total, Unit = unit, Freezeids = "", Createtime = DateTime.Now };
            db.TnetUepayhisSet.Add(uePayHis);
            if (db.SaveChanges() <= 0)
            {
                Alert("生成UE订单失败");
                return false;
            }
            ChargeUE = new ChargeDto
            {
                businesstypeid = 20001,
                amount = total,
                unit = unit,
                body = "充值V点",
                subject = "充值V点",
                orderno = uePayHis.Id.ToString(),
                paycode = ueConfig.Paycode,
                noticeurl = Helper.DomainUrl + "/UENotice/Success",
                createtime = uePayHis.Createtime.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return true;
        }

        /// <summary>
        /// SV充值V点
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="Price"></param>
        /// <param name="payPwd"></param>
        /// <param name="isCheckPayPwd"></param>
        /// <returns></returns>
        public bool ChargeVDian_SVPay(TnetReginfo regInfo, decimal Price, string payPwd, bool isCheckPayPwd = true)
        {
            //检查支付密码
            if (isCheckPayPwd && !CheckPayPwd(regInfo, payPwd, true))
            {
                //Alert("支付密码错误");
                return false;
            }
            BeginTransfer();
            Purse fromPurse = purseFactory.UserCVPurse(regInfo.Nodeid);
            Purse toPurse = purseFactory.SystemPurseRand(regInfo.Nodeid);
            Currency currency = new Currency(CurrencyType.RMB, Price * 0.1m); //1sv=10v点
            if (fromPurse.UsableBalance < currency.Amount)
            {
                Alert("转账钱包余额不足", -100);//此Result在自动充值V点中使用
                return false;
            }
            var result = Transfer(fromPurse, toPurse, currency, 12, "SV充值V点");
            if (!result.IsSuccess)
            {
                EndTransfer(false);
                return false;
            }
            db.BeginTransaction();
            if (!ChargeVDian_Pro(regInfo, Price))
            {
                db.Rollback();
                EndTransfer(false);
                return false;
            }

            db.Commit();
            EndTransfer(true);
            return result.IsSuccess;
        }
        /// <summary>
        /// ue支付回调
        /// </summary>
        /// <param name="uePayHis"></param>
        /// <returns></returns>
        public bool ChargeVDian_Notice(TnetUepayhis uePayHis)
        {
            db.BeginTransaction();
            try
            {
                TnetReginfo regInfo = PxinCache.GetRegInfo(uePayHis.Nodeid);
                int Price = int.Parse(uePayHis.BusinessParams);
                if (!ChargeVDian_Pro(regInfo, Price))
                {
                    db.Rollback();
                    return false;
                }
                uePayHis.BusinessId = _businessID;
                uePayHis.Status = 2;
                db.Entry(uePayHis).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() <= 0)
                {
                    Alert("更新订单状态失败");
                    db.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Info("充值V点，支付回调失败。原因：" + ex);
                Alert("更新订单状态失败");
                db.Rollback();
                return false;
            }
            db.Commit();
            return true;
        }

        /// <summary>
        /// 支付V点流程
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        public bool ChargeVDian_Pro(TnetReginfo regInfo, decimal Price)
        {
            TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == regInfo.Nodeid);
            if (userinfo == null)
            {
                userinfo = new TpxinUserinfo
                {
                    Nodeid = regInfo.Nodeid,
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
            //1.添加 信友圈付费V点历史表 TPXIN_PAYHIS
            TpxinPayhis payhis = new TpxinPayhis
            {
                Createtime = DateTime.Now,
                Infoid = 0,
                Typeid = 1,
                Nodeid = regInfo.Nodeid,
                Tonodeid = 0,
                Price = Price,
                Remarks = "充值V点"
            };
            //2.修改用户信息表的v点
            db.TpxinPayhisSet.Add(payhis);
            //userinfo.V += Price;
            //3.添加金额变化记录
            //var amountChangeHis = CreateAmountChangeHis(regInfo.Nodeid, 1, Price, (int)AmountChangeReason.ChargeVDian, Guid.NewGuid().ToString(), "充值V点");
            //db.TpxinAmountChangeHisSet.Add(amountChangeHis);
            if (db.SaveChanges() <= 0)
            {
                Alert("充值V点失败");
                return false;
            }
            //由VP服务处理V点
            var vp = new VPHelper();
            var result = vp.SetV(new VPChargeVDian
            {
                Amount = Price,
                Nodeid = regInfo.Nodeid,
                Reason = (int)AmountChangeReason.ChargeVDian,
                Remark = "充值V点",
                Transferid = payhis.Hisid.ToString(),
            });
            if (result.Result <= 0)
            {
                Alert("充值V点失败");
                log.Error("充值V点失败:" + result.Message);
                return false;
            }
            _businessID = payhis.Hisid;
            return true;
        }
        #endregion

        #region 支付V点(查看文章)
        /// <summary>
        /// 支付V点(查看文章)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public MessageDto PayVDian(ReqPayVDian req)
        {
            #region 数据验证
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (regInfo == null)
            {
                Alert("用户不存在");
                return null;
            }
            //检查支付密码
            //if (!CheckPayPwd(regInfo, req.Pwd, false))
            //{
            //    Alert("支付密码错误");
            //    return false;
            //}

            TpxinMessage tpxinMessage = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.InfoID && a.Status == 1);
            if (tpxinMessage == null)
            {
                Alert("文章不存在");
                return null;
            }
            if (tpxinMessage.Price == 0)
            {
                Alert("该文章不需要收费");
                return null;
            }
            if (tpxinMessage.Nodeid == regInfo.Nodeid)
            {
                Alert("不能给自己的文章付费");
                return null;
            }
            //TpxinUserinfo tpxinUserinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == regInfo.Nodeid);
            var vp = new VPHelper();
            var vpDian = vp.GetTpxinUserinfo(regInfo.Nodeid);
            if (vpDian.VDianBalance < tpxinMessage.Price)
            {
                Alert("V点余额不足，请先充值");
                return null;
            }
            TpxinPayhis pay = db.TpxinPayhisSet.FirstOrDefault(a => a.Infoid == tpxinMessage.Infoid && a.Nodeid == regInfo.Nodeid && a.Typeid == 3);
            if (pay != null)
            {
                Alert("您已支付查看文章费用");
                return null;
            }

            #endregion
            try
            {
                db.BeginTransaction();
                //添加查看用户v点支付历史
                TpxinPayhis payhis = new TpxinPayhis()
                {
                    Createtime = DateTime.Now,
                    Infoid = tpxinMessage.Infoid,
                    Typeid = 3,
                    Nodeid = regInfo.Nodeid,
                    Tonodeid = tpxinMessage.Nodeid,
                    Price = tpxinMessage.Price,
                    Remarks = "查看文章"
                };
                db.TpxinPayhisSet.Add(payhis);
                //减去查看用户v点数量
                //TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == regInfo.Nodeid);
                //if (userinfo.V <= 0)
                //{
                //    Alert("支付失败,V点不足");
                //    return null;
                //}
                //userinfo.V -= tpxinMessage.Price;
                //var transferId = Guid.NewGuid().ToString();

                //增加发布用户v点数量
                //TpxinUserinfo userinfo1 = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == tpxinMessage.Nodeid);
                //userinfo1.V += tpxinMessage.Price;

                var tpxinMsgUser = PxinCache.GetRegInfo(tpxinMessage.Nodeid);
                var nodeName = tpxinMsgUser == null ? "" : tpxinMsgUser.Nodename;

                //添加金额变化记录
                //var reduce = CreateAmountChangeHis(regInfo.Nodeid, 1, -tpxinMessage.Price, (int)AmountChangeReason.ViewArticle, transferId, $"查看付费-{nodeName}");
                //db.TpxinAmountChangeHisSet.Add(reduce);

                //var add = CreateAmountChangeHis(tpxinMessage.Nodeid, 1, tpxinMessage.Price, (int)AmountChangeReason.ViewArticle, transferId, $"查看收款-{regInfo.Nodename}");
                //db.TpxinAmountChangeHisSet.Add(add);

                if (db.SaveChanges() <= 0)
                {
                    Alert("支付失败:" + db.Message);
                    log.Error("查看文章,支付失败,db:" + db.Message);
                    db.Rollback();
                    return null;
                }
                //VP服务设置V点

                var result = vp.SetV(new VPPayVDian
                {
                    FromNodeid = regInfo.Nodeid,
                    FromRemark = $"查看付费-{Helper.FilterChar(nodeName)}",
                    ToNodeid = tpxinMessage.Nodeid,
                    ToRemark = $"查看收款-{Helper.FilterChar(regInfo.Nodename)}",
                    Amount = tpxinMessage.Price,
                    Reason = (int)AmountChangeReason.ViewArticle,
                    Transferid = payhis.Hisid.ToString(),
                });
                if (result.Result <= 0)
                {
                    Alert(result.Message, result.Result);
                    db.Rollback();
                    return null;
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                log.Info("查看文章,支付失败。原因：" + ex);
                Alert("支付失败");
                db.Rollback();
                return null;
            }

            var query = from msg in db.VpxinMessageSet
                        where msg.Infoid == req.InfoID
                        select new MessageDto
                        {
                            Commentnum = msg.Commentnum,
                            Nodeid = msg.Msgnodeid,
                            Localnodeid = msg.Localnodeid,
                            Content = msg.Content,
                            Createtime = msg.Createtime,
                            Down = msg.Down,
                            Infoid = msg.Infoid,
                            Ispay = msg.Ispay,
                            Picurl = msg.Picurl,
                            Price = msg.Price,
                            Reward = msg.Reward,
                            Sound = msg.Sound,
                            Up = msg.Up,
                            Video = msg.Video,
                            IsDown = msg.IsDown,
                            IsUp = msg.IsUp
                        };
            MessageDto msgModel = query.FirstOrDefault();
            Alert("支付成功", 1);
            return msgModel;
        }
        #endregion

        #region V点交易记录
        /// <summary>
        /// V点交易记录 历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PxinPayhisDto> GetVDianHis(ReqGetPVDianHis req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            var query = from pay in db.VpxinPayhisSet

                        where pay.Nodeid == regInfo.Nodeid
                        select new PxinPayhisDto
                        {
                            Hisid = pay.Hisid,
                            Price = pay.Price,
                            Nodename = pay.Nodename,
                            Createtime = pay.Createtime,
                            Typeid = pay.Typeid
                        };
            query = query.OrderByDescending(a => a.Createtime).Skip(req.PageSize * (req.PageIndex - 1)).Take(req.PageSize);//进行分页
            List<PxinPayhisDto> HisList = query.ToList();
            return HisList;
        }
        #endregion

        #region P点历史
        /// <summary>
        /// P点历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<PxinPraiseDto> GetPDianHis(Models.ReqGetPVDianHis req)
        {
            //TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            var query = from Praise in db.VpxinPraiseSet
                        where Praise.Nodeid == req.Nodeid
                        select new PxinPraiseDto
                        {
                            Hisid = Praise.Hisid,
                            Createtime = Praise.Createtime,
                            Nodename = Praise.Nodename,
                            Reward = Praise.Reward,
                            Status = Praise.Status,
                            Type = Praise.Type,
                            // Statusname = (Praise.Type == 1) ? "踩-" + Praise.Nodename + "" : (Praise.Type == 2) ? "赞-" + Praise.Nodename + "" : (Praise.Type == 3) ? "打赏-" + Praise.Nodename + "" : "赏金-" + Praise.Nodename + "",
                        };
            query = query.OrderByDescending(a => a.Createtime).Skip(req.PageSize * (req.PageIndex - 1)).Take(req.PageSize);//进行分页
            List<PxinPraiseDto> HisList = query.ToList();
            return HisList;
        }
        #endregion

        #region 新版V点P点交易历史
        /// <summary>
        /// 新版V点P点交易历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IList<PxinAmountChangeHisDto> GetPxinAmountChangeHis(PxinAmountChangeHisReq req)
        {
            var result = db.TpxinAmountChangeHisSet.Where(p => p.Nodeid == req.Nodeid && p.Typeid == req.TypeId)
                                                   .Select(p => new PxinAmountChangeHisDto
                                                   {
                                                       TypeId = p.Reason,
                                                       Remark = p.Remarks,
                                                       Price = p.Amount,
                                                       _balanceafter = p.Amountafter,
                                                       CreateTime = p.Createtime
                                                   })
                                                   .OrderByDescending(a => a.CreateTime)
                                                   .Skip(req.PageSize * (req.PageIndex - 1)).Take(req.PageSize)
                                                   .ToList();
            return result;
        }
        #endregion

        #region 发布信友圈信息
        /// <summary>
        /// 发布信友圈信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateMsg(ReqPxinMessage req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinMessage tpxinMessage = new TpxinMessage
            {
                Infoid = db.GetPrimaryKeyValue<TpxinMessage>(),
                Nodeid = req.Nodeid,
                Price = req.Price,
                Content = req.Content,
                Picurl = req.Picurl,
                Sound = req.Sound,
                Video = req.Video,
                Createtime = DateTime.Now,
                Remarks = "",
                Status = 1,
                Up = 0,
                Down = 0,
                Commentnum = 0
            };
            try
            {
                db.BeginTransaction();
                //添加信友圈信息
                db.TpxinMessageSet.Add(tpxinMessage);

                //发布信息扣一个v点
                //1.添加v点历史
                db.TpxinPayhisSet.Add(new TpxinPayhis
                {
                    Nodeid = req.Nodeid,
                    Tonodeid = 0,
                    Createtime = DateTime.Now,
                    Infoid = tpxinMessage.Infoid,
                    Price = -1,
                    Remarks = "发布文章",
                    Typeid = 2
                });
                //2.扣掉信友圈信息表的V点数量
                //TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == req.Nodeid);
                //userinfo.V -= 1;
                //if (userinfo.V < 0)
                //{
                //    Alert("发布信友圈失败，V点不足", -100);
                //    return false;
                //}
                ////添加金额变化记录
                //var amountChangeHis = CreateAmountChangeHis(req.Nodeid, 1, -1, (int)AmountChangeReason.PublishArticle, Guid.NewGuid().ToString(), "发布文章");
                //db.TpxinAmountChangeHisSet.Add(amountChangeHis);

                #region 由VP服务来处理V点P点操作
                if (db.SaveChanges() <= 0)
                {
                    Alert("发布信友圈失败");
                    log.Error("发布信友圈失败,提交db失败：" + db.Message);
                    db.Rollback();
                    return false;
                }
                var vp = new VPHelper();
                var result = vp.SetV(new VPChargeVDian
                {
                    Nodeid = req.Nodeid,
                    Amount = -1,
                    Reason = (int)AmountChangeReason.PublishArticle,
                    Remark = "发布文章",
                    Transferid = tpxinMessage.Infoid.ToString(),
                });
                if (result.Result <= 0)
                {
                    Alert(result.Message, result.Result);
                    db.Rollback();
                    return false;
                }
                db.Commit();
                #endregion


            }
            catch (Exception ex)
            {
                log.Error("发布信友圈失败，原因" + ex);
                Alert("发布信友圈失败");
                db.Rollback();
                return false;
            }
            Alert("发布信友圈成功", 1);
            PxinSerivce.EnqueueMsg(tpxinMessage.Infoid);
            return true;
        }

        #endregion

        #region 删除信友圈
        /// <summary>
        /// 删除信友圈
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteMsg(ReqDeleteMsg req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinMessage message = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Status == 1);
            if (message == null)
            {
                Alert("删除的信友圈不存在");
                return false;
            }
            message.Status = 0;
            if (db.SaveChanges() <= 0)
            {
                Alert("删除信友圈失败");
                return false;
            }
            Alert("删除信友圈成功", 1);
            return true;
        }
        #endregion

        #region 获取信友圈用户基本信息

        /// <summary>
        /// 获取信友圈用户基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<UserinfoDto> GetUserInfo(ReqUserInfo req)
        {
            int[] Snodeids;
            if (string.IsNullOrEmpty(req.Snodeids))
            {
                AddUserInfo(req.Nodeid);
                Snodeids = new int[] { req.Nodeid };
            }
            else
            {
                Snodeids = Array.ConvertAll(req.Snodeids.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries), s => Convert.ToInt32(s));
            }
            var query = from user in db.TpxinUserinfoSet
                        join reginfo in db.TnetReginfoSet.Select(a => new { a.Nodename, a.Nodeid }) on user.Nodeid equals reginfo.Nodeid
                        join chatuser in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid).Select(a => new { a.Nickname, a.Friendnodeid }) on user.Nodeid equals chatuser.Friendnodeid into chatuser_join
                        from chatuser in chatuser_join.DefaultIfEmpty()
                        join photo in db.TnetUserphotoSet.Select(a => new { a.Appphoto, a.Nodeid }) on user.Nodeid equals photo.Nodeid into photo_join
                        from photo in photo_join.DefaultIfEmpty()
                        join cu in db.TchatUserSet.Select(p => new { p.Personalsign, p.Nodeid }) on user.Nodeid equals cu.Nodeid into cu_join
                        from cu in cu_join.DefaultIfEmpty()
                        where Snodeids.Contains(user.Nodeid)
                        select new UserinfoDto
                        {
                            Nickname = chatuser.Nickname,
                            Nodeid = user.Nodeid,
                            Nodename = reginfo.Nodename,
                            Appphoto = photo.Appphoto,
                            Backpic = user.Backpic,
                            Createtime = user.Createtime,
                            Down = user.Down,
                            Infoid = user.Infoid,
                            P = user.P,
                            Remarks = user.Remarks,
                            Up = user.Up,
                            V = user.V,
                            Personalsign = cu.Personalsign
                        };
            var list = query.ToList();
            var vp = new VPHelper();
            for (int i = 0; i < list.Count; i++)
            {
                var vpDian= vp.GetTpxinUserinfo(list[i].Nodeid);
                list[i].V = vpDian.VDianBalance;
                list[i].P = vpDian.PDianBalance;
            }
            return list;
        }

        /// <summary>
        /// 添加信友圈信息
        /// </summary>
        /// <param name="Nodeid"></param>
        /// <returns></returns>
        public bool AddUserInfo(int Nodeid)
        {
            TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == Nodeid);
            if (userinfo == null)
            {
                db.TpxinUserinfoSet.Add(new TpxinUserinfo
                {
                    Nodeid = Nodeid,
                    Backpic = "",
                    Createtime = DateTime.Now,
                    Up = 0,
                    Down = 0,
                    P = 0,
                    V = 0,
                    Remarks = ""
                });
                if (db.SaveChanges() <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 获取信友圈消息
        /// <summary>
        /// 获取信友圈-根据用户查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public FriMessageCollection GetMsg(ReqGetMsg req)
        {
            FriMessageCollection result = new FriMessageCollection();
            TnetReginfo regInfo = db.TnetReginfoSet.Find(req.Snodeid);
            if (regInfo == null)
            {
                Alert("用户不存在");
                return null;
            }
            //当前用户是否是查看用户好友
            List<TchatFriend> friList = db.TchatFriendSet.Where(c => c.Friendstatus == 1 && (c.Mynodeid == req.Nodeid || c.Friendnodeid == req.Nodeid)).ToList();
            //查看用户是达人时不是好友也可以查看他的朋友圈
            var user = db.TnetReginfoSet.Where(c => c.Nodeid == req.Snodeid).FirstOrDefault();
            if ((user?.Isenterprise != 3 && user?.Isenterprise != 4))
            {
                if (!(friList.Select(a => a.Mynodeid).Contains(req.Snodeid) || friList.Select(a => a.Friendnodeid).Contains(req.Snodeid)))
                {
                    Alert("请添加好友再查看该用户信友圈");
                    return null;
                }
            }

            var query = from msg in db.TpxinMessageSet
                        join tp in db.TpxinPayhisSet.Where(a => a.Nodeid == req.Nodeid) on msg.Infoid equals tp.Infoid into tp_join
                        from tp in tp_join.DefaultIfEmpty()
                        join tp2 in db.TpxinPraiseSet.Where(a => a.Fromnodeid == req.Nodeid) on msg.Infoid equals tp2.Infoid into tp2_join
                        from tp2 in tp2_join.DefaultIfEmpty()
                        where msg.Nodeid == req.Snodeid && msg.Status == 1
                        select new MessageDto
                        {
                            Commentnum = msg.Commentnum,
                            Createtime = msg.Createtime,
                            Down = msg.Down,
                            Infoid = msg.Infoid,
                            Up = msg.Up,
                            Nodeid = msg.Nodeid,
                            Localnodeid = req.Nodeid,
                            Ispay = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? 1 : 0,
                            IsUp = (tp2.Fromnodeid == req.Nodeid && tp2.Tonodeid == msg.Nodeid && tp2.Status == 1) ? 1 : 0,
                            IsDown = (tp2.Fromnodeid == req.Nodeid && tp2.Tonodeid == msg.Nodeid && tp2.Status == -1) ? 1 : 0,
                            Reward = tp2 == null ? 0 : tp2.Reward,
                            Price = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? 0 : msg.Price,
                            Content = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? msg.Content : (msg.Content.Substring(0, 5) + "..."),
                            Video = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? msg.Video : "",
                            Sound = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? msg.Sound : "",
                            Picurl = (msg.Nodeid == req.Nodeid || msg.Price == 0 || (tp != null && tp.Hisid > 0)) ? msg.Picurl : ""
                        };


            result.Messages = query.OrderByDescending(a => a.Createtime).Skip(req.PageSize * (req.PageIndex - 1)).Take(req.PageSize).ToList();//进行分页
            int[] infos = result.Messages.Select(a => a.Infoid).ToArray();
            int[] mynodeids = friList.Select(a => a.Mynodeid).ToArray();
            int[] Friendnodeids = friList.Select(a => a.Friendnodeid).ToArray();
            var queryComment = from comment in db.TpxinCommentHisSet
                               join msg in db.TpxinMessageSet on comment.Infoid equals msg.Infoid
                               where infos.Contains(msg.Infoid) && (mynodeids.Contains(comment.Nodeid) || Friendnodeids.Contains(comment.Nodeid))
                               orderby comment.Createtime ascending
                               select new CommentDto
                               {
                                   Content = comment.Content,
                                   Createtime = comment.Createtime,
                                   Hisid = comment.Hisid,
                                   Infoid = comment.Infoid,
                                   Nodeid = comment.Nodeid,
                                   Phisid = comment.Phisid,
                                   Pnodeid = comment.Pnodeid
                               };
            result.Comments = queryComment.ToList();
            return result;
        }
        /// <summary>
        /// 获取信友圈消息[首页]
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public FriMessageCollection GetMsgHome(ReqGetMsgHome req)
        {
            TnetReginfo reginfo = HttpContext.Current.GetRegInfo();
            FriMessageCollection result = new FriMessageCollection();
            var queryMsg = from msg in db.VpxinMessageSet
                           where msg.Localnodeid == req.Nodeid && msg.Createtime > req.StartTime
                           select new MessageDto
                           {
                               Commentnum = msg.Commentnum,
                               Nodeid = msg.Msgnodeid,
                               Localnodeid = msg.Localnodeid,
                               Content = msg.Content,
                               Createtime = msg.Createtime,
                               Down = msg.Down,
                               Infoid = msg.Infoid,
                               Ispay = msg.Ispay,
                               Picurl = msg.Picurl,
                               Price = msg.Price,
                               Reward = msg.Reward,
                               Sound = msg.Sound,
                               Up = msg.Up,
                               Video = msg.Video,
                               IsDown = msg.IsDown,
                               IsUp = msg.IsUp,
                               Hisid = msg.Hisid
                           };
            result.Messages = queryMsg.OrderByDescending(a => a.Createtime).Skip(req.PageSize * (req.PageIndex - 1)).Take(req.PageSize).ToList();
            bool isResult = IsMessage(result.Messages);
            int[] infos = result.Messages.Select(a => a.Infoid).ToArray();
            var queryComment = from mu in db.TpxinMessageUesrSet
                               join comment in db.TpxinCommentHisSet on new { mu.Typeid, mu.Infoid } equals new { Typeid = 1, Infoid = comment.Hisid }
                               join msg in db.TpxinMessageSet on comment.Infoid equals msg.Infoid
                               where mu.Nodeid == reginfo.Nodeid && msg.Createtime > req.StartTime && infos.Contains(msg.Infoid)
                               orderby comment.Createtime ascending
                               select new CommentDto
                               {
                                   Content = comment.Content,
                                   Createtime = comment.Createtime,
                                   Hisid = comment.Hisid,
                                   Infoid = comment.Infoid,
                                   Nodeid = comment.Nodeid,
                                   Phisid = comment.Phisid,
                                   Pnodeid = comment.Pnodeid
                               };
            result.Comments = queryComment.ToList();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsMessage(IEnumerable<MessageDto> messageDtos)
        {
            List<int> Hisids = messageDtos.Select(a => a.Hisid).ToList();
            List<TpxinMessageUesr> tpxinMessageUesrs = db.TpxinMessageUesrSet.Where(a => Hisids.Contains(a.Hisid)).ToList();
            foreach (var item in tpxinMessageUesrs)
            {
                item.Status = 1;
            }
            return db.SaveChanges() > 0;
        }

        #endregion

        #region 评论
        /// <summary>
        /// 评论id
        /// </summary>
        public int CommentHisId { get; private set; }
        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateComment(ReqCreateComment req)
        {
            if (req.Content.Length > 100)
            {
                Alert("评论不能超过100字哦");
                return false;
            }
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TpxinMessage tpxinMessage = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Status == 1);
            if (tpxinMessage == null)
            {
                Alert("信友圈信息不存在");
                return false;
            }
            //发布文章的用户不只是自己的要支付V点
            if (tpxinMessage.Price > 0 && tpxinMessage.Nodeid != regInfo.Nodeid)
            {
                TpxinPayhis tpxinPayhis = db.TpxinPayhisSet.FirstOrDefault(a => a.Infoid == tpxinMessage.Infoid && a.Nodeid == req.Nodeid && a.Typeid == 3);
                if (tpxinPayhis == null)
                {
                    Alert("请支付V点查看后评论");
                    return false;
                }
            }
            int Pnodeid = 0;
            if (req.Phisid > 0)
            {
                TpxinCommentHis His = db.TpxinCommentHisSet.FirstOrDefault(a => a.Hisid == req.Phisid && a.Status == 1 && a.Infoid == req.Infoid);
                if (His == null)
                {
                    Alert("回复的评论不存在,或已删除");
                    return false;
                }
                Pnodeid = His.Nodeid;
                if (Pnodeid == req.Nodeid)
                {
                    Alert("自己不能回复自己的评论");
                    return false;
                }
            }
            //添加评论历史表
            TpxinCommentHis commentHis = new TpxinCommentHis
            {
                Nodeid = req.Nodeid,
                Content = req.Content,
                Createtime = DateTime.Now,
                Infoid = req.Infoid,
                Remarks = "",
                Status = 1,
                Phisid = req.Phisid,
                Pnodeid = Pnodeid
            };
            db.TpxinCommentHisSet.Add(commentHis);
            //添加文章评论次数
            tpxinMessage.Commentnum += 1;
            if (db.SaveChanges() <= 0)
            {
                Alert("评论失败");
                return false;
            }
            PxinSerivce.EnqueueComment(commentHis.Hisid);
            Alert("评论成功", 1);
            CommentHisId = commentHis.Hisid;
            return true;
        }
        #endregion

        #region 点赞或踩
        /// <summary>
        /// 点赞或踩
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateAttitude(ReqCreateAttitude req)
        {

            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinMessage tpxinMessage = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Status == 1);
            if (tpxinMessage == null)
            {
                Alert("信友圈信息不存在");
                return false;
            }
            if (req.Isupdown != -1 && req.Isupdown != 1)
            {
                Alert("参数不正确");
                return false;
            }
            if (regInfo.Nodeid == tpxinMessage.Nodeid)
            {
                Alert("不能自己给自己点赞或踩");
                return false;
            }
            if (tpxinMessage.Price > 0)
            {
                TpxinPayhis tpxinPayhis = db.TpxinPayhisSet.FirstOrDefault(a => a.Infoid == tpxinMessage.Infoid && a.Nodeid == req.Nodeid && a.Typeid == 3);
                if (tpxinPayhis == null)
                {
                    Alert("请支付V点查看后点赞或踩");
                    return false;
                }
            }
            TpxinPraise tpxinPraise = db.TpxinPraiseSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Fromnodeid == req.Nodeid);
            if (tpxinPraise != null && tpxinPraise.Status != 0)
            {
                Alert("一个文章只能点赞或踩一次");
                return false;
            }
            try
            {
                db.BeginTransaction();
                if (tpxinPraise == null)
                {
                    //添加信友圈踩赞历史表
                    tpxinPraise = new TpxinPraise
                    {
                        Infoid = req.Infoid,
                        Createtime = DateTime.Now,
                        Fromnodeid = req.Nodeid,
                        Tonodeid = tpxinMessage.Nodeid,
                        Remarks = "",
                        Reward = 0,
                        Status = req.Isupdown
                    };
                    db.TpxinPraiseSet.Add(tpxinPraise);
                }
                else
                {
                    tpxinPraise.Status = req.Isupdown;
                }
                //查询法比用户信息
                TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == tpxinMessage.Nodeid);
                if (userinfo == null)
                {
                    Alert("用户不存在");
                    db.Rollback();
                    return false;
                }
                if (req.Isupdown == 1)
                {
                    //添加文章表的赞的次数
                    tpxinMessage.Up += 1;
                    //增加一个用户p点
                    userinfo.Up += 1;
                }
                else
                {
                    //添加文章表的踩的次数
                    tpxinMessage.Down += 1;
                    //减去一个用户p点
                    userinfo.Down += 1;
                }
                ////添加金额变化记录
                //var amount = req.Isupdown == 1 ? 1 : -1;
                //var reason = req.Isupdown == 1 ? AmountChangeReason.PraiseArticle : AmountChangeReason.TreadArticle;
                ////var tpxinMsgUser = PxinCache.GetRegInfo(userinfo.Nodeid);//db.TchatUserSet.First(c => c.Nodeid == userinfo.Nodeid);//

                //var remarks = (req.Isupdown == 1 ? "赞-" : "踩-") + regInfo.Nodename;
                //var amountChangeHis = CreateAmountChangeHis(tpxinMessage.Nodeid, 2, amount, (int)reason, Guid.NewGuid().ToString(), remarks);
                //db.TpxinAmountChangeHisSet.Add(amountChangeHis);

                #region 由VP服务来处理V点P点操作
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    log.Error("点赞点踩失败,提交db失败：" + db.Message);
                    db.Rollback();
                    return false;
                }
                var vp = new VPHelper();
                var result = vp.SetP(new VPAuction
                {
                    Nodeid = tpxinMessage.Nodeid,
                    Reason = (int)(req.Isupdown == 1 ? AmountChangeReason.PraiseArticle : AmountChangeReason.TreadArticle),
                    Remark = (req.Isupdown == 1 ? "赞-" : "踩-") + regInfo.Nodename,
                    Amount = req.Isupdown == 1 ? 1 : -1,
                    Transferid = tpxinPraise.Hisid.ToString(),
                });
                if (result.Result <= 0)
                {
                    Alert(result.Message, result.Result);
                    db.Rollback();
                    return false;
                }
                db.Commit();
                #endregion

            }
            catch (Exception ex)
            {
                log.Info("点赞或踩失败，原因：" + ex);
                Alert("操作失败");
                db.Rollback();
                return false;
            }
            Alert("操作成功", 1);
            return true;
        }
        #endregion

        #region 打赏
        /// <summary>
        /// 打赏
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateReward(ReqCreateReward req)
        {
            if (req.Reward > 100)
            {
                Alert("打赏金额不能超过100V");
                return false;
            }
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinMessage tpxinMessage = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Status == 1);
            if (tpxinMessage == null)
            {
                Alert("信友圈信息不存在");
                return false;
            }
            if (regInfo.Nodeid == tpxinMessage.Nodeid)
            {
                Alert("不能自己给自己打赏");
                return false;
            }
            if (tpxinMessage.Price > 0)
            {
                TpxinPayhis tpxinPayhis = db.TpxinPayhisSet.FirstOrDefault(a => a.Infoid == tpxinMessage.Infoid && a.Nodeid == req.Nodeid && a.Typeid == 3);
                if (tpxinPayhis == null)
                {
                    Alert("请支付V点查看后点赞或踩");
                    return false;
                }
            }
            TpxinPraise tpxinPraise = db.TpxinPraiseSet.FirstOrDefault(a => a.Infoid == req.Infoid && a.Fromnodeid == req.Nodeid);
            if (tpxinPraise != null && tpxinPraise.Reward > 0)
            {
                Alert("一个文章只能打赏一次");
                return false;
            }
            #region 由VP服务设置V点
            try
            {
                db.BeginTransaction();
                if (tpxinPraise == null)
                {
                    //添加打赏用户踩赞历史表
                    tpxinPraise = new TpxinPraise
                    {
                        Infoid = req.Infoid,
                        Createtime = DateTime.Now,
                        Fromnodeid = req.Nodeid,
                        Tonodeid = tpxinMessage.Nodeid,
                        Remarks = "",
                        Reward = req.Reward,
                        Status = 0
                    };
                    db.TpxinPraiseSet.Add(tpxinPraise);
                }
                else
                {
                    tpxinPraise.Reward = req.Reward;
                }

                var tpxinMsgUser = PxinCache.GetRegInfo(tpxinMessage.Nodeid);
                if (db.SaveChanges() <= 0)
                {
                    Alert("打赏失败");
                    log.Error("打赏失败,提交db失败:" + db.Message);
                    db.Rollback();
                    return false;
                }
                var vp = new VPHelper();
                var result = vp.SetV(new VPPayVDian
                {
                    FromNodeid = req.Nodeid,
                    FromRemark = $"打赏-{Helper.FilterChar(tpxinMsgUser.Nodename)}",
                    ToNodeid = tpxinMessage.Nodeid,
                    ToRemark = $"赏金-{Helper.FilterChar(regInfo.Nodename)}",
                    Amount = req.Reward,
                    Reason = (int)AmountChangeReason.Reward,
                    Transferid = tpxinPraise.Hisid.ToString(),
                });
                if (result.Result <= 0)
                {
                    Alert(result.Message, result.Result);
                    db.Rollback();
                    return false;
                }
                db.Commit();
            }
            catch (Exception ex)
            {
                Alert("打赏失败");
                log.Error("打赏失败,异常:" + ex);
                db.Rollback();
                return false;
            }
            #endregion
            Alert("打赏成功", 1);
            return true;
        }
        #endregion

        #region 修改背景图片
        /// <summary>
        /// 修改背景图片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateBackgImg(ReqUpdateBackgImg req)
        {
            //http://client.xiang-xin.net/images2/tempfile/20190817/f6745392-2006-42cf-b63a-8094e944a7b0.JPEG
            string tempUrl = "/" + req.BackImg.Substring(req.BackImg.IndexOf("images2/", StringComparison.OrdinalIgnoreCase));
            string tempFile = HttpContext.Current.Request.MapPath(tempUrl).ToLower();
            string destFile = tempFile.Replace("tempfile", "backimg");
            string destDir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            File.Move(tempFile, destFile);
            string destUrl = AppConfig.Userphoto.Trim('/') + "/" + destFile.Substring(destFile.IndexOf("backimg")).Replace("\\", "/");
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinUserinfo tpxinUserinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == req.Nodeid);
            tpxinUserinfo.Backpic = destUrl;
            if (db.SaveChanges() <= 0)
            {
                Alert("修改背景图片失败");
                return false;
            }
            Alert("修改背景图片成功", 1);
            return true;
        }
        #endregion

        #region 举报信友圈
        /// <summary>
        /// 添加举报信友圈信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateReport(ReqReport req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TpxinMessage tpxinMessage = db.TpxinMessageSet.FirstOrDefault(a => a.Infoid == req.InfoId && a.Status == 1);
            if (tpxinMessage == null)
            {
                Alert("信友圈信息不存在");
                return false;
            }
            TpxinReport tpxinReport = db.TpxinReportSet.FirstOrDefault(a => a.Infoid == req.InfoId && a.Satatus == 0 && a.Nodeid == req.Nodeid);
            if (tpxinReport != null)
            {
                Alert("您已举报该信友圈，请等待管理员审核");
                return false;
            }
            db.TpxinReportSet.Add(new TpxinReport
            {
                Createtime = DateTime.Now,
                Infoid = req.InfoId,
                Nodeid = regInfo.Nodeid,
                Reason = req.Reason,
                Remarks = req.Remarks,
                Satatus = 0
            });
            if (db.SaveChanges() <= 0)
            {
                Alert("举报失败");
                return false;
            }
            Alert("举报成功", 1);
            return true;
        }


        #endregion

        #region MyRegion
        /// <summary>
        /// 是否有最新信友圈
        /// </summary>
        /// <returns></returns>
        public int IsNewMessage(Reqbase req)
        {
            VpxinMessage vpxinMessage = db.VpxinMessageSet.Where(a => a.Localnodeid == req.Nodeid && a.Msgnodeid != req.Nodeid).OrderByDescending(a => a.Createtime).FirstOrDefault();
            if (vpxinMessage == null)
            {
                Alert("暂无最新消息");
                return 0;
            }
            else
            {
                if (vpxinMessage.Status == 0)
                {
                    Alert("有最新消息", 1);
                    return vpxinMessage.Msgnodeid;
                }
                else
                {
                    Alert("暂无最新消息");
                    return 0;
                }
            }
        }
        #endregion

        #region ue支付返回参数
        private int _businessID = 0;
        /// <summary>
        /// 
        /// </summary>
        public ChargeDto ChargeUE { get; set; }
        #endregion
        /// <summary>
        /// 转账测试
        /// </summary>
        /// <returns></returns>
        public bool TransferTest()
        {
            BeginTransfer();
            Purse toPurse = purseFactory.UserCVPurse(3434909);
            Purse fromPurse = purseFactory.SystemPurseRand(3434909);
            Currency currency = new Currency(CurrencyType.RMB, 1);
            var result = Transfer(fromPurse, toPurse, currency, 123123, "转账测试");
            if (!result.IsSuccess)
            {
                EndTransfer(false);
            }
            EndTransfer(true);
            return result.IsSuccess;
        }


        #region Private_Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="type">类型 1=V点 2=P点 3=SVC</param>
        /// <param name="amount"></param>
        /// <param name="reason">变化原因(1=发布文章 2=查看文章 3=点赞文章 4=踩文章 5=聊天计费 6=充值)</param>
        /// <param name="transferId"></param>
        /// <param name="remarks"></param>
        /// <param name="amountBefore"></param>
        /// <param name="amountAfter"></param>
        /// <returns></returns>
        private TpxinAmountChangeHis CreateAmountChangeHis(int nodeId, int type, decimal amount, int reason, string transferId, string remarks, decimal amountBefore, decimal amountAfter)
        {
            return new TpxinAmountChangeHis()
            {
                Nodeid = nodeId,
                Typeid = type,
                Amount = amount,
                Reason = reason,
                Transferid = transferId,
                Createtime = DateTime.Now,
                Remarks = remarks,
                Amountbefore = amountBefore,
                Amountafter = amountAfter
            };
        }
        #endregion
    }
}
