using Common.Facade.Models;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// A点竞拍
    /// </summary>
    public class AuctionAController : ApiController
    {
        private AuctionAFacade facade = new AuctionAFacade();
        /// <summary>
        /// 本月竞拍数据（我的竞拍总数和竞拍记录）
        /// </summary>
        /// <param name="req"></param>
        [HttpPost]
        public Respbase<ThisMonthDataDto> GetThisMonthData(Reqbase req)
        {
            var data= facade.GetThisMonthData(req);
            if (data == null)
            {
                return new Respbase<ThisMonthDataDto>() { Result = 0, Message = facade.PromptInfo.Message, Data = null };
            }
            return new Respbase<ThisMonthDataDto>() { Result = 1, Message = "成功", Data = data };
        }
        /// <summary>
        /// 竞拍排名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<AuctionHisDto>> GetAuctionRanking(Reqbase req)
        {
            var data = facade.GetAuctionRanking();
            return new Respbase<List<AuctionHisDto>> { Result = 1, Data = data };
        }
        /// <summary>
        /// 推送竞拍排名
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Respbase PushAuctionRanking(ReqCacheAuctionRanking req)
        {
            facade.CacheAuctionRanking(req.AuctionHisDtos);
            return new Respbase();
        }
        /// <summary>
        /// 竞拍支付
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase PayAuction(ReqPayAuction req)
        {
            var result= facade.PayAuction(req);
            if (!result)
            {
                return new Respbase { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = 1, Message = "支付成功" };
        }
        /// <summary>
        /// 我的竞拍历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<MyAuctionHisDto>> GetMyAuctionHis(ReqMyAuctionHis req)
        {
            var data= facade.GetMyAuctionHis(req);
            return new Respbase<List<MyAuctionHisDto>> { Result = 1, Message = "成功", Data = data };
        }
        /// <summary>
        /// 我的A点
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<MyAuctionADto>> GetMyAuctionA(Reqbase req)
        {
            var data = facade.GetMyAuctionA(req);
            return new Respbase<List<MyAuctionADto>> { Result = 1, Message = "成功", Data = data };
        }
        /// <summary>
        /// 竞拍配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<MyTpxinPaiConfig> GetAuctionConfig(Reqbase req)
        {
            var data = facade.GetAuctionConfig(req.Nodeid);
            if (data == null)
            {
               return new Respbase<MyTpxinPaiConfig> { Result = 0, Message = "本月没有竞拍" };
            }
            return new Respbase<MyTpxinPaiConfig> { Result = 1, Data = data };
        }
        /// <summary>
        /// 竞拍详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<AuctionDetailsDto>> GetAuctionDetails(ReqAuctionDetails req)
        {
            var data = facade.GetAuctionDetails(req);
            if (data == null)
            {
                return new Respbase<List<AuctionDetailsDto>> { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase<List<AuctionDetailsDto>> { Result = 1, Data = data };
        }
        /// <summary>
        /// 竞拍加价页面数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<AuctionAddpriceDto> GetAuctionAddprice(Reqbase req)
        {
            var data = facade.GetAuctionAddprice(req);
            if (data == null)
            {
                return new Respbase<AuctionAddpriceDto> { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase<AuctionAddpriceDto> { Result = 1, Data = data };
        }
        /// <summary>
        /// 竞拍加价支付
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase AuctionAddpricePay(ReqPayAuctionAddPrice req)
        {
            var result = facade.AuctionAddpricePay(req);
            if (!result)
            {
                return new Respbase { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = 1, Message = "支付成功" };
        }
  }
}
