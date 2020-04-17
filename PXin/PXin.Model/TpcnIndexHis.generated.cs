using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// P指数变化历史表
    /// </summary>
    public partial class TpcnIndexHis
    {
        public TpcnIndexHis()
        { 
Hisid = 0;                                         
Infoid = 0;                                         
Num = 0;                                         
Localnum = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  TPCN_Index_info.infoid
        ///</summary>
        public int Infoid { get; set; }
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
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  注册人数
        ///</summary>
        public int? Regnum { get; set; }
        /// <summary>
        ///  登录人数
        ///</summary>
        public int? Loginnum { get; set; }
        /// <summary>
        ///  登录IP数
        ///</summary>
        public int? Loginipnum { get; set; }
        /// <summary>
        ///  昨天的指数
        ///</summary>
        public decimal? Beforenum { get; set; }
        /// <summary>
        ///  昨天的注册人数
        ///</summary>
        public int? Beregnum { get; set; }
        /// <summary>
        ///  昨天的登录人数
        ///</summary>
        public int? Beloginnum { get; set; }
        /// <summary>
        ///  昨天的登录IP数
        ///</summary>
        public int? Beloginipnum { get; set; }
        /// <summary>
        ///  上一期指数
        ///</summary>
        public decimal? Benum { get; set; }
    
        
    }
}