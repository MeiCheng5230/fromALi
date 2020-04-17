using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 相信可兑换商品
    /// </summary>
    public partial class TpxinChargeProduct
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinChargeProduct()
        { 
Id = 0;                                         
Typeid = 0;                                         
Name = null;                                         
Pic = null;                                         
Price = 0;                                         
Priceunit = null;                                         
Purseconfigid = 0;                                         
Seqno = 0;                                         
Isdel = 0;                                         
Pdtvalue = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  类型 1=兑换SV 2=兑换SVC 3=兑换YG的会员码 4=兑换PCN的认证码
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  图片
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  价格
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  价格单位，显示用
        ///</summary>
        public string Priceunit { get; set; }
        /// <summary>
        ///  支付钱包tnet_purse_config.infoid
        ///</summary>
        public int Purseconfigid { get; set; }
        /// <summary>
        ///  显示顺序，小数排前面
        ///</summary>
        public int Seqno { get; set; }
        /// <summary>
        ///  是否删除,1-删除
        ///</summary>
        public int Isdel { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  产品价值
        ///</summary>
        public decimal Pdtvalue { get; set; }
        /// <summary>
        /// 商品说明
        /// </summary>
        public string Note { get; set; }
    }
}