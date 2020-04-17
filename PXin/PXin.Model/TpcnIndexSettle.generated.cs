using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// p指数结算表
    /// </summary>
    public partial class TpcnIndexSettle
    {
        public TpcnIndexSettle()
        { 
Settleid = 0;                                         
Infoid = 0;                                         
Periods = 0;                                         
Num = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Settleid { get; set; }
        /// <summary>
        ///  关联TPCN_INDEX_USER.INFOID
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  结算期数
        ///</summary>
        public int Periods { get; set; }
        /// <summary>
        ///  结算金额
        ///</summary>
        public decimal Num { get; set; }
        /// <summary>
        ///  结算时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}