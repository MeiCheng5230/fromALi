using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public partial class TzcAuthLog
    {
        /// <summary>
        /// 
        /// </summary>
        public TzcAuthLog()
        {
            Infoid = 0;
            Nodeid = 0;
            Status = 0;
            Payment = 0;
            Isidentify = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  真实姓名
        ///</summary>
        public string Realname { get; set; }
        /// <summary>
        ///  性别，1-男，2-女
        ///</summary>
        public int? Sex { get; set; }
        /// <summary>
        ///  生日
        ///</summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        ///  身份证号码
        ///</summary>
        public string Idcard { get; set; }
        /// <summary>
        ///  身份证正面照
        ///</summary>
        public string Idcardpic1 { get; set; }
        /// <summary>
        ///  身份证手持照
        ///</summary>
        public string Idcardpic2 { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  状态,-1-未完成,0-等待审核，1-通过，2-拒绝，4-资料不完整，5-已通过实人认证
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  审核人
        ///</summary>
        public int? Auditnodeid { get; set; }
        /// <summary>
        ///  审核时间
        ///</summary>
        public DateTime? Audittime { get; set; }
        /// <summary>
        ///  支付方式,0-CV,1-认证码,2-PCN绑定
        ///</summary>
        public int Payment { get; set; }
        /// <summary>
        ///  0-未识别，1-识别成功，2-识别失败
        ///</summary>
        public int Isidentify { get; set; }
        /// <summary>
        ///  民族
        ///</summary>
        public string Race { get; set; }
        /// <summary>
        ///  住址
        ///</summary>
        public string Address { get; set; }
        /// <summary>
        ///  签发机关
        ///</summary>
        public string IssuedBy { get; set; }
        /// <summary>
        ///  有效日期
        ///</summary>
        public string ValidDate { get; set; }
        /// <summary>
        ///  身份证反面照
        ///</summary>
        public string Idcardpic3 { get; set; }
        /// <summary>
        ///  百度API返回的对比结果
        ///</summary>
        public string Baiduapiret { get; set; }


    }
}