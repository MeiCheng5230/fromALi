using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 群组发送历史
    /// </summary>
    public partial class TchatGroupSendHis
    {
        public TchatGroupSendHis()
        {
            Id = 0;
            Nodeid = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        public int Nodeid { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public int Groupid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }



    }
}