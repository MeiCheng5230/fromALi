using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
   

    public class VchatGroup
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string Groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public string Descript { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 群组状态，0-待提交，1-正常,2-解散
        /// </summary>
        public int Groupstate { get; set; }
        /// <summary>
        /// 群组人数
        /// </summary>
        public int Personcount { get; set; }
        /// <summary>
        /// 0-普通群，1-收费群，2-系统群,3-广播群
        /// </summary>
        public int Grouptype { get; set; }
        /// <summary>
        /// 解散时间
        /// </summary>
        public DateTime? Dismisstime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Createnodecode { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Createnodename { get; set; }
        /// <summary>
        /// 群组头像
        /// </summary>
        public string Grouppic { get; set; }
        /// <summary>
        ///  群组号
        ///</summary>
        public string Groupcode { get; set; }
        /// <summary>
        ///  审核状态，0-等待审核，1-审核通过，2-审核拒绝
        ///</summary>
        public int Auditstate { get; set; }
        /// <summary>
        ///  认证状态，0-未认证，1-已认证
        ///</summary>
        public int Authstate { get; set; }
        /// <summary>
        /// 完整图像URL地址
        /// </summary>
        public string Grouppicfull
        {
            get
            {
                if (string.IsNullOrEmpty(Grouppic)) return string.Empty;
                return "http://client.xiang-xin.net" + Grouppic;
            }
        }
    }
}
