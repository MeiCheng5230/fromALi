using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 消息/站内信
    /// </summary>
    public partial class TueMessage
    {
        public TueMessage()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Title = string.Empty;                                         
Status = 0;                                         
Modifytime = new DateTime();                                         
Typeid = 0;                                         
Fkid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  具体内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  状态 0=未查看 1=已查看
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  修改时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  类型 1=系统
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  外界ID,与具体业务有关
        ///</summary>
        public int Fkid { get; set; }
        /// <summary>
        ///  需要跳转的页面地址
        ///</summary>
        public string Jumpurl { get; set; }
    
        
    }
}