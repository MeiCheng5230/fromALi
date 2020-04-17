using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户中奖信息表
    /// </summary>
    public partial class TpxinLuckDrawDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinLuckDrawDetail()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Num = 0;                                         
Status = 0;                                         
Fromtime = DateTime.Now;                                         
Endtime = DateTime.Now;                                         
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
        ///  中奖数量 0,1,2,8
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  状态 0=未使用 1=已使用
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  有效期开始时间
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  有效期结束时间
        ///</summary>
        public DateTime Endtime { get; set; }
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