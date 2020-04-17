using Common.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 扫描证件
    /// </summary>
    public class ScanLicense
    {
        private static IDictionary<LicenseTypeEnum, ScanLicense> Instances = new Dictionary<LicenseTypeEnum, ScanLicense>();
        private static object LOCK_CREATEINSTANCE = new object();
        private static object LOCK_CREATETOKEN = new object();
        private readonly Log log = new Log(typeof(ScanLicense));
        /// <summary>
        /// 扫描证件的API地址
        /// </summary>
        private string SCANLICENSEURL = "";
        /// <summary>
        /// APIToken获取地址
        /// </summary>
        private string APITOKENURL = "";
        /// <summary>
        /// 上次调用API时间
        /// </summary>
        private DateTime currDateTime = DateTime.Now.AddSeconds(-1);
        /// <summary>
        /// token有效时间
        /// </summary>
        private DateTime tokenTime = DateTime.Now.AddDays(-30);
        /// <summary>
        /// 证件类型
        /// </summary>
        private LicenseTypeEnum LicenseType { get; }
        private string ClientID { get; set; }
        private string ClientSecret { get; set; }
        private string ApiTokenJson { get; set; }
        private ScanLicense(LicenseTypeEnum licenseType)
        {
            LicenseType = licenseType;
            AssignValueToProperty();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ScanLicense GetInstance(LicenseTypeEnum type)
        {
            if (Instances.ContainsKey(type))
            {
                if (Instances[type] == null)
                {
                    lock (LOCK_CREATEINSTANCE)
                    {
                        if (Instances[type] == null)
                        {
                            Instances[type] = new ScanLicense(type);
                        }
                    }
                }
                return Instances[type];
            }
            else
            {
                lock (LOCK_CREATEINSTANCE)
                {
                    if (!Instances.ContainsKey(type))
                    {
                        Instances.Add(type, new ScanLicense(type));
                    }
                }
                return Instances[type];
            }
        }

        /// <summary>
        /// 扫描证件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>(是否识别成功,错误消息,识别结果)</returns>
        public (bool, string, object) Execute(ExecuteEntity entity)
        {
            if (LicenseType == LicenseTypeEnum.Driver)
            {
                return Execute_Driver(entity);
            }
            return (false, "扫描失败", null);
        }
        /// <summary>
        /// api必填参数赋值
        /// </summary>
        private void AssignValueToProperty()
        {
            if (LicenseType == LicenseTypeEnum.Driver)
            {
                ClientID = AppConfig.BaiduAPIKey1;   // "wOfocQGDmC6q1NSXNqefKwaG";  
                ClientSecret = AppConfig.BaiduSecretKey1;// "XSFRxur3b6zxfTIpM0KY2pwlZLXvGIDm"; 
                SCANLICENSEURL = "https://aip.baidubce.com/rest/2.0/ocr/v1/driving_license";
                APITOKENURL = "https://aip.baidubce.com/oauth/2.0/token";
            }
        }
        /// <summary>
        /// 获取调用api的token
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="tokenReq"></param>
        /// <returns></returns>
        private TResult GetAccessToken<TResult>(dynamic tokenReq)
        {
            string token = ApiTokenJson;
            if (string.IsNullOrEmpty(token) || tokenTime.AddDays(1) < DateTime.Now)
            {
                TResult tokenResult = GetAccessTokenInner<TResult>(tokenReq);
                if (tokenResult == null)
                {
                    token = null;
                }
                else
                {
                    token = JsonConvert.SerializeObject(tokenResult);
                }
                ApiTokenJson = token;
                tokenTime = DateTime.Now;
                return tokenResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<TResult>(token);
            }
        }
        /// <summary>
        /// 获取调用api token
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="tokenReq"></param>
        /// <returns></returns>
        private TResult GetAccessTokenInner<TResult>(dynamic tokenReq)
        {
            lock (LOCK_CREATETOKEN)
            {
                QPSControl();
                log.Info("LicenseType:" + LicenseType.ToString() + "——Url:" + APITOKENURL + "，Param:" + JsonConvert.SerializeObject(tokenReq));
                string result = PostWebRequest(APITOKENURL, tokenReq);
                if (string.IsNullOrEmpty(result))
                {
                    log.Info("获取APIToken接口调用失败");
                    return default;
                }
                return JsonConvert.DeserializeObject<TResult>(result);
            }
        }

        private string PostWebRequest<TReqEntity>(string url, TReqEntity reqEntity)
        {
            currDateTime = DateTime.Now;
            try
            {
                return Common.Mvc.HttpHelper.HttpSimulation.Instance.RequestByJsonOrQueryString(url, reqEntity, Common.Mvc.HttpHelper.RequestType.POST, false);
            }
            catch (Exception)
            {
                log.Error("Api调用失败:" + "url(" + url + ")" + "，Querty(" + JsonConvert.SerializeObject(reqEntity) + ")");
                return null;
            }

        }
        /// <summary>
        /// 识别驾驶证
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private (bool, string, DriverResp) Execute_Driver(ExecuteEntity entity)
        {
            string imageBase64 = GetImageBase64(entity.PhotoPhysicalPath);
            DriverReq req = new DriverReq
            {
                image = imageBase64,
                detect_direction = "true"
            };
            req.access_token = GetAccessToken<AccessTokenResult>(new { grant_type = "client_credentials", client_id = ClientID, client_secret = ClientSecret })?.access_token;
            string resultStr = PostWebRequest(SCANLICENSEURL, req);
            if (string.IsNullOrEmpty(resultStr))
            {
                return (false, "识别驾驶证错误", null);
            }
            DriverResp resp = JsonConvert.DeserializeObject<DriverResp>(resultStr);
            if (!string.IsNullOrEmpty(resp.error_code))
            {
                return (false, "识别驾驶证错误", null);
            }
            if (resp.words_result == null || string.IsNullOrEmpty(resp.words_result.证号.words) || string.IsNullOrEmpty(resp.words_result.准驾车型.words) || string.IsNullOrEmpty(resp.words_result.姓名.words))
            {
                return (false, "识别驾驶证错误", null);
            }
            return (true, "", resp);
        }

        /// <summary>
        /// 将服务器本地图片转为Base64格式
        /// </summary>
        /// <param name="photoPhysicalPath">绝对路径</param>
        /// <returns></returns>
        private string GetImageBase64(string photoPhysicalPath)
        {
            if (!File.Exists(photoPhysicalPath))
            {
                return string.Empty;
            }
            string base64String = string.Empty;
            Bitmap bmp1 = new Bitmap(photoPhysicalPath);
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

        /// <summary>
        /// 调用百度接口频率控制，1秒不超过2次
        /// </summary>
        private void QPSControl()
        {
            int diff = (int)((TimeSpan)(DateTime.Now - currDateTime)).TotalMilliseconds;
            if (diff < 500)
            {
                Thread.Sleep(500 - diff);
            }
        }

    }

    public class ExecuteEntity
    {
        public string PhotoPhysicalPath { get; set; }
    }

    /// <summary>
    /// 证件类型
    /// </summary>
    public enum LicenseTypeEnum
    {
        /// <summary>
        /// 驾驶证
        /// </summary>
        Driver
    }

    #region 驾驶证请求返回参数
    public class DriverReq
    {
        public string image { get; set; }
        public string detect_direction { get; set; }
        public string access_token { get; set; }
        public bool unified_valid_period { get; set; }
    }

    public class DriverResp
    {
        public string log_id { get; set; }
        public int words_result_num { get; set; }
        public DriverResult words_result { get; set; }
        public string error_code { get; set; }
        public string error_msg { get; set; }
    }

    public class DriverResult
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
    #region DriverResultClass
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
    public class 住址
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
    #endregion
}
