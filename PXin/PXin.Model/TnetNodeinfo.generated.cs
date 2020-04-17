using System;

namespace PXin.Model
{
    /// <summary>
    /// 注册信息，对应个人用户
    /// </summary>
    public partial class TnetNodeinfo
    {
        /// <summary>
        /// 
        /// </summary>
        public TnetNodeinfo()
        {
            Nodeid = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  身份证号码
        ///</summary>
        public string Idcardno { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Idcardaddr { get; set; }
        public string Contactname { get; set; }
        public string Contacttel { get; set; }
        public string Company { get; set; }
        public string Comregionid { get; set; }
        public string Companyaddr { get; set; }
        public string Famregionid { get; set; }
        public string Familyaddr { get; set; }
        public string Othregionid { get; set; }
        public string Otheraddr { get; set; }
        public int? Defaultaddr { get; set; }
        public string Remarks { get; set; }
        public DateTime Createtime { get; set; }
        public string Name { get; set; }
        public int? Sex { get; set; }
        /// <summary>
        ///  民族
        ///</summary>
        public string Nation { get; set; }
        /// <summary>
        ///  有效期开始时间
        ///</summary>
        public DateTime? Beginvalidity { get; set; }
        /// <summary>
        ///  有效期结束时间
        ///</summary>
        public DateTime? Endvalidity { get; set; }
        /// <summary>
        ///  签发机关
        ///</summary>
        public string Issuing { get; set; }


    }
}