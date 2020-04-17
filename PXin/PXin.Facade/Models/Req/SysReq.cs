using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// App下载地址
    /// </summary>
    public class AppDownloadDto
    {
        /// <summary>
        /// IosApp下载地址
        /// </summary>
        public string Ios { get; set; }
        /// <summary>
        /// 安卓App下载地址
        /// </summary>
        public string Android { get; set; }
    }
    /// <summary>
    /// 获取手机国际区号Req
    /// </summary>
    public class AreaCodeReq : Reqbase
    {
    }
}
