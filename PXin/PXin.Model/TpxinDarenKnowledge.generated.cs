using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人知识库表
    /// </summary>
    public partial class TpxinDarenKnowledge
    {
        public TpxinDarenKnowledge()
        { 
Id = 0;                                         
Title = string.Empty;                                         
Paytype = 0;                                         
Price = 0;                                         
Content = string.Empty;                                         
Status = 0;                                         
Modifytime = new DateTime();                                         
Nodeid = 0;                                         
Num = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  主题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  支付类型 0=V点 1=UV
        ///</summary>
        public int Paytype { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  状态 0=草稿 1=已发布
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  修改时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  创建人nodeid
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  付款浏览人数
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  语音文件地址
        ///</summary>
        public string Voice { get; set; }
    
        
    }
}