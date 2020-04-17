using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using PXin.DB;
using PXin.Facade;
using PXin.Facade.ApiFacade;
using PXin.Facade.CommonService;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.UserDto;
using PXin.Facade.Models.UserReq;

namespace PXin.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAuthController : ApiController
    {
        /// <summary>
        /// 获取用户认证信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserAuthInfoDto> GetUserAuthInfo(Reqbase req)
        {
            var facade = new UserAuthFacade();
            return new Respbase<UserAuthInfoDto>()
            {
                Data = facade.GetUserAuthInfo(req.Nodeid),
                Result = facade.PromptInfo.Result,
                Message = facade.PromptInfo.Message
            };
        }

        /// <summary>
        /// 用户认证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserAuthInfoDto> UserAuthByPurse(AuthenByPurseReq req)
        {
            var facade = new UserAuthFacade();
            var data = facade.UserAuthByPurse(req);
            return new Respbase<UserAuthInfoDto>()
            {
                Result = facade.PromptInfo.Result,
                Data = data,
                Message = facade.PromptInfo.Message
            };
        }

        /// <summary>
        /// 获取用户认证照片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserAuthPic> GetUserAuthPicUrl(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            int nodeId = req.Nodeid;
            var authLog = db.TzcAuthLogSet.FirstOrDefault(w => w.Nodeid == nodeId);
            string data = authLog == null ? "" : authLog.Idcardpic1.StartsWith("http") ? authLog.Idcardpic1 : Helper.DomainUrl + "/" + authLog.Idcardpic1;
            return new Respbase<UserAuthPic>()
            {
                Result = string.IsNullOrEmpty(data) ? -1 : 1,
                Data = new UserAuthPic
                {
                    PicUrl1 = authLog == null ? "" : authLog.Idcardpic2.StartsWith("http") ? authLog.Idcardpic2 : Helper.DomainUrl + "/" + authLog.Idcardpic2,
                    PicUrl2 = authLog == null ? "" : authLog.Idcardpic1.StartsWith("http") ? authLog.Idcardpic1 : Helper.DomainUrl + "/" + authLog.Idcardpic1
                },
                Message = string.IsNullOrEmpty(data) ?
                "当前用户未实名认证" : "获取照片成功"
            };
        }

        /// <summary>
        /// 获取pcn认证用户身份证正面照片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserAuthPic> GetUserIDCardFrontPicFromPcn(GetIDCardPicFromPcnReq req)
        {
            var facade = new UserAuthFacade();
            var data = facade.GetUserIDCardFrontPicFromPcn(req);
            return new Respbase<UserAuthPic>()
            {
                Result = facade.PromptInfo.Result,
                Data = new UserAuthPic { PicUrl2 = data, PicUrl1 = data },
                Message = facade.PromptInfo.Message
            };
        }

        /// <summary>
        /// 通过PCN认证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserAuthInfoDto> UserAuthByPCN(AuthByPCNReq req)
        {
            var facade = new UserAuthFacade();
            var data = facade.UserAuthByPCN(req);
            return new Respbase<UserAuthInfoDto>()
            {
                Result = facade.PromptInfo.Result,
                Data = data,
                Message = facade.PromptInfo.Message
            };
        }

        /// <summary>
        /// 驾驶证绑定
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DriverLicenseDto> AuthDriverLicense(AuthDriverLicenseReq req)
        {
            var facade = new UserAuthFacade();
            var result = facade.AuthDriverLicense(req);
            if (result != null && result.FileNo?.Length > 4 && result.Name?.Length > 1)
            {
                string fileNo = result.FileNo;
                fileNo = fileNo.Substring(0, 4) + "***********" + fileNo.Substring(fileNo.Length - 2, 2);
                result.FileNo = fileNo;
                result.Name = result.Name.Substring(0, 1) + "**";
            }
            return new Respbase<DriverLicenseDto> { Data = result, Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result };
        }

        /// <summary>
        /// 获取驾驶证绑定信息(Result=-100：未绑定)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DriverLicenseDto> GetDriverLicense(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var driLicLog = db.TnetDriveLicLogSet.FirstOrDefault(f => f.Nodeid == req.Nodeid);
            if (driLicLog == null)
            {
                return new Respbase<DriverLicenseDto>() { Result = -100, Message = "未绑定" };
            }
            string fileNo = driLicLog.Fileno;
            fileNo = fileNo.Substring(0, 4) + "***********" + fileNo.Substring(fileNo.Length - 2, 2);
            return new Respbase<DriverLicenseDto>()
            {
                Data = new DriverLicenseDto()
                { DriverLicenseUrl = driLicLog.Cardimg, AppendixUrl = driLicLog.CardimgAppendix, FileNo = fileNo, Name = driLicLog.Name.Substring(0, 1) + "**", Status = driLicLog.Status }
            };
        }

        /// <summary>
        /// 解绑驾驶证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteDriverLicense(Reqbase req)
        {
            var facade = new UserAuthFacade();
            facade.DeleteDriverLicense(req.Nodeid);
            return facade.PromptInfo;
        }
    }
}
