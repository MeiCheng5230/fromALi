using Common.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.Controllers
{
    public class AdminBaseController : Controller
    {
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            Helper.ClearDbAndTransfer();
        }
    }
}