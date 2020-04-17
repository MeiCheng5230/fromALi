using System;

namespace PXin.Model
{
    /// <summary>
    /// TzcIdcardrecLog
    /// </summary>
    public partial class TzcIdcardrecLog
    {
        /// <summary>
        /// 
        /// </summary>
        public TzcIdcardrecLog()
        {
            Id = 0;
            Pic = null;
            Recresult = null;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  图片
        ///</summary>
        public string Pic { get; set; }
        /// <summary>
        ///  识别结果
        ///</summary>
        public string Recresult { get; set; }
        /// <summary>
        ///  识别时间
        ///</summary>
        public DateTime Createtime { get; set; }


    }
}