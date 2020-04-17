using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class CardConfigDto
    {
        /// <summary>
        ///  主键id
        ///</summary>
        public int Configid { get; set; }
        /// <summary>
        ///  对外显示名
        ///</summary>
        public string Showname { get; set; }
        /// <summary>
        ///  价格元
        ///</summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigList
    {
        /// <summary>
        /// 
        /// </summary>
        public ConfigList()
        {
            List = new List<CardConfigDto>();
        }
        /// <summary>
        /// 额度配置列表
        /// </summary>
        public List<CardConfigDto> List { get; set; }
        /// <summary>
        /// 类别名
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 当前类别库存余额
        /// </summary>
        public decimal Stocknum { get; set; }
    }

    /// <summary>
    /// 微信购买返回
    /// </summary>
    public class BuyDto
    {
        /// <summary>
        /// 字符串
        /// </summary>
        public Pingpp.Models.Charge ChargeStr { get; set; }
        /// <summary>
        /// 测试
        /// </summary>
        public string Guid { get; set; }
    }

    /// <summary>
    /// svc码统计
    /// </summary>
    public class SvcStatisDto
    {
        /// <summary>
        ///  价格元
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  数量
        ///</summary>
        public int Count { get; set; }
        /// <summary>
        /// 是否有零售权限 0=没有 1=有
        /// </summary>
        public int Issale { get; set; }
        /// <summary>
        /// 是否同意协议 0=否 1=是
        /// </summary>
        public int IsAgreement { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MySvcDto
    {
        /// <summary>
        ///  卡号
        ///</summary>
        public string Cardno { get; set; }
        /// <summary>
        ///  价格元
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  卡类别id 1=SVC 2=DOS
        ///</summary>
        public string Areaid { get; set; }
        

    }

    /// <summary>
    /// 
    /// </summary>
    public class MySvchisDto
    {
        /// <summary>
        ///  价格元
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///  显示
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  类型 0=微信等购买 1=零售 2=红包 3=专户DOS兑换 4=第三方购买 5=使用 6=转让
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  卡号
        ///</summary>
        public string Cardno { get; set; }
        /// <summary>
        ///  货币类型
        ///</summary>
        public string AmountType { get; set; }
        /// <summary>
        ///  账号
        ///</summary>
        public string NodeCode { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SVUserInfoDto
    {
        /// <summary>
        ///  用户账号
        ///</summary>
        public string NodeCode { get; set; }
        /// <summary>
        ///  用户姓名
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  用户手机号
        ///</summary>
        public string Phone { get; set; }
        /// <summary>
        ///  是否是充值商 0=不是 1=是
        ///</summary>
        public int IsZYS { get; set; }
        /// <summary>
        ///  是否是自己 0=不是 1=是
        ///</summary>
        public int IsSelf { get; set; }

    }
}
