using Common.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.App_Start
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminMvcFilter : System.Web.Mvc.ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            Helper.ClearDbAndTransfer();
        }
    }
}