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
    /// 兑换商品请求参数
    /// </summary>
    public class ReqExchange
    {
    }
    /// <summary>
    /// 商品详情
    /// </summary>
    public class ReqProducDetails : Reqbase
    {
        /// <summary>
        /// id 
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ReqExchangeUserInfo : Reqbase
    {
        /// <summary>
        /// 第三方账号（优谷必填，pcn，ue选填）
        /// </summary>
        public string nodeCode { get; set; }

        /// <summary>
        ///账号类型  0=UE、4=Pcn、5=优谷
        /// </summary>
        [Required]
        public int typeId { get; set; }
    }
    /// <summary>
    /// svc相信码
    /// </summary>
    public class ReqSvcRecharge : Reqbase
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int Num { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string NodeCode { get; set; }
    }
    /// <summary>
    /// 优谷vip码-p客认证码
    /// </summary>
    public class ReqYGVipRecharge : ReqSvcRecharge
    {
        /// <summary>
        /// 拥有者id，优谷id 或者pcn id
        /// </summary>
        [Required]
        public int PNodeId { get; set; }
    }
    /// <summary>
    /// 转出
    /// </summary>
    public class ReqUeTransfer : Reqbase
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string PayPwd { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        [Range(0.01, 99999999999)]
        public decimal Amount { get; set; }

    }
    /// <summary>
    /// 转入
    /// </summary>
    public class ReqUeTransferIn : Reqbase
    {
        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        [Range(0.01, 99999999999)]
        public decimal Amount { get; set; }
    }
    /// <summary>
    /// 绑定ue账号
    /// </summary>
    public class ReqBindingUe : Reqbase
    {
        /// <summary>
        /// ue账号
        /// </summary>
        [Required]
        public string UeNodeCode { get; set; }
        /// <summary>
        /// ue密码
        /// </summary>
        [Required]
        public string UeNodePwd { get; set; }
    }
    /// <summary>
    /// 兑换商品
    /// </summary>
    public class ReqProductRecharge : Reqbase
    {
        ///// <summary>
        ///// 兑换类型 1=兑换SV 2=兑换SVC 3=兑换YG的会员码 4=兑换PCN的认证码
        ///// </summary>
        //[Required]
        //public int TypeId { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string PayPwd { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int Num { get; set; }

        /// <summary>
        /// 拥有者nodecode，优谷nodecode 或者pcn nodecode(兑换YG的会员码,兑换PCN的认证码时需要)
        /// </summary>
        public string PnodeCode { get; set; }

    }
}
