using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户A点信息（数据来源于服务）
    /// </summary>
    public partial class TpxinPaiUser
    {
        public TpxinPaiUser()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Num = 0;                                         
Fromtime = new DateTime();                                         
Endtime = new DateTime();                                         
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
        ///  A点数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  有效期从
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  有效期到
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