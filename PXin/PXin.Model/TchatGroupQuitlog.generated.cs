using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 用户退群历史
    /// </summary>
    public partial class TchatGroupQuitlog
    {
        public TchatGroupQuitlog()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 群id
        /// </summary>
        public int Groupid { get; set; }
        /// <summary>
        /// 0-普通群，1-收费群，2-系统群
        /// </summary>
        public int GroupType { get; set; }
        /// <summary>
        /// 加入此群需要的用户等级，对grouptype=2有效
        /// </summary>
        public int UserGradeLevel { get; set; }
        /// <summary>
        /// 退出类型,1-主动退，2-群主移出群
        /// </summary>
        public int Quittype { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
    }
}
