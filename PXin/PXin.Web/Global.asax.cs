using Common.Facade;
using Common.Mvc;
using PXin.DB;
using PXin.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Winner.CU.Balance.GlobalCurrency;

namespace PXin.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    /// <summary>
    /// 
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        static Log log = new Log("MvcApplication");
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            //log4net.Config.XmlConfigurator.Configure();
            log.Info("Pxin Start");
            AssemblyConfig.InitWebAssembly();

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CurrencyConfig.Register();

            var TimeServiceStart = System.Configuration.ConfigurationManager.AppSettings["TimeServiceStart"];
            if(string.IsNullOrEmpty(TimeServiceStart) || TimeServiceStart == "1")
            {
                PxinSerivce.Register();
            }
        }
        /// <summary>
        ///       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            
            //string logStr = string.Empty;
            //Exception ex = this.Server.GetLastError();
            //if (ex.InnerException != null)
            //    ex = ex.InnerException;
            //log.Error("{0}", ex.ToString());
            //Server.ClearError();
        }
    }
}
