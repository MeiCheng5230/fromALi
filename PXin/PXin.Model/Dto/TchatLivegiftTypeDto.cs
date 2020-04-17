using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatLivegiftTypeDto
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 礼物名称
        /// </summary>
        public string Giftname { get; set; }
        /// <summary>
        /// 礼物图片
        /// </summary>
        public string Giftpic { get; set; }
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
        /// 礼物单价，单位：元
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public int PurseId { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
