using Common.Facade.Models;
using PXin.Facade.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 领取红包页面信息Req
    /// </summary>
    public class RedPacketInfoReq : Reqbase
    {

    }
    /// <summary>
    /// 领取红包Req
    /// </summary>
    public class ReceiveRedPacketReq : Reqbase
    {
        /// <summary>
        /// 红包Id
        /// </summary>
        [Required]
        public int InfoId { get; set; }
    }
    /// <summary>
    /// 我的红包奖励Req
    /// </summary>
    public class MyRedPacketReq : Reqbase
    {
    }
    /// <summary>
    /// 我的红包奖励领取详情Req
    /// </summary>
    public class MyRedPacketDetailReq : Reqbase
    {
        /// <summary>
        /// 历史id
        /// </summary>
        public int HisId { get; set; }
    }
    /// <summary>
    /// 获取兑换页面信息Req
    /// </summary>
    public class ExchangeInfoReq : Reqbase
    {

    }
    /// <summary>
    /// 兑换Req
    /// </summary>
    public class ExchangeReq : Reqbase
    {
        /// <summary>
        /// 兑换类型(1:SVC充值码,2:SV余额)
        /// </summary>
        [Required]
        public ExchangeType ExchangeType { get; set; }
        /// <summary>
        /// 所兑换各种规格及张数
        /// </summary>
        public List<ExchangeSpecs> Specs { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 当ExchangeType=2时,要兑换的数量
        /// </summary>
        public decimal Num { get; set; }
    }
    /// <summary>
    /// 规格
    /// </summary>
    public class ExchangeSpecs
    {
        /// <summary>
        /// id
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 张数
        /// </summary>
        public int Num { get; set; }
    }
}
