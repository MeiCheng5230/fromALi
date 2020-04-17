using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 用户激活码信息表
    /// </summary>
    public partial class TbtcActivation
    {
        public TbtcActivation()
        {
            InfoId = 0;
            NodeId = 0;
            Code = string.Empty;
            Status = 0;
            UseTime = new DateTime();
            Sid = 0;
            ActiveTime = new DateTime();
            Typeid = 0;
            Codetype = 0;
            Fromid = 0;
            Chgnodeid = 0;
            Chgtransferid = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int InfoId { get; set; }
        /// <summary>
        ///  激活码拥有人
        ///</summary>
        public int NodeId { get; set; }
        /// <summary>
        ///  激活码，唯一
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  使用状态 0=未使用 1=已使用
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  使用人，STATUS=0时为空
        ///</summary>
        public int? UserId { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///  使用时间
        ///</summary>
        public DateTime UseTime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  过期时间
        ///</summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        ///  APPID
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  激活时间
        ///</summary>
        public DateTime ActiveTime { get; set; }
        /// <summary>
        ///  P客等级/VIP等级
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  年=12 月=1
        ///</summary>
        public int Codetype { get; set; }
        /// <summary>
        ///  没用
        ///</summary>
        public int? BuynodeId { get; set; }
        /// <summary>
        ///  来源 1=正常购买 2=赠送
        ///</summary>
        public int? Fromid { get; set; }
        /// <summary>
        ///  换码用户ID
        ///</summary>
        public int? Chgnodeid { get; set; }
        /// <summary>
        ///  换码转账ID
        ///</summary>
        public int? Chgtransferid { get; set; }


    }
}