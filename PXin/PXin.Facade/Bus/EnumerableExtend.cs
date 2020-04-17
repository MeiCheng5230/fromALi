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
    public static class EnumerableExtend
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }
    }
}
