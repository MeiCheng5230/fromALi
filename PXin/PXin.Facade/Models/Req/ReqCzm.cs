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
    public class BuyReq : Reqbase
    {
        /// <summary>
        /// 支付类型 1=微信
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 是否立即使用 0=否 1=是
        /// </summary>
        public int IsUseNow { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Pwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SaleReq : Reqbase
    {
        /// <summary>
        /// 规格id+数量,每种规格逗号分割  例如选择1000 1张，5000 1张，传入("1|1,2|1")
        /// </summary>
        public string Specifications { get; set; }
        /// <summary>
        /// 目标用户的账号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Paypwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RecoveryReq : Reqbase
    {
        /// <summary>
        /// 目标用户的账号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 充值码卡号(1,2,3) 多个以逗号分割
        /// </summary>
        public string Cards { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Paypwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UseByOwnerReq : Reqbase
    {
        /// <summary>
        /// 充值码卡号(1,2,3) 多个以逗号分割
        /// </summary>
        public string Cards { get; set; }
        /// <summary>
        /// 目标用户的账号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Paypwd { get; set; }
        /// <summary>
        /// 是否立即充值v点 0=否 1=是
        /// </summary>
        public int IsVDNow { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class UseByCardReq : Reqbase
    {
        /// <summary>
        /// 充值码卡号(1,2,3) 多个以逗号分割
        /// </summary>
        public string Cards { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SvToSvcCardReq : Reqbase
    {
        /// <summary>
        /// 生成数量规格 例："1000|1,5000|1,10000|2"
        /// </summary>
        public string Config { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Paypwd { get; set; }
    }



    /// <summary>
    /// 
    /// </summary>
    public class SVUserInfoReq : Reqbase
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 是否允许使用邮箱手机号查找 0=不允许 1=允许
        /// </summary>
        public int IsPhoneEmail { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HisReq : Reqbase
    {
        /// <summary>
        /// 类型id
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SuccessReq : Reqbase
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Orderno { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SvcByGroupbyAmountReq : Reqbase
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }
    }
    /// <summary>
    /// 冻结SVCreq
    /// </summary>
    public class FrozenSvcReq : Reqbase
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        [Required]
        public string Batch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public List<FrozenSvcInfo> FrozenSvcInfos { get; set; }
    }
    /// <summary>
    /// 购买svc充值码Req
    /// </summary>
    public class BuySvcCodeReq : Reqbase
    {
        /// <summary>
        ///  tpm2_info主键
        ///</summary>
        [Required]
        public int PmInfoid { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }
        /// <summary>
        /// 面额
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// 卖家Nodecode
        /// </summary>
        [Required]
        public string SellNodecode { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        [Required]
        public string Batch { get; set; }
    }
    /// <summary>
    /// 取消发布
    /// </summary>
    public class CancelReleaseReq : Reqbase
    {
        /// <summary>
        ///  tpm2_info主键
        ///</summary>
        [Required]
        public int PmInfoid { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        public string Nodecode { get; set; }
        /// <summary>
        /// 面额
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class FrozenSvcInfo
    {
        /// <summary>
        /// 面额
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Number { get; set; }
        /// <summary>
        /// 码库拍卖表主键列表
        /// </summary>
        [Required]
        public List<int> PmInfoids { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SVCConfig
    {
        /// <summary>
        /// 金额
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }
}
