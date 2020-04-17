using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 零售码库存零售历史
    /// </summary>
    public partial class TblUserJxsStockhis2
    {
        public TblUserJxsStockhis2()
        { 
Hisid = 0;                                         
Infoid = 0;                                         
Typeid = 0;                                         
Nodeid = 0;                                         
Amount = 0;                                         
Num = 0;                                         
Totalamount = 0;                                         
Opnodeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  tbl_user_jxs.infoid
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  类型 0=零售给用户 1=用户转让给充值商
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  另一方NODEID，合伙人Nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  面额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  张数
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  总量AMOUNT*NUM
        ///</summary>
        public decimal Totalamount { get; set; }
        /// <summary>
        ///  操作人NODEID
        ///</summary>
        public int Opnodeid { get; set; }
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