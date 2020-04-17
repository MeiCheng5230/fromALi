using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// TnetLockuser
    /// </summary>
    public partial class TnetLockuser
    {
        public TnetLockuser()
        { 
Nodeid = 0;                                         
            }

        /// <summary>
        ///  已废除，采用新增字段NODEID
        ///</summary>
        public string Nodecode { get; set; }
        public DateTime? Locktime { get; set; }
        public DateTime? Unlocktime { get; set; }
        public int? Locktype { get; set; }
        public string Remarks { get; set; }
        /// <summary>
        ///  用户Nodeid
        ///</summary>
        public int Nodeid { get; set; }
    
        
    }
}