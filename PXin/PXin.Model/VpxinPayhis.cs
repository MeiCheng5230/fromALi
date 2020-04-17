using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
   public  class VpxinPayhis
    {
        public VpxinPayhis()
        {
            Hisid = 0;
            Infoid = 0;
            Price = 0;
            Nodeid = 0;
            Typeid = 0;
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
        ///  金额
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 类型 1=充值 2=发布文章 3=查看文章
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 查询V点交易记录用户id
        /// </summary>
        public int Nodeid { get; set; }
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
