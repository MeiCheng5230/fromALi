using Common.Facade;
using Common.Facade.Models;
using PXin.Facade.CommonService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 上传文件请求
    /// </summary>
    public class ReqUploadFile : Reqbase
    {
        /// <summary>
        /// 通过base64加密的文件/图片文件
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// 类型 jpg,jpeg,png,gif,bmp,mp4
        /// </summary>
        [Required]
        public string Typeid { get; set; }

        /// <summary>
        /// 图片作用类型（1：身份证正面图片；2：身份证反面图片；5意见反馈；6驾驶证行驶证；7驾驶证副页）
        /// </summary>
        public FileActionType ImageActionType { get; set; }
    }
    /// <summary>
    /// 上传文件回复
    /// </summary>
    public class UploadFileDto
    {
        /// <summary>
        /// 上传文件相对路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 上传文件完整路径
        /// </summary>
        public string FullUrl
        {
            get
            {
                return Common.Facade.Helper.DomainUrl + Url;
            }
        }
    }

    /// <summary>
    /// 身份证输出参数
    /// </summary>
    public class IdCardUploadFileDto : UploadFileDto
    {
        /// <summary>
        /// 身份证
        /// </summary>
        public IdentCard IdentCard { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DriverLicenseResultDto
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsCheck { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
