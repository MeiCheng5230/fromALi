using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播房间用户进出历史
    /// </summary>
    public partial class TchatLiveroomVisitHis
    {
        public TchatLiveroomVisitHis()
        {
            Inputtime = DateTime.Now;
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
        /// tchat_liveroom.id
        /// </summary>
        public int Roomid { get; set; }
        /// <summary>
        /// 期数
        /// </summary>
        public int Periodnum { get; set; }
        /// <summary>
        /// 进入时间
        /// </summary>
        public DateTime Inputtime { get; set; }
        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime Hbtime { get; set; }
        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime? Outputtime { get; set; }
        /// <summary>
        /// 冻结历史id
        /// </summary>
        public string Freezeids { get; set; }
        /// <summary>
        /// 冻结礼物钱包id
        /// </summary>
        public string Freezegiftpurseids { get; set; }
        /// <summary>
        /// 冻结金额
        /// </summary>
        public string Freezeamounts { get; set; }
        /// <summary>
        /// 冻结状态，0-未解冻，1-已解冻
        /// </summary>
        public int Freezestatus { get; set; }
    }
}
