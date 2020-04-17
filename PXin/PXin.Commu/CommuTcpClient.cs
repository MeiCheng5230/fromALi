using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PXin.Common;
using PXin.Protocal;

namespace PXin.Commu
{
    /// <summary>
    /// tcp通用类-客户端
    /// </summary>
    public class CommuTcpClient
    {
        public event MessageEventHander MsgSendEventHandler;
        public event MessageEventHander MsgRecvEventHandler;
        public event CommuEventHander ConnectEventHandler;
        private Socket _client;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 缓冲区指针位置
        /// </summary>
        private int _position;
        private int _msgExceptCount;
        private int _recvZeroByteCounter;
        /// <summary>
        /// 最近接收消息时间
        /// </summary>
        public DateTime LastActiveTime { get; private set; }
        /// <summary>
        /// 收发消息平衡数，发送一条消息加一，接收一条消息减一
        /// </summary>
        public int MsgExceptCount { get { return _msgExceptCount; } }
        /// <summary>
        /// 连接标识[IP:PORT]
        /// </summary>
        public string Identity { get; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 推送消息计数器
        /// </summary>
        public int InputInfoCounter { get; set; }

        public CommuTcpClient(Socket socket)
        {
            _client = socket;
            IPEndPoint endPoint = (IPEndPoint)_client.RemoteEndPoint;
            Identity = endPoint.Address.ToString() + ":" + endPoint.Port.ToString();
            LastActiveTime = DateTime.Now;
            _recvZeroByteCounter = 0;
        }
        /// <summary>
        /// 启动接收数据
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            _buffer = new byte[1024];
            _position = 0;
            _client.BeginReceive(_buffer, _position, 1024, SocketFlags.None, new AsyncCallback(RecvData), _client);
            return true;
        }
        public bool Close(string reason)
        {
            if (_client != null)
            {
                try
                {
                    _client.Shutdown(SocketShutdown.Both);
                }
                catch { }
                _client.Close();
            }
            _client = null;
            OnRaiseConnectEvent(Identity, reason);
            return true;
        }
        public bool SendData(byte[] data)
        {
            lock (this)
            {
                try
                {
                    _client.Send(data);
                    Interlocked.Increment(ref _msgExceptCount);
                    LastActiveTime = DateTime.Now;
#if DEBUG
                    Log.RunInfo(Identity + ",发送，" + _msgExceptCount);
#endif
                    OnRaiseMsgSendEvent(data);
                    return true;
                }
                catch (System.Exception err)
                {
                    Close(Identity + "发送数据异常，" + err.ToString());
                    Log.ExceptInfo("发送数据异常，" + err.ToString());
                }
                return false;
            }
        }
        /// <summary>
        /// 接收来自来客户端的数据
        /// </summary>
        /// <param name="iar"></param>
        private void RecvData(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                int recvLen = client.EndReceive(iar);
                if (recvLen == 0)
                {
                    _recvZeroByteCounter++;
                    Log.ExceptInfo("接收数据包,接收长度=" + recvLen + ",recvZeroByteCounter=" + _recvZeroByteCounter);
                    if (_recvZeroByteCounter > 3)
                    {
                        Close("客户端自动断开，接收数据长度为0");
                        return;
                    }
                    Thread.Sleep(1);
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                if (recvLen + _position < MessageHeader.Length)//包头信息不完整
                {
                    //接收数据长度不够
                    _position += recvLen;
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                //读取数据
                MessageHeader header = new MessageHeader(_buffer);
                if (recvLen + _position < header.Total_Length)//接收到的数据与发送数据不匹配，发生分包
                {
                    //接收数据长度不够
                    _position += recvLen;
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                if (Nodeid <= 0 && header.Command_Id != PXin_COMMAND_TYPE.Login)
                {
                    Close("非法数据[未登陆发送非登陆包]，" + header.ToString());
                    return;
                }
#if DEBUG
                if (recvLen != header.Total_Length)
                {
                    Log.ExceptInfo("接收数据包长度不一致，包长度=" + header.Total_Length + ",接收长度=" + recvLen + ",Position=" + _position);
                }
#endif
                recvLen += _position;
                if (recvLen > header.Total_Length)//已经包含了一个完整包的长度
                {
#if DEBUG
                    Log.ExceptInfo(Identity + ",开始解包,长度:" + recvLen);
#endif
                    int tmpcounter = 0;     //解包个数
                    byte[] tmpbuffer = null;//临时缓冲区
                    int tmpposition = 0;    //临时缓冲区指针位置
                    while (tmpposition + header.Total_Length <= recvLen)//_buffer中已经包含一个完整的数据包
                    {
                        tmpbuffer = new byte[header.Total_Length];//用来接收一个完整的包
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, tmpbuffer.Length);//将一个完整的包复制到临时缓存区
                        OnRaiseMsgRecvEvent((int)header.Command_Id, tmpbuffer);//用完整的包数据执行业务，根据CommandID
                        tmpcounter++;
                        tmpposition += (int)header.Total_Length;
                        if (tmpposition + MessageHeader.Length > recvLen)//是否还存在完整的包头长度
                        {
                            break;
                        }
                        tmpbuffer = new byte[MessageHeader.Length];
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, MessageHeader.Length);
                        header = new MessageHeader(tmpbuffer);
                    }
#if DEBUG
                    Log.ExceptInfo(Identity + ",结束解包,解包个数:" + tmpcounter);
#endif
                    tmpbuffer = null;
                    if (tmpposition < recvLen)//_buffer中还存在有另一个包的数据
                    {
                        tmpbuffer = new byte[recvLen - tmpposition];
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, recvLen - tmpposition);
                    }
                    _buffer = new byte[1024];
                    _position = 0;
                    if (tmpbuffer != null)
                    {
                        Buffer.BlockCopy(tmpbuffer, 0, _buffer, 0, tmpbuffer.Length);//存储多余的包数据
                        _position = tmpbuffer.Length;
                    }
                }
                else
                {//刚好是完整的包，直接解析
                    OnRaiseMsgRecvEvent((int)header.Command_Id, _buffer);
                    _buffer = new byte[1024];
                    _position = 0;
                }
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo(err.ToString());
                _buffer = new byte[1024];
                _position = 0;
            }
            try
            {
                if (client.Connected)
                {
                    //继续接收来自来客户端的数据
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                }
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo(err.ToString());
                Close("接收数据异常," + err.ToString());
            }
        }

