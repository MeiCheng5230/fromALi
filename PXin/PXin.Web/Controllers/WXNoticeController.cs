using ApiAuthCore;
using Common.Mvc;
using Common.UEPay;
using Newtonsoft.Json.Linq;
using PXin.DB;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PXin.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class WXNoticeController : Controller
    {
        Log log = new Log(typeof(WXNoticeController));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Success()
        {
            if (Request.RequestType.ToUpper() == "POST")
            {
                JToken type = null;
                string metadata = "";
                string orderno = "";
                if (Request.Url.ToString().IndexOf("localhost") > 0)
                {
                    #region 测试注释
                    orderno = Request["orderno"];
                    type = "charge.succeeded";
                    metadata = Request["metadata"];
                    #endregion
                }
                else
                {
                    Log log = new Log("Webhooks");
                    log.Info("-----------------Ping++ 回调 Webhooks Begin-------------------------");

                    //获取 post 的 event对象 
                    string inputData = ReadStream(Request.InputStream);
                    log.Info(inputData);
                    //获取 header 中的签名
                    string sig = Request.Headers.Get("x-pingplusplus-signature");
                    log.Info("----sig---:" + sig);

                    JObject jObject = JObject.Parse(inputData);
                    type = jObject.SelectToken("type");

                    log.Info("----type----:" + type.ToString());
                    log.Info("----order_no----:" + jObject.SelectToken("data.object.order_no").ToString());
                    log.Info("----metadata----:" + jObject.SelectToken("data.object.metadata.type").ToString());
                    string sid = string.Empty;
                    try
                    {
                        sid = jObject.SelectToken("data.object.metadata.sid").ToString();
                        log.Info("----metadata.sid----:" + sid);
                    }
                    catch
                    {

                    }

                    metadata = jObject.SelectToken("data.object.metadata.type").ToString();

                    orderno = jObject.SelectToken("data.object.order_no").ToString();

                    //公钥路径（请检查你的公钥 .pem 文件存放路径）
                    string path = Server.MapPath("/Key/key.pem");

                    //验证签名
                    string result = VerifySignedHash(inputData, sig, path);

                    log.Info("验证签名结果:" + result);

                    if (result != "verify success")
                    {
                        log.Info("pingpp验证签名失败，处理流程结束：" + inputData);
                        return Content("Failed");
                    }

                }

                if (type.ToString() == "charge.succeeded" || type.ToString() == "refund.succeeded")
                {
                    if (metadata.Equals("svc", StringComparison.OrdinalIgnoreCase))
                    {
                        CZMFacade facade = new CZMFacade();
                        if (!facade.PingppPaySuccess_SVC(orderno))
                        {
                            log.Info(facade.PromptInfo.ToString());
                            log.Info("--------购买SVC充值码支付失败-------");
                            Response.StatusCode = 500;
                            log.Info("-----------------Ping++ 回调 Webhooks End-------------------------");
                            return Content("Failed");
                        }
                        log.Info("--------购买SVC充值码支付成功-------");

                    }
                    else if (metadata.Equals("uvcharge", StringComparison.OrdinalIgnoreCase))
                    {
                        CZMFacade facade = new CZMFacade();
                        if (!facade.PingppPaySuccess_UV(orderno))
                        {
                            log.Info(facade.PromptInfo.ToString());
                            log.Info("--------购买UV充值码支付失败-------");
                            Response.StatusCode = 500;
                            log.Info("-----------------Ping++ 回调 Webhooks End-------------------------");
                            return Content("Failed");
                        }
                        log.Info("--------购买UV充值码支付成功-------");
                    }
                    else
                    {
                        log.Info(metadata + ",未知业务回调，没有处理");
                        Response.StatusCode = 500;
                        log.Info("-----------------Ping++ 回调 Webhooks End-------------------------");
                        return Content("Failed");
                    }
                    // TODO what you need do


                }
                else
                {
                    log.Info("--------支付失败-------");
                    log.Info("-----------------Ping++ 回调 Webhooks End-------------------------");
                    Response.StatusCode = 500;
                    return Content("Failed");
                }

                log.Info("-----------------Ping++ 回调 Webhooks End-------------------------");
                Response.StatusCode = 200;
                return Content("ok");
            }

            Response.StatusCode = 500;
            return Content("Failed");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_DataToVerify"></param>
        /// <param name="str_SignedData"></param>
        /// <param name="str_publicKeyFilePath"></param>
        /// <returns></returns>
        private string VerifySignedHash(string str_DataToVerify, string str_SignedData, string str_publicKeyFilePath)
        {
            byte[] SignedData = Convert.FromBase64String(str_SignedData);

            UTF8Encoding ByteConverter = new UTF8Encoding();
            byte[] DataToVerify = ByteConverter.GetBytes(str_DataToVerify);
            try
            {
                string sPublicKeyPEM = System.IO.File.ReadAllText(str_publicKeyFilePath);
                //string sPublicKeyPEM = str_publicKeyFilePath; 
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                rsa.PersistKeyInCsp = false;
                rsa.LoadPublicKeyPEM(sPublicKeyPEM);

                if (rsa.VerifyData(DataToVerify, "SHA256", SignedData))
                {
                    return "verify success";
                }
                else
                {
                    return "verify fail";
                }

            }
            catch (CryptographicException e)
            {
                log.Info("str_publicKeyFilePath=" + str_publicKeyFilePath);
                log.Info(e.Message);

                return "verify error";
            }

        }
    }
}
