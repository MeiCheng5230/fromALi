using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.Models;
using io.rong;
using MvcPaging;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.Models;
using PXin.Model;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;
using Winner.EncodeDecode;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatFacade()
        {
            db.Database.CommandTimeout = null;
        }
        #region 刷新用户信息

        /// <summary>
        /// 刷新用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool RefreshUserInfo(Reqbase req)
        {
            TchatUser chatUser = db.TchatUserSet.First(c => c.Nodeid == req.Nodeid);
            if (chatUser == null)
            {
                Alert("查找用户失败");
                return false;
            }
            string appPhoto = GetAppPhoto(req.Nodeid);
            RongResult result = RongCloudServer.RefreshUser(AppConfig.AppKey, AppConfig.AppSecret, req.Nodeid.ToString(), chatUser.Nickname, appPhoto);
            log.Info("刷新用户信息结果,nodeid=" + result.Result);
            if (!result.Result)
            {
                Alert("刷新融云用户信息失败");
                return false;
            }
            Alert("成功", 1);
            return true;
        }
        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public string GetAppPhoto(int nodeid)
        {
            TnetUserphoto Photo = db.TnetUserphotoSet.FirstOrDefault(a => a.Nodeid == nodeid);
            if (Photo == null)
            {
                return AppConfig.DefaultPhoto;
            }
            else
            {
                return Photo.Appphoto;
            }
        }
        #endregion

        #region 获取本人用户信息
        /// <summary>
        /// 获取本人用户信息
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="gtclientid"></param>
        /// <param name="deviceToken"></param>
        /// <returns></returns>
        public ChatUserDto PXinLogin(int nodeid, string gtclientid, string deviceToken)
        {
            //log.Info($"PXinLoginStart:nodeid={nodeid}&gtclientid={gtclientid}&deviceToken={deviceToken}");
            TnetReginfo regInfo = PxinCache.GetRegInfo(nodeid);
            if (string.IsNullOrEmpty(gtclientid))
            {
                gtclientid = string.Empty;
                //log.Info(string.Format("PXin用户登录,nodeid={0},nodecode={1},nodename={2},个推Clientid为空", regInfo.Nodeid, regInfo.Nodecode, regInfo.Nodename));
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                deviceToken = string.Empty;
            }

            ChatUserDto pxUser = GetPxUser(regInfo.Nodeid);
            bool isFirstLogin = false;
            if (pxUser == null)
            {
                TchatUser cu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
                if (cu == null)
                {
                    log.Info("注册P信用户 Nodecode=" + regInfo.Nodecode);
                    cu = RegUser(regInfo, gtclientid, deviceToken);
                    pxUser = GetPxUser(regInfo.Nodeid);
                    if (pxUser == null)
                    {
                        Alert("注册P信用户失败");
                        return pxUser;
                    }
                    isFirstLogin = true;
                }
                else
                {
                    Alert("用户账号异常");
                    return pxUser;
                }
                pxUser = GetPxUser(regInfo.Nodeid);
            }


            bool result = AddUserInfo(regInfo.Nodeid);

            if (string.IsNullOrEmpty(pxUser.GtClientid))
            {
                pxUser.GtClientid = string.Empty;
            }
            if (string.IsNullOrEmpty(pxUser.DeviceToken))
            {
                pxUser.DeviceToken = string.Empty;
            }
            if (!pxUser.GtClientid.Equals(gtclientid, StringComparison.OrdinalIgnoreCase)
                || !pxUser.DeviceToken.Equals(deviceToken, StringComparison.OrdinalIgnoreCase))
            {
                TchatUser tu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == pxUser.NodeId);
                tu.Gtclientid = gtclientid;
                tu.Devicetoken = deviceToken;
                TnetReginfoExt ext = db.TnetReginfoExtSet.FirstOrDefault(c => c.Nodeid == pxUser.NodeId);
                if (ext != null)
                {
                    ext.Gtclientid = gtclientid;
                    ext.Devicetoken = deviceToken;
                }

                db.SaveChanges();

                //同步用户信息
                db.SyncChatUserFull(regInfo.Nodeid);

                pxUser.GtClientid = gtclientid;
                pxUser.DeviceToken = deviceToken;
            }
            if (isFirstLogin)
            {
                //个推
                int[] friendids = db.TchatFriendSet.Where(c => c.Friendnodeid == pxUser.NodeId).Select(c => c.Mynodeid).ToArray();
                if (friendids.Length > 0)
                {
                    List<ChatUserDto> tus = GetPxUsers(regInfo, friendids);
                    foreach (var item in tus)
                    {
                        SendFriendQueust(item, pxUser, string.Empty);
                    }
                }
            }
            //log.Info($"PXinLoginEnd:nodeid={nodeid}&gtclientid={gtclientid}&deviceToken={deviceToken}");

            if (pxUser == null || string.IsNullOrEmpty(pxUser.Token))
            {
                return null;
            }
            //pxUser.Config = new PxinConfig { DefaultPublic = GetPublicIdsByNodeid(regInfo.Nodeid) };
            return pxUser;
        }


        /// <summary>
        /// 添加信友圈信息
        /// </summary>
        /// <param name="Nodeid"></param>
        /// <returns></returns>
        public bool AddUserInfo(int Nodeid)
        {
            TpxinUserinfo userinfo = db.TpxinUserinfoSet.FirstOrDefault(a => a.Nodeid == Nodeid);
            if (userinfo == null)
            {
                decimal.TryParse(AppConfig.RegisterGiveVDian, out decimal giftV);
                db.TpxinUserinfoSet.Add(new TpxinUserinfo
                {
                    Nodeid = Nodeid,
                    Backpic = "",
                    Createtime = DateTime.Now,
                    Up = 0,
                    Down = 0,
                    P = 0,
                    V = giftV,
                    Remarks = ""
                });
                TpxinPayhis payhis = new TpxinPayhis
                {
                    Createtime = DateTime.Now,
                    Infoid = 0,
                    Typeid = 1,
                    Nodeid = Nodeid,
                    Tonodeid = 0,
                    Price = (int)giftV,
                    Remarks = $"注册送{giftV}点"
                };
                //2.修改用户信息表的v点
                db.TpxinPayhisSet.Add(payhis);
                db.TpxinAmountChangeHisSet.Add(new TpxinAmountChangeHis
                {
                    Amount = giftV,
                    Nodeid = Nodeid,
                    Reason = 8,
                    Transferid = Guid.NewGuid().ToString(),
                    Typeid = 1,
                    Createtime = DateTime.Now,
                    Remarks = "注册送V点",
                    Amountbefore = 0,
                    Amountafter = giftV
                });
                if (db.SaveChanges() <= 0)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 注册P信用户
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="gtclientid"></param>
        /// <param name="deviceToken"></param>
        /// <returns></returns>
        public TchatUser RegUser(TnetReginfo regInfo, string gtclientid, string deviceToken)
        {
            string appPhoto = GetAppPhoto(regInfo.Nodeid);
            RongCloudServer rcs = new RongCloudServer();
            RongResultToken result = RongCloudServer.GetToken(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), regInfo.Nodename, appPhoto);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                log.Info(result.errorMessage);
                return null;
            }
            TchatUser chatUser = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
            if (chatUser != null)
            {
                Alert($"注册失败,用户{regInfo.Nodecode}已存在");
                log.Info($"注册失败,用户{regInfo.Nodecode}已存在");
                return null;
            }
            TchatUser cu = new TchatUser();
            cu.Nodeid = regInfo.Nodeid;
            cu.Token = result.token;
            cu.Gtclientid = gtclientid;
            cu.Devicetoken = deviceToken;
            cu.Nickname = string.Empty;
            db.TchatUserSet.Add(cu);
            if (db.SaveChanges() > 0)
            {
                //检查群组信息，加入群组
                int[] groupids = db.TchatGroupUserSet.Where(a => a.Userid == regInfo.Nodeid).Select(a => a.Groupid).ToArray();
                List<TchatGroup> groups = db.TchatGroupSet.Where(a => groupids.Contains(a.Id) && a.Groupstate == 1).OrderByDescending(a => a.Createtime).ToList();
                if (groups != null && groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        RongResult result2 = RongCloudServer.JoinGroup(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), group.Id.ToString(), group.Groupname);
                        if (!result.Result)
                        {
                            Alert("加入群组失败,code=1");
                            log.Info(result.errorMessage);
                            return null;
                        }
                    }
                }
                //同步用户信息
                db.SyncChatUserFull(regInfo.Nodeid);
                return cu;
            }
            return null;
        }

        /// <summary>
        /// 获取P信用户
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public ChatUserDto GetPxUser(int nodeId)
        {
            var query = from user in db.TchatUserFullSet.Where(a => a.Nodeid == nodeId)
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == nodeId).Select(a => new { a.Allowviewmedynamic, a.Viewhedynamic, a.Friendnodeid }) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            // Remarks = user.,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取PXin用户
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="userids">用户nodeid列表</param>
        /// <returns></returns>
        public List<ChatUserDto> GetPxUsers(TnetReginfo regInfo, int[] userids)
        {
            log.Info("GetPxUsers userids=" + userids);
            var query = from user in db.TchatUserFullSet.Where(a => userids.Contains(a.Nodeid))
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == regInfo.Nodeid) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            return query.ToList();
        }

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="questUser"></param>
        /// <param name="destUser"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool SendFriendQueust(ChatUserDto questUser, ChatUserDto destUser, string remarks)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = string.Format(@"{0}请求添加好友，附加消息：{1}", questUser.NodeName, remarks);
            string extra = JsonConvert.SerializeObject(questUser);
            string content = JsonConvert.SerializeObject(new
            {
                operation = "addfriend",
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = "添加好友"
            });

            //发送添加好友请求消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            //向cas发送添加好友请求消息
            Facade.Bus.EventBus.GetInstance().Publish(new Facade.Bus.Event.AddFriendNoticeEvent(content));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendUser"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void GtPush(ChatUserDto friendUser, string title, string content)
        {
            GtPush(friendUser.NodeId, friendUser.GtClientid, friendUser.DeviceToken, title, content);
        }
        /// <summary>
        /// 
        /// </summary>
        public void GtPush(int nodeid, string gtClientid, string deviceToken, string title, string content)
        {
            if (string.IsNullOrEmpty(gtClientid))
            {
                log.Info(nodeid + "暂时没有GtClientId");
                return;
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                string logStr = "andriod发送目标：" + nodeid + "," + gtClientid;
                GtPushAndriod(gtClientid, title, content, logStr);
            }
            else
            {
                string logStr = "IOS发送目标：" + nodeid + "," + gtClientid;
                GtPushIOSTransmission(gtClientid, deviceToken, title, content, logStr);
            }
        }

        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="db1"></param>
        /// <param name="nodeid"></param>
        /// <param name="content"></param>
        /// <param name="url"></param>
        /// <param name="title"></param>
        public void AddMessage(PXinContext db1,int nodeid,string content,string url,string title)
        {
            db1.TueMessageSet.Add(new TueMessage
            {
                Content = content,
                Createtime = DateTime.Now,
                Jumpurl = url,
                Nodeid = nodeid,
                Status = 0,
                Title = title,
                Modifytime=DateTime.Now,
                Typeid=1,
                Fkid=0
            });

            if (db1.SaveChanges()<=0)
            {
                log.Info("增加消息失败:" +content+ nodeid);
            }
            
        }

        private void GtPushIOSTransmission(string gtClientid, string deviceToken, string title, string content, string logStr)
        {
            IGtPush push = null;
            TransmissionTemplate template = null;
            // 单推消息模型
            SingleMessage message = null;

            //{"taskId":"OSS-0406_53638fd9d469f037637e0da0e47efa5f","result":"ok","status":"successed_online"}
            com.igetui.api.openservice.igetui.Target target = null;
            try
            {
                push = new IGtPush(AppConfig.GtHost, AppConfig.GtAppKey, AppConfig.GtMasterSecret);

                template = TransmissionTemplateFriend(title, content);

                message = new SingleMessage();
                message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
                message.OfflineExpireTime = 1000 * 3600 * 12;     // 离线有效时间，单位为毫秒，可选
                message.Data = template;
                message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

                target = new com.igetui.api.openservice.igetui.Target();
                target.appId = AppConfig.GtAppId;
                target.clientId = gtClientid;

                //log.Info("IOS发送目标：" + friendUser.NodeId + "," + friendUser.NodeCode + "," + friendUser.NodeName + "," + friendUser.GtClientid);
                log.Info(logStr);
                log.Info("IOS发送内容：" + content);
                string pushResult = push.pushMessageToSingle(message, target);
                log.Info("IOS服务端返回结果：" + pushResult);
                GtResult gtResult = JsonConvert.DeserializeObject<GtResult>(pushResult);
                if (gtResult.IsSuccess && !gtResult.IsOnline)
                {
                    //GtPushIOSAPN(friendUser, title, content);
                }
            }
            catch (RequestException e)
            {
                log.Warn(e);
                try
                {
                    String requestId = e.RequestId;
                    string pushResult = push.pushMessageToSingle(message, target);
                    log.Info("服务端返回结果：" + pushResult);
                    GtResult gtResult = JsonConvert.DeserializeObject<GtResult>(pushResult);
                    if (gtResult.IsSuccess && !gtResult.IsOnline)
                    {
                        GtPushIOSAPN(deviceToken, title, content);
                    }
                }
                catch (Exception exp)
                {
                    log.Warn(exp);
                }
            }
        }
        private void GtPushIOSAPN(string deviceToken, string title, string content)
        {
            //APN高级推送
            IGtPush push = new IGtPush(AppConfig.GtHost, AppConfig.GtAppKey, AppConfig.GtMasterSecret);
            APNTemplate template = new APNTemplate();
            APNPayload apnpayload = new APNPayload();
            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
            alertMsg.Body = title;
            alertMsg.ActionLocKey = "ActionLocKey";
            alertMsg.LocKey = "系统消息";
            alertMsg.addLocArg("LocArg");
            alertMsg.LaunchImage = "LaunchImage";

            //IOS8.2支持字段
            alertMsg.Title = title;
            alertMsg.TitleLocKey = "添加好友";
            alertMsg.addTitleLocArg("TitleLocArg");

            apnpayload.AlertMsg = alertMsg;
            apnpayload.Badge = 10;
            apnpayload.ContentAvailable = 1;
            apnpayload.Category = "";
            apnpayload.Sound = "test1.wav";
            apnpayload.addCustomMsg("payload", "payload");
            apnpayload.addCustomMsg("content", content);
            template.setAPNInfo(apnpayload);
            /*单个用户推送接口*/
            SingleMessage singleMessage = new SingleMessage();
            singleMessage.Data = template;

            try
            {
                string pushResult = push.pushAPNMessageToSingle(AppConfig.GtAppId, deviceToken, singleMessage);
                log.Info("服务端返回结果：" + pushResult);
            }
            catch (RequestException e)
            {
                log.Info("服务端返回结果：" + e.ToString());
            }
        }
        private void GtPushAndriod(string gtClientid, string title, string content, string logStr)
        {
            IGtPush push = null;
            //NotificationTemplate template = NotificationTemplateFriend(title, content);
            TransmissionTemplate template = null;
            // 单推消息模型
            SingleMessage message = null;

            //{"taskId":"OSS-0406_53638fd9d469f037637e0da0e47efa5f","result":"ok","status":"successed_online"}
            com.igetui.api.openservice.igetui.Target target = null;
            try
            {
                push = new IGtPush(AppConfig.GtHost, AppConfig.GtAppKey, AppConfig.GtMasterSecret);

                template = TransmissionTemplateFriend(title, content);

                message = new SingleMessage();
                message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
                message.OfflineExpireTime = 1000 * 3600 * 12;     // 离线有效时间，单位为毫秒，可选
                message.Data = template;
                message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

                target = new com.igetui.api.openservice.igetui.Target();
                target.appId = AppConfig.GtAppId;
                target.clientId = gtClientid;
                //log.Info("andriod发送目标：" + friendUser.NodeId + "," + friendUser.NodeCode + "," + friendUser.NodeName + "," + friendUser.GtClientid);
                log.Info(logStr);
                log.Info("andriod发送内容：" + content);
                string pushResult = push.pushMessageToSingle(message, target);
                log.Info("andriod推送服务端返回结果：" + pushResult);
            }
            catch (RequestException e)
            {
                log.Warn(e);
                String requestId = e.RequestId;
                try
                {
                    String pushResult = push.pushMessageToSingle(message, target, requestId);
                    log.Info("andriod推送服务端返回结果：" + pushResult);
                }
                catch (Exception exp)
                {
                    log.Warn(exp);
                }
            }
            catch (Exception e)
            {
                log.Warn(e);
            }
        }
        private TransmissionTemplate TransmissionTemplateFriend(string title, string content)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = AppConfig.GtAppId;
            template.AppKey = AppConfig.GtAppKey;
            template.TransmissionType = "2";            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionContent = content;     //透传内容

            //APN高级推送
            APNPayload apnpayload = new APNPayload();
            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
            alertMsg.Body = title;
            alertMsg.ActionLocKey = "ActionLocKey";
            alertMsg.LocKey = "系统消息";
            alertMsg.addLocArg("LocArg");
            alertMsg.LaunchImage = "LaunchImage";
            //IOS8.2支持字段
            alertMsg.Title = "系统消息";
            alertMsg.TitleLocKey = "添加好友";
            alertMsg.addTitleLocArg("TitleLocArg");

            apnpayload.AlertMsg = alertMsg;
            apnpayload.Badge = 10;
            apnpayload.ContentAvailable = 1;
            //apnpayload.Category = "";
            apnpayload.Sound = "friend.wav";
            apnpayload.addCustomMsg("payload", "payload");
            template.setAPNInfo(apnpayload);

            return template;
        }
        /// <summary>
        /// 获取用户需要自动关注的公众号
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public string GetPublicIdsByNodeid(int nodeid)
        {
            string ret = "3434909";
            if (nodeid == 3434909)
            {
                ret += ",youdianpartner";
            }
            return ret;
            //            PXinContext ctx = HttpContext.Current.GetDbContext<PXinContext>();
            //            OracleParameter pNodeid = new OracleParameter("nodeId", nodeid);
            //            string sql = @"select public_id from tchat_public where public_type = 1
            //union
            //select public_id from tchat_public_user where node_id = :nodeId";
            //            List<string> pubs = ctx.Database.SqlQuery<string>(sql, pNodeid).Distinct().ToList();
            //            string ret = "";
            //            foreach (var item in pubs)
            //            {
            //                ret += item + ",";
            //            }
            //            return ret.Trim(',');
        }

        #endregion

        #region 查询用户信息
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public List<ChatUserDto> QueryUserInfo(QueryUserInfoReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (string.IsNullOrEmpty(req.userids))
            {
                Alert("参数错误");
                return null;
            }
            string[] uids = req.userids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            int[] nodeids = Array.ConvertAll(uids, a =>
            {
                int.TryParse(a, out int temp);
                return temp;
            });
            var query = from user in db.TchatUserFullSet.Where(a => nodeids.Contains(a.Nodeid))
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid).Select(a => new { a.Nickname, a.Allowviewmedynamic, a.Viewhedynamic, a.Friendnodeid }) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        orderby user.Id descending
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            Alert("获取用户成功", 1);
            return query.ToList();

        }
        #endregion

        #region 修改我的基本信息
        /// <summary>
        /// 修改我的基本信息 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateMyInfo(UpdateMyInfoReq req)
        {
            //paramType 1-昵称，2-性名，3-所在地[省市id逗号隔开]，4-个性签名，5-显示真实姓名[0-不显示，1-显示]
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            int nodeid = regInfo.Nodeid;
            TchatUser chatUser = db.TchatUserSet.First(c => c.Nodeid == nodeid);
            if (chatUser == null)
            {
                Alert("查找用户失败");
                return false;
            }
            if (req.paramType == 1)
            {
                chatUser.Nickname = req.paramValue;
            }
            else if (req.paramType == 2)
            {
                chatUser.Sex = req.paramValue;
            }
            else if (req.paramType == 3)
            {
                string[] pc = req.paramValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (pc.Length == 2)
                {
                    int pid = Convert.ToInt32(pc[0]);
                    int cid = Convert.ToInt32(pc[1]);
                    VnetProvince province = db.VnetProvinceSet.FirstOrDefault(c => c.ProvinceId == pid);
                    VnetCity city = db.VnetCitySet.FirstOrDefault(c => c.CityId == cid);
                    if (province == null || city == null)
                    {
                        Alert("参数错误1");
                        return false;
                    }
                    chatUser.Provinceid = province.ProvinceId;
                    chatUser.Provincename = province.ProvinceName;
                    chatUser.Cityid = city.CityId;
                    chatUser.Cityname = city.CityName;
                }
                else
                {
                    Alert("参数错误2");
                    return false;
                }
            }
            else if (req.paramType == 4)
            {
                chatUser.Personalsign = req.paramValue;
            }
            else if (req.paramType == 5)
            {
                chatUser.Showrealname = Convert.ToInt32(req.paramValue);
            }
            if (req.paramType == 1)
            {
                ChatFacade facade = new ChatFacade();
                string appPhoto = facade.GetAppPhoto(regInfo.Nodeid);
                RongResult result = RongCloudServer.RefreshUser(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), chatUser.Nickname, appPhoto);
                if (!result.Result)
                {
                    Alert("刷新融云用户信息失败");
                    return false;
                }
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("修改我的基本信息失败");
                return false;
            }
            //同步用户信息
            db.SyncChatUserFull(nodeid);

            Alert("修改我的基本信息成功", 1);
            return true;
        }
        #endregion

        #region 修改我的昵称
        /// <summary>
        /// 修改我的昵称
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateMyNick(UpdateMyNickReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatUser chatUser = db.TchatUserSet.First(c => c.Nodeid == regInfo.Nodeid);
            if (chatUser == null)
            {
                Alert("查找用户失败");
                return false;
            }
            chatUser.Nickname = req.nickname;
            ChatFacade facade = new ChatFacade();
            string appPhoto = facade.GetAppPhoto(regInfo.Nodeid);
            RongResult result = RongCloudServer.RefreshUser(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), chatUser.Nickname, appPhoto);
            if (!result.Result)
            {
                Alert("刷新融云用户信息失败");
                return false;
            }
            if (db.SaveChanges() <= 0)
            {
                Alert("修改昵称失败");
                return false;
            }
            //同步用户信息
            db.SyncChatUserFull(regInfo.Nodeid);
            Alert("修改昵称成功", 1);
            return true;
        }
        #endregion

        #region 我的好友
        /// <summary>
        /// 我的好友
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ChatUserDto> MyFriend(Reqbase req)
        {
            var query = from user in db.TchatUserFullSet
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        orderby user.Nodeid descending
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            List<int> frinodeids = db.TchatFriendSet.Where(a => a.Friendnodeid == req.Nodeid && a.Friendstatus == 1).Select(a => a.Mynodeid).ToList();
            List<int> mynodeids = db.TchatFriendSet.Where(a => a.Mynodeid == req.Nodeid && a.Friendstatus == 1).Select(a => a.Friendnodeid).ToList();
            int[] nodeids = frinodeids.Union(mynodeids).ToArray();
            List<ChatUserDto> returnList = query.Where(a => nodeids.Contains(a.NodeId)).ToList();
            return returnList;
        }
        #endregion

        #region 查找好友
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatUserDto> QueryFriend(QueryFriendReq req)
        {
            var query = from user in db.TchatUserFullSet
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            if (!string.IsNullOrEmpty(req.key))
            {
                query = query.Where(a => a.NodeCode == req.key || a.NodeName == req.key || a.Mobileno == req.key);
            }
            //query = query.OrderByDescending(a => a.NodeId).Skip(req.pageSize * (req.pageIndex - 1)).Take(req.pageSize);//进行分页
            IPagedList<ChatUserDto> returnList = query.OrderByDescending(a => a.NodeId).ToPagedList(req.pageIndex, req.pageSize);
            return returnList;
        }
        #endregion

        #region 添加好友
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ChatUserDto AddFriend(AddFriendReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo fRegInfo = db.TnetReginfoSet.SingleOrDefault(c => c.Nodecode == req.usercode);
            if (fRegInfo == null)
            {
                Alert("添加的好友不存在");
                return null;
            }
            if (regInfo.Nodeid == fRegInfo.Nodeid)
            {
                Alert("不能添加自已为好友");
                return null;
            }
            ChatUserDto pxUser = null;
            TchatUser cu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == fRegInfo.Nodeid);
            if (cu == null)
            {
                cu = RegUser(fRegInfo, string.Empty, string.Empty);
                pxUser = GetPxUser(fRegInfo.Nodeid);
                if (pxUser == null)
                {
                    Alert("给好友注册P信用户失败");
                    return null;
                }
            }
            else
            {
                pxUser = GetPxUser(fRegInfo.Nodeid);
            }
            if (pxUser == null)
            {
                Alert("好友账号异常");
                return null;
            }

            TchatFriend friend = db.TchatFriendSet.FirstOrDefault(c => (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid && c.Friendstatus == 1)
                || (c.Friendnodeid == fRegInfo.Nodeid && c.Mynodeid == regInfo.Nodeid && c.Friendstatus == 1));
            if (friend != null)
            {
                Alert("您与" + fRegInfo.Nodename + "已经是好友了");
                return null;
            }

            if (cu.IsValidfriend == 0)
            {//加好友无需对方确认
                friend = db.TchatFriendSet.FirstOrDefault(c => (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid && c.Friendstatus <= 1)
                || (c.Friendnodeid == fRegInfo.Nodeid && c.Mynodeid == regInfo.Nodeid && c.Friendstatus <= 1));
                if (friend != null && friend.Friendstatus == 1)
                {
                    Alert(fRegInfo.Nodename + "已经是您的好友了");
                    return null;
                }
                else if (friend != null && friend.Friendstatus == 0)
                {
                    friend.Friendstatus = 1;
                }
                else
                {
                    friend = new TchatFriend();
                    friend.Mynodeid = regInfo.Nodeid;
                    friend.Friendnodeid = fRegInfo.Nodeid;
                    friend.Remarks = req.remarks;
                    friend.Friendstatus = 1;
                    db.TchatFriendSet.Add(friend);
                }
                if (db.SaveChanges() < 1)
                {
                    Alert("添加好友失败");
                }
                Success("好友添加成功", 2);
                ChatUserDto questUser1 = GetPxUser(regInfo.Nodeid);
                SendFriendResponse(questUser1, pxUser, 1, req.remarks, 1);
                return pxUser;
            }

            ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
            friend = db.TchatFriendSet.FirstOrDefault(c => (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid && c.Friendstatus <= 1)
                || (c.Friendnodeid == fRegInfo.Nodeid && c.Mynodeid == regInfo.Nodeid && c.Friendstatus <= 1));
            if (friend != null)
            {
                SendFriendQueust(questUser, pxUser, req.remarks);
                return pxUser;
            }
            friend = new TchatFriend();
            friend.Mynodeid = regInfo.Nodeid;
            friend.Friendnodeid = fRegInfo.Nodeid;
            friend.Remarks = req.remarks;
            db.TchatFriendSet.Add(friend);
            if (db.SaveChanges() < 1)
            {
                Alert("添加好友失败");
            }
            SendFriendQueust(questUser, pxUser, req.remarks);
            return pxUser;
        }
        #endregion

        #region 添加好友确认
        /// <summary>
        /// 添加好友确认
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ChatUserDto AddFriendConfirm(AddFriendConfirmReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo fRegInfo = db.TnetReginfoSet.SingleOrDefault(c => c.Nodecode == req.usercode);
            if (fRegInfo == null)
            {
                Alert("添加的好友不存在");
                return null;
            }
            ChatUserDto pxUser = GetPxUser(fRegInfo.Nodeid);
            if (pxUser == null)
            {
                Alert("用户不存在");
                return null;
            }
            ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
            TchatFriend friend = db.TchatFriendSet.FirstOrDefault(c => (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid && c.Friendstatus == 1)
                || (c.Friendnodeid == fRegInfo.Nodeid && c.Mynodeid == regInfo.Nodeid && c.Friendstatus == 1));
            if (friend != null)
            {
                Alert("您与" + fRegInfo.Nodename + "已经是好友了");
                SendFriendResponse(questUser, pxUser, req.status, req.remarks);
                return null;
            }
            friend = db.TchatFriendSet.FirstOrDefault(c => c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid && c.Friendstatus == 0);
            if (friend == null)
            {
                Alert("好友请求不存在");
                return null;
            }
            friend.Friendstatus = req.status;
            if (db.SaveChanges() < 1)
            {
                Alert("确认好友失败");
                return null;
            }
            SendFriendResponse(questUser, pxUser, req.status, req.remarks);
            return pxUser;
        }

        private bool SendFriendResponse(ChatUserDto questUser, ChatUserDto destUser, int status, string remarks, int isNoValid = 0)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = string.Format(@"{0}{1}了您的好友请求，附加消息：{2}", questUser.NodeName, (status == 1 ? "通过" : "拒绝"), remarks);
            if (isNoValid == 1)
            {
                sysMsg = string.Format(@"{0}已添加您为好友，附加消息：{1}", questUser.NodeName, remarks);
            }
            string extra = JsonConvert.SerializeObject(questUser);
            string operation = status == 1 ? "respfriendpass" : "respfriendrefuse";
            string content = JsonConvert.SerializeObject(new
            {
                operation = operation,
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = "添加好友回复"
            });

            //发送添加好友请求回复消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;
        }
        #endregion

        #region 修改好友信息
        /// <summary>
        /// 修改好友信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateMyFriendInfo(UpdateMyFriendInfoReq req)
        {

            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo fFriend = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.usercode);
            if (fFriend == null || fFriend.Nodeid == req.Nodeid)
            {
                Alert("好友不存在");
                return false;
            }
            TchatFriendNick friendNick = db.TchatFriendNickSet.FirstOrDefault(c => c.Mynodeid == req.Nodeid && c.Friendnodeid == fFriend.Nodeid);
            if (friendNick == null)
            {
                if (req.paramType == 1)
                {
                    db.TchatFriendNickSet.Add(new TchatFriendNick { Mynodeid = req.Nodeid, Friendnodeid = fFriend.Nodeid, Nickname = req.paramValue });
                }
                else if (req.paramType == 2)
                {
                    db.TchatFriendNickSet.Add(new TchatFriendNick { Mynodeid = req.Nodeid, Friendnodeid = fFriend.Nodeid, Allowviewmedynamic = Convert.ToInt32(req.paramValue) });
                }
                else if (req.paramType == 3)
                {
                    db.TchatFriendNickSet.Add(new TchatFriendNick { Mynodeid = req.Nodeid, Friendnodeid = fFriend.Nodeid, Viewhedynamic = Convert.ToInt32(req.paramValue) });
                }

            }
            else
            {
                if (req.paramType == 1)
                {
                    friendNick.Nickname = req.paramValue;
                }
                else if (req.paramType == 2)
                {
                    friendNick.Allowviewmedynamic = Convert.ToInt32(req.paramValue);
                }
                else if (req.paramType == 3)
                {
                    friendNick.Viewhedynamic = Convert.ToInt32(req.paramValue);
                }
            }
            if (db.SaveChanges() < 1)
            {
                Alert("修改失败");
                return false;
            }
            Alert("修改成功", 1);
            return true;
        }
        #endregion

        #region 修改好友备注
        /// <summary>
        /// 修改好友备注
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateMyFriendRemarks(UpdateMyFriendRemarksReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            int nodeid = regInfo.Nodeid;
            TnetReginfo fFriend = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.usercode);
            if (fFriend == null || fFriend.Nodeid == nodeid)
            {
                Alert("好友不存在");
                return false;
            }
            log.Info("UpdateMyFriendRemarks,usercode=" + req.usercode + "," + fFriend.Nodeid);
            TchatFriendNick friendNick = db.TchatFriendNickSet.FirstOrDefault(c => c.Mynodeid == nodeid && c.Friendnodeid == fFriend.Nodeid);
            if (friendNick == null)
            {
                db.TchatFriendNickSet.Add(new TchatFriendNick { Mynodeid = nodeid, Friendnodeid = fFriend.Nodeid, Nickname = req.remarks });
            }
            else
            {
                friendNick.Nickname = req.remarks;
            }
            if (db.SaveChanges() < 1)
            {
                Alert("修改失败");
                return false;
            }
            Alert("修改成功", 1);
            return true;
        }

        #endregion

        #region 删除好友
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteFriend(DeleteFriendReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo fRegInfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.usercode);
            if (fRegInfo == null)
            {
                Alert("好友用户不存在");
                return false;
            }
            db.BeginTransaction();
            //删除好友倍率设置
            var rate = db.TchatRateSet.FirstOrDefault(p => p.Sender == fRegInfo.Nodeid && p.Receiver == req.Nodeid && p.Typeid == 1);
            if (rate != null)
                db.TchatRateSet.Remove(rate);
            List<TchatFriend> friends = db.TchatFriendSet.Where(c => ((c.Mynodeid == regInfo.Nodeid && c.Friendnodeid == fRegInfo.Nodeid) || (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid))).ToList();
            if (friends != null)
            {
                for (int i = 0; i < friends.Count; i++)
                {
                    db.TchatFriendSet.Remove(friends[i]);

                }
                if (db.SaveChanges() < 1)
                {
                    db.Rollback();
                    Alert("删除好友失败");
                    return false;
                }
            }
            else
            {
                db.Rollback();
                Alert("好友不存在");
                return false;
            }
            //删除好友昵称
            List<TchatFriendNick> friendnicks = db.TchatFriendNickSet.Where(c => ((c.Mynodeid == regInfo.Nodeid && c.Friendnodeid == fRegInfo.Nodeid) || (c.Mynodeid == fRegInfo.Nodeid && c.Friendnodeid == regInfo.Nodeid))).ToList();
            if (friends != null && friendnicks.Count > 0)
            {
                for (int i = 0; i < friendnicks.Count; i++)
                {
                    db.TchatFriendNickSet.Remove(friendnicks[i]);
                }
                if (db.SaveChanges() < 1)
                {
                    db.Rollback();
                    Alert("删除好友备注失败");
                    return false;
                }
            }

            //删除推送相互推送的信友圈信息
            int[] infoids = db.TpxinMessageSet.Where(a => a.Nodeid == req.Nodeid).Select(a => a.Infoid).ToArray();
            int[] finfoids = db.TpxinMessageSet.Where(a => a.Nodeid == fRegInfo.Nodeid).Select(a => a.Infoid).ToArray();
            List<TpxinMessageUesr> messageUesrs = db.TpxinMessageUesrSet.Where(a => a.Typeid == 0).Where(a => (infoids.Contains(a.Infoid) && a.Nodeid == fRegInfo.Nodeid) || (finfoids.Contains(a.Infoid) && a.Nodeid == req.Nodeid)).ToList();
            //删除推送相互推送的评论信息
            int[] cominfoids = db.TpxinCommentHisSet.Where(a => a.Nodeid == req.Nodeid).Select(a => a.Hisid).ToArray();
            int[] fcominfoids = db.TpxinCommentHisSet.Where(a => a.Nodeid == fRegInfo.Nodeid).Select(a => a.Hisid).ToArray();
            List<TpxinMessageUesr> commessageUesrs = db.TpxinMessageUesrSet.Where(a => a.Typeid == 1).Where(a => (cominfoids.Contains(a.Infoid) && a.Nodeid == fRegInfo.Nodeid) || (fcominfoids.Contains(a.Infoid) && a.Nodeid == req.Nodeid)).ToList();
            List<TpxinMessageUesr> messageUesrlist = messageUesrs.Union(commessageUesrs).ToList();

            if (messageUesrlist != null && messageUesrlist.Count > 0)
            {
                foreach (var item in messageUesrlist)
                {
                    db.TpxinMessageUesrSet.Remove(item);
                }
                if (db.SaveChanges() < 1)
                {
                    db.Rollback();
                    Alert("删除好友信友圈信息失败");
                    return false;
                }
            }

            ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
            ChatUserDto destUser = GetPxUser(fRegInfo.Nodeid);
            SendDeleteFriendNotice(questUser, destUser);
            db.Commit();
            Alert("删除好友成功", 1);
            return true;
        }

        private bool SendDeleteFriendNotice(ChatUserDto questUser, ChatUserDto destUser)
        {
            //string msgType = "RC:ContactNtf";//添加联系人消息
            string sysMsg = string.Format(@"{0}删除好友", questUser.NodeName);
            string extra = JsonConvert.SerializeObject(questUser);
            string content = JsonConvert.SerializeObject(new
            {
                operation = "deletefriend",
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = "删除好友"
            });
            //发送删除好友消息
            //RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            //if (!result.Result)
            //{
            //    Msg = result.errorMessage;
            //    return false;
            //}
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;
        }
        #endregion

        //----------------------------分割分割-------------------------------------------------------------------------------------------------------------------

        #region 创建群组
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ChatGroupDto CreateGroup(CreateGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (IsFilterWord(req.groupname))
            {
                Alert("群名称中含有敏感词不合法");
                return null;
            }
            string filePath = "";
            if (!string.IsNullOrEmpty(req.grouppic))
            {
                filePath = SaveImage(req.grouppic);
            }
            int count = db.TchatGroupSet.Count(c => c.Creater == regInfo.Nodeid && c.Groupstate == 1);
            if (count >= AppConfig.GroupMaxQuantity)
            {
                Alert("每个用户最多只能创建" + AppConfig.GroupMaxQuantity + ",您已经创建了" + count + "群");
                return null;
            }
            TchatGroup group = new TchatGroup { Creater = regInfo.Nodeid, Groupname = req.groupname, Descript = req.descript, Grouppic = filePath, Groupstate = 0, Grouptype = 0, Transferid = 0 };
            db.TchatGroupSet.Add(group);
            db.BeginTransaction();
            if (db.SaveChanges() < 1)
            {
                Alert("创建群组失败,code=1");
                db.Rollback();
                return null;
            }
            RongResult result = RongCloudServer.CreateGroup(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), group.Id.ToString(), group.Groupname);
            if (!result.Result)
            {
                Alert("创建群组失败,code=2");
                log.Info(result.errorMessage);
                db.Rollback();
                return null;
            }
            group.Groupstate = 1;
            TchatGroupUser guser = new TchatGroupUser();
            guser.Groupid = group.Id;
            guser.Userid = group.Creater;
            db.TchatGroupUserSet.Add(guser);
            if (db.SaveChanges() < 1)
            {
                Alert("创建群组失败,code=3");
                db.Rollback();
                return null;
            }
            db.Commit();
            return GroupGet(group.Id);
        }

        private ChatGroupDto GroupGet(int groupid)
        {
            var query = (from tgp in db.VchatGroupSet
                         select new ChatGroupDto
                         {
                             Auditstate = tgp.Auditstate,
                             Authstate = tgp.Authstate,
                             Creater = tgp.Creater,
                             Createtime = tgp.Createtime,
                             Descript = tgp.Descript,
                             Dismisstime = tgp.Dismisstime,
                             Groupcode = tgp.Groupcode,
                             Groupname = tgp.Groupname,
                             Grouppic = tgp.Grouppic,
                             Groupstate = tgp.Groupstate,
                             Id = tgp.Id,
                             GroupType = tgp.Grouptype,
                             PersonCount = tgp.Personcount,
                             CreateNodecode = tgp.Createnodecode,
                             CreateNodename = tgp.Createnodename
                         }).Where(a => a.Id == groupid).OrderByDescending(a => a.Id);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// 查询敏感词
        /// </summary>
        /// <param name="filterWord"></param>
        /// <returns></returns>
        public bool IsFilterWord(string filterWord)
        {
            int instrnum = db.TchatFilterwordSet.Where(a => a.Filterword.Contains(filterWord)).Count();
            if (instrnum > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public string SaveImage(string base64Str)
        {
            return SaveImage(base64Str, "/Content/imgfile/");
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public string SaveImage(string base64Str, string dir)
        {
            base64Str = base64Str.Replace("\\", "").Replace(" ", "+").Substring(base64Str.LastIndexOf(",") + 1);
            string filename = string.Empty;
            try
            {
                byte[] image = Convert.FromBase64String(base64Str);
                MemoryStream ms = new MemoryStream(image);
                filename = dir + Guid.NewGuid().ToString() + ".png";
                string filepath = HttpContext.Current.Request.MapPath(filename);
                Bitmap b = (Bitmap)Image.FromStream(ms);
                b.Save(filepath, ImageFormat.Png);
            }
            catch (Exception exp)
            {

                Alert("上传图片失败");
                filename = string.Empty;
                log.Info(exp.ToString());
            }
            return filename;
        }
        #endregion

        #region 修改群组
        /// <summary>
        /// 修改群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ChatGroupDto UpdateGroup(UpdateGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (IsFilterWord(req.groupname))
            {
                Alert("群名称中含有敏感词不合法");
                return null;
            }
            string filePath = "";
            if (!string.IsNullOrEmpty(req.grouppic))
            {
                filePath = SaveImage(req.grouppic);
            }
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群不存在");
                return null;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群的创建者不能修改");
                return null;
            }
            if (!string.IsNullOrEmpty(req.groupname) && !group.Groupname.Equals(req.groupname, StringComparison.OrdinalIgnoreCase))
            {
                RongResult result = RongCloudServer.RefreshGroup(AppConfig.AppKey, AppConfig.AppSecret, req.groupid.ToString(), req.groupname);
                if (!result.Result)
                {
                    Alert("修改群失败,code=1");
                    log.Info(result.errorMessage);
                    return null;
                }
                group.Groupname = req.groupname;
            }
            group.Descript = req.descript;
            if (!string.IsNullOrEmpty(filePath))
            {
                group.Grouppic = filePath;
            }
            if (db.SaveChanges() < 1)
            {
                Alert("修改群失败");
                return null;
            }
            Alert("更新群成功");
            return GroupGet(group.Id);
        }
        #endregion

        #region 查找群组
        /// <summary>
        /// 查找群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatGroupDto> QueryGroup(QueryGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            var query = (from tgp in db.VchatGroupSet
                         select new ChatGroupDto
                         {
                             Auditstate = tgp.Auditstate,
                             Authstate = tgp.Authstate,
                             Creater = tgp.Creater,
                             Createtime = tgp.Createtime,
                             Descript = tgp.Descript,
                             Dismisstime = tgp.Dismisstime,
                             Groupcode = tgp.Groupcode,
                             Groupname = tgp.Groupname,
                             Grouppic = tgp.Grouppic,
                             Groupstate = tgp.Groupstate,
                             Id = tgp.Id,
                             GroupType = tgp.Grouptype,
                             PersonCount = tgp.Personcount,
                             CreateNodecode = tgp.Createnodecode,
                             CreateNodename = tgp.Createnodename
                         });

            //select groupid from tchat_group_user where userid

            int[] userids = db.TchatGroupUserSet.Where(a => a.Userid == regInfo.Nodeid).Select(a => a.Groupid).ToArray();

            query = query.Where(a => !(userids.Contains(a.Id)) && (a.GroupType == 0 || a.GroupType == 1));
            if (!string.IsNullOrEmpty(req.key))
            {

                query = query.Where(a => a.Groupname.Contains(req.key));
            }
            query = query.OrderByDescending(a => a.Id);
            return query.ToPagedList(req.pageIndex, req.pageSize);
        }

        #endregion


        #region 我的群组
        /// <summary>
        /// 我的群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ChatGroupDto> MyGroup(Reqbase req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            var query = (from tgp in db.VchatGroupSet
                         select new ChatGroupDto
                         {
                             Auditstate = tgp.Auditstate,
                             Authstate = tgp.Authstate,
                             Creater = tgp.Creater,
                             Createtime = tgp.Createtime,
                             Descript = tgp.Descript,
                             Dismisstime = tgp.Dismisstime,
                             Groupcode = tgp.Groupcode,
                             Groupname = tgp.Groupname,
                             Grouppic = tgp.Grouppic,
                             Groupstate = tgp.Groupstate,
                             Id = tgp.Id,
                             GroupType = tgp.Grouptype,
                             PersonCount = tgp.Personcount,
                             CreateNodecode = tgp.Createnodecode,
                             CreateNodename = tgp.Createnodename
                         });
            int[] userids = db.TchatGroupUserSet.Where(a => a.Userid == regInfo.Nodeid).Select(a => a.Groupid).ToArray();
            query = query.Where(a => userids.Contains(a.Id) && a.GroupType != 3).OrderByDescending(a => a.Id);
            return query.ToList();
        }
        #endregion

        #region 加入群组
        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool JoinGroup(JoinGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            TchatGroupUser gUser = db.TchatGroupUserSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == regInfo.Nodeid);
            if (gUser != null)
            {
                Alert("您已经是该群组成员了");
                return false;
            }
            if (group.Grouptype == 3)
            {
                //广播群直接入群
                RongResult result = RongCloudServer.JoinGroup(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), group.Id.ToString(), group.Groupname);
                if (!result.Result)
                {
                    Alert("加入群组失败,code=1");
                    log.Info(result.errorMessage);
                    return false;
                }
                int cnt = db.TchatGroupUserSet.Count(x => x.Groupid == req.groupid);
                group.PersonCount = cnt + 1;

                db.TchatGroupUserSet.Add(new TchatGroupUser { Groupid = group.Id, Userid = regInfo.Nodeid, Creattime = DateTime.Now });
                if (db.SaveChanges() < 1)
                {
                    Alert("加入群组失败");
                    return false;
                }
            }
            else
            {
                ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
                ChatUserDto groupMaster = GetPxUser(group.Creater);
                if (!SendGroupQuest(questUser, groupMaster, group, req.remarks))
                {
                    Alert("申请加入群组失败");
                    return false;
                }
            }

            Alert("申请加入群组成功，等待管理员审核", 1);
            return true;
        }


        private bool SendGroupQuest(ChatUserDto questUser, ChatUserDto destUser, TchatGroup group, string remarks)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = string.Format(@"{0}请求加入群组:{1}，附加消息：{2}", questUser.NodeName, group.Groupname, remarks);
            string extra = JsonConvert.SerializeObject(new { QuestUser = questUser, Group = group });
            string content = JsonConvert.SerializeObject(new
            {
                operation = "addgroup",
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = "加入群组"
            });

            //发送添加好友请求消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;

        }
        #endregion

        #region 确认加入群组
        /// <summary>
        /// 确认加入群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool JoinGroupConfirm(JoinGroupConfirmReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主");
                return false;
            }
            TnetReginfo regUser = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.usercode);
            if (regUser == null)
            {
                Alert("申请加入群组用户不存在");
                return false;
            }
            if (req.status == 1)
            {
                TchatGroupUser gUser = db.TchatGroupUserSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == regUser.Nodeid);
                if (gUser != null)
                {
                    Alert("您已经是该群组成员了");
                    return false;
                }
                RongResult result = RongCloudServer.JoinGroup(AppConfig.AppKey, AppConfig.AppSecret, regUser.Nodeid.ToString(), group.Id.ToString(), group.Groupname);
                if (!result.Result)
                {
                    Alert("加入群组失败,code=1");
                    log.Info(result.errorMessage);
                    return false;
                }

                int cnt = db.TchatGroupUserSet.Count(x => x.Groupid == req.groupid);
                group.PersonCount = cnt + 1;

                gUser = new TchatGroupUser();
                gUser.Groupid = group.Id;
                gUser.Userid = regUser.Nodeid;
                db.TchatGroupUserSet.Add(gUser);

                if (db.SaveChanges() < 1)
                {
                    Alert("加入群组失败,code=2");
                    return false;
                }
            }
            ChatUserDto destUser = GetPxUser(regUser.Nodeid);
            TnetReginfo regInfoMaster = db.TnetReginfoSet.FirstOrDefault(c => c.Nodeid == group.Creater);
            ChatUserDto questUser = GetPxUser(regInfoMaster.Nodeid);
            if (!SendGroupResponse(questUser, destUser, group, req.status, req.remarks))
            {
                Alert("处理加入群组失败");
                return false;
            }
            Alert("加入群组成功", 1);
            return true;
        }

        private bool SendGroupResponse(ChatUserDto questUser, ChatUserDto destUser, TchatGroup group, int status, string remarks)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = string.Format(@"{0}{1}了您的加入群组:{2}请求，附加消息：{3}", questUser.NodeName, (status == 1 ? "通过" : "拒绝"), group.Groupname, remarks);
            string extra = JsonConvert.SerializeObject(new { QuestUser = questUser, Group = group });
            string operation = status == 1 ? "respgrouppass" : "respgrouprefuse";
            string content = JsonConvert.SerializeObject(new
            {
                operation = operation,
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = "加入群组回复"
            });

            //发送加入群组回复消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;
        }

        #endregion

        #region 群主拉人入群
        /// <summary>
        /// 群主拉人入群
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool JoinGroupInvitation(JoinGroupInvitationReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            string[] useridarr = req.userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (useridarr.Length > 50)
            {
                Alert("拉取人数不能超过50");
                return false;
            }
            int[] userids = Array.ConvertAll<string, int>(useridarr, s => int.Parse(s));
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能拉人加入群组");
                return false;
            }
            List<ChatUserDto> pxUsers = GetPxUsers(regInfo, userids);
            string[] alluserids = pxUsers.Select(c => c.NodeId.ToString()).ToArray();
            string[] ryuserids = pxUsers.Where(c => !string.IsNullOrEmpty(c.Token)).Select(c => c.NodeId.ToString()).ToArray();
            string[] dbuserid = db.TchatGroupUserSet.Where(c => c.Groupid == req.groupid).Select(c => c.Userid.ToString()).ToArray();
            RongResult result = RongCloudServer.JoinGroup(AppConfig.AppKey, AppConfig.AppSecret, ryuserids, group.Id.ToString(), group.Groupname);
            if (!result.Result)
            {
                Alert("加入群组失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            foreach (var item in alluserids.Except(dbuserid))
            {
                int tid = int.Parse(item);
                db.TchatGroupUserSet.Add(new TchatGroupUser { Groupid = req.groupid, Userid = tid });
            }
            if (db.SaveChanges() < 1)
            {
                Alert("加入群组失败,code=2");
                return false;
            }
            if (group.Grouptype != 3)
            {
                ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
                string msg = string.Format(@"{0}邀请您加入了群组{1}", regInfo.Nodename, group.Groupname);
                SendGroupInvitationNotice(questUser, group, pxUsers.Where(c => !string.IsNullOrEmpty(c.Token)).ToList(), msg);
            }
            Alert("加入群组成功", 1);
            return true;
        }

        private bool SendGroupInvitationNotice(ChatUserDto questUser, TchatGroup group, List<ChatUserDto> users, string msg)
        {
            //string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = msg; //quitgroup,removegroup,dismissgroup
            string extra = JsonConvert.SerializeObject(new { Group = group });//userid-受影响的群用户
            foreach (var destUser in users)
            {
                string content = JsonConvert.SerializeObject(new
                {
                    operation = "invitationgroup",
                    sourceUserId = questUser.NodeId.ToString(),
                    targetUserId = destUser.NodeId.ToString(),
                    message = sysMsg,
                    extra = extra
                });
                string pushContent = sysMsg;
                string pushData = JsonConvert.SerializeObject(new
                {
                    pushData = sysMsg,
                    extra = "群主拉好友入群"
                });

                //发送添加好友请求消息
                //RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
                //if (!result.Result)
                //{
                //    Msg = result.errorMessage;
                //    return false;
                //}
                //发送个推
                GtPush(destUser, sysMsg, content);
            }
            return true;
        }
        #endregion

        #region 退出群组
        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool QuitGroup(QuitGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);


            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater == regInfo.Nodeid)
            {
                Alert("您是群主不能退出群组");
                return false;
            }
            RongResult result = RongCloudServer.QuitGroup(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), group.Id.ToString());
            if (!result.Result)
            {
                Alert("退出群组失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            TchatGroupUser gUser = db.TchatGroupUserSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == regInfo.Nodeid);
            if (gUser != null)
            {
                int cnt = db.TchatGroupUserSet.Count(x => x.Groupid == req.groupid);
                group.PersonCount = cnt - 1;

                db.TchatGroupUserSet.Remove(gUser);
                if (group.Grouptype == 2)
                {
                    //记录退出系统群记录
                    db.TchatGroupQuitlogSet.Add(new TchatGroupQuitlog { Nodeid = regInfo.Nodeid, Groupid = group.Id, Quittype = 1, UserGradeLevel = group.Usergradelevel, GroupType = group.Grouptype });
                }
                if (db.SaveChanges() < 1)
                {
                    Alert("退出群组失败,code=2");
                    return false;
                }
                if (group.Grouptype != 3)
                {
                    string msg = string.Format("{0}退出了群组:{1}", regInfo.Nodename, group.Groupname);
                    string operation = "quitgroup";
                    ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
                    TnetReginfo regInfoMaster = db.TnetReginfoSet.First(c => c.Nodeid == group.Creater);
                    ChatUserDto destUser = GetPxUser(regInfoMaster.Nodeid);
                    SendGroupMasterNotice(questUser, destUser, group, operation, msg);
                    //SendGroupQuitNotice(questUser, group, regInfo.Nodeid, operation, msg);
                }
            }

            Alert("退出群组成功", 1);
            return true;
        }

        /// <summary>
        /// 给群主发送消息
        /// </summary>
        /// <param name="questUser"></param>
        /// <param name="destUser"></param>
        /// <param name="group"></param>
        /// <param name="operation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool SendGroupMasterNotice(ChatUserDto questUser, ChatUserDto destUser, TchatGroup group, string operation, string msg)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = msg; //quitgroup,removegroup,dismissgroup
            string extra = JsonConvert.SerializeObject(new { Group = group, Userid = questUser.NodeId });//userid-受影响的群用户

            string content = JsonConvert.SerializeObject(new
            {
                operation = operation,
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = operation
            });
            //发送群组成员退出请求消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;
        }
        #endregion

        #region 移除群成员

        /// <summary>
        /// 移除群成员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool RemoveGroupUser(RemoveGroupUserReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            TnetReginfo regUser = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.usercode);
            if (regUser == null)
            {
                Alert("移除群组用户不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能移除成员");
                return false;
            }
            if (group.Creater == regUser.Nodeid)
            {
                Alert("群主不能移除出群组");
                return false;
            }
            RongResult result = RongCloudServer.QuitGroup(AppConfig.AppKey, AppConfig.AppSecret, regUser.Nodeid.ToString(), group.Id.ToString());
            if (!result.Result)
            {
                Alert("退出群组失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            TchatGroupUser gUser = db.TchatGroupUserSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == regUser.Nodeid);
            if (gUser != null)
            {
                int cnt = db.TchatGroupUserSet.Count(x => x.Groupid == req.groupid);
                group.PersonCount = cnt - 1;

                db.TchatGroupUserSet.Remove(gUser);
                if (group.Grouptype == 2)
                {
                    //记录移出系统群记录
                    db.TchatGroupQuitlogSet.Add(new TchatGroupQuitlog { Nodeid = regInfo.Nodeid, Groupid = group.Id, Quittype = 2, UserGradeLevel = group.Usergradelevel, GroupType = group.Grouptype });
                }
                if (db.SaveChanges() < 1)
                {
                    Alert("退出群组失败,code=2");
                    return false;
                }
                string msg = string.Format("群主{0}把{1}移出了群组:{2}", regInfo.Nodename, regUser.Nodename, group.Groupname);
                string operation = "removegroup";
                ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
                ChatUserDto destUser = GetPxUser(regUser.Nodeid);
                SendRemoveUserNotice(questUser, destUser, group, operation, msg);
                //SendGroupQuitNotice(questUser, group, regUser.Nodeid, operation, msg);
            }

            Alert("移出群组成功", 1);
            return true;
        }

        /// <summary>
        /// 给被移除的人发送消息
        /// </summary>
        /// <param name="questUser"></param>
        /// <param name="destUser"></param>
        /// <param name="group"></param>
        /// <param name="operation"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool SendRemoveUserNotice(ChatUserDto questUser, ChatUserDto destUser, TchatGroup group, string operation, string msg)
        {
            string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = msg; //quitgroup,removegroup,dismissgroup
            string extra = JsonConvert.SerializeObject(new { Group = group, Userid = destUser.NodeId });//userid-受影响的群用户

            string content = JsonConvert.SerializeObject(new
            {
                operation = operation,
                sourceUserId = questUser.NodeId.ToString(),
                targetUserId = destUser.NodeId.ToString(),
                message = sysMsg,
                extra = extra
            });
            string pushContent = sysMsg;
            string pushData = JsonConvert.SerializeObject(new
            {
                pushData = sysMsg,
                extra = operation
            });
            //发送移除群组成员请求消息
            RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
            if (!result.Result)
            {
                Alert(result.errorMessage);
                return false;
            }
            //发送个推
            GtPush(destUser, sysMsg, content);
            return true;
        }

        #endregion

        #region 解散群组
        /// <summary>
        /// 移除群成员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DismissGroup(DismissGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能解散群组");
                return false;
            }
            if (group.Grouptype == 2)
            {
                Alert("系统群不能解散");
                return false;
            }
            RongResult result = RongCloudServer.DismissGroup(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), group.Id.ToString());
            if (!result.Result)
            {
                Alert("解散群组失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            group.Groupstate = 2;
            if (db.SaveChanges() < 1)
            {
                Alert("解散群组失败,code=2");
                return false;
            }
            string msg = string.Format("群主{0}解散了群组:{1}", regInfo.Nodename, group.Groupname);
            string operation = "dismissgroup";
            ChatUserDto questUser = GetPxUser(regInfo.Nodeid);
            SendGroupQuitNotice(questUser, group, 0, operation, msg);

            Alert("解散群组成功", 1);
            return true;
        }

        private bool SendGroupQuitNotice(ChatUserDto questUser, TchatGroup group, int userid, string operation, string msg)
        {
            //string msgType = "RC:ContactNtf";//添加联系人消息
            //string msgType = "RC:TxtMsg";
            string sysMsg = msg; //quitgroup,removegroup,dismissgroup
            string extra = JsonConvert.SerializeObject(new { Group = group, Userid = userid });//userid-受影响的群用户

            List<ChatUserDto> users = QueryGroupUser(null, group.Id, 1, 9999).ToList();
            foreach (var destUser in users)
            {
                string content = JsonConvert.SerializeObject(new
                {
                    operation = operation,
                    sourceUserId = questUser.NodeId.ToString(),
                    targetUserId = destUser.NodeId.ToString(),
                    message = sysMsg,
                    extra = extra
                });
                string pushContent = sysMsg;
                string pushData = JsonConvert.SerializeObject(new
                {
                    pushData = sysMsg,
                    extra = operation
                });
                //发送添加好友请求消息
                //RongResult result = RongCloudServer.PublishSystemMessage(AppConfig.AppKey, AppConfig.AppSecret, questUser.NodeId.ToString(), destUser.NodeId.ToString(), msgType, content, pushContent, pushData);
                //if (!result.Result)
                //{
                //    Msg = result.errorMessage;
                //    return false;
                //}
                //发送个推
                GtPush(destUser, sysMsg, content);
            }
            return true;
        }

        /// <summary>
        /// 查询群组成员
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="groupid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ChatUserDto> QueryGroupUser(TnetReginfo regInfo, int groupid, int pageIndex, int pageSize)
        {
            var query = (from tgu in db.TchatGroupUserSet.Where(a => a.Groupid == groupid)
                         join tg in db.TchatGroupSet on tgu.Groupid equals tg.Id
                         join user in db.TchatUserFullSet.OrderByDescending(a => a.Id) on tgu.Userid equals user.Nodeid
                         join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == 0) on user.Nodeid equals fri.Friendnodeid into fri_join
                         from fri in fri_join.DefaultIfEmpty()
                         orderby user.Id descending
                         select new ChatUserDto
                         {
                             //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                             //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                             AppPhoto = user.Appphoto,
                             //Cityid = user.Cityid,
                             //Cityname = user.Cityname,
                             // Config = user.Config,
                             DeviceToken = user.Devicetoken,
                             GradeId = user.Gradeid,
                             GradeName = user.Gtclientid,
                             GtClientid = user.Gtclientid,
                             Nickname = user.Nickname,
                             NodeCode = user.Nodecode,
                             NodeId = user.Nodeid,
                             NodeName = user.Nodename,
                             Personalsign = user.Personalsign,
                             //Provinceid = user.Provinceid,
                             //Provincename = user.Provincename,
                             Remarks = fri.Nickname,
                             //Sex = user.Sex,
                             Showrealname = user.Showrealname,
                             //TeamName = user.Teamname,
                             Token = user.Token,
                             //Email = user.Email,
                             Mobileno = user.Mobileno
                             // TxUserSig = user.t
                         });


            return query.ToPagedList(pageIndex, pageSize);
        }
        #endregion

        #region 查询群组成员
        /// <summary>
        /// 查询群组成员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatUserDto> QueryGroupUser(QueryGroupUserReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            if (db.TchatGroupUserSet.Where(a => a.Userid.Equals(req.Nodeid)) == null)
            {
                Alert("您不是群成员");
                return null;
            }

            var query = (from tgu in db.TchatGroupUserSet.Where(a => a.Groupid == req.groupid)
                         join tg in db.TchatGroupSet on tgu.Groupid equals tg.Id
                         join user in db.TchatUserFullSet.OrderByDescending(a => a.Id) on tgu.Userid equals user.Nodeid
                         join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == regInfo.Nodeid) on user.Nodeid equals fri.Friendnodeid into fri_join
                         from fri in fri_join.DefaultIfEmpty()
                         orderby user.Id descending
                         select new ChatUserDto
                         {
                             //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                             //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                             AppPhoto = user.Appphoto,
                             //Cityid = user.Cityid,
                             //Cityname = user.Cityname,
                             // Config = user.Config,
                             DeviceToken = user.Devicetoken,
                             GradeId = user.Gradeid,
                             GradeName = user.Gtclientid,
                             GtClientid = user.Gtclientid,
                             Nickname = user.Nickname,
                             NodeCode = user.Nodecode,
                             NodeId = user.Nodeid,
                             NodeName = user.Nodename,
                             Personalsign = user.Personalsign,
                             //Provinceid = user.Provinceid,
                             //Provincename = user.Provincename,
                             Remarks = fri.Nickname,
                             //Sex = user.Sex,
                             Showrealname = user.Showrealname,
                             //TeamName = user.Teamname,
                             Token = user.Token,
                             //Email = user.Email,
                             Mobileno = user.Mobileno
                             // TxUserSig = user.t
                         });


            return query.ToPagedList(req.pageIndex, req.pageSize);
        }
        #endregion

        #region 群组禁言
        /// <summary>
        /// 群组禁言
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool GagUserAddGroup(GagUserAddGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能使用禁言功能");
                return false;
            }
            string[] uids = req.userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in uids)
            {
                int uid = Convert.ToInt32(item);
                TchatGroupUsergag ug = db.TchatGroupUsergagSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == uid && c.Status == 0);
                if (ug != null)
                {
                    ug.Status = 1;
                }
                db.TchatGroupUsergagSet.Add(new TchatGroupUsergag
                {
                    Groupid = req.groupid,
                    Userid = uid,
                    Minute = req.minute,
                    Status = 0,
                    Optnodeid = regInfo.Nodeid
                });
            }
            RongResult result = RongCloudServer.GagAddGroup(AppConfig.AppKey, AppConfig.AppSecret, uids, req.groupid.ToString(), req.minute);
            if (!result.Result)
            {
                Alert("禁言用户失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            if (db.SaveChanges() < 1)
            {
                Alert("禁言用户失败,code=2");
                return false;
            }
            Alert("禁言用户成功", 1);
            return true;
        }
        #endregion

        #region 群组禁言移除
        /// <summary>
        /// 群组禁言移除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool GagUserRemoveGroup(GagUserRemoveGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return false;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能使用禁言功能");
                return false;
            }
            string[] uids = req.userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in uids)
            {
                int uid = Convert.ToInt32(item);
                TchatGroupUsergag ug = db.TchatGroupUsergagSet.FirstOrDefault(c => c.Groupid == req.groupid && c.Userid == uid && c.Status == 0);
                if (ug != null)
                {
                    ug.Status = 1;
                    ug.Canceltime = DateTime.Now;
                }
            }
            RongResult result = RongCloudServer.GagRollbackGroup(AppConfig.AppKey, AppConfig.AppSecret, uids, req.groupid.ToString());
            if (!result.Result)
            {
                Alert("移除禁言用户失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            if (db.SaveChanges() < 1)
            {
                Alert("移除禁言用户失败,code=2");
                return false;
            }
            Alert("移除禁言用户成功", 1);
            return true;
        }
        #endregion

        #region 群组禁言查询
        /// <summary>
        /// 群组禁言查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<GagUser> GagUserQueryGroup(GagUserQueryGroupReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatGroup group = db.TchatGroupSet.FirstOrDefault(c => c.Id == req.groupid && c.Groupstate == 1);
            if (group == null)
            {
                Alert("群组不存在");
                return null;
            }
            if (group.Creater != regInfo.Nodeid)
            {
                Alert("您不是群主，不能使用禁言功能");
                return null;
            }
            RongGagQueryResult result = RongCloudServer.GagListGroup(AppConfig.AppKey, AppConfig.AppSecret, req.groupid.ToString());
            if (!result.Result)
            {
                Alert("查询禁言用户失败,code=1");
                log.Info(result.errorMessage);
                return null;
            }
            Alert("查询禁言用户成功", 1);
            return result.users.ToList();
        }
        #endregion

        #region 创建系统聊天室
        /// <summary>
        /// 创建系统聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TchatRoom CreateChatRoom(CreateChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (regInfo.Nodeid != AppConfig.SysUserId)
            {
                Alert("您没有权限创建聊天室");
                return null;
            }
            PXinContext ctx = HttpContext.Current.GetDbContext<PXinContext>();
            if (IsFilterWord(req.roomname))
            {
                Alert("聊天室名称中含有敏感词不合法");
                return null;
            }
            string filename = string.Empty;
            if (!string.IsNullOrEmpty(req.roompic))
            {
                filename = SaveImage(req.roompic);
                if (string.IsNullOrEmpty(filename))
                {
                    return null;
                }
            }
            TchatRoom room = new TchatRoom { Creater = regInfo.Nodeid, Roomname = req.roomname, Roompwd = req.roompwd, Remarks = req.descript, Roomstate = 0, Roomtype = 2, Transferid = 0, Roompic = filename };
            ctx.TchatRoomSet.Add(room);
            if (ctx.SaveChanges() < 1)
            {
                Alert("创建聊天室失败,code=1");
                return null;
            }
            RongResult result = RongCloudServer.CreateChatroom(AppConfig.AppKey, AppConfig.AppSecret, new String[] { room.Id.ToString() }, new String[] { room.Roomname });
            if (!result.Result)
            {
                Alert("创建聊天室失败,code=2");
                log.Info(result.errorMessage);
                return null;
            }
            room.Roomstate = 1;
            if (ctx.SaveChanges() < 1)
            {
                Alert("创建聊天室失败,code=3");
                return null;
            }
            room.Roompwd = string.Empty;
            return room;
        }
        #endregion

        #region 创建收费聊天室
        /// <summary>
        /// 创建收费聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TchatRoom CreateChatRoom2(CreateChatRoom2Req req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (!UserPwd.Check(req.paypwd, regInfo.UserpwdBak, regInfo.Nodeid, regInfo.Nodecode))
            {
                Alert("支付密码错误");
                return null;
            }
            if (IsFilterWord(req.roomname))
            {
                Alert("聊天室名称中含有敏感词不合法");
                return null;
            }
            string filename = string.Empty;
            if (!string.IsNullOrEmpty(req.roompic))
            {
                filename = SaveImage(req.roompic);
                if (string.IsNullOrEmpty(filename))
                {
                    return null;
                }
            }
            //检查完毕,开始转账            
            BeginTransfer();
            //扣费
            decimal amount = AppConfig.CreateChatRoomAmount;
            Currency currency = new Currency(CurrencyType.RMB, amount);
            Purse fromPurse = purseFactory.UserGFVPurse(regInfo.Nodeid);
            Purse toPurse = purseFactory.SystemPurseRand(regInfo.Nodeid);
            if (fromPurse.UsableBalance / 100 < amount)
            {
                Alert("余额不足");
                EndTransfer(false);
                return null;
            }
            TransferResult transferResult = wcfProxy.Transfer(fromPurse, toPurse, currency, AppConfig.CreateChatRoomReason, "创建聊天室");
            if (!transferResult.IsSuccess)
            {
                EndTransfer(false);
                Alert(transferResult.Message);
                return null;
            }

            //创建聊天室
            TchatRoom room = new TchatRoom { Creater = regInfo.Nodeid, Roomname = req.roomname, Roompwd = req.roompwd, Remarks = req.descript, Roomstate = 0, Roomtype = 1, Transferid = transferResult.TransferId, Roompic = filename };
            db.BeginTransaction();
            db.TchatRoomSet.Add(room);
            if (db.SaveChanges() < 1)
            {
                db.Rollback();
                EndTransfer(false);
                Alert("创建聊天室失败,code=1");
                return null;
            }
            RongResult result = RongCloudServer.CreateChatroom(AppConfig.AppKey, AppConfig.AppSecret, new String[] { room.Id.ToString() }, new String[] { room.Roomname });
            if (!result.Result)
            {
                db.Rollback();
                EndTransfer(false);
                Alert("创建聊天室失败,code=2");
                log.Info(result.errorMessage);
                return null;
            }
            room.Roomstate = 1;
            if (db.SaveChanges() < 1)
            {
                db.Rollback();
                EndTransfer(false);

            }
            room.Roompwd = string.Empty;
            EndTransfer();
            db.Commit();
            return room;
        }
        #endregion

        #region 修改聊天室信息
        /// <summary>
        /// 修改聊天室信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public TchatRoom UpdateChatRoom(UpdateChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            if (IsFilterWord(req.roomname))
            {
                Alert("聊天室名称中含有敏感词不合法");
                return null;
            }
            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return null;
            }
            if (room.Creater != regInfo.Nodeid)
            {
                Alert("您不是聊天室的创建者不能修改");
                return null;
            }
            string filename = string.Empty;
            if (!string.IsNullOrEmpty(req.roompic))
            {
                filename = SaveImage(req.roompic);
                if (string.IsNullOrEmpty(filename))
                {
                    return null;
                }
            }
            if (!string.IsNullOrEmpty(req.roomname) && !room.Roomname.Equals(req.roomname, StringComparison.OrdinalIgnoreCase))
            {
                RongResult result = RongCloudServer.CreateChatroom(AppConfig.AppKey, AppConfig.AppSecret, new String[] { room.Id.ToString() }, new String[] { room.Roomname });
                if (!result.Result)
                {
                    Alert("修改聊天室失败,code=1");
                    log.Info(result.errorMessage);
                    return null;
                }
                room.Roomname = req.roomname;
            }
            if (!string.IsNullOrEmpty(req.descript))
            {
                room.Remarks = req.descript;
            }
            if (!string.IsNullOrEmpty(filename))
            {
                room.Roompic = filename;
            }
            if (db.SaveChanges() < 1)
            {
                Alert("修改聊天室失败");
                return null;

            }
            Alert("更新聊天成功", 1);
            return room;
        }
        #endregion

        #region 修改聊天室密码
        /// <summary>
        /// 修改聊天室密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateChatRoomPwd(UpdateChatRoomPwdReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return false;
            }
            if (room.Creater != regInfo.Nodeid)
            {
                Alert("您不是聊天室的创建者不能修改");
                return false;
            }
            if (!string.IsNullOrEmpty(room.Roompwd) && !room.Roompwd.Equals(req.oldpwd, StringComparison.OrdinalIgnoreCase))
            {
                Alert("旧密码错误修改失败");
                return false;
            }
            room.Roompwd = req.roompwd;
            if (db.SaveChanges() < 1)
            {
                Alert("旧密码修改失败");
                return false;
            };
            Alert("修改密码成功", 1);
            return true;
        }
        #endregion

        #region 查询聊天室
        /// <summary>
        /// 查询聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatRoomDto> QueryChatRoom(QueryChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            var query = (from tcr in db.TchatRoomSet
                         join reg in db.TnetReginfoSet on tcr.Creater equals reg.Nodeid into reg_join
                         from reg in reg_join.DefaultIfEmpty()
                         select new ChatRoomDto
                         {
                             Createnodecode = reg.Nodecode,
                             Dismisstime = tcr.Dismisstime,
                             Creater = tcr.Creater,
                             Remarks = tcr.Remarks,
                             Createnodename = reg.Nodename,
                             Createtime = tcr.Createtime,
                             Haspwd = (tcr.Roompwd.Length > 0) ? 1 : 0,
                             Id = tcr.Id,
                             Personcount = tcr.Personcount,
                             Roomname = tcr.Roomname,
                             Roompic = tcr.Roompic,
                             Roompwd = tcr.Roompwd,
                             Roomstate = tcr.Roomstate,
                             Roomtype = tcr.Roomtype,
                             Transferid = tcr.Transferid
                         }).Where(a => a.Roomstate == 1);

            if (!string.IsNullOrEmpty(req.key))
            {
                if (int.TryParse(req.key, out int roomid))
                {
                    query = query.Where(a => a.Id == roomid);
                }
                else
                {
                    query = query.Where(a => a.Roomname.Contains(req.key));
                }
            }
            if (req.myroom == 1)
            {
                query = query.Where(a => a.Creater == regInfo.Nodeid);
            }
            else if (req.myroom == 2)
            {
                query = query.Where(a => a.Roomtype == 2);
            }
            query = query.OrderByDescending(a => a.Roomtype).ThenByDescending(a => a.Id);
            return query.ToPagedList(req.pageIndex, req.pageSize);
        }
        #endregion

        #region 加入聊天室
        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool JoinChatRoom(JoinChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return false;
            }
            if (!string.IsNullOrEmpty(room.Roompwd) && room.Creater != regInfo.Nodeid)
            {
                if (!req.roompwd.Equals(room.Roompwd, StringComparison.OrdinalIgnoreCase))
                {
                    Alert("加入聊天室失败,原因:密码错误");
                    return false;
                }
            }
            RongResult result = null;
            RongChatRoom rcr = RongCloudServer.queryChatroom(AppConfig.AppKey, AppConfig.AppSecret, new string[] { room.Id.ToString() });
            if (!rcr.Result || rcr.chatRooms == null || rcr.chatRooms.Length == 0)
            {
                result = RongCloudServer.CreateChatroom(AppConfig.AppKey, AppConfig.AppSecret, new String[] { room.Id.ToString() }, new String[] { room.Roomname });
                if (!result.Result)
                {
                    Alert(result.errorMessage);
                    log.Info("加入聊天室失败,code=3," + result.errorMessage);
                    return false;
                }
                room.Personcount = 0;
            }
            result = RongCloudServer.joinChatroom(AppConfig.AppKey, AppConfig.AppSecret, regInfo.Nodeid.ToString(), req.roomid.ToString());
            if (!result.Result)
            {
                Alert(result.errorMessage);
                log.Info("加入聊天室失败,code=1," + result.errorMessage);
                return false;
            }
            room.Personcount += 1;
            TchatRoomLog roomLog = new TchatRoomLog { Actiontype = 1, Nodeid = regInfo.Nodeid, Roomid = req.roomid };
            db.TchatRoomLogSet.Add(roomLog);
            if (db.SaveChanges() < 1)
            {
                Alert("加入聊天室失败");
                return false;
            }
            Alert("加入聊天室成功", 1);
            return true;
        }
        #endregion

        #region 退出聊天室
        /// <summary>
        /// 退出聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool QuitChatRoom(QuitChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return false;
            }
            room.Personcount -= 1;
            if (room.Personcount < 0)
            {
                room.Personcount = 0;
            }
            TchatRoomLog roomLog = new TchatRoomLog { Actiontype = 2, Nodeid = regInfo.Nodeid, Roomid = req.roomid };
            db.TchatRoomLogSet.Add(roomLog);
            if (db.SaveChanges() < 1)
            {
                Alert("退出聊天室失败");
                return false;
            }
            Alert("退出聊天室成功", 1);
            return true;
        }
        #endregion

        #region 销毁/解散聊天室
        /// <summary>
        /// 销毁/解散聊天室
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DestroyChatRoom(DestroyChatRoomReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return false;
            }
            if (room.Creater != regInfo.Nodeid)
            {
                Alert("您不是聊天室创建者,不能解散聊天室");
                return false;
            }
            if (room.Roomtype == 2)
            {
                Alert("系统聊天室不能解散");
                return false;
            }
            RongResult result = RongCloudServer.DestroyChatroom(AppConfig.AppKey, AppConfig.AppSecret, new String[] { room.Id.ToString() });
            if (!result.Result)
            {
                Alert("解散聊天室失败,code=1");
                log.Info(result.errorMessage);
                return false;
            }
            room.Roomstate = 2;
            room.Dismisstime = DateTime.Now;
            if (db.SaveChanges() < 1)
            {
                Alert("解散聊天室失败,code=2");
                return false;
            }
            Alert("解散聊天室成功", 1);
            return true;
        }
        #endregion

        #region 查询聊天室当前人数
        /// <summary>
        /// 查询聊天室当前人数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public int QueryChatRoomUserCount(QueryChatRoomUserCountReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            TchatRoom room = db.TchatRoomSet.FirstOrDefault(c => c.Id == req.roomid && c.Roomstate == 1);
            if (room == null)
            {
                Alert("聊天室不存在");
                return -1;
            }
            RongChatRoomUser rcru = RongCloudServer.queryChatroomUser(AppConfig.AppKey, AppConfig.AppSecret, room.Id.ToString());
            if (rcru.Result)
            {
                room.Personcount = rcru.total;
            }
            else
            {
                room.Personcount = 0;
            }
            TchatRoomLog roomLog = new TchatRoomLog { Actiontype = 1, Nodeid = regInfo.Nodeid, Roomid = req.roomid };
            db.SaveChanges();
            Alert("查询成功", 1);
            return room.Personcount;

        }
        #endregion

        #region 查找公众号
        /// <summary>
        /// 查找公众号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ChatPublicDto> QueryPublic(QueryPublicReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            var query = (from pub in db.TchatPublicSet
                         select new ChatPublicDto
                         {
                             Createtime = pub.Createtime,
                             Id = pub.Id,
                             PublicId = pub.PublicId,
                             PublicLogo = pub.PublicLogo,
                             PublicName = pub.PublicName,
                             PublicType = pub.PublicType,
                             Remarks = pub.Remarks
                         }).Where(a => a.PublicType == 3);

            if (!string.IsNullOrEmpty(req.key))
            {
                query = query.Where(a => a.PublicName.Contains(req.key) || a.PublicId.Contains(req.key));
            }
            query = query.OrderByDescending(a => a.Id);
            return query.ToList();
        }
        #endregion

        #region 获取省份列表
        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<VnetProvince> ProvinceList(Reqbase req)
        {
            //TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            List<VnetProvince> dtos = db.VnetProvinceSet.ToList();
            return dtos;
        }
        #endregion

        #region 获取城市列表
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<VnetCity> CityList(Reqbase req)
        {
            //TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            List<VnetCity> dtos = db.VnetCitySet.ToList();
            return dtos;
        }
        #endregion

        #region 添加关注
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ConcernAdd(ConcernAddReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo concernInfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.concernCode);
            if (concernInfo == null)
            {
                Alert("要关注的用户不存在");
                return false;
            }
            if (concernInfo.Nodeid == req.Nodeid)
            {
                Alert("自己不需要关注自己");
                return false;
            }
            TchatLiveFans fans = db.TchatLiveFansSet.FirstOrDefault(c => c.Mynodeid == concernInfo.Nodeid && c.Fansid == regInfo.Nodeid && c.Status == 1);
            if (fans != null)
            {
                Alert("已经关注过了");
                return false;
            }
            db.TchatLiveFansSet.Add(new TchatLiveFans { Mynodeid = concernInfo.Nodeid, Fansid = regInfo.Nodeid, Status = 1 });
            if (db.SaveChanges() < 1)
            {
                Alert("添加关注失败");
                return false;
            }
            Alert("添加关注成功", 1);
            return true;
        }
        #endregion

        #region 取消关注
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ConcernCancel(ConcernCancelReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            TnetReginfo concernInfo = db.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == req.concernCode);
            if (concernInfo == null)
            {
                Alert("要取消关注的用户不存在");
                return false;
            }
            TchatLiveFans fans = db.TchatLiveFansSet.FirstOrDefault(c => c.Mynodeid == concernInfo.Nodeid && c.Fansid == regInfo.Nodeid && c.Status == 1);
            if (fans == null)
            {
                Alert("没找到关注信息");
                return false;
            }
            fans.Status = 2;
            fans.Canceltime = DateTime.Now;
            if (db.SaveChanges() < 1)
            {
                Alert("取消关注失败");
                return false;
            }
            Alert("取消关注成功", 1);
            return true;
        }
        #endregion

        #region  我的粉丝列表
        /// <summary>
        /// 我的粉丝列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatUserDto> MyFans(MyFansReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            var query = from fans in db.TchatLiveFansSet.Where(a => a.Mynodeid == req.Nodeid && a.Status == 1).OrderByDescending(a => a.Id)
                        join user in db.TchatUserFullSet on fans.Fansid equals user.Nodeid
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid).Select(a => new { a.Nickname, a.Allowviewmedynamic, a.Viewhedynamic, a.Friendnodeid }) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        orderby fans.Id descending
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };


            return query.ToPagedList(req.pageIndex, req.pageSize);
        }

        #endregion

        #region 我的关注列表
        /// <summary>
        /// 我的关注列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IPagedList<ChatUserDto> MyConcerns(MyConcernsReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);

            var query = from fans in db.TchatLiveFansSet.Where(a => a.Fansid == req.Nodeid && a.Status == 1).OrderByDescending(a => a.Id)
                        join user in db.TchatUserFullSet on fans.Mynodeid equals user.Nodeid
                        join fri in db.TchatFriendNickSet.Where(a => a.Mynodeid == req.Nodeid).Select(a => new { a.Allowviewmedynamic, a.Viewhedynamic, a.Friendnodeid, a.Nickname }) on user.Nodeid equals fri.Friendnodeid into fri_join
                        from fri in fri_join.DefaultIfEmpty()
                        orderby fans.Id descending
                        select new ChatUserDto
                        {
                            //Allowviewmedynamic = fri == null ? 1 : fri.Allowviewmedynamic,
                            //Viewhedynamic = fri == null ? 1 : fri.Viewhedynamic,
                            AppPhoto = user.Appphoto,
                            //Cityid = user.Cityid,
                            //Cityname = user.Cityname,
                            // Config = user.Config,
                            DeviceToken = user.Devicetoken,
                            GradeId = user.Gradeid,
                            GradeName = user.Gtclientid,
                            GtClientid = user.Gtclientid,
                            Nickname = user.Nickname,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Personalsign = user.Personalsign,
                            //Provinceid = user.Provinceid,
                            //Provincename = user.Provincename,
                            Remarks = fri.Nickname,
                            //Sex = user.Sex,
                            Showrealname = user.Showrealname,
                            //TeamName = user.Teamname,
                            Token = user.Token,
                            //Email = user.Email,
                            Mobileno = user.Mobileno
                            // TxUserSig = user.t
                        };
            return query.ToPagedList(req.pageIndex, req.pageSize);
        }
        #endregion

        #region 附近的人
        /// <summary>
        /// 附近的人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<List<GetNearbyDto>> GetNearby(GetNearbyReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            //1.检查最近1个小时，该用户是否已经定位过了
            DateTime time = DateTime.Now.AddHours(-24);//1个小时内
            TpxinNearby tpxin_nearby2 = db.TpxinNearbySet.FirstOrDefault(c => c.Nodeid == req.Nodeid && c.Createtime > time);
            if (tpxin_nearby2 == null)
            {
                TpxinNearby tpxin_nearby = new TpxinNearby();
                tpxin_nearby.Nodeid = req.Nodeid;
                tpxin_nearby.Nickname = regInfo.Nodename;
                tpxin_nearby.Photo = AppConfig.DefaultPhoto;
                tpxin_nearby.Longitude = req.Longitude;
                tpxin_nearby.Latitude = req.Latitude;
                tpxin_nearby.Createtime = DateTime.Now;
                tpxin_nearby.Remarks = "";
                TnetUserphoto tnet_userphoto = db.TnetUserphotoSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
                if (tnet_userphoto != null && !string.IsNullOrEmpty(tnet_userphoto.Appphoto))
                {
                    tpxin_nearby.Photo = tnet_userphoto.Appphoto;
                    if (!tpxin_nearby.Photo.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    {
                        tpxin_nearby.Photo = AppConfig.Userphoto + tpxin_nearby.Photo;
                    }
                }
                TchatUser tchat_user = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
                if (tchat_user != null && !string.IsNullOrEmpty(tchat_user.Nickname))
                {
                    tpxin_nearby.Nickname = tchat_user.Nickname;
                }
                db.TpxinNearbySet.Add(tpxin_nearby);
                if (db.SaveChanges() < 1)
                {
                    return new Respbase<List<GetNearbyDto>> { Result = -1, Message = "保存用户信息失败", Data = null };
                }
            }
            var query = from nearby in db.TpxinNearbySet
                        where nearby.Nodeid != req.Nodeid
                        && nearby.Createtime > time
                        orderby Functions.GetJuLi(req.Longitude, req.Latitude, nearby.Longitude, nearby.Latitude)
                        select new GetNearbyDto
                        {
                            Latitude = nearby.Latitude,
                            Longitude = nearby.Longitude,
                            Photo = nearby.Photo,
                            Nodeid = nearby.Nodeid,
                            Nickname = nearby.Nickname,
                            distance = Functions.GetJuLi(req.Longitude, req.Latitude, nearby.Longitude, nearby.Latitude)
                        };
            query = query.Where(w => w.distance <= (300 * 1000));
            List<GetNearbyDto> purselist = query.ToList();
            return new Respbase<List<GetNearbyDto>> { Data = purselist };
        }
        #endregion

        #region 摇一摇
        /// <summary>
        /// 摇一摇
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<GetYaoyiyaoDto> GetYaoyiyao(GetNearbyReq req)
        {
            TnetReginfo regInfo = PxinCache.GetRegInfo(req.Nodeid);
            //将本次的记录存储起来
            TpxinYaoyiyao tpxin_yaoyiyao = new TpxinYaoyiyao();
            tpxin_yaoyiyao.Nodeid = req.Nodeid;
            tpxin_yaoyiyao.Createtime = DateTime.Now;
            tpxin_yaoyiyao.Remarks = "";
            tpxin_yaoyiyao.Status = 1;
            tpxin_yaoyiyao.Nickname = regInfo.Nodename;
            tpxin_yaoyiyao.Longitude = req.Longitude;
            tpxin_yaoyiyao.Latitude = req.Latitude;
            TnetUserphoto tnet_userphoto = db.TnetUserphotoSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
            if (tnet_userphoto != null && !string.IsNullOrEmpty(tnet_userphoto.Appphoto))
            {
                tpxin_yaoyiyao.Photo = tnet_userphoto.Appphoto;
                if (!tpxin_yaoyiyao.Photo.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    tpxin_yaoyiyao.Photo = AppConfig.Userphoto + tpxin_yaoyiyao.Photo;
                }

            }
            TchatUser tchat_user = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == regInfo.Nodeid);
            if (tchat_user != null && !string.IsNullOrEmpty(tchat_user.Nickname))
            {
                tpxin_yaoyiyao.Nickname = tchat_user.Nickname;
            }

            db.TpxinYaoyiyaoSet.Add(tpxin_yaoyiyao);
            if (db.SaveChanges() <= 0)
            {
                return new Respbase<GetYaoyiyaoDto> { Result = -1, Message = "保存用户信息失败", Data = null };
            }

            DateTime time = DateTime.Now.AddMinutes(-1);//1分钟内摇一摇
            var query = from nearby in db.TpxinYaoyiyaoSet
                        where nearby.Nodeid != req.Nodeid
                            && nearby.Createtime > time
                            && nearby.Status == 1
                        orderby nearby.Createtime descending
                        select new GetYaoyiyaoDto
                        {
                            Photo = nearby.Photo,
                            Nodeid = nearby.Nodeid,
                            Nickname = nearby.Nickname,
                            distance = Functions.GetJuLi(req.Longitude, req.Latitude, nearby.Longitude, nearby.Latitude)
                        };

            GetYaoyiyaoDto purselist = query.FirstOrDefault();
            return new Respbase<GetYaoyiyaoDto> { Data = purselist };
        }
        #endregion

        #region 通用倍率
        /// <summary>
        /// 获取通用倍率
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<CommonRateDto> GetCommonRate(CommonRateReq req)
        {
            var tchatReceiverRate = db.TchatRateSet.FirstOrDefault(p => p.Sender == req.Sender && p.Receiver == req.Receiver && p.Typeid == req.Type);
            var rate = tchatReceiverRate == null ? 1 : tchatReceiverRate.Rate;
            return new Respbase<CommonRateDto> { Data = new CommonRateDto() { Rate = rate ?? 1 } };
        }
        //public Respbase<CommonRateDto> SetCommonRate(CommonRateReq req)
        //{

        //}
        #endregion


        public string GetChatHis()
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("date", "2019082311");

            String postStr = buildQueryStr(dicList);

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            var tp = (int)(DateTime.Now - startTime).TotalSeconds;

            Random rd = new Random();
            int rd_i = rd.Next();
            String nonce = Convert.ToString(rd_i);

            String timestamp = Convert.ToString(tp);

            String signature = GetHash("GaPOjBgmxknD" + nonce + timestamp);

            //ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://api-cn.ronghub.com/message/history.json");

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";

            myRequest.Headers.Add("App-Key", "25wehl3uwm6bw");
            myRequest.Headers.Add("Nonce", nonce);
            myRequest.Headers.Add("Timestamp", timestamp);
            myRequest.Headers.Add("Signature", signature);

            byte[] data = System.Text.Encoding.UTF8.GetBytes(postStr);
            myRequest.ContentLength = postStr.Length;

            Stream newStream = myRequest.GetRequestStream();

            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            System.Net.HttpWebResponse myResponse = null;
            try
            {
                myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);

                string content = reader.ReadToEnd();
                return content;
            }
            //异常请求
            catch (System.Net.WebException e)
            {
                myResponse = (System.Net.HttpWebResponse)e.Response;
                using (Stream errData = myResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(errData))
                    {
                        string text = reader.ReadToEnd();

                        return text;
                    }
                }
            }
        }

        private String GetHash(String input)
        {
            //建立SHA1对象
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] dataToHash = enc.GetBytes(input);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
        private static String buildQueryStr(Dictionary<String, String> dicList)
        {
            String postStr = "";

            foreach (var item in dicList)
            {
                postStr += item.Key + "=" + HttpUtility.UrlEncode(item.Value, System.Text.Encoding.UTF8) + "&";
            }
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
            return postStr;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GtResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOnline { get { return status.Equals("successed_online", StringComparison.OrdinalIgnoreCase); } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get { return result.Equals("ok", StringComparison.OrdinalIgnoreCase); } }
    }


}
