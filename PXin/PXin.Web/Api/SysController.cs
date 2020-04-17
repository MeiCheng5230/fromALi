using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade;
using PXin.Facade.ApiFacade;
using PXin.Facade.CommonService;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;

namespace PXin.Web.Api
{
    /// <summary>
    /// 基础API
    /// </summary>
    public class SysController : ApiController
    {
        /// <summary>
        /// 上传文件，没有签名验证
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [OverrideActionFilters]
        public Respbase<IdCardUploadFileDto> UploadFile(ReqUploadFile req)
        {
            FileService fileService = new FileService();
            if (req.ImageActionType == FileActionType.临时)
            {
                string fileName = $"/images2/tempfile/{DateTime.Now.ToString("yyyyMMdd")}/{Guid.NewGuid().ToString()}.{req.Typeid}";
                string physicsFileName = System.Web.Hosting.HostingEnvironment.MapPath(fileName);
                if (!Helper.Base64StringToImage(req.Content, physicsFileName, req.Typeid))
                {
                    return new Respbase<IdCardUploadFileDto> { Result = -1, Message = "保存文件失败", Data = null };
                }
                return new Respbase<IdCardUploadFileDto> { Data = new IdCardUploadFileDto { Url = fileName } };
            }
            else
            {
                var result = UploadImg(req);
                return result;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private Respbase<IdCardUploadFileDto> UploadImg(ReqUploadFile req)
        {
            FileService fileService = new FileService();
            if (!fileService.SaveFile(req))
            {
                return new Respbase<IdCardUploadFileDto> { Result = -1, Message = "保存文件失败", Data = null };
            }

            if (req.ImageActionType == FileActionType.身份证正面图片 || req.ImageActionType == FileActionType.身份证反面图片)
            {
                IdCardService idCardService = new IdCardService();
                var res = idCardService.IdCardPicRecognise((int)req.ImageActionType, fileService.PhysicsFilePath);
                if (res.Result == 1)
                {
                    return new Respbase<IdCardUploadFileDto>
                    {
                        Result = res.Result,
                        Data = new IdCardUploadFileDto
                        {
                            Url = fileService.FilePath,
                            IdentCard = res.Data
                        },
                        Message = res.Message
                    };
                }
                return new Respbase<IdCardUploadFileDto> { Result = res.Result, Data = new IdCardUploadFileDto { Url = fileService.Image2FilePath, }, Message = res.Message };
            }

            if (req.ImageActionType == FileActionType.驾驶证行驶证)
            {
                string photoPhysicalPath = FileService.GetPhysicsFilePath(fileService.FilePath);
                (bool success, string message, object result) = ScanLicense.GetInstance(LicenseTypeEnum.Driver).Execute(new ExecuteEntity() { PhotoPhysicalPath = photoPhysicalPath });
                if (!success)
                {
                    return new Respbase<IdCardUploadFileDto> { Result = -1, Message = "识别驾驶证失败", Data = new IdCardUploadFileDto { Url = fileService.Image2FilePath } };
                }
                DriverResult driResult = ((DriverResp)result).words_result;
                PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
                var tnetNodeInfo = db.TnetNodeinfoSet.FirstOrDefault(w => w.Nodeid == req.Nodeid);
                if (tnetNodeInfo == null)
                {
                    return new Respbase<IdCardUploadFileDto> { Result = -1, Message = "用户未进行身份认证", Data = new IdCardUploadFileDto { Url = fileService.Image2FilePath } };
                }
                if (tnetNodeInfo.Name.Trim() != driResult.姓名.words.Trim() || tnetNodeInfo.Idcardno.Trim() != driResult.证号.words.Trim())
                {
                    return new Respbase<IdCardUploadFileDto> { Result = -1, Message = "驾驶证信息与身份证信息不一致", Data = new IdCardUploadFileDto { Url = fileService.Image2FilePath } };
                }
            }

            return new Respbase<IdCardUploadFileDto> { Data = new IdCardUploadFileDto { Url = fileService.Image2FilePath } };
        }

        /// <summary>
        /// 发送验证码短信
        /// </summary>
        /// <param name="req">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase SendSms(ReqSms req)
        {
            SmsFacade smsFacade = new SmsFacade();
            smsFacade.SendSms(req.Typeid, req.Btypeid, req.Mobileno, req.Content, req.Sid);
            return smsFacade.PromptInfo;
        }
        /// <summary>
        /// 检查验证码是否正确
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase CheckVerificationCode(ReqVerificationCode req)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var regCode = db.TssoRegcodeSet.Where(c => c.Regcode == req.Mobileno && c.Authcode == req.Code && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
            if (regCode == null)
            {
                return new Respbase { Result = -1, Message = "验证码不正确或者失效" };
            }
            return new Respbase();
        }
        /// <summary>
        /// 获取基本配置信息
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<dynamic> GetConfig(ReqConfig req)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var predicate = db.CreatePredicate<TappConfig>();
            predicate = predicate.And(c => c.Sid == req.Sid);
            //if (req.Updatetime.HasValue)
            //{
            //  predicate = predicate.And(c => c.Updatetime >= req.Updatetime.Value);
            //}
            List<TappConfig> tappConfigs = db.TappConfigSet.AsNoTracking().Where(predicate).ToList();
            dynamic configData = new ExpandoObject();
            IDictionary<string, object> dict = configData as IDictionary<string, object>;
            List<TappH5Config> h5Configs = db.TappH5ConfigsSet.ToList();
            if(h5Configs != null && h5Configs.Count > 0)
            {
                dict.Add("h5config", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(h5Configs))));
            }
            
