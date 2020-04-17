using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.UserDto
{
    /// <summary>
    /// 用户绑定的第三方账号返回的参数
    /// </summary>
    public class UserOpensDto
    {
        /// <summary>
        /// 绑定的第三方账号类型：1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        /// </summary>
        public int Opentype { get; set; }
        /// <summary>
        /// UE的账号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 账号绑定的状态：0-未绑定、1-绑定
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 被邀请用户
    /// </summary>
    public class InviteesDto
    {
        /// <summary>
        /// 被邀请人姓名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 被邀请人账号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [JsonIgnore]
        public DateTime UseTime { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string RegTime
        {
            get
            {
                if (string.IsNullOrEmpty(Nodecode))
                {
                    return "未使用";
                }
                else
                {
                    return UseTime.ToString("yyyy-MM-dd hh:mm:ss");
                }
            }
        }
        /// <summary>
        /// 使用的邀请码
        /// </summary>
        public string InvitationCode { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>

        public bool IsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(Nodecode);
            }
        }
    }
    /// <summary>
    /// V点自动充值金额
    /// </summary>
    public class AutoChargeAmountVDto
    {
        /// <summary>
        /// 为0时，表示未设置自动充值
        /// </summary>
        public decimal Amount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DriveLicenseDto
    {
        /// <summary>
        /// 驾驶证正面
        /// </summary>
        public string Pic1 { get; set; }

        /// <summary>
        /// 驾驶证副页
        /// </summary>
        public string Pic2 { get; set; }

        /// <summary>
        /// 驾驶证副业档案编号
        /// </summary>
        public string Fileno { get; set; }

        /// <summary>
        /// 审核状态：0待审核，1通过，2拒绝
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IdCardAuthDto
    {
        /// <summary>
        /// 身份证正面照
        /// </summary>
        public string Pic1 { get; set; }

        /// <summary>
        /// 身份证反面照
        /// </summary>
        public string Pic3 { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCardNum { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string IdCardName { get; set; }

        /// <summary>
        /// 审核状态：0待审核，1通过，2拒绝
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PcnAcountInfoDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PhotoUrl { get; set; }
    }
    /// <summary>
    /// 认证状态输出参数
    /// </summary>
    public class AuthStatusDto
    {
        /// <summary>
        /// 真名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Mobileno { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 状态，0-等待审核，1-通过，2-拒绝，4-资料不完整，5-已通过实人认证，6-认证过期
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 站内信返回的参数
    /// </summary>
    public class MailDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 具体内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 信息状态: 0.未查看 1.已查看
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string ClickUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IsMailDto
    {
        /// <summary>
        /// 是否有未读消息 0=否 1=是
        /// </summary>
        public int Status { get; set; }
    }
}
