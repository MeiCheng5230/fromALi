using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Commu.DataAccess
{
    /// <summary>
    /// 用户注册信息表
    /// </summary>
    public partial class TnetReginfo
    {
        public TnetReginfo()
        {
            Nodeid = 0;
            Isconfirmed = 0;
            Isenterprise = 0;
            Zoneid = 0;
            Currcent = 0;
            Centalert = 0;
            Cashalert = 0;
            Status = 0;
        }

        /// <summary>
        ///  主键
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  DTN号
        ///</summary>
        public string Nodecode { get; set; }
        /// <summary>
        ///  用户姓名
        ///</summary>
        public string Nodename { get; set; }
        /// <summary>
        ///  是否认证 1-认证，0-未认证
        ///</summary>
        public int Isconfirmed { get; set; }
        /// <summary>
        ///  状态  0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
        ///</summary>
        public int Isenterprise { get; set; }
        /// <summary>
        ///  推荐人NodeID(目前新用户注册，推荐人信息写入此字段)
        ///</summary>
        public int? Introducer { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  用户密码
        ///</summary>
        public string Userpwd { get; set; }
        public int Zoneid { get; set; }
        public decimal Currcent { get; set; }
        public long Centalert { get; set; }
        /// <summary>
        ///  最高免密支付金额
        ///</summary>
        public double Cashalert { get; set; }
        public int? Acceptterm { get; set; }
        /// <summary>
        ///  用户状态,未激活=0,已激活=1,已注销=2,已封锁=3
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  支付密码
        ///</summary>
        public string UserpwdBak { get; set; }
        /// <summary>
        ///  认证到期时间
        ///</summary>
        public DateTime? Authtime { get; set; }
        public string Photourl { get; set; }
        public int? Fatherid { get; set; }
        public int? Nodelevel { get; set; }
        /// <summary>
        ///  Email
        ///</summary>
        public string Email { get; set; }
        /// <summary>
        ///  手机号码
        ///</summary>
        public string Mobileno { get; set; }
    }
}
