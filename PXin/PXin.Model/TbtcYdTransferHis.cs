using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// TbtcYdTransferHis
    /// </summary>
    public partial class TbtcYdTransferHis
    {
        public TbtcYdTransferHis()
        { 
HisId = 0;                                         
NodeId = 0;                                         
Grade = 0;                                         
FromPurseId = 0;                                         
Amount = 0;                                         
ToPurseId = 0;                                         
Status = 0;                                         
Reason = 0;                                         
BeginTime = DateTime.Now;                                         
EndTime = DateTime.Now.AddDays(1);                                         
Sid = 0;                                         
Currencytype = 43;                                         
Cv = 0;                                         
Ismax = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int HisId { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int NodeId { get; set; }
        /// <summary>
        ///  用户等级
        ///</summary>
        public int Grade { get; set; }
        /// <summary>
        ///  从钱包
        ///</summary>
        public int FromPurseId { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  到钱包
        ///</summary>
        public int ToPurseId { get; set; }
        /// <summary>
        ///  领取状态,0-未领取，1-已领取,-1-不能领取
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  转账原因
        ///</summary>
        public int Reason { get; set; }
        /// <summary>
        ///  转账备注
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  钱包单位
        ///</summary>
        public int? CurrencyId { get; set; }
        /// <summary>
        ///  开始时间
        ///</summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        ///  结束时间
        ///</summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        ///  APP对用的SID
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  页面显示的内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  钱包类型 43=优惠券DP ,46=经验值 EP, 48=CUF ,3=DOS
        ///</summary>
        public int Currencytype { get; set; }
        /// <summary>
        ///  cuf余额
        ///</summary>
        public decimal Cv { get; set; }
        /// <summary>
        ///  图片完整路径
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  用户等级类型
        ///</summary>
        public int? Typeid { get; set; }
        public decimal? Rate { get; set; }
        public int Ismax { get; set; }
        /// <summary>
        ///  说明信息
        ///</summary>
        public string Message { get; set; }
        /// <summary>
        ///  机器码
        ///</summary>
        public string Terminalcode { get; set; }
        /// <summary>
        ///  红包上限
        ///</summary>
        public string Hblimit { get; set; }
    
        
    }
}