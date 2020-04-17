using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人视频信息表
    /// </summary>
    public partial class TpxinDarenVideo
    {
        public TpxinDarenVideo()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Url = string.Empty;                                         
Browsenum = 0;                                         
Praisenum = 0;                                         
Duration = 0;                                         
            }

        /// <summary>
        ///  主键
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  视频地址
        ///</summary>
        public string Url { get; set; }
        /// <summary>
        ///  浏览数
        ///</summary>
        public int Browsenum { get; set; }
        /// <summary>
        ///  点赞数
        ///</summary>
        public int Praisenum { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  视频时长（秒）
        ///</summary>
        public int Duration { get; set; }
        /// <summary>
        ///  视频第一帧图片地址
        ///</summary>
        public string Imageurl { get; set; }
    
        
    }
}