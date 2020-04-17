using System;

namespace PXin.Model
{
    public class VpxinMessage
    {
        public VpxinMessage()
        {
            Infoid = 0;
            Hisid = 0;
            Localnodeid = 0;
            Msgnodeid = 0;
            Price = 0;
            Up = 0;
            Down = 0;
            Commentnum = 0;
            Ispay = 0;
            Reward = 0;
            IsDown = 0;
            IsUp = 0;
            Status = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  推送消息主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  登录用户ID
        ///</summary>
        public int Localnodeid { get; set; }
        /// <summary>
        ///  发布用户ID
        ///</summary>
        public int Msgnodeid { get; set; }

        /// <summary>
        /// 需要支付多少V点开通
        ///</summary>
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  视频文件
        ///</summary>
        public string Video { get; set; }
        /// <summary>
        ///  音频文件
        ///</summary>
        public string Sound { get; set; }
        /// <summary>
        ///  图片URL,多张用逗号隔开
        ///</summary>
        public string Picurl { get; set; }
        /// <summary>
        ///  赞
        ///</summary>
        public int Up { get; set; }
        /// <summary>
        ///  踩
        ///</summary>
        public int Down { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  评论数量
        ///</summary>
        public int Commentnum { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public int Ispay { get; set; }
        /// <summary>
        ///  已打赏金额
        ///</summary>
        public decimal Reward { get; set; }

        /// <summary>
        ///  是否赞
        ///</summary>
        public int IsUp { get; set; }
        /// <summary>
        ///  是否踩
        ///</summary>
        public int IsDown { get; set; }

        /// <summary>
        ///  是否已发送给用户 0=未发送 1=已发送
        ///</summary>
        public int Status { get; set; }
    }
}
