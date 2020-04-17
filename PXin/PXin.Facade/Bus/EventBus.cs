using PXin.Facade.Bus.Interface;
using PXin.Facade.Bus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Bus
{
    /// <summary>
    /// 
    /// </summary>
    public class EventBus : IEventBus
    {
        private event EventHandler<CommonEventArgs> Event;
        /// <summary>
        /// 
        /// </summary>

        private List<IEventHandler> _eventHandlers;

        private EventBus() { }
        private static EventBus _EventBus = null;
        private static object EventBus_Lock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static EventBus GetInstance()
        {
            if (_EventBus == null)
            {
                lock (EventBus_Lock)
                {
                    if (_EventBus == null)
                    {
                        _EventBus = new EventBus();
                    }
                }
            }
            return _EventBus;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public EventBus Register(IEventHandler eventHandler)
        {
            _eventHandlers = _eventHandlers == null ? new List<IEventHandler>() : _eventHandlers;
            _eventHandlers.Add(eventHandler);
            return _EventBus;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Publish(IEvent @event)
        {
            return Task.Run(() =>
            {
                Event?.Invoke(this, new CommonEventArgs(@event));
            });
        }
        /// <summary>
        /// 
        /// </summary>
        public void Subscribe()
        {
            Event += (sender, e) =>
            {
                _eventHandlers.Where(eh => eh.CanHandle(e.Event)).ToList().ForEach(async eh =>
                {
                    await eh.HandleAsync(e.Event);
                });
            };
        }
    }
}
