using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 推荐达人列表
    /// </summary>
    public partial class TpxinDarenDefault
    {
        public TpxinDarenDefault()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Orderno = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型 0=聊一聊 1=达人专区
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  排序,由小到大
        ///</summary>
        public int Orderno { get; set; }
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