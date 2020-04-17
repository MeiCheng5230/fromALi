using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人扩展表2(职业/教育)
    /// </summary>
    public partial class TpxinDarenExt2
    {
        public TpxinDarenExt2()
        { 
Extid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Showname = string.Empty;                                         
Education = string.Empty;                                         
Fromtime = new DateTime();                                         
Endtime = new DateTime();                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Extid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型 0=教育 1=职业
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  学校/公司机构
        ///</summary>
        public string Showname { get; set; }
        /// <summary>
        ///  学历/头衔
        ///</summary>
        public string Education { get; set; }
        /// <summary>
        ///  图片的完整URL地址,多个用逗号隔开
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  从时间
        ///</summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        ///  到时间,当大于当前时间表示至今
        ///</summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  专业
        ///</summary>
        public string Subject { get; set; }
    
        
    }
}