using Common.Mvc;
using io.rong;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Linq;
using MvcPaging;
using Common.Facade.Models;

namespace PXin.Web.Api
{
  /// <summary>
  /// 
  /// </summary>
  public class ChatController : ApiController
  {
    /// <summary>
    /// P信账号登录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChatUserDto> Login(LoginReq req)
    {
      ChatFacade facade = new ChatFacade();
      ChatUserDto user = facade.PXinLogin(req.Nodeid, req.Gtclientid, req.Devicetoken);
      return new Respbase<ChatUserDto> { Data = user };
    }

    /// <summary>
    /// 根据userid获取用户信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Respbase<List<ChatUserDto>> QueryUserInfo(QueryUserInfoReq req)
    {
      ChatFacade facade = new ChatFacade();
      List<ChatUserDto> dtos = facade.QueryUserInfo(req);
      return new Respbase<List<ChatUserDto>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = dtos };
    }

    /// <summary>
    /// 刷新用户信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase RefreshUserInfo(Reqbase req)
    {
      Log log = new Log(typeof(ChatController));
      log.Info("刷新用户信息,nodeid=" + req.Nodeid);
      ChatFacade facade = new ChatFacade();
      bool result = facade.RefreshUserInfo(req);
      return facade.PromptInfo;

    }

    /// <summary>
    /// 修改我的基本信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase UpdateMyInfo(UpdateMyInfoReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.UpdateMyInfo(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 修改我的昵称地址[停止使用]
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase UpdateMyNick(UpdateMyNickReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.UpdateMyNick(req);
      return facade.PromptInfo;
    }


    /// <summary>
    /// 我的好友
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<ChatUserDto>> MyFriend(Reqbase req)
    {
      ChatFacade facade = new ChatFacade();
      List<ChatUserDto> dtos = facade.MyFriend(req);
      return new Respbase<List<ChatUserDto>> { Data = dtos };
    }

    /// <summary>
    /// 查找好友
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryFriendDtos> QueryFriend(QueryFriendReq req)
    {
      ChatFacade facade = new ChatFacade();
      IPagedList<ChatUserDto> dtos = facade.QueryFriend(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryFriendDtos> { Message = "没有找到符合搜索条件的用户", Result = -1, Data = null };
      }
      return new Respbase<QueryFriendDtos> { Data = new QueryFriendDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }


    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChatUserDto> AddFriend(AddFriendReq req)
    {
      ChatFacade facade = new ChatFacade();
      ChatUserDto result = facade.AddFriend(req);
      if (result == null)
      {
        return new Respbase<ChatUserDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
      }
      if (facade.PromptInfo.Result == 2)
      {
        return new Respbase<ChatUserDto> { Result = 1, Message = facade.PromptInfo.Message, Data = result };
      }
      return new Respbase<ChatUserDto> { Result = 1, Message = "添加好友请求发起成功，请等待对方验证通过", Data = result };
    }


    /// <summary>
    /// 添加好友确认
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChatUserDto> AddFriendConfirm(AddFriendConfirmReq req)
    {
      ChatFacade facade = new ChatFacade();
      ChatUserDto result = facade.AddFriendConfirm(req);
      if (result == null)
      {
        return new Respbase<ChatUserDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
      }
      return new Respbase<ChatUserDto> { Data = result };
    }

    /// <summary>
    /// 修改好友信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase UpdateMyFriendInfo(UpdateMyFriendInfoReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.UpdateMyFriendInfo(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 修改好友备注[停用]
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase UpdateMyFriendRemarks(UpdateMyFriendRemarksReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.UpdateMyFriendRemarks(req);
      return facade.PromptInfo;
    }


    /// <summary>
    /// 删除好友
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase DeleteFriend(DeleteFriendReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.DeleteFriend(req);
      return facade.PromptInfo;
    }

    //----------------------------分割分割-------------------------------------------------------------------------------
    /// <summary>
    /// 创建群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChatGroupDto> CreateGroup(CreateGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      ChatGroupDto group = facade.CreateGroup(req);
      if (group == null)
      {
        return new Respbase<ChatGroupDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
      }
      return new Respbase<ChatGroupDto> { Data = group };
    }

