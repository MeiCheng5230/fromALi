using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Common.UEPay;
using MvcPaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winner.CU.Balance.GlobalCurrency;
using static PXin.Facade.CommonService.ExpressAPI;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityFacade : FacadeBase<PXinContext>
    {
        //private DateTime beginTime = new DateTime(2019, 10, 21);
        //private DateTime endTime = new DateTime(2019, 11, 5, 23, 59, 59);

        /// <summary>
        /// 获取十月送手机活动的领取手机和支付服务费的数量
        /// </summary>
        public OctoberActivityCountDto GetOctoberActivityCount(OctoberActivityCountReq req)
        {
            var activity = db.TpxinActivitySet.FirstOrDefault(p => p.Id == req.ActivityId);
            if (activity == null)
            {
                Alert("找不到活动信息,请重试");
                return null;
            }
            OctoberActivityCountDto dto = new OctoberActivityCountDto();
            dto.PayCount = db.TpxinOctoberActivitySet.Count(x => x.Pnodeid == req.Nodeid && x.ActivityId == req.ActivityId);
            dto.ReceiveCount = db.TpxinOctoberActivitySet.Count(x => x.Nodeid == req.Nodeid && x.ActivityId == req.ActivityId);
            return dto;
        }

        /// <summary>
        /// 支付服务费的列表
        /// </summary>
        public List<OctoberActivityPayDto> GetPayList(OctoberActivityListReq req)
        {
            var activity = db.TpxinActivitySet.FirstOrDefault(p => p.Id == req.ActivityId);
            if (activity == null)
            {
                Alert("找不到活动信息,请重试");
                return null;
            }
            var list = (from x in db.TpxinOctoberActivitySet.Where(x => x.Pnodeid == req.Nodeid && x.ActivityId == req.ActivityId)
                        join b in db.TnetReginfoSet on x.Nodeid equals b.Nodeid
                        select new OctoberActivityPayDto
                        {
                            Amount = x.Pamount,
                            Nodeid = x.Nodeid,
                            Expressno = x.Expressno,
                            Id = x.Id,
                            Nodecode = b.Nodecode,
                            Nodename = b.Nodename,
                            Note = x.Pnote,
                            MyStatus = x.Status,
                            Typeid = x.Typeid,
                            CreateTime = x.Createtime,
                            Ptransferids = x.Ptransferids,
                            Transferids = x.Transferids
                        }).ToList();
            list.ForEach(x =>
            {
                x.PayStatus = string.IsNullOrWhiteSpace(x.Transferids) ? 0 : 1;
                if (x.MyStatus == 1)
                {
                    x.MyStatus = string.IsNullOrWhiteSpace(x.Ptransferids) ? 0 : 1;
                }
                if (x.MyStatus == 2)
                {
                    x.MyStatus = 1;
                }
            });
            DateTime now = DateTime.Now;
            if (now >= activity.PayStarttime && now <= activity.PayEndtime)
            {
                list = list.OrderBy(x => x.MyStatus).ToList();
            }
            return list;
        }

        /// <summary>
        /// 领取手机资格的列表
        /// </summary>
        public List<OctoberActivityPayDto> GetReceiveList(OctoberActivityListReq req)
        {
            var list = (from x in db.TpxinOctoberActivitySet.Where(x => x.Nodeid == req.Nodeid && x.ActivityId == req.ActivityId)
                        join b in db.TnetReginfoSet on x.Pnodeid equals b.Nodeid
                        select new OctoberActivityPayDto
                        {
                            Amount = x.Amount,
                            Nodeid = x.Pnodeid,
                            Expressno = x.Expressno,
                            Id = x.Id,
                            Nodecode = b.Nodecode,
                            Nodename = b.Nodename,
                            Note = x.Note,
                            MyStatus = x.Status,
                            Typeid = x.Typeid,
                            CreateTime = x.Createtime,
                            Ptransferids = x.Ptransferids,
                            Transferids = x.Transferids,
                        }).ToList();
            list.ForEach(x =>
            {
                x.PayStatus = string.IsNullOrWhiteSpace(x.Ptransferids) ? 0 : 1;
                if (x.MyStatus == 1)
                {
                    x.MyStatus = string.IsNullOrWhiteSpace(x.Transferids) ? 0 : 1;
                }
                if (x.MyStatus == 2)
                {
                    x.MyStatus = 1;
                }
            });
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public UePayCallDto UEPayCallDto { get; set; }

        /// <summary>
        /// 调用ue支付
        /// </summary>
        public bool OctoberActivityDosUEPrepare(OctoberActivityDosUEPrepareReq req)
        {
            log.Info($"十月送手机活动支付服务费,nodeid={req.Nodeid},服务费={req.Price}");
            var activity = db.TpxinActivitySet.FirstOrDefault(p => p.Id == req.ActivityId);
            if (activity == null)
            {
                Alert("找不到活动信息,请重试");
                return false;
            }
            DateTime now = DateTime.Now;
            if (AppConfig.IsUseSms && !(now >= activity.PayStarttime && now <= activity.PayEndtime))
            {
                Alert("缴费时间已过期");
                return false;
            }
            string[] ids = req.DataId.Split('_');
            if (req.PayType == 2)
            {
                if (ids.Length != 1)
                {
                    Alert("只能支付一个");
                    return false;
                }
                var cnt = db.TpxinOctoberActivitySet.Count(x => x.Nodeid == req.Nodeid && x.Transferids != null && x.ActivityId == req.ActivityId);
                if (cnt > 0)
                {
                    Alert("已经存在缴过费的记录，只能支付一个");
                    return false;
                }
            }
            foreach (string sid in ids)
            {
                int tid = Convert.ToInt32(sid);
                var entity = db.TpxinOctoberActivitySet.FirstOrDefault(x => x.Id == tid);
                if (req.PayType == 1)
                {
                    if (!string.IsNullOrEmpty(entity.Ptransferids))
                    {
                        Alert("不能选中已缴费的记录，请重新打开当前页面");
                        return false;
                    }
                }
                if (req.PayType == 2)
                {
                    if (!string.IsNullOrEmpty(entity.Transferids))
                    {
                        Alert("不能选中已缴费的记录，请重新打开当前页面");
                        return false;
                    }
                }
            }

            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TpcnUepayconfig ueConfig = db.TpcnUepayconfigSet.FirstOrDefault(a => a.Typeid == 1);
            if (ueConfig == null || ueConfig.Id <= 0)
            {
                Alert("获取UE支付配置失败");
                return false;
            }
            Currency currency = new Currency(CurrencyType.DOS_矿沙, req.Price);
            decimal total = currency.Amount;
            int unit = currency.Type.CurrencyId;
            TnetUepayhis uePayHis = new TnetUepayhis
            {
                Typeid = 20008,
                Nodeid = regInfo.Nodeid,
                BusinessParams = 10 + "|" + req.DataId + "|" + req.PayType + "|" + req.Nodeid + "|" + req.Price.ToString(),
                Amount = total,
                Unit = unit,
                Freezeids = "",
                Createtime = DateTime.Now
            };
            db.TnetUepayhisSet.Add(uePayHis);
            if (db.SaveChanges() <= 0)
            {
                Alert("生成UE订单失败");
                return false;
            }
            var charge = new ChargeDto
            {
                businesstypeid = 20008,
                amount = total,
                unit = unit,
                body = "十月送手机活动支付服务费",
                subject = "十月送手机活动支付服务费",
                orderno = uePayHis.Id.ToString(),
                paycode = ueConfig.Paycode,
                noticeurl = Helper.DomainUrl + "/UENotice/Success",
                createtime = uePayHis.Createtime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            UEPayCallDto = new UePayCallDto();
            UEPayCallDto.Charge = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(charge)));
            UEPayCallDto.sign = Md5.SignString(UEPayCallDto.Charge + AppConfig.AppSecurityString);
            UEPayCallDto.orderno = uePayHis.Id.ToString();
            return true;
        }

        /// <summary>
        /// ue支付回调
        /// </summary>
        public bool OctoberActivityDosUEPay_Notice(TnetUepayhis uePayHis)
        {
            db.BeginTransaction();
            try
            {
                TnetReginfo regInfo = PxinCache.GetRegInfo(uePayHis.Nodeid);
                string[] paras = uePayHis.BusinessParams.Split('|');

                if (int.TryParse(paras[0], out int month) && month == 10)//10月活动
                {
                    if (paras.Length != 5)
                    {
                        Alert("OctoberActivityDosUEPay_Notice TnetUepayhis.BusinessParams 数据有问题");
                        db.Rollback();
                        return false;
                    }
                    string ids = paras[1];
                    string[] idsArr = ids.Split('_');
                    foreach (var item in idsArr)
                    {
                        int id = int.Parse(item);
                        var entity = db.TpxinOctoberActivitySet.FirstOrDefault(x => x.Id == id);
                        if (paras[2] == "1")
                        {
                            entity.Ptransferids = uePayHis.Id.ToString();
                            entity.Ptransfertime = DateTime.Now;
                            //entity.Pamount = decimal.Parse(paras[3]);
                        }
                        else
                        {
                            entity.Transferids = uePayHis.Id.ToString();
                            entity.Transfertime = DateTime.Now;
                            //entity.Amount = decimal.Parse(paras[3]);
                        }
                        if (!string.IsNullOrWhiteSpace(entity.Transferids) && !string.IsNullOrWhiteSpace(entity.Ptransferids))
                        {
                            entity.Status = 2;
                        }
                        else
                        {
                            entity.Status = 1;
                        }
                    }
                }
                else
                {
                    if (!NovemberActivityDosPay_Pro(paras, uePayHis))
                    {
                        db.Rollback();
                        return false;
                    }
                }
                uePayHis.BusinessId = 0;
                uePayHis.Status = 2;
                if (db.Entry(uePayHis).State == EntityState.Detached)
                {
                    db.TnetUepayhisSet.Attach(uePayHis);
                    db.Entry(uePayHis).State = EntityState.Modified;
                }
                if (db.SaveChanges() <= 0)
                {
                    Alert("更新订单状态失败");
                    db.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Info("支付回调失败。原因：" + ex);
                db.Rollback();
                return false;
            }
            db.Commit();
            return true;
        }

        /// <summary>
        /// 查询快递
        /// </summary>
        public ExpressResp2 GetExpressInfo(string expressNum)
        {
            Req req = new Req();
            req.com = "";
            req.num = expressNum;
            ExpressResp2 resp = ExpressSearch(req);
            if (resp != null && resp.code.Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                return resp;
            }
            Alert("暂无快递信息");
            return null;
        }

        /// <summary>
        /// 领取手机资格的列表
        /// </summary>
        public IPagedList<OctoberActivityDto2> GetReceiveList2(string Nodename, string Nodecode, string Mobile, int TypeId, int Status, int page = 1, int pageSize = 10)
        {
            var query = from x in db.TpxinOctoberActivitySet.Where(x => x.Status > 1 && x.Status < 4)
                        join b in db.TnetReginfoSet on x.Nodeid equals b.Nodeid
                        select new OctoberActivityDto2
                        {
                            Nodeid = x.Nodeid,
                            Expressno = x.Expressno,
                            Id = x.Id,
                            Nodename = b.Nodename,
                            Note = x.Note,
                            Typeid = x.Typeid,
                            CreateTime = x.Createtime,
                            Status = x.Status,
                            Mobile = b.Mobileno,
                            Nodecode = b.Nodecode
                        };
            if (!string.IsNullOrEmpty(Nodename))
            {
                string name = Nodename.Trim();
                query = query.Where(a => a.Nodename.Contains(name));
            }
            if (!string.IsNullOrEmpty(Nodecode))
            {
                string code = Nodecode.Trim();
                query = query.Where(a => a.Nodecode.Contains(code));
            }
            if (!string.IsNullOrEmpty(Mobile))
            {
                string code = Mobile.Trim();
                query = query.Where(a => a.Mobile.Contains(code));
            }
            if (TypeId != 0)
            {
                query = query.Where(a => a.Typeid == TypeId);
            }
            if (Status != 0)
            {
                query = query.Where(a => a.Status == Status);
            }
            query = query.OrderByDescending(a => a.Id);
            return query.ToPagedList(page, pageSize);
        }
        /// <summary>
        /// 
        /// </summary>
        public Respbase Express(int id, string expressNo)
        {
            var entity = db.TpxinOctoberActivitySet.FirstOrDefault(x => x.Id == id);
            if (entity != null && entity.Status == 2)
            {
                entity.Expressno = expressNo;
                entity.Status = 3;
                entity.Sendtime = DateTime.Now;
                if (db.SaveChanges() <= 0)
                {
                    return new Respbase();
                }
            }
            return new Respbase { Message = "保存失败", Result = -1 };
        }

        /// <summary>
        /// 
        /// </summary>
        public OctoberActivityDto2 GetDetail(int id)
        {
            var query = from x in db.TpxinOctoberActivitySet.Where(x => x.Id == id)
                        join b in db.TnetReginfoSet on x.Nodeid equals b.Nodeid
                        join c in db.TnetReginfoSet on x.Pnodeid equals c.Nodeid
                        select new OctoberActivityDto2
                        {
                            Nodeid = x.Nodeid,
                            Expressno = x.Expressno,
                            Id = x.Id,
                            Nodename = b.Nodename,
                            Note = x.Note,
                            Typeid = x.Typeid,
                            CreateTime = x.Createtime,
                            Status = x.Status,
                            Mobile = b.Mobileno,
                            Nodecode = b.Nodecode,
                            PNodecode = c.Nodecode,
                            PNodeid = c.Nodeid,
                            PNodename = c.Nodename
                        };
            var result = query.FirstOrDefault();
            return result;
        }
        /// <summary>
        /// 检查每月活动是否绑定pcn帐号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool HasBindActivityThirdparty(HasBindActivityThirdpartyReq req)
        {
            return db.TpxinActivityThirdpartySet.FirstOrDefault(p => p.Nodeid == req.Nodeid && p.ActivityId == req.ActivityId) != null;
        }
        /// <summary>
        /// 每月活动绑定pcn帐号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool BindActivityThirdparty(BindActivityThirdpartyReq req)
        {
            var entity = db.TpxinActivityThirdpartySet.FirstOrDefault(p => p.Nodeid == req.Nodeid && p.ActivityId == req.ActivityId);
            if (entity != null)
            {
                Alert("对不起,你已经绑定过pcn帐号");
                return false;
            }
            var resultStr = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/user/GetUserInfo", new { Nodecode = req.PcnAccount, Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid });
            if (string.IsNullOrEmpty(resultStr) || !resultStr.Contains("result"))
            {
                Alert("网络异常,请重试!");
                return false;
            }
            var userResult = JObject.Parse(resultStr);
            if (!(int.TryParse(userResult.GetValue("result").ToString(), out int res) && res > 0))
            {
                Alert($"绑定失败,{userResult.GetValue("message").ToString()}");
                return false;
            }
            entity = new TpxinActivityThirdparty();
            entity.Nodeid = req.Nodeid;
            entity.Targetid = req.PcnAccount;
            entity.Remarks = "绑定pcn帐号";
            entity.ActivityId = req.ActivityId;
            db.TpxinActivityThirdpartySet.Add(entity);
            var result = db.SaveChanges();
            if (result <= 0)
            {
                Alert("绑定失败");
                return false;
            }
            return result > 0;
        }
        #region 活动
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="req"></param>
        public List<ActivityDto> GetActivitys(Reqbase req)
        {
            return db.TpxinActivitySet.Where(p => DateTime.Now > p.ActivityStarttime).Select(p => new ActivityDto
            {
                Id = p.Id,
                ActivityName = p.ActivityName,
                ActivityStarttime = p.ActivityStarttime,
                ActivityEndtime = p.ActivityEndtime,
                PayStarttime = p.PayStarttime,
                PayEndtime = p.PayEndtime,
                Cover = p.Cover,
                Createtime = p.Createtime
            }).ToList();
        }
        #endregion
        #region 11月活动-迪拜见证之旅
        /// <summary>
        /// 获取11月活动-迪拜见证之旅 服务费的数量
        /// </summary>
        public NovemberActivityCountDto GetNovemberActivityCount(Reqbase req)
        {
            return new NovemberActivityCountDto
            {
                SatisfyCondiCount = db.VpxinOctoberActivitySet.Count(x => x.Pnodeid == req.Nodeid),
                QualifyCount = db.VpxinOctoberActivitySet.Count(x => x.Nodeid == req.Nodeid)
            };
        }
        /// <summary>
        /// 已满足条件和已获得资格列表
        /// </summary>
        public VpxinOctoberActivityDto GetVpxinOctoberActivitys(VpxinOctoberActivityReq req)
        {
            var activity = db.TpxinActivitySet.FirstOrDefault(p => p.Id == req.ActivityId);
            if (activity == null)
            {
                Alert("找不到活动信息,请重试");
                return null;
            }
            var result = new VpxinOctoberActivityDto();
            result.Status = DateTime.Now >= activity.PayStarttime ? (DateTime.Now <= activity.PayEndtime ? 2 : 1) : 0;
            result.SatisfyCondiList = (from x in db.VpxinOctoberActivitySet
                                       join p in db.TpxinOctoberActivitySet on new { x.Nodeid, x.Pnodeid, req.ActivityId } equals new { p.Nodeid, p.Pnodeid, p.ActivityId } into p_join
                                       from p in p_join.DefaultIfEmpty()
                                       join b in db.TnetReginfoSet on x.Nodeid equals b.Nodeid
                                       where x.Pnodeid == req.Nodeid
                                       select new SatisfyCondiAndQualify
                                       {
                                           HisId = x.Hisid,
                                           Amount = x.Pamount,
                                           Nodeid = x.Nodeid,
                                           TempId = p.Id,
                                           Nodecode = b.Nodecode,
                                           Nodename = b.Nodename,
                                           Note = x.Pnote,
                                           Typeid = x.Typeid,
                                           TempCreateTime = p.Createtime,
                                           Ptransferids = p.Ptransferids,
                                           Transferids = p.Transferids,
                                           Status = p.Status == 1 ? (p.Ptransferids == null ? 0 : 1) : (p.Status == 2 ? 1 : p.Status),
                                           PayStatus = p.Transferids == null ? 0 : 1
                                       }).ToList();
            result.QualifyList = (from x in db.VpxinOctoberActivitySet
                                  join p in db.TpxinOctoberActivitySet on new { x.Nodeid, x.Pnodeid, req.ActivityId } equals new { p.Nodeid, p.Pnodeid, p.ActivityId } into p_join
                                  from p in p_join.DefaultIfEmpty()
                                  join b in db.TnetReginfoSet on x.Pnodeid equals b.Nodeid
                                  where x.Nodeid == req.Nodeid
                                  select new SatisfyCondiAndQualify
                                  {
                                      HisId = x.Hisid,
                                      Amount = x.Amount,
                                      Nodeid = x.Pnodeid,
                                      TempId = p.Id,
                                      Nodecode = b.Nodecode,
                                      Nodename = b.Nodename,
                                      Note = x.Note,
                                      Typeid = x.Typeid,
                                      TempCreateTime = p.Createtime,
                                      Ptransferids = p.Ptransferids,
                                      Transferids = p.Transferids,
                                      Status = p.Status == 1 ? (p.Transferids == null ? 0 : 1) : (p.Status == 2 ? 1 : p.Status),
                                      PayStatus = x.Pnodeid == 1 ? 1 : (p.Ptransferids == null ? 0 : 1),
                                  }).ToList();
            return result;
        }
        /// <summary>
        /// 调用ue支付
        /// </summary>
        public NovemberActivityDosPayDto NovemberActivityDosPay(NovemberActivityDosPayReq req)
        {
            log.Info($"11月活动-迪拜见证之旅支付服务费,nodeid={req.Nodeid},服务费={req.Price}");

            if (!NovemberActivityDosPayCheck(req))
                return null;
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            var businessParams = 11 + "|" + req.BusinessIdStr + "|" + req.PayType + "|" + req.HisIdStr + "|" + req.ActivityId;
            var uePayResult = UEPay.UePayHelper.DosWithUePay(db, regInfo, 1, CurrencyType.DOS_矿沙, req.Price, 20008, businessParams, "11月活动-迪拜见证之旅支付服务费", "11月活动-迪拜见证之旅支付服务费").Result;
            if (!uePayResult.IsSuccess)
            {
                Alert(uePayResult.Message);
                return null;
            }
            return new NovemberActivityDosPayDto
            {
                ChargeStr = uePayResult.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = uePayResult.OrderNo
            };
        }
        private bool NovemberActivityDosPayCheck(NovemberActivityDosPayReq req)
        {
            var activity = db.TpxinActivitySet.FirstOrDefault(p => p.Id == req.ActivityId);
            if (activity == null)
            {
                Alert("找不到活动信息,请重试");
                return false;
            }
            if (AppConfig.IsUseSms && DateTime.Now < activity.PayStarttime)
            {
                Alert("未到缴费时间");
                return false;
            }
            if (AppConfig.IsUseSms && DateTime.Now > activity.PayEndtime)
            {
                Alert("缴费时间已过期");
                return false;
            }
            string[] ids = req.BusinessIdStr.Split('_');
            if (req.PayType == 2)
            {
                if (ids.Length != 1)
                {
                    Alert("只能支付一个");
                    return false;
                }
                var cnt = db.TpxinOctoberActivitySet.Count(x => x.Nodeid == req.Nodeid && x.Transferids != null && x.ActivityId == req.ActivityId);
                if (cnt > 0)
                {
                    Alert("已经存在缴过费的记录，只能支付一个");
                    return false;
                }
            }
            foreach (string sid in ids)
            {
                int tid = Convert.ToInt32(sid);
                var entity = db.TpxinOctoberActivitySet.FirstOrDefault(x => x.Id == tid);
                if (entity != null)
                {
                    if (req.PayType == 1)
                    {
                        if (!string.IsNullOrEmpty(entity.Ptransferids))
                        {
                            Alert("不能选中已缴费的记录，请重新打开当前页面");
                            return false;
                        }
                    }
                    if (req.PayType == 2)
                    {
                        if (!string.IsNullOrEmpty(entity.Transferids))
                        {
                            Alert("不能选中已缴费的记录，请重新打开当前页面");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool NovemberActivityDosPay_Pro(string[] businessParams, TnetUepayhis uePayHis)
        {
            if (businessParams.Length != 5)
            {
                Alert("NovemberActivityDosPay_Pro TnetUepayhis.BusinessParams 数据有问题");
                return false;
            }
            string businessIdStr = businessParams[1];
            string[] businessIds = businessIdStr.Split('_');
            string hisIdStr = businessParams[3];
            string[] hisIds = hisIdStr.Split('_');
            if (businessIds.Length != hisIds.Length)
            {
                Alert("NovemberActivityDosPay_Pro TnetUepayhis.BusinessParams 数据有问题");
                return false;
            }
            var index = 0;
            foreach (var item in businessIds)
            {
                var idResult = int.TryParse(item, out int id);
                var hisIdResult = int.TryParse(hisIds[index], out int hisId);
                var activityIdResult = int.TryParse(businessParams[4], out int activityId);
                if (idResult && hisIdResult && activityIdResult)
                {
                    var entity = db.TpxinOctoberActivitySet.FirstOrDefault(x => x.Id == id);
                    if (entity == null)
                    {
                        var vEntity = db.VpxinOctoberActivitySet.FirstOrDefault(p => p.Hisid == hisId);
                        if (vEntity == null)
                        {
                            Alert($"获取VpxinOctoberActivitySet数据失败,hisid={hisId}");
                            return false;
                        }
                        entity = new TpxinOctoberActivity();
                        entity.Typeid = vEntity.Typeid;
                        entity.Nodeid = vEntity.Nodeid;
                        entity.Note = vEntity.Note;
                        entity.Amount = vEntity.Amount;
                        entity.Pnodeid = vEntity.Pnodeid;
                        entity.Pnote = vEntity.Pnote;
                        entity.Pamount = vEntity.Pamount;
                        entity.Createtime = DateTime.Now;
                        entity.ActivityId = activityId;
                        db.TpxinOctoberActivitySet.Add(entity);
                    }
                    if (businessParams[2] == "1")
                    {
                        entity.Ptransferids = uePayHis.Id.ToString();
                        entity.Ptransfertime = DateTime.Now;
                    }
                    else
                    {
                        entity.Transferids = uePayHis.Id.ToString();
                        entity.Transfertime = DateTime.Now;
                    }
                    if (!string.IsNullOrWhiteSpace(entity.Transferids) && !string.IsNullOrWhiteSpace(entity.Ptransferids) || (!string.IsNullOrWhiteSpace(entity.Transferids) && entity.Pnodeid == 1))
                        entity.Status = 2;
                    else
                        entity.Status = 1;
                }
                else
                {
                    Alert($"获取业务参数失败,参数：{businessParams}");
                    return false;
                }
                index += 1;
            }
            return true;
        }
        #endregion
    }
}
