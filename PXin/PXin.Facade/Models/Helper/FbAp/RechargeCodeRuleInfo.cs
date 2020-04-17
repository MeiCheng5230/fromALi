namespace PXin.Facade.Models.Helper.FbAp
{
    /// <summary>
    /// 充值码兑换规则
    /// </summary>
    public class RechargeCodeRuleInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// DOS价格
        /// </summary>
        public decimal DosPrice { get; set; }
        /// <summary>
        /// 零售码库存
        /// </summary>
        public int RetailCodeStock { get; set; }
        /// <summary>
        /// 批发码库存
        /// </summary>
        public int WholesaleCodeStock { get; set; }
        /// <summary>
        /// 比例
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string Subtitle { get; set; }
        /// <summary>
        /// codeDos
        /// </summary>
        public string Dos { get { return DosPrice + "DOS"; } }
        /// <summary>
        /// code信息
        /// </summary>
        public string Code
        {
            get
            {
                string tmp = string.Empty;
                if (RetailCodeStock > 0)
                {
                    tmp += RetailCodeStock + "SVC零售码";
                }
                if (WholesaleCodeStock > 0 && RetailCodeStock > 0)
                {
                    tmp += "+" + WholesaleCodeStock + "SVC批发码";
                }
                else if (WholesaleCodeStock > 0)
                {
                    tmp += WholesaleCodeStock + "SVC批发码";
                }
                return tmp;
            }
        }
        /// <summary>
        /// 是否为促销活动
        /// </summary>
        public bool IsPromotion { get; set; }
    }
}
