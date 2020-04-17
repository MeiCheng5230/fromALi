using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 第三方获取Pcn账号校验
    /// </summary>
    public class ThirdPartyVerifyReq : Reqbase
    {
        /// <summary>
        /// 分配给第三方Key
        /// </summary>
        [Required]
        [MinLength(1)]
        public string SecretKey { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThridPayReq : Reqbase
    {
        /// <summary>
        /// 分配给第三方Key
        /// </summary>
        [Required]
        [MinLength(1)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 支付类型:3000-SV支付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 消费金额(元),精确到小数点后2位
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Orderno { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Subject { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Body { get; set; }

        /// <summary>
        /// 通知Url
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Noticeurl { get; set; }

        /// <summary>
        /// 支付密码(base64)
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Pwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetThridPayhisReq : Reqbase
    {
        /// <summary>
        /// 分配给第三方Key
        /// </summary>
        [Required]
        [MinLength(1)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Orderno { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Timestamp { get; set; }

        /// <summary>
        /// 签名加密
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Signature { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetThridPayTypeReq : Reqbase
    {
        /// <summary>
        /// 分配给第三方Key
        /// </summary>
        [Required]
        [MinLength(1)]
        public string SecretKey { get; set; }
    }
}
