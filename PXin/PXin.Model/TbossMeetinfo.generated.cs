using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 会议信息表
    /// </summary>
    public partial class TbossMeetinfo
    {
        public TbossMeetinfo()
        { 
Infoid = 0;                                         
Title = null;                                         
Starttime = new DateTime();                                         
Address = null;                                         
Detail = null;                                         
Opnoded = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  开始时间,精准到年月日时分秒
        ///</summary>
        public DateTime Starttime { get; set; }
        /// <summary>
        ///  地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  详情URL完整地址
        ///</summary>
        public string Detail { get; set; }
        /// <summary>
        ///  录入人NODEID
        ///</summary>
        public int Opnoded { get; set; }
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