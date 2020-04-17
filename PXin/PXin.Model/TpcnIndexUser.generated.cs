using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户P指数历史表
    /// </summary>
    public partial class TpcnIndexUser
    {
        public TpcnIndexUser()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Num = 0;                                         
Periods = 0;                                         
Fromid = 0;                                         
Typeid = 0;                                         
Price = 0;                                         
Status = 0;                                         
Settle = 0;                                         
Localnum = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  数量
        ///</summary>
        public decimal Num { get; set; }
        /// <summary>
        ///  下一期期数，插入时默认值为1
        ///</summary>
        public int Periods { get; set; }
        /// <summary>
        ///  来源 0=购买入门版 1=高级版 2=购买广告码 3=升级版 7=升级vip
        ///</summary>
        public int Fromid { get; set; }
        /// <summary>
        ///  外界ID,由FROMID来定，用于对账
        ///</summary>
        public int? Pkid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  类型，1-P指数
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  状态 0=待确认 1=已确认 2=已取消
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  是否结算 0=不结算 1=结算
        ///</summary>
        public int Settle { get; set; }
        /// <summary>
        ///  过期回收，关联tnet_paivip_info.id(BOBO用)
        ///</summary>
        public int? Gqid { get; set; }
        /// <summary>
        ///  已回馈总金额
        ///</summary>
        public decimal Localnum { get; set; }
    
        
    }
}