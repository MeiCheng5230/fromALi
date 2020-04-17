using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// app意见反馈信息记录表
    /// </summary>
    public partial class TappFeedback
    {
        /// <summary>
        /// 
        /// </summary>
        public TappFeedback()
        {
            Id = 0;
            Nodeid = 0;
            Client = 0;
            Version = string.Empty;
            Status = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  反馈人
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  内容,可为空
        ///</summary>
        public string Message { get; set; }
        /// <summary>
        ///  图片
        ///</summary>
        public string Image { get; set; }
        /// <summary>
        ///  IOS：1 android：2
        ///</summary>
        public int Client { get; set; }
        /// <summary>
        ///  版本信息
        ///</summary>
        public string Version { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注信息
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  意见处理人,客服NODEID
        ///</summary>
        public int? Opnodeid { get; set; }
        /// <summary>
        ///  状态 0=待处理 1=已处理
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  处理结果,客户可见
        ///</summary>
        public string Note { get; set; }

        /// <summary>
        ///  联系方式
        ///</summary>
        public string Mobile { get; set; }
    }
}