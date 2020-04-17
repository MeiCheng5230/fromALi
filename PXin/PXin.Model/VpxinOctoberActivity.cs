using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model
{
    public class VpxinOctoberActivity
    {
        public VpxinOctoberActivity()
        {
            Pnodeid = 0;
            Nodeid = 0;
            Typeid = 0;
            Amount = 0;
            Pamount = 0;
        }
        public int Hisid { get; set; }
        /// <summary>
        ///  上级用户
        ///</summary>
        public int Pnodeid { get; set; }
        /// <summary>
        ///  领取手机用户
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  活动类型，1-代开充值商、2-代理人进货、3-零售SVC充值码并充值SV
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  领取手机用户看到的活动描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  上级用户看到的活动描述
        ///</summary>
        public string Pnote { get; set; }
        /// <summary>
        ///  领取手机用户支付金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  上级用户支付金额
        ///</summary>
        public decimal Pamount { get; set; }
    }
}
