using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Collections;
using System.Configuration;
using Common;
using Pingpp.Models;
using Common.Mvc;

namespace Wsx.UserCenter.Facade
{
    public class GetCharges
    {
        protected Log log = new Log(typeof(GetCharges));
        public Charge CreateCharges(string apiKey,string appId, string channel,string orderno,decimal amount,string subject,string body,string type)
        {
            //Pingpp.apiKey = Config.ApiKey;
            Pingpp.Pingpp.SetApiKey(apiKey);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Dictionary<string, object> extra = new Dictionary<string, object>();
            if (channel.ToString().Equals("alipay_wap"))
            {
                //extra.Add("success_url", ConfigurationManager.AppSettings["Alipay_url"]); //支付宝支付成功回调地址
                //extra.Add("cancel_url", "http://www.yourdomain.com/cancel");
            }
            else if (channel.ToString().Equals("wx_pub"))
            {
                extra.Add("open_id", ConfigurationManager.AppSettings["Open_id"]);
            }
            else if (channel.ToString().Equals("upacp_wap"))
            {
                extra.Add("result_url", "http://www.yourdomain.com/result");
            }
            else if (channel.ToString().Equals("upmp_wap"))
            {
                extra.Add("result_url", "http://www.yourdomain.com/result?code=");
            }
            else if (channel.ToString().Equals("bfb_wap"))
            {
                extra.Add("result_url", "http://www.yourdomain.com/result");
                extra.Add("bfb_login", true);
            }
            else if (channel.ToString().Equals("wx_pub_qr"))
            {
                extra.Add("product_id", "asdfsadfadsf");
            }
            else if (channel.ToString().Equals("yeepay_wap"))
            {
                extra.Add("product_category", "1");
                extra.Add("identity_id", "sadfsdaf");
                extra.Add("identity_type", 1);
                extra.Add("terminal_type", 1);
                extra.Add("terminal_id", "sadfsadf");
                extra.Add("user_ua", "sadfsdaf");
                extra.Add("result_url", "http://www.yourdomain.com/result");
            }
            else if (channel.ToString().Equals("jdpay_wap"))
            {
                extra.Add("success_url", "http://www.yourdomain.com/success");
                extra.Add("fail_url", "http://www.yourdomain.com/fail");
                extra.Add("token", "fjdilkkydoqlpiunchdysiqkanczxude");//32 位字符串
            }

            Dictionary<string, string> app = new Dictionary<string, string>();
            //app.Add("id", Config.AppId);
            app.Add("id", appId);

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("order_no", orderno);
            param.Add("amount", amount * 100);
            param.Add("channel", channel);
            param.Add("currency", "cny");
            param.Add("subject", subject);
            param.Add("body", body);

            IPHostEntry myEntry = Dns.GetHostEntry(Dns.GetHostName());
            string ipaddress = myEntry.AddressList.FirstOrDefault<IPAddress>(c => c.AddressFamily.ToString().Equals("InterNetwork")).ToString();

            param.Add("client_ip", ipaddress);
            param.Add("app", app);
            param.Add("extra", extra);

            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("type",type);
            param.Add("metadata", metadata);

            try
            {
                Charge charge = Charge.Create(param);
                log.Info("---------------Charge对象----------------");
                log.Info(charge.ToString());
                log.Info("------------------Ping++日志END---------------------------");
                //return JsonConvert.SerializeObject(model);
                return charge;
            }
            catch (Exception)
            {
                return null;
                //return JsonConvert.SerializeObject(model);
            }
        }
    }
}