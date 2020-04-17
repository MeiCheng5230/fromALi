using System;
using System.Collections.Generic;

namespace PXin.Commu.DataAccess
{
    /// <summary>
    /// 相信V点P点变化历史表
    /// </summary>
    public partial class TpxinAmountChangeHis
    {
        public TpxinAmountChangeHis()
        {
            Hisid = 0;
            Nodeid = 0;
            Typeid = 0;
            Amount = 0;
            Reason = 0;
            Transferid = null;
            Amountbefore = 0;
            Amountafter = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  金额变化对象
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型 1=V点 2=P点 3=SVC 4A点
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  变化金额
        ///</summary>
        public decimal Amount { get; set; }
        /// <summary>
        ///  变化原因(1=发布文章 2=查看文章 3=点赞文章 4=踩文章 5=打赏 6-聊天计费 7=充值 8=注册送V点,
        ///  2000-A点竟拍,2001-A点竟换失败退还,
        ///  3000-红包中奖兑换SV，3001-红包中奖兑换充值码(SVC))
        ///  4000-A点抽奖 4001=A点结算
        ///</summary>
        public int Reason { get; set; }
        /// <summary>
        ///  转账ID，标识两个用户属于同一个transferid
        ///</summary>
        public string Transferid { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  对应mongodb-tpxin_amount_change_his表的_id
        ///</summary>
        public string Mongodbid { get; set; }
        /// <summary>
        ///  变化前金额
        ///</summary>
        public decimal Amountbefore { get; set; }
        /// <summary>
        ///  变化后金额
        ///</summary>
        public decimal Amountafter { get; set; }


    }
}