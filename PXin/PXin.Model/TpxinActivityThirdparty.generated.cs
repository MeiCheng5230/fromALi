using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 每月活动绑定pcn帐号表
    /// </summary>
    public partial class TpxinActivityThirdparty
    {
        public TpxinActivityThirdparty()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Targetid = null;
ActivityId = 1;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  第三方帐号
        ///</summary>
        public string Targetid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }

        /// <summary>
        ///  活动表id
        ///</summary>
        public int ActivityId { get; set; }
    }
}