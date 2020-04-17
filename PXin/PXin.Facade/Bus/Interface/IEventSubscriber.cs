using MediatR;
using PXin.Facade.Bus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventSubscriber
    {
        /// <summary>
        /// 事件订阅
        /// </summary>
        void Subscribe(EventHandler<CommonEventArgs> @event);
    }
}
