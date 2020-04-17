using PXin.Facade.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Protocal
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeeRateQuery : MessageBase<ChatFeeRateQueryBody>
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatFeeRateQuery()
            : base(PXinCommandType.ChatFeeRateQuery, Util.GetSquence())
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageString"></param>
        public ChatFeeRateQuery(string messageString)
            : base(messageString)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeeRateQueryBody
    {
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Receiver { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeeRateQueryResp : MessageBase<ChatFeeRateQueryRespBody>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceId"></param>
        public ChatFeeRateQueryResp(long sequenceId) :
            base(PXinCommandType.ChatFeeRateQueryResp, sequenceId)
        {

        }

    }
    public class ChatFeeRateQueryRespBody
    {
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal ReceiverRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal SenderRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal VDianBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal PDianBalance { get; set; }
    }
}
