using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAddressReq : Reqbase
    {
        /// <summary>
        /// 省id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 市id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 区id
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddrDetail { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Moblie { get; set; }
        /// <summary>
        /// 是否默认地址 0=否 1=是
        /// </summary>
        public int IsDefaultAddr { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EditAddressReq : CreateAddressReq
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ConsigneeId { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DeleteAddressReq : Reqbase
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ConsigneeId { get; set; }
    }
}
