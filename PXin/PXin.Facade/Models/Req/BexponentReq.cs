using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 获取列表分页请求
    /// </summary>
    public class GetByPageBase : Reqbase
    {
        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int PageNum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HkInfoDetail : Reqbase
    {
        /// <summary>
        /// infoid
        /// </summary>
        public int Infoid { get; set; }
    }
}
