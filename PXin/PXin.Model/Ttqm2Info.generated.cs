using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 提取码基本信息表
    /// </summary>
    public partial class Ttqm2Info
    {
        public Ttqm2Info()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Pwd = string.Empty;                                         
Typeid = 0;                                         
Price = 0;                                         
Fee = 0;                                         
Status = 0;                                         
Modifytime = new DateTime();                                         
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
        ///  提取码卡号/密码
        ///</summary>
        public string Pwd { get; set; }
        /// <summary>
        ///  类型 1:广告SV 2:相信SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  手续费
        ///</summary>
        public decimal Fee { get; set; }
        /// <summary>
        ///  状态 0=未使用 1=已使用 2=已转换
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  最后一次状态修改时间
        ///</summary>
        public DateTime Modifytime { get; set; }
    
        
    }
}