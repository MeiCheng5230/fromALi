using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Enum
{
    /// <summary>
    /// 红包领取状态
    /// </summary>
    public enum RedpacketReceiveStatus
    {
        /// <summary>
        /// 已结算
        /// </summary>
        Settled = 1,
        /// <summary>
        /// 已失效
        /// </summary>
        Invalid = 2,
        /// <summary>
        /// 已领取
        /// </summary>
        Received = 3,
        /// <summary>
        /// 未领取
        /// </summary>
        UnReceived = 4
    }
    /// <summary>
    /// 兑换类型
    /// </summary>
    public enum ExchangeType
    {
        /// <summary>
        /// SVC充值码
        /// </summary>
        SVC=1,
        /// <summary>
        /// SV余额
        /// </summary>
        SV=2
    }
}
