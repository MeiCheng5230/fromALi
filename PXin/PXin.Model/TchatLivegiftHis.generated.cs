using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播送礼历史
    /// </summary>
    public partial class TchatLivegiftHis
    {
        public TchatLivegiftHis()
        {
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
        /// 进入房间历史id,tchat_liveroom_visit_his
        /// </summary>
        public int Visitid { get; set; }
        /// <summary>
        /// 送礼物人的nodeid
        /// </summary>
        public int Fromnodeid { get; set; }
        /// <summary>
        /// 礼物类型
        /// </summary>
        public int Gifttype { get; set; }
        /// <summary>
        /// 礼物钱包ID
        /// </summary>
        public int Giftpurseid { get; set; }
        /// <summary>
        /// 礼物价格
        /// </summary>
        public decimal Giftprice { get; set; }
        /// <summary>
        /// 礼物数量
        /// </summary>
        public decimal Giftnum { get; set; }
        /// <summary>
        /// 送礼时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 状态，0-未结算，1-已结算
        /// </summary>
        public int Status { get; set; }
    }
}
