using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 相信红包信息扩展表
    /// </summary>
    public partial class TbtcYdTransferHisExt2
    {
        public TbtcYdTransferHisExt2()
        { 
Extid = 0;                                         
Nodeid = 0;                                         
Hisid = 0;                                         
Infoid = 0;                                         
Num = 0;                                         
Typeid = 0;                                         
Amount = 0;                                         
Status = 0;                                         
Endtime = DateTime.Now;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Extid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  tbtc_yd_transfer_his.hisid
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  TPXIN_PAI_USER.infoid
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  奖励类型 1=SV 2=SVC充值码 3=专户DOS
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  奖励金额元
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  状态 -1=不符合条件，不可领取 0=未结算 1=已结算
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  预期结算时间TPXIN_PAI_USER.endtime
        ///</summary>
        public DateTime Endtime { get; set; }
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