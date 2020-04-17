using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Dto
{
    public class TchatRedenvePurseDto
    {
        public TchatRedenvePurseDto()
        {
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public int Purseid { get; set; }
        /// <summary>
        /// 钱包名称
        /// </summary>
        public string Pursename { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
