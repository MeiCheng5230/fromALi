using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户协议
    /// </summary>
    public partial class TnetUserAgreement
    {
        public TnetUserAgreement()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Type = 0;                                         
Agreed = 0;                                         
Version = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  tnet_reginfo.nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  1-优谷注册协议，2-优谷会员协议--20001-相信注册协议，20003-相信竞拍协议，20004-相信充值协议
        ///</summary>
        public int Type { get; set; }
        /// <summary>
        ///  1-同意
        ///</summary>
        public int Agreed { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  版本号
        ///</summary>
        public int Version { get; set; }
    
        
    }
}