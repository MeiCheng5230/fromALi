using PXin.Facade.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Protocal
{
    public class ChatFee : MessageBase<ChatFeeBody>
    {
        public ChatFee()
            : base(PXinCommandType.ChatFee, Util.GetSquence())
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageString"></param>
        public ChatFee(string messageString)
            : base(messageString)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeeResp : MessageBase<ChatFeeRespBody>
    {
        public ChatFeeResp(long sequenceId) :
            base(PXinCommandType.ChatFeeResp, sequenceId)
        {

        }
    }
    public class ChatFeeRespBody
    {
        public uint Status { get; set; }
        public string StatusDesc { get; set; }
        public uint FeeType { get; set; }
        public decimal VDianBalance { get; set; }
        public decimal PDianBalance { get; set; }
    }

}
