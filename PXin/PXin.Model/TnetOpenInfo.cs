using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 功能开通表
    /// </summary>
    public partial class TnetOpenInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public TnetOpenInfo()
        { 
Infoid = 0;                                         
Typeid = 0;                                         
Nodeid = 0;                                         
Payment = 1;                                         
Amount = 0;                                         
Fromtime = DateTime.Now;                                         
Endtime = DateTime.Now.AddDays(365);                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  类型 2=开通乐购红包, 10001=双基数DOS,20001-开通相信专户DOS
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  支付方式 1=UE的DOS
        ///</summary>
        public int Payment { get; set; }
        /// <summary>
        ///  金额(PAYMENT对应的主单位)
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  开始时间
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  结束时间
        ///</summary>
        public DateTime Endtime { get; set; }
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