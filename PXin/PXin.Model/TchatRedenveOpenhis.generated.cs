using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 抢红包历史
    /// </summary>
    public partial class TchatRedenveOpenhis
    {
        public TchatRedenveOpenhis()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// tchat_redenve_sendhis.id
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 抢得红包金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否手气最佳,1-手气最佳
        /// </summary>
        public int Isoptimum { get; set; }
    }
}
