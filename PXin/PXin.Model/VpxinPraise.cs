using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
    public class VpxinPraise
    {
        public VpxinPraise()
        {
            Hisid = 0;
            Infoid = 0;
            Status = 0;
            Reward = 0;
            Nodeid = 0;
            Type = 0;

        }
        /// <summary>
        /// 主键值
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int Infoid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 赏金
        /// </summary>
        public int Reward { get; set; }
        /// <summary>
        /// 查询p点交易记录用户id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 2=赞 1=踩 3=打赏 4=赏金
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Nodename { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }

    }
}
