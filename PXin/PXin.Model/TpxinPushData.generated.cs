using System;

namespace PXin.Model
{
    /// <summary>
    /// 定时推送数据
    /// </summary>
    public partial class TpxinPushData
    {
        public TpxinPushData()
        {
            Id = 0;
            Nodeid = 0;
            Typeid = 0;
            Title = null;
            Content = null;
            Url = null;
            Expecttime = DateTime.Now;
            Pushtime = DateTime.Now;
            Status = 0;
        }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  用户NODEID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型分类,20001-A点出局推送
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  标题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  点击推送消息,进入到URL地址，格式:http://client.xiang-xin.net/App/Believe/index.html?{sign}#/auction
        ///</summary>
        public string Url { get; set; }
        /// <summary>
        ///  期望推送时间
        ///</summary>
        public DateTime Expecttime { get; set; }
        /// <summary>
        ///  推送时间
        ///</summary>
        public DateTime Pushtime { get; set; }
        /// <summary>
        ///  状态,0-未推送，1-已推送,2-推送失败
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