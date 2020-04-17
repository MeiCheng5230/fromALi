using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 相信APP SVC充值码配置表
    /// </summary>
    public partial class TblcCentcardConfig
    {
        public TblcCentcardConfig()
        { 
Configid = 0;                                         
Areaid = 0;                                         
Showname = string.Empty;                                         
Price = 0;                                         
Bnum = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Configid { get; set; }
        /// <summary>
        ///  tblc_centcard.areaid 1=SVC充值码
        ///</summary>
        public int Areaid { get; set; }
        /// <summary>
        ///  对外显示名
        ///</summary>
        public string Showname { get; set; }
        /// <summary>
        ///  价格元
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  B指数
        ///</summary>
        public decimal Bnum { get; set; }
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