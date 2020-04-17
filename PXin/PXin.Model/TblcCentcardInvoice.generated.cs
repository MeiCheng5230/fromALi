using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// SVC充值码发票
    /// </summary>
    public partial class TblcCentcardInvoice
    {
        public TblcCentcardInvoice()
        { 
Infoid = 0;                                         
Idno = 0;                                         
Status = 0;                                         
Typeid = 0;                                         
Isperson = 0;                                         
Modifytime = new DateTime();                                         
Nodeid = 0;                                         
Address = string.Empty;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  tblc_centcard.idno
        ///</summary>
        public int Idno { get; set; }
        /// <summary>
        ///  状态 1=审核中 2=已开票 3=审核不通过
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  1=电子普通发票 2=增值税专用发票
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  是否个人  1=个人 0=企业
        ///</summary>
        public int Isperson { get; set; }
        /// <summary>
        ///  抬头,个人为抬头,企业为企业名
        ///</summary>
        public string Head { get; set; }
        /// <summary>
        ///  企业识别码
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  最后一次修改状态时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  用户填写的地址,增值发票时此字段为地址表id,普通发票时为邮箱地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  快递号
        ///</summary>
        public string Expressno { get; set; }
    
        
    }
}