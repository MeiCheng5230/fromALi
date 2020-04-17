using System;

namespace PXin.Model
{
    /// <summary>
    /// 优谷认证绑定PCN账号
    /// </summary>
    public partial class TzcAuthBindpcn
    {
        public TzcAuthBindpcn()
        {
            Id = 0;
            Nodeid = 0;
            Pcnnodeid = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        public int Nodeid { get; set; }
        public int Pcnnodeid { get; set; }
        public DateTime Createtime { get; set; }
        public string Remarks { get; set; }


    }
}