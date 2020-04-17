using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 发票增票资质审核表
    /// </summary>
    public partial class TpxinInvoiceLimit
    {
        public TpxinInvoiceLimit()
        { 
Id = 0;                                         
Company = string.Empty;                                         
Taxnum = string.Empty;                                         
Address = string.Empty;                                         
Mobile = string.Empty;                                         
Bank = string.Empty;                                         
Cardno = string.Empty;                                         
Status = 0;                                         
Nodeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  公司名
        ///</summary>
        public string Company { get; set; }
        /// <summary>
        ///  税号
        ///</summary>
        public string Taxnum { get; set; }
        /// <summary>
        ///  公司地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  公司电话
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  开户银行
        ///</summary>
        public string Bank { get; set; }
        /// <summary>
        ///  银行卡号
        ///</summary>
        public string Cardno { get; set; }
        /// <summary>
        ///  审核状态 1=审核中 2=审核通过 3=审核拒绝
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
        ///  申请人nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  拒绝理由
        ///</summary>
        public string Note { get; set; }
    
        
    }
}