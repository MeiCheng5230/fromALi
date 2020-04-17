using Common.Facade.Models;
using Common.UEPay;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static PXin.Facade.CommonService.ExpressAPI;

namespace PXin.Web.Api
{
    /// <summary>
    /// 十月送手机活动
    /// </summary>
    public class ActivityController : ApiController
    {
        /// <summary>
        /// 获取十月送手机活动的领取手机和支付服务费的数量
        /// </summary>
        [HttpPost]
        public Respbase<OctoberActivityCountDto> GetOctoberActivityCount(OctoberActivityCountReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.GetOctoberActivityCount(req);
            return new Respbase<OctoberActivityCountDto> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }

        /// <summary>
        /// 获取十月送手机活动的领取手机和支付服务费的列表
        /// </summary>
        [HttpPost]
        public Respbase<OctoberActivityListDto> GetOctoberActivityList(OctoberActivityListReq req)
        {
            OctoberActivityListDto dto = new OctoberActivityListDto();
            var facade = new ActivityFacade();
            dto.PayList = facade.GetPayList(req);
            dto.ReceiveList = facade.GetReceiveList(req);
            return new Respbase<OctoberActivityListDto> { Data = dto, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }

        /// <summary>
        /// 调用ue支付
        /// </summary>
        [HttpPost]
        public Respbase<UePayCallDto> OctoberActivityDosUEPrepare(OctoberActivityDosUEPrepareReq req)
        {
            var facade = new ActivityFacade();
            if (facade.OctoberActivityDosUEPrepare(req))
            {
                return new Respbase<UePayCallDto> { Data = facade.UEPayCallDto, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
            }
            return new Respbase<UePayCallDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
        }

        /// <summary>
        /// 查询快递
        /// </summary>
        [HttpPost]
        public Respbase<ExpressResp2> GetExpressInfo(GetExpressInfoReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.GetExpressInfo(req.ExpressNo);
            return new Respbase<ExpressResp2> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        public Respbase<List<ActivityDto>> GetActivitys(Reqbase req)
        {
            var facade = new ActivityFacade();
            var result = facade.GetActivitys(req);
            return new Respbase<List<ActivityDto>> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }
        /// <summary>
        /// 获取11月活动-迪拜见证之旅 服务费的数量
        /// </summary>
        [HttpPost]
        public Respbase<NovemberActivityCountDto> GetNovemberActivityCount(Reqbase req)
        {
            var facade = new ActivityFacade();
            var result = facade.GetNovemberActivityCount(req);
            return new Respbase<NovemberActivityCountDto> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }
        /// <summary>
        /// 检查每月活动是否绑定pcn帐号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase HasBindActivityThirdparty(HasBindActivityThirdpartyReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.HasBindActivityThirdparty(req);
            return new Respbase() { Message = result ? facade.PromptInfo.Message : "你还没有绑定PCN账号", Result = result ? 1 : 0 };
        }
        /// <summary>
        /// 每月活动绑定pcn帐号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase BindActivityThirdparty(BindActivityThirdpartyReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.BindActivityThirdparty(req);
            return new Respbase() { Message = facade.PromptInfo.Message, Result = result ? 1 : 0 };
        }
        /// <summary>
        /// 已满足条件和已获得资格列表
        /// </summary>
        [HttpPost]
        public Respbase<VpxinOctoberActivityDto> GetVpxinOctoberActivitys(VpxinOctoberActivityReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.GetVpxinOctoberActivitys(req);
            return new Respbase<VpxinOctoberActivityDto> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }

        /// <summary>
        /// 调用ue支付
        /// </summary>
        [HttpPost]
        public Respbase<NovemberActivityDosPayDto> NovemberActivityDosPay(NovemberActivityDosPayReq req)
        {
            var facade = new ActivityFacade();
            var result = facade.NovemberActivityDosPay(req);
            return new Respbase<NovemberActivityDosPayDto> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }
    }
}
