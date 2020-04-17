using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatRedenveOpenhisDto
    {
        public TchatRedenveOpenhisDto()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// tchat_redenve_sendhis.id
        /// </summary>
        public int Hisid { get; set; }
        /// <summary>
        /// 抢得红包金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否手气最佳,1-手气最佳
        /// </summary>
        public int Isoptimum { get; set; }
        /// <summary>
        /// 红包类型，10-私人红包，20-群普通红包，21-群拼手气红包
        /// </summary>
        public int Redtype { get; set; }
        /// <summary>
        /// 钱包类型,tchat_redenve_purse.id
        /// </summary>
        public int Pursetype { get; set; }

        public string NodeCode { get; set; }

        public string NodeName { get; set; }

        public string NickName { get; set; }

        public string Pic { get; set; }
    }
}
