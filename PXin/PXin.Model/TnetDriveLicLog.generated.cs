using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 驾使证识别历史
    /// </summary>
    public partial class TnetDriveLicLog
    {
        public TnetDriveLicLog()
        { 
Id = 0;                                         
Nodeid = 0;                                         
Cardno = null;                                         
VehicleType = null;                                         
Name = null;                                         
Cardimg = null;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  tnet_reginfo.nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  证号
        ///</summary>
        public string Cardno { get; set; }
        /// <summary>
        ///  车型
        ///</summary>
        public string VehicleType { get; set; }
        /// <summary>
        ///  姓名
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  性别
        ///</summary>
        public string Sex { get; set; }
        /// <summary>
        ///  国籍
        ///</summary>
        public string Country { get; set; }
        /// <summary>
        ///  住址
        ///</summary>
        public string Addr { get; set; }
        /// <summary>
        ///  出生日期
        ///</summary>
        public string Birthday { get; set; }
        /// <summary>
        ///  初次领证日期
        ///</summary>
        public string Firtdate { get; set; }
        /// <summary>
        ///  有效期限
        ///</summary>
        public string ValidPeriod { get; set; }
        /// <summary>
        ///  识别时间（创建时间）
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  证件图片
        ///</summary>
        public string Cardimg { get; set; }
        /// <summary>
        ///  驾驶证副业档案编号
        ///</summary>
        public string Fileno { get; set; }
        /// <summary>
        ///  是否允许修改 1=审核通过 2=审核拒绝
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  至
        ///</summary>
        public string Enddate { get; set; }
        /// <summary>
        ///  驾驶证附页
        ///</summary>
        public string CardimgAppendix { get; set; }
    
        
    }
}