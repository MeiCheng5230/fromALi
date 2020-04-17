using Common.Mvc;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.Areas.DaRenAdmin.Controllers
{
    public class InvoiceController : MvcBaseController
    {
        // GET: DaRenAdmin/Invoice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VerifyInvoice()
        {
            return View();
        }

        /// <summary>
        /// 获取增票资质申请列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetInvioceQualificaList(GetInvioceQualificaListReq req)
        {
            InvioceFacade facaed = new InvioceFacade();
            var result = facaed.GetInvioceQualificaList(req);
            if (result == null)
            {
                return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Data = result, Message = "成功", Result = 1 });
        }

        /// <summary>
        /// 获取开票列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWriteInvioceList(GetWriteInvioceListReq req)
        {
            InvioceFacade facaed = new InvioceFacade();
            var result = facaed.GetWriteInvioceList(req);
            if (result == null)
            {
                return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Data = result, Message = "成功", Result = 1 });
        }

        /// <summary>
        /// 审核增票资质
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyInvioceQualifica(VerifyInvioceReq req)
        {
            InvioceFacade facaed = new InvioceFacade();
            var result = facaed.VerifyInvioceQualifica(req);
            if (!result)
            {
                return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Data = result, Message = "成功", Result = 1 });
        }

        /// <summary>
        /// 开票审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyWriteInvioce(VerifyWriteInvioceReq req)
        {
            InvioceFacade facaed = new InvioceFacade();
            var result = facaed.VerifyWriteInvioce(req);
            if (!result)
            {
                return Json(new { Data = result, Message = facaed.PromptInfo.Message, Result = -1 });
            }
            return Json(new { Data = result, Message = "成功", Result = 1 });
        }
        
    }
}