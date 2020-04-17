using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using PXin.Protocal;
using System.Threading;
using PXin.Common;
namespace PXin.Commu
{
    /// <summary>
    /// tcp通用类-服务端
    /// </summary>
    public class CommuTcpServer
    {
        private Socket _listerSocket;
        /// <summary>
        /// 连接事件
        /// </summary>
        public event CommuEventHander ConnectEventHandler;
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                PxinConst.ServerState = 0;
                _listerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                _listerSocket.Bind(endPoint);
                _listerSocket.Listen(3000);
                _listerSocket.BeginAccept(new AsyncCallback(AcceptConn), _listerSocket);
            }
            catch (Exception ex)
            {
                Log.ExceptInfo(ex.ToString());
            }
            return true;
        }
        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            if (_listerSocket != null)
            {
                PxinConst.ServerState = 2;
                if (_listerSocket.Connected)
                {
                    try
                    {
                        _listerSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch { }
                }
                _listerSocket.Close();
                _listerSocket = null;
            }
            return true;
        }
        /// <summary>
        /// 委托方法-接受客户端连接
        /// </summary>
        /// <param name="iar"></param>
        private void AcceptConn(IAsyncResult iar)
        {
            Socket listerSock = (Socket)iar.AsyncState;
            Socket client = null;
            try
            {
                client = listerSock.EndAccept(iar);
                OnRaiseConnectEvent(client);
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo("接收连接处理失败:" + err.ToString());
                if(client != null)
                {
                    client.Close();
                }
            }
            if (PxinConst.ServerState == 0)
            {
                listerSock.BeginAccept(new AsyncCallback(AcceptConn), listerSock);
            }
        }
        /// <summary>
        /// 发布"连接"事件
        /// </summary>
        /// <param name="client"></param>
        private void OnRaiseConnectEvent(Socket client)
        {
            ConnectEventHandler?.Invoke(this, new CommuEventArgs(client));
        }
    }
}
