using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.Models;
using PXin.DB;
using PXin.Facade.Models.Req;
using PXin.Facade.Models.Dto;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Mvc.HttpHelper;
using Newtonsoft.Json;
using System.Web.Caching;
using System.Web;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// A点竞拍
    /// </summary>
    public class AuctionAFacade : FacadeBase<PXinContext>
    {
        private static object obj = new object();
        private readonly Cache _objCache = HttpRuntime.Cache;
        /// <summary>
        /// 本月竞拍数据（我的竞拍总数和竞拍记录）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ThisMonthDataDto GetThisMonthData(Reqbase req)
        {
            var now = DateTime.Now;
            var config = GetConfigCahce();
            if (config == null)
            {
                Alert("本月没有竞拍");
                return null;
            }
            var myanum = GetMyADianCache(req.Nodeid);
            var his = db.TpxinPaiHisSet.Where(w => w.Nodeid == req.Nodeid && w.Configid == config.Configid).ToList();
            var leading = his.Where(s => s.Status == 0).Sum(e => e.Num);
            var outs = his.Where(w => w.Status == -1 || w.Status == -2).Sum(s => s.Num);

            var agree = GetProtocolCache(req.Nodeid);
            var data = new ThisMonthDataDto()
            {
                ANum = config.Num,
                AddPrice = config.Addprice,
                MyLeading = leading,
                MyOut = outs,
                MyA = myanum,
                MinPrice = config.Minprice,
                IsAgreement = agree,
            };
            #region 竞拍排名
            var list = GetAuctionRanking();
            data.AuctionHis = list;
            #endregion
            return data;
        }
        /// <summary>
        /// 竞拍排名
        /// </summary>
        /// <returns></returns>
        public List<AuctionHisDto> GetAuctionRanking()
        {
            var rankingCache = _objCache.Get("AuctionRanking");
            if (rankingCache == null)
            {
                var json = HttpSimulation.Instance.Request(AppConfig.PxinInternalServiceUrl + "/api/cache/GetAuctionRanking", null);
                var data = JsonConvert.DeserializeObject<Respbase<List<AuctionHisDto>>>(json);
                return data.Data ?? new List<AuctionHisDto>();
            }
            return JsonConvert.DeserializeObject<List<AuctionHisDto>>(rankingCache.ToString());
        }
        /// <summary>
        /// 缓存竞拍排名
        /// </summary>
        /// <param name="list"></param>
        public void CacheAuctionRanking(List<AuctionHisDto> list)
        {
            if (list?.Count > 0)
            {
                _objCache.Insert("AuctionRanking", JsonConvert.SerializeObject(list));
            }
        }
        /// <summary>
        /// a点竞拍支付
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool PayAuction(ReqPayAuction req)
        {
            lock (obj)
            {
                try
                {
                    db.BeginTransaction();
                    (bool result, TpxinPaiConfig config, TpxinUserinfo user) = PayCheck(req.Nodeid, req.PayPwd, req.Num, req.Num * req.MinPrice);
                    if (!result)
                    {
                        return false;
                    }
                    (bool payResult, int id) = PayNew(req, user, config);
                    if (payResult)
                    {
                        if (db.SaveChanges() <= 0)
                        {
                            log.Info("a点竞拍支付提交失败:" + db.Message);
                            db.Rollback();
                            Alert("提交失败");
                            return false;
                        }
                        if (!ComputeMinPrice(config))
                        {
                            return false;
                        }
                        var vpResult = AuctionVP(req.Nodeid, req.Num * req.MinPrice, "A点竞拍", id.ToString());
                        if (vpResult.Result <= 0)
                        {
                            Alert(vpResult.Message, vpResult.Result);
                            db.Rollback();
                            return false;
                        }
                        db.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    db.Rollback();
                    log.Error("A点竞拍支付异常：" + e);
                    Alert("支付失败");
                    return false;
                }
            }

        }
        /// <summary>
        /// 计算最低价
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private bool ComputeMinPrice(TpxinPaiConfig config)
        {
            var sqls = "select * from vpxin_pai_his";
            var lists = db.Database.SqlQuery<AuctionHisDto>(sqls).ToList();
            var orderList = lists.Where(w => w.Afternum <= config.Num).OrderBy(o => o.Price).ToList();
            var minprice = orderList.Select(s => s.Price).FirstOrDefault();
            var nums = orderList.Sum(s => s.Num);
            if (nums >= config.Num)
            {
                config.Minprice = minprice + config.Addprice;
                db.Entry(config).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() < 0)
                {
                    log.Info("a点竞拍支付修改失败:" + db.Message);
                    db.Rollback();
                    Alert("提交失败");
                    return false;
                }
            }
            //出局由服务计算

            //var prices= lists.Where(w => w.Afternum > config.Num).Select(s => s.Price).ToList();
            //if (prices.Count > 0)
            //{
            //    string sql = $"update TPXIN_PAI_HIS set status=-2 where status=0 and  price in ({string.Join(",", prices)})";
            //    db.ExecuteSqlCommand(sql);
            //}
            return true;
        }
        /// <summary>
        /// 支付检查
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="payPwd"></param>
        /// <param name="sumNum">总数量</param>
        /// <param name="amount">总价（数量*价格）</param>
        /// <returns></returns>
        private (bool, TpxinPaiConfig, TpxinUserinfo) PayCheck(int nodeid, string payPwd, int sumNum, decimal amount)
        {
            ExchangeFacade facade = new ExchangeFacade();
            TnetReginfo user = new TnetReginfo();
            if (!facade.CheckPwd(nodeid, payPwd, ref user))
            {
                Alert(facade.PromptInfo.Message);
                return (false, null, null);
            }
            var now = DateTime.Now;
            int monthDay = DateTime.DaysInMonth(now.Year, now.Month);
            var next = new DateTime(now.Year, now.Month, monthDay).AddHours(12);

            if (next <= now)
            {
                Alert("已超过截止日期，不能竞拍");
                return (false, null, null);
            }
            var config = GetConfig();
            if (config == null)
            {
                Alert("本月没有竞拍");
                return (false, null, null);
            }
            if (sumNum > config.Num)
            {
                Alert("竞拍数量大于总数量");
                return (false, null, null);
            }
            var pxinUser = db.TpxinUserinfoSet.Where(w => w.Nodeid == nodeid).FirstOrDefault();
            if (pxinUser == null)
            {
                Alert("不是相信app用户");
                return (false, null, null);
            }
            //if (pxinUser.P < amount)
            //{
            //    Alert("p点余额不足");
            //    return (false, null, null);
            //}
            return (true, config, pxinUser);
        }

        private (bool, int id) PayNew(ReqPayAuction req, TpxinUserinfo pxinUser, TpxinPaiConfig config)
        {
            var price = req.MinPrice;
            var amount = price * req.Num;
            var now = DateTime.Now;
            var sql = "select * from vpxin_pai_his";
            var list = db.Database.SqlQuery<AuctionHisDto>(sql).ToList();
            var nums = list.Sum(s => s.Num);
            //var minprice = list.Where(w => w.Afternum <= config.Num).OrderBy(o => o.Price).Select(s => s.Price).FirstOrDefault();
            var id = db.GetPrimaryKeyValue<TpxinPaiHis>();
            if (req.Num + nums <= config.Num)
            {
                //竞拍操作
                var paihis = new TpxinPaiHis
                {
                    Hisid = id,
                    Configid = config.Configid,
                    Createtime = now,
                    Nodeid = req.Nodeid,
                    Num = req.Num,
                    Price = price,
                    Totalprice = amount,
                    Rankinfo = null,
                    Status = 0,
                    Remarks = $"{price}p点",
                };
                db.TpxinPaiHisSet.Add(paihis);
            }
            else
            {
                var numeq = list.Where(w => w.Price >= price).Sum(s => s.Num);
                if (req.Num + numeq > config.Num)
                {
                    log.Info($"竞拍失败-大于最低价：购买数量超过剩余总数；购买数量：{req.Num};已竞拍数：{numeq}；当前价：{price}");
                    Alert("竞拍价格低于当前最低价");
                    return (false, 0);
                }
                //竞拍操作
                var paihis = new TpxinPaiHis
                {
                    Hisid = id,
                    Configid = config.Configid,
                    Createtime = now,
                    Nodeid = req.Nodeid,
                    Num = req.Num,
                    Price = price,
                    Totalprice = amount,
                    Rankinfo = null,
                    Status = 0,
                    Remarks = $"{price}p点",
                };
                db.TpxinPaiHisSet.Add(paihis);
            }
            return (true, id);
        }
        /// <summary>
        /// 我的竞拍历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<MyAuctionHisDto> GetMyAuctionHis(ReqMyAuctionHis req)
        {
            Helper.DateTimeRange(req.QueryDate, out var startDate, out var endDate);
            if (startDate.Month == DateTime.Now.Month)
            {
                var query = db.TpxinPaiHisSet.Where(w => w.Nodeid == req.Nodeid && w.Createtime >= startDate && w.Createtime <= endDate)
                    .Select(q => new MyAuctionHisDto
                    {
                        CreateTime = q.Createtime,
                        Id = q.Hisid,
                        Num = q.Num,
                        Price = q.Price,
                        Status = q.Status,
                    }).OrderByDescending(o => o.Status).ThenByDescending(o => o.CreateTime).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize);
                return query.ToList();
            }
            else
            {
                var query = db.TpxinPaiHisOldSet.Where(w => w.Nodeid == req.Nodeid && w.Createtime >= startDate && w.Createtime <= endDate)
                    .Select(q => new MyAuctionHisDto
                    {
                        CreateTime = q.Createtime,
                        Id = q.Hisid,
                        Num = q.Num,
                        Price = q.Price,
                        Status = q.Status,
                    }).OrderByDescending(o => o.Status).ThenByDescending(o => o.CreateTime).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize);
                return query.ToList();
            }

        }
        /// <summary>
        /// 竞拍加价页面数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public AuctionAddpriceDto GetAuctionAddprice(Reqbase req)
        {
            var config = GetConfig();
            if (config == null)
            {
                Alert("本月没有竞拍");
                return null;
            }
            var myAuctionHis = db.TpxinPaiHisSet.Where(w => w.Nodeid == req.Nodeid && (w.Status == 0 || w.Status == -2) && w.Configid == config.Configid).Select(q => new MyAuctionHisDto
            {
                CreateTime = q.Createtime,
                Id = q.Hisid,
                Num = q.Num,
                Price = q.Price,
                Status = q.Status,
            }).OrderByDescending(o => o.Status).ThenByDescending(o => o.CreateTime).ToList();
            return new AuctionAddpriceDto { Multiple = config.Multiple, myAuctionHis = myAuctionHis };
        }
        /// <summary>
        /// 竞拍加价支付
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool AuctionAddpricePay(ReqPayAuctionAddPrice req)
        {
            var auctionHis = db.TpxinPaiHisSet.Where(w => req.Auctionid.Contains(w.Hisid)).ToList();
            #region 验证
            if (auctionHis.Count != req.Auctionid.Count)
            {
                var hisids = req.Auctionid.Except(auctionHis.Select(s => s.Hisid)).ToList();
                log.Info($"AuctionAddpricePay参数错误,hisid不存在：{string.Join(",", hisids)}");
                Alert("参数错误");
                return false;
            }
            if (auctionHis.Any(a => (a.Status == -1 || a.Status == 1)))
            {
                Alert("此竞拍不可加价");
                return false;
            }
            #endregion
            try
            {
                lock (obj)
                {
                    db.BeginTransaction();
                    #region 释放P点
                    //var puser= db.TpxinUserinfoSet.FirstOrDefault(w => w.Nodeid == req.Nodeid);
                    //var freedP = auctionHis.Sum(s => s.Totalprice);
                    //puser.P += freedP;
                    //db.Entry(puser).State = System.Data.Entity.EntityState.Modified;
                    //db.TpxinAmountChangeHisSet.Add(new TpxinAmountChangeHis
                    //{
                    //    Amount = freedP,
                    //    Createtime = DateTime.Now,
                    //    Nodeid = req.Nodeid,
                    //    Reason = 2000,
                    //    Remarks = "A点竞拍加价退还",
                    //    Typeid = 2,
                    //    Transferid = Guid.NewGuid().ToString(),
                    //    Amountbefore = puser.P- freedP,
                    //    Amountafter = puser.P
                    //});
                    //if (db.SaveChanges() <= 0)
                    //{
                    //    log.Info("竞拍加价支付提交失败:" + db.Message);
                    //    db.Rollback();
                    //    Alert("提交失败");
                    //    return false;
                    //}
                    #endregion
                    //var amountP = auctionHis.Select(s => s.Num * (s.Price + req.Price)).Sum();
                    var sumNum = auctionHis.Sum(s => s.Num);
                    var amountP = sumNum * req.Price;
                    (bool result, TpxinPaiConfig config, TpxinUserinfo user) = PayCheck(req.Nodeid, req.PayPwd, sumNum, amountP);
                    if (!result)
                    {
                        db.Rollback();
                        return false;
                    }
                    var transferid = new List<int>();
                    foreach (var item in auctionHis)
                    {
                        var reqPay = new ReqPayAuction() { Num = item.Num, MinPrice = req.Price + item.Price, PayPwd = req.PayPwd, Nodeid = req.Nodeid };
                        (bool payResult, int id) = PayNew(reqPay, user, config);
                        if (!payResult)
                        {
                            db.Rollback();
                            return false;
                        }
                        transferid.Add(id);
                    }
                    var ids = string.Join(",", auctionHis.Select(s => s.Hisid).ToList());
                    var delteResult = db.ExecuteSqlCommand($"DELETE tpxin_pai_his WHERE hisid in ({ids})");
                    if (delteResult <= 0)
                    {
                        log.Info("竞拍加价支付删除失败,ids:" + ids);
                        Alert("竞拍加价支付失败");
                        db.Rollback();
                        return false;
                    }
                    if (db.SaveChanges() <= 0)
                    {
                        log.Info("竞拍加价支付提交失败:" + db.Message);
                        db.Rollback();
                        Alert("提交失败");
                        return false;
                    }
                    if (!ComputeMinPrice(config))
                    {
                        return false;
                    }
                    //调用接口支付p点
                    var vpResult = AuctionVP(req.Nodeid, amountP, "A点竞拍加价", string.Join(",", transferid));
                    if (vpResult.Result <= 0)
                    {
                        Alert(vpResult.Message, vpResult.Result);
                        db.Rollback();
                        return false;
                    }
                    db.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {
                db.Rollback();
                log.Error("A点竞拍加价支付异常：" + e);
                Alert("支付失败");
                return false;
            }
        }
        /// <summary>
        /// 我的A点
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<MyAuctionADto> GetMyAuctionA(Reqbase req)
        {
            //var result = from u in db.TpxinPaiUserSet
            //             join e in db.TbtcYdTransferHisExt2Set on u.Infoid equals e.Infoid
            //             where u.Nodeid==req.Nodeid
            //             orderby u.Createtime descending
            //             group new { u.Num, e.Status, e.Typeid, u.Fromtime, u.Endtime, e.Amount } by new {u.Infoid} into g
            //             select new MyAuctionADto
            //             {
            //                  Status=g.Where(s=>s.Status==1).Any()?1:0,
            //                  BeginDate=g.Min(m=>m.Fromtime),
            //                  EndDate=g.Min(m=>m.Endtime),
            //                  Num=g.Max(m=>m.Num),
            //                  SV=g.Where(w=>w.Typeid==1).Select(s=>s.Amount).DefaultIfEmpty().Sum(),
            //                  SVC= g.Where(w => w.Typeid == 2).Select(s => s.Amount).DefaultIfEmpty().Sum(),
            //                  DOS= g.Where(w => w.Typeid == 3).Select(s => s.Amount).DefaultIfEmpty().Sum(),
            //             };
            //var list = result.ToList();
            //return list;
            var result = (from u in db.TpxinPaiUserSet
                          join e in db.TbtcYdTransferHisExt2Set on u.Infoid equals e.Infoid
                          where u.Nodeid == req.Nodeid
                          orderby u.Createtime descending
                          select new
                          {
                              e.Infoid,
                              e.Status,
                              BeginDate = u.Fromtime,
                              EndDate = u.Endtime,
                              u.Num,
                              e.Amount,
                              e.Typeid,
                          }).ToList();
            var list = result.GroupBy(g => g.Infoid).Select(s => new MyAuctionADto
            {
                Status = s.Where(w => w.Status == 1).Any() ? 1 : 0,
                BeginDate = s.Min(m => m.BeginDate),
                EndDate = s.Min(m => m.EndDate),
                Num = s.Max(m => m.Num),
                SV = s.Where(w => w.Typeid == 1).Select(ss => ss.Amount).DefaultIfEmpty().Sum(),
                SVC = s.Where(w => w.Typeid == 2).Select(ss => ss.Amount).DefaultIfEmpty().Sum(),
                DOS = s.Where(w => w.Typeid == 3).Select(ss => ss.Amount).DefaultIfEmpty().Sum(),
            }).ToList();
            return list;
        }
        /// <summary>
        /// 获取竞拍配置(竞拍页面刷新获取实时的竞拍配置)
        /// </summary>
        /// <returns></returns>
        public MyTpxinPaiConfig GetAuctionConfig(int nodeid)
        {
            var now = DateTime.Now;
            var config = GetConfig();
            if (config == null)
            {
                return null;
            }
            var vp = new VPHelper();
            var vpDian = vp.GetTpxinUserinfo(nodeid);
            return new MyTpxinPaiConfig
            {
                Addprice = config.Addprice,
                Minprice = config.Minprice,
                MyP = vpDian.PDianBalance,
                Multiple = config.Multiple,
            };
        }
        /// <summary>
        /// 获取竞拍详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<AuctionDetailsDto> GetAuctionDetails(ReqAuctionDetails req)
        {
            DateTime beginDate = DateTime.MinValue, endDate = DateTime.MinValue;
            var now = DateTime.Now;
            switch (req.QueryTimeType)
            {
                case 1:
                    if (now.Day > 7)
                    {
                        beginDate = now.AddDays(-6).Date;
                    }
                    else
                    {
                        beginDate = now.AddDays(1 - now.Day).Date;
                    }
                    endDate = now.AddDays(1).Date.AddSeconds(-1);
                    break;
                case 2:
                    if (now.Day <= 7)
                    {
                        Alert("没有7天前的数据");
                        return null;
                    }
                    beginDate = now.AddDays(1 - now.Day).Date;
                    endDate = now.AddDays(-6).Date.AddSeconds(-1);
                    break;
                case 3:
                    beginDate = now.AddDays(1 - now.Day).Date;
                    endDate = now.AddDays(1 - now.Day).Date.AddMonths(1).AddSeconds(-1);
                    break;
                default:
                    Alert("参数错误");
                    return null;
            }
            string sql = $@"select TO_CHAR(createtime,'MM-dd')as createdate,PRICE,sum(num)as num from tpxin_pai_his
                            WHERE  createtime>= TO_DATE('{beginDate}', 'yyyy-MM-dd HH24:mi:ss') and createtime<= TO_DATE('{endDate}', 'yyyy-MM-dd HH24:mi:ss')
                            GROUP BY PRICE,TO_CHAR(createtime, 'MM-dd') ORDER BY TO_CHAR(createtime, 'MM-dd') ";
            return db.Database.SqlQuery<AuctionDetailsDto>(sql).ToList();
        }
        /// <summary>
        /// 获取竞拍配置
        /// </summary>
        /// <returns></returns>
        private TpxinPaiConfig GetConfig()
        {
            var now = DateTime.Now;
            var preMonthStart = now.AddDays(1 - now.Day).Date;
            var preMonthEnd = now.AddDays(1 - now.Day).Date.AddMonths(1).AddSeconds(-1);
            return db.TpxinPaiConfigSet.Where(w => w.Month >= preMonthStart && w.Month <= preMonthEnd).FirstOrDefault();
        }
        private TpxinPaiConfig GetConfigCahce()
        {
            var cacheConfig = _objCache.Get("AuctionConfig");
            if (cacheConfig == null)
            {
                var config = GetConfig();
                var endTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
                _objCache.Insert("AuctionConfig", JsonConvert.SerializeObject(config), null, endTime, Cache.NoSlidingExpiration);
                return config;
            }
            return JsonConvert.DeserializeObject<TpxinPaiConfig>(cacheConfig.ToString());
        }
        private int GetMyADianCache(int nodeid)
        {
            var key = $"AuctionMyADian_{nodeid}";
            var myAdain = _objCache.Get(key);
            if (myAdain == null)
            {
                var myanum = db.TpxinPaiUserSet.Where(w => w.Nodeid == nodeid).Select(s => s.Num).DefaultIfEmpty().Sum();
                var now = DateTime.Now;
                var endTime = now.AddDays(1 - now.Day).Date.AddMonths(1).AddSeconds(-1);
                var timeCache = DateTime.Parse(endTime.ToString("yyyy-MM-dd") + " 12:00:00");
                //当月最后一天12:00-23:59:59 不缓存
                if (!(now > timeCache && now < endTime))
                {
                    _objCache.Insert(key, myanum, null, timeCache, Cache.NoSlidingExpiration);
                }
                return myanum;
            }
            int mya = 0;
            int.TryParse(myAdain.ToString(), out mya);
            return mya;
        }
        private int GetProtocolCache(int nodeid)
        {
            var key = $"AuctionProtocol_{nodeid}";
            var protocol = _objCache.Get(key);
            if (protocol == null)
            {
                var agree = db.TnetUserAgreementSet.Where(c => c.Nodeid == nodeid && c.Type == 20003 && c.Version == 2).FirstOrDefault() == null ? 0 : 1;
                _objCache.Insert(key, agree);
                return agree;
            }
            int myprotocol = 0;
            int.TryParse(protocol.ToString(), out myprotocol);
            return myprotocol;
        }
        /// <summary>
        /// 调用Vp接口
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amount"></param>
        /// <param name="remark"></param>
        /// <param name="transferid"></param>
        /// <returns></returns>
        private VPRespbase AuctionVP(int nodeid, decimal amount, string remark, string transferid)
        {
            var vp = new VPHelper();
            var result = vp.SetP(new VPAuction
            {
                Nodeid = nodeid,
                Amount = -amount,
                Reason = 2000,
                Remark = remark,
                Transferid = transferid,
            });
            return result;
        }
    }
}
