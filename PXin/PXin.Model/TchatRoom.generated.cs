using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 聊天室
    /// </summary>
    public partial class TchatRoom
    {
        public TchatRoom()
        {
            Roomtype = 0;
            Roomstate = 0;
            Transferid = 0;
            Personcount = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 0-普通聊天室，1-收费聊天室,2-系统聊天室
        /// </summary>
        public int Roomtype { get; set; }
        /// <summary>
        /// 聊天室名称
        /// </summary>
        public string Roomname { get; set; }
        /// <summary>
        /// 聊天室密码
        /// </summary>
        public string Roompwd { get; set; }
        /// <summary>
        /// 聊天室备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 群组状态，0-待提交，1-正常,2-解散
        /// </summary>
        public int Roomstate { get; set; }
        /// <summary>
        /// 转账Id
        /// </summary>
        public int Transferid { get; set; }
        /// <summary>
        /// 解散时间
        /// </summary>
        public DateTime? Dismisstime { get; set; }
        /// <summary>
        /// 聊天室图片
        /// </summary>
        public string Roompic { get; set; }
        /// <summary>
        /// 聊天室当前人数
        /// </summary>
        public int Personcount { get; set; }
    }
}
