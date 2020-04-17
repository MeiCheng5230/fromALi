using System;

namespace PXin.Model
{
    /// <summary>
    /// TfinBilltrans
    /// </summary>
    public partial class TfinBilltrans
    {
        public TfinBilltrans()
        {
            Transid = 0;
            Nodeid = 0;
            Amount = 0;
            Status = 0;
            Paytype = 0;
        }

        /// <summary>
        ///  主键ID
        ///</summary>
        public int Transid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  充值ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  金额，单位元
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  帮助人ID
        ///</summary>
        public int? Helperid { get; set; }
        /// <summary>
        ///  是否成功    0：等待结果，1：成功  2: 失败
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  最后更新时间
        ///</summary>
        public DateTime? Modifytime { get; set; }
        /// <summary>
        ///  0：CV充值 ，1：ISell网银支付， 2：网页订购网银支付

        ///</summary>
        public int Paytype { get; set; }
        /// <summary>
        ///  资金渠道,0未指定,1数贸钱包,2信付通,3支付宝,4财付通,5PayPal,6拉卡拉,7网上银行,8所有
        ///</summary>
        public int? Chargetype { get; set; }
        /// <summary>
        ///  管理员备注信息
        ///</summary>
        public string OpRemarks { get; set; }
        /// <summary>
        ///  转帐ID组合
        ///</summary>
        public string Transferids { get; set; }
        /// <summary>
        ///  多个网银支付渠道的ID，用于区别不同的签约公司
        ///</summary>
        public string Gateid { get; set; }


    }
}