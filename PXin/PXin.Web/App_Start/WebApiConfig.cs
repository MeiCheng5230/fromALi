using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Common.Facade;
using Common.Mvc;
using Common.Mvc.Filter;

namespace PXin.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            //JSON格式化设置,只格化输入参数
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new JsonWinner.ContractResolverPropertyNameLower();
            //去掉XML格式化器
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            //WEB API BUG
            config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());

            config.Filters.Add(new ApiException());

            //增加安全认证过滤器
            config.Filters.Add(new ApiSignCheck());
            //路由配置
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
    /// <summary>
    /// api bug : System.OperationCanceledException 异常处理
    /// </summary>
    class CancelledTaskBugWorkaroundMessageHandler : DelegatingHandler
    {
        static Log log = new Log("CancelledTaskBugWorkaroundMessageHandler");
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            // Try to suppress response content when the cancellation token has fired; ASP.NET will log to the Application event log if there's content in this case.
            if (cancellationToken.IsCancellationRequested)
            {
                log.Info("取消任务:" + request?.RequestUri?.ToString());
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            return response;
        }
    }
}
