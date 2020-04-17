using Common.Facade.Models;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 达人专区
    /// </summary>
    public class DaRenController : ApiController
    {
        /// <summary>
        /// 获取达人首页信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DarenHomeInfoDto> GetDarenHomeInfo(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDarenHomeInfo(req);
            if (result!=null)
            {
                return new Respbase<DarenHomeInfoDto> { Result = 1, Message = "成功",Data= result };
            }
            return new Respbase<DarenHomeInfoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message};
        }

        /// <summary>
        /// 获取默认推荐的达人列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<DaRenInfoDto>> GetDefaultDaRens(GetDefaultDaRenReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDefaultDaRens(req);
            if (result != null)
            {
                return new Respbase<List<DaRenInfoDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<DaRenInfoDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取专业分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<ClassificationDto>> GetClassifications(GetClassifications req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetClassifications(req);
            if (result != null)
            {
                return new Respbase<List<ClassificationDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<ClassificationDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取专业分类一级id对应二级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<Classification>> GetClassificas(GetClassificasReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetClassificas(req);
            if (result != null)
            {
                return new Respbase<List<Classification>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<Classification>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取热门关键字
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<HotKeyWordDto>> GetHotKeywords(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetHotKeywords(req);
            if (result != null)
            {
                return new Respbase<List<HotKeyWordDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<HotKeyWordDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取教育经历列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<DaRenEduDto>> GetDaRenEdus(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenEdus(req);
            if (result != null)
            {
                return new Respbase<List<DaRenEduDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<DaRenEduDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取职业经历列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<DaRenOccupationDto>> GetDaRenOccupations(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenOccupations(req);
            if (result != null)
            {
                return new Respbase<List<DaRenOccupationDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<DaRenOccupationDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 删除职业/教育经历
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteDaRenOccOrEdu(DeleteDaRenExtReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.DeleteDaRenExt2(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功"};
            }
            return new Respbase{ Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 搜索达人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<DaRenInfoDto>> SearchDaRen(SearchWiseManReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.SearchDaRen(req);
            if (result != null)
            {
                return new Respbase<List<DaRenInfoDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<DaRenInfoDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加专业分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateClassifications(CreateDaRenExt1Req req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.CreateDaRenExt1(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功"};
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取达人个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DaRenInfoSelfDto> GetDaRenInfoSelf(GetDaRenInfoSelfNew req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenInfoSelf(req);
            if (result!=null)
            {
                return new Respbase<DaRenInfoSelfDto> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<DaRenInfoSelfDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取申请填写数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DaRenAbovementionedDataDto> GetAbovementionedData(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetAbovementionedData(req);
            if (result != null)
            {
                return new Respbase<DaRenAbovementionedDataDto> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<DaRenAbovementionedDataDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加或修改自我介绍
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateSelfIntroduction(UpdateSelfIntroductionReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateSelfIntroduction(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase{ Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加或修改达人达语
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateGreetings(UpdateGreetingsReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateGreetings(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 修改欢迎语
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateWelcome(UpdateWelcomeReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateWelcome(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加教育领域数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateDaRenEdu(CreateDaRenEduReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.CreateDaRenEdu(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 修改教育领域数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateDaRenEdu(UpdateDaRenEdu req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateDaRenEdu(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加职业经历数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateDaRenOccupations(CreateDaRenOccupations req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.CreateDaRenOccupations(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 修改职业领域数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateDaRenOccupations(UpdateDaRenOccupations req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateDaRenOccupations(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 聊天界面获取达人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<ChatDarenInfoDto> GetChatDarenInfo(GetDaRenInfoSelf req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetChatDarenInfo(req);
            if (result != null)
            {
                return new Respbase<ChatDarenInfoDto> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<ChatDarenInfoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 添加或更新专业资格认证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateSpecializedPics(UpdateSpecializedPics req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.UpdateSpecializedPics(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功"};
            }
            return new Respbase{ Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 达人申请提交审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase VerifyDaRen(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.VerifyDaRen(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 我的视频上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<CreateVideoDto> CreateVideo(UpdateVideo req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.CreateVideo(req);
            if (result!=null)
            {
                return new Respbase<CreateVideoDto> { Result = 1, Message = "成功" ,Data= result };
            }
            return new Respbase<CreateVideoDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取我的视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<VideoBase>> GetMyVideo(Reqbase req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetMyVideo(req);
            if (result != null)
            {
                return new Respbase<List<VideoBase>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<VideoBase>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }
        

        /// <summary>
        /// 删除我的视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteVideo(DeleteVideoReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.DeleteVideo(req);
            if (result)
            {
                return new Respbase{ Result = 1, Message = "成功"};
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteFile(UpdateSpecializedPics req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.DeleteFile(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 新增/修改知识库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateOrUpdateDaRenKnowledge(CreateKnowledgeReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.CreateOrUpdateDaRenKnowledge(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 删除知识库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteDaRenKnowledge(DaRenKnowledgeReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.DeleteDaRenKnowledge(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 点赞或者浏览达人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase BrowseOrPraiseSomeOne(BrowseSomeOneReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.BrowseOrPraiseSomeOne(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 点赞或者浏览视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase BrowseOrPraiseVideo(BrowseVideoReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.BrowseOrPraiseVideo(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = "成功" };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取知识库详细数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DaRenKnowledgeDto> GetDaRenKnowledgeDetail(DaRenKnowledgeReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenKnowledge(req);
            if (result!=null)
            {
                return new Respbase<DaRenKnowledgeDto> { Result = 1, Message = "成功" ,Data= result };
            }
            return new Respbase<DaRenKnowledgeDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 获取我的知识库列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<KnowledgeLists> GetMyKnowledges(GetDaRenKnowledgesReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenKnowledges(req);
            if (result != null)
            {
                return new Respbase<KnowledgeLists> { Result = 1, Message = "成功" ,Data= result };
            }
            return new Respbase<KnowledgeLists> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 查看某人的知识库列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<KnowledgeDto>> GetDaRenKnowledgesByOne(GetDaRenKnowledgesByOneReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.GetDaRenKnowledgesByOne(req);
            if (result != null)
            {
                return new Respbase<List<KnowledgeDto>> { Result = 1, Message = "成功", Data = result };
            }
            return new Respbase<List<KnowledgeDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 支付查看达人知识库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SeeAndBuyDaRenKnowledge(SeeDaRenKnowledgeReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.SeeAndBuyDaRenKnowledge(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }

        /// <summary>
        /// 设置倍率保护
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase SetProtectRate(SetProtectRateReq req)
        {
            DaRenFacade facade = new DaRenFacade();
            var result = facade.SetProtectRate(req);
            if (result)
            {
                return new Respbase { Result = 1, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
        }
        

    }
}