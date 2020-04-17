using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 充值商代理人续费历史表
    /// </summary>
    public partial class TblUserJxsXf
    {
        public TblUserJxsXf()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Infoid = 0;                                         
Amount = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  付款人
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  TBL_USER_JXS.INFOID
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  支付DOS金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}