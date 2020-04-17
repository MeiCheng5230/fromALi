using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 兑换
    /// </summary>
    public class ExchangeDto
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class UePayCallDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Charge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string orderno { get; set; }
    }

    /// <summary>
    /// 用户信息及dos
    /// </summary>
    public class DosInfoDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 用户头像url
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// dos 余额
        /// </summary>
        public decimal Dos { get; set; }
        /// <summary>
        /// 账号 
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 是否开通专属账号 1：开通 0：未开通
        /// </summary>
        public int IsOpenInfo { get; set; }
    }
    /// <summary>
    /// 用户信息(优谷vip码和p客认证)
    /// </summary>
    public class ChargeUserInfoDto
    {
        /// <summary>
        /// 获取ue用户信息时此id是传进来的id(获取ue时result=-5表示没有绑定ue账号)
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string NodeCode { get; set; }

        /// <summary>
        /// ue余额（只有获取ue用户信息才有值，其他为0）
        /// </summary>
        public decimal UeBalance { get; set; }
    }
    /// <summary>
    /// ue用户信息
    /// </summary>
    public class UeUserInfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string NodeCode { get; set; }
    }
    /// <summary>
    /// 兑换列表
    /// </summary>
    public class ChargeProductDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///  名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  图片
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  价格
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  价格单位，显示用
        ///</summary>
        public string Priceunit { get; set; }
        /// <summary>
        ///  产品价值
        ///</summary>
        public decimal Pdtvalue { get; set; }
        /// <summary>
        /// 商品说明
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 兑换类型  1=兑换SV 2=兑换SVC 3=兑换YG的会员码 4=兑换PCN的认证码
        /// </summary>
        public int TypeId { get; set; }
    }
    /// <summary>
    /// 开通专属账号
    /// </summary>
    public class OpenInfUeoDto
    {
        /// <summary>
        /// 续费信息字符串
        /// </summary>
        public string ChargeStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderNo { get; set; }

    }
    /// <summary>
    /// 兑换历史
    /// </summary>
    public class RechargeHisDto
    {
        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  商品名称
        ///</summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 备注信息 列如：账号：1500000
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  类型 1=兑换SV 2=兑换SVC 3=兑换YG的会员码 4=兑换PCN的认证码
        ///</summary>
        public int Typeid { get; set; }
    }
    /// <summary>
    /// 获取已发布的svc充值码Dto
    /// </summary>
    public class ReleaseSvcCodeDto
    {
        /// <summary>
        /// 使用者nodeid
        /// </summary>
        public int UseNodeId { get; set; }
    }
    /// <summary>
    /// 根据面额统计获取svc充值码Dto
    /// </summary>
    public class SvcByGroupbyAmountDto
    {
        /// <summary>
        /// 面额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
    }
}
