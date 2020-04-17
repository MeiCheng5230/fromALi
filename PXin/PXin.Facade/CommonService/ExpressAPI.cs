using Common;
using Common.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace PXin.Facade.CommonService
{
    public static class ExpressAPI
    {
        private static HttpClient httpClient = new HttpClient();

        static ExpressAPI()
        {
            ServicePointManager.DefaultConnectionLimit = 5;
            httpClient.Timeout = new TimeSpan(0, 0, 30);
        }
        private static object objSync = new object();
        private static Log log = new Log(typeof(ExpressAPI));
        private static DateTime currDateTime = DateTime.Now.AddSeconds(-1);
        private static string KuaiDiAPPKey = AppConfig.KuaiDiAPPKey;        
        private static string KuaiDiAPPSecret = AppConfig.KuaiDiAPPSecret; 
        private static string KuaiDiAPPCode = AppConfig.KuaiDiAPPCode;  

        /// <summary>
        /// 调用接口频率控制，1秒不超过2次
        /// </summary>
        private static void QPSControl()
        {
            int diff = (int)((TimeSpan)(DateTime.Now - currDateTime)).TotalMilliseconds;
            if (diff < 500)
            {
                Thread.Sleep(500 - diff);
            }
        }

        /// <summary>
        /// 获取快递公司名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //public static JArray ComSearch(string req)
        //{
        //    string url = string.Format(@"http://wuliu.market.alicloudapi.com/getExpressList?num={0}&key={1}", req, kuaidi100key);
        //    return BusinessPost1(url);
        //}

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static ExpressResp2 ExpressSearch(Req req)
        {
            string url = string.Format(@"https://goexpress.market.alicloudapi.com/goexpress?no={0}&type={1}", req.num, req.com);
            return BusinessPost<ExpressResp2>(url);
        }


        public static ExpressResp test(Req req)
        {
            string url = string.Format(@"https://www.kuaidi100.com/query?type={0}&postid={1}&id=&valicode=&temp=&phone=", req.com,req.num);
            return JsonConvert.DeserializeObject<ExpressResp>(GetResponse(url));
        }
        

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static T1 BusinessPost<T1>(string url)
            where T1 : RespBase
        {
            string resp = GetResponse(url);
            if (string.IsNullOrEmpty(resp))
            {
                return null;
            }
            T1 result = JsonConvert.DeserializeObject<T1>(resp);
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static JArray BusinessPost1(string url)
        {
            string resp = GetResponse(url);
            if (string.IsNullOrEmpty(resp))
            {
                return null;
            }
            JArray result = (JArray)JsonConvert.DeserializeObject(resp);
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            lock (objSync)
            {
                QPSControl();

                log.Info("Req-Url=" + url);
                //log.Info("Req-Post=" + content);
                if (url.StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                try
                {
                    //HttpClient httpClient = new HttpClient();

                    currDateTime = DateTime.Now;
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "Get";
                    request.Headers.Add("Authorization", "APPCODE " + KuaiDiAPPCode);
                    //request.Headers.Add("X-Ca-Request-Mode", "debug");
                    //request.Headers.Add("X-Ca-Key", KuaiDiAPPKey);
                    //request.Headers.Add("X-Ca-Stage", "RELEASE");
                    //request.Headers.Add("x-ca-nonce", Guid.NewGuid().ToString("N"));
                    //request.Headers.Add("X-Ca-Timestamp", GetTimeStamp());
                    //request.Headers.Add("gateway_channel", "http");
                    //request.Headers.Add("X-Ca-Signature", GetSign());
                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(stream, encode);
                    string detail = reader.ReadToEnd();
                    return detail;
                }
                catch (Exception err)
                {
                    currDateTime = DateTime.Now;
                    log.Info(err.ToString());
                }
                return string.Empty;
            }
        }

        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 加密签名
        /// </summary>
        /// <returns></returns>
        public static string GetSign()
        {
            KuaiDiAPPSecret = KuaiDiAPPSecret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(KuaiDiAPPSecret);
            byte[] messageBytes = encoding.GetBytes(KuaiDiAPPSecret);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        public class Req
        {
            public Req()
            {
                phone = "";
            }
            public string com { get; set; }
            public string num { get; set; }
            public string phone { get; set; }

        }

        public class RespBase
        {

        }

        public class ComList : RespBase
        {
            public List<ExpressCom> list { get; set; }
        }


        public class ExpressCom
        {
            public string comCode { get; set; }
            public string id { get; set; }
            public string noCount { get; set; }
            public string noPre { get; set; }
            public string startTime { get; set; }
        }

        public class ExpressResp : RespBase
        {
            public string msg { get; set; }
            public string status { get; set; }
            public ExpressData result { get; set; }
            public string picurl { get; set; }
        }

        public class ExpressData
        {
            public string number { get; set; }
            public string type { get; set; }
            public List<RouteInfo> list { get; set; }
            public string deliverystatus { get; set; }
            public string issign { get; set; }
            public string expName { get; set; }
            public string expSite { get; set; }
            public string expPhone { get; set; }
            public string courier { get; set; }
            public string courierPhone { get; set; }
        }

        public class RouteInfo
        {
            public string time { get; set; }
            public string status { get; set; }
        }
        public class ExpressResp2: RespBase
        {
            /// <summary>
            /// 成功则返回“OK”
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 快递单号
            /// </summary>
            public string no { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 快递记录
            /// </summary>
            public ExpressItem2[] list { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// 消息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 快递名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 快递网址
            /// </summary>
            public string site { get; set; }
            /// <summary>
            /// 快递联系方式
            /// </summary>
            public string phone { get; set; }
            /// <summary>
            /// 快递Logo地址
            /// </summary>
            public string logo { get; set; }
        }

        public class ExpressItem2
        {
            /// <summary>
            /// 快递记录信息
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 时间
            /// </summary>
            public string time { get; set; }
        }
    }
}
