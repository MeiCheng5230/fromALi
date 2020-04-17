using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// btc充值日志
    /// </summary>
    public partial class TblcBtcChargeLog
    {
        public TblcBtcChargeLog()
        { 
Logid = 0;                                         
Nodeid = 0;                                         
Amount = 0;                                         
Counts = 0;                                         
Guidstr = string.Empty;                                         
State = 0;                                         
Payment = 0;                                         
Typeid = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Logid { get; set; }
        /// <summary>
        ///  充值用户
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  面额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  充值数量
        ///</summary>
        public int Counts { get; set; }
        /// <summary>
        ///  充值唯一编号
        ///</summary>
        public string Guidstr { get; set; }
        /// <summary>
        ///  用户备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  状态:0-已提交，1-成功，2-失败
        ///</summary>
        public int State { get; set; }
        /// <summary>
        ///  创建日期
        ///</summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        ///  btc通知时间
        ///</summary>
        public DateTime? NoticeTime { get; set; }
        /// <summary>
        ///  支付方式 0=微信 1=网银  2-环讯
        ///</summary>
        public int Payment { get; set; }
        /// <summary>
        ///  充值码类型,0-SV,1-DOS
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  APPSTOREID
        ///</summary>
        public int? Appstoreid { get; set; }
    
        
    }
}