using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 好友昵称
    /// </summary>
    public partial class TchatFriendNick
    {
        public TchatFriendNick()
        {
            Allowviewmedynamic = 1;
            Viewhedynamic = 1;
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
        /// 好友备注
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 是否允许查看我的动态，1-允许，0-不充许
        /// </summary>
        public int Allowviewmedynamic { get; set; }
        /// <summary>
        /// 是否查看他的动态，1-看，0-不看
        /// </summary>
        public int Viewhedynamic { get; set; }
    }
}
