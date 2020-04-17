using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Commu.Model
{
    public class PushPDian
    {
        /// <summary>
        /// 接收者NodeId
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 给接收者推送的P点
        /// </summary>
        public decimal PDianPush { get; set; }
        /// <summary>
        /// 接收者P点余额
        /// </summary>
        public decimal PDianBalance { get; set; }
    }
}
