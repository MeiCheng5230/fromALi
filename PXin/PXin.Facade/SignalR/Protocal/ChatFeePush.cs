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
    public class ChatFeePush : MessageBase<ChatFeePushBody>
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatFeePush()
            : base(PXinCommandType.ChatFeePush, Util.GetSquence())
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeePushBody
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal PDianBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal PDianPush { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ChatFeePushResp
    {

    }
}
