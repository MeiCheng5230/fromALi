using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Common.UEPay;
using PXin.DB;
using PXin.Model;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Winner.CU.Balance.GlobalCurrency;

namespace PXin.Facade.UEPay
{
    /// <summary>
    /// 
    /// </summary>
    public class UePayHelper
    {
        private static Log log = new Log(typeof(UePayHelper));
        /// <summary>
        /// ue支付(调用ue客户端时使用)
        /// </summary>
        /// <param name="db">数据库上下文</param>
        /// <param name="userInfo">支付用户</param>
        /// <param name="uePayConfigTypeId">业务类型</param>
        /// <param name="currencyType">货币类型</param>
        /// <param name="amount">支付金额</param>
        /// <param name="uePayHisTypeId">业务类型</param>
        /// <param name="businessParams">业务相关参数</param>
        /// <param name="body">商品描述</param>
        /// <param name="subject">商品名称</param>
        /// <param name="freezeids">冻结ID列</param>
        /// <param name="pNodeId">父NodeId（eg:经销商支付时就需要要获取它的上级专营商的NodeId）</param>
        /// <returns></returns>
        public static async Task<DosWithUePayDto> DosWithUePay(
            PXinContext db, TnetReginfo userInfo,
            int uePayConfigTypeId, CurrencyType currencyType, decimal amount, int uePayHisTypeId, string businessParams,
            string body, string subject,
            string freezeids = "", int pNodeId = 0)
        {
            var ueConfig = await db.TpcnUepayconfigSet.FirstOrDefaultAsync(p => p.Typeid == uePayConfigTypeId);
            if (ueConfig == null)
            {
                log.Info("获取UE支付配置失败");
                return new DosWithUePayDto() { IsSuccess = false, Message = "获取UE支付配置失败" };
            }
            Currency currency = new Currency(currencyType, amount);
            decimal total = currency.Amount;
            int unit = currency.Type.CurrencyId;
            TnetUepayhis uePayHis = new TnetUepayhis { Typeid = uePayHisTypeId, Nodeid = userInfo.Nodeid, BusinessParams = businessParams, Amount = total, Unit = unit, Freezeids = freezeids, Createtime = DateTime.Now };
            db.TnetUepayhisSet.Add(uePayHis);
            var falg = await db.SaveChangesAsync() > 0;
            if (!falg)
            {
                log.Info("生成UE订单失败,NodeId=:" + uePayHis.Nodeid);
                return new DosWithUePayDto() { IsSuccess = false, Message = "生成UE订单失败" };
            }
            var recvNodeCode = "";
            if (pNodeId > 0)//代理人支付时,获取充值商帐号(收钱帐号)，向它支付
            {
                var parentUserInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = pNodeId.ToString() });
                if (parentUserInfo == null)
                {
                    log.Info("获取上级用户信息失败,NodeId=:" + uePayHis.Nodeid);
                    return new DosWithUePayDto() { IsSuccess = false, Message = "获取上级用户信息失败" };
                }
                recvNodeCode = parentUserInfo.Nodecode;
            }
            var chargeDto = new ChargeDto
            {
                businesstypeid = uePayHisTypeId,
                amount = total,
                unit = unit,
                body = body,
                subject = subject,
                orderno = uePayHis.Id.ToString(),
                createtime = uePayHis.Createtime.ToString("yyyy-MM-dd HH:mm:ss"),
                paycode = ueConfig.Paycode,
                recvfromid = pNodeId > 0 ? 6 : 0,
                recvaccount = recvNodeCode,
                noticeurl = Common.Facade.Helper.DomainUrl + "/UENotice/Success"
            };
            var chargeStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chargeDto)));
            return new DosWithUePayDto() { IsSuccess = true, ChargeStr = chargeStr, OrderNo = chargeDto.orderno };
        }
    }
}
