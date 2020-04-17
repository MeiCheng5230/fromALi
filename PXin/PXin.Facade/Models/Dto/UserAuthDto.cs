using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 用户认证信息内容
    /// </summary>
    public class UserAuthInfoDto
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }
        /// <summary>
        /// 认证过期时间(yyyy-MM-dd Or "尚未认证")
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 认证状态(-2：未认证、-1：未完成、0：等待审核、1：通过、2：拒绝、4：资料不完整、5：已通过实人认证)
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 用户认证图片
    /// </summary>
    public class UserAuthPic
    {
        /// <summary>
        /// 手持照
        /// </summary>
        public string PicUrl1 { get; set; }
        /// <summary>
        /// 身份证正面照
        /// </summary>
        public string PicUrl2 { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DriverLicenseDto
    {
        /// <summary>
        /// 驾驶证图片地址
        /// </summary>
        public string DriverLicenseUrl { get; set; }
        /// <summary>
        /// 驾驶证附页地址
        /// </summary>
        public string AppendixUrl { get; set; }
        /// <summary>
        /// 档案号
        /// </summary>
        public string FileNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 证件审核状态,1=审核通过 2=审核拒绝
        /// </summary>
        public int Status { get; set; }
    }
}
