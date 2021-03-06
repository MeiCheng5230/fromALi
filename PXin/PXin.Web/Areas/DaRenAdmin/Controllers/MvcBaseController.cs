﻿using PXin.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PXin.Web.Areas.DaRenAdmin.Controllers
{
    [AdminMvcFilter]
    public class MvcBaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new ToJsonResult
            {
                Data = data,
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }
        protected JsonResult MyJson(object data)
        {
            return new ToJsonResult
            {
                Data = data,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }


        /// <summary>
        /// 说明：转化为Jason
        /// 作者: CallmeYhz
        /// </summary>
        public class ToJsonResult : JsonResult
        {
            const string error = "该请求已被封锁，因为敏感信息透露给第三方网站，这是一个GET请求时使用的。为了可以GET请求，请设置JsonRequestBehavior AllowGet。";
            /// <summary>
            /// 格式化字符串
            /// </summary>
            public string FormateStr
            {
                get;
                set;
            }
            /// <summary>
            /// 说明：重写ExecueResult方法
            /// 作者：CallmeYhz    
            /// </summary>
            /// <param name="context"></param>
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(error);
                }
                HttpResponseBase response = context.HttpContext.Response;

                if (!String.IsNullOrEmpty(ContentType))
                {
                    response.ContentType = ContentType;
                }
                else
                {
                    response.ContentType = "application/json";
                }
                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }
                if (Data != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string jsonstring = serializer.Serialize(Data);
                    //string hashOldPassword = @"\\/Date\((\param+)\+\param+\)\\/";
                    string p = @"\\/Date\(-{0,1}\d+\)\\/";
                    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                    Regex reg = new Regex(p);
                    jsonstring = reg.Replace(jsonstring, matchEvaluator);
                    response.Write(jsonstring);
                }
            }

            /// <summary>
            /// 说明：将Json序列化的时间由/Date(1294499956278+0800)转为字符串
            /// 作者：CallmeYhz   
            /// </summary>
            private string ConvertJsonDateToDateString(Match m)
            {
                string result = string.Empty;
                string p = @"\d";
                var cArray = m.Value.ToCharArray();
                StringBuilder sb = new StringBuilder();
                Regex reg = new Regex(p);
                for (int i = 0; i < cArray.Length; i++)
                {
                    if (reg.IsMatch(cArray[i].ToString()))
                    {
                        sb.Append(cArray[i]);
                    }
                }
                // reg.Replace(m.Value;
                DateTime dt = new DateTime(1970, 1, 1);
                if (m.ToString().IndexOf('-') > 0)
                {
                    dt = dt.AddMilliseconds(-long.Parse(sb.ToString()));
                }
                else
                {
                    dt = dt.AddMilliseconds(long.Parse(sb.ToString()));
                }

                dt = dt.ToLocalTime();
                result = dt.ToString(this.FormateStr);
                return result;
            }
        }
    }
}