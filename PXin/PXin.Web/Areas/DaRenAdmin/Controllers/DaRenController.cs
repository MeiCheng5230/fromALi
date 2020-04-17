using Common.Mvc;
using Newtonsoft.Json;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.Areas.DaRenAdmin.Controllers
{
    public class DaRenController : MvcBaseController
    {
        // GET: DaRenAdmin/DaRen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// 获取待审核用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAwaitVerifyDaRen(GetAwaitVerifyDaRenReq req)
        {
            DaRenFacade facaed = new DaRenFacade();
            var result = facaed.GetAwaitVerifyDaRen(req);
            if (result == null)
            {
                return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Data = result, Message = "成功", Result = 1});
        }

        /// <summary>
        /// 获取待审核达人详情信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVerifyDaRenDetail(GetVerifyDaRenDetailReq req)
        {
            DaRenFacade facaed = new DaRenFacade();
            var result = facaed.GetVerifyDaRenDetail(req);
            if (result != null)
            {
                return MyJson(new { Data = result, Message = "成功", Result = 1 });
            }
            
            return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminVerifyDaRen(AdminVerifyDaRenReq req)
        {
            DaRenFacade facaed = new DaRenFacade();
            var result = facaed.AdminVerifyDaRen(req);
            if (!result)
            {
                return Json(new { Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new {  Message = "成功", Result = 1 });
        }
        
        /// <summary>
        /// 添加默认推荐达人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDefultDaRen(CreateDefultDaRenReq req)
        {
            DaRenFacade facaed = new DaRenFacade();
            var result = facaed.CreateDefultDaRen(req);
            if (!result)
            {
                return Json(new { Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Message = "成功", Result = 1 });
        }
        
        /// <summary>
        /// 获取静态页达人信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Anonymous]
        public ActionResult GetDaRenInfo(GetDaRenStaticReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenInfo(req.NodeCode);
            if (result==null)
            {
                return Json(new { Message = facade.PromptInfo.Message, Result = -1 });
            }
            return MyJson(new { Message = "成功", Result = 1, Data = result });
        }

    }
}