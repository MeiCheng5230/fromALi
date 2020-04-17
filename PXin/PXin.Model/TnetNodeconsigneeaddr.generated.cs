using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 收货人信息管理表
    /// </summary>
    public partial class TnetNodeconsigneeaddr
    {
        public TnetNodeconsigneeaddr()
        { 
Consigneeid = 0;                                         
Nodeid = 0;                                         
Countryid = 0;                                         
Provinceid = 0;                                         
Cityid = 0;                                         
Regionid = 0;                                         
Address = string.Empty;                                         
Consigneename = string.Empty;                                         
Isdefault = 0;                                         
Isspecial = 0;                                         
Isdel = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Consigneeid { get; set; }
        /// <summary>
        ///  关联Tnet_reginfo
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  国家
        ///</summary>
        public int Countryid { get; set; }
        /// <summary>
        ///  省份
        ///</summary>
        public int Provinceid { get; set; }
        /// <summary>
        ///  城市
        ///</summary>
        public int Cityid { get; set; }
        /// <summary>
        ///  区
        ///</summary>
        public int Regionid { get; set; }
        /// <summary>
        ///  详细地址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  邮政编码
        ///</summary>
        public string Postcode { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  收货人名称
        ///</summary>
        public string Consigneename { get; set; }
        /// <summary>
        ///  收货人手机号码
        ///</summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  收货人电话号码
        ///</summary>
        public string Phone { get; set; }
        /// <summary>
        ///  乡镇ID_ 对应表 Tnet_Town
        ///</summary>
        public int? TownId { get; set; }
        /// <summary>
        ///  是否是默认收货地址 0-否 1-是
        ///</summary>
        public int Isdefault { get; set; }
        /// <summary>
        ///  是否为专用收货地址
        ///</summary>
        public int Isspecial { get; set; }
        /// <summary>
        ///  高德经度 IOS用
        ///</summary>
        public string Longitude { get; set; }
        /// <summary>
        ///  高德维度 IOS用
        ///</summary>
        public string Latitude { get; set; }
        /// <summary>
        ///  百度经度 安卓用
        ///</summary>
        public string Longitude2 { get; set; }
        /// <summary>
        ///  百度维度 安卓用
        ///</summary>
        public string Latitude2 { get; set; }
        /// <summary>
        ///  是否删除   0=未删除 1=已删除

        ///</summary>
        public int Isdel { get; set; }
    
        
    }
}