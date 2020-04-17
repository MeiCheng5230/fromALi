using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong
{
    public class RongResult
    {
        //{"url":"/user/getToken.json","code":1002,"errorMessage":"invalid App-Key."}
        public string code { get; set; }
        public string url { get; set; }
        public string errorMessage { get; set; }
        public bool Result { get { return code == "200"; } }
    }
    public class RongResultToken : RongResult
    {
        //{"code":200,"userId":"3434909","token":"bUoNs9ScJMbXNQOwQ7P4W/L8Rq/jVYYxQLqZnxPmZEkUUqWznVQe9OPJaRIC/DzPVulp5KKa/xw1kpG10TxBawwOBa/JJVOe"}
        public string userId { get; set; }
        public string token { get; set; }
    }
    //{"code":200,"chatRooms":[{"chrmId":"1040","name":"P客大家庭","time":"2016-06-13 18:9:4"}]}
    public class RongChatRoom : RongResult
    {
        public ChatRoomItem[] chatRooms { get; set; }
    }
    public class ChatRoomItem
    {
        public string chrmId { get; set; }
        public string name { get; set; }
        public string time { get; set; }
    }
    public class RongChatRoomUser : RongResult
    {
        //{"code":200,"total":1,"users":[{"time":"2016-06-17 16:37:51","id":"3434909"}]}
        public int total { get; set; }
        public ChatRoomUserItem[] users { get; set; }
    }
    public class ChatRoomUserItem
    {
        public string id { get; set; }
        public string time { get; set; }
    }
    public class RongGagQueryResult : RongResult
    {
        public GagUser[] users { get; set; }
    }
    public class GagUser
    {
        public string time { get; set; }
        public string userId { get; set; }
    }
}
