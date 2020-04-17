using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 通过内部钱包认证用户请求参数
    /// </summary>
    public class AuthenByPurseReq : Reqbase
    {
        /// <summary>
        /// 身份证正面照片Url地址
        /// </summary>
        [Required]
        public string CardFrontPicUrl { get; set; }
        /// <summary>
        /// 身份证背面照片Url地址
        /// </summary>
        [Required]
        public string CardBackPicUrl { get; set; }
        /// <summary>
        /// 手持身份证照片Url地址
        /// </summary>
        [Required]
        public string HoldCardPicUrl { get; set; }
        /// <summary>
        /// 百度接口评分
        /// </summary>
        [Required]
        public string BaiDuRet { get; set; }
    }

    /// <summary>
    /// 获取PCN中的身份证正面照
    /// </summary>
    public class GetIDCardPicFromPcnReq : Reqbase
    {
        /// <summary>
        /// PCN用户登录账号
        /// </summary>
        [Required]
        public string PcnNodecode { get; set; }
        /// <summary>
        /// PCN用户登录密码(Base64)
        /// </summary>
        [Required]
        public string PcnLoginPwd { get; set; }
    }

    /// <summary>
    /// 通过PCN认证请求参数
    /// </summary>
    public class AuthByPCNReq : Reqbase
    {
        /// <summary>
        /// PCN用户登录账号
        /// </summary>
        [Required]
        public string PcnNodecode { get; set; }
        /// <summary>
        /// PCN用户登录密码(Base64)
        /// </summary>
        [Required]
        public string PcnLoginPwd { get; set; }
        /// <summary>
        /// PCN用户认证身份证正面照片地址
        /// </summary>
        [Required]
        public string PcnIDCardFrontPicUrl { get; set; }
        /// <summary>
        /// 活体照片地址
        /// </summary>
        [Required]
        public string LivingPicUrl { get; set; }
    }

    /// <summary>
    /// 驾驶证认证
    /// </summary>
    public class AuthDriverLicenseReq : Reqbase
    {
        /// <summary>
        /// 驾驶证正面照
        /// </summary>
        [Required]
        public string FrontImgUrl { get; set; }
        /// <summary>
        /// 驾驶证附页
        /// </summary>
        [Required]
        public string AppendixImgUrl { get; set; }
        /// <summary>
        /// 档案号
        /// </summary>
        [Required]
        public string FileNo { get; set; }
    }
}
