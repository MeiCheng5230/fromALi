using Newtonsoft.Json;
using PXin.Facade.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.SignalR.Protocal
{
    /// <summary>
    /// 消息头
    /// </summary>
    public class MessageHeader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sequenceId"></param>
        public MessageHeader(PXinCommandType commandType, long sequenceId)
        {
            this.CommandType = commandType;
            this.SequenceId = sequenceId;
        }
        /// <summary>
        /// 
        /// </summary>
        public PXinCommandType CommandType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long SequenceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("\tMessageHeader:\r\n\t\tCommand_Id:{0}\r\n\t\tSequence_Id:{1}\r\n",
                            this.CommandType,
                            this.SequenceId);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MessageBase<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageBase(PXinCommandType commandType, long sequenceId)
        {
            this.Header = new MessageHeader(commandType, sequenceId);
            this.Body = new T();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageString"></param>
        public MessageBase(string messageString)
        {
            this.Header = JsonConvert.DeserializeObject<MessageHeader>(messageString);
            this.Body = JsonConvert.DeserializeObject<T>(messageString);
        }
        /// <summary>
        /// 
        /// </summary>
        public MessageHeader Header { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public T Body { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return "[\r\n"
               + this.Header.ToString() + "\r\n"
               + string.Format("\tMessageBody:{0}]", JsonConvert.SerializeObject(Body));
        }
    }
}
