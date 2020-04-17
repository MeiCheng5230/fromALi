using System;

namespace PXin.Model
{
    /// <summary>
    /// 相信A点竟拍抽奖
    /// </summary>
    public partial class TpxinLuckDraw
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinLuckDraw()
        {
            Id = 0;
            Nodeid = 0;
            Num = 0;
            Usednum = 0;
            Starttime = new DateTime();
            Endtime = new DateTime();
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  用户NODEID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  可抽奖次数
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  已使用次数
        ///</summary>
        public int Usednum { get; set; }
        /// <summary>
        ///  开始时间
        ///</summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        ///  结束时间
        ///</summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }


    }
}