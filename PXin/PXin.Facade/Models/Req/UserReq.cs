using Common.Facade.Models;
using System.ComponentModel.DataAnnotations;

namespace PXin.Facade.Models.UserReq
{
    /// <summary>
    /// 设置自动充值V点
    /// </summary>
    public class SetAutoChargeVPointReq : Reqbase
    {
        /// <summary>
        /// 支付密码(Base64加密)
        /// </summary>
        [Required]
        public string PayPwd { get; set; }
        /// <summary>
        /// 自动充值金额
        /// </summary>
        [Range(0, 99999999)]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 查询是否同意协议请求
    /// </summary>
    public class AgreementReq : Reqbase
    {
        /// <summary>
        /// 协议类型 20001-注册协议，2-会员协议，20003-竞拍协议，20004-充值协议
        /// </summary>
        [Required]
        public int Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IsValidDto : Reqbase
    {
        /// <summary>
        /// 加我为好友时是否需要验证，1是，0否
        /// </summary>
        public int IsValid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IsOpenDto : Reqbase
    {
        /// <summary>
        /// 1打开，0关闭
        /// </summary>
        public int IsOpen { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FeedbackReq : Reqbase
    {
        /// <summary>
        /// 反馈的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 反馈的内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 图片url,多个图片用,分隔
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///  联系方式
        ///</summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 当前版本号
        /// </summary>
        [Required]
        public string Version { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IdAuthReq : Reqbase
    {
        /// <summary>
        /// 身份证正面照
        /// </summary>
        [Required]
        public string Pic1 { get; set; }

        /// <summary>
        /// 身份证反面照
        /// </summary>
        [Required]
        public string Pic3 { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCardNum { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string IdCardName { get; set; }
    }

    /// <summary>
    /// 驾驶证
    /// </summary>
    public class DriveLicenseReq : Reqbase
    {
        /// <summary>
        /// 驾驶证正面
        /// </summary>
        [Required]
        public string Pic1 { get; set; }

        /// <summary>
        /// 驾驶证副页
        /// </summary>
        [Required]
        public string Pic2 { get; set; }

        /// <summary>
        /// 驾驶证副业档案编号
        /// </summary>
        [Required]
        public string Fileno { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BindPcnAcountReq : Reqbase
    {
        /// <summary>
        /// PCN账号
        /// </summary>
        [Required]
        public string NodeCode { get; set; }
        /// <summary>
        /// PCN密码
        /// </summary>
        [Required]
        public string Pwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PcnAuthStatusByCodeReq : Reqbase
    {
        /// <summary>
        /// PCN账号
        /// </summary>
        [Required]
        public string NodeCode { get; set; }
    }

    /// <summary>
    /// 站内信输入的参数
    /// </summary>
    public class ReqMail : Reqbase
    {
        /// <summary>
        /// 类型 0=全部 1=系统
        /// </summary>
        [Required]
        public int Typeid { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int PageNum { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 删除消息
    /// </summary>
    public class ReqDeleteMail : Reqbase
    {
        /// <summary>
        /// 消息id
        /// </summary>
        [Required]
        public int HisId { get; set; }
    }

    /// <summary>
    /// 读消息
    /// </summary>
    public class ReqReadMail : Reqbase
    {
        /// <summary>
        /// 消息id -1表示全部
        /// </summary>
        [Required]
        public string HisId { get; set; }
    }
    
}
