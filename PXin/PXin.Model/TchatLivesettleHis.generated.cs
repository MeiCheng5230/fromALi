using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播结算历史
    /// </summary>
    public partial class TchatLivesettleHis
    {
        public TchatLivesettleHis()
        {
            Visitid = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// tchat_liveroom.id
        /// </summary>
        public int Roomid { get; set; }
        /// <summary>
        /// 期数
        /// </summary>
        public int Periodnum { get; set; }
        /// <summary>
        /// 进入房间历史id,tchat_liveroom_visit_his,主播为0
        /// </summary>
        public int Visitid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 礼物钱包ID
        /// </summary>
        public int Giftpurseid { get; set; }
        /// <summary>
        /// 金额，负数为用户送礼，正数为主播收礼
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 转账id
        /// </summary>
        public int Transferid { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime Createtime { get; private set; }
    }
}
