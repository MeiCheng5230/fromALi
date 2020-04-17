using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// APP本地H5页面配置
    /// </summary>
    public partial class TappH5Config
    {
        /// <summary>
        /// 
        /// </summary>
        public TappH5Config()
        { 
Id = 0;                                         
Name = null;                                         
Version = null;                                         
Downurl = null;                                         
Onlineurl = null;                                         
Updatetime = DateTime.Now;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  包名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  版本号
        ///</summary>
        public string Version { get; set; }
        /// <summary>
        ///  文件下载地址
        ///</summary>
        public string Downurl { get; set; }
        /// <summary>
        ///  在线服务地址
        ///</summary>
        public string Onlineurl { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  最后更新时间
        ///</summary>
        public DateTime Updatetime { get; set; }
    
        
    }
}