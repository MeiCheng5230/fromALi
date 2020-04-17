using Common.Facade;
using Common.Mvc;
using Common.Mvc.Models;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.CommonService
{
    public class BaiduLicense : FacadeBase<PXinContext>
    {
        private static HttpClient httpClient = new HttpClient();

        static BaiduLicense()
        {
            ServicePointManager.DefaultConnectionLimit = 5;
            httpClient.Timeout = new TimeSpan(0, 0, 30);
        }
        private static Log log = new Log(typeof(BaiduLicense));
        private static object objSync = new object();
        private static DateTime currDateTime = DateTime.Now.AddSeconds(-1);
        private static DateTime tokenTime = DateTime.Now.AddDays(-30);
        private static string clientId = AppConfig.BaiduAPIKey1;   // "wOfocQGDmC6q1NSXNqefKwaG";        
        private static string clientSecret = AppConfig.BaiduSecretKey1;// "XSFRxur3b6zxfTIpM0KY2pwlZLXvGIDm"; 
        /// <summary>
        /// 调用百度接口频率控制，1秒不超过2次
        /// </summary>
        private static void QPSControl()
        {
            int diff = (int)((TimeSpan)(DateTime.Now - currDateTime)).TotalMilliseconds;
            if (diff < 500)
            {
                Thread.Sleep(500 - diff);
            }
        }

        #region 行驶证驾驶证识别业务

        /// <summary>
        /// 识别行驶证
        /// </summary>
        public bool VehicleLicense(TnetReginfo regInfo, string vehiclelicenseimg)
        {
            TnetVehicleLicLog vehicleLicLog = db.TnetVehicleLicLogSet.FirstOrDefault(x => x.Nodeid == regInfo.Nodeid);
            if (vehicleLicLog != null)
            {
                if (vehiclelicenseimg == vehicleLicLog.Cardimg)
                {
                    Alert("修改行驶证成功");
                    return true;
                }
                if (vehicleLicLog.Status == 1)
                {
                    Alert("您的行驶证不能修改");
                    return false;
                }
            }
            string filePath1 = HttpContext.Current.Server.MapPath(vehiclelicenseimg);
            string img = HttpUtility.UrlEncode(GetImageBase64(filePath1));
            BaiduLicense.VehicleReq req = new BaiduLicense.VehicleReq
            {
                image = img,
                detect_direction = "true"
            };
            BaiduLicense.VehicleResp resp = null;
            try
            {
                //调用百度接口识别行驶证
                resp = BaiduLicense.VehicleSearch(req);
                if (!string.IsNullOrEmpty(resp.error_code))
                {
                    Alert("识别行驶证错误");
                    return false;
                }
                if (string.IsNullOrEmpty(resp.words_result.品牌型号.words) || string.IsNullOrEmpty(resp.words_result.发动机号码.words) || string.IsNullOrEmpty(resp.words_result.号牌号码.words))
                {
                    Alert("识别行驶证错误");
                    return false;
                }
                TnetVehicleLicLog vehicleLicLog1 = db.TnetVehicleLicLogSet.FirstOrDefault(x => x.Licplateno == resp.words_result.号牌号码.words);
                if (vehicleLicLog1 != null)
                {
                    Alert("该行驶证已被使用");
                    return false;
                }

                //TnetNodeinfo tnet_Nodeinfo = db.TnetNodeinfoSet.FirstOrDefault(x => x.Nodeid == regInfo.Nodeid);
                //if (tnet_Nodeinfo == null)
                //{
                //    Alert("您不是认证用户请先认证");
                //    return false;
                //}
                //if (tnet_Nodeinfo.NAME.Trim() != resp.words_result.所有人.words.Trim())
                //{
                //    Alert("您上传的行驶证所有人【" + resp.words_result.所有人.words + "】与认证用户名字【" + tnet_Nodeinfo.NAME + "】不一致");
                //    return false;
                //}

                if (vehicleLicLog == null)
                {
                    vehicleLicLog = new TnetVehicleLicLog();
                }
                //识别成功添加到行驶证识别表
                vehicleLicLog.Belonger = resp.words_result.所有人.words;
                vehicleLicLog.Address = resp.words_result.住址 == null ? "" : resp.words_result.住址.words;
                vehicleLicLog.Brandmodel = resp.words_result.品牌型号 == null ? "" : resp.words_result.品牌型号.words;
                vehicleLicLog.Cardimg = vehiclelicenseimg;
                vehicleLicLog.Carliccode = resp.words_result.车辆识别代号 == null ? "" : resp.words_result.车辆识别代号.words;
                vehicleLicLog.Createtime = DateTime.Now;
                vehicleLicLog.Cartype = resp.words_result.车辆类型 == null ? "" : resp.words_result.车辆类型.words;
                vehicleLicLog.Engineno = resp.words_result.发动机号码 == null ? "" : resp.words_result.发动机号码.words;
                vehicleLicLog.Firtdate = resp.words_result.发证日期 == null ? "" : resp.words_result.发证日期.words;
                vehicleLicLog.Registertime = resp.words_result.注册日期 == null ? "" : resp.words_result.注册日期.words;
                vehicleLicLog.Licplateno = resp.words_result.号牌号码 == null ? "" : resp.words_result.号牌号码.words;
                vehicleLicLog.Usenature = resp.words_result.使用性质 == null ? "" : resp.words_result.使用性质.words;
                vehicleLicLog.Nodeid = regInfo.Nodeid;
                vehicleLicLog.Remarks = "";
                vehicleLicLog.Status = 1;

                if (vehicleLicLog.Id > 0)
                {
                    ////修改
                    //if (!vehicleLicLog.Update())
                    //{
                    //    log.Info("修改行驶证失败，原因：" + vehicleLicLog.DebugInfo.SqlExecutable);
                    //    Alert("修改行驶证失败");
                    //    return false;
                    //}
                }
                else
                {
                    //添加
                    db.TnetVehicleLicLogSet.Add(vehicleLicLog);
                    //if (!vehicleLicLog.Insert())
                    //{
                    //    log.Info("添加行驶证失败，原因：" + vehicleLicLog.DebugInfo.SqlExecutable);
                    //    Alert("添加行驶证失败");
                    //    return false;
                    //}
                }
                if (db.SaveChanges() <= 0)
                {
                    Alert("保存行驶证失败");
                    log.Info("保存行驶证失败，原因：" + db.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Info("行驶证改动异常，原因：" + ex);
                Alert(ex.Message);
                return false;
            }
            Alert("验证行驶证成功");
            return true;
        }

        /// <summary>
        /// 识别驾驶证
        /// </summary>
        public bool DrivLicense(TnetReginfo regInfo, string imageurl, string imageurl2, string fileno)
        {
            TnetDriveLicLog driveLicLog = db.TnetDriveLicLogSet.FirstOrDefault(x => x.Nodeid == regInfo.Nodeid);
            if (driveLicLog != null)
            {
                if (imageurl == driveLicLog.Cardimg && fileno == driveLicLog.Fileno)
                {
                    Alert("修改驾驶证成功");
                    return true;
                }
                if (driveLicLog.Status == 1)
                {
                    Alert("您的驾驶证不能修改");
                    return false;
                }
            }

            string filePath1 = FileService.GetPhysicsFilePath(imageurl);
            string img = HttpUtility.UrlEncode(GetImageBase64(filePath1));
            BaiduLicense.DrivReq req = new BaiduLicense.DrivReq
            {
                image = img,
                detect_direction = "true"
            };
            BaiduLicense.DrivResp resp = null;
            try
            {
                //调用百度接口识别驾驶证
                resp = BaiduLicense.DrivSearch(req);
                if (!string.IsNullOrEmpty(resp.error_code))
                {
                    Alert("识别驾驶证错误");
                    return false;
                }
                if (string.IsNullOrEmpty(resp.words_result.证号.words) || string.IsNullOrEmpty(resp.words_result.准驾车型.words) || string.IsNullOrEmpty(resp.words_result.姓名.words))
                {
                    Alert("识别驾驶证错误");
                    return false;
                }
                TnetDriveLicLog driveLicLog1 = db.TnetDriveLicLogSet.FirstOrDefault(x=>x.Cardno== resp.words_result.证号.words);
                if (driveLicLog1 != null)
                {
                    Alert("该驾驶证已被使用");
                    return false;
                }
                //TnetNodeinfo tnet_Nodeinfo = db.TnetNodeinfoSet.FirstOrDefault(x => x.Nodeid == regInfo.Nodeid);
                //if (tnet_Nodeinfo == null)
                //{
                //    Alert("获取注册信息失败");
                //    return false;
                //}


                //if (tnet_Nodeinfo.NAME.Trim() != resp.words_result.姓名.words.Trim())
                //{
                //    Alert("您上传的驾驶证姓名【" + resp.words_result.姓名.words + "】与认证用户名字【" + tnet_Nodeinfo.NAME + "】不一致");
                //    return false;
                //}

                //if (tnet_Nodeinfo.IDCARDNO.Trim() != resp.words_result.证号.words.Trim())
                //{
                //    Alert("您上传的驾驶证证件号【" + resp.words_result.证号.words + "】与认证用户证件号【" + tnet_Nodeinfo.IDCARDNO + "】不一致");
                //    return false;
                //}
                if (driveLicLog == null)
                {
                    driveLicLog = new TnetDriveLicLog();
                }
                //识别成功添加到驾驶证识别表
                driveLicLog.Name = resp.words_result.姓名.words;
                driveLicLog.Addr = resp.words_result.住址 == null ? "" : resp.words_result.住址.words;
                driveLicLog.Birthday = resp.words_result.出生日期 == null ? "" : resp.words_result.出生日期.words;
                FileService fileService = new FileService();
                DateTime now = DateTime.Now;
                driveLicLog.Cardimg = fileService.CombinePicUrl(imageurl, now, FileActionType.驾驶证行驶证);
                driveLicLog.CardimgAppendix = fileService.CombinePicUrl(imageurl2, now, FileActionType.驾驶证副页);
                driveLicLog.Cardno = resp.words_result.证号 == null ? "" : resp.words_result.证号.words;
                driveLicLog.Country = resp.words_result.国籍 == null ? "" : resp.words_result.国籍.words;
                driveLicLog.Enddate = resp.words_result.至 == null ? "" : resp.words_result.至.words;
                driveLicLog.VehicleType = resp.words_result.准驾车型 == null ? "" : resp.words_result.准驾车型.words;
                driveLicLog.ValidPeriod = resp.words_result.有效期限 == null ? "" : resp.words_result.有效期限.words;
                driveLicLog.Sex = resp.words_result.性别 == null ? "" : resp.words_result.性别.words;
                driveLicLog.Nodeid = regInfo.Nodeid;
                driveLicLog.Firtdate = resp.words_result.初次领证日期 == null ? "" : resp.words_result.初次领证日期.words;
                driveLicLog.Remarks = "";
                driveLicLog.Status = 1;
                driveLicLog.Fileno = fileno;
                driveLicLog.Createtime = DateTime.Now;


                if (driveLicLog.Id > 0)
                {
                    ////修改
                    //if (!driveLicLog.Update())
                    //{
                    //    log.Info("修改驾驶证失败，原因：" + driveLicLog.DebugInfo.SqlExecutable);
                    //    Alert("修改驾驶证失败");
                    //    return false;
                    //}
                }
                else
                {
                    //添加
                    db.TnetDriveLicLogSet.Add(driveLicLog);
                    //if (!driveLicLog.Insert())
                    //{
                    //    log.Info("添加驾驶证失败，原因：" + driveLicLog.DebugInfo.SqlExecutable);
                    //    Alert("添加驾驶证失败");
                    //    return false;
                    //}
                }
                if (db.SaveChanges() <= 0)
                {
                    Alert("保存驾驶证失败");
                    log.Info("保存驾驶证失败，原因：" + db.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
            Alert("识别驾驶证成功");
            return true;

        }

        private string GetImageBase64(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return string.Empty;
            }
            string base64String = string.Empty;
            Bitmap bmp1 = new Bitmap(fileName);
            using (MemoryStream ms1 = new MemoryStream())
            {
                bmp1.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr1 = new byte[ms1.Length];
                ms1.Position = 0;
                ms1.Read(arr1, 0, (int)ms1.Length);
                ms1.Close();
                base64String = Convert.ToBase64String(arr1);
            }
            return base64String;
        }
        #endregion

        #region 行驶证驾驶证识别post调用百度接口


        /// <summary>
        /// 验证驾驶证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static DrivResp DrivSearch(DrivReq req)
        {
            string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/driving_license";
            return BusinessPost<DrivReq, DrivResp>(url, req);
        }

        /// <summary>
        /// 验证行驶
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static VehicleResp VehicleSearch(VehicleReq req)
        {
            string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/vehicle_license";
            return BusinessPost<VehicleReq, VehicleResp>(url, req);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public static T2 BusinessPost<T1, T2>(string url, T1 req)//
        //where T1 : BusinessRep
        //where T2 : BusinessResp
        {
            string resp = PostWebRequest(url, req);
            if (string.IsNullOrEmpty(resp))
            {
                return default(T2);
            }
            T2 result = JsonConvert.DeserializeObject<T2>(resp);
            return result;
        }
        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        private static string PostWebRequest(string url, object postData)
        {

            lock (objSync)
            {
                QPSControl();
                string content = JsonConvert.SerializeObject(postData).TrimStart('{').TrimEnd('}').Replace(",", "&").Replace(":", "=").Replace("\"", "");
                url += "?access_token=" + GetAccessToken().access_token;

                log.Info("Req-Url=" + url);
                log.Info("Req-Post=" + content);
                if (url.StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                try
                {
                    //FormUrlEncodedContent httpContent = new FormUrlEncodedContent(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(postData)));
                    HttpContent httpContent = new StringContent(content);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    httpContent.Headers.ContentType.CharSet = "utf-8";
                    //HttpClient httpClient = new HttpClient();

                    currDateTime = DateTime.Now;

                    HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                    string result = response.Content.ReadAsStringAsync().Result;
                    log.Info("Resp=" + result);
                    return result;
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
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        public static AccessTokenResult GetAccessToken()
        {
            string token = HttpContext.Current.Application["BaiudAPI_Token1"] as string;
            if (string.IsNullOrEmpty(token) || tokenTime.AddDays(1) < DateTime.Now)
            {
                AccessTokenResult tokenResult = GetAccessTokenInner();
                token = JsonConvert.SerializeObject(tokenResult);
                HttpContext.Current.Application["BaiudAPI_Token1"] = token;
                tokenTime = DateTime.Now;
                return tokenResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<AccessTokenResult>(token);
            }
        }
        private static AccessTokenResult GetAccessTokenInner()
        {
            lock (objSync)
            {
                QPSControl();
                string authHost = "https://aip.baidubce.com/oauth/2.0/token";
                //HttpClient client = new HttpClient();
                List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
                paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
                paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
                try
                {
                    HttpResponseMessage response = httpClient.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
                    string resp = response.Content.ReadAsStringAsync().Result;
                    AccessTokenResult result = JsonConvert.DeserializeObject<AccessTokenResult>(resp);
                    return result;
                }
                catch (Exception exp)
                {
                    log.Info("GetAccessTokenInner:" + exp.ToString());
                }
                return null;
            }
        }
        #endregion

        #region 行驶证请求返回参数


        public class VehicleReq
        {
            /// <summary>
            /// base64编码后进行urlencode，要求base64编码和urlencode后大小不超过4M  jpg/png/bmp
            /// </summary>
            public string image { get; set; }
            /// <summary>
            /// true、false
            /// </summary>
            public string detect_direction { get; set; }
            /// <summary>
            /// normal，缺省
            /// </summary>
            public string accuracy { get; set; }
        }
        public class VehicleResp
        {
            public string log_id { get; set; }
            public string words_result_num { get; set; }
            public Vehicle_Result words_result { get; set; }

            public string error_code { get; set; }
            public string error_msg { get; set; }
        }

        public class Vehicle_Result
        {
            public 品牌型号 品牌型号 { get; set; }
            public 发证日期 发证日期 { get; set; }
            public 使用性质 使用性质 { get; set; }
            public 发动机号码 发动机号码 { get; set; }
            public 号牌号码 号牌号码 { get; set; }
            public 所有人 所有人 { get; set; }
            public 住址 住址 { get; set; }
            public 注册日期 注册日期 { get; set; }
            public 车辆识别代号 车辆识别代号 { get; set; }
            public 车辆类型 车辆类型 { get; set; }
        }

        public class 品牌型号
        {
            public string words { get; set; }
        }

        public class 发证日期
        {
            public string words { get; set; }
        }

        public class 使用性质
        {
            public string words { get; set; }
        }

        public class 发动机号码
        {
            public string words { get; set; }
        }

        public class 号牌号码
        {
            public string words { get; set; }
        }

        public class 所有人
        {
            public string words { get; set; }
        }

        public class 住址
        {
            public string words { get; set; }
        }

        public class 注册日期
        {
            public string words { get; set; }
        }

        public class 车辆识别代号
        {
            public string words { get; set; }
        }

        public class 车辆类型
        {
            public string words { get; set; }
        }
        #endregion
        #region 驾驶证请求返回参数


        public class DrivReq
        {
            public string image { get; set; }
            public string detect_direction { get; set; }
            public bool unified_valid_period { get; set; }

        }

        public class DrivResp
        {
            public string log_id { get; set; }
            public int words_result_num { get; set; }
            public Driv_Result words_result { get; set; }

            public string error_code { get; set; }
            public string error_msg { get; set; }
        }


        public class Driv_Result
        {
            public 证号 证号 { get; set; }
            public 有效期限 有效期限 { get; set; }
            public 准驾车型 准驾车型 { get; set; }
            public 至 至 { get; set; }
            public 住址 住址 { get; set; }
            public 姓名 姓名 { get; set; }
            public 国籍 国籍 { get; set; }
            public 出生日期 出生日期 { get; set; }
            public 性别 性别 { get; set; }
            public 初次领证日期 初次领证日期 { get; set; }
        }

        public class 证号
        {
            public string words { get; set; }
        }

        public class 有效期限
        {
            public string words { get; set; }
        }

        public class 准驾车型
        {
            public string words { get; set; }
        }

        public class 至
        {
            public string words { get; set; }
        }

        public class 姓名
        {
            public string words { get; set; }
        }

        public class 国籍
        {
            public string words { get; set; }
        }

        public class 出生日期
        {
            public string words { get; set; }
        }

        public class 性别
        {
            public string words { get; set; }
        }

        public class 初次领证日期
        {
            public string words { get; set; }
        }
        #endregion
    }

    /// <summary>
    /// AccessToken回复
    /// </summary>
    public class AccessTokenResult
    {
        public string access_token { get; set; }
        public string session_key { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string session_secret { get; set; }
        public int expires_in { get; set; }
    }
}
