using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Common.Facade;
using Common.Facade.Models;
using Common.UEPay;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Enum;
using PXin.Facade.Models.Req;
using PXin.Model;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;
using Winner.EncodeDecode;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 领取红包逻辑
    /// </summary>
    public class RedpacketFacade : FacadeBase<PXinContext>
    {
        #region PublicMethods
        /// <summary>
        /// 获取领取红包页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<RedPacketInfoDto> GetRedPacketInfo(RedPacketInfoReq req)
        {
            var resultDto = new RedPacketInfoDto();

            var dt = DateTime.Now;
            ////当月开始时间 00:00:00
            //var preMonthStart = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            ////当月结束时间 23:59:59
            //var preMonthEnd = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
            //var counter = await db.TblcCentcardHisSet.CountAsync(his => his.Nodeid == req.Nodeid && his.Createtime >= preMonthStart && his.Createtime <= preMonthEnd && (his.Typeid == 0 || his.Typeid == 1));
            //resultDto.IsCompleteTask = counter > 0;//本月完成购买充值卡任务
            resultDto.IsCompleteTask = PXinContext.IsFinishTask(db, req.Nodeid, 1);
            resultDto.IsCompleteTask1 = PXinContext.IsFinishTask(db, req.Nodeid, 0);

            var open = await db.TnetOpenInfoSet.FirstOrDefaultAsync(w => dt >= w.Fromtime && dt <= w.Endtime && w.Typeid == 20001 && w.Nodeid == req.Nodeid);
            resultDto.IsOpen = open == null ? 0 : 1;

            var user = db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault(); //PxinCache.GetRegInfo(req.Nodeid);// HttpContext.Current.GetRegInfo();
            resultDto.Name = user?.Nodename;
            resultDto.NodeCode = user?.Nodecode;

            var redPacket = await db.TbtcYdTransferHisSet.FirstOrDefaultAsync(h => h.NodeId == req.Nodeid && h.BeginTime <= dt && h.EndTime > dt);
            resultDto.Status = redPacket == null ? 2 : redPacket.Status;//红包状态
            resultDto.InfoId = redPacket == null ? 0 : redPacket.HisId;

            var pxinUser = await db.TpxinUserinfoSet.FirstOrDefaultAsync(u => u.Nodeid == req.Nodeid);
            resultDto.ADain = pxinUser == null ? 0 : pxinUser.Apoint;//当前用户通过竞拍所得A点

            //log.Info($"GetRedPacketInfo:nodeid={req.Nodeid}&counter={counter}&isopen={resultDto.IsOpen}&Status={resultDto.Status}&ADain={resultDto.ADain}");
            //resultDto.Status = (resultDto.ADain > 0 && resultDto.IsCompleteTask) ? resultDto.Status : 2;

            resultDto.Status = resultDto.ADain > 0 ? resultDto.Status : 2;

            if (redPacket == null)
            {
                resultDto.SV = 0;
                resultDto.SVC = 0;
                resultDto.DOS = 0;
            }
            else
            {
                var redPackets = await (from t in db.TbtcYdTransferHisSet.Where(h => h.NodeId == req.Nodeid)
                                        join e in db.TbtcYdTransferHisExt2Set on t.HisId equals e.Hisid into temp_transfer
                                        from transferHis in temp_transfer.DefaultIfEmpty()
                                        where t.Status == 1
                                        select new
                                        {
                                            TypeId = transferHis == null ? default : transferHis.Typeid,
                                            Amount = transferHis == null ? default : transferHis.Amount,
                                            ReceStatus = t.Status,
                                            SettStatus = transferHis == null ? default : transferHis.Status,
                                        }).ToListAsync();
                //var redPacketHis = await db.TbtcYdTransferHisExt2Set.Where(p => p.Hisid == redPacket.HisId).ToListAsync();
                resultDto.SV = redPackets.Count == 0 ? 0 : redPackets.Where(p => p.TypeId == 1).Sum(sv => sv.Amount);
                resultDto.SVC = redPackets.Count == 0 ? 0 : redPackets.Where(p => p.TypeId == 2).Sum(sv => sv.Amount);
                resultDto.DOS = redPackets.Count == 0 ? 0 : redPackets.Where(p => p.TypeId == 3).Sum(sv => sv.Amount);
            }
            return resultDto;
        }
        /// <summary>
        /// 领取红包
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ReceiveRedPacketDto> ReceiveRedPacket(ReceiveRedPacketReq req)
        {
            var dt = DateTime.Now;
            var redPacket = await db.TbtcYdTransferHisSet.FirstOrDefaultAsync(h => h.HisId == req.InfoId && h.BeginTime <= dt && h.EndTime > dt);
            if (redPacket == null)
            {
                Alert("红包信息不存在!");
                return null;
            }
            if (redPacket.Status == 1)
            {
                Alert("今日红包已领取!");
                return null;
            }
            if (redPacket.Status == -1)
            {
                Alert("您不能领取红包!");
                return null;
            }
            ////当月开始时间 00:00:00
            //var preMonthStart = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).Date;
            ////当月结束时间 23:59:59
            //var preMonthEnd = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
            //var counter = await db.TblcCentcardHisSet.AsNoTracking().CountAsync(his => his.Nodeid == req.Nodeid && his.Createtime >= preMonthStart && his.Createtime <= preMonthEnd);
            var isCompleteTask = PXinContext.IsFinishTask(db, req.Nodeid, 0);
            //获取A点
            var pxinUser = await db.TpxinUserinfoSet.FirstOrDefaultAsync(u => u.Nodeid == req.Nodeid);
            if (!isCompleteTask || pxinUser.Apoint < 0)
            {
                Alert("您没有权限领取红包!");
                return null;
            }
            redPacket.Status = 1;//修改为已领取状态
            var resultDto = new ReceiveRedPacketDto();
            var redPacketHis = await db.TbtcYdTransferHisExt2Set.AsNoTracking().Where(p => p.Hisid == redPacket.HisId).ToListAsync();
            if (await db.SaveChangesAsync() > 0)
            {
                resultDto.SV = redPacketHis.Count == 0 ? 0 : redPacketHis.Where(p => p.Typeid == 1).Sum(sv => sv.Amount);
                resultDto.SVC = redPacketHis.Count == 0 ? 0 : redPacketHis.Where(p => p.Typeid == 2).Sum(sv => sv.Amount);
                resultDto.DOS = redPacketHis.Count == 0 ? 0 : redPacketHis.Where(p => p.Typeid == 3).Sum(sv => sv.Amount);
            }
            return resultDto;
        }
        /// <summary>
        /// 获取我的红包奖励
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<MyRedPacketDto> GetMyRedPacket(MyRedPacketReq req)
        {
            var redPackets = await (from t in db.TbtcYdTransferHisSet.Where(h => h.NodeId == req.Nodeid)
                                    join e in db.TbtcYdTransferHisExt2Set on t.HisId equals e.Hisid into temp_transfer
                                    from transferHis in temp_transfer.DefaultIfEmpty()
                                    select new
                                    {
                                        TypeId = transferHis == null ? default : transferHis.Typeid,
                                        Amount = transferHis == null ? default : transferHis.Amount,
                                        ReceStatus = t.Status,
                                        SettStatus = transferHis == null ? default : transferHis.Status,
                                        Hisid = t.HisId
                                    }).ToListAsync();
            var resultDto = new MyRedPacketDto();
            resultDto.SVC = (await db.TpxinUserinfoSet.FirstOrDefaultAsync(u => u.Nodeid == req.Nodeid)).Svc;
            resultDto.SettleAmounts.AddRange(
                new List<ReceiveAmount> {
                    new ReceiveAmount {Status=RedpacketReceiveStatus.Settled,
                        SV = redPackets.Where(p => p.TypeId == 1 && p.SettStatus == 1).Sum(sv => sv.Amount),
                        SVC = redPackets.Where(p => p.TypeId == 2 && p.SettStatus == 1).Sum(sv => sv.Amount),
                        DOS = redPackets.Where(p => p.TypeId == 3 && p.SettStatus == 1).Sum(sv => sv.Amount)},
                    new ReceiveAmount {Status=RedpacketReceiveStatus.Invalid,
                        SV = redPackets.Where(p => p.TypeId == 1 && p.SettStatus == -1).Sum(sv => sv.Amount),
                        SVC = redPackets.Where(p => p.TypeId == 2 && p.SettStatus == -1).Sum(sv => sv.Amount),
                        DOS = redPackets.Where(p => p.TypeId == 3 && p.SettStatus == -1).Sum(sv => sv.Amount)},
                });
            var result = from t in db.TbtcYdTransferHisSet
                         where t.NodeId == req.Nodeid && t.Sid == 81127
                         orderby t.Status descending, t.HisId descending
                         select new
                         {
                             Status = t.Status,
                             HisId = t.HisId,
                             createtime = t.BeginTime
                         };
            result.ToList().ForEach((p) =>
            {
                resultDto.ReceiveAmounts.Add(new ReceiveAmount
                {
                    Status = p.Status == 0 ? RedpacketReceiveStatus.UnReceived : RedpacketReceiveStatus.Received,
                    SV = redPackets.Where(c => c.Hisid == p.HisId && c.TypeId == 1).Sum(sv => sv.Amount),
                    SVC = redPackets.Where(c => c.TypeId == 2 && c.Hisid == p.HisId).Sum(sv => sv.Amount),
                    DOS = redPackets.Where(c => c.TypeId == 3 && c.Hisid == p.HisId).Sum(sv => sv.Amount),
                    Hisid = p.HisId,
                    Time = p.createtime
                });
            });

            return resultDto;
        }
        /// <summary>
        /// 红包奖励领取详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<MyRedPacketDetailDto> GetMyRedPacketDetail(MyRedPacketDetailReq req)
        {
            var redPackets = await (from t in db.TbtcYdTransferHisSet.Where(h => h.NodeId == req.Nodeid)
                                    join e in db.TbtcYdTransferHisExt2Set on t.HisId equals e.Hisid into temp_transfer
                                    from transferHis in temp_transfer.DefaultIfEmpty()
                                    where t.HisId == req.HisId
                                    select new
                                    {
                                        InfoId = transferHis == null ? default : transferHis.Infoid,
                                        TypeId = transferHis == null ? default : transferHis.Typeid,
                                        Amount = transferHis == null ? default : transferHis.Amount,
                                        ReceStatus = t.Status,
                                        SettStatus = transferHis == null ? default : transferHis.Status,
                                        ADian = transferHis == null ? default : transferHis.Num
                                    }).ToListAsync();
            var resultDto = new MyRedPacketDetailDto();
            resultDto.ADian = (await db.TpxinUserinfoSet.FirstOrDefaultAsync(u => u.Nodeid == req.Nodeid)).Apoint;
            resultDto.ReceiveAmount = new ReceiveAmount
            {
                Status = redPackets[0].ReceStatus == 1 ? RedpacketReceiveStatus.Received : RedpacketReceiveStatus.UnReceived,
                SV = redPackets.Where(p => p.TypeId == 1).Sum(sv => sv.Amount),
                SVC = redPackets.Where(p => p.TypeId == 2).Sum(sv => sv.Amount),
                DOS = redPackets.Where(p => p.TypeId == 3).Sum(sv => sv.Amount)
            };
            redPackets.OrderByDescending(c => c.InfoId).ToList().ForEach(his =>
              {
                  if (his.InfoId > 0)
                  {
                      var detail = resultDto.ReceiveAmountDetails.FirstOrDefault(p => p.InfoId == his.InfoId);
                      if (detail == null)
                      {
                          detail = new ReceiveAmountDetail { InfoId = his.InfoId };
                          resultDto.ReceiveAmountDetails.Add(detail);
                      }
                      if (his.TypeId == 1)
                      {
                          detail.SV = his.Amount;
                      }
                      else if (his.TypeId == 2)
                      {
                          detail.SVC = his.Amount;
                      }
                      else if (his.TypeId == 3)
                      {
                          detail.DOS = his.Amount;
                      }
                      detail.ADian += his.ADian;
                      detail.StatusDesc = his.SettStatus == 0 ? "待结算" : (his.SettStatus == 1 ? "已结算" : "已失效");
                  }
              });
            return resultDto;
        }
        /// <summary>
        /// 获取兑换页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ExchangeInfoDto> GetExchangeInfo(ExchangeInfoReq req)
        {
            var centCardConfigs = await db.TblcCentcardConfigSet.ToListAsync();
            var user = await db.TpxinUserinfoSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            var rate = await db.TblUserJxsSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefaultAsync();
            return new ExchangeInfoDto
            {
                SVC = (decimal)user?.Svc,
                Specs = centCardConfigs.Select(p => new Models.Dto.Specs { InfoId = p.Configid, ShowName = p.Showname }),
                Rate = rate?.Typeid > 0 ? rate?.Typeid <= 4 ? 1 : 2 : 3,
            };
        }
        /// <summary>
        /// 兑换
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> Exchange(ExchangeReq req)
        {
            if (!CheckPwd(req.Password))
            {
                Alert("支付密码不正确!");
                return false;
            }
            if (req.ExchangeType == ExchangeType.SVC)
            {
                return await SvcExchange(req);
            }
            else if (req.ExchangeType == ExchangeType.SV)
            {
                return await SvExchange(req);
            }
            else
            {
                Alert("参数不正确");
                return false;
            }
        }
        #endregion
        /// <summary>
        /// 获取有户参与A点竟拍抽奖信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Respbase<LuckDrawInfo>> GetLuckDrawInfo(Reqbase req)
        {
            DateTime now = DateTime.Now;
            LuckDrawInfo dto = new LuckDrawInfo();
            var user = db.TpxinUserinfoSet.FirstOrDefault(c => c.Nodeid == req.Nodeid);
            if (user == null)
            {
                return new Respbase<LuckDrawInfo> { Data = dto, Message = "您没有抽奖资格，只有每月竞拍结束后在前100名的用户才可以A点抽奖。", Result = 0 };
            }
            dto.A = user.Apoint;
            var luckDraw = await db.TpxinLuckDrawSet.Where(c => c.Nodeid == req.Nodeid).OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            if (luckDraw == null)
            {
                return new Respbase<LuckDrawInfo> { Data = dto, Message = "您没有抽奖资格，只有每月竞拍结束后在前100名的用户才可以A点抽奖。", Result = 0 };
            }
            else if (luckDraw.Num - luckDraw.Usednum <= 0)
            {
                dto.Status = -1;
                return new Respbase<LuckDrawInfo> { Data = dto, Message = "抽奖次数已用完", Result = -1 };
            }
            else if (luckDraw.Starttime > now || luckDraw.Endtime < now)
            {
                dto.Status = -2;
                return new Respbase<LuckDrawInfo> { Data = dto, Message = "A点抽奖时间为每月最后1天的14:00到24:00", Result = -2 };
            }
            else
            {
                dto.Num = luckDraw.Num - luckDraw.Usednum;
                dto.Status = 1;
                return new Respbase<LuckDrawInfo> { Data = dto, Result = 1 };
            }
        }
        /// <summary>
        /// 有户参与A点竟拍抽奖信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<LuckDrawInfo> LuckDraw(Reqbase req)
        {
            lock (string.Intern(req.Nodeid.ToString()))
            {
                LuckDrawInfo dto = new LuckDrawInfo();
                DateTime now = DateTime.Now;
                var user = db.TpxinUserinfoSet.FirstOrDefault(c => c.Nodeid == req.Nodeid);
                if (user == null)
                {
                    return new Respbase<LuckDrawInfo> { Data = dto, Message = "您没有抽奖资格，只有每月竞拍结束后在前100名的用户才可以A点抽奖。", Result = 0 };
                }
                dto.A = user.Apoint;
                var luckDraw = db.TpxinLuckDrawSet.Where(c => c.Nodeid == req.Nodeid).OrderByDescending(c=>c.Id).FirstOrDefault();
                if (luckDraw == null)
                {
                    return new Respbase<LuckDrawInfo> { Data = dto, Message = "您没有抽奖资格，只有每月竞拍结束后在前100名的用户才可以A点抽奖。", Result = 0 };
                }
                else if (luckDraw.Num - luckDraw.Usednum <= 0)
                {
                    dto.Status = -1;
                    return new Respbase<LuckDrawInfo> { Data = dto, Message = "抽奖次数已用完", Result = -1 };
                }
                else if (luckDraw.Starttime > now || luckDraw.Endtime < now)
                {
                    dto.Status = -2;
                    return new Respbase<LuckDrawInfo> { Data = dto, Message = "A点抽奖时间为每月最后1天的14:00到24:00", Result = -2 };
                }
                else
                {
                    TpxinLuckDrawDetail detail = db.TpxinLuckDrawDetailSet.FirstOrDefault(c => c.Nodeid == req.Nodeid && c.Fromtime <= now && c.Endtime >= now && c.Status == 0);
                    if (detail == null)
                    {
                        dto.Status = -1;
                        return new Respbase<LuckDrawInfo> { Data = dto, Message = "抽奖次数已用完", Result = -1 };
                    }
                    detail.Status = 1;

                    luckDraw.Usednum += 1;
                    var amountBefore = user.Apoint;
                    user.Apoint += detail.Num;

                    db.TpxinAmountChangeHisSet.Add(new TpxinAmountChangeHis
                    {
                        Amount = detail.Num,
                        Nodeid = req.Nodeid,
                        Reason = 4000,
                        Remarks = "A点竟拍抽奖",
                        Transferid = Guid.NewGuid().ToString(),
                        Typeid = 4,
                        Createtime = DateTime.Now,
                        Amountbefore = amountBefore,
                        Amountafter = user.Apoint,
                    });
                    if (db.SaveChanges() <= 1)
                    {
                        log.Info("抽奖失败:" + db.Message);
                        return new Respbase<LuckDrawInfo> { Data = dto, Message = "抽奖失败", Result = -3 };
                    }

                    dto.A = user.Apoint;
                    dto.Num = luckDraw.Num - luckDraw.Usednum;
                    dto.Amount = detail.Num;
                    dto.Status = 1;
                    return new Respbase<LuckDrawInfo> { Data = dto, Result = 1 };
                }
            }
        }
        /// <summary>
        /// A点竟拍抽奖历名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<LuckDrawHis> GetLuckDrawHis(Reqbase req)
        {
            return db.TpxinAmountChangeHisSet
                .Where(c => c.Nodeid == req.Nodeid && c.Typeid == 4 && c.Reason == 4000).OrderByDescending(c => c.Createtime)
                .Select(c =>
                       new LuckDrawHis
                       {
                           Createtime = c.Createtime,
                           BalanceAfter=c.Amountafter,
                           Amount = c.Amount
                       }).ToList();
        }
        #region PrivateMethods
        /// <summary>
        /// 兑换svc充值码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<bool> SvcExchange(ExchangeReq req)
        {
            if (req.Specs.Count <= 0)
            {
                Alert("请最少选择一张充值张兑换!");
                return false;
            }
            var infoIds = req.Specs.Select(s => s.InfoId);
            var configs = await db.TblcCentcardConfigSet
                                         .Where(p => infoIds.Contains(p.Configid))
                                         .ToListAsync();
            var centCardConfigs = configs.Select(c => new
            {
                Amount = c.Price,
                Num = req.Specs.FirstOrDefault(s => s.InfoId == c.Configid).Num
            });
            if (centCardConfigs.Count() == 0)
            {
                Alert("对不起,目前找不到您要兑换的充值卡额度!");
                return false;
            }
            db.BeginTransaction();
            var user = await db.TpxinUserinfoSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            var amount = centCardConfigs.Sum(c => c.Amount * c.Num);
            if (user.Svc < amount)
            {
                db.Rollback();
                Alert("可兑换数量不足!");
                return false;
            }
            var amountBefore = user.Svc;
            user.Svc -= amount;
            AddAmountChangeHis(req.Nodeid, 3, amount, 3001, Guid.NewGuid().ToString(), "红包SVC兑换SVC充值码", amountBefore, user.Svc);
            AddCentCard(centCardConfigs, req.Nodeid);

            if (db.SaveChanges() <= 0)
            {
                db.Rollback();
                return false;
            }

            var payConfig = GetPayConfig();
            if (payConfig == null)
            {
                db.Rollback();
                return false;
            }
            //从ue扣除dos
            var userinfo = HttpContext.Current.GetRegInfo();
            var rate = await db.TblUserJxsSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefaultAsync();
            decimal syf = (decimal)(rate.Typeid > 0 ? rate.Typeid <= 4 ? 0.01 : 0.02 : 0.03) * amount;
            var result = UeApi.Recovery(payConfig.Accesskeyid, payConfig.Accesssecret, userinfo.Nodecode, 6, 1, syf, 8, "兑换SVC充值码扣除手续费", 10072);
            if (result == null || result.Result <= 0)
            {
                //失败
                db.Rollback();
                var error = "兑换SVC充值码扣除手续费：" + result?.Message;
                log.Info(error);
                Alert(error);
                return false;
            }

            db.Commit();
            return true;
        }
        /// <summary>
        /// 新增金额变化历史
        /// </summary>
        /// <param name="nodeId">用户Id</param>
        /// <param name="typeId">变化金额类型</param>
        /// <param name="amount">变化金额</param>
        /// <param name="reason">变化原因</param>
        /// <param name="transferId">转帐ID</param>
        /// <param name="remarks">描述</param>
        /// <param name="amountBefore">变化前金额</param>
        /// <param name="amountAfter">变化后金额</param>
        private void AddAmountChangeHis(int nodeId, int typeId, decimal amount, int reason, string transferId, string remarks, decimal amountBefore, decimal amountAfter)
        {
            //svc变化历史
            var amountChangeHis = new TpxinAmountChangeHis();
            amountChangeHis.Nodeid = nodeId;
            amountChangeHis.Typeid = typeId;
            amountChangeHis.Amount = amount;
            amountChangeHis.Reason = reason;
            amountChangeHis.Transferid = transferId;
            amountChangeHis.Createtime = DateTime.Now;
            amountChangeHis.Remarks = remarks;
            amountChangeHis.Amountbefore = amountBefore;
            amountChangeHis.Amountafter = amountAfter;
            db.TpxinAmountChangeHisSet.Add(amountChangeHis);
        }
        /// <summary>
        /// 新增充值卡
        /// </summary>
        /// <param name="centCardConfigs"></param>
        /// <param name="nodeId"></param>
        private void AddCentCard(dynamic centCardConfigs, int nodeId)
        {
            //生成SVC充值卡
            foreach (var centCardConfig in centCardConfigs)
            {
                for (int i = 0; i < centCardConfig.Num; i++)
                {
                    var cardId = db.GetPrimaryKeyValue<TblcCentcard>();
                    db.TblcCentcardSet.Add(new TblcCentcard
                    {
                        Idno = cardId,
                        Cardno = PXinContext.GetCentcard(db),
                        Cardpwd = "0",
                        Ispwdrequired = 0,
                        Amount = centCardConfig.Amount,
                        Expiredtime = DateTime.Now.AddYears(1),
                        Createdtime = DateTime.Now,
                        Areaid = "1",
                        Status = 0,
                        Usenodeid = nodeId,
                        Remarks = "红包SVC兑换SVC充值码",
                        Fromid = 2,
                    });
                    db.TblcCentcardHisSet.Add(new TblcCentcardHis
                    {
                        Idno = cardId,
                        Createtime = DateTime.Now,
                        Nodeid = nodeId,
                        Note = "红包SVC兑换",
                        Opnodeid = nodeId,
                        Typeid = 2,
                        Remarks = "新增SVC充值码"
                    });
                }
            }
        }
        /// <summary>
        /// 兑换sv
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<bool> SvExchange(ExchangeReq req)
        {
            var user = await db.TpxinUserinfoSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            if (user.Svc < req.Num)
            {
                Alert("可兑换数量不足!");
                return false;
            }
            var amountBefore = user.Svc;
            user.Svc -= req.Num;
            AddAmountChangeHis(req.Nodeid, 3, req.Num, 3000, Guid.NewGuid().ToString(), "红包SVC兑换SV", amountBefore, user.Svc);
            //svc变化历史
            BeginTransfer();
            Purse fromPurse = purseFactory.SystemPurseRand(req.Nodeid);
            Purse toPurse = purseFactory.UserCVPurse(req.Nodeid);
            var currency = new Currency(CurrencyType.RMB, req.Num);
            var transferResult = Transfer(fromPurse, toPurse, currency, 8, "红包SVC兑换SV");
            if (!transferResult.IsSuccess)
            {
                EndTransfer(false);
                Alert("红包SVC兑换SV失败!");
                return false;
            }
            if (await db.SaveChangesAsync() <= 0)
            {
                EndTransfer(false);
                Alert("红包SVC兑换SV失败!");
                return false;
            }
            EndTransfer();
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
        /// 获取支付配置
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        private TpcnUepayconfig GetPayConfig(int payType = 2)
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
        #endregion
    }
}
