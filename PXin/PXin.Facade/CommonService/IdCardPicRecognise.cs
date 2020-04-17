using Common.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public class IdCardPicRecognise
    {
        private readonly Log log = new Log(typeof(IdCardPicRecognise));
        /// <summary>
        /// 识别身份证图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IdentResult Regcognise(string fileName)
        {
            UploadFile uf = new UploadFile
            {
                Filename = Path.GetFileName(fileName),
                Name = "image_file",
                Data = File.ReadAllBytes(fileName)
            };
            if (uf.Data.LongLength / 1024.0 / 1024 >= 1.5)
            {
                log.Info("图片[" + fileName + "]太大，length=" + uf.Data.LongLength + ",进行压缩");
                string tmp = Path.GetFileNameWithoutExtension(fileName);
                string destFileName = fileName.Replace(tmp, tmp + "_small");
                if (GetThumImage(fileName, 50, 1, destFileName))
                {
                    fileName = destFileName;
                }
                uf.Filename = Path.GetFileName(fileName);
                uf.Data = File.ReadAllBytes(fileName);
            }
            NameValueCollection stringDict = new NameValueCollection();
            stringDict.Add("api_key", "G_W5zLtBcImF31eQ64BYUAm7sVJOdsw1");
            stringDict.Add("api_secret", "MTr-f-t2fXf-tfaCpLusuO5ysJQo8X5S");
            //stringDict.Add("legality", "1");

            string retString = Decode(HttpPostData(new List<UploadFile> { uf }, stringDict));
            log.Info("图片[" + fileName + "],result=" + retString);
            IdentResult result = null;
            try
            {
                result = JsonConvert.DeserializeObject<IdentResult>(retString);
            }
            catch (Exception exp)
            {
                log.Info("JSON反序列化失败," + exp);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public IdentResultAliyun RegcogniseAliyun(string fileName, int type)
        {
            IdentResultAliyun result = null;
            string retString = AliyunOCR.AliyunIdentityOCR(fileName, type);
            try
            {
                var jObject = JObject.Parse(retString);
                var j = jObject["outputs"][0]["outputValue"]["dataValue"];
                string str = j.ToString();//.Replace("\\", ""); ;
                result = JsonConvert.DeserializeObject<IdentResultAliyun>(str);
            }
            catch (Exception exp)
            {
                log.Info("JSON反序列化失败," + exp);
            }
            return result;
        }
        /// <summary>
        /// 提交带文件的form
        /// </summary>
        private string HttpPostData(IEnumerable<UploadFile> files, NameValueCollection stringDict)
        {
            string respCnt;
            try
            {
                var memStream = new MemoryStream();
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
                // 设置属性
                webRequest.Method = "POST";
                webRequest.Timeout = 5000;
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                // 写入文件
                if (files != null && files.Count() > 0)
                {
                    const string filePartHeader =
                        "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                        "Content-Type: image/jpeg\r\n\r\n";
                    string endFile = "\r\n";
                    var endFileBytes = Encoding.UTF8.GetBytes(endFile);
                    foreach (UploadFile file in files)
                    {
                        var header = string.Format(filePartHeader, file.Name, file.Filename);
                        var headerbytes = Encoding.UTF8.GetBytes(header);

                        memStream.Write(beginBoundary, 0, beginBoundary.Length);
                        memStream.Write(headerbytes, 0, headerbytes.Length);
                        memStream.Write(file.Data, 0, file.Data.Length);
                        memStream.Write(endFileBytes, 0, endFileBytes.Length);
                    }
                }
                if (stringDict != null)
                {
                    // 写入字符串的Key
                    var stringKeyHeader = "--" + boundary +
                                           "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                           "\r\n\r\n{1}\r\n";
                    foreach (string key in stringDict.Keys)
                    {
                        string s = string.Format(stringKeyHeader, key, stringDict[key]);
                        byte[] data = Encoding.UTF8.GetBytes(s);
                        memStream.Write(data, 0, data.Length);
                    }
                }
                // 写入最后的结束边界符
                memStream.Write(endBoundary, 0, endBoundary.Length);

                webRequest.ContentLength = memStream.Length;

                var requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                var tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();

                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();

                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    respCnt = httpStreamReader.ReadToEnd();
                }
                httpWebResponse.Close();
                webRequest.Abort();
            }
            catch (WebException webExcept)
            {
                if (webExcept.Response != null)
                {
                    StreamReader sr = new StreamReader(webExcept.Response.GetResponseStream(), Encoding.UTF8);
                    respCnt = sr.ReadToEnd();
                }
                else
                {
                    respCnt = "{\"error_message\":\"" + (int)webExcept.Status + "\"}";
                }
            }
            return respCnt;
        }
        /// <summary>  
        /// 生成缩略图  
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        private bool GetThumImage(String sourceFile, long quality, int multiple, String outputFile)
        {
            try
            {
                long imageQuality = quality;
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / multiple), (int)(yWidth / multiple));
                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
                g.Dispose();
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>  
        /// 获取图片编码信息  
        /// </summary>  
        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private readonly string url = "https://api-cn.faceplusplus.com/cardpp/v1/ocridcard";
        private readonly Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
        private string Decode(string s)
        {
            return reUnicode.Replace(s, m =>
            {
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out short c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }
    }
}
