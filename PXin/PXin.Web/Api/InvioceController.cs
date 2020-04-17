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
    /// <summary>
    /// 发票专区
    /// </summary>
    public class InvioceController : ApiController
    {
        /// <summary>
        /// 获取首页开票统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<InvioceStatisticsDto> GetInvioceStatistics(Reqbase req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.GetInvioceStatistics(req);
            if (result!=null)
            {
                return new Respbase<InvioceStatisticsDto> { Result = 1, Message = facade.PromptInfo.Message,Data=result };
            }
            return new Respbase<InvioceStatisticsDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取可申请列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<InvioceMayApplyDto>> GetMayApplyInvioceHis(PageBase req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.GetMayApplyInvioceHis(req);
            if (result != null)
            {
                return new Respbase<List<InvioceMayApplyDto>> { Result = 1, Message = facade.PromptInfo.Message, Data = result };
            }
            return new Respbase<List<InvioceMayApplyDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取已申请列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<InvioceAlreadyApplyDto>> GetAlreadyApplyInvioceHis(PageBase req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.GetAlreadyApplyInvioceHis(req);
            if (result != null)
            {
                return new Respbase<List<InvioceAlreadyApplyDto>> { Result = 1, Message = facade.PromptInfo.Message, Data = result };
            }
            return new Respbase<List<InvioceAlreadyApplyDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 提交增票资质申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase ApplyInvioceQualifica(ApplyInvioceQualificaReq req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.ApplyInvioceQualifica(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = facade.PromptInfo.Message};
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取增票资质
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<InvioceQualificaDto> GetInvioceQualifica(Reqbase req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.GetInvioceQualifica(req);
            if (result != null)
            {
                return new Respbase<InvioceQualificaDto> { Result = 1, Message = facade.PromptInfo.Message, Data = result };
            }
            return new Respbase<InvioceQualificaDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 开票申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase ApplyWriteInvioce(ApplyWriteInvioceReq req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.ApplyWriteInvioce(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SendEmail(SendEmailReq req)
        {
            InvioceFacade facade = new InvioceFacade();
            facade.SendEmail(req);
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取电子发票详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<InvioceDetailDto> GetElectronicInvioceDetail(InvioceDetailReq req)
        {
            InvioceFacade facade = new InvioceFacade();
            var result = facade.GetElectronicInvioceDetail(req);
            if (result != null)
            {
                return new Respbase<InvioceDetailDto> { Result = 1, Message = facade.PromptInfo.Message, Data = result };
            }
            return new Respbase<InvioceDetailDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }
        

    }
}
