using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 评论用户推送表
    /// </summary>
    public partial class TpxinCommentHisUser
    {
        public TpxinCommentHisUser()
        { 
Pkid = 0;                                         
Hisid = 0;                                         
Nodeid = 0;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Pkid { get; set; }
        /// <summary>
        ///  TPXIN_COMMENT_HIS.HISID
        ///</summary>
        public int Hisid { get; set; }
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
    
        
    }
}