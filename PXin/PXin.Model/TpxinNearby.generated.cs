using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// P信 附近的人
    /// </summary>
    public partial class TpxinNearby
    {
        public TpxinNearby()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Longitude = string.Empty;                                         
Latitude = string.Empty;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  昵称
        ///</summary>
        public string Nickname { get; set; }
        /// <summary>
        ///  头像
        ///</summary>
        public string Photo { get; set; }
        /// <summary>
        ///  经度
        ///</summary>
        public string Longitude { get; set; }
        /// <summary>
        ///  纬度
        ///</summary>
        public string Latitude { get; set; }
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