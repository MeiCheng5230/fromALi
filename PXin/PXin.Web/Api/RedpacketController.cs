using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Facade.Models;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;

namespace PXin.Web.Api
{
    /// <summary>
    /// 领取红包接口
    /// </summary>
    public class RedpacketController : ApiController
    {
        private readonly RedpacketFacade _redpacketFacade;
        /// <summary>
        /// ctor
        /// </summary>
        public RedpacketController()
        {
            this._redpacketFacade = new RedpacketFacade();
        }
        /// <summary>
        /// 获取领取红包页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<RedPacketInfoDto>> GetRedPacketInfo(RedPacketInfoReq req)
        {
            var result = await _redpacketFacade.GetRedPacketInfo(req);
            if (result == null)
            {
                return new Respbase<RedPacketInfoDto>() { Data = null, Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<RedPacketInfoDto>() { Data = result };
        }
        /// <summary>
        /// 领取红包
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<ReceiveRedPacketDto>> ReceiveRedPacket(ReceiveRedPacketReq req)
        {
            var result = await _redpacketFacade.ReceiveRedPacket(req);
            if (result == null)
            {
                return new Respbase<ReceiveRedPacketDto>() { Data = null, Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<ReceiveRedPacketDto>() { Data = result };
        }
        /// <summary>
        /// 获取我的红包奖励
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<MyRedPacketDto>> GetMyRedPacket(MyRedPacketReq req)
        {
            var result = await _redpacketFacade.GetMyRedPacket(req);
            if (result == null)
            {
                return new Respbase<MyRedPacketDto>() { Data = null, Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<MyRedPacketDto>() { Data = result };
        }
        /// <summary>
        /// 红包奖励领取详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<MyRedPacketDetailDto>> GetMyRedPacketDetail(MyRedPacketDetailReq req)
        {
            var result = await _redpacketFacade.GetMyRedPacketDetail(req);
            if (result == null)
            {
                return new Respbase<MyRedPacketDetailDto>() { Data = null, Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<MyRedPacketDetailDto>() { Data = result };
        }
        /// <summary>
        /// 获取兑换页面信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<ExchangeInfoDto>> GetExchangeInfo(ExchangeInfoReq req)
        {
            var result = await _redpacketFacade.GetExchangeInfo(req);
            if (result == null)
            {
                return new Respbase<ExchangeInfoDto>() { Data = null, Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<ExchangeInfoDto>() { Data = result };
        }

        /// <summary>
        /// 兑换
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> Exchange(ExchangeReq req)
        {
            var result = await _redpacketFacade.Exchange(req);
            if (!result)
            {
                return new Respbase<bool>() { Message = _redpacketFacade.PromptInfo.Message, Result = _redpacketFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }

        /// <summary>
        /// 获取有户参与A点竟拍抽奖信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<LuckDrawInfo>> GetLuckDrawInfo(Reqbase req)
        {
            return await _redpacketFacade.GetLuckDrawInfo(req);
        }
        /// <summary>
        /// A点竟拍抽奖信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<LuckDrawInfo> LuckDraw(Reqbase req)
        {
            return _redpacketFacade.LuckDraw(req);
        }
        /// <summary>
        /// 获取A点竟拍抽奖历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<LuckDrawHis>> GetLuckDrawHis(Reqbase req)
        {
            var dto = _redpacketFacade.GetLuckDrawHis(req);
            return new Respbase<List<LuckDrawHis>> { Data = dto };
        }
    }
}
