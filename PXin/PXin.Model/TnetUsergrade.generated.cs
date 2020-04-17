using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// TnetUsergrade
    /// </summary>
    public partial class TnetUsergrade
    {
        public TnetUsergrade()
        {
            Sid = 81100;
            Typeid = 1;
            Rate = 1;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Idno { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Gradeid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Createtime { get; set; }
        /// <summary>
        /// APPID
        /// </summary>
        public int Sid { get; set; }
        /// <summary>
        /// 对应激活码类型
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 领取红包的倍率
        /// </summary>
        public decimal Rate { get; set; }
    }
}
