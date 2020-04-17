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
    public interface IEvent
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 时间戳
        /// </summary>
        long Timestamp { get; }
    }

}
