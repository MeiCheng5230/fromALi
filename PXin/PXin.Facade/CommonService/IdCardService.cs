using Common.Facade.Models;
using Common.Mvc;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 身份证识别服务
    /// </summary>
    public class IdCardService
    {
        Log log = new Log("IdCardService");

        /// <summary>
        /// 获取身份证识别结果
        /// </summary>
        public IdentResult GetRecResult(string pic)
        {
            pic = System.IO.Path.GetFileName(pic);

            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var idcardrecLog = db.TzcIdcardrecLogSet.FirstOrDefault(x => x.Pic == pic);
            if (idcardrecLog != null)
            {
                return JsonConvert.DeserializeObject<IdentResult>(idcardrecLog.Recresult);
            }
            return null;
        }

        /// <summary>
        /// 识别身份证照片
        /// </summary>
        public Respbase<IdentCard> IdCardPicRecognise(int imageActionType, string fullFileName)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();

            IdCardPicRecognise picRec = new IdCardPicRecognise();
            IdentResult result = picRec.Regcognise(fullFileName);
            if (result != null
                && result.Success
                && result.cards != null
                && result.cards.Length >= 1)
            {
                if (imageActionType == 1 && (result.cards[0].side != "front"
                    || string.IsNullOrEmpty(result.cards[0].id_card_number) || result.cards[0].id_card_number.Length != 18
                    || !CheckCard18(result.cards[0].id_card_number)
                    || string.IsNullOrEmpty(result.cards[0].name) || result.cards[0].name.Length < 2))
                {
                    return new Respbase<IdentCard> { Message = "身份证正面识别错误，请重新上传", Result = 0 };
                }
                else if (imageActionType == 2 && (result.cards[0].side != "back"
                    || !IsValidDate(result.cards[0].valid_date)))
                {
                    return new Respbase<IdentCard> { Message = "身份证背面识别错误，请重新上传", Result = 0 };
                }
                else
                {
                    TzcIdcardrecLog recogniseLog = new TzcIdcardrecLog
                    {
                        Pic = System.IO.Path.GetFileName(fullFileName),
                        Recresult = JsonConvert.SerializeObject(result)
                    };
                    db.TzcIdcardrecLogSet.Add(recogniseLog);
                    if (db.SaveChanges() <= 0)
                    {
                        return new Respbase<IdentCard> { Message = "保存识别结果失败", Result = 0 };
                    }
                    else
                    {
                        return new Respbase<IdentCard> { Message = "上传成功", Result = 1, Data = result.cards[0] };
                    }
                }
            }
            else
            {
                return new Respbase<IdentCard> { Message = "上传照片识别失败,请重新上传清晰照片", Result = 0 };
            }
        }

        #region Private Method
        private bool CheckCard18(string CardId)//CheckCard18方法用于检查18位身份证号码的合法性
        {
            if (long.TryParse(CardId.Remove(17), out long n) == false || n < Math.Pow(10, 16) || long.TryParse(CardId.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证
            string[] Myaddress = new string[]{ "11","22","35","44","53","12",
                "23","36","45","54","13","31","37","46","61","14","32","41",
                "50","62","15","33","42","51","63","21","34","43","52","64",
                "65","71","81","82","91"};
            for (int kk = 0; kk < Myaddress.Length; kk++)
            {
                if (Myaddress[kk].ToString() == CardId.Remove(2))
                {
                    string Mybirth = CardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                    if (!DateTime.TryParse(Mybirth, out _))
                    {
                        log.Info("身份证识别失败:" + CardId);
                        return false;//生日验证
                    }
                    string[] MyVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                    string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                    char[] ai = CardId.Remove(17).ToCharArray();
                    int sum = 0;
                    for (int i = 0; i < 17; i++)
                        sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
                    Math.DivRem(sum, 11, out int y);
                    if (MyVarifyCode[y] != CardId.Substring(17, 1).ToLower())
                    {
                        log.Info("身份证识别失败:" + CardId);
                        return false;//校验码验证
                    }
                    log.Info("身份证识别成功:" + CardId);
                    return true;//符合GB11643-1999标准
                }
            }
            log.Info("身份证识别失败:" + CardId);
            return false;
        }

        private bool IsValidDate(string valid_dateStr)
        {
            try
            {
                string[] valid_date = valid_dateStr.Split('-');

                if (valid_date != null && valid_date.Length == 2)
                {
                    DateTime start = Convert.ToDateTime(valid_date[0].Replace(".", "-"));
                    DateTime end;
                    if (valid_date[1].Trim() != "长期")
                    {
                        end = Convert.ToDateTime(valid_date[1].Replace(".", "-"));
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion 
    }

    /// <summary>
    /// 
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 
        /// </summary>
        public UploadFile()
        {
            //application/x-jpg
            //audio/x-ms-wax
            //ContentType = "application/octet-stream";
            //ContentType = "image/jpeg";
            //ContentType = "Content-type: application/x-jpg";
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Filename { get; set; }
        //public string ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }
    }

    /// <summary>
    /// 身份证识别
    /// </summary>
    public class IdentResult
    {
        /// <summary>
        /// 耗时
        /// </summary>
        public string time_used { get; set; }
        /// <summary>
        /// 当请求失败时才会返回此字符串
        /// </summary>
        public string error_message { get; set; }
        /// <summary>
        /// 每一次请求的唯一的字符串
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// 检测出证件的数组,注：如果没有检测出证件则为空数组
        /// </summary>
        public IdentCard[] cards { get; set; }
        /// <summary>
        /// 识别是否成功
        /// </summary>
        public bool Success
        {
            get { return string.IsNullOrEmpty(error_message) && cards != null && cards.Length > 0; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IdentCard
    {
        /// <summary>
        /// 性别（男/女）
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_card_number { get; set; }
        /// <summary>
        /// 生日，格式为YYYY-MM-DD
        /// </summary>
        public string birthday { get; set; }
        /// <summary>
        /// 民族（汉字）
        /// </summary>
        public string race { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string address { get; set; }
        ///// <summary>
        ///// 身份证照片的合法性检查结果，暂时无用
        ///// </summary>
        //public CardLegality legality { get; set; }
        /// <summary>
        /// 证件类型。返回1，代表是身份证
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 表示身份证的国徽面或人像面。返回值为：ront: 人像面，back: 国徽面
        /// </summary>
        public string side { get; set; }
        /// <summary>
        /// 签发机关
        /// </summary>
        public string issued_by { get; set; }
        /// <summary>
        /// 有效日期，返回值有两种格式：一个16位长度的字符串：YYYY.MM.DD-YYYY.MM.DD或是：YYYY.MM.DD-长期
        /// </summary>
        public string valid_date { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CardLegality
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal Edited { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Photocopy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ID Photo")]
        public decimal ID_Photo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Screen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("Temporary ID Photo")]
        public decimal Temporary_ID_Photo { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IdentResultAliyun
    {
        // "dataValue": "{\"config_str\":\"{\\\"side\\\":\\\"back\\\"}\",\"end_date\":\"20290223\",\"issue\":\"一重庆市公安葛合川川分局\",\"request_id\":\"20170607203849_05bccc92cb232094dceaa1a979aec9c8\",\"start_date\":\"20090223\",\"success\":true}"}}]}
        //{"address":"重庆市合川区钱塘镇梨子村4组73号","birth":"19760314","config_str":"{\"side\":\"face\"}","face_rect":{"angle":0,"center":{"x":209.99996948242188,"y":484},"size":{"height":101.99998474121094,"width":93.999984741210938}},"name":"谢小梅","nationality":"汉","num":"510226197603147586","request_id":"20170607193845_6775014334187b6413941293aaecdf3b","sex":"女","success":true}
        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string birth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string config_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FaceRect face_rect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nationality { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string start_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string end_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string request_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool success { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class FaceRect
    {
        /// <summary>
        /// 
        /// </summary>
        public int angle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Center center { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public cSize size { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Center
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal y { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class cSize
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal width { get; set; }
    }
}
