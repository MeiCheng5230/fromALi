using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatLiveRoomDto
    {
        private string _appPhoto;
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 房间号,默认为主播nodecode
        /// </summary>
        public string Roomid { get; set; }
        /// <summary>
        /// 房间名称
        /// </summary>
        public string Roomname { get; set; }
        /// <summary>
        /// 房间图片
        /// </summary>
        public string Roompic { get; set; }
        /// <summary>
        /// 房间备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 主播nodeid
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 房间状态，0-待提交，1-直播中，2-已关闭
        /// </summary>
        public int Roomstate { get; set; }
        /// <summary>
        /// 当前期次
        /// </summary>
        public int Periodnum { get; set; }
        /// <summary>
        /// 房间位置，经纬度
        /// </summary>
        public string Roomlocation { get; set; }
        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime Livehbtime { get; set; }
        /// <summary>
        /// 观看人数
        /// </summary>
        public int Livewatchcount { get; set; }
        /// <summary>
        /// 点赞人数
        /// </summary>
        public int Liveadmirecount { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodecode { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodename { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNickname { get; set; }
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
                    return "http://images2.xiang-xin.net/userphoto/noempty.png";
                }
                else
                {
                    return _appPhoto;
                }
            }
        }

    }
}
