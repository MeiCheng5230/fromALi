using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信用户
    /// </summary>
    public partial class TchatUser
    {
        /// <summary>
        /// 
        /// </summary>
        public TchatUser()
        {
            Provinceid = 0;
            Cityid = 0;
            Showrealname = 0;
            IsValidfriend = 1;
            IsSysNotice = 1;
            IsNoticeDetail = 1;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// tnet_reginfo.nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 融云唯一标识
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 个推Clientid
        /// </summary>
        public string Gtclientid { get; set; }
        /// <summary>
        /// IOS专用
        /// </summary>
        public string Devicetoken { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public int Provinceid { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Provincename { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public int Cityid { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string Cityname { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Personalsign { get; set; }
        /// <summary>
        /// 显示真实姓名 1-显示，0-不显示
        /// </summary>
        public int Showrealname { get; set; }

        /// <summary>
        /// 加我为好友时是否需要验证，1是，0否
        /// </summary>
        public int IsValidfriend { get; set; }

        /// <summary>
        /// 通知设置：系统动态通知，1是，0否
        /// </summary>
        public int IsSysNotice { get; set; }

        /// <summary>
        /// 通知设置：通知显示详情，1是，0否
        /// </summary>
        public int IsNoticeDetail { get; set; }
    }
}
