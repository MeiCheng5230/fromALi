using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 卡表
    /// </summary>
    public partial class TblcCentcard
    {
        public TblcCentcard()
        { 
Idno = 0;                                         
Cardno = string.Empty;                                         
Fromid = 0;                                         
            }

        /// <summary>
        ///  序列号:SEQ_TBLC_CENTCARD.nextval
        ///</summary>
        public int Idno { get; set; }
        /// <summary>
        ///  卡号
        ///</summary>
        public string Cardno { get; set; }
        /// <summary>
        ///  卡号密码(说明：ISPWDREQUIRED为0就不需要密码，为1就需要密码)
        ///</summary>
        public string Cardpwd { get; set; }
        /// <summary>
        ///  是否需要密码(0:不需要密码;1:需要密码;)
        ///</summary>
        public int? Ispwdrequired { get; set; }
        /// <summary>
        ///  额度/金额
        ///</summary>
        public decimal? Amount { get; set; }
        /// <summary>
        ///  对应的商家SID,为null时表示与商家无关
        ///</summary>
        public int? Sid { get; set; }
        /// <summary>
        ///  有效期
        ///</summary>
        public DateTime? Expiredtime { get; set; }
        /// <summary>
        ///  卡号生成时间
        ///</summary>
        public DateTime? Createdtime { get; set; }
        /// <summary>
        ///  1=SVC 2=DOS
        ///</summary>
        public string Areaid { get; set; }
        /// <summary>
        ///  状态：0:未启用;1:未使用;2:已使用 3=已转让/已回收 4=冻结
        ///</summary>
        public int? Status { get; set; }
        /// <summary>
        ///  使用者NODEID/购买人
        ///</summary>
        public int? Usenodeid { get; set; }
        /// <summary>
        ///  使用时间
        ///</summary>
        public DateTime? Usedate { get; set; }
        /// <summary>
        ///  对应的产品PRODUCTID，暂时没用
        ///</summary>
        public int? Productid { get; set; }
        /// <summary>
        ///  暂时没用
        ///</summary>
        public int? Assignbusiid { get; set; }
        /// <summary>
        ///  暂时没用
        ///</summary>
        public decimal? Assignvalue { get; set; }
        /// <summary>
        ///  暂时没用
        ///</summary>
        public int? Assignnodeid { get; set; }
        /// <summary>
        ///  暂时没用
        ///</summary>
        public DateTime? Assigntime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  所属期次
        ///</summary>
        public string Period { get; set; }
        /// <summary>
        ///  来源 0=微信购买 1=充值商/代理人零售 2=红包兑换 3=专户DOS兑换，4-第三方支付购买
        ///</summary>
        public int Fromid { get; set; }
    
        
    }
}