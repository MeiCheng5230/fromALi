using PXin.Common;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PXin.Commu.Common
{
    public static class Helper
    {
        public static string GetMask(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "*";
            string temp = str;
            if (str.Length > 1)
                temp = str.Substring(str.Length - 1);
            string mask = string.Empty;
            int maskLength = str.Length - 1;
            for (int i = 0; i < maskLength; i++)
                mask += "*";
            temp = mask + temp;
            return temp;
        }
        public static bool CheckMd5(string str, string sign, string _key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(str + _key));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            if (!sb.ToString().Equals(sign, StringComparison.OrdinalIgnoreCase))
            {
                Log.MessageInfo("签名错误:src:" + str + _key);
                Log.MessageInfo("sign:" + sign);
                Log.MessageInfo("mysign:" + sb.ToString());
                return false;
            }
            return true;
        }
    }
}
