using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户协议分类
    /// </summary>
    public partial class TnetUserProtocal
    {
        public TnetUserProtocal()
        { 
Id = 0;                                         
Name = string.Empty;                                         
Type = 0;                                         
Version = 0;                                         
Content = string.Empty;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  协议名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  类型 20001-注册协议，2-会员协议，20003-竞拍协议，20004-充值协议
        ///</summary>
        public int Type { get; set; }
        /// <summary>
        ///  版本号
        ///</summary>
        public int Version { get; set; }
        /// <summary>
        ///  协议内容存URL地址
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  状态，1-有效，0-无效
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