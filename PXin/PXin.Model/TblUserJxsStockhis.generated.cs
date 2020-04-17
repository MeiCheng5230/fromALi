using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 经销商专营商进出货历史
    /// </summary>
    public partial class TblUserJxsStockhis
    {
        public TblUserJxsStockhis()
        { 
Id = 0;                                         
Batchnum = 0;                                         
Jsxid = 0;                                         
Typeid = 5;                                         
Stocktype = 0;                                         
Num = 0;                                         
Amountdp = 0;                                         
Amountdos = 0;                                         
Transferids = null;                                         
Numcua = 0;                                         
Rate = 1;
Status = 0;
Isshow = 1;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  进货批次，进出货批次相同
        ///</summary>
        public int Batchnum { get; set; }
        /// <summary>
        ///  tbl_user_jsx.infoid,-1为系统
        ///</summary>
        public int Jsxid { get; set; }
        /// <summary>
        ///  1=总公司 2=省 3=市 4=区 5=经销商, &lt;=4专营商
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  进出货类型，1-零售码进货，2-批发码进货，3-零售码出货，4-批发码出货
        ///</summary>
        public int Stocktype { get; set; }
        /// <summary>
        ///  进出货数量,NUM-NUMCUA=正常进货数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  DP支付金额
        ///</summary>
        public decimal Amountdp { get; set; }
        /// <summary>
        ///  DOS支付金额
        ///</summary>
        public decimal Amountdos { get; set; }
        /// <summary>
        ///  转账ID
        ///</summary>
        public string Transferids { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  CUA进出货数量
        ///</summary>
        public int Numcua { get; set; }
        /// <summary>
        ///  比例
        ///</summary>
        public decimal Rate { get; set; }
        /// <summary>
        ///  Pcn Nodecode
        ///</summary>
        public string Pcnnodecode { get; set; }
        /// <summary>
        ///  参加活动处理状态，0=不参与，1=待审核，2=已奖励 3=审核通过 4=审核拒绝 5=余额不足
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  是否显示 1=显示
        ///</summary>
        public int Isshow { get; set; }

    }
}