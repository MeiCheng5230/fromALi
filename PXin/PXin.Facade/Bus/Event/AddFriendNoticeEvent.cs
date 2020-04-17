using PXin.Facade.Bus.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class AddFriendNoticeEvent : IEvent
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public AddFriendNoticeEvent(string message)
        {
            Message = message;
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id => new Guid();
        /// <summary>
        /// 
        /// </summary>
        public long Timestamp => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}
