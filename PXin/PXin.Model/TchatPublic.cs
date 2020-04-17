using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信公众号
    /// </summary>
    public partial class TchatPublic
    {
        public TchatPublic()
        {
            PublicType = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公众号Id
        /// </summary>
        public string PublicId { get; set; }
        /// <summary>
        /// 公众号名称
        /// </summary>
        public string PublicName { get; set; }
        /// <summary>
        /// 公众号类型,1-所有人自动关注,2-部分人自动关注，3-用户主动关注
        /// </summary>
        public int PublicType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 公众号Logo地址，完整的url地址
        /// </summary>
        public string PublicLogo { get; set; }
    }
}
