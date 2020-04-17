using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatRoomDto
    {
        public TchatRoomDto()
        {
            Roomtype = 0;
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 0-普通聊天室，1-收费聊天室,2-系统聊天室
        /// </summary>
        public int Roomtype { get; set; }
        /// <summary>
        /// 聊天室名称
        /// </summary>
        public string Roomname { get; set; }
        /// <summary>
        /// 聊天室图片
        /// </summary>
        public string RoomPic { get; set; }
        /// <summary>
        /// 是否有密码,1-有密码，0-没密码
        /// </summary>
        public int HasPwd { get; set; }
        /// <summary>
        /// 聊天室备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; private set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodecode { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateNodename { get; set; }
        /// <summary>
        /// 聊天室当前人数
        /// </summary>
        public int Personcount { get; set; }
    }
}
