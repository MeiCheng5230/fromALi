using Common.Mvc;
using PXin.DB;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PXin.Model;
using Common.Mvc.Models;
using System;
using Common.Facade;
using SwaggerExport;
using System.Net.Http;
using PXin.Facade.ApiFacade;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using Common.Facade.Models;
using System.Text;
using Newtonsoft.Json;

namespace PXin.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        private static Log log = new Log(typeof(HomeController));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult GetParam(int nodeid)
        {
            System.Web.HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            Common.Mvc.Models.TnetReginfo regInfo = Common.Mvc.HttpHelper.CommonApiTransfer.Instance.GetTnetReginfo(new Common.Mvc.HttpHelper.GetRegInfoReq { RegInfoKey = nodeid.ToString() });
            int sid = 81127;
            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sign = Helper.GetSign(nodeid, sid, tm, CommonConfig.ApiAuthString);
            return Content($"nodeid={nodeid}&sid={sid}&tm={tm}&sign={sign}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Index()
        {
            //PXinContext db = new PXinContext();
            //TnetReginfo regInfo = db.TnetReginfoSet.OrderBy(c => Functions.Randomget()).FirstOrDefault();
            //PXinContext ctx = HttpContext.GetDbContext<PXinContext>();
            //ViewBag.UserInfo = ctx.TnetReginfoSet.Where(c => c.Nodeid == 3434909).FirstOrDefault();
            //return View();
            return RedirectToAction("Index", "Test");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult DaRenShare(string nodecode)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenInfo(nodecode);
            ViewBag.data = result;
            return View();
        }
        /// <summary>
        /// 导出swagger
        /// </summary>
        /// <param name="type"></param>
        [HttpGet]
        public void ExportApiWord(string type)
        {

            try
            {
                var url = Request.Url.Host + ":" + Request.Url.Port + "/swagger/docs/v1";
                log.Info(url);
                ApiJsonHandle.ExportSwagger(url, "相信接口文档", type);
            }
            catch (Exception e)
            {
                log.Error(e);
            }

        }

        /// <summary>
        /// 表情包下载
        /// </summary>
        /// <param name="type"></param>
        [HttpGet]
        public void DownloadEmoticon(int nodeid,int id)
        {
            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.DownloadEmoticon(nodeid, id);
            if (result.Item1)
            {
                var s = new FileStream(result.Item3, FileMode.Open);
                var type = System.IO.Path.GetExtension(result.Item3);
                byte[] array = new byte[s.Length];
                s.Read(array, 0, array.Length);
                s.Seek(0L, SeekOrigin.Begin);
                s.Dispose();
                HttpContext.Response.ContentType = "application/octet-stream";
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(result.Item2 + type, System.Text.Encoding.UTF8));
                HttpContext.Response.BinaryWrite(array);
            }
            else
            {
                var data = new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.BinaryWrite(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            }

            HttpContext.Response.Flush();
            HttpContext.Response.End();

        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Register(string r)
        {
            return Redirect(GetRegisterUrl(r));
        }
        private string GetRegisterUrl(string r)
        {
            int sid = 81122;
            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sign = Helper.GetSign(0, sid, tm, CommonConfig.ApiAuthString);
            return $"/App/Register/index.htm?r={r}&nodeid=0&sid={sid}&tm={tm}&sign={sign}";
        }
    }
}
