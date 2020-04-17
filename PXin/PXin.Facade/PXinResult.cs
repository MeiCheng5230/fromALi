using Common.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade
{
    /// <summary>
    /// 
    /// </summary>
    public class PXinResult
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PXinResult<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public PXinResult()
        {
            Result = true;
            Msg = "成功";
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PXinUser
    {
        /// <summary>
        /// 
        /// </summary>
        private string _appPhoto;
        /// <summary>
        /// 
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GtClientid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceToken { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int? GradeId { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>
        public string GradeName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;   //新改图片地址
                }
                return _appPhoto;
            }
        }
        /// <summary>
        /// 队伍名称
        /// </summary>
        public string TeamName { get; set; }
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
        /// 好友备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否允许查看我的动态，1-允许，0-不充许
        /// </summary>
        public int Allowviewmedynamic { get; set; }
        /// <summary>
        /// 是否查看他的动态，1-看，0-不看
        /// </summary>
        public int Viewhedynamic { get; set; }
        /// <summary>
        /// 腾讯usersig
        /// </summary>
        public string TxUserSig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PxinConfig Config { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PxinConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public int CreateGroupMinGrade { get { return AppConfig.CreateGroupMinGrade; } }
        /// <summary>
        /// 
        /// </summary>
        public int GroupMaxQuantity { get { return AppConfig.GroupMaxQuantity; } }
        /// <summary>
        /// 
        /// </summary>
        public int DiscussionMaxQuantity { get { return AppConfig.DiscussionMaxQuantity; } }
        /// <summary>
        /// 
        /// </summary>
        public decimal CreateGroupAmount { get { return AppConfig.CreateGroupAmount; } }
        /// <summary>
        /// 
        /// </summary>
        public decimal CreateChatRoomAmount { get { return AppConfig.CreateChatRoomAmount; } }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultPublic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal LiveSendBarrageGiftId { get { return AppConfig.LiveSendBarrageGiftId; } }
        /// <summary>
        /// 
        /// </summary>
        public string LiveRoomTip { get { return AppConfig.LiveRoomTip; } }
    }
}
