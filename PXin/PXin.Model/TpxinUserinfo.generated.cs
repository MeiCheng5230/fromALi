using System;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈用户基本信息
    /// </summary>
    public partial class TpxinUserinfo
    {
        public TpxinUserinfo()
        {
            Infoid = 0;
            Nodeid = 0;
            Up = 0;
            Down = 0;
            P = 0;
            V = 0;
            Lastbuysvc = 0;
            Svc = 0;
            Apoint = 0;
            Autochargevamount = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户的NODEID,唯一
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  顶部大图片URL(空时有默认图片)
        ///</summary>
        public string Backpic { get; set; }
        /// <summary>
        ///  总赞数量
        ///</summary>
        public int Up { get; set; }
        /// <summary>
        ///  总踩数量
        ///</summary>
        public int Down { get; set; }
        /// <summary>
        ///  P点(来源于赞/踩/打赏) 可为负数
        ///</summary>
        public decimal P { get; set; }
        /// <summary>
        ///  V点(发布和看文章) &gt;=0
        ///</summary>
        public decimal V { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  上月微信等充值SVC金额(来源于服务)
        ///</summary>
        public decimal Lastbuysvc { get; set; }
        /// <summary>
        ///  当前可兑换到SVC的数量(来源于服务)
        ///</summary>
        public decimal Svc { get; set; }
        /// <summary>
        ///  a点(来源于服务)
        ///</summary>
        public int Apoint { get; set; }
        /// <summary>
        ///  设置自动充值V点金额，=0表示未通，&gt;0表示每次自动充值金额
        ///</summary>
        public decimal Autochargevamount { get; set; }


    }
}