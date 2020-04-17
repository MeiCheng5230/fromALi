using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 更换充值商历史表
    /// </summary>
    public partial class TblUserJxsChange
    {
        public TblUserJxsChange()
        { 
Hisid = 0;                                         
Infoid = 0;                                         
Typeid = 0;                                         
Fromstatus = 0;                                         
Endstatus = 0;                                         
Note = null;                                         
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
        ///  tbl_user_jxs.typeid
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  变化前的值
        ///</summary>
        public int Fromstatus { get; set; }
        /// <summary>
        ///  变化后的值
        ///</summary>
        public int Endstatus { get; set; }
        /// <summary>
        ///  对外的描述
        ///</summary>
        public string Note { get; set; }
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