using Common.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Pxin.InternalService.Protocal;
using Pxin.InternalServiceTCP.Facade.Facade;
using PXin.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PXin.SignalR.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        Log logger = new Log(typeof(ChatHub));

        LoginFacade loginFacade = new LoginFacade();
        ChatFeeFacade chatFeeFacade = new ChatFeeFacade();
        ChatFeeRateFacade chatFeeRateFacade = new ChatFeeRateFacade();
        private readonly static SignalRClientManager<string, BelieveCasClient> _connections = SignalRClientManager<string, BelieveCasClient>.GetInstance();
        /// <summary>
        /// ctor
        /// </summary>
        public ChatHub()
        {
            //EventBus.GetInstance().Register(new AddFriendNoticeEventHandler()).Subscribe();
        }
        #region Connection
        /// <summary>
        /// 连接完成后
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var chatUser = new BelieveCasClient(Clients.Caller);
            _connections.Add(Context.ConnectionId, chatUser);
            MessageInfo("连接成功");
            return base.OnConnected();
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(Context.ConnectionId);
            MessageInfo("断开连接");
            return base.OnDisconnected(stopCalled);
        }
        /// <summary>
        /// 重连
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            var connectionId = Context.ConnectionId;

            if (_connections.GetConnection(connectionId) == null)
            {
                _connections.Add(connectionId, new BelieveCasClient(Clients.Client(Context.ConnectionId)));
            }
            MessageInfo("重连成功");
            return base.OnReconnected();
        }
        #endregion

        #region Message
        /// <summary>
        /// 接收客户端消息
        /// </summary>
        /// <param name="messageString"></param>
        public void ReceiveChatMessage(string messageString)
        {
            logger.Info("messageString=" + messageString);
            var messageHeader = JsonConvert.DeserializeObject<MessageHeader>(messageString);
            switch (messageHeader.Command_Id)
            {
                case PXin_COMMAND_TYPE.Login:
                    ProLogin(messageString);
                    break;
                case PXin_COMMAND_TYPE.LoginResp:
                    break;
                case PXin_COMMAND_TYPE.Logout:
                    ProLogout(messageString);
                    break;
                case PXin_COMMAND_TYPE.LogoutResp:
                    //ProWt_LogoutResp(messageBase);
                    break;
                case PXin_COMMAND_TYPE.Active:
                    break;
                case PXin_COMMAND_TYPE.ActiveResp:
                    break;
                case PXin_COMMAND_TYPE.ChatFee:
                    ProChatFee(messageString);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeResp:
                    break;
                case PXin_COMMAND_TYPE.ChatFeePush:
                    break;
                case PXin_COMMAND_TYPE.ChatFeePushResp:
                    //ProChatFeePushResp(messageBase);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSet:
                    ProChatFeeRateSet(messageString);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSetResp:
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateQuery:
                    ProChatFeeRateQuery(messageString);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateQueryResp:
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSetPush:
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSetPushResp:
                    //ProChatFeeRateSetPushResp(messageBase);
                    break;
                default:
                    logger.Error("非法请求，Msg=" + messageString);
                    break;
            }

        }
        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void SendChatMessage(BelieveCasClient client, string message)
        {
            if (client == null || client?.Client == null)
            {
                logger.Error("异常：客户端为空");
                return;
            }
            client?.Client.receiveMessage(message);
        }
        #endregion

        #region Logic
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="messageString"></param>
        private void ProLogin(string messageString)
        {
            var client = _connections.GetConnection(Context.ConnectionId);
            if (client == null || string.IsNullOrWhiteSpace(messageString))
            {
                this.OnReconnected();
                return;
            }
            Login login = new Login(messageString);
            MessageInfo("调用登录逻辑之前");
            var result = loginFacade.ProLogin(login.ToBytes());
            MessageInfo("调用登录逻辑之后");
            if (result.Count == 0) MessageInfo("登录失败");
            foreach (var item in result)
            {
                client.Nodeid = item.GetNodeid();
                Clients.Caller.receiveMessage(item.ToJson());
            }
        }
        /// <summary>
        /// 客户端主动断开连接(登出)请求
        /// </summary>
        /// <param name="messageString"></param>
        private void ProLogout(string messageString)
        {
            Wt_Logout logout = new Wt_Logout(messageString);
            this.OnDisconnected(true);
        }
        /// <summary>
        /// 客户端主动断开连接(登出)响应
        /// </summary>
        /// <param name="messageString"></param>
        private void ProWt_LogoutResp(string messageString)
        {

        }
        /// <summary>
        /// 聊天计费倍率查询
        /// </summary>
        /// <param name="messageString"></param>
        private void ProChatFeeRateQuery(string messageString)
        {
            var client = _connections.GetConnection(Context.ConnectionId);
            if (client == null || client.Nodeid == 0)
            {
                MessageInfo($"聊天计费倍率查询客户端为空,client={client}");
                this.OnReconnected();
                return;
            }
            var chatFeeRateQuery = new ChatFeeRateQuery(messageString);
            MessageInfo("调用聊天计费倍率查询逻辑之前");
            var result = chatFeeRateFacade.ProChatFeeRateQuery(chatFeeRateQuery.ToBytes(), client.Nodeid);
            MessageInfo("调用聊天计费倍率查询逻辑之后");
            if (result.Count != 1) MessageInfo("调用聊天计费倍率查询逻辑失败");
            foreach (var item in result)
            {
                //var receiveClient = _connections.GetConnections().Values.FirstOrDefault(c => c.Nodeid == item.GetNodeid());
                //if (receiveClient == null)
                //{
                //    MessageInfo("聊天计费倍率查询结果推送失败：" + item.ToJson());
                //}
                Clients.Caller.receiveMessage(item.ToJson());
                //SendChatMessage(receiveClient, item.ToJson());
            }
        }
        /// <summary>
        /// 聊天计费
        /// </summary>
        /// <param name="messageString"></param>
        private void ProChatFee(string messageString)
        {
            var client = _connections.GetConnection(Context.ConnectionId);
            if (client == null || client.Nodeid == 0)
            {
                this.OnReconnected();
                return;
            }
            var chatFee = new ChatFee(messageString);
            MessageInfo("调用聊天计费逻辑之前");
            var result = chatFeeFacade.ProChatFee(chatFee.ToBytes(), client.Nodeid);
            MessageInfo("调用聊天计费逻辑之后");
            if (result.Count == 0) MessageInfo("调用聊天计费逻辑失败");
            foreach (var item in result)
            {
                var receiveClient = _connections.GetConnections().Values.FirstOrDefault(c => c.Nodeid == item.GetNodeid());
                SendChatMessage(receiveClient, item.ToJson());
            }
        }
        /// <summary>
        /// 聊天计费倍率设置
        /// </summary>
        /// <param name="messageString"></param>
        private void ProChatFeeRateSet(string messageString)
        {
            var client = _connections.GetConnection(Context.ConnectionId);
            if (client == null || client.Nodeid == 0)
            {
                this.OnReconnected();
                return;
            }
            var chatFeeRateSet = new ChatFeeRateSet(messageString);
            MessageInfo("调用聊天计费倍率设置逻辑之前");
            var result = chatFeeRateFacade.ProChatFeeRateSet(chatFeeRateSet.ToBytes(), client.Nodeid);
            MessageInfo("调用聊天计费倍率设置逻辑之后");
            if (result.Count == 0) MessageInfo("调用聊天计费倍率设置逻辑失败");
            foreach (var item in result)
            {
                var receiveClient = _connections.GetConnections().Values.FirstOrDefault(c => c.Nodeid == item.GetNodeid());
                SendChatMessage(receiveClient, item.ToJson());
            }
        }
        /// <summary>
        /// 聊天计费倍率设置推送响应
        /// </summary>
        /// <param name="messageString"></param>
        private void ProChatFeeRateSetPushResp(string messageString)
        {
        }
        /// <summary>
        /// 聊天计费推送响应
        /// </summary>
        /// <param name="messageString"></param>
        private void ProChatFeePushResp(string messageString)
        {
        }
        /// <summary>
        /// 日志消息
        /// </summary>
        /// <param name="message"></param>
        private void MessageInfo(string message)
        {
            var connectionId = Context.ConnectionId;
            var client = _connections.GetConnection(connectionId);
            logger.Info($"BelieveCas:{connectionId}-{client?.Nodeid}-Message:{message}");
        }
        #endregion
    }
}