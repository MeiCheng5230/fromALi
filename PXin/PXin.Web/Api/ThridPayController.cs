using Common.Facade.Models;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXin.Web.Api
{
    public class ThridPayController : ApiController
    {
        /// <summary>
        /// 第三方获取Pcn账号校验
        /// </summary>
        [HttpPost]
        public Respbase<ThirdPartyVerifyDto> ThirdPartyVerify(ThirdPartyVerifyReq req)
        {
            ThridPayFacade facade = new ThridPayFacade();
            var result = facade.ThirdPartyVerify(req);
            return result;
        }

        /// <summary>
        /// 第三方支付接口
        /// </summary>
        [HttpPost]
        public Respbase<ThridPayDto> ThridPartyPay(ThridPayReq req)
        {
            ThridPayFacade facade = new ThridPayFacade();
            var result = facade.ThridPartyPay(req);
            return result;
        }

        /// <summary>
        /// 查询支付状态
        /// </summary>
        [HttpPost]
        public Respbase<GetThridPayhisDto> GetThridPayhis(GetThridPayhisReq req)
        {
            ThridPayFacade facade = new ThridPayFacade();
            var result = facade.GetThridPayhis(req);
            return result;
        }

        /// <summary>
        /// 获取支付类型
        /// </summary>
        [HttpPost]
        public Respbase<List<GetThridPayTypeDto>> GetThridPayType(GetThridPayTypeReq req)
        {
            List<GetThridPayTypeDto> dtos = new List<GetThridPayTypeDto>();
            dtos.Add(new GetThridPayTypeDto { TypeId = 3000, TypeName = "SV" });
            dtos.Add(new GetThridPayTypeDto { TypeId = 3001, TypeName = "V点" });
            return new Respbase<List<GetThridPayTypeDto>>() { Data = dtos };
        }
    }
}
