using Common.Facade;
using Common.Facade.Models;
using Common.UEPay;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 兑换专区
    /// </summary>
    public class ExchangeController : ApiController
    {
        private  ExchangeFacade facade = new ExchangeFacade();

        #region 获取数据
        /// <summary>
        /// 获取用户信息及专户dos余额
        /// </summary>
        /// <param name="reqbase"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<DosInfoDto> GetDosInfo(Reqbase reqbase)
        {
            var data= facade.GetDosInfo(reqbase.Nodeid);
            if (data == null)
            {
                return new Respbase<DosInfoDto> { Result = 0, Message = "用户不存在", Data = null };
            }
            return new Respbase<DosInfoDto> { Result = 1, Data = data };
        }
        /// <summary>
        /// 获取精品兑换列表
        /// </summary>
        /// <param name="reqbase"></param>
        [HttpPost]
        public Respbase<List<ChargeProductDto>> GetChargeProductList(Reqbase reqbase)
        {
            return new Respbase<List<ChargeProductDto>>{ Result=1,Data= facade.GetChargeProductList()};
        }
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Respbase<TpxinChargeProduct> GetChargeProducDetails(ReqProducDetails req)
        //{
        //    var data = facade.GetChargeProducDetails(req.Id);
        //    if (data == null)
        //    {
        //        return new Respbase<TpxinChargeProduct> { Result = 0, Message = "查无此商品", Data = null };
        //    }
        //    return new Respbase<TpxinChargeProduct> { Result = 1, Data = data };
        //}
        /// <summary>
        /// 获取PCN或优谷用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        //[HttpPost]
        //public Respbase<ChargeUserInfoDto> GetPCNUserByNodeCode(ReqExchangeUserInfo info)
        //{
        //    ChargeUserInfoDto data = null;
        //    switch (info.typeId)
        //    {
        //        case 3:
        //           data= facade.GetYGUserInfo(info.nodeCode);
        //            break;
        //        case 4:
        //           data= facade.GetPCNUserInfo(info.nodeCode);
        //            break;
        //        default:
        //            facade.Alert("typeid不正确");
        //            break;
        //    }
        //    if (data == null)
        //    {
        //        return new Respbase<ChargeUserInfoDto> { Result = 0, Message = facade.PromptInfo.Message, Data = null };
        //    }
        //    return new Respbase<ChargeUserInfoDto> { Result = 1, Data = data };
        //}
        #endregion

        #region 商品兑换
        /// <summary>
        /// 兑换商品
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase ProductRecharge(ReqProductRecharge req)
        {
            var result = facade.ProductRecharge(req);
            if (!result)
            {
                return new Respbase { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = 1, Message = "兑换成功" };
        }
        /// <summary>
        /// 兑换历史
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<RechargeHisDto>>GetRechargeHisList(GetByPageBase req)
        {
            var data= facade.GetTpxinChargeHisList(req);
            return new Respbase<List<RechargeHisDto>> { Result = 1, Data = data };
        }
        #endregion

        #region 转入 转出
        /// <summary>
        /// 开通专属账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<OpenInfUeoDto> OpenInfo(Reqbase req)
        {
            var data = facade.OpenInfo(req);
            if (data == null)
            {
                return new Respbase<OpenInfUeoDto> { Result = 0, Message = facade.PromptInfo.Message,Data=null };
            }
            return new Respbase<OpenInfUeoDto> { Result = 1,  Data = data };
        }
        ///// <summary>
        ///// 获取ue用户信息
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public Respbase<UeUserInfo> GetUeUserInfo(Reqbase req)
        //{
        //    var data = facade.GetUeInfo(req.Nodeid, 2);
        //    if (data == null)
        //    {
        //        return new Respbase<UeUserInfo> { Result = 0, Message = facade.PromptInfo.Message, Data = null };
        //    }
        //    if (data.Result<= 0)
        //    {
        //       return new Respbase<UeUserInfo> { Result = -5, Message = data.Message, Data = null };//-5表示没有绑定ue账号
        //    }
        //    return new Respbase<UeUserInfo> { Result = 1, Data = data.Data };
          
        //}
         /// <summary>
         /// 绑定ue（更改绑定）
         /// </summary>
         /// <param name="req"></param>
         /// <returns></returns>
        [HttpPost]
        public Respbase<UeUserInfoDto> BindingUeAccount(ReqBindingUe req)
        {
            var data = facade.BindingUeAccount(req);
            if (data == null)
            {
                return new Respbase<UeUserInfoDto> { Result = 0, Message = facade.PromptInfo.Message,Data=null };
            }
            return new Respbase<UeUserInfoDto> { Result = 1, Message = "绑定成功",Data=data };
        }
        /// <summary>
        /// 转入（UEDOS转入到专户DOS）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<OpenInfUeoDto> UeTransferInDos(ReqUeTransferIn req )
        {
            var result = facade.UeTransferInDos(req);
            if (result==null)
            {
                return new Respbase<OpenInfUeoDto> { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase<OpenInfUeoDto> { Result = 1, Data=result };
        }
        /// <summary>
        /// 转出（专户DOS转出到UEDOS）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase DosTransferOutUe(ReqUeTransfer req)
        {
            var result = facade.DosTransferOutUe(req.Nodeid, req.Amount, req.PayPwd);
            if (!result)
            {
                return new Respbase { Result = 0, Message = facade.PromptInfo.Message };
            }
            return new Respbase { Result = 1, Message = "转出成功" };
        }
        #endregion
    }
}
