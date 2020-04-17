using Common.Facade;
using Common.Facade.Models;
using MvcPaging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 注册请求参数
    /// </summary>
    public class ReqReginfo : Reqbase
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobileno { get; set; }

        ///// <summary>
        ///// 登录密码(base64)
        ///// </summary>
        //[Required]
        //public string Pwd { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        [Required]
        public string InvitationCode { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required]
        public string SmsCode { get; set; }

        /// <summary>
        /// 推荐人账号/手机/邮箱,可为空
        /// </summary>
        public string Pnodecode { get; set; }

        /// <summary>
        /// 绑定的第三方账号类型：1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        /// </summary>
        public int Opentype { get; set; }

        /// <summary>
        /// 第三方账号
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 登录类型1.iOS，2.安卓
        /// </summary>
        public int Clientid { get; set; }
        /// <summary>
        /// APP版本号
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// 注册返回参数
    /// </summary>
    public class RegInfoDto
    {
        /// <summary>
        /// 用户NODEID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Nodecode { get; set; }
    }

    /// <summary>
    /// 用户是否注册请求参数
    /// </summary>
    public class ReqIsReg : Reqbase
    {
        /// <summary>
        /// 通讯录手机号码
        /// </summary>
        [Required]
        public string Mobilenos { get; set; }
    }

    /// <summary>
    /// 用户是否注册返回参数
    /// </summary>
    public class IsRegDto
    {
        /// <summary>
        /// nodeid
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// nodecode
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string _appPhoto { get; set; }
        /// <summary>
        /// 是否注册 0未注册 1注册
        /// </summary>
        public int IsReg { get; set; }
        /// <summary>
        /// 融云token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;  //新改图片地址
                }
                return _appPhoto;
            }

        }
    }



    /// <summary>
    /// 用户登录输入参数
    /// </summary>
    public class ReqLogin : Reqbase
    {
        /// <summary>
        /// 账号/手机/邮箱
        /// </summary>
        [Required]
        public string Nodecode { get; set; }

        /// <summary>
        /// 如果是密码登录（BASE64加密），验证码登录直接输入数字
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 第三方账号登录类型：0=UE,1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        /// </summary>
        public int Logintype { get; set; }
        /// <summary>
        /// 登录类型1.iOS，2.安卓
        /// </summary>
        public int Clientid { get; set; }
        /// <summary>
        /// 个推Clientid
        /// </summary>
        public string Gtclientid { get; set; }
        /// <summary>
        /// IOS专用
        /// </summary>
        public string Devicetoken { get; set; }
        /// <summary>
        /// APP版本号
        /// </summary>
        [Required]
        public string Version { get; set; }
    }

    /// <summary>
    /// 登录返回参数
    /// </summary>
    public class LoginInfoDto
    {
        /// <summary>
        /// 用户在平台的唯一ID号(对内)
        /// </summary>
        public int Nodeid { get; set; }

        /// <summary>
        /// 用户在平台的唯一账号(对外)
        /// </summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobileno { get; set; }

        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否认证 0=未认证 1=已认证
        /// </summary>
        public int Isconfrmed { get; set; }

        /// <summary>
        /// 头像URL,没有头像会有一个默认图片
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 最高免密支付金额
        /// </summary>
        public double MaxNotPwd { get; set; }

        /// <summary>
        /// 支付密码是否设置,false=不为空 true=为空,需要设置
        /// </summary>
        public bool IsPayPwdNull { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int NewMailNum { get; set; }
        /// <summary>
        /// 免密支付类型 1=指纹支付 2=...,多个间用逗号隔开,例如:1,2,
        /// </summary>
        public string NotPwdPayType { get; set; }

        /// <summary>
        /// 加我为好友时是否需要验证，1是，0否
        /// </summary>
        public int IsValidfriend { get; set; }

        /// <summary>
        /// 通知设置：系统动态通知，1开，0关
        /// </summary>
        public int IsSysNotice { get; set; }
        /// <summary>
        /// 通知设置：通知显示详情，1开，0关
        /// </summary>
        public int IsNoticeDetail { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Personalsign { get; set; }
        /// <summary>
        /// 显示真实姓名 1-显示，0-不显示
        /// </summary>
        public int Showrealname { get; set; }
        /// <summary>
        ///token值，聊天用，具体参看融云说明 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 达人状态 0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
        /// </summary>
        public int DarenStatus { get; set; }
        /// <summary>
        /// 是否上传驾驶证 0=否 1=是
        /// </summary>
        public int DriverLicenseStatus { get; set; }

    }

    /// <summary>
    /// 获取用户基本信息请求参数
    /// </summary>
    public class ReqUser : Reqbase
    {
        /// <summary>
        /// 账号/手机/邮箱/子账号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }

        /// <summary>
        /// 钱包id,0为特殊值
        /// </summary>
        [Required]
        public int Purseid { get; set; }
    }

    /// <summary>
    /// 获取用户基本信息返回参数
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// 用户NODEID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
    }

    /// <summary>
    /// 签到输入参数
    /// </summary>
    public class ReqSign : Reqbase
    {
        ///// <summary>
        ///// 用户ID
        ///// </summary>
        //[Required]
        //[Range(1, int.MaxValue)]
        //public int Nodeid { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 登录类型1.iOS，2.安卓
        /// </summary>
        [Required]
        public int Clientid { get; set; }
        /// <summary>
        /// APP版本号
        /// </summary>
        [Required]
        public string Version { get; set; }
    }

    /// <summary>
    /// 签到返回参数
    /// </summary>
    public class SignInfoDto
    {
        /// <summary>
        /// 用户在平台的唯一ID号(对内)
        /// </summary>
        public int Nodeid { get; set; }

        /// <summary>
        /// 用户在平台的唯一账号(对外)
        /// </summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobileno { get; set; }

        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否认证 0=未认证 1=已认证
        /// </summary>
        public int Isconfrmed { get; set; }

        /// <summary>
        /// 头像URL,没有头像会有一个默认图片
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 最高免密支付金额
        /// </summary>
        public double MaxNotPwd { get; set; }
        /// <summary>
        /// 支付密码是否设置,false=不为空 true=为空,需要设置
        /// </summary>
        public bool IsPayPwdNull { get; set; }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int NewMailNum { get; set; }
        /// <summary>
        /// 免密支付类型 1=指纹支付 2=...,多个间用逗号隔开,例如:1,2,
        /// </summary>
        public string NotPwdPayType { get; set; }
    }

    /// <summary>
    /// 我页面返回的参数
    /// </summary>
    public class MyDto
    {
        /// <summary>
        /// 用户在平台的唯一ID号(对内)
        /// </summary>
        public int Nodeid { get; set; }

        /// <summary>
        /// 用户在平台的唯一账号(对外)
        /// </summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nodename { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobileno { get; set; }

        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否认证 0=未认证 1=已认证
        /// </summary>
        public int Isconfrmed { get; set; }

        /// <summary>
        /// 头像URL,没有头像会有一个默认图片
        /// </summary>
        public string Pic { get; set; }

        /// <summary>
        /// 最高免密支付金额
        /// </summary>
        public double MaxNotPwd { get; set; }
        /// <summary>
        /// 支付密码是否设置,false=不为空 true=为空,需要设置
        /// </summary>
        public bool IsPayPwdNull { get; set; }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int NewMailNum { get; set; }
        /// <summary>
        /// 免密支付类型 1=指纹支付 2=...,多个间用逗号隔开,例如:1,2,
        /// </summary>
        public string NotPwdPayType { get; set; }
    }


    /// <summary>
    /// 修改用户基本信息输入参数
    /// </summary>
    public class ReqEditUserInfo : Reqbase
    {
        /// <summary>
        /// 输入的用户类型1.姓名, 2.邮箱, 3.头像的url  
        /// 4.支付密码  5.登录密码  6.最高免密支付金额[4,5,6下个版本删除]
        /// </summary>
        [Required]
        public int Type { get; set; }
        /// <summary>
        /// 输入修改的信息(修改密码类都需要base64)
        /// </summary>
        [Required]
        public string Info { get; set; }
        /// <summary>
        /// 短信验证码(姓名,头像等信息验证码为空)
        /// </summary>
        public string SmsCode { get; set; }
    }
    /// <summary>
    /// 修改用户昵称和个性签名Req
    /// </summary>
    public class UserNickAndSignReq : Reqbase
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string NickName { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string PersonalSign { get; set; }
    }

    /// <summary>
    /// 修改用户的登录密码（忘记密码功能）
    /// </summary>
    public class ReqEditLoginPwd : Reqbase
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobileno { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required]
        public string SmsCode { get; set; }
        /// <summary>
        /// 新登录密码(base64)
        /// </summary>
        [Required]
        public string Pwd { get; set; }
    }

    /// <summary>
    /// 修改密码请求参数
    /// </summary>
    public class ChangePwdReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangePwdReq()
        {
            Type = 1;
        }
        /// <summary>
        /// 原密码(base64)
        /// </summary>
        [Required]
        public string OldPwd { get; set; }
        /// <summary>
        /// 新密码(base64)
        /// </summary>
        [Required]
        public string NewPwd { get; set; }

        /// <summary>
        /// 密码类型(1:登录密码,2:支付密码)
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 忘记密码请求参数
    /// </summary>
    public class ForgetPwdReq : Reqbase
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobileno { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required]
        public string SmsCode { get; set; }
        /// <summary>
        /// 新登录密码(base64)
        /// </summary>
        [Required]
        public string NewPwd { get; set; }
        /// <summary>
        /// 密码类型(1:登录密码,2:支付密码)
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 用户修改手机号码
    /// </summary>
    public class ReqEditMobileno : Reqbase
    {
        /// <summary>
        /// 用户的旧号码
        /// </summary>
        [Required]
        public string Oldmobileno { get; set; }
        /// <summary>
        /// 用户的新号码
        /// </summary>
        [Required]
        public string Newmobileno { get; set; }
        /// <summary>
        /// 旧号码的验证码
        /// </summary>
        [Required]
        public string Oldsmscode { get; set; }
        /// <summary>
        /// 新号码的验证码
        /// </summary>
        [Required]
        public string Newsmscode { get; set; }
    }

    /// <summary>
    /// 绑定第三方账号输入的参数
    /// </summary>
    public class ReqCreateUserOpen : Reqbase
    {
        ///// <summary>
        ///// 用户的ID
        ///// </summary>
        //public int Nodeid { get; set; }
        /// <summary>
        /// 绑定的第三方账号类型：1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        /// </summary>
        public int Opentype { get; set; }
        /// <summary>
        /// 第三方的账号,Opentype=4/5时，openid=nodecode
        /// </summary>
        public string Openid { get; set; }
    }

    /// <summary>
    /// 解除第三方绑定输入的参数
    /// </summary>
    public class ReqDeleteUserOpen : Reqbase
    {
        ///// <summary>
        ///// 用户的ID
        ///// </summary>
        //public int Nodeid { get; set; }

        /// <summary>
        /// 绑定的第三方账号类型：1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        /// </summary>
        public int Opentype { get; set; }
    }


    /// <summary>
    /// 用户信息请求
    /// </summary>
    public class LoginReq : Reqbase
    {
        /// <summary>
        /// 个推Clientid
        /// </summary>
        public string Gtclientid { get; set; }
        /// <summary>
        /// IOS专用
        /// </summary>
        public string Devicetoken { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ChatUserDto
    {
        /// <summary>
        /// 
        /// </summary>
        private string _appPhoto;
        /// <summary>
        /// 
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        ///token值，聊天用，具体参看融云说明 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 个推clientid
        /// </summary>
        [JsonIgnore()]
        public string GtClientid { get; set; }
        /// <summary>
        /// ios专用
        /// </summary>
        [JsonIgnore()]
        public string DeviceToken { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int? GradeId { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;
                }
                return _appPhoto;
            }

        }
        ///// <summary>
        ///// 队伍名称
        ///// </summary>
        //public string TeamName { get; set; }
        ///// <summary>
        ///// 性别
        ///// </summary>
        //public string Sex { get; set; }
        ///// <summary>
        ///// 省
        ///// </summary>
        //public int Provinceid { get; set; }
        ///// <summary>
        ///// 省
        ///// </summary>
        //public string Provincename { get; set; }
        ///// <summary>
        ///// 市
        ///// </summary>
        //public int Cityid { get; set; }
        ///// <summary>
        ///// 市
        ///// </summary>
        //public string Cityname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        //public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Personalsign { get; set; }
        /// <summary>
        /// 显示真实姓名 1-显示，0-不显示
        /// </summary>
        public int Showrealname { get; set; }
        private string _remarks = "";
        /// <summary>
        /// 好友备注
        /// </summary>
        public string Remarks
        {
            get
            {
                return _remarks ?? string.Empty;
            }
            set
            {
                _remarks = value;
            }

        }
        ///// <summary>
        ///// 是否允许查看我的动态，1-允许，0-不充许
        ///// </summary>
        //public int Allowviewmedynamic { get; set; }
        ///// <summary>
        ///// 是否查看他的动态，1-看，0-不看
        ///// </summary>
        //public int Viewhedynamic { get; set; }
        ///// <summary>
        ///// 腾讯usersig
        ///// </summary>
        //public string TxUserSig { get; set; }
        /// <summary>
        /// 相关配置属性，CreateGroupMinGrade-创建群组需要的最小等级，GroupMaxQuantity-创建群组最大数量，DiscussionMaxQuantity-创建讨论组最大数量,CreateGroupAmount-创建默认群扣费金额,CreateChatRoomAmount-创建聊天室扣费金额，DefaultPublic-需要默认关注的公众号,多个用逗号隔开，LiveSendBarrageGiftId-直播间发送弹幕对应礼物ID，LiveRoomTip-直播间系统提示文字
        /// </summary>
        //public PxinConfig Config { get; set; }
    }
    /// <summary>
    /// 修改我的信息请求参数
    /// </summary>
    public class UpdateMyInfoReq : Reqbase
    {
        /// <summary>
        /// 类型，1-昵称，2-性别，3-所在地[省市id逗号隔开]，4-个性签名，5-显示真实姓名[0-不显示，1-显示]
        /// </summary>
        [Required]
        public int paramType { get; set; }
        /// <summary>
        /// 修改值
        /// </summary>
        [Required]
        public string paramValue { get; set; }
    }
    /// <summary>
    /// 修改昵称请求参数
    /// </summary>
    public class UpdateMyNickReq : Reqbase
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string nickname { get; set; }
    }

    /// <summary>
    /// 查询好友请求参数
    /// </summary>
    public class QueryFriendReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryFriendReq()
        {
            pageIndex = 1;
            pageSize = 10;
        }

        /// <summary>
        ///查询关键字精确查询 NodeCode NodeName Email Mobileno  
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 查询好友返回参数
    /// </summary>
    public class QueryFriendDtos
    {
        /// <summary>
        /// 参看获取用户信息返回值
        /// </summary>
        public IPagedList<ChatUserDto> Item { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo pageInfo { get; set; }
    }

    /// <summary>
    /// 添加好友请求参数
    /// </summary>
    public class AddFriendReq : Reqbase
    {
        /// <summary>
        /// 好友nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
    }

    /// <summary>
    /// 添加好友确认请求参数
    /// </summary>
    public class AddFriendConfirmReq : Reqbase
    {
        /// <summary>
        /// 好友nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
        /// <summary>
        /// 1-通过，2-拒绝
        /// </summary>
        [Required]
        public int status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
    }

    /// <summary>
    /// 修改好友信息请求参数
    /// </summary>
    public class UpdateMyFriendInfoReq : Reqbase
    {
        /// <summary>
        /// 好友nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
        /// <summary>
        /// 类型，1-好友备注，2-是否允许查看我的动态[1-允许，0-不充许]，3-是否查看他的动态[1-看，0-不看]
        /// </summary>
        [Required]
        public int paramType { get; set; }
        /// <summary>
        /// 修改值
        /// </summary>
        public string paramValue { get; set; }
    }


    /// <summary>
    /// 修改好友备注请求参数
    /// </summary>
    public class UpdateMyFriendRemarksReq : Reqbase
    {
        /// <summary>
        /// 好友nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        public string remarks { get; set; }
    }

    /// <summary>
    /// 删除好友请求参数
    /// </summary>
    public class DeleteFriendReq : Reqbase
    {
        /// <summary>
        /// 好友nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
    }

    /// <summary>
    /// 创建群组请求参数
    /// </summary>
    public class CreateGroupReq : Reqbase
    {
        /// <summary>
        /// 群组名称
        /// </summary>
        [Required]
        public string groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public string descript { get; set; }
        /// <summary>
        /// 群组图片，图片base64字符串
        /// </summary>
        public string grouppic { get; set; }
    }

    /// <summary>
    /// 群组dto
    /// </summary>
    public class ChatGroupDto
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string Groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public string Descript { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 群组状态，0-待提交，1-正常,2-解散
        /// </summary>
        public int Groupstate { get; set; }
        /// <summary>
        /// 群组人数
        /// </summary>
        public int PersonCount { get; set; }
        /// <summary>
        /// 0-普通群，1-收费群，2-系统群,3-广播群
        /// </summary>
        public int GroupType { get; set; }
        /// <summary>
        /// 解散时间
        /// </summary>
        public DateTime? Dismisstime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodecode { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodename { get; set; }
        /// <summary>
        /// 群组头像
        /// </summary>
        public string Grouppic { get; set; }
        /// <summary>
        ///  群组号
        ///</summary>
        public string Groupcode { get; set; }
        /// <summary>
        ///  审核状态，0-等待审核，1-审核通过，2-审核拒绝
        ///</summary>
        public int Auditstate { get; set; }
        /// <summary>
        ///  认证状态，0-未认证，1-已认证
        ///</summary>
        public int Authstate { get; set; }
        /// <summary>
        /// 完整图像URL地址
        /// </summary>
        public string GrouppicFull
        {
            get
            {
                if (string.IsNullOrEmpty(Grouppic)) return string.Empty;
                if (Grouppic.Contains("http://"))
                {
                    return Grouppic;
                }
                else
                {
                    return Common.Facade.Helper.DomainUrl + Grouppic;
                }
            }
        }
    }

    /// <summary>
    /// 修改群组
    /// </summary>
    public class UpdateGroupReq : Reqbase
    {
        /// <summary>
        ///群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public string descript { get; set; }
        /// <summary>
        /// 群组图片，图片base64字符串
        /// </summary>
        public string grouppic { get; set; }
    }

    /// <summary>
    /// 查询群组请求参数
    /// </summary>
    public class QueryGroupReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryGroupReq()
        {
            pageIndex = 1;
            pageSize = 10;
        }

        /// <summary>
        ///查询关键字 模糊查询群组名
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 查询群组返回参数
    /// </summary>
    public class QueryGroupDtos
    {
        /// <summary>
        /// 参看群组dto返回值
        /// </summary>
        public IPagedList<ChatGroupDto> Item { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo pageInfo { get; set; }
    }

    /// <summary>
    /// 加入群组请求参数
    /// </summary>
    public class JoinGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        public int groupid { get; set; }
        /// <summary>
        /// 请求加群备注
        /// </summary>
        public string remarks { get; set; }
    }

    /// <summary>
    /// 加入群组确认请求参数
    /// </summary>
    public class JoinGroupConfirmReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 1-通过，2-拒绝
        /// </summary>
        [Required]
        public int status { get; set; }
        /// <summary>
        /// 请求入群用户nodecode
        /// </summary>
        [Required]
        public string usercode { get; set; }
        /// <summary>
        /// 请求加群备注
        /// </summary>
        public string remarks { get; set; }
    }

    /// <summary>
    /// 群主拉人入群请求参数
    /// </summary>
    public class JoinGroupInvitationReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 请求入群用户nodeid
        /// </summary>
        [Required]
        public string userids { get; set; }
        /// <summary>
        /// 请求加群备注
        /// </summary>
        public int remarks { get; set; }
    }
    /// <summary>
    /// 退出群组请求参数
    /// </summary>
    public class QuitGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
    }
    /// <summary>
    /// 移除群组成员请求参数
    /// </summary>
    public class RemoveGroupUserReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 要移除的群组成员
        /// </summary>
        [Required]
        public string usercode { get; set; }
    }

    /// <summary>
    /// 解散群组请求参数
    /// </summary>
    public class DismissGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
    }

    /// <summary>
    /// 查询群组成员请求参数
    /// </summary>
    public class QueryGroupUserReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryGroupUserReq()
        {
            pageIndex = 1;
            pageSize = 9999;
        }
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        ///  页大小
        /// </summary>
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 群组禁言请求参数
    /// </summary>
    public class GagUserAddGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 禁言用户nodeid,多个用逗号隔开
        /// </summary>
        [Required]
        public string userids { get; set; }
        /// <summary>
        /// 禁言时间，单位分钟，最大值为43200分钟
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int minute { get; set; }
    }

    /// <summary>
    /// 群组禁言移除请求参数
    /// </summary>
    public class GagUserRemoveGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int groupid { get; set; }
        /// <summary>
        /// 禁言用户nodeid,多个用逗号隔开
        /// </summary>
        [Required]
        public string userids { get; set; }
    }

    /// <summary>
    /// 群组禁言查询请求参数
    /// </summary>
    public class GagUserQueryGroupReq : Reqbase
    {
        /// <summary>
        /// 群组id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int groupid { get; set; }
    }
    /// <summary>
    /// 群组禁言查询返回参数
    /// </summary>
    public class GagUserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userId { get; set; }
    }

    /// <summary>
    /// 创建系统聊天室地址 nodecode=10000的用户才能创建成功
    /// </summary>
    public class CreateChatRoomReq : Reqbase
    {
        /// <summary>
        /// 聊天室名称
        /// </summary>
        [Required]
        public string roomname { get; set; }
        /// <summary>
        /// 聊天室描述
        /// </summary>
        public string descript { get; set; }
        /// <summary>
        /// 聊天室密码,可为空
        /// </summary>
        public string roompwd { get; set; }
        /// <summary>
        ///聊天室图片,png格式，base64编码
        /// </summary>
        public string roompic { get; set; }
    }


    /// <summary>
    /// 创建聊天室返回参数
    /// </summary>
    public partial class ChatRoomDto
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatRoomDto()
        {
            Roomtype = 0;
            Roomstate = 0;
            Transferid = 0;
            Personcount = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 0-普通聊天室，1-收费聊天室,2-系统聊天室
        /// </summary>
        public int Roomtype { get; set; }
        /// <summary>
        /// 聊天室名称
        /// </summary>
        public string Roomname { get; set; }
        /// <summary>
        /// 聊天室密码
        /// </summary>
        public string Roompwd { get; set; }
        /// <summary>
        /// 聊天室备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 群组状态，0-待提交，1-正常,2-解散
        /// </summary>
        public int Roomstate { get; set; }
        /// <summary>
        /// 转账Id
        /// </summary>
        public int Transferid { get; set; }
        /// <summary>
        /// 解散时间
        /// </summary>
        public DateTime? Dismisstime { get; set; }
        /// <summary>
        /// 聊天室图片
        /// </summary>
        public string Roompic { get; set; }
        /// <summary>
        /// 聊天室当前人数
        /// </summary>
        public int Personcount { get; set; }

        /// <summary>
        /// 是否有密码,1-有密码，0-没密码
        /// </summary>
        public int Haspwd { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Createnodecode { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Createnodename { get; set; }
    }


    /// <summary>
    /// 创建收费聊天室地址请求参数
    /// </summary>
    public class CreateChatRoom2Req : CreateChatRoomReq
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string paypwd { get; set; }
    }
    /// <summary>
    /// 修改聊天室信息地址请求参数
    /// </summary>
    public class UpdateChatRoomReq : CreateChatRoomReq
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
    }
    /// <summary>
    /// 修改聊天室密码请求参数
    /// </summary>
    public class UpdateChatRoomPwdReq : Reqbase
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
        /// <summary>
        /// 聊天室旧密码
        /// </summary>
        public string oldpwd { get; set; }
        /// <summary>
        /// 聊天室密码,可为空
        /// </summary>
        public string roompwd { get; set; }
    }

    /// <summary>
    /// 查询聊天室请求参数
    /// </summary>
    public class QueryChatRoomReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryChatRoomReq()
        {
            myroom = 0;
            pageIndex = 1;
            pageSize = 10;
        }
        /// <summary>
        /// 查询关键词[可为聊天室id,聊天室名称]
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 0-查询所有聊天室,1-查询我创建的聊天室,2-查询系统聊天室
        /// </summary>
        [Required]
        public int myroom { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 查询聊天室返回参数
    /// </summary>
    public class QueryChatRoomDtos
    {
        /// <summary>
        /// 参看创建聊天室返回参数
        /// </summary>
        public IPagedList<ChatRoomDto> Item { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo pageInfo { get; set; }
    }

    /// <summary>
    /// 加入聊天室请求参数
    /// </summary>
    public class JoinChatRoomReq : Reqbase
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
        /// <summary>
        /// 聊天室密码,可为空
        /// </summary>
        public string roompwd { get; set; }
    }

    /// <summary>
    /// 退出聊天室请求参数
    /// </summary>
    public class QuitChatRoomReq : Reqbase
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
    }



    /// <summary>
    /// 销毁/解散聊天室请求参数
    /// </summary>
    public class DestroyChatRoomReq : Reqbase
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
    }




    /// <summary>
    /// 查询聊天室当前人数请求参数
    /// </summary>
    public class QueryChatRoomUserCountReq : Reqbase
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int roomid { get; set; }
    }

    /// <summary>
    /// 查找公众号请求参数
    /// </summary>
    public class QueryPublicReq : Reqbase
    {
        /// <summary>
        /// 查询关键字 精确查询 公众号id 公众号名
        /// </summary>
        [Required]
        public string key { get; set; }
    }

    /// <summary>
    /// P信公众号
    /// </summary>
    public partial class ChatPublicDto
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatPublicDto()
        {
            PublicType = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公众号Id
        /// </summary>
        public string PublicId { get; set; }
        /// <summary>
        /// 公众号名称
        /// </summary>
        public string PublicName { get; set; }
        /// <summary>
        /// 公众号类型,1-所有人自动关注,2-部分人自动关注，3-用户主动关注
        /// </summary>
        public int PublicType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 公众号Logo地址，完整的url地址
        /// </summary>
        public string PublicLogo { get; set; }
    }
    /// <summary>
    /// 省份
    /// </summary>
    public class ProvinceDto
    {
        /// <summary>
        /// 省id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 添加关注请求参数
    /// </summary>
    public class ConcernAddReq : Reqbase
    {
        /// <summary>
        /// 要关注的用户nodecode
        /// </summary>
        [Required]
        public string concernCode { get; set; }
    }



    /// <summary>
    /// 取消关注请求参数
    /// </summary>
    public class ConcernCancelReq : Reqbase
    {
        /// <summary>
        /// 要取消关注的用户nodecode
        /// </summary>
        [Required]
        public string concernCode { get; set; }
    }



    /// <summary>
    /// 我的粉丝列表请求参数
    /// </summary>
    public class MyFansReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public MyFansReq()
        {
            pageIndex = 1;
            pageSize = 10;
        }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, 9999999999)]
        [Required]
        public int pageSize { get; set; }
    }


    /// <summary>
    /// 我的关注列表请求参数
    /// </summary>
    public class MyConcernsReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public MyConcernsReq()
        {
            pageIndex = 1;
            pageSize = 10;
        }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, 9999999999)]
        [Required]
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 获得附近的人请求
    /// </summary>
    public class GetNearbyReq : Reqbase
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
    }
    /// <summary>
    /// 获得附近的人返回
    /// </summary>
    public class GetNearbyDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string _photo { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo
        {
            set
            {
                _photo = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_photo))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_photo.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _photo;
                }
                return _photo;
            }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 距离，米
        /// </summary>
        public double distance { get; set; }
    }

    /// <summary>
    /// 获得摇一摇返回
    /// </summary>
    public class GetYaoyiyaoDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string _photo { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo
        {
            set
            {
                _photo = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_photo))
                {
                    return AppConfig.DefaultPhoto;
                }
                else
                {
                    if (_photo.Contains("http://"))
                    {
                        return _photo;
                    }
                    else
                    {
                        return Common.Facade.Helper.DomainUrl + _photo;
                    }
                }
            }
        }

        /// <summary>
        /// 距离，米
        /// </summary>
        public double distance { get; set; }
    }

    /// <summary>
    /// 根据userid获取用户信息请求参数
    /// </summary>
    public class QueryUserInfoReq : Reqbase
    {
        /// <summary>
        /// 对应融云的userid,多个用逗号隔开，比如:123,345,324
        /// </summary>
        public string userids { get; set; }
    }
    /// <summary>
    /// 获取通用倍率req
    /// </summary>
    public class CommonRateReq : Reqbase
    {
        /// <summary>
        /// 发送者：通用倍率该值为0
        /// </summary>
        public int Sender { get; set; }
        /// <summary>
        /// 接收者：自己nodeid
        /// </summary>
        public int Receiver { get; set; }
        /// <summary>
        /// 查询类型:通用倍率该值为3
        /// </summary>
        public int Type { get; set; }
    }
    /// <summary>
    /// 获取通用倍率Dto
    /// </summary>
    public class CommonRateDto
    {
        /// <summary>
        /// 通用倍率
        /// </summary>
        public decimal Rate { get; set; }
    }

    #region 暂时不用  不要删除



    /// <summary>
    /// 创建广播号地址请求参数
    /// </summary>
    public class CreateGroup3Req : Reqbase
    {
        /// <summary>
        /// 群组名称
        /// </summary>
        [Required]
        public string groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public string descript { get; set; }
        /// <summary>
        /// 群组头像，图片base64字符串
        /// </summary>
        public string grouppic { get; set; }
        /// <summary>
        /// 群组号
        /// </summary>
        [Required]
        public string groupcode { get; set; }
        /// <summary>
        /// 广播号激活码
        /// </summary>
        [Required]
        public string activecode { get; set; }
    }

    /// <summary>
    ///找广播号请求参数
    /// </summary>
    public class QueryGroup3Req : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryGroup3Req()
        {
            pageIndex = 1;
            pageSize = 10;
        }

        /// <summary>
        ///查询关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int pageSize { get; set; }
    }
    #endregion

    #region 发送消息扣减v点加p点请求参数
    /// <summary>
    /// 发送消息扣减v点加p点请求参数
    /// </summary>
    public class ReqSendmsgOper : Reqbase
    {
        /// <summary>
        /// 消息类型 1=文字信息，图片信息， 语音信息， 表情信息
        /// </summary>
        public int Msgtype { get; set; }
        /// <summary>
        /// Msgtype:1=字数，2=图片大小，语音长度，表情个数
        /// </summary>
        public decimal MsgContent { get; set; }
    }
    #endregion
}

