using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 热门搜索关键字
    /// </summary>
    public partial class TpxinDarenSearch
    {
        public TpxinDarenSearch()
        { 
Infoid = 0;                                         
Showname = string.Empty;                                         
Times = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  关键字
        ///</summary>
        public string Showname { get; set; }
        /// <summary>
        ///  搜索次数
        ///</summary>
        public int Times { get; set; }
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