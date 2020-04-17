using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatLivegiftPurseDto
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 钱包名称
        /// </summary>
        public string Pursename { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unitname { get; set; }
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
        /// 钱包ID
        /// </summary>
        public int PurseId { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 冻结ID
        /// </summary>
        public int FreezeId { get; set; }
    }
}
