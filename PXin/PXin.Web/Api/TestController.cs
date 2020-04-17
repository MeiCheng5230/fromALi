using Common.Facade.Models;
using PXin.Facade;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PXin.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase Index(Reqbase req)
        {
            return new Respbase();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentid"></param>
        /// <returns></returns>
        [OverrideActionFilters]
        public Respbase TestCommentPush(int commentid)
        {
            PxinSerivce.EnqueueComment(commentid);
            return new Respbase();
        }

        //[OverrideActionFilters]
        //public Respbase TestTranfUVCallBack(string orderNo)
        //{
        //    var facade = new CZMFacade();
        //    facade.PingppPaySuccess_UV(orderNo);
        //    return facade.PromptInfo;
        //}
    }
}
