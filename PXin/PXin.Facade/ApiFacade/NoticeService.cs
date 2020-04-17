using Common.Facade;
using Common.Mvc;
using PXin.DB;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class NoticeService : FacadeBase<PXinContext>
    {
        private TpcnThirdPayhis payHis;
        private TpcnThirdPartner partner;
        /// <summary>
        /// 
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NoticeService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisid"></param>
        public NoticeService(int hisid)
        {
            Hisid = hisid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payHis"></param>
        public NoticeService(TpcnThirdPayhis payHis)
        {
            this.payHis = payHis;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisid"></param>
        public void Execute(int hisid)
        {
            Hisid = hisid;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payHis"></param>
        public void Execute(TpcnThirdPayhis payHis)
        {
            this.payHis = payHis;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            string guidStr = Guid.NewGuid().ToString();
            log.Info($"开始调用通知{guidStr}");

            if (Try(ExecuteCore))
            {
                log.Info($"调用通知成功-结束{guidStr}");
            }
            else
            {
                log.Info($"调用通知失败-结束{guidStr}");
            }
            Hisid = 0;
            payHis = null;
        }

        public bool ExecuteCore()
        {
            payHis = db.TpcnThirdPayhisSet.FirstOrDefault(x => x.Hisid == Hisid);
            if (!(Hisid > 0 && payHis != null))
            {
                log.Info($"{nameof(Hisid)}={Hisid}:支付历史不存在");
                return false;
            }
            if (payHis.Storequest != 0)
            {
                log.Info($"{nameof(Hisid)}={Hisid}:通知已成功送达");
                return false;
            }
            partner = db.TpcnThirdPartnerSet.FirstOrDefault(x => x.Id == payHis.Partnerid);
            if (partner == null)
            {
                log.Info($"签名id={payHis.Partnerid}: PCN签名信息不存在");
                return false;
            }

            Dictionary<string, string> immutableMap = BuildMap();
            //签名
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            immutableMap.Add("timestamp", timestamp);
            string Signature = Md5.SignString(partner.Accesskeyid + payHis.Orderno + timestamp + partner.Accesssecret);
            immutableMap.Add("signature", Signature);
            string queryString = ConcatQueryString(immutableMap);
            log.Info("req-url=" + payHis.Notifyurl);
            log.Info("req-cnt=" + queryString);
            string result = Try(PostReqeust, payHis.Notifyurl, queryString);
            log.Info("post的结果" + result);
            if ("OK".Equals(result, StringComparison.OrdinalIgnoreCase))
            {
                payHis.Storequest = 2;
                payHis.Notifyfailnumber += 1;
            }
            else
            {
                payHis.Notifyfailnumber += 1;
                payHis.Nextnotifytime = DateTime.Now.AddSeconds(GetNextnotifytime(payHis.Notifyfailnumber));
            }
            if (db.SaveChanges() <= 0)
            {
                return false;
            }
            return true;
        }



        private int GetNextnotifytime(int failNum)
        {
            if (failNum == 0) return 60;
            else if (failNum == 1) return 300;
            else if (failNum == 2) return 600;
            else if (failNum == 3) return 1800;
            else if (failNum == 4) return 3600;
            else if (failNum == 5) return 43200;
            else return 86400;
        }


        private Dictionary<string, string> BuildMap()
        {
            Dictionary<string, string> immutableMap = new Dictionary<string, string>();
            //公共参数
            immutableMap.Add("secretkey", partner.Accesskeyid);
            immutableMap.Add("orderpcn", payHis.Hisid.ToString());
            immutableMap.Add("paytype", payHis.Paytype.ToString());
            immutableMap.Add("amount", payHis.Amount.ToString());
            immutableMap.Add("orderno", payHis.Orderno.ToString());
            immutableMap.Add("subject", payHis.Subject.ToString());
            immutableMap.Add("body", payHis.Body.ToString());
            immutableMap.Add("createtime", payHis.Createtime.ToString("yyyy-MM-dd HH:mm:ss"));
            immutableMap.Add("paystatus", payHis.Paystatus.ToString());
            return immutableMap;
        }
        private String ConcatQueryString(Dictionary<String, String> immutableMap)
        {
            if (null == immutableMap)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();

            foreach (var entry in immutableMap)
            {
                String key = entry.Key;
                String val = entry.Value;

                sb.Append(Encode(key));
                if (val != null)
                {
                    sb.Append("=").Append(Encode(val));
                }
                sb.Append("&");
            }

            int strIndex = sb.Length;
            if (immutableMap.Count > 0)
                sb.Remove(strIndex - 1, 1);

            return sb.ToString();
        }



        //Post请求
        public static string PostReqeust(string url, string obj = null)
        {
            string param = (obj);//参数
            byte[] bs = Encoding.Default.GetBytes(param);

            //创建一个新的HttpWebRequest对象。
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

            // 将方法属性设置为“POST”以将数据发布到URI。
            req.Method = "POST";

            //设置contentType属性。
            req.ContentType = "application/x-www-form-urlencoded";

            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                HttpWebResponse response2 = (HttpWebResponse)req.GetResponse();

                StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.UTF8);
                string result = sr2.ReadToEnd();

                return result;
            }

        }
        public static string Encode(String value)
        {
            return HttpUtility.UrlEncode(value, Encoding.UTF8);
        }
        protected TResult Try<TResult>(Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception exp)
            {
                log.Info(exp);
                return default(TResult);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        protected TResult Try<TArg, TResult>(Func<TArg, TArg, TResult> func, TArg arg1, TArg arg2)
        {
            try
            {
                return func(arg1, arg2);
            }
            catch (Exception exp)
            {
                log.Info(exp);
                return default(TResult);
            }
        }
    }
}
