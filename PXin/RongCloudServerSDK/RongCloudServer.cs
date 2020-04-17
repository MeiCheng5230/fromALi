using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Web;
using Newtonsoft.Json;

namespace io.rong
{
    public class RongCloudServer
    {

        /**
         * 构建请求参数
         */
        private static String buildQueryStr(Dictionary<String, String> dicList)
        {
            String postStr = "";

            foreach (var item in dicList)
            {
                postStr += item.Key + "=" + HttpUtility.UrlEncode(item.Value, Encoding.UTF8) + "&";
            }
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
            return postStr;
        }
        private static String buildQueryStr(List<KeyValuePair<String, String>> dicList)
        {
            String postStr = "";

            foreach (var item in dicList)
            {
                postStr += item.Key + "=" + HttpUtility.UrlEncode(item.Value, Encoding.UTF8) + "&";
            }
            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));
            return postStr;
        }
        private static String buildParamStr(String[] arrParams)
        {
            String postStr = "";

            for (int i = 0; i < arrParams.Length; i++)
            {
                if (0 == i)
                {
                    postStr = "chatroomId=" + HttpUtility.UrlDecode(arrParams[0], Encoding.UTF8);
                }
                else
                {
                    postStr = postStr + "&" + "chatroomId=" + HttpUtility.UrlEncode(arrParams[i], Encoding.UTF8);
                }
            }
            return postStr;
        }

        /**
         * 获取 token
         */
        public static RongResultToken GetToken(String appkey, String appSecret, String userId, String name, String portraitUri)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("name", name);
            dicList.Add("portraitUri", portraitUri);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.getTokenUrl, postStr);

            string retString = client.ExecutePost();
            RongResultToken result = new RongResultToken();
            try
            {
                result = JsonConvert.DeserializeObject<RongResultToken>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 刷新用户信息
         */
        public static RongResult RefreshUser(String appkey, String appSecret, String userId, String name, String portraitUri)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            if (!string.IsNullOrEmpty(name))
            {
                dicList.Add("name", name);
            }
            if (!string.IsNullOrEmpty(portraitUri))
            {
                dicList.Add("portraitUri", portraitUri);
            }
            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.refreshUserUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 创建 群组
         */
        public static RongResult CreateGroup(String appkey, String appSecret, String userId, String groupId, String groupName)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("groupId", groupId);
            dicList.Add("groupName", groupName);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.createGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 创建 群组
         */
        public static RongResult RefreshGroup(String appkey, String appSecret, String groupId, String groupName)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("groupId", groupId);
            dicList.Add("groupName", groupName);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.refreshGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 加入 群组
         */
        public static RongResult JoinGroup(String appkey, String appSecret, String userId, String groupId, String groupName)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("groupId", groupId);
            dicList.Add("groupName", groupName);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.joinGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /**
         * 加入 群组
         */
        public static RongResult JoinGroup(String appkey, String appSecret, String[] userId, String groupId, String groupName)
        {
            List<KeyValuePair<String, String>> dicList = new List<KeyValuePair<String, String>>();
            foreach (var item in userId)
            {
                dicList.Add(new KeyValuePair<string, string>("userId", item));
            }
            dicList.Add(new KeyValuePair<string, string>("groupId", groupId));
            dicList.Add(new KeyValuePair<string, string>("groupName", groupName));

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.joinGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 退出 群组
         */
        public static RongResult QuitGroup(String appkey, String appSecret, String userId, String groupId)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("groupId", groupId);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.quitGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
         * 解散 群组
         */
        public static RongResult DismissGroup(String appkey, String appSecret, String userId, String groupId)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("groupId", groupId);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.dismissUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;

        }

        /**
         * 同步群组
         */
        public static String syncGroup(String appkey, String appSecret, String userId, String[] groupId, String[] groupName)
        {

            String postStr = "userId=" + userId + "&";
            String id, name;

            for (int i = 0; i < groupId.Length; i++)
            {
                id = HttpUtility.UrlEncode(groupId[i], Encoding.UTF8);
                name = HttpUtility.UrlEncode(groupName[i], Encoding.UTF8);
                postStr += "group[" + id + "]=" + name + "&";
            }

            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.syncGroupUrl, postStr);

            return client.ExecutePost();
        }

        /**
        * 群组禁言
        */
        public static RongResult GagAddGroup(String appkey, String appSecret, String[] userId, String groupId, int minute)
        {
            List<KeyValuePair<String, String>> dicList = new List<KeyValuePair<String, String>>();
            foreach (var item in userId)
            {
                dicList.Add(new KeyValuePair<string, string>("userId", item));
            }
            dicList.Add(new KeyValuePair<string, string>("groupId", groupId));
            dicList.Add(new KeyValuePair<string, string>("minute", minute.ToString()));

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.gagaddGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /**
        * 群组禁言移除
        */
        public static RongResult GagRollbackGroup(String appkey, String appSecret, String[] userId, String groupId)
        {
            List<KeyValuePair<String, String>> dicList = new List<KeyValuePair<String, String>>();
            foreach (var item in userId)
            {
                dicList.Add(new KeyValuePair<string, string>("userId", item));
            }
            dicList.Add(new KeyValuePair<string, string>("groupId", groupId));

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.gagrollbakGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /**
        * 群组禁言查询
        */
        public static RongGagQueryResult GagListGroup(String appkey, String appSecret, String groupId)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("groupId", groupId);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.gaglistGroupUrl, postStr);

            string retString = client.ExecutePost();
            RongGagQueryResult result = new RongGagQueryResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongGagQueryResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <param name="objectName"></param>
        /// <param name="content">RC:TxtMsg消息格式{"content":"hello"}  RC:ImgMsg消息格式{"content":"ergaqreg", "imageKey":"http://www.demo.com/1.jpg"}  RC:VcMsg消息格式{"content":"ergaqreg","duration":3}</param>
        /// <returns></returns>
        public static RongResult PublishMessage(String appkey, String appSecret, String fromUserId, String toUserId, String objectName, String content, String pushContent, String pushData)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("fromUserId", fromUserId);
            dicList.Add("toUserId", toUserId);
            dicList.Add("objectName", objectName);
            dicList.Add("content", content);
            if (!string.IsNullOrEmpty(pushData))
            {
                dicList.Add("pushData", pushData);
            }
            if (!string.IsNullOrEmpty(pushContent))
            {
                dicList.Add("pushContent", pushContent);
            }

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.sendMsgUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="fromUserId">发送人用户 Id</param>
        /// <param name="toUserId">接收用户Id，提供多个本参数可以实现向多用户发送系统消息，上限为 100 人。</param>
        /// <param name="objectName">消息类型[RC:TxtMsg][RC:ContactNtf]</param>
        /// <param name="objectName">消息类型</param>
        /// <param name="content">发送消息内容，参考融云消息类型表.示例说明；如果 objectName 为自定义消息类型，该参数可自定义格式</param>
        /// <param name="pushContent">定义显示的 Push 内容，如果 objectName 为融云内置消息类型时，则发送后用户一定会收到 Push 信息。 如果为自定义消息，则 pushContent 为自定义消息显示的 Push 内容，如果不传则用户不会收到 Push 通知</param>
        /// <param name="pushData">针对 iOS 平台为 Push 通知时附加到 payload 中，Android 客户端收到推送消息时对应字段名为 pushData。</param>
        /// <returns></returns>
        public static RongResult PublishSystemMessage(String appkey, String appSecret, String fromUserId, String toUserId, String objectName, String content, String pushContent, String pushData)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("fromUserId", fromUserId);
            dicList.Add("toUserId", toUserId);
            dicList.Add("objectName", objectName);
            dicList.Add("content", content);
            if (!string.IsNullOrEmpty(pushData))
            {
                dicList.Add("pushData", pushData);
            }
            if (!string.IsNullOrEmpty(pushContent))
            {
                dicList.Add("pushContent", pushContent);
            }
            String postStr = buildQueryStr(dicList);
            System.Diagnostics.Trace.WriteLine(postStr);
            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.sendSystemMsgUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 广播消息暂时未开放
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="fromUserId"></param>
        /// <param name="objectName"></param>
        /// <param name="content">RC:TxtMsg消息格式{"content":"hello"}  RC:ImgMsg消息格式{"content":"ergaqreg", "imageKey":"http://www.demo.com/1.jpg"}  RC:VcMsg消息格式{"content":"ergaqreg","duration":3}</param>
        /// <returns></returns>
        public static RongResult BroadcastMessage(String appkey, String appSecret, String fromUserId, String objectName, String content)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("content", content);
            dicList.Add("fromUserId", fromUserId);
            dicList.Add("objectName", objectName);
            dicList.Add("pushContent", "");
            dicList.Add("pushData", "");

            String postStr = buildQueryStr(dicList);
            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.broadcastUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomInfo">chatroom[id10001]=name1001</param>
        /// <returns></returns>
        public static RongResult CreateChatroom(String appkey, String appSecret, String[] chatroomId, String[] chatroomName)
        {
            String postStr = null;

            String id, name;

            for (int i = 0; i < chatroomId.Length; i++)
            {
                id = HttpUtility.UrlEncode(chatroomId[i], Encoding.UTF8);
                name = HttpUtility.UrlEncode(chatroomName[i], Encoding.UTF8);
                postStr += "chatroom[" + id + "]=" + name + "&";
            }

            postStr = postStr.Substring(0, postStr.LastIndexOf('&'));

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.createChatroomUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomIdInfo">chatroomId=id1001</param>
        /// <returns></returns>
        public static RongResult DestroyChatroom(String appkey, String appSecret, String[] chatroomIdInfo)
        {
            String postStr = null;

            postStr = buildParamStr(chatroomIdInfo);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.destroyChatroomUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        public static RongChatRoom queryChatroom(String appkey, String appSecret, String[] chatroomId)
        {
            String postStr = null;

            postStr = buildParamStr(chatroomId);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.queryChatroomUrl, postStr);

            string retString = client.ExecutePost();
            RongChatRoom result = new RongChatRoom();
            try
            {
                result = JsonConvert.DeserializeObject<RongChatRoom>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        public static RongResult joinChatroom(String appkey, String appSecret, string userId, string roomId)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("userId", userId);
            dicList.Add("chatroomId", roomId);

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.joinChatroomUrl, postStr);

            string retString = client.ExecutePost();
            RongResult result = new RongResult();
            try
            {
                result = JsonConvert.DeserializeObject<RongResult>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appSecret"></param>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        public static RongChatRoomUser queryChatroomUser(String appkey, String appSecret, String chatroomId)
        {
            Dictionary<String, String> dicList = new Dictionary<String, String>();
            dicList.Add("chatroomId", chatroomId);
            dicList.Add("count", "1");
            dicList.Add("order", "2");

            String postStr = buildQueryStr(dicList);

            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.queryChatroomUserUrl, postStr);

            string retString = client.ExecutePost();
            RongChatRoomUser result = new RongChatRoomUser();
            try
            {
                result = JsonConvert.DeserializeObject<RongChatRoomUser>(retString);
            }
            catch (Exception)
            {
                result.errorMessage = retString;
                result.code = "-100";
            }
            return result;
        }
        public static string PushMessage(String appkey, String appSecret,string pushString)
        {
            RongHttpClient client = new RongHttpClient(appkey, appSecret, InterfaceUrl.pushUrl, pushString);

            string retString = client.ExecutePostJSON();

            return retString;

        }


        public class PushData
        {
            public string[] platform { get; set; }
            public string fromuserid { get; set; }
            public Audience audience { get; set; }
            public Message message { get; set; }
            public Notification notification { get; set; }
        }

        public class Audience
        {
            public string[] tag { get; set; }
            public string[] tag_or { get; set; }
            public string[] userid { get; set; }
            public bool is_to_all { get; set; }
        }

        public class Message
        {
            public string content { get; set; }
            public string objectName { get; set; }
        }

        public class Notification
        {
            public string alert { get; set; }
            public Ios ios { get; set; }
            public Android android { get; set; }
        }

        public class Ios
        {
            public string alert { get; set; }
            public Extras extras { get; set; }
        }

        public class Extras
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Android
        {
            public string alert { get; set; }
            public Extras1 extras { get; set; }
        }

        public class Extras1
        {
            public string id { get; set; }
            public string name { get; set; }
        }

    }
}
