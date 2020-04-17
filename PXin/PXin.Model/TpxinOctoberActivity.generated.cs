using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 相信十月活动
    /// </summary>
    public partial class TpxinOctoberActivity
    {
        public TpxinOctoberActivity()
        { 
Id = 0;                                         
Typeid = 0;                                         
Nodeid = 0;                                         
Note = null;                                         
Amount = 0;                                         
Pnodeid = 0;                                         
Pnote = null;                                         
Pamount = 0;                                         
Status = 0;
ActivityId = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  活动类型，1-代开充值商、2-代理人进货、3-零售SVC充值码并充值SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  领取手机用户
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  领取手机用户看到的活动描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  领取手机用户支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  领取手机用户转账ID，tnet_uepayhis.id
        ///</summary>
        public string Transferids { get; set; }
        /// <summary>
        ///  领取手机用户支付时间
        ///</summary>
        public DateTime? Transfertime { get; set; }
        /// <summary>
        ///  上级用户
        ///</summary>
        public int Pnodeid { get; set; }
        /// <summary>
        ///  上级用户看到的活动描述
        ///</summary>
        public string Pnote { get; set; }
        /// <summary>
        ///  上级用户支付金额
        ///</summary>
        public decimal Pamount { get; set; }
        /// <summary>
        ///  上级用户转账ID，tnet_uepayhis.id
        ///</summary>
        public string Ptransferids { get; set; }
        /// <summary>
        ///  上级用户支付时间
        ///</summary>
        public DateTime? Ptransfertime { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  0-未支付，1-部分支付，2-已支付，3-已发货，4-已退款
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  物流单号
        ///</summary>
        public string Expressno { get; set; }
        /// <summary>
        ///  发货时间
        ///</summary>
        public DateTime? Sendtime { get; set; }
        /// <summary>
        ///  活动表id
        ///</summary>
        public int ActivityId { get; set; }

    }
}