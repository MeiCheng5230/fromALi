using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// A点竞拍
    /// </summary>
    public class AuctionADto
    {
    }
    /// <summary>
    /// 本月竞拍数据
    /// </summary>
    public class ThisMonthDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public ThisMonthDataDto()
        {
            AuctionHis = new List<AuctionHisDto>();
        }
        /// <summary>
        /// 本月竞拍A点数
        /// </summary>
        public int ANum { get; set; }
        /// <summary>
        /// 加价幅度
        /// </summary>
        public decimal AddPrice { get; set; }
        /// <summary>
        /// 本月领先的A点数
        /// </summary>
        public int MyLeading { get; set; }
        /// <summary>
        /// 本月出局的A点数
        /// </summary>
        public int MyOut { get; set; }
        /// <summary>
        /// 我的A点 
        /// </summary>
        public int MyA { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// 是否同意协议 0=否 1=是
        /// </summary>
        public int IsAgreement { get; set; }
        /// <summary>
        /// 竞拍历史
        /// </summary>
        public List<AuctionHisDto> AuctionHis { get; set; }
    }
    /// <summary>
    /// 竞拍历史
    /// </summary>
    public class AuctionHisDto
    {
        ///// <summary>
        ///// 排序字段
        ///// </summary>
        //public int R { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 排名开始
        /// </summary>
        public int Befornum { get; set; }
        /// <summary>
        /// 排名结束
        /// </summary>
        public int Afternum { get; set; }
    }
    /// <summary>
    /// 我的竞拍历史
    /// </summary>
    public class MyAuctionHisDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 竞拍单价（p点）
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 竞拍数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 状态 -1=出局(P点已退) 0=领先 1=已完成 -2=出局
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 我的A点
    /// </summary>
    public class MyAuctionADto
    {
        /// <summary>
        /// 状态  0=未结算 1=已结算
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 有效期-开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 有效期-结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// SV余额
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// SVC充值码
        /// </summary>
        public decimal SVC { get; set; }
        /// <summary>
        /// 专户DOS
        /// </summary>
        public decimal DOS { get; set; }
    }
    /// <summary>
    /// 竞拍配置
    /// </summary>
    public class MyTpxinPaiConfig
    {
        /// <summary>
        ///  低价
        ///</summary>
        public decimal Minprice { get; set; }
        /// <summary>
        ///  加价幅度
        ///</summary>
        public decimal Addprice { get; set; }
        /// <summary>
        /// 我的p点
        /// </summary>
        public decimal MyP { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }
    }
    /// <summary>
    /// 竞拍详情
    /// </summary>
    public class AuctionDetailsDto
    {
        /// <summary>
        /// 竞拍价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 竞拍数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 竞拍日期
        /// </summary>
        public string CreateDate { get; set; }
    }
    /// <summary>
    /// 竞拍加价页面数据
    /// </summary>
    public class AuctionAddpriceDto
    {
        /// <summary>
        /// 竞拍历史
        /// </summary>
        public List<MyAuctionHisDto> myAuctionHis { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }
    }
}
