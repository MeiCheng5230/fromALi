using Common.Facade.Models;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 检查验证码请求
    /// </summary>
    public class ReqVerificationCode : Reqbase
    {
        /// <summary>
        /// 接收验证码手机号码
        /// </summary>
        public string Mobileno { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }
}
