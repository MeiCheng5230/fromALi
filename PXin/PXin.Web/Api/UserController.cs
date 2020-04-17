using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.Filter;
using Common.Mvc.Models;
using PXin.DB;
using PXin.Facade;
using PXin.Facade.ApiFacade;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.UserDto;
using PXin.Facade.Models.UserReq;

namespace PXin.Web.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// 设置V点自动充值数量(关闭时，Amount设置为0)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase SetAutoChargeVPoint(SetAutoChargeVPointReq req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var reginfo = HttpContext.Current.GetRegInfo();
            var facade = new UserFacade();
            if (!facade.CheckPayPwd(reginfo, req.PayPwd))
            {
                return new Respbase() { Result = facade.PromptInfo.Result, Message = facade.PromptInfo.Message };
            }
            var userinfo = db.TpxinUserinfoSet.FirstOrDefault(f => f.Nodeid == req.Nodeid);
            if (userinfo == null)
            {
                return new Respbase() { Result = -1, Message = "无效用户" };
            }
            userinfo.Autochargevamount = req.Amount;
            if (db.SaveChanges() < 0)
            {
                return new Respbase() { Result = -1, Message = "设置失败" };
            }
            return new Respbase() { Result = 1, Message = "设置成功" };
        }

        /// <summary>
        /// 获取V点自动充值的数量(为0时，为未设置)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<AutoChargeAmountVDto> GetAutoChargeAmountV(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var userinfo = db.TpxinUserinfoSet.FirstOrDefault(f => f.Nodeid == req.Nodeid);
            if (userinfo == null)
            {
                return new Respbase<AutoChargeAmountVDto>() { Result = -1, Message = "无效用户", Data = new AutoChargeAmountVDto() };
            }
            return new Respbase<AutoChargeAmountVDto>() { Data = new AutoChargeAmountVDto() { Amount = userinfo.Autochargevamount } };
        }

        /// <summary>
        /// 自动充值V点，当SV不足时，返回微信充值SVC的充值地址(Result=-100时，表示SV余额不足)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Respbase AutoChargeVPoint(Reqbase req)
        {
            var facade = new UserFacade();
            var flag = facade.AutoChargeVPoint(req.Nodeid);
            return facade.PromptInfo;
        }

        /// <summary>
        /// 获取用户邀请码使用情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<InviteesDto>> GetInviteesList(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();
            var query = from code in db.TnetReginfoCodeSet
                        join reg in db.TnetReginfoSet on code.Usenodeid equals reg.Nodeid into temp
                        from invitees in temp.DefaultIfEmpty()
                        where code.Nodeid == req.Nodeid
                        orderby code.Createtime descending
                        select new InviteesDto()
                        {
                            InvitationCode = code.Code,
                            Nodecode = invitees.Nodecode,
                            NodeName = invitees.Nodename,
                            UseTime = code.Usetime
                        };
            return new Respbase<List<InviteesDto>>() { Data = query.ToList() };
        }

        /// <summary>
        /// 注册验证手机号是否可用 
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnLogin]
        public Respbase RegValidateMobile(ReqSms req)
        {
            if (!AppConfig.IsOpenRegister)
            {
                return new Respbase { Result = -1, Message = "对不起，内测阶段暂停注册" };
            }
            UserFacade userFacade = new UserFacade();
            userFacade.RegValidateMobile(req.Mobileno.Trim(), req.InvitationCode);
            return userFacade.PromptInfo;
        }
        /// <summary>
        /// 用户注册 
        /// </summary>
        /// <param name="reg">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [UnLogin]
        public Respbase<RegInfoDto> Reg(ReqReginfo reg)
        {
            if (!AppConfig.IsOpenRegister)
            {
                return new Respbase<RegInfoDto> { Result = -1, Message = "对不起，内测阶段暂停注册" };
            }
            //1.注册新用户
            UserFacade user = new UserFacade();
            var result = user.Reg(reg);
            if (result == null)
            {
                return new Respbase<RegInfoDto> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            if (reg.Opentype == 4)
            {
                user.PxintoXinUser(reg.Openid, result.Nodeid);
            }

            //注册pxin用户并给推荐人发送添加好友邀请
            user.RegChatUser(reg, result);
            return new Respbase<RegInfoDto> { Result = 1, Message = "注册成功", Data = new RegInfoDto { Nodeid = result.Nodeid, Nodecode = result.Nodecode } };
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="login">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<LoginInfoDto> UserLogin(ReqLogin login)
        {
            UserFacade user = new UserFacade();
            var result = user.Login(login);
            if (result == null)
            {
                return new Respbase<LoginInfoDto> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<LoginInfoDto> { Result = 1, Message = "登录成功", Data = result };
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userinfo">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<UserInfoDto> GetUserInfo(ReqUser userinfo)
        {
            userinfo.Nodecode = userinfo.Nodecode.Trim();
            ////方法一,自己写SQL
            //UETokenContext db = HttpContext.Current.GetDbContext<UETokenContext>();
            //string sql = string.Format(@"select nodeid
            //            ,substr(nodecode,0,2)||'**'||substr(nodecode,7) nodecode
            //            ,substr(nodename,0,1)||'**' nodename
            //            ,case when mobileno is not null then substr(mobileno,0,3)||'****'||substr(mobileno,8) else '' end mobileno
            //            ,case when email is not null then substr(email,0,1)||'**'||substr(email,instr(email,'@')) else '' end email
            //            from tnet_reginfo
            //            where nodecode ={0}
            //            or mobileno = {0}
            //            or email = {0}", SqlHelper.ToSQLParamStr(userbaseinfo.Nodecode));
            //var reg = db.SqlQuery<UserInfo>(sql).FirstOrDefault();
            //if (reg == null)
            //    return Json(new Respbase<UserInfo> { Result = -1, Message = "用户不存在", Data = null });

            //return Json(new Respbase<UserInfo> { Data = reg });

            //方法二,通过LINQ获取数据,然后自己做判断
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodecode == userinfo.Nodecode || c.Mobileno == userinfo.Nodecode || c.Email == userinfo.Nodecode).FirstOrDefault();
            if (tnet_reginfo != null)
            {
                string nodeNameFilter = Helper.FilterChar(tnet_reginfo.Nodename);
                //获取到主账号
                return new Respbase<UserInfoDto>
                {
                    Data = new UserInfoDto
                    {
                        Nodeid = tnet_reginfo.Nodeid,
                        Nodecode = tnet_reginfo.Nodecode.Substring(0, 2) + "**" + (tnet_reginfo.Nodecode.Length <= 6 ? "" : tnet_reginfo.Nodecode.Substring(6)),
                        Nodename = nodeNameFilter.Length > 0 ? nodeNameFilter.Substring(0, 1) + "**" : "",
                        Mobileno = tnet_reginfo.Mobileno == null ? "" : tnet_reginfo.Mobileno.Substring(0, 3) + "****" + (tnet_reginfo.Mobileno.Length <= 7 ? "" : tnet_reginfo.Mobileno.Substring(7)),
                        Email = tnet_reginfo.Email == null ? "" : tnet_reginfo.Email.Substring(0, 1) + "**" + (tnet_reginfo.Email.IndexOf("@") <= 0 ? "" : tnet_reginfo.Email.Substring(tnet_reginfo.Email.IndexOf("@")))
                    }
                };
            }
            //没找到主账号
            if (userinfo.Purseid == 0)
            {
                return new Respbase<UserInfoDto> { Result = -1, Message = "用户不存在", Data = null };
            }
            else
            {
                //purseid 大于0,进一步判断其子账号
                var tblc_user_purse_sub = db.TblcUserPurseSub2Set.Where(c => c.Pursecode == userinfo.Nodecode).FirstOrDefault();
                if (tblc_user_purse_sub == null)
                {
                    //没找到子账号
                    return new Respbase<UserInfoDto> { Result = -1, Message = "子账号不存在", Data = null };
                }
                List<TblcUserPurse> userPurses = db.TblcUserPurseSet.Where(c => c.Purseid == tblc_user_purse_sub.Purseid || c.Purseid == userinfo.Purseid).ToList();
                if (userPurses == null || userPurses.Count != 2
                    || userPurses[0].Pursetype != userPurses[1].Pursetype
                    || userPurses[0].Ownerid != userPurses[1].Ownerid
                    || userPurses[0].Subid != userPurses[1].Subid
                    || userPurses[0].Currencytype != userPurses[1].Currencytype)
                {
                    return new Respbase<UserInfoDto> { Result = -1, Message = "用户不存在", Data = null };
                }
                //找到子账号
                return new Respbase<UserInfoDto>
                {
                    Data = new UserInfoDto
                    {
                        Nodeid = tblc_user_purse_sub.Nodeid,
                        Nodecode = tblc_user_purse_sub.Pursecode,
                        Nodename = tblc_user_purse_sub.Pursename,
                        Mobileno = "",
                        Email = ""
                    }
                };
            }
        }


        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="sign">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<SignInfoDto> Sign(ReqSign sign)
        {
            UserFacade user = new UserFacade();
            var result = user.Sign(sign);
            if (result == null)
            {
                return new Respbase<SignInfoDto> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<SignInfoDto> { Result = 1, Message = "签到成功", Data = result };
        }

        /// <summary>
        /// 获得我的基本信息(我页面调用)
        /// </summary>
        /// <param name="my">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<LoginInfoDto> GetMy(Reqbase my)
        {
            UserFacade user = new UserFacade();
            var result = user.GetMy(my);
            if (result == null)
            {
                return new Respbase<LoginInfoDto> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<LoginInfoDto> { Result = 1, Message = "操作成功", Data = result };
        }

        /// <summary>
        /// 修改用户常规信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase EditUserInfo(ReqEditUserInfo userinfo)
        {
            UserFacade user = new UserFacade();
            user.EditUserInfo(userinfo);
            return user.PromptInfo;
        }
        /// <summary>
        /// 修改用户昵称和个性签名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase UpdateUserNickAndSign(UserNickAndSignReq req)
        {
            UserFacade user = new UserFacade();
            user.UpdateUserNickAndSign(req);
            return user.PromptInfo;
        }
        /// <summary>
        /// 下个版本删除此方法
        /// </summary>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public Respbase EditLoginPwd(ReqEditLoginPwd loginPwd)
        {
            UserFacade user = new UserFacade();
            user.EditLoginPwd(loginPwd);
            return user.PromptInfo;
        }
        /// <summary>
        /// 修改密码、支付密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase ChangePwd(ChangePwdReq req)
        {
            UserFacade user = new UserFacade();
            user.ChangePwd(req);
            return user.PromptInfo;
        }
        /// <summary>
        /// 忘记密码、支付密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase ForgetPwd(ForgetPwdReq req)
        {
            UserFacade user = new UserFacade();
            user.ForgetPwd(req);
            return user.PromptInfo;
        }
        /// <summary>
        /// 修改用户的手机号码
        /// </summary>
        /// <param name="editMobileno"></param>
        /// <returns></returns>
        public Respbase EditMobileno(ReqEditMobileno editMobileno)
        {
            UserFacade user = new UserFacade();
            user.EditMobileno(editMobileno);
            return user.PromptInfo;
        }

        /// <summary>
        /// 查询用户所有第三方账号绑定状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<UserOpensDto>> GetUserOpens(Reqbase req)
        {
            var db = HttpContext.Current.GetDbContext<PXinContext>();

            var query = from tsso_open_user in db.TssoOpenUserSet
                        where tsso_open_user.Nodeid == req.Nodeid
                        select new UserOpensDto
                        {
                            Opentype = tsso_open_user.Opentype,
                            NodeCode = tsso_open_user.Openid,
                            Status = 1
                        };
            List<UserOpensDto> UseropensList = query.ToList();
            List<UserOpensDto> list = new List<UserOpensDto>() {
                new UserOpensDto {  Opentype=1, Status=0 },
                new UserOpensDto {  Opentype=2, Status=0 },
                new UserOpensDto {  Opentype=3, Status=0 },
                new UserOpensDto {  Opentype=4, Status=0 },
                new UserOpensDto {  Opentype=5, Status=0 }
            };
            foreach (var item in UseropensList)
            {
                foreach (var item1 in list)
                {
                    if (item.Opentype == item1.Opentype)
                    {
                        item1.NodeCode = item.NodeCode;
                        item1.Status = 1;
                    }
                }
            }
            return new Respbase<List<UserOpensDto>> { Data = list };
        }

        /// <summary>
        /// 绑定第三方账号接口
        /// </summary>
        /// <param name="createUserOpen"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase CreateUserOpen(ReqCreateUserOpen createUserOpen)
        {
            UserFacade user = new UserFacade();
            user.CreateUserOpen(createUserOpen);
            return user.PromptInfo;
            //throw new NotSupportedException("");
        }


        /// <summary>
        /// 解除第三方账号绑定
        /// </summary>
        /// <param name="delUserOpen"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteUserOpen(ReqDeleteUserOpen delUserOpen)
        {
            UserFacade user = new UserFacade();
            user.DeleteUserOpen(delUserOpen);
            return user.PromptInfo;
            //throw new NotSupportedException("");
        }


        /// <summary>
        /// 通讯录用户是否注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<IsRegDto>> IsReg(ReqIsReg req)
        {
            UserFacade user = new UserFacade();
            List<IsRegDto> result = user.IsReg(req);
            if (result == null)
            {
                return new Respbase<List<IsRegDto>> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<List<IsRegDto>> { Data = result };

        }


        /// <summary>
        /// 邀请通讯录好友注册 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase InviteReg(ReqIsReg req)
        {
            UserFacade user = new UserFacade();
            bool result = user.InviteReg(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 查询是否同意协议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase GetIsAgree(AgreementReq req)
        {
            UserFacade user = new UserFacade();
            bool result = user.GetIsAgree(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 设置同意协议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase SetAgreeAgreement(AgreementReq req)
        {
            UserFacade user = new UserFacade();
            bool result = user.AgreeAgreement(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 设置加我为好友时是否需要验证
        /// </summary>
        [HttpPost]
        public Respbase SetIsValidFriend(IsValidDto req)
        {
            UserFacade user = new UserFacade();
            bool result = user.EditIsValidFriend(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 通知设置：系统动态通知，1开，0关
        /// </summary>
        [HttpPost]
        public Respbase SetIsSysNotice(IsOpenDto req)
        {
            UserFacade user = new UserFacade();
            bool result = user.EditIsSysNotice(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 通知设置：通知显示详情，1开，0关
        /// </summary>
        [HttpPost]
        public Respbase SetIsNoticeDetail(IsOpenDto req)
        {
            UserFacade user = new UserFacade();
            bool result = user.EditIsNoticeDetail(req);
            return user.PromptInfo;
        }

        /// <summary>
        /// 意见反馈
        /// </summary>
        [HttpPost]
        public Respbase CreateFeedback(FeedbackReq req)
        {
            UserFacade user = new UserFacade();
            bool result = user.CreateFeedback(req);
            return user.PromptInfo;
        }

        #region 获取pcn 优谷 ue 用户信息
        /// <summary>
        /// 获取pcn 优谷 ue 用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<ChargeUserInfoDto> GetYGPCNUEUserInfo(ReqExchangeUserInfo req)
        {
            ExchangeFacade exchange = new ExchangeFacade();
            ChargeUserInfoDto data = null;
            int resultcode = 0;
            switch (req.typeId)
            {
                case 0:
                    data = exchange.GetUeInfo(req.Nodeid, 1, out resultcode);
                    break;
                case 4:
                    data = exchange.GetPCNUserInfo(req.nodeCode, req.Nodeid);
                    break;
                case 5:
                    data = exchange.GetYGUserInfo(req.nodeCode);
                    break;
                default:
                    exchange.Alert("typeid不正确");
                    break;
            }
            if (data == null)
            {
                var isue = req.typeId == 0;
                return new Respbase<ChargeUserInfoDto> { Result = isue ? resultcode : 0, Message = exchange.PromptInfo.Message };
            }
            return new Respbase<ChargeUserInfoDto> { Result = 1, Data = data };
        }
        #endregion

        /// <summary>
        /// 登录绑定pcn账号
        /// </summary>
        [HttpPost]
        public Respbase BindPcnAcount(BindPcnAcountReq req)
        {
            var facade = new UserFacade();
            facade.BindPcnAcount(req);
            return facade.PromptInfo;
        }

        /// <summary>
        /// 用户的站内信(消息中心)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<List<MailDto>> GetMails(ReqMail req)
        {
            UserFacade user = new UserFacade();
            List<MailDto> result = user.GetMails(req);
            if (result == null)
            {
                return new Respbase<List<MailDto>> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<List<MailDto>> { Data = result };

        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase DeleteMail(ReqDeleteMail req)
        {
            UserFacade user = new UserFacade();
            var result = user.DeleteMail(req);
            return user.PromptInfo;

        }

        /// <summary>
        /// 读消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase ReadMail(ReqReadMail req)
        {
            UserFacade user = new UserFacade();
            var result = user.ReadMail(req);
            return user.PromptInfo;

        }

        /// <summary>
        /// 是否存在未读消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Respbase<IsMailDto> IsMails(Reqbase req)
        {
            UserFacade user = new UserFacade();
            var result = user.IsMails(req);
            if (result == null)
            {
                return new Respbase<IsMailDto> { Result = user.PromptInfo.Result, Message = user.PromptInfo.Message, Data = null };
            }
            return new Respbase<IsMailDto> { Data = result };

        }

    }
}
