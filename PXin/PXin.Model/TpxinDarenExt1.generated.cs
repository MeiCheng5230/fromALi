using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人扩展表(类型)
    /// </summary>
    public partial class TpxinDarenExt1
    {
        public TpxinDarenExt1()
        { 
Extid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Ptypeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Extid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型ID,TPXIN_DAREN_TYPE.TYPEID
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  一级分类(查询全部用)
        ///</summary>
        public int Ptypeid { get; set; }
    
        
    }
}