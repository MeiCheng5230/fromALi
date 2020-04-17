using Common.Facade;
using Common.Mvc.Models;
using PXin.DB;
using PXin.Facade.Models.Helper.FbAp;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;

namespace PXin.Facade.ApiFacade.Business
{
    /// <summary>
    /// 兑换充值码
    /// </summary>
    public class ExChangeRechargeCode : FbApFacade
    {
        /// <summary>
        /// 代理人/充值商
        /// </summary>
        public TblUserJxs UserJxs { get; set; }
        /// <summary>
        /// sid
        /// </summary>
        public int Sid { get; set; }
        /// <summary>
        /// 上级NodeId
        /// </summary>
        public int PNodeId { get; set; }
        /// <summary>
        /// 会员码兑换规则
        /// </summary>
        public RechargeCodeRuleInfo RechargeCodeRuleInfo { get; set; }
        /// <summary>
        /// 经销商上级
        /// </summary>
        public TblUserJxs JsxParent { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public TnetReginfo UserInfo { get; set; }
        /// <summary>
        /// 业务Id
        /// </summary>
        public int BusinessId { get; set; }
        /// <summary>
        /// 是否参与促销活动
        /// </summary>
        public bool IsPromotion { get; set; }
        /// <summary>
        /// 优谷帐号
        /// </summary>
        public string Nodecode { get; set; }
        /// <summary>
        /// 是否为代开充值商
        /// </summary>
        public bool IsOpenCzs { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        public ExChangeRechargeCode(TblUserJxs jxs, TnetReginfo userInfo, RechargeCodeRuleInfo rule, TblUserJxs jsxParent,
            int sid, int pNodeId, bool isPromotion, string nodecode)
        {
            this.UserJxs = jxs;
            this.UserInfo = userInfo;
            this.Sid = sid;
            this.PNodeId = pNodeId;
            this.RechargeCodeRuleInfo = rule;
            this.JsxParent = jsxParent;
            this.IsPromotion = isPromotion;
            this.Nodecode = isPromotion ? nodecode : "";
            this.IsOpenCzs = nodecode == "OpenFbap";
        }
        /// <summary>
        /// 执行兑换会员码流程
        /// </summary>
        public bool Execute()
        {
            //进出货
            if (!StockExchangeCode())
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 进出货
        /// </summary>
        /// <returns></returns>
        private bool StockExchangeCode()
        {
            this.BusinessId = db.GetPrimaryKeyValue<TblUserJxsStockhis>();
            TblUserJxsStockhis his = new TblUserJxsStockhis();
            //进货历史
            if (RechargeCodeRuleInfo.WholesaleCodeStock > 0)
            {
                his = GetInInitUserJxsStockhis();
                his.Stocktype = 2;
                his.Num = this.RechargeCodeRuleInfo.WholesaleCodeStock;
                his.Amountdos = this.RechargeCodeRuleInfo.DosPrice;
                db.TblUserJxsStockhisSet.Add(his);
                if (!(db.SaveChanges() > 0))
                {
                    Alert("写进货历史失败");
                    log.Info("写进货历史失败:");
                    return false;
                }
            }
            if (this.RechargeCodeRuleInfo.RetailCodeStock > 0)
            {
                his = GetInInitUserJxsStockhis();
                his.Stocktype = 1;
                his.Num = this.RechargeCodeRuleInfo.RetailCodeStock;
                his.Amountdos = this.RechargeCodeRuleInfo.DosPrice;
                db.TblUserJxsStockhisSet.Add(his);
                if (!(db.SaveChanges() > 0))
                {
                    Alert("写进货历史失败");
                    log.Info("写进货历史失败");
                    return false;
                }
            }
            //出货历史
            if (this.RechargeCodeRuleInfo.RetailCodeStock > 0)
            {
                //进货商加数量
                this.UserJxs.Stocknum2 += this.RechargeCodeRuleInfo.RetailCodeStock;
                his = GetOutInitUserJxsStockhis();
                his.Stocktype = 4;
                his.Num = this.RechargeCodeRuleInfo.RetailCodeStock;
                db.TblUserJxsStockhisSet.Add(his);
                if (!(db.SaveChanges() > 0))
                {
                    Alert("写出货历史失败");
                    log.Info("写出货历史失败:");
                    return false;
                }
            }
            if (this.RechargeCodeRuleInfo.WholesaleCodeStock > 0)
            {
                //进货商加数量
                this.UserJxs.Stocknum += this.RechargeCodeRuleInfo.WholesaleCodeStock;
                //出货历史
                his = GetOutInitUserJxsStockhis();
                his.Stocktype = 4;
                his.Num = this.RechargeCodeRuleInfo.WholesaleCodeStock;

                db.TblUserJxsStockhisSet.Add(his);
                if (!(db.SaveChanges() > 0))
                {
                    Alert("写出货历史失败");
                    log.Info("写出货历史失败:");
                    return false;
                }
            }

            if (this.JsxParent != null && this.JsxParent.Infoid > 0)
            {
                //扣充值商的货
                this.JsxParent.Stocknum -= this.RechargeCodeRuleInfo.RetailCodeStock;
                if (!(db.SaveChanges() > 0))
                {
                    Alert("更新出货充值商失败");
                    log.Info("更新出货充值商失败:");
                    return false;
                }
            }
            this.UserJxs.Lastdate = DateTime.Now; //更新进货商最后进货时间
            this.UserJxs.Isfirst = 0;             //非首次进货
            //更新进货商库存数量和最后进货时间
            if (!(db.SaveChanges() > 0))
            {
                Alert("更新进货商信息失败");
                log.Info("更新进货商信息失败");
                return false;
            }
            return true;
        }
        #region Private_Method
        private TblUserJxsStockhis GetInInitUserJxsStockhis()
        {
            TblUserJxsStockhis his = new TblUserJxsStockhis();
            //进货历史
            his.Batchnum = this.BusinessId;
            his.Jsxid = this.UserJxs.Infoid;
            his.Typeid = this.UserJxs.Typeid;
            his.Amountdos = this.RechargeCodeRuleInfo.DosPrice;
            his.Transferids = "无";
            his.Createtime = DateTime.Now;
            his.Rate = this.IsOpenCzs ? 1 : this.RechargeCodeRuleInfo.Rate;
            his.Amountdp = 0;
            his.Pcnnodecode = this.IsPromotion ? this.Nodecode : "";
            his.Status = this.IsPromotion ? 1 : 0;
            his.Remarks = this.IsOpenCzs ? "代开充值商" : "";
            return his;
        }
        private TblUserJxsStockhis GetOutInitUserJxsStockhis()
        {
            TblUserJxsStockhis his = new TblUserJxsStockhis();
            //出货历史
            his.Batchnum = this.BusinessId;
            his.Jsxid = this.JsxParent != null ? this.JsxParent.Infoid : -1;
            his.Typeid = this.JsxParent != null ? this.JsxParent.Typeid : -1;
            his.Amountdos = this.RechargeCodeRuleInfo.DosPrice;
            his.Transferids = "无";
            his.Createtime = DateTime.Now;
            his.Rate = this.IsOpenCzs ? 1 : this.RechargeCodeRuleInfo.Rate;
            his.Pcnnodecode = this.IsPromotion ? this.Nodecode : "";
            his.Status = this.IsPromotion ? 1 : 0;
            his.Amountdp = 0;
            his.Remarks = this.IsOpenCzs ? "代开充值商" : "";
            return his;
        }
        #endregion
    }
}
