using System;
using System.Collections.Generic;

namespace PXin.Commu.DataAccess
{ 
    /// <summary>
    /// 聊天计费表
    /// </summary>
    public partial class TchatRate
    {
        public TchatRate()
        { 
Id = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  typeid=1,私聊;typeid=2,群聊
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  消息发送者
        ///</summary>
        public int Sender { get; set; }
        /// <summary>
        ///  消息接收者(倍率设置者),typeid=1时receive为tnet_reginfo.nodeid;typeid=2时receiver为tchat_group.groupid
        ///</summary>
        public int Receiver { get; set; }
        /// <summary>
        ///  倍率
        ///</summary>
        public decimal Rate { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime? Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}