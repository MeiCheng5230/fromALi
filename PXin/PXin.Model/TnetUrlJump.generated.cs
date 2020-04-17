using System;

namespace PXin.Model
{
    /// <summary>
    /// url地址转换规则表
    /// </summary>
    public partial class TnetUrlJump
    {
        public TnetUrlJump()
        {
            Id = 0;
            Sid = 0;
            Name = null;
            Rule = null;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  方便测试库识别环境 PCN-81122,YG-81123,UE-81126,BELIEVE-81127
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  规则
        ///</summary>
        public string Rule { get; set; }
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