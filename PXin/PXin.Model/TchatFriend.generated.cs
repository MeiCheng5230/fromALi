using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信我的好码
    /// </summary>
    public partial class TchatFriend
    {
        public TchatFriend()
        {
            Friendstatus = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// tnet_reginfo.nodeid
        /// </summary>
        public int Mynodeid { get; set; }
        /// <summary>
        /// 好友nodeid
        /// </summary>
        public int Friendnodeid { get; set; }
        /// <summary>
        /// 添加好友时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 0-申请状态，1-通过
        /// </summary>
        public int Friendstatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
