using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播间礼物钱包
    /// </summary>
    public partial class TchatLivegiftPurse
    {
        public TchatLivegiftPurse()
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
        /// 拥有者类型,tblc_user_purse.ownertype
        /// </summary>
        public int Ownertype { get; set; }
        /// <summary>
        /// 钱包类型,tblc_user_purse.pusertype
        /// </summary>
        public int Pursertype { get; set; }
        /// <summary>
        /// 子钱包ID,tblc_user_purse.subid
        /// </summary>
        public int Subid { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unitname { get; set; }
    }
}
