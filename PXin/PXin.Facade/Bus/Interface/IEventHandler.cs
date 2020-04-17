using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventHandler<T> : IEventHandler
        where T : IEvent
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleAsync(T @event, CancellationToken cancellationToken = default(CancellationToken));
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 可否处理
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        bool CanHandle(IEvent @event);
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken));
    }
}
