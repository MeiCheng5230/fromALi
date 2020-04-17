using ApiAuthCore;
using PXin.DB;
using PXin.Facade;
using PXin.Model;
using Common.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PXin.Facade.ApiFacade;
using Common.UEPay;

namespace PXin.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UENoticeController : Controller
    {


        Log log = new Log(typeof(UENoticeController));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Success()
        {
            string content = ApiAuth.GetRequestContent();
            log.Info("PayorderNotice:" + content);
            PXinContext ctx = HttpContext.GetDbContext<PXinContext>();
            TpcnUepayconfig ueConfig = ctx.TpcnUepayconfigSet.FirstOrDefault(c => c.Id == 1);
            if (ueConfig == null)
            {
                log.Info("获取UE支付配置失败");
                return Content("failed");
            }
            PayHisDto payHis = new PayHisDto();
            if (Request.Url.ToString().IndexOf("localhost") > 0)
            {
                #region 测试注释
                payHis.Paystatus = 1;//测试默认为1付款成功
                payHis.Orderno = Request["orderno"];
                payHis.OrdernoUE = Request["orderue"];
                #endregion
            }
            else
            {
                #region 测试注释
                if (!ApiAuth.Verify(ueConfig.Accesssecret))
                {
                    log.Info("签名验证失败");
                    return Content("failed");
                }
                try
                {
                    payHis = UeApi.RequestDeserializeObject<PayHisDto>();
                }
                catch (Exception err)
                {
                    log.Info(err);
                    return Content("failed");
                }
                #endregion
            }
            if (payHis == null || string.IsNullOrEmpty(payHis.Orderno) || string.IsNullOrEmpty(payHis.OrdernoUE))
            {
                log.Info("订单数据异常:" + JsonConvert.SerializeObject(payHis));
                return Content("fail");
            }
            if (payHis.Paystatus == 1)
            {
                //付款成功状态
                if (!PaySuccessNotice(payHis))
                {
                    return Content("fail");
                }
            }
            else
            {
                //其它状态不处理
                log.Info("订单数据支付状态不等于1:" + JsonConvert.SerializeObject(payHis));
            }
            return Content("OK");
        }
        private bool PaySuccessNotice(PayHisDto apiueHis)
        {
            PXinContext ctx = HttpContext.GetDbContext<PXinContext>();

            //付款成功状态
            int oid = Convert.ToInt32(apiueHis.Orderno);
            TnetUepayhis uePayHis = ctx.TnetUepayhisSet.FirstOrDefault(c => c.Id == oid);

            if (uePayHis == null)
            {
                log.Info("orderid:" + apiueHis.Orderno + "不存在");
                Response.Write("fail");
                return false;
            }
            if (uePayHis.Status != 0)
            {
                log.Info("orderid:" + apiueHis.Orderno + "已处理");
                return false;
            }
            uePayHis.Ordernoue = apiueHis.OrdernoUE;
            //uePayHis.Status = 1;
            //if (ctx.SaveChanges() <= 0)
            //{
            //    log.Info("orderid=" + uePayHis.Id + ",更新订单状态失败");
            //    Response.Write("fail");
            //    return false;
            //}
            if (uePayHis.Typeid == 20001)
            {
                //13-充值V点
                FriFacade facade = new FriFacade();
                bool result = facade.ChargeVDian_Notice(uePayHis);
                if (!result)
                {
                    log.Info("充值V点失败,原因" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20002)
            {
                //5-充值商新增代理人
                FbApFacade facade = new FbApFacade();
                bool result = facade.AddUserJxs_Notice(uePayHis).Result;
                if (!result)
                {
                    log.Info("充值商新增代理人失败，原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20003 || uePayHis.Typeid == 20007)
            {
                //1-兑换充值码(进货)
                FbApFacade facade = new FbApFacade();
                bool result = facade.ExChangeRechargeCode_Notice(uePayHis).Result;
                if (!result)
                {
                    log.Info("兑换充值码失败，原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20004)
            {
                //开通专属账号
                var facade = new ExchangeFacade();
                var result = facade.OpenInfo_Notice(uePayHis);
                if (!result)
                {
                    log.Info("开通专属账号失败原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20005)
            {
                //18-充值商续费
                FbApFacade facade = new FbApFacade();
                bool result = facade.Renew_Notice(uePayHis).Result;
                if (!result)
                {
                    log.Info("充值商续费失败，原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20006)
            {
                //ue转账到相信
                var facade = new ExchangeFacade();
                var result = facade.UeTransferInDos_Notice(uePayHis);
                if (!result)
                {
                    log.Info("ue转账到相信失败原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20008)
            {
                //十月送手机活动
                var facade = new ActivityFacade();
                var result = facade.OctoberActivityDosUEPay_Notice(uePayHis);
                if (!result)
                {
                    log.Info("十月送手机活动支付服务费失败原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else if (uePayHis.Typeid == 20010)
            {
                //代开充值商
                var facade = new FbApFacade();
                var result = facade.OpenCzs_Notice(uePayHis).Result;
                if (!result)
                {
                    log.Info("代开充值商支付服务费失败,原因：" + facade.PromptInfo.Message);
                }
                return result;
            }
            else
            {
                //未知业务类型
                log.Info("未知业务类型:" + JsonConvert.SerializeObject(apiueHis));
                return false;
            }
        }
    }
}
