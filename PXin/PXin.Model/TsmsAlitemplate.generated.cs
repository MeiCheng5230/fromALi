using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 阿里短信模板
    /// </summary>
    public partial class TsmsAlitemplate
    {
        public TsmsAlitemplate()
        { 
Id = 0;                                         
Tmpcode = string.Empty;                                         
Tmpcnt = string.Empty;                                         
Smssign = string.Empty;                                         
Tmpval = string.Empty;                                         
Status = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  模板CODE
        ///</summary>
        public string Tmpcode { get; set; }
        /// <summary>
        ///  模板内容
        ///</summary>
        public string Tmpcnt { get; set; }
        /// <summary>
        ///  短信签名
        ///</summary>
        public string Smssign { get; set; }
        /// <summary>
        ///  模板变量
        ///</summary>
        public string Tmpval { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  状态，0-未启用，1-已启用
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  Access Key ID
        ///</summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        ///  Access Key Secret
        ///</summary>
        public string AccessKeySecret { get; set; }
    
        
    }
}