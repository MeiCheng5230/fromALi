using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 行驶证历史表
    /// </summary>
    public partial class TnetVehicleLicLog
    {
        /// <summary>
        /// 
        /// </summary>
        public TnetVehicleLicLog()
        {
            Id = 0;
            Nodeid = 0;
            Brandmodel = null;
            Engineno = null;
            Licplateno = null;
            Belonger = null;
            Cardimg = null;
            Status = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  NUMBER(10)

        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  品牌型号
        ///</summary>
        public string Brandmodel { get; set; }
        /// <summary>
        ///  发证日期
        ///</summary>
        public string Firtdate { get; set; }
        /// <summary>
        ///  使用性质
        ///</summary>
        public string Usenature { get; set; }
        /// <summary>
        ///  发动机号码
        ///</summary>
        public string Engineno { get; set; }
        /// <summary>
        ///  号牌号码
        ///</summary>
        public string Licplateno { get; set; }
        /// <summary>
        ///  所有人
        ///</summary>
        public string Belonger { get; set; }
        /// <summary>
        ///  住址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  注册日期
        ///</summary>
        public string Registertime { get; set; }
        /// <summary>
        ///  车辆识别代号
        ///</summary>
        public string Carliccode { get; set; }
        /// <summary>
        ///  车辆类型
        ///</summary>
        public string Cartype { get; set; }
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
        ///  是否允许修改 1=审核通过 2=审核拒绝
        ///</summary>
        public int Status { get; set; }
    
        
    }
}