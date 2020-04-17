using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 图片类别, 1：身份证正面图片；2：身份证反面图片；3：百度人脸识别图片；4：用户头像；5意见反馈；6驾驶证行驶证及副页
    /// </summary>
    public enum FileActionType : int
    {
        /// <summary>
        /// 
        /// </summary>
        临时 = 0,
        /// <summary>
        /// 
        /// </summary>
        身份证正面图片 = 1,
        /// <summary>
        /// 
        /// </summary>
        身份证反面图片 = 2,
        /// <summary>
        /// 
        /// </summary>
        百度人脸识别图片 = 3,
        /// <summary>
        /// 
        /// </summary>
        用户头像 = 4,
        /// <summary>
        /// 
        /// </summary>
        意见反馈 = 5,
        /// <summary>
        /// 
        /// </summary>
        驾驶证行驶证 = 6,

        /// <summary>
        /// 
        /// </summary>
        驾驶证副页 = 7,
    }
}
