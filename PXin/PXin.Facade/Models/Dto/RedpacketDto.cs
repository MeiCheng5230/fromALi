using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PXin.Facade.Models.Enum;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 领取红包页面信息Dto
    /// </summary>
    public class RedPacketInfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int IsOpen { get; set; }
        /// <summary>
        /// 红包Id
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 红包领取状态(0:未领取，1:已领取,-1:不能领取,2:没有权限领取)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 红包领取状态描述
        /// </summary>
        public string StatusDesc
        {
            get
            {
                switch (Status)
                {
                    case 0: return "领取红包";
                    case 1: return "今日红包已领取";
                    case -1: return "不能领取";
                    case 2: return "本月无权限领取";
                    default:
                        return "本月无权限领取";
                }
            }
        }
        /// <summary>
        /// 拥有A点
        /// </summary>
        public int ADain { get; set; }
        /// <summary>
        /// 本月是否完成任务(完成任务才能参与活动)
        /// </summary>
        public bool IsCompleteTask { get; set; }
        /// <summary>
        /// 上月是否完成任务(完成任务才能参与活动)
        /// </summary>
        public bool IsCompleteTask1 { get; set; }
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 专户DOS
        /// </summary>
        public decimal DOS { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户nodecode
        /// </summary>
        public string NodeCode { get; set; }
    }

    /// <summary>
    /// 领取红包Dto
    /// </summary>
    public class ReceiveRedPacketDto
    {
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 专户DOS
        /// </summary>
        public decimal DOS { get; set; }
    }
    /// <summary>
    /// 我的红包奖励Dto
    /// </summary>
    public class MyRedPacketDto
    {
        /// <summary>
        /// 可兑换SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 红包结算列表
        /// </summary>
        public List<ReceiveAmount> SettleAmounts { get; set; } = new List<ReceiveAmount>();
        /// <summary>
        /// 红包领取奖励列表
        /// </summary>
        public List<ReceiveAmount> ReceiveAmounts { get; set; } = new List<ReceiveAmount>();

    }
    /// <summary>
    /// 红包奖励领取情况
    /// </summary>
    public class ReceiveAmount
    {
        /// <summary>
        /// 状态(1:已结算,2:已失效,3:已领取,4:未领取)
        /// </summary>
        public RedpacketReceiveStatus Status { get; set; }
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 专户DOS
        /// </summary>
        public decimal DOS { get; set; }
        /// <summary>
        /// 历史id
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 红包时间
        /// </summary>
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 我的红包奖励领取详情Dto
    /// </summary>
    public class MyRedPacketDetailDto
    {
        /// <summary>
        /// A点个数
        /// </summary>
        public int ADian { get; set; }
        /// <summary>
        /// 已领取红包奖励情况
        /// </summary>
        public ReceiveAmount ReceiveAmount { get; set; }
        /// <summary>
        /// 红包奖励列表
        /// </summary>
        public List<ReceiveAmountDetail> ReceiveAmountDetails { get; set; } = new List<ReceiveAmountDetail>();
    }
    /// <summary>
    /// 红包奖励领取情况
    /// </summary>
    public class ReceiveAmountDetail
    {
        /// <summary>
        /// tpxin_pai_user infoid
        /// </summary>
        [JsonIgnore]
        public int InfoId { get; set; }
        /// <summary>
        /// A点个数
        /// </summary>
        public int ADian { get; set; }
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 专户DOS
        /// </summary>
        public decimal DOS { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusDesc { get; set; }
    }

    /// <summary>
    /// 获取兑换页面信息Dto
    /// </summary>
    public class ExchangeInfoDto
    {
        /// <summary>
        /// 兑换类型
        /// </summary>
        public List<string> Type { get => new List<string> { "SVC充值码", "SV" }; }
        /// <summary>
        /// 拥有SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 手续费比例
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public IEnumerable<Specs> Specs { get; set; }
    }
    /// <summary>
    /// 规格
    /// </summary>
    public class Specs
    {
        /// <summary>
        /// 兑换时要传的id
        /// </summary>
        public int InfoId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ShowName { get; set; }
    }

    /// <summary>
    /// A点竟拍抽奖信息
    /// </summary>
    public class LuckDrawInfo
    {
        /// <summary>
        /// 是否有资格参与抽奖 -2-已过期,-1-抽奖次数已用完,0-无资格 1-有资格
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 可抽奖次数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// A点余额
        /// </summary>
        public decimal A { get; set; }
        /// <summary>
        /// 本次抽奖金额
        /// </summary>
        public int Amount { get; set; }
    }
    /// <summary>
    /// A点竟拍抽奖历史
    /// </summary>
    public class LuckDrawHis
    {
        /// <summary>
        /// 抽奖金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public decimal BalanceAfter { get; set; }
        /// <summary>
        /// 抽奖时间
        /// </summary>
        public DateTime Createtime { get; set; }
    }
}