            if (tappConfigs.Count > 0)
            {
                foreach (var item in tappConfigs)
                {
                    if (req.Client == 3 && item.Propertyname.Equals("inviteurl", StringComparison.OrdinalIgnoreCase))
                    {
                        //ios 商店
                        dict.Add(item.Propertyname, "");
                    }
                    else
                    {
                        if (item.Propertyname == "FeeRules")
                        {
                            string[] rulesAttr = item.Propertyvalue.Split('-');
                            dict.Add("updatetime", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            dict.Add("wordpayunit", rulesAttr[0]);
                            dict.Add("picpayunit", rulesAttr[1]);
                            dict.Add("voicepayunit", rulesAttr[2]);
                            dict.Add("emojipayunit", rulesAttr[3]);
                            dict.Add("mappayunit", rulesAttr[4]);
                            dict.Add("sVideopayunit", rulesAttr[5]);
                            dict.Add("vCallpayunit", rulesAttr[6]);
                            dict.Add("videopayunit", rulesAttr[7]);
                            dict.Add("emoticonpayunit", rulesAttr[8]);
                            //语音通话: vCallPayUnit   小视频：sVideoPayUnit  视频通话：videoPayUnit
                        }
                        dict.Add(item.Propertyname, string.IsNullOrEmpty(item.Propertyvalue) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(item.Propertyvalue)));
                    }
                }
                return new Respbase<dynamic> { Data = configData };
            }
            else
            {
                return new Respbase<dynamic> { Result = 1, Message = "成功" };
            }
        }
        /// <summary>
        /// 获取本地部署H5配置信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<H5ConfigDto> GetH5Config(ReqH5Config req)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            req.Name = req.Name.ToLower();
            TappH5Config h5Config = db.TappH5ConfigsSet.FirstOrDefault(c => c.Name == req.Name);
            if(h5Config != null)
            {
                var result = new H5ConfigDto();
                result.ChargeStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(h5Config)));
                result.Sign = Common.Mvc.Md5.SignString(result.ChargeStr + AppConfig.AppSecurityString).ToUpper();
                return new Respbase<H5ConfigDto> {  Data = result};
            }
            return new Respbase<H5ConfigDto> { Data = null };
        }
        /// <summary>
        /// 获取手机国际区号
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [OverrideActionFilters]
        public Respbase<List<AreaCodeDto>> GetAreaCode(AreaCodeReq req)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var result = db.TnetAreacodeSet.OrderBy(p => p.Id).Select(p => new AreaCodeDto
            {
                Id = p.Id,
                Areaname = p.Areaname,
                Code = p.Code,
                Commonuse = p.Commonuse,
                Country = p.Country,
                Createtime = p.Createtime,
                EnCountry = p.EnCountry,
                Remarks = p.Remarks
            }).ToList();
            return new Respbase<List<AreaCodeDto>> { Data = result };
        }

        /// <summary>
        /// 验证Sign
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Anonymous]
        public Respbase CheckSign(Reqbase req)
        {
            if (!req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, CommonConfig.ApiAuthString), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, CommonConfig.CasAuthString), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, "DvUZIrmKXs"), StringComparison.OrdinalIgnoreCase)
            && !req.Sign.Equals(Helper.GetSign(req.Nodeid, req.Sid, req.Tm, ConfigurationManager.AppSettings["PcnAuthString"]), StringComparison.OrdinalIgnoreCase))
            {
                return new Respbase { Result = -1, Message = "签名错误" };
            }
            else
            {
                return new Respbase();
            }
        }

        /// <summary>
        /// App下载地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OverrideActionFilters]
        public Respbase<AppDownloadDto> GetAppDownloadUrl()
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var configs = db.TappConfigSet.AsNoTracking().Where(w => w.Sid == 81127).ToList();
            var iosProp = configs
                .Where(w => w.Propertyname.Equals(AppConfig.AppDownloadProperty_IOS, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var androidProp = configs
                .Where(w => w.Propertyname.Equals(AppConfig.AppDownloadProperty_Android, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return new Respbase<AppDownloadDto>
            {
                Data = new AppDownloadDto()
                {
                    Ios = iosProp == null ? "" : iosProp.Propertyvalue,
                    Android = androidProp == null ? "" : androidProp.Propertyvalue
                }
            };
        }

        /// <summary>
        /// TCP服务配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [OverrideActionFilters]
        public Respbase<TCPServerConfigDto> GetTCPServerConfig()
        {
            var apiurl = "api/Cache/GetTCPServerConfig";
            var url = AppConfig.WsxServiceAPIHost + (AppConfig.WsxServiceAPIHost.EndsWith("//") ? apiurl : "//" + apiurl);
            var result = Common.Mvc.HttpHelper.HttpSimulation.Instance.Request(url, null);
            return JsonConvert.DeserializeObject<Respbase<TCPServerConfigDto>>(result);
        }
    }
}
