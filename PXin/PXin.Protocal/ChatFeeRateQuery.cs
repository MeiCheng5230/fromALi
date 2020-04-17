using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    [Serializable]
    public class ChatFeeRateQuery : MessageBase<ChatFeeRateQueryBody>
    {
        public ChatFeeRateQuery()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateQuery, Util.GetSquence())
        {
        }
        public ChatFeeRateQuery(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateQueryBody
    {
        public int Type { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
    }
    [Serializable]
    public class ChatFeeRateQueryResp : MessageBase<ChatFeeRateQueryRespBody>
    {
        public ChatFeeRateQueryResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeRateQueryResp, squeueID)
        {
        }
        public ChatFeeRateQueryResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRateQueryRespBody
    {
        public int Type { get; set; }
        public int Sender { get; set; }
        public decimal ReceiverRate { get; set; }
        public decimal SenderRate { get; set; }
        public decimal VDianBalance { get; set; }
        public decimal PDianBalance { get; set; }
    }
}
