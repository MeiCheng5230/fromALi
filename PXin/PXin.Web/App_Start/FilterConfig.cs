using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common.Facade;
using Common.Mvc;

namespace PXin.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MVCExceptionAttribute());
#if !DEBUG
            //filters.Add(new LogExceptionFilterAttribute());
#endif
            //filters.Add(new HandleErrorAttribute());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MVCExceptionAttribute : HandleErrorAttribute
    {
        private Log log = new Log(typeof(MVCExceptionAttribute));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            log.Info("Excetion ReqUrl=" + filterContext.HttpContext.Request.ToString());
            log.Info("Excetion ReqContent=" + Helper.GetRequestContent());
            log.Info("Excetion Info=" + filterContext.Exception.ToString());
            Helper.ClearDbAndTransfer();
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new ContentResult { Content = "网络繁忙,请稍后重试" };
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new JsonResult
                {
                    Data = new { Result = 0, Message = "网络繁忙,请稍后重试" },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet

                };
            }
            filterContext.ExceptionHandled = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LogExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.IsChildAction && !filterContext.ExceptionHandled)
            {
                Log log = new Log("WebExcept");
                Exception innerException = filterContext.Exception;
                log.Debug(innerException.ToString());
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                ViewResult result = new ViewResult
                {
                    ViewName = "Error",
                    MasterName = null,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 300;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }
    }
}