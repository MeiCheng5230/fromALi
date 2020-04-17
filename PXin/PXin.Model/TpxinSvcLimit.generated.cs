using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 生成SVC限制规则表
    /// </summary>
    public partial class TpxinSvcLimit
    {
        public TpxinSvcLimit()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Totalamount = 0;                                         
Localamount1 = 0;                                         
Localamount2 = 0;                                         
Fromtime = new DateTime();                                         
Endtime = new DateTime();                                         
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
        ///  总计限额
        ///</summary>
        public decimal Totalamount { get; set; }
        /// <summary>
        ///  当前已使用SV生成SVC充值码限额
        ///</summary>
        public decimal Localamount1 { get; set; }
        /// <summary>
        ///  当前已使用SV提取码生成SVC充值码限额
        ///</summary>
        public decimal Localamount2 { get; set; }
        /// <summary>
        ///  开始时间,NODEID+起止时间段最多1条数据
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  结束时间
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