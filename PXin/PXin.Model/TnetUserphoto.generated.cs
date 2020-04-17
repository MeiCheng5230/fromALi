using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 用户图片信息
    /// </summary>
    public partial class TnetUserphoto
    {
        public TnetUserphoto()
        {
            Nodeid = 0;
        }

        public int Nodeid { get; set; }
        /// <summary>
        ///  照片
        ///</summary>
        public byte[] Photo { get; set; }
        /// <summary>
        ///  签名图
        ///</summary>
        public byte[] Signature { get; set; }
        /// <summary>
        ///  身份证
        ///</summary>
        public byte[] Idcard { get; set; }
        public DateTime? Createtime { get; set; }
        /// <summary>
        ///  App用户头像
        ///</summary>
        public string Appphoto { get; set; }
        internal byte? photostyle { get; set; }
        public bool? Photostyle
        {
            get { if (!photostyle.HasValue) return null; else return photostyle > 0; }
            set { if (!value.HasValue) photostyle = null; else photostyle = value.Value ? (byte)1 : (byte)0; }
        }
    }
}