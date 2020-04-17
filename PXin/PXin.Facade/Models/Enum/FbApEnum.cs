using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Enum
{
    /// <summary>
    /// 认证资料类型
    /// </summary>
    public enum AuthDataType
    {
        /// <summary>
        /// 个人资料
        /// </summary>
        Personal = 1,
        /// <summary>
        /// 公司资料
        /// </summary>
        Company = 2,
        /// <summary>
        /// 合同照片
        /// </summary>
        Contract = 3,
        /// <summary>
        /// 补充资料
        /// </summary>
        Supplement = 4
    }
    /// <summary>
    /// 图片作用类型
    /// </summary>
    public enum ImageActionType
    {
        /// <summary>
        /// 身份证正面图片
        /// </summary>
        IdentFront = 1,
        /// <summary>
        /// 身份证反面图片
        /// </summary>
        IdentBack = 2,
        /// <summary>
        /// 手持身份证正面照
        /// </summary>
        Hold = 3,
        /// <summary>
        /// 公司照片
        /// </summary>
        Company = 4,
        /// <summary>
        /// 营业执照
        /// </summary>
        License = 5,
        /// <summary>
        /// 开户许可证
        /// </summary>
        Permit = 6,
        /// <summary>
        /// 租赁合同
        /// </summary>
        Lease = 7,
        /// <summary>
        /// 合同
        /// </summary>
        Contract = 8,
    }
}
