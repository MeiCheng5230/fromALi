using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// A点竞拍配置表
    /// </summary>
    public partial class TpxinPaiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinPaiConfig()
        { 
Configid = 0;                                         
Month = new DateTime();                                         
Num = 0;                                         
Minprice = 0;                                         
Addprice = 0;                                         
Localprice = 0;
Multiple = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Configid { get; set; }
        /// <summary>
        ///  月份
        ///</summary>
        public DateTime Month { get; set; }
        /// <summary>
        ///  总数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  低价
        ///</summary>
        public decimal Minprice { get; set; }
        /// <summary>
        ///  加价幅度
        ///</summary>
        public decimal Addprice { get; set; }
        /// <summary>
        ///  当前低价
        ///</summary>
        public decimal Localprice { get; set; }
        /// <summary>
        ///  倍数
        ///</summary>
        public int Multiple { get; set; }
    }
}