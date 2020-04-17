using Common.Mvc;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXin.SignalR
{
    //public class ExceptionModule : IHttpModule
    //{
    //    Log logger = new Log(typeof(ExceptionModule));
    //    public void Init(HttpApplication context)
    //    {
    //        context.Error += new EventHandler(ErrorHandler);
    //    }
    //    private void ErrorHandler(object sender, EventArgs e)
    //    {
    //        HttpContext context = HttpContext.Current;
    //        Exception ex = context.Server.GetLastError();
    //        logger.Error(ex.ToString());
    //        context.Server.ClearError();
    //    }
    //    public void Dispose()
    //    {

    //    }
    //}

    public class ExceptionModule : HubPipelineModule
    {
        Log logger = new Log(typeof(ExceptionModule));
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            logger.Error("=> Exception " + exceptionContext.Error.Message);
            if (exceptionContext.Error.InnerException != null)
            {
                logger.Error("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
            }
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}