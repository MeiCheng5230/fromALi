using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈赞踩历史表
    /// </summary>
    public partial class TpxinPraise
    {
        public TpxinPraise()
        {
            Hisid = 0;
            Infoid = 0;
            Fromnodeid = 0;
            Tonodeid = 0;
            Status = 0;
            Reward = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  TPXIN_MESSAGE.infoid
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID,INFOID+Fromnodeid唯一
        ///</summary>
        public int Fromnodeid { get; set; }
        /// <summary>
        ///  用户ID,INFOID+NODEID唯一
        ///</summary>
        public int Tonodeid { get; set; }
        /// <summary>
        ///  状态 -1=踩 0=即没赞也没踩 1=赞
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  最后一次操作时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  打赏金额
        ///</summary>
        public int Reward { get; set; }


    }
}