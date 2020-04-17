using Common.Mvc;
using System.Web.Mvc;
using System.Web.Routing;

namespace PXin.Web.Areas.DaRenAdmin
{
    public class DaRenAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DaRenAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            Route route = new Route(
                "DaRenAdmin/{controller}/{action}/{id}",
                new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }),
                new MainRouteHandler());
            if (route.DataTokens == null)
            {
                route.DataTokens = new RouteValueDictionary();
            }
            route.DataTokens["area"] = this.AreaName;
            route.DataTokens["Namespaces"] = new string[] { "PXin.Web.Areas.DaRenAdmin.*" };
            route.DataTokens["UseNamespaceFallback"] = true;
            context.Routes.Add("DaRenAdmin", route);

            //context.MapRoute(
            //    "DaRenAdmin_default",
            //    "DaRenAdmin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}