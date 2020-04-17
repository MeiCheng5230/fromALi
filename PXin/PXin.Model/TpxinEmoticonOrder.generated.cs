using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 表情包订单
    /// </summary>
    public partial class TpxinEmoticonOrder
    {
        public TpxinEmoticonOrder()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Materialid = 0;                                         
Amount = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  tnet_reginfo.nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  tpxin_emoticon_material.id
        ///</summary>
        public int Materialid { get; set; }
        /// <summary>
        ///  支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  转账ID
        ///</summary>
        public string Transferid { get; set; }
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