using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using PXin.Common;
using PXin.Commu.Common;
using PXin.Commu.DataAccess;
using PXin.Commu.Model;
using PXin.Protocal;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Wt.DataAccess;

namespace PXin.Commu.Facade
{
    /// <summary>
    /// ServerFacade
    /// </summary>
    public class ServerFacade
    {
        /// <summary>
        /// show接收/发送的消息事件
        /// </summary>
        public event MessageEventHander MsgHandler_Show;
        /// <summary>
        /// 客户端连接时show消息事件
        /// </summary>
        public event CommuEventHander ConnectHandler_Show;

        private CommuTcpServer _server = new CommuTcpServer();
        private Timer _tmr;
        /// <summary>
        /// 客户端连接
        /// </summary>
        public Dictionary<string, CommuTcpClient> CommuClient { get; }

        private object commuClientSyncObj = new object();

        PXinDb pXinDb = new PXinDb();

        public ServerFacade()
        {
            CommuClient = new Dictionary<string, CommuTcpClient>();
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <returns></returns>
        public bool StartServer()
        {
            _server = new CommuTcpServer();
            //订阅"客户端连接"事件
            _server.ConnectEventHandler += new CommuEventHander(_server_ConnHandler);
            _server.Start();

            _tmr = new Timer(new TimerCallback(CheckTimeOutSocket), null, Timeout.Infinite, Timeout.Infinite);
            _tmr.Change(PxinConst.Timeout * 1000, 0);

            return true;
        }
        /// <summary>
        /// 收到发布"客户端连接"事件时的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _server_ConnHandler(object sender, CommuEventArgs e)
        {
            //接收到新连接
            if (e.State == 0)
            {
                AddCommuClient(e.Client);
            }
            ConnectHandler_Show?.Invoke(sender, e);
        }
        /// <summary>
        /// 新增客户端连接
        /// </summary>
        /// <param name="socket">客户端Socket</param>
        private CommuTcpClient AddCommuClient(Socket socket)
        {
            lock (commuClientSyncObj)
            {
                CommuTcpClient tcpClient = new CommuTcpClient(socket);
                if (CommuClient.ContainsKey(tcpClient.Identity))
                {
                    CommuClient[tcpClient.Identity].Close("客户端用同一个端口连接");
                }
                Log.RunInfo("新增Socket:" + tcpClient.Identity);
                tcpClient.MsgSendEventHandler += new MessageEventHander(tcpClient_MsgSendHandler);
                tcpClient.ConnectEventHandler += new CommuEventHander(tcpClient_ConnectHandler);
                tcpClient.MsgRecvEventHandler += new MessageEventHander(tcpClient_MsgRecvHandler);

                CommuClient.Add(tcpClient.Identity, tcpClient);
                //CommuClient[tcpClient.Identity] = tcpClient;
                tcpClient.Start();
                return tcpClient;
            }
        }
        /// <summary>
        /// 关闭服务器
        /// </summary>
        /// <returns></returns>
        public bool StopServer()
        {
            bool ret = _server.Close();
            List<string> commus = CommuClient.Keys.ToList();
            foreach (var item in commus)
            {
                CommuClient[item].Close("服务器关闭");
            }
            return true;
        }
        /// <summary>
        /// 移除客户端连接
        /// </summary>
        /// <param name="identity">客户端连接标识</param>
        public void RemoveCommuClient(string identity, string reason)
        {
            lock (commuClientSyncObj)
            {
                if (CommuClient.ContainsKey(identity))
                {
                    CommuClient.Remove(identity);
                    Log.RunInfo("移除Socket:" + identity + "," + reason);
                }
            }
        }
        /// <summary>
        /// 事件方法-断开客户端连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcpClient_ConnectHandler(object sender, CommuEventArgs e)
        {
            if (e.State == 1)
            {
                //断开连接
                CommuTcpClient commuClient = sender as CommuTcpClient;
                RemoveCommuClient(commuClient.Identity, e.Reason);
            }
            ConnectHandler_Show?.Invoke(sender, e);
        }
        /// <summary>
        /// 事件方法-消息显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcpClient_MsgSendHandler(object sender, MessageEventArgs e)
        {
            CommuTcpClient client = sender as CommuTcpClient;
            Log.MessageInfo($"{client.Identity}-{client.Nodeid}-Send:{e.Msg}");
            MsgHandler_Show?.Invoke(sender, e);
        }
        /// <summary>
        /// 事件方法-接收客户端消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcpClient_MsgRecvHandler(object sender, MessageEventArgs e)
        {
            //读取数据
            CommuTcpClient client = sender as CommuTcpClient;
            MessageHeader header = new MessageHeader(e.Buffer);
            switch (header.Command_Id)
            {
                case PXin_COMMAND_TYPE.Login:
                    ProLogin(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.LoginResp:
                    break;
                case PXin_COMMAND_TYPE.Logout:
                    ProLogout(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.LogoutResp:
                    ProWt_LogoutResp(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.Active:
                    ProActive(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ActiveResp:
                    ProActiveResp(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ChatFee:
                    ProChatFee(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ChatFeePushResp:
                    ProChatFeePushResp(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSet:
                    ProChatFeeRateSet(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateQuery:
                    ProChatFeeRateQuery(client, e.Buffer);
                    break;
                case PXin_COMMAND_TYPE.ChatFeeRateSetPushResp:
                    ProChatFeeRateSetPushResp(client, e.Buffer);
                    break;
                default:
                    Log.MessageInfo("非法包，Msg=" + header.ToString());
                    break;
            }
        }
        /// <summary>
        /// 检测超时连接
        /// </summary>
        /// <param name="obj"></param>
        private void CheckTimeOutSocket(object obj)
        {
            _tmr.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                DateTime dt = DateTime.Now.AddSeconds(-PxinConst.Timeout * 3);
                if (CommuClient != null && CommuClient.Count > 0)
                {
                    Log.RunInfo("当前连接数:" + CommuClient.Count);
                    List<CommuTcpClient> clients = CommuClient.Values.Where(c => c.Nodeid <= 0 && c.LastActiveTime < dt).ToList();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        clients[i].Close("超时未登陆成功连接,LastActiveTime=" + clients[i].LastActiveTime);
                    }
                    clients = CommuClient.Values.Where(c => (c.MsgExceptCount >= 3 || c.MsgExceptCount <= -3) && c.LastActiveTime < dt).ToList();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        clients[i].Close("超时未响应连接,LastActiveTime=" + clients[i].LastActiveTime + ",MsgExceptCount=" + clients[i].MsgExceptCount);
                    }
                    clients = CommuClient.Values.Where(c => c.LastActiveTime < dt).ToList();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        Active active = new Active();
                        ThreadHelper.InputQueue(new ThreadData { CommuTcpClient = clients[i], MsgData = active.ToBytes() });
                    }
                }
            }
            catch (System.Exception err)
            {
                Log.ExceptInfo(err.ToString());
            }
            _tmr.Change(PxinConst.Timeout * 1000, 0);
        }
        #region 消息处理

        private void OnRaiseMsgRecvEvent(CommuTcpClient client, string msg)
        {
            Log.MessageInfo($"{client.Identity}-{client.Nodeid}-Recv:{msg}");
            MsgHandler_Show?.Invoke(client, new MessageEventArgs(client.Identity, string.Empty, msg));
        }
        /// <summary>
        /// 链路测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProActive(CommuTcpClient client, byte[] buffer)
        {
            Active active = new Active(buffer);
            //OnRaiseMsgRecvEvent(client, active.ToString());
            ActiveResp activeResp = new ActiveResp(active.Header.Sequence_Id);
            client.SendData(activeResp.ToBytes());
        }
        /// <summary>
        /// 链路测试响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProActiveResp(CommuTcpClient client, byte[] buffer)
        {
            ActiveResp activeResp = new ActiveResp(buffer);
            //OnRaiseMsgRecvEvent(client, activeResp.ToString());
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProLogin(CommuTcpClient client, byte[] buffer)
        {
            Login login = new Login(buffer);
            OnRaiseMsgRecvEvent(client, login.ToString());
            LoginResp loginResp = new LoginResp(login.Header.Sequence_Id);
            //验证登陆
            ResultMsg msg = LoginValidate(login.Body, out int nodeId, out decimal pDian, out decimal vDian);
            var chatRate = pXinDb.GetTchatRate(0, nodeId, 3);//查询自己的倍率
            if (!msg.Success)
            {
                //验证失败
                loginResp.Body.Status = 0;
                loginResp.Body.LoginDesc = msg.Message;
            }
            else
            {
                //登陆成功
                loginResp.Body.Status = 1;
                loginResp.Body.LoginDesc = "登录成功";
                loginResp.Body.VDianBalance = vDian;
                loginResp.Body.PDianBalance = pDian;
                loginResp.Body.Rate = chatRate.Id == 0 ? 1 : chatRate.Rate;
                client.Nodeid = nodeId;
                string[] identitys = CommuClient.Values.Where(c => c.Nodeid == nodeId && c.Identity != client.Identity).Select(c => c.Identity).ToArray();
                foreach (var item in identitys)
                {
                    CommuClient[item].Close("抛弃旧连接");
                }
            }
            client.SendData(loginResp.ToBytes());
        }
        #region ProLogin_Call_Method
        /// <summary>
        /// 登录逻辑
        /// </summary>
        /// <param name="loginData"></param>
        /// <param name="nodeId"></param>
        /// <param name="pDian"></param>
        /// <param name="vDian"></param>
        /// <returns></returns>
        public ResultMsg LoginValidate(LoginBody loginData, out int nodeId, out decimal pDian, out decimal vDian)
        {
            nodeId = 0;
            pDian = 0;
            vDian = 0;
            ResultMsg msg = new ResultMsg() { Success = true };
            string appKey = ConfigurationManager.AppSettings["AppSign"];
            if (string.IsNullOrEmpty(appKey))
                appKey = "af83f787e8911dea9b3bf677746ebac9";
            if (!Helper.CheckMd5(loginData.NodeCode + loginData.Pwd, loginData.Sign, appKey))
            {
                msg.Success = false;
                msg.Message = "签名错误";
                msg.Result = 0;
                return msg;
            }
            
            PXinDb pXinDb = new PXinDb();
            nodeId = pXinDb.LoginByNodeCode(loginData.NodeCode);
            if (nodeId == 0)
            {
                msg.Success = false;
                msg.Message = "账号不存在";
                msg.Result = 0;
                return msg;
            }
            if (pXinDb.CheckLockUser(nodeId))
            {
                msg.Success = false;
                msg.Message = "用户已冻结";
                msg.Result = 0;
                return msg;
            }
            var userInfo = pXinDb.GetUserInfoByNodeId(nodeId);
            vDian = userInfo.V;
            pDian = userInfo.P;
            return msg;
        }
        #endregion
        /// <summary>
        /// 聊天计费
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProChatFee(CommuTcpClient client, byte[] buffer)
        {
            ChatFee chatFee = new ChatFee(buffer);

            OnRaiseMsgRecvEvent(client, chatFee.ToString());
            ChatFeeResp chatFeeResp = new ChatFeeResp(chatFee.Header.Sequence_Id);
            //处理计费业务
            List<PushPDian> pushPList = new List<PushPDian>();
            var result = (true, "");
            var key = client.Nodeid.ToString() + chatFee.Header.Sequence_Id;
            var chatFeeHis = pXinDb.GetChatFeeHisBySeq(client.Nodeid.ToString() + chatFee.Header.Sequence_Id);
            if (chatFeeHis.Hisid == 0)//判断是否是丢包情况重发
            {
                if (!string.IsNullOrEmpty(chatFee.Body.Receiver))
                {
                    result = HandChatFeeBusiness(chatFee, client.Nodeid, pushPList);
                }
                else
                {
                    result = (false, "无效参数:Receiver");
                }
            }
            var tpxinUserinfo = pXinDb.GetUserInfoByNodeId(client.Nodeid);
            chatFeeResp.Body.FeeType = chatFee.Body.FeeType;
            chatFeeResp.Body.Status = result.Item1 == true ? Convert.ToUInt32(1) : Convert.ToUInt32(0);
            chatFeeResp.Body.StatusDesc = result.Item1 == true ? "成功" : result.Item2;
            chatFeeResp.Body.VDianBalance = tpxinUserinfo.V;
            chatFeeResp.Body.PDianBalance = tpxinUserinfo.P;
            client.SendData(chatFeeResp.ToBytes());
            if (result.Item1)
            {
                if (chatFee.Body.ReceiveType == 1)//接收者类型为用户
                {
                    int receiverid = 0;
                    Int32.TryParse(chatFee.Body.Receiver, out receiverid);
                    var pushPDian = pushPList.FirstOrDefault(p => p.NodeId == receiverid);
                    PushReceive(pushPDian);
                }
                else if (chatFee.Body.ReceiveType == 2 || chatFee.Body.ReceiveType == 3)//接收者类型为群时
                {
                    foreach (var item in pushPList)//遍历群成员进行推送
                    {
                        PushReceive(item);
                    }
                }
            }
        }

        #region ProChatFee_Call_Method
        /// <summary>
        /// 处理聊天计费业务
        /// </summary>
        /// <param name="chatFeeBody"></param>
        /// <param name="nodeId"></param>
        /// <param name="pushPList"></param>
        /// <returns></returns>
        private (bool, string) HandChatFeeBusiness(ChatFee chatFee, int nodeId, List<PushPDian> pushPList)
        {
            var result = (false, "");
            using (var connection = OracleHelper.GetConnection())
            {
                connection.Open();
                //开启一个事务
                OracleTransaction transaction = connection.BeginTransaction();
                try
                {
                    TchatFeehis tchatFeehis = AddTchatFeehisBusiness(nodeId, chatFee, transaction);
                    if (tchatFeehis == null)
                    {
                        transaction.Rollback();
                        return (false, "聊天计费失败,请联系管理员!");
                    }
                    var transferId = Guid.NewGuid().ToString();
                    if (chatFee.Body.ReceiveType == 1)//个人
                    {
                        //发送者计费操作
                        result = SendFeeOperation(nodeId, tchatFeehis.Amount, 1, transaction, transferId);
                        //接收者计费操作
                        if (result.Item1)
                            result = ReceiverFeeOperation(tchatFeehis.Receiver, tchatFeehis.Amount, transaction, pushPList, transferId);
                    }
                    else if (chatFee.Body.ReceiveType == 2)//群
                    {
                        List<int> userIdList = pXinDb.GetGroupUserId(tchatFeehis.Groupid).ToList();
                        if (userIdList.Count == 0)
                        {
                            transaction.Rollback();
                            return (false, "群组不存在,请创建一个群组!");
                        }
                        //发送者
                        result = SendFeeOperation(nodeId, tchatFeehis.Amount, userIdList.Count(), transaction, transferId);
                        //接收者
                        if (result.Item1)
                        {
                            foreach (var userId in userIdList.Where(p => p != nodeId))
                            {
                                result = ReceiverFeeOperation(userId, tchatFeehis.Amount /** (userIdList.Count() + 1)*/, transaction, pushPList, transferId);
                                if (!result.Item1)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else if (chatFee.Body.ReceiveType == 3)
                    {
                        List<int> useridList = new List<int>();
                        var userids = chatFee.Body.Receiver.Split(',');
                        int validUseridCount = 0;//有效消息接收者ID数量
                        foreach (var item in userids)
                        {
                            try
                            {
                                useridList.Add(Convert.ToInt32(item));
                            }
                            catch (Exception) { }
                        }
                        if (useridList.Count == 0)
                        {
                            transaction.Rollback();
                            return (false, "参数格式错误：Receiver");
                        }
                        //消息接收者获得金额
                        foreach (var userId in useridList.Where(p => p != nodeId))
                        {
                            result = ReceiverFeeOperation(userId, tchatFeehis.Amount /** (userIdList.Count() + 1)*/, transaction, pushPList, transferId);
                            if (result.Item1)
                                validUseridCount++;
                            else break;
                        }
                        //消息发送者扣除金额
                        result = SendFeeOperation(nodeId, tchatFeehis.Amount, validUseridCount + 1, transaction, transferId);
                    }
                    if (result.Item1)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.MessageInfo(ex.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 聊天扣费表添加业务
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="chatFeeBody"></param>
        /// <param name="pXinDb"></param>
        /// <returns></returns>
        private TchatFeehis AddTchatFeehisBusiness(int nodeId, ChatFee chatFee, OracleTransaction transaction)
        {
            TchatFeehis tchatFeehis = new TchatFeehis();
            try
            {
                var feeRuleArray = pXinDb.GetFeeRules(81127, "FeeRules").Propertyvalue.Split('-');

                tchatFeehis.Nodeid = nodeId;
                tchatFeehis.Feetype = Convert.ToInt32(chatFee.Body.FeeType);
                tchatFeehis.Businesstype = Convert.ToInt32(chatFee.Body.BusinessType);
                tchatFeehis.Num = chatFee.Body.Num;
                for (int i = 0; i < feeRuleArray.Length; i++)
                {
                    if (tchatFeehis.Businesstype == i + 1)//匹配到计费规则
                    {
                        tchatFeehis.Amount = -(tchatFeehis.Num * Convert.ToDecimal(feeRuleArray[i]) * chatFee.Body.Rate);//根据规则计算金额
                        if (tchatFeehis.Amount == 0)
                        {
                            Log.MessageInfo($"匹配到计费规则失败,nodeId={nodeId},tchatFeehis.Num={tchatFeehis.Num},Convert.ToDecimal(feeRuleArray[i])={Convert.ToDecimal(feeRuleArray[i])},chatFee.Body.Rate={chatFee.Body.Rate},tchatFeehis.Amount={tchatFeehis.Amount}");
                            return null;
                        }
                        break;
                    }
                }
                if (chatFee.Body.ReceiveType == 1)
                {
                    tchatFeehis.Receiver = Convert.ToInt32(chatFee.Body.Receiver);
                }
                else if (chatFee.Body.ReceiveType == 2)
                {
                    tchatFeehis.Groupid = Convert.ToInt32(chatFee.Body.Receiver);
                }
                else if (chatFee.Body.ReceiveType == 3)
                {//讨论组
                    tchatFeehis.Groupid = -1;
                }
                tchatFeehis.Sendtime = chatFee.Body.FeeTime;
                tchatFeehis.Status = 1;
                tchatFeehis.Createtime = DateTime.Now;
                tchatFeehis.Remarks = "聊天计费";
                tchatFeehis.Sequenceid = nodeId.ToString() + chatFee.Header.Sequence_Id;
                if (!pXinDb.AddChatFee(tchatFeehis, transaction))
                {
                    Log.MessageInfo("聊天计费AddChatFee失败，nodeId" + nodeId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.MessageInfo("聊天计费AddChatFee失败，" + ex.ToString());
                return null;
            }
            return tchatFeehis;
        }
        /// <summary>
        /// 发送者扣费操作
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="pXinDb"></param>
        /// <param name="amount">金额</param>
        /// <param name="personCount">金额</param>
        /// <param name="isGorup">是否为群发送</param>
        private (bool, string) SendFeeOperation(int nodeId, decimal amount, int personCount, OracleTransaction transaction, string transferId)
        {
            var userInfo = pXinDb.GetUserInfoByNodeId(nodeId);
            if (userInfo != null)
            {
                if (userInfo.V < Math.Abs(amount * personCount))
                {
                    Log.MessageInfo("聊天计费发送者" + nodeId + "的v点不足,扣费失败");
                    return (false, "v点不足,请充值后再发送!");
                }
                else
                {
                    var amountBeforeV = userInfo.V;
                    var amountBeforeP = userInfo.P;
                    userInfo.V = amount * personCount;
                    userInfo.P = Math.Abs(amount);
                    if (userInfo.V == 0 || userInfo.P == 0)
                    {
                        Log.MessageInfo($"v点获取失败，nodeId：{nodeId},v点：{userInfo.V},p点：{userInfo.P},amount:{amount},personCount:{personCount}");
                        return (false, "聊天计费失败,请联系管理员!");
                    }
                    var vChangeHis = CreateAmountChangeHis(nodeId, 1, userInfo.V, 6, transferId, $"聊天扣减", amountBeforeV, amountBeforeV + userInfo.V);
                    var pChangeHis = CreateAmountChangeHis(nodeId, 2, userInfo.P, 6, transferId, $"聊天增加", amountBeforeP, amountBeforeP + userInfo.P);
                    Log.MessageInfo("聊天计费测试一波，userInfo.V：" + userInfo.V);
                    Log.MessageInfo("聊天计费测试二波，vChangeHis：" + JsonConvert.SerializeObject(vChangeHis));
                    var falg1 = pXinDb.AddTpxinAmountChangeHis(vChangeHis, transaction);
                    var falg2 = pXinDb.AddTpxinAmountChangeHis(pChangeHis, transaction);
                    if (!pXinDb.UpdateTpxinUserinfo(userInfo, transaction)
                        || !falg1
                        || !falg2)
                    {
                        Log.MessageInfo("聊天计费UpdateTpxinUserinfo失败，nodeId" + nodeId);
                        return (false, "聊天计费失败,请联系管理员!");
                    }
                    return (true, null);
                }
            }
            else
            {
                Log.MessageInfo("聊天计费发送者：" + nodeId + "不存在");
                return (false, "找不到您当前信息,请重新登录再尝试!");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        /// <param name="reason"></param>
        /// <param name="transferId"></param>
        /// <param name="remarks"></param>
        /// <param name="amountBefore"></param>
        /// <param name="amountAfer"></param>
        /// <returns></returns>
        private TpxinAmountChangeHis CreateAmountChangeHis(int nodeId, int type, decimal amount, int reason, string transferId, string remarks, decimal amountBefore, decimal amountAfer)
        {
            return new TpxinAmountChangeHis()
            {
                Nodeid = nodeId,
                Typeid = type,
                Amount = amount,
                Reason = reason,
                Transferid = transferId,
                Createtime = DateTime.Now,
                Remarks = remarks,
                Amountbefore = amountBefore,
                Amountafter = amountAfer
            };
        }

        /// <summary>
        /// 接收者计费操作
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="pXinDb"></param>
        /// <param name="amount">金额</param>
        /// <param name="isGorup">是否为群发送</param>
        private (bool, string) ReceiverFeeOperation(int receiverId, decimal amount, OracleTransaction transaction, List<PushPDian> pushPList, string transferId)
        {
            var userInfo = pXinDb.GetUserInfoByNodeId(receiverId);
            var pushPDianResp = new PushPDian { NodeId = receiverId, PDianPush = 1, PDianBalance = userInfo.P + Math.Abs(amount) };
            if (userInfo != null && userInfo.Nodeid > 0)
            {
                var amountBeforeP = userInfo.P;
                userInfo.P = Math.Abs(amount);
                userInfo.V = 0;
                if (userInfo.P == 0)
                {
                    Log.MessageInfo($"ReceiverFeeOperation v点获取失败，nodeId：{receiverId},p点：{userInfo.P},amount:{amount}");
                    return (false, "聊天计费失败,请联系管理员!");
                }
                pushPDianResp.PDianPush = Math.Abs(amount);
                var pChangeHis = CreateAmountChangeHis(receiverId, 2, userInfo.P, 6, transferId, "聊天增加", amountBeforeP, amountBeforeP + userInfo.P);
                var falg = pXinDb.AddTpxinAmountChangeHis(pChangeHis, transaction);
                if (!pXinDb.UpdateTpxinUserinfo(userInfo, transaction)
                    || !falg)
                {
                    Log.MessageInfo("聊天计费UpdateTpxinUserinfo失败，receiverId" + receiverId);
                    return (false, "聊天计费失败,请联系管理员!");
                }
                pushPList.Add(pushPDianResp);
                return (true, null);
            }
            else
            {
                Log.MessageInfo("聊天计费接收者：" + receiverId + "不存在");
                return (false, "找不到您当前信息,请重新登录再尝试!");
            }
        }
        /// <summary>
        /// 给接收者推送
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="pushPDian"></param>
        private void PushReceive(PushPDian pushPDian)
        {
            if (pushPDian != null)
            {
                CommuTcpClient receiveClient = CommuClient.Values.FirstOrDefault(c => c.Nodeid == pushPDian.NodeId);
                if (receiveClient != null)
                {
                    //给接收用户推送P点余额
                    ChatFeePush chatFeePush = new ChatFeePush();
                    chatFeePush.Body.PDianPush = pushPDian.PDianPush;
                    chatFeePush.Body.PDianBalance = pushPDian.PDianBalance;
                    receiveClient.SendData(chatFeePush.ToBytes());
                }
            }
        }
        #endregion
        /// <summary>
        /// 聊天计费推送响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProChatFeePushResp(CommuTcpClient client, byte[] buffer)
        {
            ChatFeePushResp chatFeePushResp = new ChatFeePushResp(buffer);
            OnRaiseMsgRecvEvent(client, chatFeePushResp.ToString());
        }
        /// <summary>
        /// 客户端主动断开连接(登出)请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProLogout(CommuTcpClient client, byte[] buffer)
        {
            Wt_Logout logout = new Wt_Logout(buffer);
            OnRaiseMsgRecvEvent(client, logout.ToString());
            LogoutResp logoutResp = new LogoutResp(logout.Header.Sequence_Id);
            client.SendData(logoutResp.ToBytes());
            //关闭连接
            client.Close("客户端主动发起断开连接");
        }
        /// <summary>
        /// 客户端主动断开连接(登出)响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProWt_LogoutResp(CommuTcpClient client, byte[] buffer)
        {
            LogoutResp logout = new LogoutResp(buffer);
            OnRaiseMsgRecvEvent(client, logout.ToString());
        }

        /// <summary>
        /// 聊天计费倍率设置
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProChatFeeRateSet(CommuTcpClient client, byte[] buffer)
        {
            ChatFeeRateSet chatFeeRateSet = new ChatFeeRateSet(buffer);
            OnRaiseMsgRecvEvent(client, chatFeeRateSet.ToString());
            TchatRate tchatRate = new TchatRate();
            tchatRate.Typeid = chatFeeRateSet.Body.Type;
            tchatRate.Sender = chatFeeRateSet.Body.Sender;//0;
            tchatRate.Receiver = chatFeeRateSet.Body.Receiver;
            tchatRate.Rate = chatFeeRateSet.Body.Rate;
            tchatRate.Createtime = DateTime.Now;
            tchatRate.Remarks = "聊天计费倍率设置";
            var flag = true;
            var result = pXinDb.GetTchatRate(tchatRate.Sender, tchatRate.Receiver, tchatRate.Typeid);
            if (result.Id == 0)
            {
                if (tchatRate.Rate > 1)
                {
                    flag = pXinDb.ChatFeeRateSet(tchatRate);
                }
            }
            else
            {
                flag = pXinDb.UpdateChatFeeRate(tchatRate);
            }
            tchatRate.Rate = flag == false ? result.Rate : tchatRate.Rate;
            ChatFeeRateSetResp chatFeeRateSetResp = new ChatFeeRateSetResp(chatFeeRateSet.Header.Sequence_Id);
            chatFeeRateSetResp.Body.Status = flag == true ? (uint)1 : 0;
            chatFeeRateSetResp.Body.Rate = tchatRate.Rate;
            client.SendData(chatFeeRateSetResp.ToBytes());
            //推送
            RateSetPush(client.Nodeid, chatFeeRateSet, tchatRate);
        }

        #region Rate_Set_Push
        private void RateSetPush(int nodeId, ChatFeeRateSet chatFeeRateSet, TchatRate tchatRate)
        {
            if (chatFeeRateSet.Body.Type == 1 && chatFeeRateSet.Body.Sender > 0)//当为私聊且在聊天中时推送
            {
                var ratePush = new PushRate() { NodeId = chatFeeRateSet.Body.Sender, Rate = tchatRate.Rate, SNodeId = chatFeeRateSet.Body.Receiver };
                var pxinUser = pXinDb.GetReginfoByNodeid(chatFeeRateSet.Body.Sender);
                if (pxinUser.Isenterprise == 3)//当用户是达人时
                {
                    ratePush.Rate = 1;
                }
                RateSetPush(ratePush);
            }
            else //接收者类型为群时
            {
                List<PushRate> pushRates = new List<PushRate>();
                var userIds = pXinDb.GetGroupUserId(chatFeeRateSet.Body.Receiver).Where(p => p != nodeId).ToList();
                if (chatFeeRateSet.Body.Type == 3)//通用倍率
                {
                    userIds = pXinDb.GetTchatFriendNodeIds(chatFeeRateSet.Body.Receiver);
                }
                userIds.ForEach(p =>
                {
                    pushRates.Add(new PushRate
                    {
                        NodeId = p,
                        Rate = tchatRate.Rate,
                        SNodeId = chatFeeRateSet.Body.Receiver
                    });
                });
                foreach (var item in pushRates)//遍历群成员进行推送
                {
                    RateSetPush(item);
                }
            }
        }
        private void RateSetPush(PushRate pushRate)
        {
            CommuTcpClient receiveClient = CommuClient.Values.FirstOrDefault(c => c.Nodeid == pushRate.NodeId);
            if (receiveClient != null && pushRate != null)
            {
                //给聊天中的用户推送倍率
                ChatFeeRateSetPush chatFeeRateSetPush = new ChatFeeRateSetPush();
                chatFeeRateSetPush.Body.Rate = pushRate.Rate;
                chatFeeRateSetPush.Body.SNodeId = pushRate.SNodeId;
                receiveClient.SendData(chatFeeRateSetPush.ToBytes());
            }
        }
        #endregion
        /// <summary>
        /// 聊天计费倍率设置推送响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProChatFeeRateSetPushResp(CommuTcpClient client, byte[] buffer)
        {
            ChatFeeRateSetPushResp chatFeeRateSetPushResp = new ChatFeeRateSetPushResp(buffer);
            OnRaiseMsgRecvEvent(client, chatFeeRateSetPushResp.ToString());
        }

        /// <summary>
        /// 聊天计费倍率查询
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        private void ProChatFeeRateQuery(CommuTcpClient client, byte[] buffer)
        {
            ChatFeeRateQuery chatFeeRateQuery = new ChatFeeRateQuery(buffer);
            OnRaiseMsgRecvEvent(client, chatFeeRateQuery.ToString());
            var tchatReceiverRate = pXinDb.GetTchatRate(chatFeeRateQuery.Body.Sender, chatFeeRateQuery.Body.Receiver, chatFeeRateQuery.Body.Type);
            var tchatSenderRate = new TchatRate();
            if (chatFeeRateQuery.Body.Type == 1)
            {
                if (tchatReceiverRate.Id == 0)//没有设置私聊倍率
                    tchatReceiverRate = pXinDb.GetTchatRate(0, chatFeeRateQuery.Body.Receiver, 3);
                tchatSenderRate = pXinDb.GetTchatRate(chatFeeRateQuery.Body.Receiver, chatFeeRateQuery.Body.Sender, chatFeeRateQuery.Body.Type);
                if (tchatSenderRate.Id == 0)//没有设置私聊倍率
                    tchatSenderRate = pXinDb.GetTchatRate(0, chatFeeRateQuery.Body.Sender, 3);
                var pxinUser = pXinDb.GetReginfoByNodeid(chatFeeRateQuery.Body.Receiver);
                tchatSenderRate.Rate = pxinUser.Isenterprise == 3 ? 1 : tchatSenderRate.Rate;//当用户是达人时
                pxinUser = pXinDb.GetReginfoByNodeid(chatFeeRateQuery.Body.Sender);
                tchatReceiverRate.Rate = pxinUser.Isenterprise == 3 ? 1 : tchatReceiverRate.Rate;//当对方用户是达人时
            }
            else
            {
                tchatSenderRate.Rate = tchatReceiverRate.Rate;
                tchatSenderRate.Id = tchatReceiverRate.Id;
            }
            ChatFeeRateQueryResp chatFeeRateQueryResp = new ChatFeeRateQueryResp(chatFeeRateQuery.Header.Sequence_Id);
            chatFeeRateQueryResp.Body.Type = chatFeeRateQuery.Body.Type;
            chatFeeRateQueryResp.Body.Sender = chatFeeRateQuery.Body.Type == 1 ? chatFeeRateQuery.Body.Sender : chatFeeRateQuery.Body.Receiver;
            chatFeeRateQueryResp.Body.ReceiverRate = tchatReceiverRate.Id == 0 ? 1 : tchatReceiverRate.Rate;
            chatFeeRateQueryResp.Body.SenderRate = tchatSenderRate.Id == 0 ? 1 : tchatSenderRate.Rate;
            var userInfo = pXinDb.GetUserInfoByNodeId(client.Nodeid);
            chatFeeRateQueryResp.Body.VDianBalance = userInfo.V;
            chatFeeRateQueryResp.Body.PDianBalance = userInfo.P;
            client.SendData(chatFeeRateQueryResp.ToBytes());
        }

        #endregion
    }
    public class ResultMsg
    {
        public bool Success;
        public string Message = "成功";
        public int Result;
    }
}
