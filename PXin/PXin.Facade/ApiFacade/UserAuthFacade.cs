using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.CommonService;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.UserDto;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAuthFacade : FacadeBase<PXinContext>
    {
        readonly string authImagePath = $"/images2/userauth/{DateTime.Now.ToString("yyyyMMdd")}/";
        readonly string driverImagePath = $"/images2/driverlicense/{DateTime.Now.ToString("yyyyMMdd")}/";
        #region 用户认证
        /// <summary>
        /// 获取用户认证信息
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public UserAuthInfoDto GetUserAuthInfo(int nodeid)
        {
            var regInfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodeid == nodeid);
            var authLog = db.TzcAuthLogSet.FirstOrDefault(f => f.Nodeid == nodeid);
            var nodeInfo = db.TnetNodeinfoSet.FirstOrDefault(f => f.Nodeid == nodeid);
            if (regInfo.Isconfirmed == 1)
            {
                if (authLog == null)
                {
                    Alert("服务器数据异常1，请联系管理员！", -1);
                    return null;
                }
                if (nodeInfo == null)
                {
                    Alert("服务器数据异常2，请联系管理员！", -2);
                    return null;
                }
            }
            else
            {
                return new UserAuthInfoDto()
                {
                    UserName = regInfo.Nodename?.Substring(0,1)+"**",
                    IdCardNo = (regInfo.Isconfirmed == 1 && authLog != null) ? authLog.Idcard.Substring(0, 6) + "********" + authLog.Idcard.Substring(14) : "无",
                    Status = authLog != null ? authLog.Status : -2,
                    EndTime = (regInfo.Isconfirmed == 1 && regInfo.Authtime.HasValue) ? regInfo.Authtime.Value.AddDays(365).ToString("yyyy-MM-dd") : "尚未认证",
                };
            }
            return new UserAuthInfoDto()
            {
                UserName = regInfo.Nodename?.Substring(0, 1) + "**",
                IdCardNo = (regInfo.Isconfirmed == 1 && authLog != null) ? authLog.Idcard.Substring(0, 6) + "********" + authLog.Idcard.Substring(14) : "无",
                Status = authLog != null ? authLog.Status : -2,
                EndTime = (regInfo.Isconfirmed == 1 && regInfo.Authtime.HasValue) ? regInfo.Authtime.Value.AddDays(365).ToString("yyyy-MM-dd") : "尚未认证",
            };
        }

        /// <summary>
        /// 用户认证(通过钱包)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public UserAuthInfoDto UserAuthByPurse(AuthenByPurseReq req)
        {
            if (!IsCanAuth(req, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog, out IdentResult identResultFront, out IdentResult identResultBack))
            {
                return null;
            }
            if (!CopyAuthPicToReallyDir(req))
            {
                return null;
            }
            if (!SaveAuthInfo(nodeInfo, authLog, identResultFront, identResultBack, req))
            {
                db.Rollback();
                DeleteAuthPic(new string[] { req.CardFrontPicUrl, req.CardBackPicUrl, req.HoldCardPicUrl });
                return null;
            }
            RemoveRegInfoCache(req.Nodeid);
            return GetUserAuthInfo(req.Nodeid);
        }

        /// <summary>
        /// 获取Pcn认证用户的身份证证明照
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetUserIDCardFrontPicFromPcn(GetIDCardPicFromPcnReq req)
        {
            PcnAcountInfoDto pcnReginfo = GetRegInfoFromPcn(req.PcnNodecode);
            if (pcnReginfo == null)
            {
                return null;
            }
            if (!CheckPcnPwd(req))
            {
                return null;
            }
            TzcAuthLog pcnAuthLog = GetAuthLogFromPcnByPcnUserNodeid(req.PcnNodecode, req);
            if (pcnAuthLog == null)
            {
                return null;
            }
            if (db.TzcAuthBindpcnSet.Where(w => w.Pcnnodeid == pcnReginfo.NodeId || w.Nodeid == req.Nodeid).Count() > 0)
            {
                Alert("此PCN账号已被其他用户绑定");
                return null;
            }
            var otherAuthLog = db.TzcAuthLogSet.Where(w => w.Idcard == pcnAuthLog.Idcard && w.Nodeid != req.Nodeid);
            if (otherAuthLog.Count() > 0)
            {
                Alert("此PCN账号身份证信息已被人使用");
                log.Info("PCN认证：PNCNodeid=" + pcnReginfo.NodeId + "的身份证：" + pcnAuthLog.Idcard + "存在相信认证库中");
                return null;
            }
            return pcnAuthLog.Idcardpic1;
        }

        private bool CheckPcnPwd(GetIDCardPicFromPcnReq req)
        {
            var myRet = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/user/Login", new { Nodecode = req.PcnNodecode, PassWord = req.PcnLoginPwd, Version = "1.0.0", Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid });
            var ret = JsonConvert.DeserializeObject<Respbase<PcnAcountInfoDto>>(myRet);
            if (ret.Result <= 0)
            {
                log.Info("CheckPcnPwd失败，" + myRet);
                Alert(ret.Message);
                return false;
            }
            return true;
        }

        private TzcAuthLog GetAuthLogFromPcnByPcnUserNodeid(string pcnNodecode, Reqbase req)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sign = Md5.SignString(time + AppConfig.AppSecurityString);
            var myRet = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/auth/GetAuthLog", new { Nodecode = pcnNodecode, Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid, ReqTime = time, AppSign = sign });
            var ret = JsonConvert.DeserializeObject<Respbase<TzcAuthLog>>(myRet);
            if (ret.Result <= 0 || string.IsNullOrEmpty(ret.Data.Idcardpic1))
            {
                log.Info("GetAuthLog，" + myRet);
                Alert("此PCN账号未进行认证");
                return null;
            }
            return ret.Data;
        }

        /// <summary>
        /// 通过PCN认证
        /// </summary>
        /// <returns></returns>
        public UserAuthInfoDto UserAuthByPCN(AuthByPCNReq req, bool isAdminAuth = false)
        {
            log.Info("认证开始:" + JsonConvert.SerializeObject(req));
            PcnAcountInfoDto pcnReginfo = GetRegInfoFromPcn(req.PcnNodecode);
            if (pcnReginfo == null)
            {
                return null;
            }
            TzcAuthLog pcnAuthLog = GetAuthLogFromPcnByPcnUserNodeid(pcnReginfo.NodeCode, req);
            if (pcnAuthLog == null)
            {
                return null;
            }
            if (!IsCanAuthByPcn(req, pcnReginfo, pcnAuthLog, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog, isAdminAuth))
            {
                return null;
            }
            if (!CopyAuthPicToLocalFromPcn(req.Nodeid, pcnAuthLog, out string oppositeUrlIDCardPicFront, out string oppositeUrlIDCardPicBack, out string livingPicReallyUrl))
            {
                return null;
            }
            if (!isAdminAuth && !CopyLivingPicToReallyDir(req, out livingPicReallyUrl))
            {
                DeleteAuthPic(new string[] { oppositeUrlIDCardPicFront, oppositeUrlIDCardPicBack });
                return null;
            }
            if (!SaveAuthInfoFromPcn(req.Nodeid, pcnAuthLog, nodeInfo, authLog, oppositeUrlIDCardPicFront, oppositeUrlIDCardPicBack, livingPicReallyUrl))
            {
                return null;
            }
            RemoveRegInfoCache(req.Nodeid);
            return GetUserAuthInfo(req.Nodeid);
        }

        /// <summary>
        /// 通过Nodecode获取PCN系统中的用户信息
        /// </summary>
        /// <param name="pcnNodeCode"></param>
        /// <returns></returns>
        private PcnAcountInfoDto GetRegInfoFromPcn(string pcnNodeCode)
        {
            var httpSimulation = HttpSimulation.Instance;
            int sid = 81123;
            string tm = DateTime.Now.ToString("yyyyMMddhhmmss");
            string sign = Common.Facade.Helper.GetSign(3434909, sid, tm, CommonConfig.ApiAuthString);
            var myRet = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/user/GetUserInfo", new { Nodecode = pcnNodeCode, Client = 1, Tm = tm, Sign = sign, Sid = sid, Nodeid = 3434909 });
            var respResult = JsonConvert.DeserializeObject<Respbase<PcnAcountInfoDto>>(myRet);
            if (respResult.Result <= 0|| respResult.Data == null)
            {
                Alert("PCN用户账号不存在");
                return null;
            }
            return respResult.Data;
        }
        /// <summary>
        /// 删除已上传的认证照片
        /// </summary>
        /// <param name="authUrls"></param>
        private void DeleteAuthPic(string[] authUrls)
        {
            string path = HttpContext.Current.Server.MapPath(authImagePath);
            foreach (var item in authUrls)
            {
                try
                {
                    File.Delete(path + Path.GetFileName(item));
                }
                catch (Exception)
                {
                    log.Info($"删除认证图片失败：{item}");
                }
            }
        }
        /// <summary>
        /// 拷贝认证照片到真实路径
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CopyAuthPicToReallyDir(AuthenByPurseReq req)
        {
            bool copySuccess = true;
            string dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            string frontUrl = authImagePath + req.Nodeid + "-" + dateTime + "-Front" + Path.GetExtension(req.CardFrontPicUrl);
            var sourceDir = $"/images2/tempfile/{DateTime.Now.ToString("yyyyMMdd")}/";
            var cutPath = "/images2";
            if (copySuccess && !Common.Facade.Helper.CopyFile(HttpContext.Current.Server.MapPath(sourceDir + Path.GetFileName(req.CardFrontPicUrl)), HttpContext.Current.Server.MapPath(frontUrl)))
            {
                copySuccess = false;
            }
            req.CardFrontPicUrl = AppConfig.ImageBaseUrl + frontUrl.Replace(cutPath, "");

            string backUrl = authImagePath + req.Nodeid + "-" + dateTime + "-Back" + Path.GetExtension(req.CardBackPicUrl);
            if (copySuccess && !Common.Facade.Helper.CopyFile(HttpContext.Current.Server.MapPath(sourceDir + Path.GetFileName(req.CardBackPicUrl)), HttpContext.Current.Server.MapPath(backUrl)))
            {
                copySuccess = false;
            }
            req.CardBackPicUrl = AppConfig.ImageBaseUrl + backUrl.Replace(cutPath, "");

            string holdUrl = authImagePath + req.Nodeid + "-" + dateTime + "-Hold" + Path.GetExtension(req.HoldCardPicUrl);
            if (copySuccess && !Common.Facade.Helper.CopyFile(HttpContext.Current.Server.MapPath(sourceDir + Path.GetFileName(req.HoldCardPicUrl)), HttpContext.Current.Server.MapPath(holdUrl)))
            {
                copySuccess = false;
            }
            req.HoldCardPicUrl = AppConfig.ImageBaseUrl + holdUrl.Replace(cutPath, "");
            if (!copySuccess)
            {
                DeleteAuthPic(new string[] { req.CardFrontPicUrl, req.CardBackPicUrl, req.HoldCardPicUrl });
                log.Error("CopyAuthPicToReallyDir", "认证失败");
                Alert("认证失败");
            }
            return copySuccess;
        }

        /// <summary>
        /// 保存用户认证信息
        /// </summary>
        /// <param name="oldnodeInfo"></param>
        /// <param name="oldAuthLog"></param>
        /// <param name="identResultFront"></param>
        /// <param name="identResultBack"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool SaveAuthInfo(TnetNodeinfo oldnodeInfo, TzcAuthLog oldAuthLog, IdentResult identResultFront, IdentResult identResultBack, AuthenByPurseReq req)
        {
            bool isFirst = oldAuthLog == null;
            if (isFirst)
            {
                oldAuthLog = new TzcAuthLog()
                {
                    Payment = -1
                };
                oldnodeInfo = new TnetNodeinfo() { };
            }
            BuildAuthLog(req, oldAuthLog, identResultFront, identResultBack);
            BuildNodeInfo(oldnodeInfo, oldAuthLog);
            if (isFirst)
            {
                db.TzcAuthLogSet.Add(oldAuthLog);
                db.TnetNodeinfoSet.Add(oldnodeInfo);
            }
            var regInfo = db.TnetReginfoSet.FirstOrDefault(f => f.Nodeid == req.Nodeid);
            regInfo.Isconfirmed = 1;
            regInfo.Nodename = oldAuthLog.Realname;
            regInfo.Authtime = DateTime.Now;
            if (db.SaveChanges() < 1)
            {
                log.Error("SaveAuthInfo", "认证失败");
                Alert("认证失败");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存用户认真信息(PCN认证)
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pcnAuthLog"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="authLog"></param>
        /// <param name="oppositeUrlIDCardPicFront"></param>
        /// <param name="oppositeUrlIDCardPicBack"></param>
        /// <param name="oppositUrlHoldIDCard"></param>
        /// <returns></returns>
        private bool SaveAuthInfoFromPcn(int nodeid, TzcAuthLog pcnAuthLog, TnetNodeinfo nodeInfo, TzcAuthLog authLog, string oppositeUrlIDCardPicFront, string oppositeUrlIDCardPicBack, string oppositUrlHoldIDCard)
        {
            bool isFirst = authLog == null;
            if (isFirst)
            {
                authLog = new TzcAuthLog();
                nodeInfo = new TnetNodeinfo();
            }
            BuildAuthLog(nodeid, pcnAuthLog, authLog,
               Common.Facade.Helper.DomainUrl + oppositeUrlIDCardPicFront,
               Common.Facade.Helper.DomainUrl + oppositeUrlIDCardPicBack,
               Common.Facade.Helper.DomainUrl + oppositUrlHoldIDCard);
            BuildNodeInfo(nodeInfo, authLog);
            if (isFirst)
            {
                db.TzcAuthLogSet.Add(authLog);
                db.TnetNodeinfoSet.Add(nodeInfo);
                db.TzcAuthBindpcnSet.Add(new TzcAuthBindpcn { Nodeid = authLog.Nodeid, Pcnnodeid = pcnAuthLog.Nodeid, Remarks = "" });
            }
            var regInfo = db.TnetReginfoSet.FirstOrDefault(f => f.Nodeid == nodeid);
            regInfo.Isconfirmed = 1;
            regInfo.Nodename = authLog.Realname;
            regInfo.Authtime = DateTime.Now;
            if (!(db.SaveChanges() > 0))
            {
                Alert("认证失败");
                log.Info("认证失败:" + db.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取用户注册信息
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <param name="authLog"></param>
        private void BuildNodeInfo(TnetNodeinfo nodeInfo, TzcAuthLog authLog)
        {
            nodeInfo.Nodeid = authLog.Nodeid;
            nodeInfo.Idcardno = authLog.Idcard;
            nodeInfo.Name = authLog.Realname;
            nodeInfo.Birthday = authLog.Birthday.Value;
            nodeInfo.Sex = authLog.Sex;
            if (!string.IsNullOrEmpty(authLog.Race))
            {
                nodeInfo.Nation = authLog.Race;
                nodeInfo.Idcardaddr = authLog.Address;
                nodeInfo.Issuing = authLog.IssuedBy;
                string[] valid_date = authLog.ValidDate.Split('-');
                if (valid_date != null && valid_date.Length == 2)
                {
                    nodeInfo.Beginvalidity = Convert.ToDateTime(valid_date[0].Replace(".", "-"));
                    if (valid_date[1].Trim() == "长期")
                    {
                        nodeInfo.Endvalidity = new DateTime(2099, 1, 1);
                    }
                    else
                    {
                        nodeInfo.Endvalidity = Convert.ToDateTime(valid_date[1].Replace(".", "-"));
                    }
                }
            }
        }
        /// <summary>
        /// 获取用户认证日志
        /// </summary>
        /// <param name="req"></param>
        /// <param name="authLog"></param>
        /// <param name="identResultFront"></param>
        /// <param name="identResultBack"></param>
        private void BuildAuthLog(AuthenByPurseReq req, TzcAuthLog authLog, IdentResult identResultFront, IdentResult identResultBack)
        {
            authLog.Nodeid = req.Nodeid;
            authLog.Createtime = DateTime.Now;
            authLog.Remarks = "用户认证";
            authLog.Realname = identResultFront.cards[0].name;
            try
            {
                authLog.Birthday = Convert.ToDateTime(identResultFront.cards[0].birthday);
            }
            catch (Exception err)
            {
                log.Info(identResultFront.cards[0].birthday + "转换为生日失败，" + err.ToString());
            }
            authLog.Race = identResultFront.cards[0].race;
            authLog.Sex = identResultFront.cards[0].gender == "女" ? 2 : 1;
            authLog.Idcard = identResultFront.cards[0].id_card_number;
            authLog.Address = identResultFront.cards[0].address;
            authLog.IssuedBy = identResultBack.cards[0].issued_by;
            authLog.ValidDate = identResultBack.cards[0].valid_date;
            //分解身份证身日性别
            if (authLog.Idcard.Length == 18 && !authLog.Birthday.HasValue)
            {
                //（1）前1、2位数字表示：所在省份的代码； 
                //（2）第3、4位数字表示：所在城市的代码； 
                //（3）第5、6位数字表示：所在区县的代码； 
                //（4）第7~14位数字表示：出生年、月、日； 
                //（5）第15、16位数字表示：所在地的派出所的代码； 
                //（6）第17位数字表示性别：奇数表示男性，偶数表示女性； 
                //（7）第18位数字是校检码：也有的说是个人信息码，用来检验身份证的正确性。校检码可以是0~9的数字，有时也用x表示(尾号是10，那么就得用X来代替)。 一般是随计算机的随机产生。
                authLog.Sex = Convert.ToInt32(authLog.Idcard.Substring(16, 1)) % 2 == 0 ? 2 : 1;
                authLog.Birthday = new DateTime(Convert.ToInt32(authLog.Idcard.Substring(6, 4)),
                    Convert.ToInt32(authLog.Idcard.Substring(10, 2)),
                    Convert.ToInt32(authLog.Idcard.Substring(12, 2)));
            }
            authLog.Idcardpic1 = req.CardFrontPicUrl;
            authLog.Idcardpic2 = req.HoldCardPicUrl;
            authLog.Idcardpic3 = req.CardBackPicUrl;
            authLog.Isidentify = 1;
            authLog.Baiduapiret = req.BaiDuRet;
            authLog.Status = 5;             //通过实人认证，不需要再审核了
        }
        /// <summary>
        /// 获取用户认证日志
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pcnAuthLog"></param>
        /// <param name="authLog"></param>
        /// <param name="UrlIDCardPicFront"></param>
        /// <param name="UrlIDCardPicBack"></param>
        /// <param name="UrlHoldIDCard"></param>
        private void BuildAuthLog(int nodeid, TzcAuthLog pcnAuthLog, TzcAuthLog authLog, string UrlIDCardPicFront, string UrlIDCardPicBack, string UrlHoldIDCard)
        {
            //http://mall2.51yougu.cn
            //images2/userauth/20190613/3502034-20190613060747-Front
            //todo:图片路径问题
            authLog.Nodeid = nodeid;
            authLog.Createtime = DateTime.Now;
            authLog.Remarks = "用户认证";
            authLog.Realname = pcnAuthLog.Realname;
            authLog.Birthday = pcnAuthLog.Birthday;
            authLog.Payment = 2;
            authLog.Race = pcnAuthLog.Race;
            authLog.Sex = pcnAuthLog.Sex;
            authLog.Idcard = pcnAuthLog.Idcard;
            authLog.Address = pcnAuthLog.Address;
            authLog.IssuedBy = pcnAuthLog.IssuedBy;
            authLog.ValidDate = pcnAuthLog.ValidDate;
            authLog.Idcardpic1 = UrlIDCardPicFront.Replace(Common.Facade.Helper.DomainUrl + "/images2", AppConfig.ImageBaseUrl);
            authLog.Idcardpic2 = UrlHoldIDCard.Replace(Common.Facade.Helper.DomainUrl + "/images2", AppConfig.ImageBaseUrl);
            authLog.Idcardpic3 = UrlIDCardPicBack.Replace(Common.Facade.Helper.DomainUrl + "/images2", AppConfig.ImageBaseUrl);
            authLog.Isidentify = 1;
            authLog.Baiduapiret = pcnAuthLog.Baiduapiret;
            authLog.Status = 5;                 //通过实人认证，不需要再审核了
            authLog.Remarks = "PCN绑定认证";
        }
        /// <summary>
        /// 是否可进行注册
        /// </summary>
        /// <param name="req"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="authLog"></param>
        /// <param name="identResultFront"></param>
        /// <param name="identResultBack"></param>
        /// <returns></returns>
        private bool IsCanAuth(AuthenByPurseReq req, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog, out IdentResult identResultFront, out IdentResult identResultBack)
        {
            identResultFront = null;
            identResultBack = null;
            if (!AuthBasicCheck(req.Nodeid, out TnetReginfo regInfo, out nodeInfo, out authLog))
            {
                return false;
            }
            if (regInfo.Isconfirmed == 1 && nodeInfo != null && authLog != null && authLog.Status == 5)
            {
                Alert("已通过实人认证，不要重复认证");
                return false;
            }
            identResultFront = GetIdentResult(req.CardFrontPicUrl);
            if (identResultFront == null)
            {
                Alert("未找到身份证正面照识别结果");
                return false;
            }
            identResultBack = GetIdentResult(req.CardBackPicUrl);
            if (identResultBack == null)
            {
                Alert("未找到身份证反面照识别结果");
                return false;
            }
            var cardNumber = identResultFront.cards[0].id_card_number;
            if (db.TzcAuthLogSet.Where(s => s.Nodeid != regInfo.Nodeid && s.Idcard == cardNumber).Count() > 0
                || db.TnetNodeinfoSet.Where(s => s.Nodeid != regInfo.Nodeid && s.Idcardno == cardNumber).Count() > 0
                )
            {
                Alert("身份证[" + identResultFront.cards[0].id_card_number + "]已存在");
                log.Info("身份证[" + identResultFront.cards[0].id_card_number + "]已存在,nodeid=" + regInfo.Nodeid);
                return false;
            }
            return true;
        }

        private bool IsCanAuthByPcnAdmin(AuthByPCNReq req, PcnAcountInfoDto pcnReginfo, TzcAuthLog pcnAuthLog, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog)
        {
            if (!AuthBasicCheck(req.Nodeid, out TnetReginfo regInfo, out nodeInfo, out authLog, true))
            {
                return false;
            }
            if (regInfo.Isconfirmed == 1 && nodeInfo != null && authLog != null && authLog.Status == 5)
            {
                Alert("已通过实人认证，不要重复认证");
                return false;
            }
            if (db.TzcAuthBindpcnSet.Where(w => w.Pcnnodeid == pcnReginfo.NodeId || w.Nodeid == req.Nodeid).Count() > 0)
            {
                Alert("用户已通过PCN认证或此PCN账号已绑定其他用户认证");
                return false;
            }
            var otherAuthLog = db.TzcAuthLogSet.Where(w => w.Idcard == pcnAuthLog.Idcard && w.Nodeid != req.Nodeid);
            if (otherAuthLog.Count() > 0)
            {
                Alert("此PCN账号身份证信息已被人使用");
                log.Info("PCN认证：PNCNodeid=" + pcnReginfo.NodeId + "的身份证：" + pcnAuthLog.Idcard + "存在相信认证库中");
                return false;
            }
            if (string.IsNullOrEmpty(pcnAuthLog.Idcardpic1) || string.IsNullOrEmpty(pcnAuthLog.Idcardpic2) || string.IsNullOrEmpty(pcnAuthLog.Idcardpic3))
            {
                Alert("此PCN账号身份证图片不完整，请联系管理员");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否可进行注册(PCN认证)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="pcnReginfo"></param>
        /// <param name="pcnAuthLog"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="authLog"></param>
        /// <returns></returns>
        private bool IsCanAuthByPcn(AuthByPCNReq req, PcnAcountInfoDto pcnReginfo, TzcAuthLog pcnAuthLog, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog, bool isAdmin = false)
        {
            if (isAdmin)
            {
                return IsCanAuthByPcnAdmin(req, pcnReginfo, pcnAuthLog, out nodeInfo, out authLog);
            }
            if (!AuthBasicCheck(req.Nodeid, out TnetReginfo regInfo, out nodeInfo, out authLog, true))
            {
                return false;
            }
            if (regInfo.Isconfirmed == 1 && nodeInfo != null && authLog != null && authLog.Status == 5)
            {
                Alert("已通过实人认证，不要重复认证");
                return false;
            }
            if (!CheckPcnPwd(new GetIDCardPicFromPcnReq { Client = req.Client, Nodeid = req.Nodeid, PcnLoginPwd = req.PcnLoginPwd, PcnNodecode = req.PcnNodecode, Sid = req.Sid, Sign = req.Sign, Tm = req.Tm }))
            {
                Alert("PCN登陆密码错误");
                return false;
            }
            if (db.TzcAuthBindpcnSet.Where(w => w.Pcnnodeid == pcnReginfo.NodeId || w.Nodeid == req.Nodeid).Count() > 0)
            {
                Alert("用户已通过PCN认证或此PCN账号已绑定其他用户认证");
                return false;
            }
            var otherAuthLog = db.TzcAuthLogSet.Where(w => w.Idcard == pcnAuthLog.Idcard && w.Nodeid != req.Nodeid);
            if (otherAuthLog.Count() > 0)
            {
                Alert("此PCN账号身份证信息已被人使用");
                log.Info("PCN认证：PNCNodeid=" + pcnReginfo.NodeId + "的身份证：" + pcnAuthLog.Idcard + "存在相信认证库中");
                return false;
            }
            if (string.IsNullOrEmpty(pcnAuthLog.Idcardpic1) || string.IsNullOrEmpty(pcnAuthLog.Idcardpic2) || string.IsNullOrEmpty(pcnAuthLog.Idcardpic3))
            {
                Alert("此PCN账号身份证图片不完整，请联系管理员");
                return false;
            }
            if (!req.PcnIDCardFrontPicUrl.Contains(pcnAuthLog.Idcardpic1))
            {
                Alert("身份证正面照非此PCN用户");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 认证参数基础检查
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="regInfo"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="authLog"></param>
        /// <param name="authByPcn"></param>
        /// <returns></returns>
        private bool AuthBasicCheck(int nodeid, out TnetReginfo regInfo, out TnetNodeinfo nodeInfo, out TzcAuthLog authLog, bool authByPcn = false)
        {
            regInfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodeid == nodeid);
            nodeInfo = db.TnetNodeinfoSet.FirstOrDefault(f => f.Nodeid == nodeid);
            authLog = db.TzcAuthLogSet.FirstOrDefault(f => f.Nodeid == nodeid);
            if (regInfo.Isconfirmed == 1 && (nodeInfo == null || authLog == null))
            {
                Alert("数据异常，请联系管理员");
                log.Info($"用户：{nodeid} 认证数据异常");
                return false;
            }
            if (regInfo.Isconfirmed == 0 && (nodeInfo != null || authLog != null))
            {
                Alert("数据异常，请联系管理员");
                log.Info($"用户：{nodeid} 认证数据异常");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取百度身份证识别内容
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns></returns>
        private IdentResult GetIdentResult(string fullFileName)
        {
            string fileName = Path.GetFileName(fullFileName);
            try
            {
                return JsonConvert.DeserializeObject<IdentResult>(db.TzcIdcardrecLogSet.FirstOrDefault(f => f.Pic == fileName).Recresult);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 清除用户信息缓存
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private bool RemoveRegInfoCache(int nodeid)
        {
            return CommonApiTransfer.Instance.RemoveTnetReginfoCache(new GetRegInfoReq() { RegInfoKey = nodeid.ToString(), KeyType = 0 });
        }
        /// <summary>
        /// 拷贝PCN用户认证身份证到本地
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pcnAuthLog"></param>
        /// <param name="oppositeUrlIDCardPicFront"></param>
        /// <param name="oppositeUrlIDCardPicBack"></param>
        /// <param name="oppositUrlHoldIDCard"></param>
        /// <returns></returns>
        private bool CopyAuthPicToLocalFromPcn(int nodeid, TzcAuthLog pcnAuthLog, out string oppositeUrlIDCardPicFront, out string oppositeUrlIDCardPicBack, out string oppositUrlHoldIDCard)
        {
            string tm = DateTime.Now.ToString("yyyyMMddhhmmss");
            oppositeUrlIDCardPicFront = authImagePath + nodeid + "-" + tm + "-Front" + Path.GetExtension(pcnAuthLog.Idcardpic1);
            if (!Common.Facade.Helper.DownLoadFileToLocalByUrl(pcnAuthLog.Idcardpic1, oppositeUrlIDCardPicFront))
            {
                Alert("没有找到PCN账号身份证正面照片，请联系客服");
                oppositeUrlIDCardPicBack = "";
                oppositUrlHoldIDCard = "";
                return false;
            }

            oppositeUrlIDCardPicBack = authImagePath + nodeid + "-" + tm + "-Back" + Path.GetExtension(pcnAuthLog.Idcardpic3);
            if (!Common.Facade.Helper.DownLoadFileToLocalByUrl(pcnAuthLog.Idcardpic3, oppositeUrlIDCardPicBack))
            {
                //Alert("没有找到PCN账号身份证反面照片，请联系客服");
                //File.Delete(HttpContext.Current.Server.MapPath(oppositeUrlIDCardPicFront));
                oppositeUrlIDCardPicBack = "";
                //return false;
            }

            oppositUrlHoldIDCard = authImagePath + nodeid + "-" + tm + "-Hold" + Path.GetExtension(pcnAuthLog.Idcardpic2);
            if (!Common.Facade.Helper.DownLoadFileToLocalByUrl(pcnAuthLog.Idcardpic2, oppositUrlHoldIDCard))
            {
                oppositUrlHoldIDCard = "";
                //    Alert("没有找到PCN账号身份证手持照片，请联系客服");
                //    File.Delete(HttpContext.Current.Server.MapPath(oppositeUrlIDCardPicFront));
                //    File.Delete(HttpContext.Current.Server.MapPath(oppositeUrlIDCardPicBack));
                //    return false;
            }
            return true;
        }
        /// <summary>
        /// 拷贝活体照片到真实路径
        /// </summary>
        /// <param name="req"></param>
        /// <param name="livingPicReallyUrl"></param>
        /// <returns></returns>
        private bool CopyLivingPicToReallyDir(AuthByPCNReq req, out string livingPicReallyUrl)
        {
            string dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            livingPicReallyUrl = authImagePath + req.Nodeid + "-" + dateTime + "-Living_PCNAuth" + Path.GetExtension(req.LivingPicUrl);
            if (!Common.Facade.Helper.CopyFile(HttpContext.Current.Server.MapPath($"/images2/tempfile/{DateTime.Now.ToString("yyyyMMdd")}/" + Path.GetFileName(req.LivingPicUrl)), HttpContext.Current.Server.MapPath(livingPicReallyUrl)))
            {
                log.Info($"认证失败,复制文件错误：{livingPicReallyUrl}");
                Alert("认证失败");
                return false;
            }
            //livingPicReallyUrl = Common.Facade.Helper.DomainUrl + livingPicReallyUrl;
            return true;
        }
        #endregion

        #region 驾驶证绑定
        /// <summary>
        /// 驾驶证绑定
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DriverLicenseDto AuthDriverLicense(AuthDriverLicenseReq req)
        {
            List<string> oldImgPath = new List<string>();
            req.FrontImgUrl = req.FrontImgUrl.Replace(Common.Facade.Helper.DomainUrl, "").Replace(AppConfig.FileRootDir,"");
            req.AppendixImgUrl = req.FrontImgUrl.Replace(Common.Facade.Helper.DomainUrl, "").Replace(AppConfig.FileRootDir, "");
            if (!File.Exists(FileService.GetPhysicsFilePath(req.FrontImgUrl)) ||
                !File.Exists(FileService.GetPhysicsFilePath(req.AppendixImgUrl))
                )
            {
                Alert("图片不存在，请先上传图片");
                return null;
            }
            var reginfo = HttpContext.Current.GetRegInfo();
            if (reginfo.Isconfirmed == 0)
            {
                Alert("您必须通过实名认证后才能绑定驾驶证");
                return null;
            }
            var driveLicLog = db.TnetDriveLicLogSet.FirstOrDefault(w => w.Nodeid == req.Nodeid);
            if (driveLicLog != null)
            {
                if (req.FrontImgUrl == driveLicLog.Cardimg && req.FileNo.Equals(driveLicLog.Fileno))
                {
                    return new DriverLicenseDto
                    { DriverLicenseUrl = driveLicLog.Cardimg, AppendixUrl = driveLicLog.CardimgAppendix, FileNo = driveLicLog.Fileno, Name = driveLicLog.Name, Status = driveLicLog.Status };
                }
                oldImgPath.Add(new Uri(driveLicLog.Cardimg).LocalPath.Replace("/driverlicense/", "/images2/driverlicense/"));
                oldImgPath.Add(new Uri(driveLicLog.CardimgAppendix).LocalPath.Replace("/driverlicense/", "/images2/driverlicense/"));
            }
            if (driveLicLog == null)
            {
                driveLicLog = new TnetDriveLicLog();
            }
            string photoPhysicalPath = FileService.GetPhysicsFilePath(req.FrontImgUrl); //(uri_driver.LocalPath);
            (bool success, string message, object result) = ScanLicense.GetInstance(LicenseTypeEnum.Driver).Execute(new ExecuteEntity() { PhotoPhysicalPath = photoPhysicalPath });
            if (!success)
            {
                Alert(message);
                return null;
            }
            DriverResult driResult = ((DriverResp)result).words_result;
            var tnetNodeInfo = db.TnetNodeinfoSet.FirstOrDefault(w => w.Nodeid == req.Nodeid);
            if (tnetNodeInfo == null)
            {
                Alert("用户认证信息错误，请联系管理员");
                return null;
            }
            if (tnetNodeInfo.Name.Trim() != driResult.姓名.words.Trim() || tnetNodeInfo.Idcardno.Trim() != driResult.证号.words.Trim())
            {
                Alert("驾驶证信息需要与身份证信息一致才能通过审核");
                //Alert("您上传的驾驶证姓名【" + driResult.姓名.words + "】与认证用户名字【" + tnetNodeInfo.Name + "】不一致");
                return null;
            }
            //if (tnetNodeInfo.Idcardno.Trim() != driResult.证号.words.Trim())
            //{
            //    Alert("您上传的驾驶证证件号【" + driResult.证号.words + "】与认证用户证件号【" + tnetNodeInfo.Idcardno + "】不一致");
            //    return false;
            //}
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string reallyPath_front = driverImagePath + req.Nodeid + "-" + datetime + "-Driver" + Path.GetExtension(req.FrontImgUrl);
            string reallyPath_appendix = driverImagePath + req.Nodeid + "-" + datetime + "-Appendix" + Path.GetExtension(req.AppendixImgUrl);
            if (
                !Common.Facade.Helper.CopyFile(FileService.GetPhysicsFilePath(req.FrontImgUrl), HttpContext.Current.Server.MapPath(reallyPath_front)) ||
                !Common.Facade.Helper.CopyFile(FileService.GetPhysicsFilePath(req.AppendixImgUrl), HttpContext.Current.Server.MapPath(reallyPath_appendix))
               )
            {
                log.Info("拷贝图片失败：{" + req.FrontImgUrl + "}To{" + reallyPath_front + "}");
                log.Info("{" + req.AppendixImgUrl + "}To{" + reallyPath_appendix + "}");
                DeleteFile(new string[] { reallyPath_front });
                Alert("驾驶证绑定失败");
                return null;
            }
            req.FrontImgUrl = reallyPath_front;
            req.AppendixImgUrl = reallyPath_appendix;
            BuildTnetDriverLicLog(driveLicLog, driResult, req);
            if (driveLicLog.Id == 0)
            {
                db.TnetDriveLicLogSet.Add(driveLicLog);
            }
            if (db.SaveChanges() <= 0)
            {
                DeleteFile(new string[] { reallyPath_front, reallyPath_appendix });
                Alert("驾驶证绑定失败");
                return null;
            }
            if (oldImgPath.Count > 0)
            {
                DeleteFile(oldImgPath.ToArray());
            }
            return new DriverLicenseDto
            { DriverLicenseUrl = driveLicLog.Cardimg, AppendixUrl = driveLicLog.CardimgAppendix, FileNo = driveLicLog.Fileno, Name = driveLicLog.Name, Status = driveLicLog.Status }; ;
        }
        /// <summary>
        /// 更具相对路径集合删除文件
        /// </summary>
        /// <param name="filePaths"></param>
        private void DeleteFile(string[] filePaths)
        {
            foreach (var item in filePaths)
            {
                try
                {
                    File.Delete(HttpContext.Current.Server.MapPath(item));
                }
                catch (Exception)
                {
                    log.Info("删除文件失败:" + item);
                }
            }
        }

        private bool BuildTnetDriverLicLog(TnetDriveLicLog oldTnetDriveLicLog, DriverResult driResult, AuthDriverLicenseReq req)
        {
            string host = HttpContext.Current.Request.Url.Host;
            host = "http://images2." + host.Substring(host.IndexOf('.') + 1);
            oldTnetDriveLicLog.Name = driResult.姓名.words;
            oldTnetDriveLicLog.Addr = driResult.住址.words;
            oldTnetDriveLicLog.Birthday = driResult.出生日期 == null ? "" : driResult.出生日期.words;
            oldTnetDriveLicLog.Cardimg = req.FrontImgUrl.Replace("/images2", host);
            oldTnetDriveLicLog.CardimgAppendix = req.AppendixImgUrl.Replace("/images2", host);
            oldTnetDriveLicLog.Cardno = driResult.证号 == null ? "" : driResult.证号.words;
            oldTnetDriveLicLog.Country = driResult.国籍 == null ? "" : driResult.国籍.words;
            oldTnetDriveLicLog.Enddate = driResult.至 == null ? "" : driResult.至.words;
            oldTnetDriveLicLog.VehicleType = driResult.准驾车型 == null ? "" : driResult.准驾车型.words;
            oldTnetDriveLicLog.ValidPeriod = driResult.有效期限 == null ? "" : driResult.有效期限.words;
            oldTnetDriveLicLog.Sex = driResult.性别 == null ? "" : driResult.性别.words;
            oldTnetDriveLicLog.Nodeid = req.Nodeid;
            oldTnetDriveLicLog.Firtdate = driResult.初次领证日期 == null ? "" : driResult.初次领证日期.words;
            oldTnetDriveLicLog.Remarks = "";
            oldTnetDriveLicLog.Status = 1;
            oldTnetDriveLicLog.Fileno = req.FileNo;
            return true;
        }

        /// <summary>
        /// 解绑驾驶证
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public bool DeleteDriverLicense(int nodeid)
        {
            string pubulicPaht = "/images2";
            var driLicLog = db.TnetDriveLicLogSet.FirstOrDefault(f => f.Nodeid == nodeid);
            if (driLicLog == null)
            {
                Alert("未绑定用户无法解绑");
                return false;
            }
            Uri.TryCreate(driLicLog.Cardimg, UriKind.Absolute, out Uri uri_license);
            Uri.TryCreate(driLicLog.CardimgAppendix, UriKind.Absolute, out Uri uri_Appendix);
            string path_License = uri_license == null ? driLicLog.Cardimg : pubulicPaht + uri_license.LocalPath;
            string path_Appendix = uri_Appendix == null ? driLicLog.CardimgAppendix : pubulicPaht + uri_Appendix.LocalPath;
            db.TnetDriveLicLogSet.Remove(driLicLog);
            if (db.SaveChanges() < 1)
            {
                Alert("解绑失败");
                return false;
            }
            DeleteFile(new string[] { path_License, path_Appendix });
            Alert("解绑成功", 1);
            return true;
        }
        #endregion
    }
}
