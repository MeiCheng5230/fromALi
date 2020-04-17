using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信过滤词
    /// </summary>
    public partial class TchatFilterword
    {
        public TchatFilterword()
        {
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int? Id { get; set; }
        public string Filterword { get; set; }


    }
}