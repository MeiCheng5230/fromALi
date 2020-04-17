using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using PXin.DB;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 充值商(fb)代理人(ap)接口
    /// </summary>
    public class FbApController : ApiController
    {
        private readonly FbApFacade _fbApFacade;
        /// <summary>
        /// ctor
        /// </summary>
        public FbApController()
        {
            this._fbApFacade = new FbApFacade();
        }

        /// <summary>
        /// 获取充值商配车情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<JxsPeiCheDto>> GetJxsPeiche(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var reginfo = HttpContext.Current.GetRegInfo();
            var query = db.VblJxsPeicheSet.Where(w => w.Nodeid == reginfo.Nodeid).Select(s => new JxsPeiCheDto()
            {
                Infoid = s.Infoid,
                PeicheStatus = s.PeicheStatus,
                PeicheStatusShow = s.PeicheStatusShow,
                ApprovalStatus = s.ApprovalStatus,
                ApprovalStatusShow = s.ApprovalStatusShow,
                FreezeStatus = s.FreezeStatus,
                FreezeStatusShow = s.FreezeStatusShow,
                PFM = s.PFM,
                SVC = s.SVC
            });
            return new Respbase<JxsPeiCheDto>() { Data = await query.FirstOrDefaultAsync() };
        }

        /// <summary>
        /// 获取(充值商/代理人)信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<UserJxsDto>> GetUserJxs(Reqbase req)
        {
            var result = await _fbApFacade.GetUserJxs(req);
            if (result == null)
            {
                return new Respbase<UserJxsDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<UserJxsDto>() { Data = result };
        }

        /// <summary>
        /// 修改(充值商/代理人)名称
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<UpdateUserJxsNameDto>> UpdateUserJxsName(UpdateUserJxsNameReq req)
        {
            var result = await _fbApFacade.UpdateUserJxsName(req);
            if (result == null)
            {
                return new Respbase<UpdateUserJxsNameDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<UpdateUserJxsNameDto>() { Data = result };
        }
        /// <summary>
        /// 获取库存记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<List<StockRecordDto>>> GetStockRecord(StockRecordReq req)
        {
            var result = await _fbApFacade.GetStockRecord(req);
            if (result == null)
            {
                return new Respbase<List<StockRecordDto>>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<List<StockRecordDto>>() { Data = result };
        }
        /// <summary>
        /// 获取90天累积进货列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<PurchaseWith90DaysDto>> Get90DaysPurchases(PurchaseWith90DaysReq req)
        {
            var result = await _fbApFacade.Get90DaysPurchases(req);
            if (result == null)
            {
                return new Respbase<PurchaseWith90DaysDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<PurchaseWith90DaysDto>() { Data = result };
        }
        /// <summary>
        /// 我的代理人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<List<MyUserJxsDto>>> GetMyUserJxs(MyUserJxsReq req)
        {
            var result = await _fbApFacade.GetMyUserJxs(req);
            if (result == null)
            {
                return new Respbase<List<MyUserJxsDto>>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<List<MyUserJxsDto>>() { Data = result };
        }
        /// <summary>
        /// 我的充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<MyUserCzsDto>> GetMyUserCzs(MyUserCzsReq req)
        {
            var result = await _fbApFacade.GetMyUserCzs(req);
            if (result == null)
            {
                return new Respbase<MyUserCzsDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<MyUserCzsDto>() { Data = result };
        }
        /// <summary>
        /// 获取审核状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<AuditStatusDto>> GetAuditStatus(AuditStatusReq req)
        {
            var result = await _fbApFacade.GetAuditStatus(req);
            if (result == null)
            {
                return new Respbase<AuditStatusDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<AuditStatusDto>() { Data = result };
        }
        /// <summary>
        /// 上传认证资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<UploadAuthDataDto>> UploadAuthData(UploadAuthDataReq req)
        {
            var result = await _fbApFacade.UploadAuthData(req);
            if (null == result)
            {
                return new Respbase<UploadAuthDataDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<UploadAuthDataDto>() { Data = result };
        }

        /// <summary>
        /// 审核代理人资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> AuditJxsInfo(AuditJxsInfoReq req)
        {
            var result = await _fbApFacade.AuditJxsInfo(req);
            if (!result)
            {
                return new Respbase<bool>() { Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }

        /// <summary>
        /// 获取认证资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<AuthDataDto>> GetAuthData(AuthDataReq req)
        {
            var result = await _fbApFacade.GetAuthData(req);
            if (result == null)
            {
                return new Respbase<AuthDataDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<AuthDataDto>() { Data = result };
        }
        /// <summary>
        /// 获取续费信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<RenewInfoDto>> GetRenewInfo(RenewInfoReq req)
        {
            var result = await _fbApFacade.GetRenewInfo(req);
            if (result == null)
            {
                return new Respbase<RenewInfoDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<RenewInfoDto>() { Data = result };
        }
        /// <summary>
        /// 续费
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<RenewDto>> Renew(RenewReq req)
        {
            var result = await _fbApFacade.Renew(req);
            if (result == null)
            {
                return new Respbase<RenewDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<RenewDto>() { Data = result };
        }
        /// <summary>
        /// 获取dos余额 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<decimal>> GetStockBalance(StockBalanceReq req)
        {
            var result = await _fbApFacade.GetStockBalance(req);
            return new Respbase<decimal>() { Data = result };
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<SearchUserDto>> SearchUser(SearchUserReq req)
        {
            var result = await _fbApFacade.SearchUser(req);
            if (result == null)
            {
                return new Respbase<SearchUserDto>() { Data = null, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<SearchUserDto>() { Data = result };
        }
        /// <summary>
        /// 新增代理人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<AddDealerDto>> AddUserJxs(AddDealerReq req)
        {
            var result = _fbApFacade.AddUserJxs(req);
            if (null == result)
            {
                return await Task.FromResult(new Respbase<AddDealerDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result });
            }
            return await Task.FromResult(new Respbase<AddDealerDto>() { Data = result });
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<FbApUserInfoDto>> GetUserInfo(UserInfoReq req)
        {
            var result = await _fbApFacade.GetUserInfo(req);
            if (null == result)
            {
                return new Respbase<FbApUserInfoDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<FbApUserInfoDto>() { Data = result };
        }
        /// <summary>
        /// 获取兑换类型信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<List<ExchangeTypeInfoDto>>> GetExchangeTypeInfo(ExchangeTypeInfoReq req)
        {
            var result = await _fbApFacade.GetExchangeTypeInfo(req);
            if (null == result)
            {
                return new Respbase<List<ExchangeTypeInfoDto>>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<List<ExchangeTypeInfoDto>>() { Data = result };
        }
        /// <summary>
        /// 兑换充值码(进货)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<ExChangeRechargeCodeDto>> ExChangeRechargeCode(ExChangeRechargeCodeReq req)
        {
            var result = await _fbApFacade.ExChangeRechargeCode(req);
            if (null == result)
            {
                return new Respbase<ExChangeRechargeCodeDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<ExChangeRechargeCodeDto>() { Data = result };
        }

        /// <summary>
        /// 获取充值商图标 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<FbapInitPageDto>> GetFbapInitPage(FbapInitPageReq req)
        {
            var result = await _fbApFacade.GetFbapInitPage(req);
            if (null == result)
            {
                return new Respbase<FbapInitPageDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<FbapInitPageDto>() { Data = result };
        }
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase VerifyPwd(VerifyPwdReq req)
        {
            var result = _fbApFacade.VerifyPwd(req);
            if (!result)
            {
                return new Respbase() { Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase();
        }

        /// <summary>
        /// 充值商申请
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<ApplyFbapDto>> ApplyFbap(ApplyFbapReq req)
        {
            var result = await _fbApFacade.ApplyFbap(req);
            if (null == result)
            {
                return new Respbase<ApplyFbapDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<ApplyFbapDto>() { Data = result };
        }


        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<List<MeetInfoDto>>> GetMeetInfos(MeetInfoReq req)
        {
            var result = await _fbApFacade.GetMeetInfos(req);
            if (null == result)
            {
                return new Respbase<List<MeetInfoDto>>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<List<MeetInfoDto>>() { Data = result };
        }
        /// <summary>
        /// 获取会议详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<MeetInfoDetailDto>> GetMeetInfoDetail(MeetInfoDetailReq req)
        {
            var result = await _fbApFacade.GetMeetInfoDetail(req);
            if (null == result)
            {
                return new Respbase<MeetInfoDetailDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<MeetInfoDetailDto>() { Data = result };
        }
        /// <summary>
        /// 参加会议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> JoinMeeting(JoinMeetingReq req)
        {
            var result = await _fbApFacade.JoinMeeting(req);
            if (!result)
            {
                return new Respbase<bool>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }


        /// <summary>
        /// 查询充值商信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<FbapInfoDto>> GetFbapInfo(FbapInfoReq req)
        {
            var result = await _fbApFacade.GetFbapInfo(req);
            if (null == result)
            {
                return new Respbase<FbapInfoDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<FbapInfoDto>() { Data = result };
        }
        /// <summary>
        /// 更换充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> ChangeFbap(ChangeFbapReq req)
        {
            var result = await _fbApFacade.ChangeFbap(req);
            if (!result)
            {
                return new Respbase<bool>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }
        /// <summary>
        /// 开通充值商检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> CheckOpenCzs(CheckOpenCzsReq req)
        {
            var result = await _fbApFacade.CheckOpenCzs(req);
            if (!result)
            {
                return new Respbase<bool>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }
        /// <summary>
        /// 开通充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<OpenCzsDto>> OpenCzs(OpenCzsReq req)
        {
            var result = await _fbApFacade.OpenCzs(req);
            if (null == result)
            {
                return new Respbase<OpenCzsDto>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<OpenCzsDto>() { Data = result };
        }

        /// <summary>
        /// 查询充值商添加代理人请求列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<List<UserJxsConfirmsDto>>> GetUserJxsConfirms(UserJxsConfirmsReq req)
        {
            var result = await _fbApFacade.GetUserJxsConfirms(req);
            if (null == result)
            {
                return new Respbase<List<UserJxsConfirmsDto>>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<List<UserJxsConfirmsDto>>() { Data = result };
        }
        /// <summary>
        /// 同意充值商添加代理人请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<Respbase<bool>> AgreeUserJxsRequst(UserJxsRequstReq req)
        {
            var result = await _fbApFacade.AgreeUserJxsRequst(req);
            if (!result)
            {
                return new Respbase<bool>() { Data = result, Message = _fbApFacade.PromptInfo.Message, Result = _fbApFacade.PromptInfo.Result };
            }
            return new Respbase<bool>() { Data = result };
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase AddFriendNoticeEvent(Reqbase req)
        {
            Facade.Bus.EventBus.GetInstance().Publish(new Facade.Bus.Event.AddFriendNoticeEvent("hello bus"));

            return new Respbase() { };
        }


    }
}
