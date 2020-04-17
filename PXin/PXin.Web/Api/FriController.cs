using Common.Facade.Models;
using Common.UEPay;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace PXin.Web.Api
{
  /// <summary>
  /// 信友圈接口
  /// </summary>
  public class FriController : ApiController
  {
    /// <summary>
    /// V点充值
    /// </summary>
    /// <param name="chargeVDian"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChargeDto> ChargeVDian(ReqChargeVDian chargeVDian)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.ChargeVDian(chargeVDian);
      if (result)
      {
        return new Respbase<ChargeDto> { Result = 1, Message = "支付成功", Data = facade.ChargeUE };
      }
      return new Respbase<ChargeDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };

    }
    /// <summary>
    /// 支付V点(查看文章)
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<MessageDto> PayVDian(ReqPayVDian req)
    {
      FriFacade facade = new FriFacade();
      MessageDto result = facade.PayVDian(req);
      return new Respbase<MessageDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
    }
    /// <summary>
    /// V点历史-需要分页显示，每次加载20条
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<PxinPayhisDto>> GetVDianHis(ReqGetPVDianHis req)
    {
      FriFacade facade = new FriFacade();
      List<PxinPayhisDto> resultList = facade.GetVDianHis(req);
      return new Respbase<List<PxinPayhisDto>> { Data = resultList };
    }
    /// <summary>
    /// P点历史-需要分页显示，每次加载20条
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<PxinPraiseDto>> GetPDianHis(ReqGetPVDianHis req)
    {
      FriFacade facade = new FriFacade();
      List<PxinPraiseDto> resultList = facade.GetPDianHis(req);
      return new Respbase<List<PxinPraiseDto>> { Data = resultList };
    }

    /// <summary>
    /// 新版V点P点交易历史-需要分页显示，每次加载20条
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<IList<PxinAmountChangeHisDto>> GetPxinAmountChangeHis(PxinAmountChangeHisReq req)
    {
      FriFacade facade = new FriFacade();
      var resultList = facade.GetPxinAmountChangeHis(req);
      return new Respbase<IList<PxinAmountChangeHisDto>> { Data = resultList };
    }
    /// <summary>
    /// 发布信友圈信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase CreateMsg(ReqPxinMessage req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.CreateMsg(req);
      return facade.PromptInfo;
    }
    /// <summary>
    /// 删除信友圈信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase DeleteMsg(ReqDeleteMsg req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.DeleteMsg(req);
      return facade.PromptInfo;
    }
    /// <summary>
    /// 获取信友圈用户基本信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<UserinfoDto>> GetUserInfo(ReqUserInfo req)
    {
      FriFacade facade = new FriFacade();
      List<UserinfoDto> userinfo = facade.GetUserInfo(req);
      return new Respbase<List<UserinfoDto>> { Data = userinfo };
    }
    /// <summary>
    /// 获取信友圈-根据用户查询
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<FriMessageCollection> GetMsg(ReqGetMsg req)
    {
      FriFacade facade = new FriFacade();
      var result = facade.GetMsg(req);
      return new Respbase<FriMessageCollection> { Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result, Data = result };
    }
    /// <summary>
    /// 获取信友圈消息-首页
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<FriMessageCollection> GetMsgHome(ReqGetMsgHome req)
    {
      FriFacade facade = new FriFacade();
      var result = facade.GetMsgHome(req);
      return new Respbase<FriMessageCollection> { Data = result };
    }

    /// <summary>
    /// 是否有最新信友圈
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<int> IsNewMessage(Reqbase req)
    {
      FriFacade facade = new FriFacade();
      int infoid = facade.IsNewMessage(req);
      return new Respbase<int> { Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result, Data = infoid };
    }

    /// <summary>
    /// 发表评论
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<CreateDto> CreateComment(ReqCreateComment req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.CreateComment(req);
      return new Respbase<CreateDto> { Message = facade.PromptInfo.Message, Result = facade.PromptInfo.Result, Data = new CreateDto { KeyId = facade.CommentHisId } };
    }
    /// <summary>
    /// 点赞或踩
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase CreateAttitude(ReqCreateAttitude req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.CreateAttitude(req);
      return facade.PromptInfo;
    }
    /// <summary>
    /// 打赏
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase CreateReward(ReqCreateReward req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.CreateReward(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 修改背景图片
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase EditBackgImg(ReqUpdateBackgImg req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.UpdateBackgImg(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 举报信友圈
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Respbase CreateReport(ReqReport req)
    {
      FriFacade facade = new FriFacade();
      bool result = facade.CreateReport(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 获取举报投诉原因
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<ReportDto>> GetReasonList(Reqbase req)
    {
      List<ReportDto> resultList = new List<ReportDto>();
      resultList.Add(new ReportDto { ReasonId = 1, ReasonName = "垃圾广告" });
      resultList.Add(new ReportDto { ReasonId = 2, ReasonName = "黄、赌、毒信息" });
      resultList.Add(new ReportDto { ReasonId = 3, ReasonName = "抄袭我的文章" });
      resultList.Add(new ReportDto { ReasonId = 4, ReasonName = "不实信息" });
      return new Respbase<List<ReportDto>> { Data = resultList };
    }

    /// <summary>
    /// 获取举报投诉原因
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Respbase TransferTest(Reqbase req)
    {
      FriFacade facade = new FriFacade();
      facade.TransferTest();
      return facade.PromptInfo;
    }
  }
}
