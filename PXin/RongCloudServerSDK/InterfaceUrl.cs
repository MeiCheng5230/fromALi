using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong
{
    class InterfaceUrl
    {
        public static String server_addr = "http://api.cn.ronghub.com";

        public static String getTokenUrl = server_addr + "/user/getToken.json";
        public static String refreshUserUrl = server_addr + "/user/refresh.json";
        public static String createGroupUrl = server_addr + "/group/create.json";
        public static String refreshGroupUrl = server_addr + "/group/refresh.json";
        public static String joinGroupUrl = server_addr + "/group/join.json";
        public static String quitGroupUrl = server_addr + "/group/quit.json";
        public static String gagaddGroupUrl = server_addr + "/group/user/gag/add.json";
        public static String gagrollbakGroupUrl = server_addr + "/group/user/gag/rollback.json";
        public static String gaglistGroupUrl = server_addr + "/group/user/gag/list.json";
        public static String dismissUrl = server_addr + "/group/dismiss.json";
        public static String syncGroupUrl = server_addr + "/group/sync.json";
        public static String sendMsgUrl = server_addr + "/message/private/publish.json";
        public static String sendSystemMsgUrl = server_addr + "/message/system/publish.json";
        public static String broadcastUrl = server_addr + "/message/broadcast.json";
        public static String createChatroomUrl = server_addr + "/chatroom/create.json";
        public static String joinChatroomUrl = server_addr + "/chatroom/join.json";
        public static String destroyChatroomUrl = server_addr + "/chatroom/destroy.json";
        public static String queryChatroomUrl = server_addr + "/chatroom/query.json";
        public static String queryChatroomUserUrl = server_addr + "/chatroom/user/query.json";
        public static String pushUrl = server_addr + "/push.json";
        //public static String getTokenUrl = server_addr + "/user/getToken.xml";
        //public static String joinGroupUrl = server_addr + "/group/join.xml";
        //public static String quitGroupUrl = server_addr + "/group/quit.xml";
        //public static String dismissUrl = server_addr + "/group/dismiss.xml";
        //public static String syncGroupUrl = server_addr + "/group/sync.xml";
        //public static String SendMsgUrl = server_addr + "/message/publish.xml";
        //public static String broadcastUrl = server_addr + "/message/broadcast.xml";
        //public static String createChatroomUrl = server_addr + "/chatroom/create.xml";
        //public static String destroyChatroomUrl = server_addr + "/chatroom/destroy.xml";
        //public static String queryChatroomUrl = server_addr + "/chatroom/query.xml";

    }
}
