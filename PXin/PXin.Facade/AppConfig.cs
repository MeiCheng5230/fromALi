using Common.Facade;
using PXin.Facade.CommonService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfig
    {
        private static readonly string _wsxServiceAPIHost = ConfigurationManager.AppSettings["Wsx.ServiceAPIHost"];
        /// <summary>
        /// 是否开启定时推送服务
        /// </summary>
        public static bool IsOpenTimedPush { get { return "true".Equals(ConfigurationManager.AppSettings["IsOpenTimedPush"], StringComparison.OrdinalIgnoreCase); } }
        /// <summary>
        /// 定时推送功能，推送时间间隔，单位：分钟
        /// </summary>
        public static int TimedPushTimeInterval { get { return Convert.ToInt32(ConfigurationManager.AppSettings["TimedPushTimeInterval"]); } }
        /// <summary>
        /// 分页 页大小
        /// </summary>
        public static int PageSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]); } }
        /// <summary>
        /// 融云Key
        /// </summary>
        public static string AppKey { get { return ConfigurationManager.AppSettings["AppKey"]; } }
        /// <summary>
        /// 融云Secret
        /// </summary>
        public static string AppSecret { get { return ConfigurationManager.AppSettings["AppSecret"]; } }
        /// <summary>
        /// App调用key
        /// </summary>
        public static string ApiKey { get { return ConfigurationManager.AppSettings["ApiKey"]; } }
        /// <summary>
        /// 创建收费群的最小等级
        /// </summary>
        public static int CreateGroupMinGrade { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CreateGroupMinGrade"]); } }
        /// <summary>
        /// 创建广播群的最小等级
        /// </summary>
        public static int CreateGroup3MinGrade { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CreateGroup3MinGrade"]); } }
        /// <summary>
        /// 群组人数上限
        /// </summary>
        public static int GroupCapacity { get { return Convert.ToInt32(ConfigurationManager.AppSettings["GroupCapacity"]); } }
        /// <summary>
        /// 创建群组最大数量
        /// </summary>
        public static int GroupMaxQuantity { get { return Convert.ToInt32(ConfigurationManager.AppSettings["GroupMaxQuantity"]); } }
        /// <summary>
        /// 创建讨论组最大数量
        /// </summary>
        public static int DiscussionMaxQuantity { get { return Convert.ToInt32(ConfigurationManager.AppSettings["DiscussionMaxQuantity"]); } }
        /// <summary>
        /// 个推参数
        /// </summary>
        public static string GtHost { get { return ConfigurationManager.AppSettings["GtHost"]; } }
        /// <summary>
        /// 
        /// </summary>
        public static string GtAppId { get { return ConfigurationManager.AppSettings["GtAppId"]; } }
        /// <summary>
        /// 
        /// </summary>
        public static string GtAppKey { get { return ConfigurationManager.AppSettings["GtAppKey"]; } }
        /// <summary>
        /// 
        /// </summary>
        public static string GtMasterSecret { get { return ConfigurationManager.AppSettings["GtMasterSecret"]; } }
        /// <summary>
        /// 创建默认群扣费金额,单位元
        /// </summary> 
        public static decimal CreateGroupAmount { get { return Convert.ToDecimal(ConfigurationManager.AppSettings["CreateGroupAmount"]); } }
        /// <summary>
        /// 创建默认群转账原因
        /// </summary>
        public static int CreateGroupReason { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CreateGroupReason"]); } }
        /// <summary>
        /// 创建聊天室扣费金额,单位元
        /// </summary> 
        public static decimal CreateChatRoomAmount { get { return Convert.ToDecimal(ConfigurationManager.AppSettings["CreateChatRoomAmount"]); } }
        /// <summary>
        /// 创建聊天室转账原因
        /// </summary>
        public static int CreateChatRoomReason { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CreateChatRoomReason"]); } }
        /// <summary>
        /// 系统账号
        /// </summary>
        public static int SysUserId { get { return Convert.ToInt32(ConfigurationManager.AppSettings["SysUserId"]); } }
        /// <summary>
        /// Appid
        /// </summary>
        public static uint Tx_sdkappid { get { return Convert.ToUInt32(ConfigurationManager.AppSettings["Tx_sdkappid"]); } }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public static string Tx_identifier { get { return ConfigurationManager.AppSettings["Tx_identifier"]; } }
        /// <summary>
        /// 私钥路径
        /// </summary>
        public static string Tx_pri_key_path { get { return ConfigurationManager.AppSettings["Tx_pri_key_path"]; } }
        /// <summary>
        /// 直播主播结算比例
        /// </summary>
        public static decimal LiveMasterSettleRate { get { return Convert.ToDecimal(ConfigurationManager.AppSettings["LiveMasterSettleRate"]); } }
        /// <summary>
        /// 直播间发送弹幕对应礼物ID
        /// </summary>
        public static decimal LiveSendBarrageGiftId { get { return Convert.ToDecimal(ConfigurationManager.AppSettings["LiveSendBarrageGiftId"]); } }
        /// <summary>
        /// 直播间系统提示文字
        /// </summary>
        public static string LiveRoomTip { get { return ConfigurationManager.AppSettings["LiveRoomTip"]; } }
        /// <summary>
        /// 非正常退出直播定时解冻结算时间间隔
        /// </summary>
        public static int LiveTimePervious { get { return Convert.ToInt32(ConfigurationManager.AppSettings["LiveTimePervious"]); } }
        /// <summary>
        /// 
        /// </summary>
        public static string Api_SignString { get { return ConfigurationManager.AppSettings["Api_SignString"]; } }
        /// <summary>
        /// 是否关闭签名验证
        /// </summary>
        public static bool SignValidationDisabled
        {
            get
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["SignValidationDisabled"], out bool _SignValidationDisabled))
                {
                    _SignValidationDisabled = false;
                }
                return _SignValidationDisabled;
            }
        }

        /// <summary>
        /// 是否关闭Swagger功能
        /// </summary>
        public static bool SwaggerDisabled
        {
            get
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["SwaggerDisabled"], out bool _SwaggerDisabled))
                {
                    _SwaggerDisabled = true;
                }
                return _SwaggerDisabled;
            }
        }


        /// <summary>
        /// 页面调起App传送参数加密字符串
        /// </summary>
        public static string AppSecurityString { get { return ConfigurationManager.AppSettings["AppSecurityString"]; } }

        /// <summary>
        /// 用户头像的新图片地址
        /// </summary>
        public static string Userphoto
        {
            get { return ConfigurationManager.AppSettings["Userphoto"]; }
        }

        /// <summary>
        /// 是否使用手机验证码（测试用）
        /// </summary>
        public static bool IsUseSms
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsUseSms"]) || "true".Equals(ConfigurationManager.AppSettings["IsUseSms"], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false;
            }
        }

        #region 短信账号配置
        /// <summary>
        /// SmsCode
        /// </summary>
        public static string SmsCode
        {
            get { return ConfigurationManager.AppSettings["SmsCode"]; }
        }
        /// <summary>
        /// SmsPwd
        /// </summary>
        public static string SmsPwd
        {
            get { return ConfigurationManager.AppSettings["SmsPwd"]; }
        }
        /// <summary>
        /// SmsServiceUrl
        /// </summary>
        public static string SmsServiceUrl
        {
            get { return ConfigurationManager.AppSettings["SmsServiceUrl"]; }
        }
        #endregion
        /// <summary>
        /// 充值商页面跳转地址
        /// </summary>
        public static string FbapUrl
        {
            get { return ConfigurationManager.AppSettings["FbapUrl"]; }
        }
        /// <summary>
        /// 充值商/代理人续费开始时间
        /// </summary>
        public static DateTime BtsContinueStart
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.Url.ToString().IndexOf("xiang-xin.net", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return new DateTime(2019, 2, 20);
                }
                else
                {
                    return new DateTime(2019, 1, 20);
                }
            }
        }
        /// <summary>
        /// pcn获取用户接口地址
        /// </summary>
        public static string PCNUserInfoUrl
        {
            get { return ConfigurationManager.AppSettings["PCNUserInfoUrl"]; }
        }
        /// <summary>
        /// yg获取用户接口地址
        /// </summary>
        public static string YGUserInfoUrl
        {
            get { return YGDomain + "/api/User/GetUserSimpleInfo"; }
        }
        /// <summary>
        /// 优谷域名
        /// </summary>
        public static string YGDomain
        {
            get { return ConfigurationManager.AppSettings["YGDomain"]; }
        }
        /// <summary>
        /// 获取Wsx.ServiceAPIHost配置
        /// </summary>
        public static string WsxServiceAPIHost
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_wsxServiceAPIHost))
                {
                    throw new Exception("配置 Wsx.ServiceAPIHost 不存在");
                }

                return _wsxServiceAPIHost;
            }
        }
        /// <summary>
        /// 默认头像
        /// </summary>
        public static string DefaultPhoto
        {
            get { return "http://images2.xiang-xin.net/userphoto/noempty.png"; }
        }
        /// <summary>
        /// 是否开放注册
        /// </summary>
        public static bool IsOpenRegister
        {
            get
            {
                return ConfigurationManager.AppSettings["IsOpenRegister"] == "1";
            }
        }

        /// <summary>
        /// IOS App下载地址
        /// </summary>
        public static string AppDownloadProperty_IOS { get { return ConfigurationManager.AppSettings["AppDownloadProperty_IOS"]; } }
        /// <summary>
        /// Android App下载地址
        /// </summary>
        public static string AppDownloadProperty_Android { get { return ConfigurationManager.AppSettings["AppDownloadProperty_Android"]; } }

        /// <summary>
        /// 充值商开通代理人所需金额
        /// </summary>
        public static string OpenDlrAmount { get { return ConfigurationManager.AppSettings["OpenDlrAmount"]; } }
        /// <summary>
        /// 充值商开通代理人所需金额
        /// </summary>
        public static string FbapStockinActivity { get { return ConfigurationManager.AppSettings["FbapStockinActivity"]; } }
        /// <summary>
        /// 内测截至时间
        /// </summary>
        public static string BetaEndTime { get { return ConfigurationManager.AppSettings["BetaEndTime"]; } }
        /// <summary>
        /// PM域名
        /// </summary>
        public static string PMDomain { get { return ConfigurationManager.AppSettings["PMDomain"]; } }


        private static readonly string _photoUrlprefix = ConfigurationManager.AppSettings["ImageBaseUrl"];

        /// <summary>
        /// 图片网址
        /// </summary>
        public static string ImageBaseUrl
        {
            get
            {
                if (_photoUrlprefix.IsNullOrWhiteSpace())
                {
                    //throw new Exception("配置 PhotoBaseUrl 不存在");
                    return Helper.DomainUrl + FileRootDir;
                }

                return _photoUrlprefix;
            }
        }

        /// <summary>
        /// 文件根目录
        /// </summary>
        public static string FileRootDir
        {
            get { return "/images2"; }
        }

        /// <summary>
        /// 百度驾驶证行驶证接口参数 API Key
        /// </summary>
        public static string BaiduAPIKey1 { get { return ConfigurationManager.AppSettings["BaiduAPIKey1"]; } }
        /// <summary>
        /// 百度驾驶证行驶证接口参数 Secret Key
        /// </summary>
        public static string BaiduSecretKey1 { get { return ConfigurationManager.AppSettings["BaiduSecretKey1"]; } }
        /// <summary>
        /// 注册送V点数
        /// </summary>
        public static string RegisterGiveVDian { get { return ConfigurationManager.AppSettings["RegisterGiveVDian"]; } }

        /// <summary>
        /// 获取pcn域名
        /// </summary>
        public static string PCNDomainUrl { get { return ConfigurationManager.AppSettings["PCNDomainUrl"]; } }

        /// <summary>
        /// 快递100实时查询key
        /// </summary>
        public static string KuaiDiAPPKey { get { return ConfigurationManager.AppSettings["KuaiDiAPPKey"]; } }
        /// <summary>
        /// 快递100实时查询Customer
        /// </summary>
        public static string KuaiDiAPPCode { get { return ConfigurationManager.AppSettings["KuaiDiAPPCode"]; } }
        /// <summary>
        /// 快递100实时查询Customer
        /// </summary>
        public static string KuaiDiAPPSecret { get { return ConfigurationManager.AppSettings["KuaiDiAPPSecret"]; } }
        /// <summary>
        /// 相信内部服务接口
        /// </summary>
        public static string PxinInternalServiceUrl { get { return ConfigurationManager.AppSettings["PxinInternalServiceUrl"]; } }
        /// <summary>
        /// sv兑换充值码限量时间配置
        /// </summary>
        public static string _cZMCreateTime { get { return ConfigurationManager.AppSettings["CZMCreateTime"]; } }
        /// <summary>
        /// sv兑换充值码限量时间配置 
        /// </summary>
        public static string CZMCreateTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cZMCreateTime))
                {
                    return "2019-11-01";
                }

                return _cZMCreateTime;
            }
        }
        /// <summary>
        /// 邮件发送服务器地址
        /// </summary>
        public static string SendEmailNodeCode { get { return ConfigurationManager.AppSettings["SendEmailNodeCode"]; } }
        /// <summary>
        /// 邮件发送配置密码
        /// </summary>
        public static string SendEmailPwd { get { return ConfigurationManager.AppSettings["SendEmailPwd"]; } }
        /// <summary>
        /// 邮件发送配置类型 qq=qq邮件  163=163邮件
        /// </summary>
        public static string SendEmailType { get { return ConfigurationManager.AppSettings["SendEmailType"]; } }


    }
}
