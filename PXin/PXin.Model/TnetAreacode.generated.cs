using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 电话号码国标代码
    /// </summary>
    public partial class TnetAreacode
    {
        public TnetAreacode()
        { 
Id = 0;                                         
Areaname = null;                                         
Country = null;                                         
Code = null;                                         
Commonuse = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  区域
        ///</summary>
        public string Areaname { get; set; }
        /// <summary>
        ///  国家
        ///</summary>
        public string Country { get; set; }
        /// <summary>
        ///  区号
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  国家首字母拼音
        ///</summary>
        public string EnCountry { get; set; }
        /// <summary>
        ///  常用=1,默认=0
        ///</summary>
        public int Commonuse { get; set; }
    
        
    }
}