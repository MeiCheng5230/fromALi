using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 钱包数据
    /// </summary>
    public class PurseDto
    {
        /// <summary>
        /// 钱包类型的ID(支付时用到)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public int Purseid { get; set; }

        /// <summary>
        /// 钱包名
        /// </summary>
        public string Pursename { get; set; }

        /// <summary>
        /// 钱包Logo
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal Freeze { get; set; }

        /// <summary>
        /// 钱包单位
        /// </summary>
        public int Purseunit { get; set; }

        /// <summary>
        /// 钱包单位名
        /// </summary>
        public string Purseunitname { get; set; }

        /// <summary>
        /// 对外显示的文字描述
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// 账单明细Url
        /// </summary>
        public string DetailUrl { get; set; }
        /// <summary>
        /// 钱包类型
        /// </summary>
        public int PurseType { get; set; }
        /// <summary>
        /// 子钱包id
        /// </summary>
        public int Subid { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string BgPic { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Purse2Dto
    {
        /// <summary>
        /// 链接地址(码库时此项为pursetype)
        /// </summary>
        public string ClickUrl { get; set; }
        /// <summary>
        /// 钱包名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 钱包余额
        /// </summary>
        public string Balance { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Purse3Dto
    {
        /// <summary>
        /// 链接地址(码库时此项为pursetype)
        /// </summary>
        public string ClickUrl { get; set; }
        /// <summary>
        /// 钱包名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 钱包类型id
        /// </summary>
        public int PurseType { get; set; }
        /// <summary>
        /// 钱包子id
        /// </summary>
        public int Subid { get; set; }
        /// <summary>
        /// 钱包余额
        /// </summary>
        public string Balance { get; set; }
    }

    /// <summary>
    /// 扣费
    /// </summary>
    public class RecoveryDto
    {
        /// <summary>
        /// 转账ID
        /// </summary>
        public string TransferId { get; set; }
    }

    /// <summary>
    /// 转账原因图标
    /// </summary>
    public class PurseHisTypeLogoDto
    {
        /// <summary>
        /// 转账原因ID
        /// </summary>
        public int TypeID { get; set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string IconUrl
        {
            get
            {
                return "http://global.xiang-xin.net/images/pursehistype/" + TypeID + ".png";
            }
        }
    }

    /// <summary>
    /// UV充值记录Dto
    /// </summary>
    public class UVChargeHisDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Transid { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public string BalanceAfter
        {
            set
            {
            }
            get
            {
                return BalanceAfter1.ToString("0.00");
            }

        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string ShowName { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        [JsonIgnore]
        public decimal BalanceAfter1 { get; set; }
    }

}
