using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    [Serializable]
    public class ChatFeePush : MessageBase<ChatFeePushBody>
    {
        public ChatFeePush()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeePush, Util.GetSquence())
        {
        }
        public ChatFeePush(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeePushBody
    {
        public decimal PDianBalance { get; set; }
        public decimal PDianPush { get; set; }
    }
    [Serializable]
    public class ChatFeePushResp : MessageBase<ChatFeePushRespBody>
    {
        public ChatFeePushResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeePushResp, squeueID)
        {
        }
        public ChatFeePushResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeePushRespBody
    {
    }
}
