using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 自动关注公众号用户
    /// </summary>
    public partial class TchatPublicUser
    {
        public TchatPublicUser()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公众号id
        /// </summary>
        public string PublicId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int NodeId { get; set; }
    }
}
