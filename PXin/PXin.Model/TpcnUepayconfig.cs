using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// PCN UE支付配置
    /// </summary>
    public partial class TpcnUepayconfig
    {
        public TpcnUepayconfig()
        { 
Id = 0;                                         
Typeid = 0;                                         
Paycode = string.Empty;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  业务类型，1-DOS打赏工头
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  收款人标识，支付需要传入此参数
        ///</summary>
        public string Paycode { get; set; }
        /// <summary>
        ///  支付成功通知地址
        ///</summary>
        public string Notifyurl { get; set; }
        /// <summary>
        ///  开启:1,关闭0 默认开启
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
        ///  API Key
        ///</summary>
        public string Accesskeyid { get; set; }
        /// <summary>
        ///  API Secret
        ///</summary>
        public string Accesssecret { get; set; }
    
        
    }
}