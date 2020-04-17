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
    /// 
    /// </summary>
    public class ReqConfig : Reqbase
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? Updatetime { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReqH5Config : Reqbase
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// APP本地H5页面配置 调用JS用
    /// </summary>
    public class H5ConfigDto
    {
        /// <summary>
        /// 配置内容
        /// </summary>
        public string ChargeStr { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }
}
