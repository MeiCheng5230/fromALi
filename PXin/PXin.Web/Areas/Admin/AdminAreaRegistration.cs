using Common.Mvc;
using System.Web.Mvc;
using System.Web.Routing;

namespace PXin.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            Route route = new Route(
                    "Admin/{controller}/{action}/{id}",
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
            route.DataTokens["Namespaces"] = new string[] { "PCN.Web.Areas.Admin.*" };
            route.DataTokens["UseNamespaceFallback"] = true;
            context.Routes.Add("Admin_default", route);
        }
    }
}