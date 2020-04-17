using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 直播礼物表
    /// </summary>
    public partial class TchatLivegiftType
    {
        public TchatLivegiftType()
        {
        }
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
        /// 拥有者类型,tchat_livegift_purse.id
        /// </summary>
        public int Giftpurseid { get; set; }
        /// <summary>
        /// 礼物单价，单位：元
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 礼物分类,0-普通，1-系统，不显示在用户礼物列表中
        /// </summary>
        public int Giftcategory { get; set; }
    }
}
