using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 客服操作历史
    /// </summary>
    public partial class TcsChangeHis
    {
        public TcsChangeHis()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Fee = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  被操作用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  操作前的值
        ///</summary>
        public string Olddata { get; set; }
        /// <summary>
        ///  操作后的值
        ///</summary>
        public string Newdata { get; set; }
        /// <summary>
        ///  手续费
        ///</summary>
        public int Fee { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  客服人员ID
        ///</summary>
        public int? Opnodeid { get; set; }
        /// <summary>
        ///  内部备注
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}