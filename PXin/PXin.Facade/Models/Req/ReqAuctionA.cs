using Common.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// A点竞拍
    /// </summary>
    public class ReqAuctionA
    {
    }
    /// <summary>
    /// 竞拍支付
    /// </summary>
    public class ReqPayAuction:Reqbase
    {
        /// <summary>
        /// 竞拍A点数量
        /// </summary>
        [Required]
        [Range(1, 999999999)]
        public int Num { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string PayPwd { get; set; }
        /// <summary>
        /// 用户竞拍底单价(底价+加价幅度)
        /// </summary>
        [Required]
        [Range(0.01,9999999999)]
        public decimal MinPrice { get; set; }
    }
    /// <summary>
    /// 竞拍加价支付
    /// </summary>
    public class ReqPayAuctionAddPrice : Reqbase
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        [Required]
        public string PayPwd { get; set; }
        /// <summary>
        /// 加价幅度
        /// </summary>
        [Required]
        [Range(0.01, 9999999999)]
        public decimal Price { get; set; }
        /// <summary>
        /// 竞拍id
        /// </summary>
        [Required]
        public List<int> Auctionid { get; set; }

    }
    /// <summary>
    /// 我的竞拍历史
    /// </summary>
    public class ReqMyAuctionHis: GetByPageBase
    {
        /// <summary>
        /// 日期（2019-7）
        /// </summary>
        [Required]
        public string QueryDate { get; set; }

    }
    /// <summary>
    /// 竞拍详情
    /// </summary>
    public class ReqAuctionDetails : Reqbase
    {
        /// <summary>
        ///时间类型（1-近7天 2-七天之前 3-本月）
        /// </summary>
        [Required]
        public int QueryTimeType { get; set; }
    }
    /// <summary>
    /// 竞拍排名
    /// </summary>
    public class ReqAuctionRanking : Reqbase
    {
        /// <summary>
        /// 当月竞拍总数
        /// </summary>
        [Required]
        public int Num { get; set; }
    }
    /// <summary>
    /// 推送竞拍历史请求参数
    /// </summary>
    public class ReqCacheAuctionRanking : Reqbase
    {
        /// <summary>
        /// 竞拍历史
        /// </summary>
        public List<AuctionHisDto> AuctionHisDtos { get; set; }
    }
}
