using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播间关注历史
    /// </summary>
    public partial class TchatLiveFans
    {
        public TchatLiveFans()
        {
            Status = 1;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户nodeid
        /// </summary>
        public int Mynodeid { get; set; }
        /// <summary>
        /// 粉丝用户nodeid
        /// </summary>
        public int Fansid { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 状态，1-已关注，2-取消关注
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 取消关注时间
        /// </summary>
        public DateTime? Canceltime { get; set; }
    }
}
