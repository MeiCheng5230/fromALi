using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人知识库查看历史表
    /// </summary>
    public partial class TpxinDarenKnowledgeHis
    {
        public TpxinDarenKnowledgeHis()
        { 
Id = 0;                                         
Pinfoid = 0;                                         
Nodeid = 0;                                         
Pnodeid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  知识库主键id 表TPXIN_DAREN_Knowledge id
        ///</summary>
        public int Pinfoid { get; set; }
        /// <summary>
        ///  查看人nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  知识库创建人
        ///</summary>
        public int Pnodeid { get; set; }
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