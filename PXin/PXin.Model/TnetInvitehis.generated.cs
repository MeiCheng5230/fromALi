using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 注册邀请历史
    /// </summary>
    public partial class TnetInvitehis
    {
        public TnetInvitehis()
        {
            Id = 0;
            Sid = 0;
            Pnodeid = 0;
            Mobileno = null;
            Transferid = 0;
            Status = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  sid
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  邀请人nodeid
        ///</summary>
        public int Pnodeid { get; set; }
        /// <summary>
        ///  被邀请人手机号码
        ///</summary>
        public string Mobileno { get; set; }
        /// <summary>
        ///  转账id
        ///</summary>
        public int Transferid { get; set; }
        /// <summary>
        ///  邀请时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  状态,0-有效，1-失效，被替换 相信（0=默认，1=邀请用户已注册）
        ///</summary>
        public int Status { get; set; }


    }
}