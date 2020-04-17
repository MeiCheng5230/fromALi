using System;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈评论历史
    /// </summary>
    public partial class TpxinCommentHis
    {
        public TpxinCommentHis()
        {
            Hisid = 0;
            Infoid = 0;
            Nodeid = 0;
            Content = string.Empty;
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
        ///  发布评论人用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  具体内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  状态 0=删除 1=正常
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
        ///  回复评论,自关联
        ///</summary>
        public int Phisid { get; set; }
        /// <summary>
        ///  回复人的NODEID
        ///</summary>
        public int Pnodeid { get; set; }
    }
}