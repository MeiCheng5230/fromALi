using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信群组用户
    /// </summary>
    public partial class TchatGroupUser
    {
        public TchatGroupUser()
        {
            Creattime = DateTime.Now;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 群组ID,tchat_group.id
        /// </summary>
        public int Groupid { get; set; }
        /// <summary>
        /// 群组成员Id,nodeid
        /// </summary>
        public int Userid { get; set; }
        /// <summary>
        /// 加入群组时间
        /// </summary>
        public DateTime Creattime { get; set; }
    }
}
