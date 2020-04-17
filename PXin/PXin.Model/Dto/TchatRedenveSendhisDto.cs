using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatRedenveSendhisDto
    {
        public TchatRedenveSendhisDto()
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
        /// 红包类型，10-私人红包，20-群普通红包，21-群拼手气红包
        /// </summary>
        public int Redtype { get; set; }
        /// <summary>
        /// 目标用户,私人红包-nodeid,群红包-groupid
        /// </summary>
        public int Destid { get; set; }
        /// <summary>
        /// 钱包类型,tchat_redenve_purse.id
        /// </summary>
        public int Pursetype { get; set; }
        /// <summary>
        /// 红包总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 红包个数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 0-抢红包中，1-红包已抢完/红包已退回
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remars { get; set; }
        /// <summary>
        /// 拆红包个数
        /// </summary>
        public int Opennum { get; set; }
        /// <summary>
        /// 拆红包总金额
        /// </summary>
        public decimal Openamount { get; set; }
        /// <summary>
        /// 退回红包金额
        /// </summary>
        public decimal Backamount { get; set; }

        public string NodeCode { get; set; }

        public string NodeName { get; set; }

        public string NickName { get; set; }

        public string Pic { get; set; }
    }
}
