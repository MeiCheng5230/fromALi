using Common.Mvc;
using PXin.DB;
using PXin.Facade.SignalR.Models;
using PXin.Facade.SignalR.Protocal;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.InternalFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatHubFacade
    {
        Log logger = new Log(typeof(ChatHubFacade));
        private readonly PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="nodeId"></param>
        /// <param name="pDian"></param>
        /// <param name="vDian"></param>
        /// <returns></returns>
        public (bool, string, int) LoginValidate(Login login, out int nodeId, out decimal pDian, out decimal vDian)
        {
            nodeId = 0;
            pDian = 0;
            vDian = 0;
            string appKey = AppConfig.Api_SignString;
            if (string.IsNullOrEmpty(appKey))
                appKey = "af83f787e8911dea9b3bf677746ebac9";
            if (!Helper.CheckMd5(login.Body.NodeCode + login.Body.Pwd, login.Body.Sign, appKey))
            {
                return (false, "签名错误", 0);
            }
            var userInfo = db.TnetReginfoSet.FirstOrDefault(p => p.Nodecode == login.Body.NodeCode);
            if (userInfo == null)
            {
                return (false, "账号不存在", 0);
            }
            nodeId = userInfo.Nodeid;
            var curTime = DateTime.Now;
            var lockUser = db.TnetLockuserSet.FirstOrDefault(p => p.Nodeid == userInfo.Nodeid && p.Unlocktime > curTime && p.Locktype == 1);
            if (lockUser != null)
            {
                return (false, "用户已冻结", 0);
            }
            var pxinUserInfo = db.TpxinUserinfoSet.FirstOrDefault(p => p.Nodeid == userInfo.Nodeid);
            vDian = pxinUserInfo.V;
            pDian = pxinUserInfo.P;
            return (true, "", 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public TchatRate GetTchatRate(int sender, int receiver, int typeId)
        {
            return db.TchatRateSet.FirstOrDefault(p => p.Sender == sender && p.Receiver == receiver && p.Typeid == typeId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public TpxinUserinfo GetUserInfoByNodeId(int nodeId)
        {
            return db.TpxinUserinfoSet.FirstOrDefault(p => p.Nodeid == nodeId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public TchatFeehis GetChatFeeHisBySeq(string seq)
        {
            return db.TchatFeehisSet.FirstOrDefault(p => p.Sequenceid == seq);
        }


        /// <summary>
        /// 处理聊天计费业务
        /// </summary>
        /// <param name="chatFee"></param>
        /// <param name="nodeId"></param>
        /// <param name="pushPList"></param>
        /// <returns></returns>
        public (bool, string) HandChatFeeBusiness(ChatFee chatFee, int nodeId, List<PushPDian> pushPList)
        {
            var result = (false, "");
            //开启一个事务
            db.BeginTransaction();
            try
            {
                TchatFeehis tchatFeehis = AddTchatFeehisBusiness(nodeId, chatFee);
                if (tchatFeehis == null)
                {
                    db.Rollback();
                    return (false, "聊天计费失败,请联系管理员!");
                }
                var transferId = Guid.NewGuid().ToString();
                if (chatFee.Body.ReceiveType == 1)//个人
                {
                    //发送者计费操作
                    result = SendFeeOperation(nodeId, tchatFeehis.Amount, 1, transferId);
                    //接收者计费操作
                    if (result.Item1)
                        result = ReceiverFeeOperation(tchatFeehis.Receiver, tchatFeehis.Amount, pushPList, transferId);
                }
                else if (chatFee.Body.ReceiveType == 2)//群
                {
                    var userIdList = db.TchatGroupUserSet.Where(p => p.Groupid == tchatFeehis.Groupid).Select(p => p.Userid).ToList();
                    if (userIdList.Count == 0)
                    {
                        db.Rollback();
                        return (false, "群组不存在,请创建一个群组!");
                    }
                    //发送者
                    result = SendFeeOperation(nodeId, tchatFeehis.Amount, userIdList.Count(), transferId);
                    //接收者
                    if (result.Item1)
                    {
                        foreach (var userId in userIdList.Where(p => p != nodeId))
                        {
                            result = ReceiverFeeOperation(userId, tchatFeehis.Amount, pushPList, transferId);
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
                        db.Rollback();
                        return (false, "参数格式错误：Receiver");
                    }
                    //消息接收者获得金额
                    foreach (var userId in useridList.Where(p => p != nodeId))
                    {
                        result = ReceiverFeeOperation(userId, tchatFeehis.Amount, pushPList, transferId);
                        if (result.Item1)
                            validUseridCount++;
                    }
                    //消息发送者扣除金额
                    result = SendFeeOperation(nodeId, tchatFeehis.Amount, validUseridCount + 1, transferId);
                }
                if (result.Item1)
                {
                    db.Commit();
                }
                else
                {
                    db.Rollback();
                }
            }
            catch (Exception ex)
            {
                db.Rollback();
                logger.Info(ex.ToString());
            }
            return result;
        }
        /// <summary>
        /// 聊天扣费表添加业务
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="chatFee"></param>
        /// <returns></returns>
        private TchatFeehis AddTchatFeehisBusiness(int nodeId, ChatFee chatFee)
        {
            TchatFeehis tchatFeehis = new TchatFeehis();
            try
            {
                var feeRuleArray = db.TappConfigSet.FirstOrDefault(p => p.Sid == 81127 && p.Propertyname == "FeeRules").Propertyvalue.Split('-');

                tchatFeehis.Nodeid = nodeId;
                tchatFeehis.Feetype = Convert.ToInt32(chatFee.Body.FeeType);
                tchatFeehis.Businesstype = Convert.ToInt32(chatFee.Body.BusinessType);
                tchatFeehis.Num = chatFee.Body.Num;
                for (int i = 0; i < feeRuleArray.Length; i++)
                {
                    if (tchatFeehis.Businesstype == i + 1)//匹配到计费规则
                    {
                        tchatFeehis.Amount = -(tchatFeehis.Num * Convert.ToDecimal(feeRuleArray[i]) * chatFee.Body.Rate);//根据规则计算金额
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
                tchatFeehis.Sequenceid = nodeId.ToString() + chatFee.Header.SequenceId;
                db.TchatFeehisSet.Add(tchatFeehis);
                if (db.SaveChanges() <= 0)
                {
                    logger.Info("聊天计费AddChatFee失败，nodeId" + nodeId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Info("聊天计费AddChatFee失败，" + ex.ToString());
                return null;
            }
            return tchatFeehis;
        }

        private (bool, string) SendFeeOperation(int nodeId, decimal amount, int personCount, string transferId)
        {
            var userInfo = db.TpxinUserinfoSet.FirstOrDefault(p => p.Nodeid == nodeId);
            if (userInfo != null)
            {
                if (userInfo.V < Math.Abs(amount * personCount))
                {
                    logger.Info("聊天计费发送者" + nodeId + "的v点不足,扣费失败");
                    return (false, "v点不足,请充值后再发送!");
                }
                else
                {
                    var v = amount * personCount;
                    var p = Math.Abs(amount);
                    userInfo.V += v;
                    userInfo.P += p;
                    var vChangeHis = CreateAmountChangeHis(nodeId, 1, v, 6, transferId, $"聊天扣减");
                    var pChangeHis = CreateAmountChangeHis(nodeId, 2, p, 6, transferId, $"聊天增加");
                    db.TpxinAmountChangeHisSet.Add(vChangeHis);
                    db.TpxinAmountChangeHisSet.Add(pChangeHis);
                    if (db.SaveChanges() <= 0)
                    {
                        logger.Info("聊天计费UpdateTpxinUserinfo失败，nodeId" + nodeId);
                        return (false, "聊天计费失败,请联系管理员!");
                    }
                    return (true, null);
                }
            }
            else
            {
                logger.Info("聊天计费发送者：" + nodeId + "不存在");
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
        /// <returns></returns>
        private TpxinAmountChangeHis CreateAmountChangeHis(int nodeId, int type, decimal amount, int reason, string transferId, string remarks)
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
            };
        }

        /// <summary>
        /// 接收者计费操作
        /// </summary>
        /// <param name="receiverId"></param>
        /// <param name="amount"></param>
        /// <param name="pushPList"></param>
        /// <param name="transferId"></param>
        /// <returns></returns>
        private (bool, string) ReceiverFeeOperation(int receiverId, decimal amount, List<PushPDian> pushPList, string transferId)
        {
            var userInfo = db.TpxinUserinfoSet.FirstOrDefault(p => p.Nodeid == receiverId);
            var pushPDianResp = new PushPDian { NodeId = receiverId, PDianPush = 1, PDianBalance = userInfo.P + Math.Abs(amount) };
            if (userInfo != null && userInfo.Nodeid > 0)
            {
                var p = Math.Abs(amount);
                userInfo.P += p;
                pushPDianResp.PDianPush = Math.Abs(amount);
                var pChangeHis = CreateAmountChangeHis(receiverId, 2, p, 6, transferId, "聊天增加");
                db.TpxinAmountChangeHisSet.Add(pChangeHis);
                if (db.SaveChanges() <= 0)
                {
                    logger.Info("聊天计费UpdateTpxinUserinfo失败，receiverId" + receiverId);
                    return (false, "聊天计费失败,请联系管理员!");
                }
                pushPList.Add(pushPDianResp);
                return (true, null);
            }
            else
            {
                logger.Info("聊天计费接收者：" + receiverId + "不存在");
                return (false, "找不到您当前信息,请重新登录再尝试!");
            }
        }
    }
}
