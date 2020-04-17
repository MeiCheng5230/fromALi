using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
    /// <summary>
    /// 充值商配车情况视图
    /// </summary>
    public class VblJxsPeiche
    {
        /// <summary>
        /// 充值商标识
        /// </summary>
        public int Infoid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 配车状态
        /// </summary>
        public int PeicheStatus { get; set; }
        /// <summary>
        /// 配车状态显示
        /// </summary>
        public string PeicheStatusShow { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApprovalStatus { get; set; }
        /// <summary>
        /// 审批状态显示
        /// </summary>
        public string ApprovalStatusShow { get; set; }
        /// <summary>
        /// 冻结状态
        /// </summary>
        public int FreezeStatus { get; set; }
        /// <summary>
        /// 冻结状态显示
        /// </summary>
        public string FreezeStatusShow { get; set; }
        /// <summary>
        /// 批发码总计
        /// </summary>
        public decimal PFM { get; set; }
        /// <summary>
        /// 回收SVC充值码总计
        /// </summary>
        public decimal SVC { get; set; }
    }
}
