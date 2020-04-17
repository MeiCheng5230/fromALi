using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 充值商新增代理人确认表
    /// </summary>
    public partial class TblUserJxsConfirm
    {
        public TblUserJxsConfirm()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Opnodeid = 0;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  要添加代理人的用户nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  操作用户ID
        ///</summary>
        public int Opnodeid { get; set; }
        /// <summary>
        ///  状态，0-未同意，1-同意，-1=已作废(多个充值商同时添加一个代理人时,代理人同意一个时其它作废)
        ///</summary>
        public int Status { get; set; }
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