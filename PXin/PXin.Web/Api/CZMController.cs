using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.Models;
using PXin.DB;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 充值码
    /// </summary>
    public class CZMController : ApiController
    {
        /// <summary>
        /// 获取充值码面额配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<ConfigList>> GetSvcConfig(Reqbase req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.GetSvcConfig(req);
            if (result != null)
            {
                return new Respbase<List<ConfigList>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<ConfigList>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
        }
        /// <summary>
        /// 购买Svc充值码,暂时只有微信
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<OpenInfUeoDto> BuySvc(BuyReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.BuySvc(req);
            return new Respbase<OpenInfUeoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
        }

        /// <summary>
        /// 零售SVC充值码，充值商代理人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SaleSvc(SaleReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.SaleSvc(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 回收SVC
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase RecoverySvc(RecoveryReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.RecoverySvc(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// Svc充值码 充值 SV
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SvcToSvByOwner(UseByOwnerReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.SvcToSvByOwner(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }
        /// <summary>
        /// 无主Svc充值码 充值 SV
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SvcToSvByCard(UseByCardReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.SvcToSvByCard(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// sv生成充值码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SvToSvcCard(SvToSvcCardReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.SvToSvcCard(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }



        /// <summary>
        /// 获取我的SVC充值码统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<SvcStatisDto> GetMySvcStatis(Reqbase req)
        {

            CZMFacade facade = new CZMFacade();
            var result = facade.GetMySvcStatis(req);
            if (result != null)
            {
                return new Respbase<SvcStatisDto> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<SvcStatisDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }
        /// <summary>
        /// 获取我的SVC充值码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<MySvcDto>> GetMySvc(Reqbase req)
        {
            // tblc_centcard
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var query = from tc in db.TblcCentcardSet
                        where tc.Usenodeid == req.Nodeid && tc.Status == 1 && tc.Areaid == "1"
                        orderby tc.Createdtime descending
                        select new MySvcDto
                        {
                            Amount = (decimal)tc.Amount,
                            Cardno = tc.Cardno,
                            Areaid = tc.Areaid,
                        };
            var result = query.ToList();
            if (result != null)
            {
                return new Respbase<List<MySvcDto>> { Data = result, Result = 1, Message = "成功" };
            }
            return new Respbase<List<MySvcDto>> { Result = -1, Message = "未找到数据" };
        }
        /// <summary>
        /// 获取我的SVC充值码变动历史
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<MySvchisDto>> GetMySvchis(HisReq req)
        {
            // tblc_centcard_his
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var query = from tc in db.TblcCentcardSet
                        join th in db.TblcCentcardHisSet on tc.Idno equals th.Idno
                        join tr in db.TnetReginfoSet on th.Nodeid equals tr.Nodeid into trdata
                        from trf in trdata.DefaultIfEmpty()
                        where tc.Usenodeid == req.Nodeid || th.Opnodeid == req.Nodeid
                        orderby th.Createtime descending
                        select new MySvchisDto
                        {
                            Amount = (decimal)tc.Amount,
                            Typeid = th.Typeid == 1 && th.Nodeid == req.Nodeid ? 4 : th.Typeid,
                            Note = th.Note,
                            CreateTime = th.Createtime,
                            Cardno = tc.Cardno,
                            AmountType = "SV",
                            NodeCode = trf.Nodecode
                        };
            List<MySvchisDto> result = null;
            if (req.TypeId != -1)
            {
                if (req.TypeId == 0)
                {
                    result = query.Where(c => c.Typeid == 0 || c.Typeid == 2 || c.Typeid == 3 || c.Typeid == 4 || c.Typeid == 7 || c.Typeid == 8 || c.Typeid == 9 || c.Typeid == 10 || c.Typeid == 11).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
                }
                else
                {
                    result = query.Where(c => c.Typeid == req.TypeId).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
                }

            }
            else
            {
                result = query.Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
            }

            if (result != null)
            {
                return new Respbase<List<MySvchisDto>> { Data = result, Result = 1, Message = "成功" };
            }
            return new Respbase<List<MySvchisDto>> { Result = -1, Message = "未找到数据" };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<SVUserInfoDto> GetUserInfo(SVUserInfoReq req)
        {

            CZMFacade facade = new CZMFacade();
            var result = facade.GetUserInfo(req);
            if (result != null)
            {
                return new Respbase<SVUserInfoDto> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<SVUserInfoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }

        /// <summary>
        /// 微信支付手动回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase Success(SuccessReq req)
        {
            CZMFacade facade = new CZMFacade();
            if (!facade.PingppPaySuccess_SVC(req.Orderno))
            {
                return new Respbase<SVUserInfoDto> { Result = -1, Message = "失败" };
            }

            return new Respbase<SVUserInfoDto> { Result = 1, Message = "成功" };
        }

        /// <summary>
        /// 根据面额统计获取svc充值码
        /// </summary>
        [HttpPost]
        public Respbase<List<SvcByGroupbyAmountDto>> GetSvcByGroupbyAmount(SvcByGroupbyAmountReq req)
        {
            CZMFacade facade = new CZMFacade();
            var result = facade.GetSvcByGroupbyAmount(req);
            if (result != null)
            {
                return new Respbase<List<SvcByGroupbyAmountDto>> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<List<SvcByGroupbyAmountDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
        }
        /// <summary>
        /// 冻结Svc充值码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase FrozenSvc(FrozenSvcReq req)
        {
            CZMFacade facade = new CZMFacade();
            if (!facade.FrozenSvc(req))
            {
                return new Respbase { Result = -1, Message = "失败" };
            }
            return new Respbase { Result = 1, Message = "成功" };
        }
        /// <summary>
        /// 购买SVC充值码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase BuySvcCode(BuySvcCodeReq req)
        {
            CZMFacade facade = new CZMFacade();
            if (!facade.BuySvcCode(req))
            {
                return new Respbase { Result = -1, Message = "失败" };
            }
            return new Respbase { Result = 1, Message = "成功" };
        }
        /// <summary>
        /// 取消发布(解冻)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CancelRelease(CancelReleaseReq req)
        {
            CZMFacade facade = new CZMFacade();
            if (!facade.CancelRelease(req))
            {
                return new Respbase { Result = -1, Message = "失败" };
            }
            return new Respbase { Result = 1, Message = "成功" };
        }
    }
}
