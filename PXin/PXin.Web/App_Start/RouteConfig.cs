using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PXin.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add("IM",
                new Route(
                    "IM/Index",
                    new RouteValueDictionary(new
                    {
                        controller = "IM",
                        action = "Index"
                    }),
                    new MainRouteHandler())
            );
            routes.Add("Default",
                new Route(
                    "{controller}/{action}/{id}",
                    new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    }),
                    new MvcRouteHandler())
            );
        }
    }
}