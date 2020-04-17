using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Mvc;

namespace PXin.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// SSO登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Anonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            SsoClient ssoClient = new SsoClient();
            if (!ssoClient.IsLogined)
            {
                return Redirect(SsoHelper.SSOLoginUrl);
            }
            if (SsoHelper.HasReturnUrl())
            {
                return Redirect(SsoHelper.ReturnUrl);
            }
            return View();
        }

        /// <summary>
        /// 退出SSO       
        /// </summary>
        /// <returns></returns>
        [Anonymous]
        public ActionResult LogOff()
        {
            Session[UserHelper.SystemUserSessionKey] = null;
            return Redirect(SsoHelper.SSOLogOutUrl);
        }
    }

}
