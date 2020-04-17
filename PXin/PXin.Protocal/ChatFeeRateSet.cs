using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    [Serializable]
    public class ChatFeeRateSet : MessageBase<ChatFeeRateSetBody>
    {
        public ChatFeeRateSet()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateSet, Util.GetSquence())
        {
        }
        public ChatFeeRateSet(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateSetBody
    {
        public int Type { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public decimal Rate { get; set; }
    }
    [Serializable]
    public class ChatFeeRateSetResp : MessageBase<ChatFeeRateSetRespBody>
    {
        public ChatFeeRateSetResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateSetResp, squeueID)
        {
        }
        public ChatFeeRateSetResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateSetRespBody
    {
        public uint Status { get; set; }
        public decimal Rate { get; set; }
    }
}
