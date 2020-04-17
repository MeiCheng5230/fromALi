using Common.Facade.Models;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 收货地址
    /// </summary>
    [RoutePrefix("api/ShoppingAddr")]
    public class ShoppingAddrController : ApiController
    {
        private ShoppingAddrFacede facede;

        /// <summary>
        /// 
        /// </summary>
        public ShoppingAddrController()
        {
            facede = new ShoppingAddrFacede();
        }

        /// <summary>
        /// 获取收货地址
        /// </summary>
        [HttpPost]
        [Route("GetShoppingAddrs")]
        public Respbase<List<ShoppingAddrsDto>> GetShoppingAddrs(Reqbase req)
        {
            var result = facede.GetShoppingAddrs(req);
            return result;
        }

        /// <summary>
        /// 新增收货地址
        /// </summary>
        [HttpPost]
        [Route("CreateAddress")]
        public Respbase CreateAddress(CreateAddressReq req)
        {
            var result = facede.CreateAddress(req);
            return result;
        }

        /// <summary>
        /// 编辑收货地址
        /// </summary>
        [HttpPost]
        [Route("EditAddress")]
        public Respbase EditAddress(EditAddressReq req)
        {
            var result = facede.EditAddress(req);
            return result;
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        [HttpPost]
        [Route("DeleteAddress")]
        public Respbase DeleteAddress(DeleteAddressReq req)
        {
            var result = facede.DeleteAddress(req);
            return result;
        }

        /// <summary>
        /// 修改设置默认地址
        /// </summary>
        [HttpPost]
        [Route("ChangeDefault")]
        public Respbase ChangeDefault(DeleteAddressReq req)
        {
            var result = facede.ChangeDefault(req);
            return result;
        }
    }
}