    /// <summary>
    /// 修改群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<ChatGroupDto> UpdateGroup(UpdateGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      ChatGroupDto group = facade.UpdateGroup(req);
      if (group == null)
      {
        return new Respbase<ChatGroupDto> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
      }
      return new Respbase<ChatGroupDto> { Data = group };
    }

    /// <summary>
    /// 查找群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryGroupDtos> QueryGroup(QueryGroupReq req)
    {

      ChatFacade facade = new ChatFacade();
      IPagedList<ChatGroupDto> dtos = facade.QueryGroup(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryGroupDtos> { Result = -1, Message = "没有找到符合搜索条件的群组", Data = null };
      }
      return new Respbase<QueryGroupDtos> { Data = new QueryGroupDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }

    /// <summary>
    /// 我的群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<ChatGroupDto>> MyGroup(Reqbase req)
    {
      ChatFacade facade = new ChatFacade();
      List<ChatGroupDto> dtos = facade.MyGroup(req);
      return new Respbase<List<ChatGroupDto>> { Data = dtos };
    }

    /// <summary>
    /// 加入群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase JoinGroup(JoinGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.JoinGroup(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 加入群组确认
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase JoinGroupConfirm(JoinGroupConfirmReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.JoinGroupConfirm(req);
      return facade.PromptInfo;
    }


    /// <summary>
    /// 群主拉人加入群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase JoinGroupInvitation(JoinGroupInvitationReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.JoinGroupInvitation(req);
      return facade.PromptInfo;
    }


    /// <summary>
    /// 退出群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase QuitGroup(QuitGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.QuitGroup(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 移除群成员
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase RemoveGroupUser(RemoveGroupUserReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.RemoveGroupUser(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 解散群组
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase DismissGroup(DismissGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.DismissGroup(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 查询群组成员
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryFriendDtos> QueryGroupUser(QueryGroupUserReq req)
    {
      ChatFacade facade = new ChatFacade();
      IPagedList<ChatUserDto> dtos = facade.QueryGroupUser(req);

      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryFriendDtos> { Result = -1, Message = (facade.PromptInfo.Message == "您不是群成员" ? "您不是群成员" : "没有找到群成员"), Data = null };
      }
      return new Respbase<QueryFriendDtos> { Data = new QueryFriendDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }

    /// <summary>
    /// 群组禁言
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase GagUserAddGroup(GagUserAddGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.GagUserAddGroup(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 群组禁言移除
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase GagUserRemoveGroup(GagUserRemoveGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.GagUserRemoveGroup(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 群组禁言查询
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<GagUser>> GagUserQueryGroup(GagUserQueryGroupReq req)
    {
      ChatFacade facade = new ChatFacade();
      List<GagUser> dtos = facade.GagUserQueryGroup(req);
      if (dtos == null)
      {
        return new Respbase<List<GagUser>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = null };
      }
      if (dtos.Count == 0)
      {
        return new Respbase<List<GagUser>> { Result = -1, Message = "本群没有禁言用户", Data = null };
      }
      return new Respbase<List<GagUser>> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = dtos };
    }

    /// <summary>
    /// 创建系统聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<TchatRoom> CreateChatRoom(CreateChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      TchatRoom result = facade.CreateChatRoom(req);
      return new Respbase<TchatRoom> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
    }

    /// <summary>
    /// 创建收费聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<TchatRoom> CreateChatRoom2(CreateChatRoom2Req req)
    {
      ChatFacade facade = new ChatFacade();
      TchatRoom result = facade.CreateChatRoom2(req);
      return new Respbase<TchatRoom> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
    }

    /// <summary>
    /// 修改聊天室信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<TchatRoom> UpdateChatRoom(UpdateChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      TchatRoom result = facade.UpdateChatRoom(req);
      return new Respbase<TchatRoom> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
    }

    /// <summary>
    /// 修改聊天室密码
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase UpdateChatRoomPwd(UpdateChatRoomPwdReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.UpdateChatRoomPwd(req);
      return facade.PromptInfo;
    }
    /// <summary>
    /// 查询聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryChatRoomDtos> QueryChatRoom(QueryChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      IPagedList<ChatRoomDto> dtos = facade.QueryChatRoom(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryChatRoomDtos> { Message = "没有找到符合搜索条件的聊天室", Result = -1, Data = null };
      }
      foreach (var item in dtos)
      {
        if (!string.IsNullOrEmpty(item.Roompic))
        {
          item.Roompic = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + item.Roompic;
        }
      }
      return new Respbase<QueryChatRoomDtos> { Data = new QueryChatRoomDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }
    /// <summary>
    /// 加入聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase JoinChatRoom(JoinChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.JoinChatRoom(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 退出聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase QuitChatRoom(QuitChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.QuitChatRoom(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 销毁/解散聊天室
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase DestroyChatRoom(DestroyChatRoomReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.DestroyChatRoom(req);
      return facade.PromptInfo;
    }



    /// <summary>
    /// 查询聊天室当前人数
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<int> QueryChatRoomUserCount(QueryChatRoomUserCountReq req)
    {
      ChatFacade facade = new ChatFacade();
      int result = facade.QueryChatRoomUserCount(req);
      return new Respbase<int> { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message, Data = result };
    }



    /// <summary>
    /// 查找公众号
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<ChatPublicDto>> QueryPublic(QueryPublicReq req)
    {
      ChatFacade facade = new ChatFacade();
      List<ChatPublicDto> dtos = facade.QueryPublic(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<List<ChatPublicDto>> { Result = -1, Message = "没有找到符合搜索条件的公众号", Data = null };
      }
      return new Respbase<List<ChatPublicDto>> { Data = dtos };
    }


    /// <summary>
    /// 获取省份列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<VnetProvince>> ProvinceList(Reqbase req)
    {
      ChatFacade facade = new ChatFacade();
      List<VnetProvince> dtos = facade.ProvinceList(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<List<VnetProvince>> { Result = -1, Message = "获取数据失败", Data = null };
      }
      return new Respbase<List<VnetProvince>> { Data = dtos };
    }

    /// <summary>
    /// 获取城市列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<VnetCity>> CityList(Reqbase req)
    {
      ChatFacade facade = new ChatFacade();
      List<VnetCity> dtos = facade.CityList(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<List<VnetCity>> { Result = -1, Message = "获取数据失败", Data = null };
      }
      return new Respbase<List<VnetCity>> { Data = dtos };
    }

    /// <summary>
    /// 添加关注
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase ConcernAdd(ConcernAddReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.ConcernAdd(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 取消关注
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase ConcernCancel(ConcernCancelReq req)
    {
      ChatFacade facade = new ChatFacade();
      bool result = facade.ConcernCancel(req);
      return facade.PromptInfo;
    }

    /// <summary>
    /// 我的粉丝列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryFriendDtos> MyFans(MyFansReq req)
    {
      ChatFacade facade = new ChatFacade();
      IPagedList<ChatUserDto> dtos = facade.MyFans(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryFriendDtos> { Result = -1, Message = "您暂时还没有粉丝", Data = null };
      }
      return new Respbase<QueryFriendDtos> { Data = new QueryFriendDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }

    /// <summary>
    /// 我的关注列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<QueryFriendDtos> MyConcerns(MyConcernsReq req)
    {
      ChatFacade facade = new ChatFacade();
      IPagedList<ChatUserDto> dtos = facade.MyConcerns(req);
      if (dtos == null || dtos.Count == 0)
      {
        return new Respbase<QueryFriendDtos> { Result = -1, Message = "您暂时还没有关注其他用户", Data = null };
      }
      return new Respbase<QueryFriendDtos> { Data = new QueryFriendDtos { Item = dtos, pageInfo = dtos.PageInfo } };
    }

    /// <summary>
    /// 附近的人
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<List<GetNearbyDto>> GetNearby(GetNearbyReq req)
    {
      ChatFacade facade = new ChatFacade();
      return facade.GetNearby(req);
    }

    /// <summary>
    /// 摇一摇
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Respbase<GetYaoyiyaoDto> GetYaoyiyao(GetNearbyReq req)
    {
      ChatFacade facade = new ChatFacade();
      return facade.GetYaoyiyao(req);
    }
    [HttpPost]
    public Respbase GetChatHis(Reqbase req)
    {
      ChatFacade facade = new ChatFacade();
      var result = facade.GetChatHis();
      return new Respbase { Message = result };
    }
  }

}
