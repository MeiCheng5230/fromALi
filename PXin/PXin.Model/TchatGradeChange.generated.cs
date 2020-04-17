using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 用户等级变化历史
    /// </summary>
    public partial class TchatGradeChange
    {
        public TchatGradeChange()
        {
            Id = 0;
            Nodeid = 0;
            Fromgrade = 0;
            Tograde = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  变化前等级
        ///</summary>
        public int Fromgrade { get; set; }
        /// <summary>
        ///  变化后等级
        ///</summary>
        public int Tograde { get; set; }
        /// <summary>
        ///  变化时间
        ///</summary>
        public DateTime Createtime { get; set; }


    }
}