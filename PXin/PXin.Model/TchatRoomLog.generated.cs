using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 聊天室日志
    /// </summary>
    public partial class TchatRoomLog
    {
        public TchatRoomLog()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 聊天室iD
        /// </summary>
        public int Roomid { get; set; }
        /// <summary>
        /// 操作,1-进入聊天室，2-退出聊天室
        /// </summary>
        public int Actiontype { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
    }
}
