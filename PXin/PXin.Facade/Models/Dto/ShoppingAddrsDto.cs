using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ShoppingAddrsDto
    {
        /// <summary>
        ///  主键
        ///</summary>
        public int Consigneeid { get; set; }
        /// <summary>
        ///  姓名
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  手机号
        ///</summary>
        public string Phone { get; set; }
        /// <summary>
        ///  地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  省市区地址
        ///</summary>
        public string PCRAddress { get; set; }
        /// <summary>
        ///  省id
        ///</summary>
        public int ProvinceId { get; set; }
        /// <summary>
        ///  市id
        ///</summary>
        public int CityId { get; set; }
        /// <summary>
        ///  区id
        ///</summary>
        public int RegionId { get; set; }
        /// <summary>
        ///  是否默认
        ///</summary>
        public int IsDefault { get; set; }
    }
}
