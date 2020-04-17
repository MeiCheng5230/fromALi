using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人浏览历史表
    /// </summary>
    public partial class TpxinDarenBrowseHis
    {
        public TpxinDarenBrowseHis()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Pnodeid = 0;                                         
Modifytime = new DateTime();                                         
Typeid = 0;                                         
Videoid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  浏览人nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  被浏览人nodeid
        ///</summary>
        public int Pnodeid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  修改时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  操作类型 1=主页浏览 2=主页点赞 3=视频浏览 4=视频点赞
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  表TPXIN_DAREN_VIDEO主键id
        ///</summary>
        public int Videoid { get; set; }
    
        
    }
}