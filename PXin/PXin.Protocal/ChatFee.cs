using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Protocal
{
    [Serializable]
    public class ChatFee : MessageBase<ChatFeeBody>
    {
        public ChatFee()
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFee, Util.GetSquence())
        {
        }
        public ChatFee(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeBody
    {
        public uint FeeType { get; set; }
        public uint BusinessType { get; set; }
        public decimal Num { get; set; }
        public decimal Rate { get; set; }
        public uint ReceiveType { get; set; }
        public string Receiver { get; set; }
        public DateTime FeeTime { get; set; }
    }
    [Serializable]
    public class ChatFeeResp : MessageBase<ChatFeeRespBody>
    {
        public ChatFeeResp(long squeueID)
            : base(MessageHeader.Length, PXin_COMMAND_TYPE.ChatFeeResp, squeueID)
        {
        }
        public ChatFeeResp(byte[] bytes)
            : base(bytes)
        {
        }
    }
    [Serializable]
    public class ChatFeeRespBody
    {
        public uint Status { get; set; }
        public string StatusDesc { get; set; }
        public uint FeeType { get; set; }
        public decimal VDianBalance { get; set; }
        public decimal PDianBalance { get; set; }
    }
}
