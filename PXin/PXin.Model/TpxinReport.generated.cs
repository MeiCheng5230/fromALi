using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈举报表
    /// </summary>
    public partial class TpxinReport
    {
        public TpxinReport()
        {
            Id = 0;
            Infoid = 0;
            Nodeid = 0;
            Reason = 0;
            Satatus = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  TPXIN_MESSAGE.infoid
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  操作人id
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  投诉原因
        ///</summary>
        public int Reason { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  0=待处理，1=已处理
        ///</summary>
        public int Satatus { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }


    }
}