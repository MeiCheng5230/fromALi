using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net.Sockets;
using System.Net;

namespace PXin.Common
{
    /// <summary>
    /// 通讯代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CommuEventHander(object sender, CommuEventArgs e);
    /// <summary>
    /// 消息代理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MessageEventHander(object sender, MessageEventArgs e);

    /// <summary>
    /// 消息事件参数
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs()
        {
        }
        public MessageEventArgs(string from, string to, int commandtype, byte[] buffer)
        {
            From = from;
            To = to;
            Commandtype = commandtype;
            Buffer = buffer;
        }
        public MessageEventArgs(string from, string to, string msg)
        {
            From = from;
            To = to;
            Msg = msg;
        }
        /// <summary>
        /// 消息发送方
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 消息接收方
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 消息命令类型
        /// </summary>
        public int Commandtype { get; set; }
        /// <summary>
        /// 消息包
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// 消息字符串
        /// </summary>
        public string Msg { get; set; }
    }
    /// <summary>
    /// 通讯事件参数
    /// </summary>
    public class CommuEventArgs : EventArgs
    {
        public CommuEventArgs(Socket client)
        {
            Client = client;
            IPEndPoint endPoint = (IPEndPoint)Client.RemoteEndPoint;
            Identity = endPoint.Address.ToString() + ":" + endPoint.Port.ToString();
            State = 0;
            Reason = "接收连接";
        }
        public CommuEventArgs(string identity, int state, string reason)
        {
            Identity = identity;
            State = state;
            Reason = reason;
        }
        public Socket Client { get; }
        /// <summary>
        /// 客户端连接标识
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// 0-连接，1-断开
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }

    /// <summary>
    /// 常用常量
    /// </summary>
    public static class PxinConst
    {
        /// <summary>
        /// 0-正常，1-连接已满，2-维护中
        /// </summary>
        public static int ServerState;
        /// <summary>
        /// 最大线程数
        /// </summary>
        public static int MaxThreadCount;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public static int MaxConnectCount;
        /// <summary>
        /// 连接超时时间，单位：秒
        /// </summary>
        public static int Timeout;
        /// <summary>
        /// 运行日志目录
        /// </summary>
        public static string DirPlayLog;
        /// <summary>
        /// 缓存消息目录名称
        /// </summary>
        public const string DirCacheMsg = "CacheMsg";
        /// <summary>
        /// 运行日志目录名称
        /// </summary>
        public const string DirRunLog = "RunLog";

        static PxinConst()
        {
            ServerState = 0;
            MaxThreadCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadCount"]);
            MaxConnectCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnectCount"]);
            Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
            DirPlayLog = ConfigurationManager.AppSettings["DirPlayLog"];
            if (string.IsNullOrEmpty(DirPlayLog))
            {
                DirPlayLog = Directory.GetCurrentDirectory();
            }
        }
    }
}
