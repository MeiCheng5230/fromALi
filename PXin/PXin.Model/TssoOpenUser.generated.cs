using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 开放平台用户注册信息
    /// </summary>
    public partial class TssoOpenUser
    {
        public TssoOpenUser()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Opentype = 0;                                         
Openid = string.Empty;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  第三方平台类型 1-QQ、2-微信、3-微博、4-Pcn、5-优谷
        ///</summary>
        public int Opentype { get; set; }
        /// <summary>
        ///  第三方账号唯一标识(优谷和PCN 为NODECODE)
        ///</summary>
        public string Openid { get; set; }
        /// <summary>
        ///  绑定时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}