using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播间冻结历史
    /// </summary>
    public partial class TchatLiveroomFreezeHis
    {
        public TchatLiveroomFreezeHis()
        {
            Id = 0;
            Visitid = 0;
            Nodeid = 0;
            Giftpurseid = 0;
            Amount = 0;
            Freezeid = 0;
            Status = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  tchat_liveroom_visit_his.id
        ///</summary>
        public int Visitid { get; set; }
        /// <summary>
        ///  nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  tchat_livegift_purse.id
        ///</summary>
        public int Giftpurseid { get; set; }
        /// <summary>
        ///  冻结金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  tblc_freeze_his.hisid
        ///</summary>
        public int Freezeid { get; set; }
        /// <summary>
        ///  冻结时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  状态,0-冻结，1-已解冻
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  解冻时间
        ///</summary>
        public DateTime? Unfreezetime { get; set; }


    }
}