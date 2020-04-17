using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// PCN合作者
    /// </summary>
    public partial class TpcnThirdPartner
    {
        public TpcnThirdPartner()
        { 
Id = 0;                                         
Accesskeyid = string.Empty;                                         
Accesssecret = string.Empty;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  20位随机字符串,A-Za-z1-9
        ///</summary>
        public string Accesskeyid { get; set; }
        /// <summary>
        ///  32位随机字符串,a-z1-9
        ///</summary>
        public string Accesssecret { get; set; }
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  商家名字
        ///</summary>
        public string Storename { get; set; }
        /// <summary>
        ///  商家Logo
        ///</summary>
        public string Logo { get; set; }
    
        
    }
}