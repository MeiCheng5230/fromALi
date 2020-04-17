using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人类型表
    /// </summary>
    public partial class TpxinDarenType
    {
        public TpxinDarenType()
        { 
Typeid = 0;                                         
Ptypeid = 0;                                         
Typename = string.Empty;                                         
Pic = string.Empty;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  上级,自关联,0表示一级
        ///</summary>
        public int Ptypeid { get; set; }
        /// <summary>
        ///  类型名/显示名
        ///</summary>
        public string Typename { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  分类完整图片URL
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  分类下是否有达人引用
        ///</summary>
        public int Status { get; set; }
    
        
    }
}