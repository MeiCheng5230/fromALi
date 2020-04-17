using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// TchatFeehis
    /// </summary>
    public partial class TchatFeehis
    {
        public TchatFeehis()
        {
            Hisid = 0;
            Nodeid = 0;
            Feetype = 0;
            Businesstype = 0;
            Groupid = 0;
            Num = 0;
            Receiver = 0;
            Amount = 0;
            Sendtime = new DateTime();
            Status = 0;
            Remarks = null;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  计费类型，1-V点，2-P点
        ///</summary>
        public int Feetype { get; set; }
        /// <summary>
        ///  业务类型,1-文字信息，2-图片信息，3-语音信息，4-表情信息，5-地图信息
        ///</summary>
        public int Businesstype { get; set; }
        /// <summary>
        ///  群ID
        ///</summary>
        public int Groupid { get; set; }
        /// <summary>
        ///  数量(BusinessType=1时表示字数，
        ///  BusinessType=2时表示图片大（小单位：M），
        ///  BusinessType=3时表示语音长度（单位：秒），
        ///  BusinessType=4时表示表情个数BusinessType = 5时表示地图条数)
        ///</summary>
        public decimal Num { get; set; }
        /// <summary>
        ///  接收者NodeId
        ///</summary>
        public int Receiver { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  扣费时间
        ///</summary>
        public DateTime Sendtime { get; set; }
        /// <summary>
        ///  状态，0-未结算，1-已结算
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  消息序列号,唯一
        ///</summary>
        public string Sequenceid { get; set; }


    }
}