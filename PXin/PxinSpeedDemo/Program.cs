using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PxinSpeedDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 100 - 25 * 3 % 4;
            Console.WriteLine(a);
        }
        static void WriteLog(string msg)
        {
            DateTime dt = DateTime.Now;
            msg = dt + ":" + msg;
            Console.WriteLine(msg);
            File.AppendAllLines(dt.ToString("yyyyMMdd") + ".txt", new List<string> { msg });
        }
        static void DeleteSigned()
        {
            OracleConnection conn = new OracleConnection("");
            conn.Open();
            OracleCommand cmd = new OracleCommand("delete from tapp_signed where nodeid = 3434909 and createtime >= trunc(sysdate)", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
    public class HttpRequestMy
    {
        public HttpRequestMy()
        {
        }
        /// <summary>
        /// 发送Get方法请求
        /// </summary>
        /// <param name="strURL">调用页面字符串</param>
        /// <returns>响应字符串</returns>
        public static string SendGetRequest(string strURL)
        {
            string strResult = string.Empty;
            try
            {
                HttpWebRequest hwRequest = (HttpWebRequest)System.Net.WebRequest.Create(strURL);
                HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();

                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);

                strResult = srReader.ReadToEnd().Trim();
                srReader.Close();

            }
            catch (System.Exception err)
            {
                System.Diagnostics.Trace.WriteLine(err.ToString());
            }

            return strResult;
        }
        /// <summary>
        /// 发送Post方法请求
        /// </summary>
        /// <param name="strURL">调用页面字符串</param>
        /// <param name="bData">参数字节数姐</param>
        /// <returns>响应字符串</returns>
        public static string SendPostRequest(string strURL, byte[] bData)
        {
            string strResult = string.Empty;
            HttpWebRequest hwRequest;

            try
            {
                hwRequest = (HttpWebRequest)WebRequest.Create(strURL);
                hwRequest.Timeout = 100000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                //写数据
                Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();

                //获取结果
                HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.UTF8);
                strResult = srReader.ReadToEnd().Trim();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                System.Diagnostics.Trace.WriteLine(err.ToString());
            }

            return strResult;
        }
    }

}
