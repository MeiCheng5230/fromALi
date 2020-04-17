using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PXin.Protocal;

namespace PXin.ClientApp
{
    public class TcpClient
    {
        private object _lock = new object();
        private Socket _client;
        private byte[] _buffer = new byte[1024];
        private int _position;
        private int _msgExceptCount;
        public string _identity;
        public string DeviceId { get; set; }
        private DateTime _lastActiveDate { get; set; }
        private DateTime _lastpayloadDate { get; set; }
        private System.Threading.Timer _timer;
        public bool IsUV = true;
        /// <summary>
        /// 收发消息平衡数，发送一条消息加一，接收一条消息减一
        /// </summary>
        public int MsgExceptCount
        {
            get { return _msgExceptCount; }
        }
        //定义处理tcp连接的事件
        public event CommuHander ConnEventHandler;
        //定义消息发送事件
        public event MessageHander MsgSendEventHandler;
        //定义消息接收事件
        public event MessageHander MsgRecvEventHandler;
        public bool Start()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["IP"]), Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));
                _client.Connect(endPoint);
                OnRaiseConnEvent(0, "连接成功");
                IPEndPoint point = (IPEndPoint)_client.LocalEndPoint;
                _identity = point.Address.ToString() + ":" + point.Port.ToString();
                _client.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, RecvData, _client);
                return true;
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo(err.ToString());
            }
            return false;
        }

        public bool Close()
        {
            try
            {
                Log.ExceptInfo("断开连接[" + _identity + "]");
                if (_client != null)
                {
                    _client.Shutdown(SocketShutdown.Both);
                    _client.Close();
                }
                _client = null;
                if (_timer != null)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timer.Dispose();
                    _timer = null;
                }
                OnRaiseConnEvent(1, "连接断开");
                return true;
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo("断开连接[" + _identity + "]失败" + err.ToString());
                return false;
            }
        }
        private void TimerInit()
        {
            if (IsUV)
            {
                if (_timer != null)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timer.Dispose();
                    _timer = null;
                }
                _timer = new Timer(TimePro, null, Timeout.Infinite, Timeout.Infinite);
                _timer.Change(1000, 1000);
            }
        }
        private void TimePro(object obj)
        {
            try
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                int rand = new Random().Next(1, 60);
                //if (rand == 5)
                //{
                //    ChatFeeRateQuery chatFeeRateQuery = new ChatFeeRateQuery();
                //    chatFeeRateQuery.Body.Type = 3;
                //    chatFeeRateQuery.Body.Receiver = 3434909;
                //    chatFeeRateQuery.Body.Sender = 0;
                //    SendMessageRateQuery(chatFeeRateQuery);
                //}
                //else
                //{
                TimeSpan span = DateTime.Now - _lastActiveDate;
                if (span.TotalSeconds > 30)
                {
                    SendMessageActive();
                }
                //}
                _timer.Change(1000, Timeout.Infinite);
            }
            catch (Exception)
            {

            }
        }
        public void RecvData(IAsyncResult ar)
        {
            var client = (Socket)ar.AsyncState;
            try
            {
                int recvLen = client.EndReceive(ar);
                if (recvLen == 0)
                {
                    //通知客户端已断开
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                if (recvLen + _position < MessageHeader.Length)
                {
                    _position += recvLen;
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                //读取数据
                MessageHeader header = new MessageHeader(_buffer);
                if (recvLen != header.Total_Length)
                {
                    Log.ExceptInfo("接收数据包长度不一致，包长度=" + header.Total_Length + ",接收长度=" + recvLen);
                }
                if (recvLen + _position < header.Total_Length)//接收到的数据与发送数据不匹配，发生分包
                {
                    //接收数据长度不够
                    _position += recvLen;
                    client.BeginReceive(_buffer, _position, _buffer.Length - _position, SocketFlags.None, new AsyncCallback(RecvData), client);
                    return;
                }
                recvLen += _position;
                if (recvLen > header.Total_Length)
                {
                    Log.ExceptInfo(_identity + ",开始解包,长度:" + recvLen);
                    int tmpcounter = 0;     //解包个数
                    byte[] tmpbuffer = null;//临时缓冲区
                    int tmpposition = 0;    //临时缓冲区指针位置
                    while (tmpposition + header.Total_Length <= recvLen)
                    {
                        tmpbuffer = new byte[header.Total_Length];//用来接收一个完整的包
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, tmpbuffer.Length);//将一个完整的包复制到临时缓存区
                        DataPrevPro(header);
                        tmpcounter++;
                        tmpposition += (int)header.Total_Length;
                        if (tmpposition + MessageHeader.Length > recvLen)
                        {
                            break;
                        }
                        tmpbuffer = new byte[MessageHeader.Length];
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, MessageHeader.Length);
                        header = new MessageHeader(tmpbuffer);
                    }
                    Log.ExceptInfo(_identity + ",结束解包,解包个数:" + tmpcounter);
                    tmpbuffer = null;
                    if (tmpposition < recvLen)
                    {
                        tmpbuffer = new byte[recvLen - tmpposition];
                        Buffer.BlockCopy(_buffer, tmpposition, tmpbuffer, 0, recvLen - tmpposition);
                    }
                    _buffer = new byte[1024];
                    _position = 0;
                    if (tmpbuffer != null)
                    {
                        Buffer.BlockCopy(tmpbuffer, 0, _buffer, 0, tmpbuffer.Length);
                        _position = tmpbuffer.Length;
                    }
                }
                else
                {
                    DataPrevPro(header);
                    _buffer = new byte[1024];
                    _position = 0;
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Trace.WriteLine(err.ToString());
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
                Close();
            }
        }

        private void DataPrevPro(MessageHeader header)
        {
            lock (_lock)
            {
                Interlocked.Decrement(ref _msgExceptCount);
                Log.RunInfo("," + _identity + ",接收," + _msgExceptCount);
            }
            #region 数据处理
            if (header.Command_Id == PXin_COMMAND_TYPE.Active)
            {
                ProWt_Active();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ActiveResp)
            {
                ProWt_ActiveResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.LoginResp)
            {
                ProWt_LoginResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.Logout)
            {
                ProLogout();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.LogoutResp)
            {
                ProLogoutResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ChatFeeResp)
            {
                ProChatFeeResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ChatFeePush)
            {
                ProChatFeePush();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ChatFeeRateSetResp)
            {
                ProChatFeeRateSetResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ChatFeeRateQueryResp)
            {
                ProChatFeeRateQueryResp();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ChatFeeRateSetPush)
            {
                ProChatFeeRateSetPush();
            }
            else if (header.Command_Id == PXin_COMMAND_TYPE.ClientCountQueryResp)
            {
                ProClientCountResp();
            }
            #endregion
        }


        private void ProWt_Active()
        {
            Active active = new Active(_buffer);
            ActiveResp actvieResp = new ActiveResp(active.Header.Sequence_Id);
            OnRaiseMsgRecvEvent(active.Header.Command_Id, active, "接收消息:" + active.ToString());
            SendData(actvieResp.ToBytes());
            OnRaiseMsgSendEvent(actvieResp.Header.Command_Id, actvieResp, "发送消息:" + actvieResp.ToString());
        }
        private void ProWt_ActiveResp()
        {
            _lastActiveDate = DateTime.Now;
            ActiveResp actvieResp = new ActiveResp(_buffer);
            OnRaiseMsgRecvEvent(actvieResp.Header.Command_Id, actvieResp, "接收消息:" + actvieResp.ToString());
        }
        private void ProWt_LoginResp()
        {
            LoginResp loginResp = new LoginResp(_buffer);
            OnRaiseMsgRecvEvent(loginResp.Header.Command_Id, loginResp, "接收消息:" + loginResp.ToString());
            TimerInit();
        }
        private void ProLogout()
        {
            Wt_Logout logout = new Wt_Logout(_buffer);
            OnRaiseMsgRecvEvent(logout.Header.Command_Id, logout, "接收消息:" + logout.ToString());
            Close();
        }
        private void ProLogoutResp()
        {
            LogoutResp logoutResp = new LogoutResp(_buffer);
            OnRaiseMsgRecvEvent(logoutResp.Header.Command_Id, logoutResp, "接收消息:" + logoutResp.ToString());
        }
        private void ProChatFeeResp()
        {
            ChatFeeResp chatFeeResp = new ChatFeeResp(_buffer);
            OnRaiseMsgRecvEvent(chatFeeResp.Header.Command_Id, chatFeeResp, "接收消息:" + chatFeeResp.ToString());
        }
        private void ProChatFeePush()
        {
            ChatFeePush chatFeePush = new ChatFeePush(_buffer);
            OnRaiseMsgRecvEvent(chatFeePush.Header.Command_Id, chatFeePush, "接收消息:" + chatFeePush.ToString());
            ChatFeePushResp chatFeePushResp = new ChatFeePushResp(chatFeePush.Header.Sequence_Id);
            SendData(chatFeePushResp.ToBytes());
            OnRaiseMsgSendEvent(chatFeePushResp.Header.Command_Id, chatFeePushResp, "发送消息:" + chatFeePushResp.ToString());
        }
        private void ProChatFeeRateSetResp()
        {
            ChatFeeRateSetResp chatFeeRateSetResp = new ChatFeeRateSetResp(_buffer);
            OnRaiseMsgRecvEvent(chatFeeRateSetResp.Header.Command_Id, chatFeeRateSetResp, "接收消息:" + chatFeeRateSetResp.ToString());
        }
        private void ProChatFeeRateQueryResp()
        {
            ChatFeeRateQueryResp chatFeeRateQueryResp = new ChatFeeRateQueryResp(_buffer);
            OnRaiseMsgRecvEvent(chatFeeRateQueryResp.Header.Command_Id, chatFeeRateQueryResp, "接收消息:" + chatFeeRateQueryResp.ToString());
        }

        private void ProChatFeeRateSetPush()
        {
            ChatFeeRateSetPush chatFeeRateSetPush = new ChatFeeRateSetPush(_buffer);
            OnRaiseMsgRecvEvent(chatFeeRateSetPush.Header.Command_Id, chatFeeRateSetPush, "接收消息:" + chatFeeRateSetPush.ToString());
            ChatFeeRateSetPushResp chatFeeRateSetPushResp = new ChatFeeRateSetPushResp(chatFeeRateSetPush.Header.Sequence_Id);
            SendData(chatFeeRateSetPushResp.ToBytes());
            OnRaiseMsgSendEvent(chatFeeRateSetPushResp.Header.Command_Id, chatFeeRateSetPushResp, "发送消息:" + chatFeeRateSetPushResp.ToString());
        }

        private void ProClientCountResp()
        {
            ClientCountQueryResp resp = new ClientCountQueryResp(_buffer);
            OnRaiseMsgRecvEvent(resp.Header.Command_Id, resp, "接收消息：" + resp.ToString());
        }

        public bool SendMessageLogin(Login login)
        {
            SendData(login.ToBytes());
            OnRaiseMsgSendEvent(login.Header.Command_Id, login, "发送消息:" + login.ToString());
            return true;
        }

        public bool SendMessageActive()
        {
            _lastActiveDate = DateTime.Now;
            Active actvie = new Active();
            SendData(actvie.ToBytes());
            OnRaiseMsgSendEvent(actvie.Header.Command_Id, actvie, "发送消息:" + actvie.ToString());
            return true;
        }
        public bool SendChatFee(uint ReceiveType = 1, string Receiver = "4242292")
        {
            ChatFee chatFee = new ChatFee();
            Random random = new Random();
            chatFee.Body = new ChatFeeBody
            {
                BusinessType = (uint)random.Next(1, 6),
                FeeType = (uint)random.Next(1, 3),
                Num = (uint)random.Next(1, 20),
                ReceiveType = ReceiveType,
                Receiver = Receiver,
                //ReceiveType = (uint)2,
                //Receiver = "449",
                FeeTime = DateTime.Now,
                Rate = random.Next(1, 10)
            };
            SendData(chatFee.ToBytes());
            OnRaiseMsgSendEvent(chatFee.Header.Command_Id, chatFee, "发送消息:" + chatFee.ToString());
            return true;
        }
        /// <summary>
        /// 聊天包重复发送
        /// </summary>
        /// <param name="ReceiveType"></param>
        /// <param name="Receiver"></param>
        /// <returns></returns>
        public bool SendChatFeeRepeat()
        {
            ChatFee chatFee = new ChatFee();
            chatFee.Header.Sequence_Id = 2019110818353116;
            Random random = new Random();
            chatFee.Body = new ChatFeeBody
            {
                BusinessType = (uint)random.Next(1, 6),
                FeeType = (uint)random.Next(1, 3),
                Num = (uint)random.Next(1, 20),
                ReceiveType = 1,
                Receiver = "2000",
                //ReceiveType = (uint)2,
                //Receiver = "449",
                FeeTime = DateTime.Now,
                Rate = random.Next(1, 10)
            };
            SendData(chatFee.ToBytes());
            OnRaiseMsgSendEvent(chatFee.Header.Command_Id, chatFee, "发送消息:" + chatFee.ToString());
            return true;
        }

        public bool SendPM()
        {
            Active actvie = new Active();
            byte[] buffer1 = actvie.ToBytes();
            byte[] buffer2 = actvie.ToBytes();
            byte[] buffer3 = actvie.ToBytes();

            byte[] buffer = new byte[buffer2.Length * 3];
            Buffer.BlockCopy(buffer1, 0, buffer, 0, buffer1.Length);
            Buffer.BlockCopy(buffer2, 0, buffer, buffer1.Length, buffer2.Length);
            Buffer.BlockCopy(buffer3, 0, buffer, buffer1.Length + buffer2.Length, buffer3.Length);

            byte[] sendByte = new byte[4];
            Buffer.BlockCopy(buffer, 0, sendByte, 0, sendByte.Length);
            SendData(sendByte);

            sendByte = new byte[12];
            Buffer.BlockCopy(buffer, 4, sendByte, 0, sendByte.Length);
            SendData(sendByte);

            sendByte = new byte[buffer.Length - 16];
            Buffer.BlockCopy(buffer, 16, sendByte, 0, sendByte.Length);
            SendData(sendByte);

            return true;
        }

        public bool SendMessageLogout()
        {
            Wt_Logout logout = new Wt_Logout();
            SendData(logout.ToBytes());
            OnRaiseMsgSendEvent(logout.Header.Command_Id, logout, "发送消息:" + logout.ToString());
            Close();
            return true;
        }
        /// <summary>
        /// 向服务器发送"倍率设置"消息
        /// </summary>
        /// <param name="chatFeeRateSet"></param>
        /// <returns></returns>
        public bool SendMessageRateSet(ChatFeeRateSet chatFeeRateSet)
        {
            SendData(chatFeeRateSet.ToBytes());
            OnRaiseMsgSendEvent(chatFeeRateSet.Header.Command_Id, chatFeeRateSet, "发送消息:" + chatFeeRateSet.ToString());
            return true;
        }
        /// <summary>
        /// 向服务器发送"倍率查询"消息
        /// </summary>
        /// <param name="chatFeeRateQuery"></param>
        /// <returns></returns>
        public bool SendMessageRateQuery(ChatFeeRateQuery chatFeeRateQuery)
        {
            SendData(chatFeeRateQuery.ToBytes());
            OnRaiseMsgSendEvent(chatFeeRateQuery.Header.Command_Id, chatFeeRateQuery, "发送消息:" + chatFeeRateQuery.ToString());
            return true;
        }

        public bool SendMessageClientCountQuery(ClientCountQuery clientCountQuery)
        {
            SendData(clientCountQuery.ToBytes());
            OnRaiseMsgSendEvent(clientCountQuery.Header.Command_Id, clientCountQuery, "发送消息:" + clientCountQuery.ToString());
            return true;
        }
        private bool SendData(byte[] data)
        {
            try
            {
                lock (_lock)
                {
                    _lastpayloadDate = DateTime.Now;
                    _client.Send(data);
                    Interlocked.Increment(ref _msgExceptCount);
                    Log.RunInfo("," + _identity + ",发送," + _msgExceptCount);
                }
                return true;
            }
            catch (Exception err)
            {
                //System.Diagnostics.Trace.WriteLine(err.ToString());
                Log.ExceptInfo("SendData：" + err.ToString());
                Close();
            }
            return false;
        }
        /// <summary>
        /// 发布"tcp连接"事件
        /// </summary>
        /// <param name="info"></param>
        private void OnRaiseConnEvent(int status, string info)
        {
            ConnEventHandler?.Invoke(this, new EventCommu(info, status));
        }
        /// <summary>
        /// 发布"消息接收"事件
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="package"></param>
        /// <param name="msg"></param>
        private void OnRaiseMsgRecvEvent(PXin_COMMAND_TYPE commandType, object package, string msg)
        {
            MsgRecvEventHandler?.Invoke(this, new EventMessage(commandType, package, msg));
        }
        /// <summary>
        /// 发布"消息发送"事件
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="package"></param>
        /// <param name="msg"></param>
        private void OnRaiseMsgSendEvent(PXin_COMMAND_TYPE commandType, object package, string msg)
        {
            MsgSendEventHandler?.Invoke(this, new EventMessage(commandType, package, msg));
        }
    }
}
