using PXin.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.SignalR.Hubs
{
    /// <summary>
    /// 聊天总线接口
    /// </summary>
    public interface IChatHub
    {
        /// <summary>
        /// 服务器下发消息到各个客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        void SendChatMessage(BelieveCasClient client, string message);
        /// <summary>
        /// 接收客户端的消息
        /// </summary>
        /// <param name="message"></param>
        void ReceiveChatMessage(string message);
    }
}
