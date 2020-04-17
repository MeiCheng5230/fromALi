using PXin.Facade.Bus.Interface;
using PXin.Facade.SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class AddFriendNoticeEventHandler : IEventHandler<AddFriendNoticeEvent>
    {
        private readonly static SignalRClient<string, BelieveCasClient> _connections = SignalRClient<string, BelieveCasClient>.GetInstance();
        /// <summary>
        /// 是否处理该事件
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public bool CanHandle(IEvent @event)
        => @event.GetType().Equals(typeof(AddFriendNoticeEvent));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
        => CanHandle(@event) ? HandleAsync((AddFriendNoticeEvent)@event, cancellationToken) : Task.Run(() => { });
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public Task HandleAsync(AddFriendNoticeEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                //_connections.GetConnections().FirstOrDefault().Value.Client.addFriendMessage(@event.Message);
            });
        }
    }
}
