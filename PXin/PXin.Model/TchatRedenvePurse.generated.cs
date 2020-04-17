using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 红包使用钱包
    /// </summary>
    public partial class TchatRedenvePurse
    {
        public TchatRedenvePurse()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 钱包名称
        /// </summary>
        public string Pursename { get; set; }
        /// <summary>
        /// 钱包类型,tblc_user_purse.pursetype
        /// </summary>
        public int Pursetype { get; set; }
        /// <summary>
        /// 钱包subid,tblc_user_purse.subid
        /// </summary>
        public int Pursesubid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 金额限制：开始等级[包含],结束等级[包含],金额，提示语|
        /// </summary>
        public string Limitamount { get; set; }
    }
}
