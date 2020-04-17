using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 用户信息表扩展
    /// </summary>
    public partial class TnetReginfoExt
    {
        public TnetReginfoExt()
        { 
Extid = 0;                                         
Nodeid = 0;                                         
Brightness = 0;                                         
Gradeid = 0;                                         
Hbnum = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Extid { get; set; }
        /// <summary>
        ///  nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  微信号
        ///</summary>
        public string Weixin { get; set; }
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  宝石亮度
        ///</summary>
        public decimal Brightness { get; set; }
        /// <summary>
        ///  宝石等级
        ///</summary>
        public int Gradeid { get; set; }
        /// <summary>
        ///  常在省
        ///</summary>
        public string Province { get; set; }
        /// <summary>
        ///  常在市
        ///</summary>
        public string City { get; set; }
        /// <summary>
        ///  常在区
        ///</summary>
        public string Area { get; set; }
        /// <summary>
        ///  国家 1=中国 0=外国
        ///</summary>
        public int? Country { get; set; }
        /// <summary>
        ///  个推Clientid
        ///</summary>
        public string Gtclientid { get; set; }
        /// <summary>
        ///  IOS专用
        ///</summary>
        public string Devicetoken { get; set; }
        /// <summary>
        ///  百度人脸库-用户组ID
        ///</summary>
        public string BaiduFaceGroupId { get; set; }
        /// <summary>
        ///  百度人脸库-人脸图片的唯一标识
        ///</summary>
        public string BaiduFaceToken { get; set; }
        /// <summary>
        ///  百度人脸库-注册百度人脸库时使用的身份证号码
        ///</summary>
        public string BaiduFaceIdcard { get; set; }
        /// <summary>
        ///  身份证正面照
        ///</summary>
        public string Idcardpic1 { get; set; }
        /// <summary>
        ///  身份证人脸照
        ///</summary>
        public string Idcardpic2 { get; set; }
        /// <summary>
        ///  身份证反面照
        ///</summary>
        public string Idcardpic3 { get; set; }
        /// <summary>
        ///  开通第三方支付时的唯一标记
        ///</summary>
        public string Token { get; set; }
        /// <summary>
        ///  红包资格数量,优谷专用
        ///</summary>
        public int Hbnum { get; set; }
    
        
    }
}