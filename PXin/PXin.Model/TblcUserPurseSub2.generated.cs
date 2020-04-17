using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 子账户关系表
    /// </summary>
    public partial class TblcUserPurseSub2
    {
        public TblcUserPurseSub2()
        { 
Infoid = 0;                                         
Purseid = 0;                                         
Subpurseid = 0;                                         
Pursecode = string.Empty;                                         
Pursename = string.Empty;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  钱包ID
        ///</summary>
        public int Purseid { get; set; }
        /// <summary>
        ///  子钱包ID
        ///</summary>
        public int Subpurseid { get; set; }
        /// <summary>
        ///  子账号号码,唯一
        ///</summary>
        public string Pursecode { get; set; }
        /// <summary>
        ///  子账户名称
        ///</summary>
        public string Pursename { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  用户的NODEID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  0=个人账户 1=企业账户
        ///</summary>
        public int Typeid { get; set; }
    
        
    }
}