        private void OnRaiseMsgSendEvent(byte[] buffer)
        {
            if (MsgSendEventHandler != null)
            {
                string msg = string.Empty;
                MessageHeader header = new MessageHeader(buffer);
                switch (header.Command_Id)
                {
                    case PXin_COMMAND_TYPE.Login:
                        msg = new Login(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.LoginResp:
                        msg = new LoginResp(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.Logout:
                        msg = new Wt_Logout(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.LogoutResp:
                        msg = new LogoutResp(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.Active:
                        msg = new Active(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ActiveResp:
                        msg = new ActiveResp(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFee:
                        msg = new ChatFee(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeeResp:
                        msg = new ChatFeeResp(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeePush:
                        msg = new ChatFeePush(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeeRateSet:
                        msg = new ChatFeeRateSet(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeeRateSetResp:
                        msg = new ChatFeeRateSetResp(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeeRateQuery:
                        msg = new ChatFeeRateQuery(buffer).ToString();
                        break;
                    case PXin_COMMAND_TYPE.ChatFeeRateQueryResp:
                        msg = new ChatFeeRateQueryResp(buffer).ToString();
                        break;
                    default:
                        break;
                }
                //发布"消息发送"事件
                if (header.Command_Id != PXin_COMMAND_TYPE.Active && header.Command_Id != PXin_COMMAND_TYPE.ActiveResp)
                {
                    MsgSendEventHandler?.Invoke(this, new MessageEventArgs(string.Empty, Identity, msg));
                }
            }
        }
        /// <summary>
        /// 发起"接收客户端消息"事件
        /// </summary>
        /// <param name="commandtype"></param>
        /// <param name="buffer"></param>
        private void OnRaiseMsgRecvEvent(int commandtype, byte[] buffer)
        {
            LastActiveTime = DateTime.Now;
            Interlocked.Decrement(ref _msgExceptCount);
#if DEBUG
            Log.RunInfo(Identity + ",接收，" + _msgExceptCount);
#endif
            MsgRecvEventHandler?.Invoke(this, new MessageEventArgs(Identity, string.Empty, commandtype, buffer));
        }
        private void OnRaiseConnectEvent(string identity, string reason)
        {
            ConnectEventHandler?.Invoke(this, new CommuEventArgs(identity, 1, reason));
        }
    }
}
