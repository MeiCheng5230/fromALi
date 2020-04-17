using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播历史
    /// </summary>
    public partial class TchatLiveroomPeriodHis
    {
        public TchatLiveroomPeriodHis()
        {
            Livewatchcount = 0;
            Liveadmirecount = 0;
            Settlestatus = 0;
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
        /// 当前期次
        /// </summary>
        public int Periodnum { get; set; }
        /// <summary>
        /// 房间位置，经纬度
        /// </summary>
        public string Roomlocation { get; set; }
        /// <summary>
        /// 观看人数
        /// </summary>
        public int Livewatchcount { get; set; }
        /// <summary>
        /// 点赞人数
        /// </summary>
        public int Liveadmirecount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        /// 主播结算状态，0-未结算，1-已结算
        /// </summary>
        public int Settlestatus { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? Settletime { get; set; }
    }
}
