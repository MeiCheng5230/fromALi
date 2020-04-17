using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Common.Facade.Models;
using Common.Mvc;
using PXin.DB;

namespace PXin.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class GoController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Index()
        {
            try
            {
                NameValueCollection nameValues = new NameValueCollection(HttpContext.Request.QueryString);
                int id = 0;
                int.TryParse(nameValues["id"], out id);
                nameValues.Remove("id");
                string jumpUrl = "";
                using (var db = new PXinContext())
                {
                    var tnetUrlJump = db.TnetUrlJumpSet.FirstOrDefault(f => f.Id == id);
                    if (tnetUrlJump == null)
                    {
                        return Json(new Respbase() { Result = -1, Message = "ID:" + id + "-无跳转链接" });
                    }
                    jumpUrl = tnetUrlJump.Rule;
                    if (jumpUrl.Contains("{domain}"))
                    {
                        jumpUrl = jumpUrl.Replace("{domain}", "localhost".Equals(HttpContext.Request.Url.Host, StringComparison.OrdinalIgnoreCase) ? "mall2.ckv-test.sulink.cn" : HttpContext.Request.Url.Host);
                    }
                    if (jumpUrl.Contains("{sign}"))
                    {
                        string queryString = "";
                        foreach (var key in nameValues.AllKeys)
                        {
                            queryString += "&" + key + "=" + nameValues[key];
                        }
                        jumpUrl = jumpUrl.Replace("{sign}", queryString.Substring(1));
                    }
                }
                return Redirect(jumpUrl);
            }
            catch (Exception)
            {
                return Redirect("http://.global.xiang-xin.net/err.html?type=1&msg=系统异常,请稍后再试");
            }
        }
    }
}
