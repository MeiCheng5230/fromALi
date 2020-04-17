using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// TpxinPaiHisOld
    /// </summary>
    public partial class TpxinPaiHisOld
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinPaiHisOld()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Num = 0;                                         
Price = 0;                                         
Totalprice = 0;                                         
Status = 0;                                         
Configid = 0;                                         
            }

        public int Hisid { get; set; }
        public int Nodeid { get; set; }
        public int Num { get; set; }
        public decimal Price { get; set; }
        public decimal Totalprice { get; set; }
        public int Status { get; set; }
        public string Rankinfo { get; set; }
        public DateTime Createtime { get; set; }
        public string Remarks { get; set; }
        public int Configid { get; set; }
    
        
    }
}