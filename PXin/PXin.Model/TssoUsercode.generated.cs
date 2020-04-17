using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// sso 产生用户帐号记录表
    /// </summary>
    public partial class TssoUsercode
    {
        public TssoUsercode()
        { 
Id = 0;                                         
Usercode = 0;                                         
Status = 0;                                         
            }

        public int Id { get; set; }
        public int Usercode { get; set; }
        /// <summary>
        ///  是否已用 0：初始化 1.已用
        ///</summary>
        public int Status { get; set; }
    
        
    }
}