using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// B指数信息
    /// </summary>
    public class BexponentDto
    {
        /// <summary>
        /// 本期回馈
        /// </summary>
        public decimal YHDRePayNow { get; set; }
        /// <summary>
        /// 累计回馈
        /// </summary>
        public decimal YHDRePayALL { get; set; }
        /// <summary>
        /// 待回馈
        /// </summary>
        public decimal YHDRePayAwait { get; set; }
        /// <summary>
        /// 指数差值（此处计算规则为：以8点和20点为分界，将一天分为3段，0-8和20-24占10%，8-20点占80%，当前指数=昨天指数+3段已经过去时间占比*当前段的比例）
        /// 公式为：当前指数=昨天指数+ 0-8点已过时间占比 X 差值 X 10%+9-20点已过时间占比 X 差值 X 80%+21-24点已过时间占比 X 差值 X 10%
        /// 例：昨天指数100，差值100，当前9点，则计算当前指数为：当前指数=100 + 1X100X10% + 1/12X100X80% + 0X100X10%=116.67
        /// </summary>
        public double LocalNum { get; set; }
        /// <summary>
        /// 昨天指数
        /// </summary>
        public double BeforeNum { get; set; }
        /// <summary>
        /// 目标指数
        /// </summary>
        public double Num { get; set; }
        /// <summary>
        /// 上期指数
        /// </summary>
        public double BeNum { get; set; }
        /// <summary>
        /// 是否完成上月任务  0=未完成 1=已完成
        /// </summary>
        public int IsCompleteTasks { get; set; }
        /// <summary>
        /// 已结算金额
        /// </summary>
        public decimal AlreadyAmount { get; set; }
        /// <summary>
        /// 已结算次数
        /// </summary>
        public int AlreadyCount { get; set; }
        /// <summary>
        /// 下次结算金额
        /// </summary>
        public double NextAmount { get; set; }
        /// <summary>
        /// 待领取优惠点
        /// </summary>
        public decimal YHDAmount { get; set; }



    }

    /// <summary>
    /// B指数回馈号信息
    /// </summary>
    public class BexponentHKDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Infoid { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        /// 状态 0=未确认 1=已确认
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 已回馈金额
        /// </summary>
        public decimal AlreadyAmount { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 待回馈金额
        /// </summary>
        public decimal AwaitAmount { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        public decimal Rate { get; set; }
    }

    /// <summary>
    /// 回馈号详细列表
    /// </summary>
    public class HKHDetailDto
    {
        /// <summary>
        /// 回馈P币
        /// </summary>
        public string DHK { get; set; }
        /// <summary>
        /// 期数
        /// </summary>
        public decimal Periods { get; set; }
        /// <summary>
        /// 回馈时间
        /// </summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        /// 状态 0=未回馈 1=已回馈
        /// </summary>
        public string Status { get; set; }


    }
}
