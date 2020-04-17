using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Commu.DataAccess
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
        ///  数量
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
