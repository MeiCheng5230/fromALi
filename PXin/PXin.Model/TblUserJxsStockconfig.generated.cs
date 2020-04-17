using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 充值商/代理人进出货配置表
    /// </summary>
    public partial class TblUserJxsStockconfig
    {
        public TblUserJxsStockconfig()
        { 
Infoid = 0;                                         
Fromtime = DateTime.Now;                                         
Endtime = DateTime.Now;                                         
Typeid = 0;                                         
Isfirst = 0;                                         
Dp = 0;                                         
Dos = 0;                                         
Lsm = 0;                                         
Pfm = 0;                                         
Rate = 1;                                         
Title = null;                                         
Isrenew = 0;                                         
Isallowcua = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  从时间
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  到时间
        ///</summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        ///  专营商类型 4=为充值商 5=代理人
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  是否首次进货
        ///</summary>
        public int Isfirst { get; set; }
        /// <summary>
        ///  需要DP
        ///</summary>
        public decimal Dp { get; set; }
        /// <summary>
        ///  需要UE的DOS
        ///</summary>
        public decimal Dos { get; set; }
        /// <summary>
        ///  零售码数量
        ///</summary>
        public int Lsm { get; set; }
        /// <summary>
        ///  批发码数量
        ///</summary>
        public int Pfm { get; set; }
        /// <summary>
        ///  比例
        ///</summary>
        public decimal Rate { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  显示名/标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  副标题
        ///</summary>
        public string Subtitle { get; set; }
        /// <summary>
        ///  是否续费，0-未续费1-已续费
        ///</summary>
        public int Isrenew { get; set; }
        /// <summary>
        ///  是否为促销活动，0=不是，1=是
        ///</summary>
        public int Isallowcua { get; set; }
    }
}