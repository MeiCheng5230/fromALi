using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 兑换历史表
    /// </summary>
    public partial class TpxinChargeHis
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinChargeHis()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Purseconfigid = 0;                                         
Price = 0;                                         
Num = 0;                                         
Amount = 0;                                         
Outnodeid = 0;                                         
Note = null;                                         
Status = 1;                                         
Fkid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型 1=兑换SV 2=兑换SVC 3=兑换YG的会员码 4=兑换PCN的认证码
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  支付钱包tnet_purse_config.infoid
        ///</summary>
        public int Purseconfigid { get; set; }
        /// <summary>
        ///  价格
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  兑换数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  拥有者NODEID，TYPEID为1,2时同NODEID，3时为YG的NODEID 4时为PCN的NODEID
        ///</summary>
        public int Outnodeid { get; set; }
        /// <summary>
        ///  用户能看到的说明
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  状态-1=已退款 0=已付款 1=已完成
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  外键唯一ID，如无则为0
        ///</summary>
        public int Fkid { get; set; }
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