using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 充值码历史表
    /// </summary>
    public partial class TblcCentcardHis
    {
        public TblcCentcardHis()
        { 
Hisid = 0;                                         
Idno = 0;                                         
Typeid = 0;                                         
Nodeid = 0;                                         
Note = string.Empty;                                         
Opnodeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  tblc_centcard.idno
        ///</summary>
        public int Idno { get; set; }
        /// <summary>
        ///  操作类型 0=微信等购买 1=零售 2=红包 3=专户DOS兑换 4=第三方购买 5=使用 6=转让
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  对应的用户ID，使用人，拥有人
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  对外显示的内容
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  操作人ID
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