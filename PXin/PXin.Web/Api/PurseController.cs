using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.Filter;
using MvcPaging;
using PXin.DB;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.UserPurseReq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PXin.Web.Api
{
    public class PurseController : ApiController
    {
        private readonly PurseFacade purseFacade;

        /// <summary>
        /// 
        /// </summary>
        public PurseController()
        {
            purseFacade = new PurseFacade();
        }

        /// <summary>
        /// 钱包账单记录分类Logo
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<PurseHisTypeLogoDto>> GetPurseHisTypeLogo(Reqbase req)
        {
            return new Respbase<List<PurseHisTypeLogoDto>>() { Data = purseFacade.GetPurseHisTypeLogo(req) };
        }

        /// <summary>
        /// 获取钱包列表
        /// </summary>
        [HttpPost]
        public Respbase<List<PurseDto>> GetPurses(Reqbase req)
        {
            var result = purseFacade.GetPurses(req);
            return result;
        }

        /// <summary>
        /// 获取首页钱包数据
        /// </summary>
        [HttpPost]
        public Respbase<List<List<Purse2Dto>>> GetPurses2(Reqbase req)
        {
            var result = purseFacade.GetPurses2(req);
            return result;
        }

        /// <summary>
        /// 获取首页钱包数据3
        /// </summary>
        [HttpPost]
        public Respbase<List<List<Purse3Dto>>> GetPurses3(Reqbase req)
        {
            var result = purseFacade.GetPurses3(req);
            return result;
        }


        /// <summary>
        /// 获取指定钱包账单记录
        /// </summary>
        [HttpPost]
        public Respbase<List<UserPurseHisDto>> GetPurseHis(PurseHisReq req)
        {
            var result = purseFacade.GetUserPurseHis(req);
            return result;
        }

        /// <summary>
        /// 扣费
        /// </summary>
        [HttpPost]
        [UnLogin]
        public Respbase<RecoveryDto> Recovery(Facade.Models.UserPurseReq.PurseRecoveryReq req)
        {
            var result = purseFacade.Recovery(req);
            return result;
        }

        /// <summary>
        /// 第三方支付
        /// </summary>
        [HttpPost]
        [OverrideActionFilters]
        public Respbase<ThridPartyPayDto> ThridPartyPay(ThridPartyPayReq req)
        {
            var result = purseFacade.ThridPartyPay(req);
            return result;
        }

        /// <summary>
        /// UV充值
        /// </summary>
        [HttpPost]
        public Respbase<Pingpp.Models.Charge> Recharge(RechargeReq req)
        {
            var facade = new CZMFacade();
            var result = facade.BuyUV(req);
            return new Respbase<Pingpp.Models.Charge>() { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }

        /// <summary>
        /// 获取UV充值记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<UVChargeHisDto>> GetUVChargeHis(UVRechargeHisReq req)
        {
            var result = purseFacade.GetUVChargeHis(req);
            return result;
           
        }
    }
}
