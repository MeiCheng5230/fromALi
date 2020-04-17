using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 码库购买/出售SVC充值卡历史表
    /// </summary>
    public partial class TblcCentcardAuctionHis
    {
        public TblcCentcardAuctionHis()
        { 
Infoid = 0;                                         
PmInfoid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;
CentcardHisid = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  tpm2_info主键
        ///</summary>
        public int PmInfoid { get; set; }
        /// <summary>
        ///  用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  操作类型(1=出售,2=购买,3=取消发布)
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  充值卡历史表主键id
        ///</summary>
        public int CentcardHisid { get; set; }
        /// <summary>
        ///  批号
        ///</summary>
        public string Batch { get; set; }

    }
}