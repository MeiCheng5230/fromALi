using PXin.Facade.ApiFacade;
using PXin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.Areas.Admin.Controllers
{
    public class OctoberActivityController : AdminBaseController
    {
        public ActionResult Index(string Nodename, string Nodecode, string Mobile, int TypeId = 0, int Status = 0, int page = 1, int pageSize = 10)
        {
            ActivityFacade facade = new ActivityFacade();
            var result = facade.GetReceiveList2(Nodename, Nodecode, Mobile, TypeId, Status, page, pageSize);
            ViewBag.Dtos = result;
            return View();
        }

        /// <summary>
        /// 发货
        /// </summary>
        public ActionResult Express(int id, string expressNo)
        {
            ActivityFacade facade = new ActivityFacade();
            var result = facade.Express(id, expressNo);
            return Json(result);
        }

        /// <summary>
        /// 详情
        /// </summary>
        public ActionResult Detail(int id)
        {
            ActivityFacade facade = new ActivityFacade();
            var detail = facade.GetDetail(id);
            if (detail == null)
            {
                return Content("无id=" + id + "的活动详情");
            }
            ViewBag.Detail = detail;
            if (!string.IsNullOrWhiteSpace(detail.Expressno))
            {
                ViewBag.ExpressDetail = facade.GetExpressInfo(detail.Expressno);
            }
            return View();
        }
    }
}