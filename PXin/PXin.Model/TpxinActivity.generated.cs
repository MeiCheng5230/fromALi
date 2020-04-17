using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 每月活动表
    /// </summary>
    public partial class TpxinActivity
    {
        public TpxinActivity()
        { 
Id = 0;                                         
ActivityStarttime = DateTime.Now;                                         
ActivityEndtime = DateTime.Now;                                         
PayStarttime = DateTime.Now;                                         
PayEndtime = DateTime.Now;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  活动名称
        ///</summary>
        public string ActivityName { get; set; }
        /// <summary>
        ///  活动开始时间
        ///</summary>
        public DateTime ActivityStarttime { get; set; }
        /// <summary>
        ///  活动结束时间
        ///</summary>
        public DateTime ActivityEndtime { get; set; }
        /// <summary>
        ///  缴费开始时间
        ///</summary>
        public DateTime PayStarttime { get; set; }
        /// <summary>
        ///  缴费结束时间
        ///</summary>
        public DateTime PayEndtime { get; set; }
        /// <summary>
        ///  封面图片
        ///</summary>
        public string Cover { get; set; }
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