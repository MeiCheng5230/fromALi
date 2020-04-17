using Common.Mvc;
using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 开电子发票接口
    /// </summary>
    public static class InvoiceAPI
    {
        private static HttpClient httpClient = new HttpClient();

        static InvoiceAPI()
        {
            ServicePointManager.DefaultConnectionLimit = 5;
            httpClient.Timeout = new TimeSpan(0, 0, 30);
        }
        private static object objSync = new object();
        private static Log log = new Log(typeof(ExpressAPI));
        private static DateTime currDateTime = DateTime.Now.AddSeconds(-1);
        private static string KuaiDiAPPKey = AppConfig.KuaiDiAPPKey;
        private static string KuaiDiAPPSecret = AppConfig.KuaiDiAPPSecret;
        private static string KuaiDiAPPCode = AppConfig.KuaiDiAPPCode;

        /// <summary>
        /// 调用接口频率控制，1秒不超过2次
        /// </summary>
        private static void QPSControl()
        {
            int diff = (int)((TimeSpan)(DateTime.Now - currDateTime)).TotalMilliseconds;
            if (diff < 500)
            {
                Thread.Sleep(500 - diff);
            }
        }


        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static ReturnElectronice ExpressSearch(ElectroniceInfo req)
        {
            string url = "";
            //com.aisinogz.www.ElectroniceInfo
            return BusinessPost<ReturnElectronice>(url, req);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static T1 BusinessPost<T1>(string url, ElectroniceInfo req)
            where T1 : ReturnElectronice
        {
            string resp = PostResponse(url, req);
            if (string.IsNullOrEmpty(resp))
            {
                return null;
            }
            T1 result = JsonConvert.DeserializeObject<T1>(resp);
            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, object postData)
        {
            lock (objSync)
            {
                QPSControl();
                string content = JsonConvert.SerializeObject(postData);
                //url += "?access_token=" + GetAccessToken().access_token;

                log.Info("Req-Url=" + url);
                //log.Info("Req-Post=" + content);
                if (url.StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                }
                try
                {
                    //FormUrlEncodedContent httpContent = new FormUrlEncodedContent(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(postData)));
                    using (HttpContent httpContent = new StringContent(content))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        httpContent.Headers.ContentType.CharSet = "utf-8";
                        HttpClient httpClient = new HttpClient();

                        currDateTime = DateTime.Now;

                        HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                        string result = response.Content.ReadAsStringAsync().Result;
                        log.Info("Resp=" + result);
                        return result;
                    }
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
        /// 需要WebService支持Post调用
        /// </summary>
        public static XmlDocument QueryPostWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(Pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        /// 设置凭证与超时时间
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }

        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }

        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        /// <summary>
        /// 开电子票据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static ReturnElectronice InvoiceSearch(ElectroniceInfo1 req)
        {
            string url = "http://www.aisinogz.com:19876/AisinoFp-test/eliWebService.ws";
            //string url = "http://www.aisinogd.com:5000/AisinoFp/eliWebService.ws";
            return BusinessWebService<ReturnElectronice>(url, req);
        }

        //public ReturnElectronice InvoiceSearch1()
        //{
        //    var s = new aisinogd.www.ElectroniceInfo();
        //    var ss = aisinogd.www.IEliWebService.invEli(s);
        //    return ss ;
        //}

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static T1 BusinessWebService<T1>(string url, ElectroniceInfo1 req)
            where T1 : ReturnElectronice
        {
            object[] data = new object[1];
            data[0] = req;
            object resp = InvokeWebService(url, "IEliWebService", "invEli", data);
            //if (string.IsNullOrEmpty(resp))
            //{
            //    return null;
            //}
            //T1 result = JsonConvert.DeserializeObject<T1>(resp);
            return null;
        }

        /// <summary> 
        /// 动态调用WebService 
        /// </summary> 
        /// <param name="url">WebService地址</param> 
        /// <param name="classname">类名</param> 
        /// <param name="methodname">方法名(模块名)</param> 
        /// <param name="args">参数列表</param> 
        /// <returns>object</returns> 
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "ServiceBase.WebService.DynamicWebLoad";
            if (classname == null || classname == "")
            {
                classname = GetClassName(url);
            }
            //获取服务描述语言(WSDL) 
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码 
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();
            //ICodeCompiler icc = csc;// csc.CreateCompiler();
            //设定编译器的参数 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            //编译代理类 
            CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            //生成代理实例,并调用方法 
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
        }

        private static string GetClassName(string url)
        {
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }

        #region[接收类]

        public class ElectroniceInfo1
        {

            private string bMB_BBHField;

            private string bzField;

            private string cHYYField;

            private string cZDMField;

            private string dDHField;

            private string dKBZField;

            private string dSPTBMField;

            private string eMAILField;

            private string eWMField;

            private string fHRField;

            private string fPQQLSHField;

            private string fPZTField;

            private string fP_DMField;

            private string fP_HMField;

            private string fP_MWField;

            private string gHFQYLXField;

            private string gHF_DZField;

            private string gHF_EMAILField;

            private string gHF_GDDHField;

            private string gHF_MCField;

            private string gHF_NSRSBHField;

            private string gHF_SFField;

            private string gHF_SJField;

            private string gHF_YHZHField;

            private double hJBHSJEField;

            private bool hJBHSJEFieldSpecified;

            private double hJSEField;

            private bool hJSEFieldSpecified;

            private string hY_DMField;

            private string hY_MCField;

            private string jQBHField;

            private string jYMField;

            private double kPHJJEField;

            private bool kPHJJEFieldSpecified;

            private string kPLXField;

            private string kPRField;

            private string kPRQField;

            private string kPXMField;

            private string kP_NSRMCField;

            private string kP_NSRSBHField;

            private string nSRDZDAHField;

            private string pYDMField;

            private string sjField;

            private string sKRField;

            private string sWJG_DMField;

            private string tSCHBZField;

            private string tSFSField;

            private string xHF_DHField;

            private string xHF_DZField;

            private string xHF_MCField;

            private string xHF_NSRSBHField;

            private string xHF_YHZHField;

            private string yFP_DMField;

            private string yFP_HMField;

            private string appIdField;

            private string authorizationCodeField;

            private string codeTypeField;

            private string contentField;

            private string dataExchangeIdField;

            private ElectroniceDetail[] detailsField;

            private string encryptCodeField;

            private string interfaceCodeField;

            private string passWordField;

            private string requestCodeField;

            private string responseCodeField;

            private string terminalCodeField;

            private string userNameField;

            private string versionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string BMB_BBH
            {
                get
                {
                    return this.bMB_BBHField;
                }
                set
                {
                    this.bMB_BBHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string BZ
            {
                get
                {
                    return this.bzField;
                }
                set
                {
                    this.bzField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string CHYY
            {
                get
                {
                    return this.cHYYField;
                }
                set
                {
                    this.cHYYField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string CZDM
            {
                get
                {
                    return this.cZDMField;
                }
                set
                {
                    this.cZDMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string DDH
            {
                get
                {
                    return this.dDHField;
                }
                set
                {
                    this.dDHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string DKBZ
            {
                get
                {
                    return this.dKBZField;
                }
                set
                {
                    this.dKBZField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string DSPTBM
            {
                get
                {
                    return this.dSPTBMField;
                }
                set
                {
                    this.dSPTBMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string EMAIL
            {
                get
                {
                    return this.eMAILField;
                }
                set
                {
                    this.eMAILField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string EWM
            {
                get
                {
                    return this.eWMField;
                }
                set
                {
                    this.eWMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FHR
            {
                get
                {
                    return this.fHRField;
                }
                set
                {
                    this.fHRField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FPQQLSH
            {
                get
                {
                    return this.fPQQLSHField;
                }
                set
                {
                    this.fPQQLSHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FPZT
            {
                get
                {
                    return this.fPZTField;
                }
                set
                {
                    this.fPZTField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FP_DM
            {
                get
                {
                    return this.fP_DMField;
                }
                set
                {
                    this.fP_DMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FP_HM
            {
                get
                {
                    return this.fP_HMField;
                }
                set
                {
                    this.fP_HMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string FP_MW
            {
                get
                {
                    return this.fP_MWField;
                }
                set
                {
                    this.fP_MWField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHFQYLX
            {
                get
                {
                    return this.gHFQYLXField;
                }
                set
                {
                    this.gHFQYLXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_DZ
            {
                get
                {
                    return this.gHF_DZField;
                }
                set
                {
                    this.gHF_DZField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_EMAIL
            {
                get
                {
                    return this.gHF_EMAILField;
                }
                set
                {
                    this.gHF_EMAILField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_GDDH
            {
                get
                {
                    return this.gHF_GDDHField;
                }
                set
                {
                    this.gHF_GDDHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_MC
            {
                get
                {
                    return this.gHF_MCField;
                }
                set
                {
                    this.gHF_MCField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_NSRSBH
            {
                get
                {
                    return this.gHF_NSRSBHField;
                }
                set
                {
                    this.gHF_NSRSBHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_SF
            {
                get
                {
                    return this.gHF_SFField;
                }
                set
                {
                    this.gHF_SFField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_SJ
            {
                get
                {
                    return this.gHF_SJField;
                }
                set
                {
                    this.gHF_SJField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string GHF_YHZH
            {
                get
                {
                    return this.gHF_YHZHField;
                }
                set
                {
                    this.gHF_YHZHField = value;
                }
            }

            /// <remarks/>
            public double HJBHSJE
            {
                get
                {
                    return this.hJBHSJEField;
                }
                set
                {
                    this.hJBHSJEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HJBHSJESpecified
            {
                get
                {
                    return this.hJBHSJEFieldSpecified;
                }
                set
                {
                    this.hJBHSJEFieldSpecified = value;
                }
            }

            /// <remarks/>
            public double HJSE
            {
                get
                {
                    return this.hJSEField;
                }
                set
                {
                    this.hJSEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool HJSESpecified
            {
                get
                {
                    return this.hJSEFieldSpecified;
                }
                set
                {
                    this.hJSEFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string HY_DM
            {
                get
                {
                    return this.hY_DMField;
                }
                set
                {
                    this.hY_DMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string HY_MC
            {
                get
                {
                    return this.hY_MCField;
                }
                set
                {
                    this.hY_MCField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string JQBH
            {
                get
                {
                    return this.jQBHField;
                }
                set
                {
                    this.jQBHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string JYM
            {
                get
                {
                    return this.jYMField;
                }
                set
                {
                    this.jYMField = value;
                }
            }

            /// <remarks/>
            public double KPHJJE
            {
                get
                {
                    return this.kPHJJEField;
                }
                set
                {
                    this.kPHJJEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool KPHJJESpecified
            {
                get
                {
                    return this.kPHJJEFieldSpecified;
                }
                set
                {
                    this.kPHJJEFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KPLX
            {
                get
                {
                    return this.kPLXField;
                }
                set
                {
                    this.kPLXField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KPR
            {
                get
                {
                    return this.kPRField;
                }
                set
                {
                    this.kPRField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KPRQ
            {
                get
                {
                    return this.kPRQField;
                }
                set
                {
                    this.kPRQField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KPXM
            {
                get
                {
                    return this.kPXMField;
                }
                set
                {
                    this.kPXMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KP_NSRMC
            {
                get
                {
                    return this.kP_NSRMCField;
                }
                set
                {
                    this.kP_NSRMCField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string KP_NSRSBH
            {
                get
                {
                    return this.kP_NSRSBHField;
                }
                set
                {
                    this.kP_NSRSBHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string NSRDZDAH
            {
                get
                {
                    return this.nSRDZDAHField;
                }
                set
                {
                    this.nSRDZDAHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string PYDM
            {
                get
                {
                    return this.pYDMField;
                }
                set
                {
                    this.pYDMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string SJ
            {
                get
                {
                    return this.sjField;
                }
                set
                {
                    this.sjField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string SKR
            {
                get
                {
                    return this.sKRField;
                }
                set
                {
                    this.sKRField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string SWJG_DM
            {
                get
                {
                    return this.sWJG_DMField;
                }
                set
                {
                    this.sWJG_DMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string TSCHBZ
            {
                get
                {
                    return this.tSCHBZField;
                }
                set
                {
                    this.tSCHBZField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string TSFS
            {
                get
                {
                    return this.tSFSField;
                }
                set
                {
                    this.tSFSField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string XHF_DH
            {
                get
                {
                    return this.xHF_DHField;
                }
                set
                {
                    this.xHF_DHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string XHF_DZ
            {
                get
                {
                    return this.xHF_DZField;
                }
                set
                {
                    this.xHF_DZField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string XHF_MC
            {
                get
                {
                    return this.xHF_MCField;
                }
                set
                {
                    this.xHF_MCField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string XHF_NSRSBH
            {
                get
                {
                    return this.xHF_NSRSBHField;
                }
                set
                {
                    this.xHF_NSRSBHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string XHF_YHZH
            {
                get
                {
                    return this.xHF_YHZHField;
                }
                set
                {
                    this.xHF_YHZHField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string YFP_DM
            {
                get
                {
                    return this.yFP_DMField;
                }
                set
                {
                    this.yFP_DMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string YFP_HM
            {
                get
                {
                    return this.yFP_HMField;
                }
                set
                {
                    this.yFP_HMField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string appId
            {
                get
                {
                    return this.appIdField;
                }
                set
                {
                    this.appIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string authorizationCode
            {
                get
                {
                    return this.authorizationCodeField;
                }
                set
                {
                    this.authorizationCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string codeType
            {
                get
                {
                    return this.codeTypeField;
                }
                set
                {
                    this.codeTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string content
            {
                get
                {
                    return this.contentField;
                }
                set
                {
                    this.contentField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string dataExchangeId
            {
                get
                {
                    return this.dataExchangeIdField;
                }
                set
                {
                    this.dataExchangeIdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true)]
            public ElectroniceDetail[] details
            {
                get
                {
                    return this.detailsField;
                }
                set
                {
                    this.detailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string encryptCode
            {
                get
                {
                    return this.encryptCodeField;
                }
                set
                {
                    this.encryptCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string interfaceCode
            {
                get
                {
                    return this.interfaceCodeField;
                }
                set
                {
                    this.interfaceCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string passWord
            {
                get
                {
                    return this.passWordField;
                }
                set
                {
                    this.passWordField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string requestCode
            {
                get
                {
                    return this.requestCodeField;
                }
                set
                {
                    this.requestCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string responseCode
            {
                get
                {
                    return this.responseCodeField;
                }
                set
                {
                    this.responseCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string terminalCode
            {
                get
                {
                    return this.terminalCodeField;
                }
                set
                {
                    this.terminalCodeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string userName
            {
                get
                {
                    return this.userNameField;
                }
                set
                {
                    this.userNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
            public string version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ElectroniceInfo
        {
            /// <summary>
            /// 发票请求唯一流水号    每张发票的发票请求唯一流水号无重复，由企业定义。升级版长度约定为20位
            /// </summary>
            public string FPQQLSH { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public string DDH { get; set; }
            /// <summary>
            /// 开票方纳税人识别号
            /// </summary>
            public string KP_NSRSBH { get; set; }
            /// <summary>
            /// 开票方名称
            /// </summary>
            public string KP_NSRMC { get; set; }
            /// <summary>
            /// 代开标志 自开(0)；一般企业选择0  代开(1)
            /// </summary>
            public string DKBZ { get; set; }
            /// <summary>
            /// 销货方识别号
            /// </summary>
            public string XHF_NSRSBH { get; set; }
            /// <summary>
            /// 销货方名称
            /// </summary>
            public string XHFMC { get; set; }
            /// <summary>
            /// 销货方地址
            /// </summary>
            public string XHF_DZ { get; set; }
            /// <summary>
            /// 销货方电话
            /// </summary>
            public string XHF_DH { get; set; }
            /// <summary>
            /// 销货方银行账号
            /// </summary>
            public string XHF_YHZH { get; set; }
            /// <summary>
            /// 购货方名称  购货方名称，即发票抬头。购货方为“个人”时，可输入名称，输入名称是为“个人(名称)”，”（”为半角；例个人(王杰)
            /// </summary>
            public string GHFMC { get; set; }
            /// <summary>
            /// 开票员 企业名称
            /// </summary>
            public string KPR { get; set; }
            /// <summary>
            /// 开票类型 1正票、2红票
            /// </summary>
            public int KPLX { get; set; }
            /// <summary>
            /// 操作代码 
            /// 10正票正常开具
            /// 11正票错票重开
            /// 20 退货折让红票
            /// 21 错票重开红票
            /// 22换票冲红（全冲红电子发票，开具纸质发票）
            /// </summary>
            public string CZDM { get; set; }
            /// <summary>
            /// 价税合计金额
            /// </summary>
            public double KPHJJE { get; set; }
            /// <summary>
            /// 税收分类编码版  目前需要传30.0
            /// </summary>
            public string BMB_BBH { get; set; }
            /// <summary>
            /// 企业分机号
            /// </summary>
            public string NSRDZDAH { get; set; }
            /// <summary>
            /// 发票种类
            /// </summary>
            public string SWJG_DM { get; set; }
            /// <summary>
            /// 主要开票项目
            /// </summary>
            public string KPXM { get; set; }
            /// <summary>
            /// 购货方识别号
            /// </summary>
            public string GHF_NSRSBH { get; set; }
            /// <summary>
            /// 购货方地址
            /// </summary>
            public string GHF_DZ { get; set; }
            /// <summary>
            /// 购货方省份
            /// </summary>
            public string GHF_SF { get; set; }
            /// <summary>
            /// 购货方固定电话
            /// </summary>
            public string GHF_GDDH { get; set; }
            /// <summary>
            /// 购货方手机
            /// </summary>
            public string GHF_SJ { get; set; }
            /// <summary>
            /// 购货方邮箱
            /// </summary>
            public string GHF_EMAIL { get; set; }
            /// <summary>
            /// 购货方企业类型
            /// </summary>
            public string GHFQYLX { get; set; }
            /// <summary>
            /// 购货方银行账号
            /// </summary>
            public string GHF_YHZH { get; set; }
            /// <summary>
            /// 行业代码
            /// </summary>
            public string HY_DM { get; set; }
            /// <summary>
            /// 行业名称
            /// </summary>
            public string HY_MC { get; set; }
            /// <summary>
            /// 收款员
            /// </summary>
            public string SKR { get; set; }
            /// <summary>
            /// 复核人
            /// </summary>
            public string FHR { get; set; }
            /// <summary>
            /// 原发票代码
            /// </summary>
            public string YFP_DM { get; set; }
            /// <summary>
            /// 原发票号码
            /// </summary>
            public string YFP_HM { get; set; }
            /// <summary>
            /// 特殊冲红标志
            /// </summary>
            public string TSCHBZ { get; set; }
            /// <summary>
            /// 冲红原因
            /// </summary>
            public string CHYY { get; set; }
            /// <summary>
            /// 合计不含税金额。所有商品行不含税金额之和。
            /// </summary>
            public string HJBHSJE { get; set; }
            /// <summary>
            /// 合计税额。所有商品行税额之和。
            /// </summary>
            public string HJSE { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string BZ { get; set; }
            /// <summary>
            /// 发票明细 
            /// </summary>
            public ElectroniceDetail DETAILS { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ElectroniceDetail
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string XMMC { get; set; }
            /// <summary>
            /// 项目数量
            /// </summary>
            public double XMSL { get; set; }
            /// <summary>
            /// 含税标志
            /// </summary>
            public string HSBZ { get; set; }
            /// <summary>
            /// 项目单价
            /// </summary>
            public double XMDJ { get; set; }
            /// <summary>
            /// 项目金额
            /// </summary>
            public double XMJE { get; set; }
            /// <summary>
            /// 商品分类编码
            /// </summary>
            public string SPBM { get; set; }
            /// <summary>
            /// 优惠政策标识商品编码版本：必填0：不使用，1使用
            /// </summary>
            public string YHZCBS { get; set; }
            /// <summary>
            /// 项目单位
            /// </summary>
            public string DW { get; set; }
            /// <summary>
            /// 规格型号
            /// </summary>
            public string GGXH { get; set; }
            /// <summary>
            /// 项目编码
            /// </summary>
            public string XMBM { get; set; }
            /// <summary>
            /// 自行编码
            /// </summary>
            public string ZXBM { get; set; }
            /// <summary>
            /// 零税率标识
            /// </summary>
            public string LSLBS { get; set; }
            /// <summary>
            /// 增值税特殊管理
            /// </summary>
            public string ZZSTSGL { get; set; }
            /// <summary>
            /// 税额
            /// </summary>
            public double SE { get; set; }
            /// <summary>
            /// 扣除额
            /// </summary>
            public double KCE { get; set; }
            /// <summary>
            /// 税率
            /// </summary>
            public string SL { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ReturnElectronice
        {
            /// <summary>
            /// 操作码
            /// </summary>
            public string CZDM { get; set; }
            /// <summary>
            /// 订单号
            /// </summary>
            public string DDH { get; set; }
            /// <summary>
            /// 发票流水号
            /// </summary>
            public string FPQQLSH { get; set; }
            /// <summary>
            /// 开票类型
            /// </summary>
            public string KPLX { get; set; }
            /// <summary>
            /// 开票日期
            /// </summary>
            public string KPRQ { get; set; }
            /// <summary>
            /// 发票号码
            /// </summary>
            public string FP_HM { get; set; }
            /// <summary>
            /// 发票代码
            /// </summary>
            public string FP_DM { get; set; }
            /// <summary>
            /// 检验码
            /// </summary>
            public string JYM { get; set; }
            /// <summary>
            /// 版式文件名称
            /// </summary>
            public string PDF_FILE { get; set; }
            /// <summary>
            /// 下载链接
            /// </summary>
            public string PDF_URL { get; set; }
            /// <summary>
            /// 结果代码
            /// </summary>
            public string RETURNCODE { get; set; }
            /// <summary>
            /// 结果描述
            /// </summary>
            public string RETURNMESSAGE { get; set; }
        }

        #endregion

    }
}
