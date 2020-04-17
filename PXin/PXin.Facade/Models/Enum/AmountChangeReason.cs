using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Enum
{
    /// <summary>
    /// 金额变动原因
    /// </summary>
    public enum AmountChangeReason
    {
        /// <summary>
        /// 发布文章
        /// </summary>
        PublishArticle = 1,
        /// <summary>
        /// 查看文章
        /// </summary>
        ViewArticle = 2,
        /// <summary>
        /// 赞文章
        /// </summary>
        PraiseArticle = 3,
        /// <summary>
        /// 踩文章
        /// </summary>
        TreadArticle = 4,
        /// <summary>
        /// 打赏
        /// </summary>
        Reward = 5,
        /// <summary>
        /// 聊天计费
        /// </summary>
        ChatFee = 6,
        /// <summary>
        /// 充值V点
        /// </summary>
        ChargeVDian = 7,
        /// <summary>
        /// 注册送V点
        /// </summary>
        RegisterGiveVDian = 8
    }
}
