using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ReqSms : Reqbase
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobileno { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string InvitationCode { get; set; }
        /// <summary>
        /// 0为特殊值，按Content内容直接给用户发送短信，
        /// 其他值为枚举值，按后台配置的模板给用户发送相应短信,
        /// 9-注册验证码，1-通用验证码
        /// </summary>
        [Required]
        public int Typeid { get; set; }
        /// <summary>
        /// 业务类型，0-全部，1-手机号已注册发送短信，2-手机号未注册发送短信
        /// </summary>
        public int Btypeid { get; set; }
        /// <summary>
        /// 短信内容（typeid=0时必填，其他时候无效传空值）
        /// </summary>
        public string Content { get; set; }
    }


}
