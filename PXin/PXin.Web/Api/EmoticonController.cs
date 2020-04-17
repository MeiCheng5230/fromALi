using Common.Facade.Models;
using Newtonsoft.Json;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PXin.Web.Api
{
    public class EmoticonController : ApiController
    {
        /// <summary>
        /// 获取热门表情包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<string>> GetHotEmoticons(HotEmoticonsReq req)
        {

            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.GetHotEmoticons(req);
            if (result != null)
            {
                return new Respbase<List<string>> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<List<string>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }

        /// <summary>
        /// 获取表情单品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<EmoticonsDto>> GetSingleEmoticons(PageBase req)
        {

            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.GetSingleEmoticons(req);
            if (result != null)
            {
                return new Respbase<List<EmoticonsDto>> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<List<EmoticonsDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }

        /// <summary>
        /// 获取表情包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<EmoticonsDto>> GetEmoticons(PageBase req)
        {
            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.GetEmoticons(req);
            if (result != null)
            {
                return new Respbase<List<EmoticonsDto>> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<List<EmoticonsDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }

        /// <summary>
        /// 搜索表情包
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<EmoticonsDto>> SearchEmoticons(SearchEmoticonsReq req)
        {

            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.SearchEmoticons(req);
            if (result != null)
            {
                return new Respbase<List<EmoticonsDto>> { Result = 1, Message = "获取成功", Data = result };
            }
            return new Respbase<List<EmoticonsDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

        }

        /// <summary>
        /// 购买表情 result=1 表示支付成功，=2表示通过下载 =99表示需要调起ue
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<OpenInfUeoDto> BuyEmoticon(DownloadEmoticonReq req)
        {

            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.BuyEmoticon(req);
            if (result.Item1)
            {
                return new Respbase<OpenInfUeoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message ,Data= result .Item2};
            }
            return new Respbase<OpenInfUeoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };

        }

        /// <summary>
        /// 获取表情包详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<EmoticonDetailDto>> GetEmoticonMaterialDetail(EmoticonMaterialDetailReq req)
        {
            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.GetEmoticonMaterialDetail(req);
            if (result!=null)
            {
                return new Respbase<List<EmoticonDetailDto>> { Result = 1, Message = "成功" ,Data= result };
            }
            return new Respbase<List<EmoticonDetailDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };

        }

        /// <summary>
        /// 查询ue支付是否成功
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase VerifyPayResult(VerifyPayReq req)
        {
            EmoticonFacade facade = new EmoticonFacade();
            var result = facade.VerifyPayResult(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "支付成功"};
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };

        }

        /// <summary>
        /// 生成描述文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateShowNameFile(CreateShowReq req)
        {
            EmoticonFacade facade = new EmoticonFacade();
            facade.CreateShowNameFile(req);
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };

        }
        



    }
}
