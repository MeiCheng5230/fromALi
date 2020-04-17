using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Newtonsoft.Json;

namespace Common.Mvc.Filter
{
  /// <summary>
  /// API过滤器
  /// </summary>
  public class ApiSignCheck : ActionFilterAttribute
  {

    private Log log = new Log(typeof(ApiSignCheck));
    private const string Key = "__action_duration__";
    /// <summary>
    /// 
    /// </summary>

    public ApiSignCheck()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionContext"></param>
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      //记录请求地址和请求参数
      //var stopWatch = new Stopwatch();
      //actionContext.Request.Properties[Key] = stopWatch;
      //stopWatch.Start();
      //log.Info($"ReqUrl={HttpContext.Current.Request.Url.ToString()}；ReqContent={ Helper.GetRequestContent()} ");
      AuthSignature(actionContext);
      base.OnActionExecuting(actionContext);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionExecutedContext"></param>
    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    {
      //if (!actionExecutedContext.Request.Properties.ContainsKey(Key))
      //{
      //  return;
      //}
      //var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
      //if (stopWatch == null)
      //{
      //  return;
      //}
      //stopWatch.Stop();
      //if (actionExecutedContext.Response != null)
      //{
      //  var result = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
      //  log.Info($"ResUrl={actionExecutedContext.Request.RequestUri.ToString()}；耗时：{stopWatch.ElapsedMilliseconds}；ResContent={result}");
      //}
      base.OnActionExecuted(actionExecutedContext);
      Helper.ClearDbAndTransfer();
    }

    private void AuthSignature(HttpActionContext actionContext)
    {
      if (actionContext.ActionDescriptor.GetCustomAttributes<AnonymousAttribute>().Count() > 0
          || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AnonymousAttribute>().Count() > 0)
      {
        return;
      }
      //log.Info("===============ReqUrl=" + HttpContext.Current.Request.Url.ToString());
      //log.Info("===============ReqContent=" + Helper.GetRequestContent());
      //#pragma warning disable IDE0019 // 使用模式匹配
      Reqbase req = actionContext.ActionArguments.First().Value as Reqbase;
      //#pragma warning restore IDE0019 // 使用模式匹配
      if (req == null)
      {
        WriteErrLog("参数匹配失败", "");
        actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = -1, Message = "参数匹配错误" })) };
        return;
      }
      if (!actionContext.ModelState.IsValid)
      {
        string modelStateMsg = GetModelError(actionContext.ModelState.Values.SelectMany(c => c.Errors));
        WriteErrLog("参数验证错误", modelStateMsg);
        actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = -1, Message = "参数验证错误" })) };
        return;
      }

      var unLogin = actionContext.ActionDescriptor.GetCustomAttributes<UnLoginAttribute>().FirstOrDefault();//如果没有UnLoginAttribute特性标记则校验用户
      TnetReginfo regInfo = null;
      if (unLogin == null)
      {
        regInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
        if (regInfo == null)
        {
          WriteErrLog("用户不存在", $"nodeid={req.Nodeid}");
          actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = -1, Message = "用户不存在" })) };
          return;
        }
      }
      if (!CommonConfig.SignValidationDisabled)
      {
        //DvUZIrmKXs
        if (!req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, CommonConfig.ApiAuthString), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, "DvUZIrmKXs"), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, CommonConfig.CasAuthString), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, ConfigurationManager.AppSettings["PcnAuthString"]), StringComparison.OrdinalIgnoreCase))
        {
          WriteErrLog("参数签名错误", JsonConvert.SerializeObject(req));
          actionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = -1, Message = "参数签名错误" })) };
          return;
        }
      }

      if (regInfo != null)
      {
        HttpContext.Current.Items.Add("CurrentUser", regInfo);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelErrors"></param>
    /// <returns></returns>
    private string GetModelError(IEnumerable<ModelError> modelErrors)
    {
      string result = string.Empty;
      foreach (var error in modelErrors)
      {
        result += error.ErrorMessage + Environment.NewLine;
      }
      return result;
    }

    /// <summary>
    /// 写错误日志
    /// </summary>
    /// <param name="title"></param>
    /// <param name="msg"></param>
    private void WriteErrLog(string title, string msg)
    {
      //log.Info($"{title} ReqUrl={HttpContext.Current.Request.Url.ToString()}");
      //log.Info($"{title} ReqContent=" + Helper.GetRequestContent());
      log.Info($"{title} Info=" + msg);
    }
  }
  /// <summary>
  /// 允许未登录状态跳过身份校验
  /// </summary>
  public class UnLoginAttribute : Attribute
  {

  }

  /// <summary>
  /// 
  /// </summary>
  public class ApiException : ExceptionFilterAttribute
  {
    private Log log = new Log(typeof(ApiSignCheck));
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionExecutedContext"></param>
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
      log.Error("Excetion ReqUrl=" + actionExecutedContext.Request.RequestUri.ToString());
      //log.Error("Excetion ReqContent=" + Helper.GetRequestContent());
      log.Error("Excetion Info=" + actionExecutedContext.Exception.ToString());
      Helper.ClearDbAndTransfer();
      if (actionExecutedContext.Exception is ArgumentException)
      {
        actionExecutedContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = 0, Message = actionExecutedContext.Exception.Message })) };
      }
      else
      {
        actionExecutedContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Content = new StringContent(JsonWinner.SerializeObjectPropertyNameLower(new Respbase { Result = 0, Message = "网络繁忙,请稍后重试" })) };
      }
      base.OnException(actionExecutedContext);
    }
  }
}
