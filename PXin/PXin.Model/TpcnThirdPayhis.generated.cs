using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// PCN合作者支付历史
    /// </summary>
    public partial class TpcnThirdPayhis
    {
        public TpcnThirdPayhis()
        { 
Hisid = 0;                                         
Partnerid = 0;                                         
Paytype = 0;                                         
Nodeid = 0;                                         
Amount = 0;                                         
Orderno = string.Empty;                                         
Subject = string.Empty;                                         
Paystatus = 0;                                         
Transferids = string.Empty;                                         
Storequest = 0;                                         
Nextnotifytime = new DateTime();                                         
Notifyfailnumber = 0;                                         
            }

        /// <summary>
        ///  主键
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  tpcn_third_partner.id
        ///</summary>
        public int Partnerid { get; set; }
        /// <summary>
        ///  1-P币充值
        ///</summary>
        public int Paytype { get; set; }
        /// <summary>
        ///  tnet_reginfo.nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  消费金额(元)
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  订单号
        ///</summary>
        public string Orderno { get; set; }
        /// <summary>
        ///  商品名称
        ///</summary>
        public string Subject { get; set; }
        /// <summary>
        ///  描述
        ///</summary>
        public string Body { get; set; }
        /// <summary>
        ///  支付时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  支付状态,0为未付款,1为付款成功,2为付款失败 ,3关闭的订单 5已退款，6部分退款
        ///</summary>
        public int Paystatus { get; set; }
        /// <summary>
        ///  转账历史ID
        ///</summary>
        public string Transferids { get; set; }
        /// <summary>
        ///  操作备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  支付成功通知地址
        ///</summary>
        public string Notifyurl { get; set; }
        /// <summary>
        ///  商家是否接收到信息,0等待,1为失败,2为成功
        ///</summary>
        public int Storequest { get; set; }
        /// <summary>
        ///  下一次通知网站时间
        ///</summary>
        public DateTime Nextnotifytime { get; set; }
        /// <summary>
        ///  接收通知失败次数
        ///</summary>
        public int Notifyfailnumber { get; set; }
    
        
    }
}