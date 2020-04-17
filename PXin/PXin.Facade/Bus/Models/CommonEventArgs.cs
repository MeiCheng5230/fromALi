using PXin.Facade.Bus.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public IEvent Event { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        public CommonEventArgs(IEvent @event)
        {
            this.Event = @event;
        }
    }
}
