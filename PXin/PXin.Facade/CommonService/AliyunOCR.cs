using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public class AliyunOCR
    {
        private const String host = "https://dm-51.data.aliyun.com";
        private const String path = "/rest/160601/ocr/ocr_idcard.json";
        private const String method = "POST";
        private const String appcode = "eaad795f918b4962988438af430fecc3";

        /// <summary>
        /// 使用阿里云识别身份证图片
        /// </summary>
        /// <param name="picturepath">图片路径</param>
        /// <param name="type">1:正面 2:反面</param>
        /// <returns>json结果</returns>
        public static string AliyunIdentityOCR(string picturepath, int type)
        {
            String querys = "";
            string retype;
            if (type == 1)
            {
                retype = "face";
            }
            else
            {
                retype = "back";
            }
            string base64str = Convert.ToBase64String(File.ReadAllBytes(picturepath));
            String bodys = "{\"inputs\": [{\"image\": {\"dataType\": 50,\"dataValue\": \"" + base64str + "\"},\"configure\": {\"dataType\": 50,\"dataValue\": \"{\\\"side\\\":\\\"" + retype + "\\\"}\"}} ]}";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }
            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/json; charset=UTF-8";
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
