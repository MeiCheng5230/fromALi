using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Bus.Interface
{
    /// <summary>
    /// 发布接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <returns></returns>
        Task Publish();
    }
}
