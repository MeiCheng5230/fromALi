using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// SSO 用户注册临时表
    /// </summary>
    public partial class TssoRegcode
    {
        public TssoRegcode()
        { 
Id = 0;                                         
Regcode = string.Empty;                                         
Status = 0;                                         
Authcode = string.Empty;                                         
Codetype = 0;                                         
Indate = new DateTime();                                         
Regtype = 0;                                         
            }

        public int Id { get; set; }
        /// <summary>
        ///  注册账号
        ///</summary>
        public string Regcode { get; set; }
        /// <summary>
        ///  状态:0.未验证,1.已验证 2.已忽略
        ///</summary>
        public int Status { get; set; }
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  安全码
        ///</summary>
        public string Authcode { get; set; }
        /// <summary>
        ///  账号类型，1.邮箱，2.手机$CodeType$
        ///</summary>
        public int Codetype { get; set; }
        /// <summary>
        ///  有效期
        ///</summary>
        public DateTime Indate { get; set; }
        /// <summary>
        ///  备注信息
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  推荐人
        ///</summary>
        public string Introducer { get; set; }
        /// <summary>
        ///  App商家Id
        ///</summary>
        public int? Appstoreid { get; set; }
        /// <summary>
        ///  来源IP
        ///</summary>
        public string Sourceip { get; set; }
        /// <summary>
        ///  注册类型,1-PC端,2-移动端
        ///</summary>
        public int Regtype { get; set; }
    
        
    }
}