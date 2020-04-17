using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// P指数基本信息表
    /// </summary>
    public partial class TpcnIndexInfo
    {
        public TpcnIndexInfo()
        { 
Infoid = 0;                                         
Periods = 0;                                         
Islocal = 0;                                         
Amount = 0;                                         
Num = 0;                                         
Localnum = 0;                                         
Modifytime = new DateTime();                                         
Nexttime = new DateTime();                                         
Typeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  期数
        ///</summary>
        public int Periods { get; set; }
        /// <summary>
        ///  是否为当前期 0=不是 1=是 1仅1个
        ///</summary>
        public int Islocal { get; set; }
        /// <summary>
        ///  奖励P币金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  指数
        ///</summary>
        public decimal Num { get; set; }
        /// <summary>
        ///  当前指数
        ///</summary>
        public decimal Localnum { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  最后一次更新当前指数时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  最快下一次结算时间
        ///</summary>
        public DateTime Nexttime { get; set; }
        /// <summary>
        ///  类型 1=P指数
        ///</summary>
        public int Typeid { get; set; }
    
        
    }
}