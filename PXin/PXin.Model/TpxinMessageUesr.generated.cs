using System;

namespace PXin.Model
{
    /// <summary>
    /// 朋友圈推送用户表
    /// </summary>
    public partial class TpxinMessageUesr
    {
        public TpxinMessageUesr()
        {
            Hisid = 0;
            Infoid = 0;
            Nodeid = 0;
            Status = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  TPXIN_MESSAGE.INFOID
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  被推送的用户
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  是否已发送给用户 0=未发送 1=已发送
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
        /// 0-文章，1-评论
        /// </summary>
        public int Typeid { get; set; }
    }
}