using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信群姐
    /// </summary>
    public partial class TchatGroup
    {
        public TchatGroup()
        {
            Id = 0;
            Groupname = string.Empty;
            Creater = 0;
            Groupstate = 0;
            Grouptype = 0;
            Transferid = 0;
            Usergradelevel = 0;
            Auditstate = 0;
            Authstate = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  群组名称
        ///</summary>
        public string Groupname { get; set; }
        /// <summary>
        ///  群组描述
        ///</summary>
        public string Descript { get; set; }
        /// <summary>
        ///  创建者
        ///</summary>
        public int Creater { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  群组状态，0-待提交，1-正常,2-解散
        ///</summary>
        public int Groupstate { get; set; }
        /// <summary>
        ///  解散时间
        ///</summary>
        public DateTime? Dismisstime { get; set; }
        /// <summary>
        ///  0-普通群，1-收费群,2-系统群,3-广播群
        ///</summary>
        public int Grouptype { get; set; }
        /// <summary>
        ///  转账Id
        ///</summary>
        public int Transferid { get; set; }
        /// <summary>
        ///  加入此群需要的用户等级，对grouptype=2有效
        ///</summary>
        public int Usergradelevel { get; set; }
        /// <summary>
        ///  群组头像
        ///</summary>
        public string Grouppic { get; set; }
        /// <summary>
        ///  群组号
        ///</summary>
        public string Groupcode { get; set; }
        /// <summary>
        ///  审核状态，0-等待审核，1-审核通过，2-审核拒绝
        ///</summary>
        public int Auditstate { get; set; }
        /// <summary>
        ///  认证状态，0-未认证，1-已认证
        ///</summary>
        public int Authstate { get; set; }

        /// <summary>
        /// 群组人数
        /// </summary>
        public int PersonCount { get; set; }
    }
}