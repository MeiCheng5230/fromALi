using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.UserPurseReq
{

    /// <summary>
    /// 查询钱包
    /// </summary>
    public class PurseReq : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public PurseReq()
        {
            Pursetype = 0;
        }
        /// <summary>
        /// 钱包类型 tnet_purse_config.infoid
        /// </summary>
        public int Pursetype { get; set; }
    }

    /// <summary>
    /// 获取制定钱包账单记录输入参数
    /// </summary>
    public class PurseHisReq : Reqbase
    {
        /// <summary>
        /// 钱包ID
        /// </summary>
        public int Purseid { get; set; }

        /// <summary>
        /// 当前页从1开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页最大记录数
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 扣费
    /// </summary>
    public class PurseRecoveryReq : Reqbase
    {
        /// <summary>
        /// 钱包类型 tnet_purse_config.infoid
        /// </summary>
        public int Pursetype { get; set; }
        /// <summary>
        /// 扣费账号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 金额单位
        /// </summary>
        public int currencytype { get; set; }
        /// <summary>
        /// 转账原因
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// 转账备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string Paypwd { get; set; }
    }

    /// <summary>
    /// 第三方支付
    /// </summary>
    public class ThridPartyPayReq
    {
        /// <summary>
        /// Nodecode
        /// </summary>
        public string Nodecode { get; set; }

        /// <summary>
        /// 金额（元）
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Subject { get; set; }

        /// <summary>
        /// 时间(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        public string ReqTime { get; set; }

        /// <summary>
        /// 加密
        /// </summary>
        public string Sign { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RechargeReq : Reqbase
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 充值渠道,微信-wx
        /// </summary>
        [Required]
        public string Channel { get; set; }

        /// <summary>
        /// 充值类型 4=UV充值
        /// </summary>
        public int Paytype { get; set; }

    }

    /// <summary>
    /// 获取UV充值记录请求参数
    /// </summary>
    public class UVRechargeHisReq : Reqbase
    {
        /// <summary>
        /// 当前页码数
        /// </summary>
        [Range(1, 9999)]
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        [Range(1, 9999)]
        public int PageCount { get; set; }
    }
}
