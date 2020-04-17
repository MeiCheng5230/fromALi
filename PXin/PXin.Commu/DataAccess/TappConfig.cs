using System;
using System.Collections.Generic;

namespace PXin.Commu.DataAccess
{ 
    /// <summary>
    /// app配置信息表
    /// </summary>
    public partial class TappConfig
    {
        public TappConfig()
        { 
Id = 0;                                         
Sid = 0;                                         
Propertyname = null;                                         
Updatetime = DateTime.Now;                                         
            }

        public int Id { get; set; }
        /// <summary>
        ///  App商家ID
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  分类名称
        ///</summary>
        public string Typename { get; set; }
        /// <summary>
        ///  属性名称
        ///</summary>
        public string Propertyname { get; set; }
        /// <summary>
        ///  属性值
        ///</summary>
        public string Propertyvalue { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        ///  更新时间
        ///</summary>
        public DateTime Updatetime { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
    
        
    }
}