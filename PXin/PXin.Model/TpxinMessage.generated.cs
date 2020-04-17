using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈消息表
    /// </summary>
    public partial class TpxinMessage
    {
        public TpxinMessage()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Price = 0;                                         
Up = 0;                                         
Down = 0;                                         
Status = 0;                                         
Commentnum = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  发布用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  &gt;=0,查看需要多少P点,0表示免费
        ///</summary>
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  赞
        ///</summary>
        public int Up { get; set; }
        /// <summary>
        ///  踩
        ///</summary>
        public int Down { get; set; }
        /// <summary>
        ///  状态 1=正常 0=删除
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  视频文件
        ///</summary>
        public string Video { get; set; }
        /// <summary>
        ///  音频文件
        ///</summary>
        public string Sound { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  图片URL,多张用逗号隔开
        ///</summary>
        public string Picurl { get; set; }
        /// <summary>
        ///  评论数量
        ///</summary>
        public int Commentnum { get; set; }
    
        
    }
}