using ApiAuthCore.Auth;
using ApiAuthCore.Utils;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Rong
{
    /// <summary>
    /// 调用融云接口
    /// </summary>
    public static class RongApi
    {
        private static Log log = new Log(typeof(RongApi));
        /// <summary>
        /// 获取融云的好友手机号码
        /// </summary>
        /// <param name="nodecode"></param>
        /// <returns></returns>
        public static PXinResult<string> GetPxinFriend(string nodecode)
        {
            string url = ConfigurationManager.AppSettings["RongUrl"] + "/Rong/GetPxinFriend";
            Dictionary<string, string> immutableMap = BuildMapCommon(nodecode);
            string queryString = ConcatQueryString(immutableMap);
            string result = HttpSimulation.Instance.Request(url, queryString);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            PXinResult<string> dto = null;
            try
            {
                dto = JsonConvert.DeserializeObject<PXinResult<string>>(result);
            }
            catch (Exception err)
            {
                log.Info(err);
            }
            return dto;
        }

        private static Dictionary<string, string> BuildMapCommon(string nodecode)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string nonce = Guid.NewGuid().ToString();
            Dictionary<string, string> immutableMap = new Dictionary<string, string>();
            //公共参数
            DictionaryUtil.Add(immutableMap, "nodecode", nodecode);
            DictionaryUtil.Add(immutableMap, "nonce", nonce);
            DictionaryUtil.Add(immutableMap, "timestamp", timestamp);
            DictionaryUtil.Add(immutableMap, "signature", GetSign(nodecode, nonce, timestamp));
            return immutableMap;
        }

        /// <summary>
        /// 获取融云的签名认证
        /// </summary>
        /// <param name="nodecode"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private static string GetSign(string nodecode, string nonce, string timestamp)
        {
            string signStr = string.Empty;
            foreach (string pwd in AppConfig.ApiKey.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                signStr += GetSign(nodecode, nonce, timestamp, pwd) + ",";
            }
            return signStr.Trim(',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodecode"></param>
        /// <param name="nonce"></param>
        /// <param name="timestamp"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private static string GetSign(string nodecode, string nonce, string timestamp, string pwd)
        {
            string src = nodecode + nonce + timestamp + pwd;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(src));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToLower();
        }

        private static string ConcatQueryString(Dictionary<String, String> immutableMap)
        {
            if (null == immutableMap)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();

            foreach (var entry in immutableMap)
            {
                string key = entry.Key;
                string val = entry.Value;

                sb.Append(AcsURLEncoder.Encode(key));
                if (val != null)
                {
                    sb.Append("=").Append(AcsURLEncoder.Encode(val));
                }
                sb.Append("&");
            }

            int strIndex = sb.Length;
            if (immutableMap.Count > 0)
                sb.Remove(strIndex - 1, 1);

            return sb.ToString();
        }

    }
}
