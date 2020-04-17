using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 优谷UE支付历史
    /// </summary>
    public partial class TnetUepayhis
    {
        public TnetUepayhis()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
BusinessParams = string.Empty;                                         
Amount = 0;                                         
Status = 0;                                         
BusinessId = 0;                                         
Unit = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  操作人
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  业务类型，1-经销号换码，2-VIP续费，3-绑定会员码2送1，4-VIP续费1送2，5-专营商开通经销商,6-拍卖资格开通、续费、提升额度
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  业务相关参数,具体业务解析此参数
        ///</summary>
        public string BusinessParams { get; set; }
        /// <summary>
        ///  支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  冻结ID列表，多个用逗号隔开
        ///</summary>
        public string Freezeids { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  状态,0-等待支付，1-已支付，2-已处理
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  UE订单号
        ///</summary>
        public string Ordernoue { get; set; }
        /// <summary>
        ///  通知时间
        ///</summary>
        public DateTime? Noticetime { get; set; }
        /// <summary>
        ///  业务主表ID
        ///</summary>
        public int BusinessId { get; set; }
        /// <summary>
        ///  金额单位
        ///</summary>
        public int Unit { get; set; }
    
        
    }
}