using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    [Serializable]
    public class ChatFeeRateSetPush : MessageBase<ChatFeeRateSetPushBody>
    {
        public ChatFeeRateSetPush()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateSetPush, Util.GetSquence())
        {
        }
        public ChatFeeRateSetPush(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateSetPushBody
    {
        public decimal Rate { get; set; }
        public int SNodeId { get; set; }
    }
    [Serializable]
    public class ChatFeeRateSetPushResp : MessageBase<ChatFeeRateSetPushRespBody>
    {
        public ChatFeeRateSetPushResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateSetPushResp, squeueID)
        {
        }
        public ChatFeeRateSetPushResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateSetPushRespBody
    {
    }
}
