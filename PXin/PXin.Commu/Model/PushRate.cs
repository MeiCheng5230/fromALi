using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Commu.Model
{
    public class PushRate
    {
        /// <summary>
        /// 发送者NodeId
        /// </summary>
        public int SNodeId { get; set; }
        /// <summary>
        /// 接收者NodeId
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 给接收者推送的倍率
        /// </summary>
        public decimal Rate { get; set; }
    }
}
