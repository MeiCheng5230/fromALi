using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Models
{
    public class Helper
    {
        static Log logger = new Log(typeof(Helper));
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
                logger.Info("签名错误:src:" + str + _key);
                logger.Info("sign:" + sign);
                logger.Info("mysign:" + sb.ToString());
                return false;
            }
            return true;
        }
    }
}
