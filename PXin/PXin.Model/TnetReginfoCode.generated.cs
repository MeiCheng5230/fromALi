using System;

namespace PXin.Model
{
    /// <summary>
    /// 注册邀请码
    /// </summary>
    public partial class TnetReginfoCode
    {
        public TnetReginfoCode()
        {
            Infoid = 0;
            Nodeid = 0;
            Code = null;
            Usenodeid = 0;
            Usetime = DateTime.Now;
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
        ///  邀请码
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  使用人/被邀请人
        ///</summary>
        public int Usenodeid { get; set; }
        /// <summary>
        ///  使用时间/被邀请人注册时间
        ///</summary>
        public DateTime Usetime { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }


    }
}