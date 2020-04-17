using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信群组禁言用户
    /// </summary>
    public partial class TchatGroupUsergag
    {
        public TchatGroupUsergag()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public int Groupid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Userid { get; set; }
        /// <summary>
        /// 禁言时长，以分钟为单位，最大值为43200分钟
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// 禁言开始时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public int Optnodeid { get; set; }
        /// <summary>
        /// 状态，0-禁言，1-取消禁言
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 取消禁言时间
        /// </summary>
        public DateTime? Canceltime { get; set; }
    }
}
