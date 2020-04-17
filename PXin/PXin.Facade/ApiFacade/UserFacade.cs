using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using MvcPaging;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.CommonService;
using PXin.Facade.Models;
using PXin.Facade.Models.UserDto;
using PXin.Facade.Models.UserReq;
using PXin.Model;
using Winner.EncodeDecode;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class UserFacade : FacadeBase<PXinContext>
    {
        #region 自动充值V点
        /// <summary>
        /// 自动充值V点
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public bool AutoChargeVPoint(int nodeid)
        {
            var userinfo = db.TpxinUserinfoSet.FirstOrDefault(f => f.Nodeid == nodeid);
            var reginfo = HttpContext.Current.GetRegInfo();
            if (userinfo == null)
            {
                Alert("无效用户", -1);
                return false;
            }
            if (userinfo.Autochargevamount == 0M)
            {
                Alert("未设置自动充值功能", -2);
                return false;
            }
            FriFacade facade_fri = new FriFacade();
            log.Info("自动充值V点：Nodeid=" + nodeid + "，Amount=" + userinfo.Autochargevamount);
            if (!facade_fri.ChargeVDian_SVPay(reginfo, userinfo.Autochargevamount, "", false))
            {
                //Result=-100时，表示SVC余额不足
                Alert(facade_fri.PromptInfo.Message, facade_fri.PromptInfo.Result);
                return false;
            }
            return true;
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 注册发送短信
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="invitationcode">邀请码</param>
        /// <returns></returns>
        public bool RegValidateMobile(string mobileNo, string invitationcode)
        {
            log.Info("RegValidateMobile手机号：" + mobileNo);
            if (string.IsNullOrEmpty(mobileNo) || (mobileNo.Substring(0, 1) != "+" && mobileNo.Length > 11))//|| mobileNo.Length != 11 || !mobileNo.StartsWith("1"))
            {
                Alert("该手机号码格式不正确");
                return false;
            }
            var tnetRegCode = db.TnetReginfoCodeSet.Where(w => w.Code.Equals(invitationcode)).FirstOrDefault();
            if (tnetRegCode == null)
            {
                Alert("无效邀请码");
                return false;
            }
            string tempMobile = mobileNo.Replace("+", "");
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Mobileno == tempMobile).FirstOrDefault();
            if (tnet_reginfo != null)
            {
                //该手机号码已经被注册了
                Alert("该手机号码已经注册,请直接登录");
                return false;
            }
            SmsFacade smsFacade = new SmsFacade();
            smsFacade.SendSms(9, 0, mobileNo, string.Empty, 81127);
            Alert(smsFacade.PromptInfo.Message, smsFacade.PromptInfo.Result);
            return smsFacade.PromptInfo.Result > 0;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        public RegInfoDto Reg(ReqReginfo reg)
        {
            if (reg.Mobileno.Substring(0, 1) == "+")
            {
                reg.Mobileno = reg.Mobileno.Substring(1);
            }
            //1.判断该手机号码是否存在(是否已经注册了)
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Mobileno == reg.Mobileno.Trim()).FirstOrDefault();
            if (tnet_reginfo != null)
            {
                //该手机号码已经被注册了
                Alert("该手机号码已经注册,请直接登录");
                return null;
            }
            var tnetRegCode = db.TnetReginfoCodeSet.Where(w => w.Code.Equals(reg.InvitationCode.Trim())).FirstOrDefault();
            if (tnetRegCode == null)
            {
                Alert("无效邀请码");
                return null;
            }
            if (tnetRegCode.Usenodeid != 0)
            {
                Alert("此邀请码已被使用");
                return null;
            }
            //2.如果有推荐人,检查推荐人是否合法
            TnetReginfo tnet_reginfo_p = null;
            int? pnodeid = null;
            if (!string.IsNullOrWhiteSpace(reg.Pnodecode))
            {
                tnet_reginfo_p = db.TnetReginfoSet.Where(c => c.Mobileno == reg.Pnodecode.Trim() || c.Nodecode == reg.Pnodecode.Trim() || c.Email == reg.Pnodecode.Trim()).FirstOrDefault();
                if (tnet_reginfo_p == null)
                {
                    //推荐人不存在
                    Alert("推荐人不存在");
                    return null;
                }
                else
                {
                    pnodeid = tnet_reginfo_p.Nodeid;
                }
            }
            else
            {
                if (tnetRegCode == null)
                {
                    //传过来的推荐人不存在 查询短信邀请表里面是否存在数据 10天内的数据
                    DateTime validTime = DateTime.Now.AddDays(-10);
                    TnetInvitehis invitehis = db.TnetInvitehisSet.Where(a => a.Createtime >= validTime && a.Mobileno == reg.Mobileno.Trim()).OrderBy(a => a.Createtime).ThenBy(a => a.Id).FirstOrDefault();
                    if (invitehis != null)
                    {
                        pnodeid = invitehis.Pnodeid;
                        invitehis.Status = 1;
                    }
                }
                else
                {
                    pnodeid = tnetRegCode.Nodeid;
                }
            }
            if (AppConfig.IsUseSms)//判断是否是测试环境
            {
                //2.获取最近一条合法的短信验证码
                var tsso_regcode = db.TssoRegcodeSet.Where(c => c.Regcode == reg.Mobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                if (tsso_regcode == null)
                {
                    //获取短信验证码失败
                    Alert("获取验证码失败");
                    return null;
                }

                //3.判断用户的短信验证码是否正确
                if (reg.SmsCode.Trim() != tsso_regcode.Authcode.Trim())
                {
                    //验证码不正确
                    Alert("验证码有误");
                    return null;
                }

                //8.修改tsso_regcode验证码的状态为已使用
                tsso_regcode.Status = 1;
            }

            //输入数据判断完毕,开始操作            
            string sql = string.Format(@"select * from (
                                        select * 
                                        from tsso_usercode 
                                        where status=0 
                                        order by dbms_random.value())
                                        where rownum = 1");
            var tsso_usercode = db.SqlQuery<TssoUsercode>(sql).FirstOrDefault();
            if (tsso_usercode == null)
            {
                //获取账号失败
                Alert("获取账号失败");
                return null;
            }
            //db.BeginTransaction();

            //5.插入tnet_reginfo注册表
            TnetReginfo tnet_reginfo2 = new TnetReginfo();
            tnet_reginfo2.Nodeid = db.GetPrimaryKeyValue<TnetReginfo>();
            tnet_reginfo2.Nodecode = tsso_usercode.Usercode.ToString().Trim();//账号
            tnet_reginfo2.Nodename = reg.Mobileno.Trim();//姓名
            tnet_reginfo2.Status = 1;
            tnet_reginfo2.Cashalert = 50;//最高免密支付金额
            tnet_reginfo2.Isconfirmed = 0;//未认证
            tnet_reginfo2.Introducer = pnodeid;//推荐人
            //tnet_reginfo2.Createtime = DateTime.Now;
            //var passwordString = "";
            //if (reg.Pwd != "")
            //{
            //    var password = Encoding.UTF8.GetString(Convert.FromBase64String(reg.Pwd));
            //    if (password.Length < 6)
            //    {
            //        Alert("密码长度最少6位");
            //        return null;
            //    }
            //    passwordString = UserPwd.Encode(password);
            //}
            //var passwordString = Encoding.UTF8.GetString(Convert.FromBase64String(reg.Pwd));
            var passwordString = reg.SmsCode.Trim();

            tnet_reginfo2.Userpwd = UserPwd.Encode(passwordString);//登录密码,需加密
            tnet_reginfo2.UserpwdBak = UserPwd.Encode(passwordString);//支付密码为空
            tnet_reginfo2.Mobileno = reg.Mobileno.Trim();//手机号码
            tnet_reginfo2.Email = "";//电子信箱
            //插入
            db.TnetReginfoSet.Add(tnet_reginfo2);

            //修改注册码使用信息
            tnetRegCode.Usenodeid = tnet_reginfo2.Nodeid;
            tnetRegCode.Usetime = DateTime.Now;

            //6.记录日志
            TueLoginHis tue_login_his = new TueLoginHis();
            tue_login_his.Nodeid = tnet_reginfo2.Nodeid;
            tue_login_his.Typeid = 1;//注册
            tue_login_his.Clientid = reg.Clientid;//客户端类型
            tue_login_his.Version = reg.Version;//版本号
            tue_login_his.Sid = reg.Sid;//APPID
            tue_login_his.Token = "";//唯一标记
            tue_login_his.Accesskeyid = "";
            tue_login_his.Timestamp = reg.Tm;//时间戳
            tue_login_his.Signaturenonce = "";
            tue_login_his.Signature = reg.Sign;
            tue_login_his.Createtime = DateTime.Now;
            tue_login_his.Remarks = "注册";

            db.TueLoginHisSet.Add(tue_login_his);

            //7.将tsso_usercode那条值状态由0改成1
            tsso_usercode.Status = 1;
            //SQL查询出来的对象，默认是不会有状态跟踪的，需要修改时，要手动修改数据状态
            db.Entry(tsso_usercode).State = System.Data.Entity.EntityState.Modified;

            //db.Entry(tsso_usercode).Property(c => c.Status).IsModified = true;

            //9.把几个常用钱包全部给他初始化出来(DOS,DOS库存,BTC,CNV,USDT)
            db.TblcUserPurseSet.Add(CreateUserPurse(tnet_reginfo2.Nodeid, 4, 0, 2));
            db.TblcUserPurseSet.Add(CreateUserPurse(tnet_reginfo2.Nodeid, 65, 0, 8));
            db.TblcUserPurseSet.Add(CreateUserPurse(tnet_reginfo2.Nodeid, 3, 11, 4));//UV

            //10.记录协议
            var version = db.TnetUserProtocalSet.Where(c => c.Type == 20001 && c.Status == 1).FirstOrDefault();
            if (version == null)
            {
                Alert("协议不存在");
                return null;
            }
            AgreeAgreement_Pro(tnet_reginfo2.Nodeid, version.Type, version.Version);



            if (reg.Opentype > 0)
            {
                //检查第三方账户是否已经被占用
                TssoOpenUser temp = db.TssoOpenUserSet.Where(c => c.Openid == reg.Openid && c.Opentype == reg.Opentype).FirstOrDefault();
                if (temp != null)
                {
                    Alert("该账户已经注册,请直接登录");
                    return null;
                }

                TssoOpenUser tss_open_user = new TssoOpenUser();
                tss_open_user.Nodeid = tnet_reginfo2.Nodeid;
                tss_open_user.Opentype = reg.Opentype;
                tss_open_user.Openid = reg.Openid;//优谷和PCN时,这里为nodecode
                tss_open_user.Createtime = DateTime.Now;
                tss_open_user.Remarks = "绑定第三方账号";
                db.TssoOpenUserSet.Add(tss_open_user);
            }
            //注册送V点
            var result = int.TryParse(AppConfig.RegisterGiveVDian, out int v);
            var tpxinUserInfo = new TpxinUserinfo
            {
                Nodeid = tnet_reginfo2.Nodeid,
                Backpic = "",
                Up = 0,
                Down = 0,
                P = 0,
                V = result ? v : 0,
                Createtime = DateTime.Now,
                Remarks = "注册送V点",
                Lastbuysvc = 0,
                Svc = 0,
                Apoint = 0,
                Autochargevamount = 0
            };
            db.TpxinUserinfoSet.Add(tpxinUserInfo);

            var am = new TpxinAmountChangeHis
            {
                Nodeid = tnet_reginfo2.Nodeid,
                Typeid = 1,
                Amount = result ? v : 0,
                Reason = (int)Models.Enum.AmountChangeReason.RegisterGiveVDian,
                Transferid = Guid.NewGuid().ToString(),
                Createtime = DateTime.Now,
                Remarks = "注册送V点",
                Amountbefore = 0,
                Amountafter = v,
            };
            db.TpxinAmountChangeHisSet.Add(am);

            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("注册失败");
                return null;
            }
            //db.Commit();
            Alert("注册成功", 1);

            //注册融云账号
            ChatFacade facade = new ChatFacade();
            facade.RegUser(tnet_reginfo2, "", "");

            return new RegInfoDto { Nodeid = tnet_reginfo2.Nodeid, Nodecode = tnet_reginfo2.Nodecode };
        }


        /// <summary>
        /// 同一个手机号是P信的好友同步到相信
        /// </summary>
        /// <param name="usercode">pcn账号</param>
        /// <param name="nodeid">注册成功的用户id</param>
        public bool PxintoXinUser(string usercode, int nodeid)
        {
            log.Info($"同一个手机号是P信的好友同步到相信，pcn账号={usercode}-相信nodeid={nodeid}");
            string mobilenos = "";
            var feeResult = Rong.RongApi.GetPxinFriend(usercode);
            if (feeResult == null || !feeResult.Result)
            {
                log.Info("调用接口获取融云好友手机号异常:" + (feeResult == null ? string.Empty : feeResult.Msg));
                Alert("调用接口获取融云好友手机号异常," + (feeResult == null ? string.Empty : feeResult.Msg));
                return false;
            }
            mobilenos = feeResult.Data;
            string[] mobattr = mobilenos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int[] nodeidattr = db.TnetReginfoSet.Where(a => mobattr.Contains(a.Mobileno)).Select(a => a.Nodeid).ToArray();
            int[] chatuserattr = db.TchatUserSet.Where(a => nodeidattr.Contains(a.Nodeid)).Select(a => a.Nodeid).ToArray();
            if (chatuserattr.Length <= 0)
            {
                log.Info($"没有需要同步的好友");
                Alert("没有需要同步的好友");
                return false;
            }
            List<TchatFriend> tchatFriends = new List<TchatFriend>();
            foreach (var item in chatuserattr)
            {
                TchatFriend tchatFriend = new TchatFriend()
                {
                    Friendnodeid = item,
                    Friendstatus = 1,
                    Mynodeid = nodeid,
                    Remarks = "绑定事同步Pxin好友到相信"
                };
                tchatFriends.Add(tchatFriend);
            }
            db.TchatFriendSet.AddRange(tchatFriends);
            if (db.SaveChanges() <= 0)
            {
                log.Info("注册绑定同步Pxin好友到相信失败" + db.Message);
                Alert("添加好友失败");
                return false;
            }

            return true;
        }


        /// <summary>
        /// 注册pxin用户并给推荐人发送添加好友邀请
        /// </summary>
        /// <param name="reg">注册请求参数</param>
        /// <param name="result">注册新用户信息</param>
        public bool RegChatUser(ReqReginfo reg, RegInfoDto result)
        {
            log.Info($"注册pxin用户,nodeid={result.Nodeid}");
            //2.注册相信用户
            ChatFacade chat = new ChatFacade();
            TnetReginfo reginfo = db.TnetReginfoSet.Find(result.Nodeid);
            //TchatUser tchatUser = chat.RegUser(reginfo, string.Empty, string.Empty);
            //if (tchatUser == null)
            //{
            //    log.Info($"注册pxin用户,nodeid={result.Nodeid}");
            //    Alert("注册Pxin用户失败");
            //    return false;
            //}
            log.Info($"注册pxin用户成功，判断是否有邀请人给邀请人发送好友请求,nodeid={result.Nodeid}");//，chatuser={tchatUser.Id}");
            #region 注册成功 给邀请人发送好友邀请推送
            //3.给邀请人发送好友邀请推送
            TnetReginfo tnet_reginfo_p = null;
            AddFriendReq req = new AddFriendReq();
            req.Nodeid = reginfo.Nodeid;
            if (!string.IsNullOrWhiteSpace(reg.Pnodecode))
            {//如果推荐人存在给推荐人发送添加好友请求
                log.Info($"给推荐人发送添加好友请求,Pnodecode={reg.Pnodecode}，nodeid={req.Nodeid}");
                tnet_reginfo_p = db.TnetReginfoSet.FirstOrDefault(a => a.Nodecode == reg.Pnodecode);
                if (tnet_reginfo_p == null)
                {
                    return false;
                }
                req.usercode = tnet_reginfo_p.Nodecode;
                req.remarks = "";
                chat.AddFriend(req);
            }
            else
            {
                //传过来的推荐人不存在 查询短信邀请表里面是否存在数据
                int[] pnodeids = db.TnetInvitehisSet.Where(a => a.Mobileno == reg.Mobileno.Trim()).Select(a => a.Pnodeid).Distinct().ToArray();
                log.Info($"推荐人不存在 查询短信邀请表里面是否存在数据发送添加好友请求,Pnodeids={pnodeids}，nodeid={req.Nodeid}");
                if (pnodeids.Length > 0)
                {
                    foreach (int item in pnodeids)
                    {
                        tnet_reginfo_p = db.TnetReginfoSet.Find(item);
                        if (tnet_reginfo_p == null)
                        {
                            continue;
                        }
                        req.usercode = tnet_reginfo_p.Nodecode;
                        req.remarks = "";
                        chat.AddFriend(req);
                    }
                }
            }
            #endregion
            return true;
        }



        /// <summary>
        /// 创建钱包
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pursetype"></param>
        /// <param name="subid"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        private TblcUserPurse CreateUserPurse(int nodeid, int pursetype, int subid, int currencyType)
        {
            TblcUserPurse tblc_user_purse_usdt = new TblcUserPurse();
            tblc_user_purse_usdt.Ownerid = nodeid;
            tblc_user_purse_usdt.Pursetype = Convert.ToByte(pursetype);
            tblc_user_purse_usdt.Subid = subid;
            tblc_user_purse_usdt.Balance = 0;
            tblc_user_purse_usdt.Minvalue = 0;
            tblc_user_purse_usdt.Ownertype = 1;
            tblc_user_purse_usdt.Currencytype = currencyType;
            tblc_user_purse_usdt.HisidLast = 0;
            tblc_user_purse_usdt.CreatetimeA = DateTime.Now;
            return tblc_user_purse_usdt;
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public LoginInfoDto Login(ReqLogin login)
        {
            log.Info("Login手机号：" + login.Nodecode);
            login.Nodecode = Uri.UnescapeDataString(login.Nodecode);
            if (login.Nodecode.Substring(0, 1) == "+")
            {
                login.Nodecode = login.Nodecode.Substring(1);
            }
            TnetReginfo tnet_reginfo = null;
            if (login.Logintype <= 0)
            {
                //1.判断用户账号是否存在
                tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodecode == login.Nodecode || c.Mobileno == login.Nodecode || c.Email == login.Nodecode).FirstOrDefault();
                if (tnet_reginfo == null)
                {
                    //未找到该用户
                    Alert("该账号未注册" + login.Nodecode);
                    return null;
                }
                #region 验证输入的是密码还是验证码


                //验证输入的是密码还是验证码
                if (IsBase64(login.Pwd))
                {
                    //2.检查密码是否正确
                    if (!CheckPwd(tnet_reginfo, login.Pwd))
                    {
                        //密码不正确
                        return null;
                    }
                }
                else   //验证码
                {
                    if (AppConfig.IsUseSms)//判断是否是测试环境
                    {
                        //1.获取最近一条合法的短信验证码
                        var tsso_regcode = db.TssoRegcodeSet.Where(c => c.Regcode == login.Nodecode && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                        if (tsso_regcode == null)
                        {
                            //获取短信验证码失败
                            Alert("获取验证码失败");
                            return null;
                        }

                        //2.判断用户的短信验证码是否正确
                        if (login.Pwd.Trim() != tsso_regcode.Authcode.Trim())
                        {
                            //验证码不正确
                            Alert("验证码有误");
                            return null;
                        }
                        tsso_regcode.Status = 1;    //修改验证码的状态
                    }
                }
                #endregion
            }
            //如果是第三方账号登录
            else
            {
                TssoOpenUser tss_open_user = db.TssoOpenUserSet.Where(c => c.Opentype == login.Logintype && c.Openid == login.Nodecode).FirstOrDefault();
                if (tss_open_user == null)
                {
                    Alert("该账户尚未绑定", -2);
                    return null;
                }
                tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == tss_open_user.Nodeid).FirstOrDefault();
                if (tnet_reginfo == null)
                {
                    Alert("获取用户基本信息失败");
                    return null;
                }
            }

            //3.检查用户是否被冻结
            TnetLockuser tnet_lockuser = db.TnetLockuserSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Locktype == 1 && c.Locktime < DateTime.Now && c.Unlocktime > DateTime.Now).FirstOrDefault();
            if (tnet_lockuser != null)
            {
                //用户被冻结
                Alert(tnet_lockuser.Remarks);//冻结原因
                return null;
            }

            //4.获取头像
            TnetUserphoto tnet_userphoto = db.TnetUserphotoSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid).FirstOrDefault();
            if (tnet_userphoto == null || tnet_userphoto.Appphoto == null)
            {
                //给他赋值个默认值
                tnet_userphoto = new TnetUserphoto { Appphoto = "userphoto/noempty.png" };
            }

            //5.记录日志TUE_LOGIN_HIS
            TueLoginHis tue_login_his = new TueLoginHis();
            tue_login_his.Nodeid = tnet_reginfo.Nodeid;
            tue_login_his.Typeid = 2;//登录
            tue_login_his.Clientid = login.Client;//客户端类型
            tue_login_his.Version = login.Version;//版本号
            tue_login_his.Sid = login.Sid;//APPID
            tue_login_his.Token = "";//手机的唯一标记
            tue_login_his.Accesskeyid = "";//系统分配
            tue_login_his.Timestamp = login.Tm;//时间戳
            tue_login_his.Signaturenonce = "";//guid
            tue_login_his.Signature = login.Sign;//签名结果
            tue_login_his.Createtime = DateTime.Now;//登录时间
            tue_login_his.Remarks = "登录";
            db.TueLoginHisSet.Add(tue_login_his);

            TnetReginfoExt ext = db.TnetReginfoExtSet.FirstOrDefault(c => c.Nodeid == tnet_reginfo.Nodeid);
            if (ext == null)
            {
                ext = new TnetReginfoExt() { Nodeid = tnet_reginfo.Nodeid };
                ext.Gtclientid = login.Gtclientid;
                ext.Devicetoken = login.Devicetoken;
                db.TnetReginfoExtSet.Add(ext);
            }
            else
            {
                ext.Gtclientid = login.Gtclientid;
                ext.Devicetoken = login.Devicetoken;
            }
            //5.更新数据到数据库
            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("登录失败");
                return null;
            }

            //6.获取未读消息数量
            //var tue_message = db.TueMessageSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Status == 0).ToList();
            int newCount = 0;
            //if(tue_message != null)
            //{
            //    newCount = tue_message.Count;
            //}
            Alert("登录成功", 1);
            TchatUser cu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == tnet_reginfo.Nodeid);
            if (cu == null)
            {
                //注册融云账号
                ChatFacade facade = new ChatFacade();
                cu = facade.RegUser(tnet_reginfo, login.Gtclientid, login.Devicetoken);
            }
            if (cu == null)
            {
                Alert("创建聊天账号失败");
                return null;
            }
            //驾驶证是否上传
            TnetDriveLicLog driveLicLog = db.TnetDriveLicLogSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Status == 1).FirstOrDefault();
            int driverstatus = driveLicLog == null ? 0 : 1;

            return new LoginInfoDto
            {
                Nodeid = tnet_reginfo.Nodeid,
                Nodecode = tnet_reginfo.Nodecode,
                Nodename = tnet_reginfo.Nodename,
                Nickname = cu.Nickname,
                Mobileno = tnet_reginfo.Mobileno,
                Email = tnet_reginfo.Email,
                Isconfrmed = tnet_reginfo.Isconfirmed,
                Pic = tnet_userphoto.Appphoto.StartsWith("http") ? tnet_userphoto.Appphoto : AppConfig.Userphoto + tnet_userphoto.Appphoto,
                MaxNotPwd = tnet_reginfo.Cashalert,
                IsPayPwdNull = string.IsNullOrEmpty(tnet_reginfo.UserpwdBak) ? true : false,
                NewMailNum = newCount,
                NotPwdPayType = tnet_reginfo.Photourl,
                IsValidfriend = cu.IsValidfriend,
                IsSysNotice = cu.IsSysNotice,
                IsNoticeDetail = cu.IsNoticeDetail,
                Token = cu.Token,
                Personalsign = cu.Personalsign,
                Showrealname = cu.Showrealname,
                DarenStatus = tnet_reginfo.Isenterprise,
                DriverLicenseStatus = driverstatus
            };
        }

        /// <summary>
        /// 验证登录时输入的密码还是验证码
        /// </summary>      
        public static bool IsBase64(string strPwd)
        {
            try
            {
                Convert.FromBase64String(strPwd);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 用户签到
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public SignInfoDto Sign(ReqSign sign)
        {
            //1.判断用户账号是否存在
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == sign.Nodeid).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                //未找到该用户
                Alert("该账号未注册");
                return null;
            }

            //2..检查用户是否被冻结
            TnetLockuser tnet_lockuser = db.TnetLockuserSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Locktype == 1 && c.Locktime < DateTime.Now && c.Unlocktime > DateTime.Now).FirstOrDefault();
            if (tnet_lockuser != null)
            {
                //用户被冻结
                Alert(tnet_lockuser.Remarks);//冻结原因
                return null;
            }

            ////3.p信用户
            //TchatUser tchat_user = db.TchatUserSet.Where(c => c.Nodeid == sign.Nodeid).FirstOrDefault();
            //if (tchat_user == null)
            //{
            //    Alert("该用户未注册P信");
            //    return null;
            //}

            ////保存个推信息
            //TnetReginfoExt ext = db.TnetReginfoExtSet.FirstOrDefault(c => c.Nodeid == sign.Nodeid);
            //if (ext != null)
            //{
            //    if (ext.Gtclientid != tchat_user.Gtclientid || ext.Devicetoken != tchat_user.Devicetoken)
            //    {
            //        ext.Gtclientid = tchat_user.Gtclientid;
            //        ext.Devicetoken = tchat_user.Devicetoken;
            //    }
            //}
            //else
            //{
            //    //db.TnetReginfoExtSet.Add(new TnetReginfoExt { Gtclientid = sign.Gtclientid, Devicetoken = sign.GtToken });
            //    TnetReginfoExt tnet_reginfo_ext = new TnetReginfoExt();
            //    tnet_reginfo_ext.Nodeid = sign.Nodeid;
            //    tnet_reginfo_ext.Createtime = DateTime.Now;
            //    tnet_reginfo_ext.Gtclientid = tchat_user.Gtclientid;
            //    tnet_reginfo_ext.Devicetoken = tchat_user.Devicetoken;
            //    tnet_reginfo_ext.Token = tchat_user.Token;
            //    db.TnetReginfoExtSet.Add(tnet_reginfo_ext);
            //}

            //3.获取头像
            TnetUserphoto tnet_userphoto = db.TnetUserphotoSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid).FirstOrDefault();
            if (tnet_userphoto == null)
            {
                //给他赋值个默认值
                tnet_userphoto = new TnetUserphoto { Appphoto = "/userphoto/noempty.png" };
            }

            //4..记录日志TUE_LOGIN_HIS
            TueLoginHis tue_login_his = new TueLoginHis();
            tue_login_his.Nodeid = tnet_reginfo.Nodeid;
            tue_login_his.Typeid = 3;//签到
            tue_login_his.Clientid = sign.Clientid;//客户端类型
            tue_login_his.Version = sign.Version;//版本号
            tue_login_his.Sid = sign.Sid;//APPID
            tue_login_his.Token = "";//手机的唯一标记
            tue_login_his.Accesskeyid = "";//系统分配
            tue_login_his.Timestamp = sign.Tm;//时间戳
            tue_login_his.Signaturenonce = "";//guid
            tue_login_his.Signature = sign.Sign;//签名结果
            tue_login_his.Longitude = sign.Longitude;//经度
            tue_login_his.Latitude = sign.Latitude;//维度
            tue_login_his.Createtime = DateTime.Now;//登录时间
            tue_login_his.Remarks = "签到";

            db.TueLoginHisSet.Add(tue_login_his);

            //5.更新数据到数据库
            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("签到失败");
                return null;
            }

            //6.获取未读消息数量
            var tue_message = db.TueMessageSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Status == 0).ToList();

            Alert("签到成功", 1);
            TchatUser cu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == tnet_reginfo.Nodeid);
            var nickname = cu == null ? "" : cu.Nickname;
            return new SignInfoDto
            {
                Nodeid = tnet_reginfo.Nodeid,
                Nodecode = tnet_reginfo.Nodecode,
                Nodename = tnet_reginfo.Nodename,
                Nickname = nickname,
                Mobileno = tnet_reginfo.Mobileno,
                Email = tnet_reginfo.Email,
                Isconfrmed = tnet_reginfo.Isconfirmed,
                Pic = tnet_userphoto.Appphoto.StartsWith("http") ? tnet_userphoto.Appphoto : AppConfig.Userphoto + tnet_userphoto.Appphoto,
                MaxNotPwd = tnet_reginfo.Cashalert,
                IsPayPwdNull = string.IsNullOrEmpty(tnet_reginfo.UserpwdBak) ? true : false,
                NewMailNum = tue_message.Count,
                NotPwdPayType = tnet_reginfo.Photourl
            };
        }
        #endregion

        #region 我页面的信息
        /// <summary>
        /// 我页面获得基本信息
        /// </summary>
        /// <param name="my"></param>
        /// <returns></returns>
        public LoginInfoDto GetMy(Reqbase my)
        {
            //1.判断用户账号是否存在
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == my.Nodeid).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                //未找到该用户
                Alert("该账号未注册");
                return null;
            }

            //2..检查用户是否被冻结
            TnetLockuser tnet_lockuser = db.TnetLockuserSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Locktype == 1 && c.Locktime < DateTime.Now && c.Unlocktime > DateTime.Now).FirstOrDefault();
            if (tnet_lockuser != null)
            {
                //用户被冻结
                Alert(tnet_lockuser.Remarks);//冻结原因
                return null;
            }

            //3.获取头像
            TnetUserphoto tnet_userphoto = db.TnetUserphotoSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid).FirstOrDefault();
            if (tnet_userphoto == null)
            {
                //给他赋值个默认值
                tnet_userphoto = new TnetUserphoto { Appphoto = "userphoto/noempty.png" };
            }

            TchatUser cu = db.TchatUserSet.FirstOrDefault(c => c.Nodeid == tnet_reginfo.Nodeid);
            if (cu == null)
            {
                //注册融云账号
                ChatFacade facade = new ChatFacade();
                facade.RegUser(tnet_reginfo, "", "");
            }

            var nickname = cu == null ? "" : cu.Nickname;
            int IsValidfriend = cu == null ? 1 : cu.IsValidfriend;
            int IsSysNotice = cu == null ? 1 : cu.IsSysNotice;
            int IsNoticeDetail = cu == null ? 1 : cu.IsNoticeDetail;

            //驾驶证是否上传
            TnetDriveLicLog driveLicLog = db.TnetDriveLicLogSet.Where(c => c.Nodeid == tnet_reginfo.Nodeid && c.Status == 1).FirstOrDefault();
            int driverstatus = driveLicLog == null ? 0 : 1;

            return new LoginInfoDto
            {
                Nodeid = tnet_reginfo.Nodeid,
                Nodecode = tnet_reginfo.Nodecode,
                Nodename = tnet_reginfo.Nodename,
                Nickname = nickname,
                Mobileno = tnet_reginfo.Mobileno,
                Email = tnet_reginfo.Email,
                Isconfrmed = tnet_reginfo.Isconfirmed,
                Pic = tnet_userphoto.Appphoto.StartsWith("http") ? tnet_userphoto.Appphoto : AppConfig.Userphoto + tnet_userphoto.Appphoto,
                MaxNotPwd = tnet_reginfo.Cashalert,
                IsPayPwdNull = string.IsNullOrEmpty(tnet_reginfo.UserpwdBak) ? true : false,
                NewMailNum = 0,
                NotPwdPayType = tnet_reginfo.Photourl,
                IsValidfriend = IsValidfriend,
                IsSysNotice = IsSysNotice,
                IsNoticeDetail = IsNoticeDetail,
                Token = cu.Token,
                Personalsign = cu.Personalsign,
                Showrealname = cu.Showrealname,
                DarenStatus = tnet_reginfo.Isenterprise,
                DriverLicenseStatus = driverstatus
            };
        }
        #endregion

        #region 修改用户的常规信息
        /// <summary>
        /// 修改用户常规信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool EditUserInfo(ReqEditUserInfo userinfo)
        {
            //1.验证nodeid
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == userinfo.Nodeid).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                Alert("该用户未注册");
                return false;
            }

            string oldData = "";
            TcsChangeHis tcs_change_his = new TcsChangeHis();

            if (userinfo.Type == 2 || userinfo.Type == 4 || userinfo.Type == 5 || userinfo.Type == 6)
            {
                //邮箱
                if (userinfo.Type == 2)
                {
                    var Email = db.TnetReginfoSet.Where(c => c.Email == userinfo.Info).FirstOrDefault();
                    if (Email == null)
                    {
                        //没有邮箱
                        tcs_change_his.Typeid = 102;
                        oldData = "";
                        tcs_change_his.Remarks = "修改邮箱";
                        tnet_reginfo.Email = userinfo.Info;
                        tcs_change_his.Newdata = userinfo.Info;
                    }
                    else
                    {
                        //有邮箱
                        Alert("邮箱已存在");
                        return false;
                    }
                }
                //登录密码
                else if (userinfo.Type == 5)
                {
                    tcs_change_his.Typeid = 105;
                    oldData = tnet_reginfo.Userpwd;
                    tcs_change_his.Remarks = "修改登录密码";
                    tnet_reginfo.Userpwd = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(userinfo.Info)));
                    tcs_change_his.Newdata = tnet_reginfo.Userpwd;
                }
                //支付密码
                else if (userinfo.Type == 4)
                {
                    tcs_change_his.Typeid = 104;
                    oldData = tnet_reginfo.UserpwdBak;
                    tcs_change_his.Remarks = "修改支付密码";
                    tnet_reginfo.UserpwdBak = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(userinfo.Info)));
                    tcs_change_his.Newdata = tnet_reginfo.UserpwdBak;
                }
                else if (userinfo.Type == 6)
                {
                    //免密支付金额
                    double maxnotfee = 0;
                    double.TryParse(userinfo.Info, out maxnotfee);

                    tcs_change_his.Typeid = 106;
                    oldData = tnet_reginfo.Cashalert.ToString();
                    tcs_change_his.Newdata = maxnotfee.ToString();
                    tcs_change_his.Remarks = "修改最高免密支付金额";
                    tnet_reginfo.Cashalert = maxnotfee;
                }

                //2.获取最近一条合法的短信验证码
                if (userinfo.Type != 6 || (userinfo.Type == 6 && double.Parse(userinfo.Info.ToString()) > 0))
                {
                    if (AppConfig.IsUseSms)//判断是否是测试环境
                    {
                        var tsso_regcode = db.TssoRegcodeSet.Where(c => c.Regcode == tnet_reginfo.Mobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                        if (tsso_regcode == null)
                        {
                            //获取短信验证码失败
                            Alert("获取验证码失败");
                            return false;
                        }
                        //3.判断用户的短信验证码是否正确

                        if (userinfo.SmsCode.Trim() != tsso_regcode.Authcode.Trim())
                        {
                            //验证码不正确
                            Alert("验证码有误");
                            return false;
                        }
                        //将验证表的验证状态由0改为1；
                        tsso_regcode.Status = 1;
                    }
                }
            }
            else if (userinfo.Type == 1)//姓名
            {
                tcs_change_his.Typeid = 101;
                oldData = tnet_reginfo.Nodename;
                tcs_change_his.Newdata = userinfo.Info;
                tcs_change_his.Remarks = "修改姓名";
                tnet_reginfo.Nodename = userinfo.Info;
            }
            else if (userinfo.Type == 3)//头像URL
            {
                if (!UploadHeadPortrait(userinfo.Info, userinfo.Nodeid))
                {
                    Alert("修改失败");
                    return false;
                }
            }


            //记录修改历史表
            //TcsChangeHis tcs_change_his = new TcsChangeHis();
            if (userinfo.Type != 3)
            {
                tcs_change_his.Nodeid = userinfo.Nodeid;
                tcs_change_his.Olddata = oldData;
                tcs_change_his.Fee = 0;
                tcs_change_his.Createtime = DateTime.Now;
                tcs_change_his.Opnodeid = tnet_reginfo.Nodeid;
                tcs_change_his.Note = tcs_change_his.Remarks;
                db.TcsChangeHisSet.Add(tcs_change_his);
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    Alert("修改失败");
                    return false;
                }
            }

            Alert("修改成功", 1);

            //同步用户信息
            db.SyncChatUserFull(userinfo.Nodeid);

            PxinCache.RemoveRegInfo(tnet_reginfo.Nodeid);
            return true;
        }
        /// <summary>
        /// 修改用户昵称和个性签名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateUserNickAndSign(UserNickAndSignReq req)
        {
            //var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            //if (tnet_reginfo == null)
            //{
            //    Alert("该用户未注册");
            //    return false;
            //}
            TchatUser chatUser = db.TchatUserSet.First(c => c.Nodeid == req.Nodeid);
            if (chatUser == null)
            {
                Alert("查找用户失败");
                return false;
            }
            chatUser.Nickname = req.NickName;
            chatUser.Personalsign = req.PersonalSign;

            //TcsChangeHis tcs_change_his = new TcsChangeHis();
            //tcs_change_his.Typeid = 101;
            //tcs_change_his.Olddata = tnet_reginfo.Nodename;
            //tnet_reginfo.Nodename = req.NickName;
            //tcs_change_his.Newdata = req.NickName;
            //tcs_change_his.Remarks = "修改姓名";
            //tcs_change_his.Nodeid = req.Nodeid;
            //tcs_change_his.Fee = 0;
            //tcs_change_his.Createtime = DateTime.Now;
            //tcs_change_his.Opnodeid = tnet_reginfo.Nodeid;
            //tcs_change_his.Note = tcs_change_his.Remarks;
            //db.TcsChangeHisSet.Add(tcs_change_his);

            //TcsChangeHis changHis = new TcsChangeHis();
            //changHis.Typeid = 107;
            //changHis.Olddata = chatUser.Personalsign;
            //changHis.Newdata = req.PersonalSign;
            //changHis.Remarks = "修改个性签名";
            //changHis.Nodeid = req.Nodeid;
            //changHis.Fee = 0;
            //changHis.Createtime = DateTime.Now;
            //changHis.Opnodeid = tnet_reginfo.Nodeid;
            //changHis.Note = changHis.Remarks;
            //db.TcsChangeHisSet.Add(changHis);

            if (db.SaveChanges() >= 0)
            {
                //同步用户信息
                db.SyncChatUserFull(req.Nodeid);
                Alert("修改成功", 1);
                return true;
            }
            else
            {
                Alert("修改失败");
                return false;
            }
        }
        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="imageTempFullUrl"></param>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public bool UploadHeadPortrait(string imageTempFullUrl, int nodeid)
        {
            string physicsFileName = System.Web.Hosting.HostingEnvironment.MapPath(imageTempFullUrl.Substring(imageTempFullUrl.IndexOf("/images2")));
            string headPortraitName = $"/images2/userphoto/{DateTime.Now.ToString("yyyyMMdd")}/{ nodeid.ToString() + "-" + Guid.NewGuid().ToString()}{Path.GetExtension(imageTempFullUrl)}";
            string physicsHeadPortraitName = System.Web.Hosting.HostingEnvironment.MapPath(headPortraitName);
            var host = HttpContext.Current.Request.Url.Host.Contains("localhost") ? "client.be.sulink.cn" : HttpContext.Current.Request.Url.Host;
            var url = headPortraitName;
            var fullUrl = "http://images2." + host.Substring(host.IndexOf(".") + 1) + url.Substring(url.IndexOf("/userphoto"));
            string oppositeOldHeadPortraitPath = "";
            db.BeginTransaction();
            var userPhoto = db.TnetUserphotoSet.FirstOrDefault(f => f.Nodeid == nodeid);
            TcsChangeHis tcs_change_his = new TcsChangeHis();
            if (userPhoto != null)
            {
                oppositeOldHeadPortraitPath = string.IsNullOrEmpty(userPhoto.Appphoto) ? "" : $"/images2/" + userPhoto.Appphoto.Substring(userPhoto.Appphoto.IndexOf(@"/userphoto") < 0 ? 0 : userPhoto.Appphoto.IndexOf(@"/userphoto"));
                tcs_change_his.Typeid = 104;
                tcs_change_his.Remarks = "修改头像";
                tcs_change_his.Olddata = userPhoto.Appphoto;
                tcs_change_his.Newdata = fullUrl;
                userPhoto.Appphoto = fullUrl;

            }
            else
            {
                userPhoto = new TnetUserphoto()
                {
                    Nodeid = nodeid,
                    Appphoto = fullUrl
                };
                tcs_change_his.Typeid = 104;
                tcs_change_his.Remarks = "新增头像";
                tcs_change_his.Olddata = "";
                tcs_change_his.Newdata = fullUrl;
                db.TnetUserphotoSet.Add(userPhoto);
            }
            tcs_change_his.Nodeid = nodeid;
            tcs_change_his.Fee = 0;
            tcs_change_his.Createtime = DateTime.Now;
            tcs_change_his.Opnodeid = nodeid;
            tcs_change_his.Note = tcs_change_his.Remarks;
            db.TcsChangeHisSet.Add(tcs_change_his);
            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                db.Rollback();
                return false;
            }
            if (!Common.Facade.Helper.CopyFile(physicsFileName, physicsHeadPortraitName))
            {
                db.Rollback();
                log.Info("文件：" + physicsFileName + ";复制到" + physicsHeadPortraitName + "失败");
                return false;
            }
            db.Commit();
            if (!string.IsNullOrEmpty(oppositeOldHeadPortraitPath))
            {
                try
                {
                    File.Delete(System.Web.Hosting.HostingEnvironment.MapPath(oppositeOldHeadPortraitPath));
                }
                catch (Exception) { }
            }
            return true;
        }
        #endregion

        #region 修改登录密码(忘记密码功能)
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ChangePwd(ChangePwdReq req)
        {
            var regInfo = db.TnetReginfoSet.FirstOrDefault(f => f.Nodeid == req.Nodeid);
            if (req.Type == 1)
            {
                if (!CheckPwd(regInfo, req.OldPwd))
                {
                    Alert("原密码输入错误");
                    return false;
                }

                regInfo.Userpwd = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd)));
            }
            else
            {
                if (!CheckPayPwd(regInfo, req.OldPwd))
                {
                    Alert("原支付密码输入错误");
                    return false;
                }

                try
                {
                    var newPayPwd = Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd));
                    Convert.ToInt32(newPayPwd);
                    if (newPayPwd.Length != 6)
                    {
                        throw new Exception("输入的支付密码非六位");
                    }
                }
                catch (Exception)
                {
                    Alert("请输入6位数字密码");
                    return false;
                }

                regInfo.UserpwdBak = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd)));
            }

            TcsChangeHis tcsChangeHis = new TcsChangeHis
            {
                Nodeid = regInfo.Nodeid,
                Typeid = req.Type == 1 ? 105 : 104,
                Olddata = req.Type == 1 ? regInfo.Userpwd : regInfo.UserpwdBak,
                Newdata = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd))),
                Fee = 0,
                Createtime = DateTime.Now,
                Opnodeid = regInfo.Nodeid,
                Note = req.Type == 1 ? "修改登陆密码" : "修改支付密码",
                Remarks = "修改登陆密码"
            };


            db.TcsChangeHisSet.Add(tcsChangeHis);
            if (db.SaveChanges() < 1)
            {
                Alert("修改失败");
                return false;
            }

            PxinCache.RemoveRegInfo(regInfo.Nodeid);
            return true;
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ForgetPwd(ForgetPwdReq req)
        {
            //1.判断该手机号码是否存在(是否已经注册了)
            var regInfo = db.TnetReginfoSet.FirstOrDefault(f => f.Mobileno == req.Mobileno.Trim());
            if (regInfo == null)
            {
                Alert("该手机号码未注册");
                return false;
            }
            string oldData = regInfo.Userpwd;
            if (AppConfig.IsUseSms)
            {
                //2.获取最近一条合法的短信验证码
                var regCode = db.TssoRegcodeSet.Where(c => c.Regcode == req.Mobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                if (regCode == null)
                {
                    //获取短信验证码失败
                    Alert("获取验证码失败");
                    return false;
                }
                //3.判断用户的短信验证码是否正确
                if (req.SmsCode.Trim() != regCode.Authcode.Trim())
                {
                    //验证码不正确
                    Alert("验证码有误");
                    return false;
                }
                //4.将验证表的验证状态由0改为1；
                regCode.Status = 1;
            }
            var newPwd = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd)));
            if (req.Type == 2)//忘记支付密码
            {
                regInfo.UserpwdBak = newPwd;
            }
            else
            {
                regInfo.Userpwd = newPwd;
            }
            //5.记录日志
            TcsChangeHis tcsChangeHis = new TcsChangeHis
            {
                Nodeid = regInfo.Nodeid,
                Typeid = 108,
                Olddata = oldData,
                Newdata = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(req.NewPwd))),
                Fee = 0,
                Createtime = DateTime.Now,
                Opnodeid = regInfo.Nodeid,
                Note = "忘记密码",
                Remarks = "忘记密码"
            };
            db.TcsChangeHisSet.Add(tcsChangeHis);
            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("修改失败");
                return false;
            }
            Alert("修改成功", 1);
            PxinCache.RemoveRegInfo(regInfo.Nodeid);
            return true;
        }
        /// <summary>
        /// 修改用户的登录密码（忘记密码功能）
        /// </summary>
        /// <param name="editLoginPwd"></param>
        /// <returns></returns>
        public bool EditLoginPwd(ReqEditLoginPwd editLoginPwd)
        {
            //1.判断该手机号码是否存在(是否已经注册了)
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Mobileno == editLoginPwd.Mobileno.Trim()).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                Alert("该手机号码未注册");
                return false;
            }
            string oldData = tnet_reginfo.Userpwd;

            if (AppConfig.IsUseSms)
            {
                //2.获取最近一条合法的短信验证码
                var tsso_regcode = db.TssoRegcodeSet.Where(c => c.Regcode == editLoginPwd.Mobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                if (tsso_regcode == null)
                {
                    //获取短信验证码失败
                    Alert("获取验证码失败");
                    return false;
                }
                //3.判断用户的短信验证码是否正确
                if (editLoginPwd.SmsCode.Trim() != tsso_regcode.Authcode.Trim())
                {
                    //验证码不正确
                    Alert("验证码有误");
                    return false;
                }
                //4.将验证表的验证状态由0改为1；
                tsso_regcode.Status = 1;
            }

            tnet_reginfo.Userpwd = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(editLoginPwd.Pwd)));

            //5.记录日志
            TcsChangeHis tcs_change_his = new TcsChangeHis();
            tcs_change_his.Nodeid = tnet_reginfo.Nodeid;
            tcs_change_his.Typeid = 108;
            tcs_change_his.Olddata = oldData;
            tcs_change_his.Newdata = UserPwd.Encode(Encoding.UTF8.GetString(Convert.FromBase64String(editLoginPwd.Pwd)));
            tcs_change_his.Fee = 0;
            tcs_change_his.Createtime = DateTime.Now;
            tcs_change_his.Opnodeid = tnet_reginfo.Nodeid;
            tcs_change_his.Note = "忘记密码";
            tcs_change_his.Remarks = "忘记密码";
            db.TcsChangeHisSet.Add(tcs_change_his);

            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("修改失败");
                return false;
            }
            Alert("修改成功", 1);

            PxinCache.RemoveRegInfo(tnet_reginfo.Nodeid);
            return true;
        }
        #endregion

        #region 修改手机号码
        /// <summary>
        /// 用户修改手机号码
        /// </summary>
        /// <param name="editMobileno"></param>
        /// <returns></returns>
        public bool EditMobileno(ReqEditMobileno editMobileno)
        {
            //1.验证nodeid以及电话号码
            TnetReginfo tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == editMobileno.Nodeid && c.Mobileno == editMobileno.Oldmobileno).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            string oldData = tnet_reginfo.Mobileno;

            //2.判断新手机号码是否已经存在
            var tnet_reginfo2 = db.TnetReginfoSet.Where(c => c.Mobileno == editMobileno.Newmobileno).FirstOrDefault();
            if (tnet_reginfo2 != null)
            {
                Alert("该手机号码已存在");
                return false;
            }
            //3.判断修改的新手机号码不能是同一个
            if (tnet_reginfo.Mobileno == editMobileno.Newmobileno)
            {
                Alert("号码错误");
                return false;
            }

            if (AppConfig.IsUseSms)
            {
                //4.获取旧号码的合法的短信验证码
                var tsso_regcode = db.TssoRegcodeSet.Where(c => c.Regcode == editMobileno.Oldmobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                if (tsso_regcode == null)
                {
                    //获取短信验证码失败
                    Alert("获取验证码失败");
                    return false;
                }
                //5.判断用户旧号码的短信验证码是否正确
                if (editMobileno.Oldsmscode.Trim() != tsso_regcode.Authcode.Trim())
                {
                    //验证码不正确
                    Alert("验证码有误");
                    return false;
                }

                tsso_regcode.Status = 1;  //把已使用的验证码的状态改为1；


                //6.获取新手机号码的合法的短信验证码
                var tsso_regcode2 = db.TssoRegcodeSet.Where(c => c.Regcode == editMobileno.Newmobileno && c.Status == 0 && c.Codetype == 2 && c.Indate >= DateTime.Now).OrderByDescending(c => c.Id).FirstOrDefault();
                if (tsso_regcode2 == null)
                {
                    //获取短信验证码失败
                    Alert("获取验证码失败2");
                    return false;
                }
                //7.判断用户新手机号码的短信验证码是否正确
                if (editMobileno.Newsmscode.Trim() != tsso_regcode2.Authcode.Trim())
                {
                    //验证码不正确
                    Alert("验证码有误2");
                    return false;
                }
                tsso_regcode2.Status = 1;   //把已使用的验证码的状态改为1；
            }
            //8.修改手机号码
            tnet_reginfo.Mobileno = editMobileno.Newmobileno;

            //9.记录日志
            TcsChangeHis tcs_change_his = new TcsChangeHis();
            tcs_change_his.Nodeid = editMobileno.Nodeid;
            tcs_change_his.Typeid = 107;
            tcs_change_his.Olddata = oldData;
            tcs_change_his.Newdata = editMobileno.Newmobileno;
            tcs_change_his.Fee = 0;
            tcs_change_his.Createtime = DateTime.Now;
            tcs_change_his.Opnodeid = editMobileno.Nodeid;
            tcs_change_his.Note = "修改手机号码";
            tcs_change_his.Remarks = "修改手机号码";
            db.TcsChangeHisSet.Add(tcs_change_his);

            //10.提交
            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("修改失败");
                return false;
            }
            Alert("修改成功", 1);

            PxinCache.RemoveRegInfo(tnet_reginfo.Nodeid);
            return true;
        }
        #endregion

        #region 绑定第三方账号
        /// <summary>
        /// 用户绑定第三方账号
        /// </summary>
        /// <param name="createUserOpen"></param>
        /// <returns></returns>
        public bool CreateUserOpen(ReqCreateUserOpen createUserOpen)
        {
            var tnet_reginfo = PxinCache.GetRegInfo(createUserOpen.Nodeid);
            if (tnet_reginfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }

            //一个人只能绑定一个同类型的第三方账户
            var tsso_openuser = db.TssoOpenUserSet.Where(c => c.Nodeid == createUserOpen.Nodeid && c.Opentype == createUserOpen.Opentype).FirstOrDefault();
            if (tsso_openuser != null)
            {
                Alert("您已经绑定过此类账号了", -2);
                return false;
            }
            //进一步判断该第三方账户是否被其他人绑定了
            var tsso_openuser2 = db.TssoOpenUserSet.Where(c => c.Opentype == createUserOpen.Opentype && c.Openid == createUserOpen.Openid).FirstOrDefault();
            if (tsso_openuser2 != null)
            {
                Alert("该账户已被其他人绑定");
                return false;
            }

            //绑定第三方账号
            TssoOpenUser tsso_open_user = new TssoOpenUser();
            tsso_open_user.Nodeid = createUserOpen.Nodeid;
            tsso_open_user.Opentype = createUserOpen.Opentype;
            tsso_open_user.Openid = createUserOpen.Openid;
            tsso_open_user.Createtime = DateTime.Now;
            tsso_open_user.Remarks = "绑定第三方账号";
            db.TssoOpenUserSet.Add(tsso_open_user);
            if (db.SaveChanges() < 0)
            {
                log.Info(db.Message);
                Alert("操作失败");
                return false;
            }

            Alert("操作成功", 1);
            return true;
        }
        #endregion

        #region 解除绑定第三方账号
        /// <summary>
        /// 解除第三方账号
        /// </summary>
        /// <param name="delUserOpen"></param>
        /// <returns></returns>
        public bool DeleteUserOpen(ReqDeleteUserOpen delUserOpen)
        {
            var tnet_reginfo = PxinCache.GetRegInfo(delUserOpen.Nodeid);
            if (tnet_reginfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }

            var tsso_open_user = db.TssoOpenUserSet.Where(c => c.Nodeid == delUserOpen.Nodeid && c.Opentype == delUserOpen.Opentype).FirstOrDefault();
            if (tsso_open_user == null)
            {
                Alert("您尚未绑定第三方账号");
                return false;
            }
            db.TssoOpenUserSet.Remove(tsso_open_user);
            if (db.SaveChanges() < 0)
            {
                log.Info(db.Message);
                Alert("操作失败");
                return false;
            }
            Alert("操作成功", 1);
            return true;
        }
        #endregion

        #region 通讯录用户是否注册
        /// <summary>
        /// 通讯录用户是否注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<IsRegDto> IsReg(ReqIsReg req)
        {
            string[] Mobilenos = req.Mobilenos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var query = from user in db.TnetReginfoSet.Where(a => Mobilenos.Contains(a.Mobileno))
                        join pho in db.TnetUserphotoSet on user.Nodeid equals pho.Nodeid into pho_join
                        from pho in pho_join.DefaultIfEmpty()
                        join chat in db.TchatUserSet on user.Nodeid equals chat.Nodeid into chat_join
                        from chat in chat_join.DefaultIfEmpty()
                        select new IsRegDto
                        {
                            AppPhoto = pho.Appphoto,
                            IsReg = 1,
                            Mobileno = user.Mobileno,
                            NodeCode = user.Nodecode,
                            NodeId = user.Nodeid,
                            NodeName = user.Nodename,
                            Token = chat.Token
                        };

            List<IsRegDto> RegDtos = query.ToList();
            Mobilenos = Mobilenos.Except(RegDtos.Select(a => a.Mobileno).ToArray()).ToArray();//取差集
            foreach (var item in Mobilenos)
            {
                IsRegDto regDto = new IsRegDto()
                {
                    AppPhoto = "",
                    IsReg = 0,
                    Mobileno = item,
                    NodeCode = "",
                    NodeId = 0,
                    NodeName = "",
                    Token = ""
                };
                RegDtos.Add(regDto);

            }
            return RegDtos;
        }

        #endregion

        #region 邀请通讯录好友注册
        /// <summary> 
        /// 邀请通讯录好友注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool InviteReg(ReqIsReg req)
        {
            DateTime curTime = DateTime.Now.Date;
            string[] Mobilenos = req.Mobilenos.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (Mobilenos.Length <= 0)
            {
                Alert("请选择通讯录好友邀请");
                return false;
            }
            //2.判断邀请的好友是否在10天内邀请过
            DateTime validTime = DateTime.Now.AddDays(-10);
            List<TnetInvitehis> tnetInvitehis = new List<TnetInvitehis>();
            foreach (var item in Mobilenos)
            {
                TnetInvitehis invitehis = db.TnetInvitehisSet.Where(a => a.Createtime >= validTime && a.Mobileno == item && a.Pnodeid == req.Nodeid).FirstOrDefault();
                if (invitehis != null)
                {
                    continue;
                }
                TnetInvitehis his = new TnetInvitehis()
                {
                    Mobileno = item,
                    Createtime = DateTime.Now,
                    Pnodeid = req.Nodeid,
                    Sid = req.Sid,
                    Status = 0,
                    Transferid = 0
                };
                tnetInvitehis.Add(his);
            }
            //2.写入邀请历史
            db.TnetInvitehisSet.AddRange(tnetInvitehis);
            if (db.SaveChanges() <= 0)
            {
                log.Info("添加邀请历史失败" + db.Message);
                Alert("添加邀请历史失败");
                return false;
            }
            //3.发送短信
            SmsFacade smsFacade = new SmsFacade();
            foreach (var item in tnetInvitehis.Select(a => a.Mobileno))
            {
                bool smsResult = smsFacade.SendSms(0, 0, item, ConfigurationManager.AppSettings["InviteSmsContent"], req.Sid);
            }

            return true;
        }
        #endregion

        #region 用户协议

        /// <summary>
        /// 获取是否已经同意协议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool GetIsAgree(AgreementReq req)
        {
            var version = db.TnetUserProtocalSet.Where(c => c.Type == req.Type && c.Status == 1).FirstOrDefault();
            if (version == null)
            {
                Alert("协议不存在");
                return false;
            }

            var agree = db.TnetUserAgreementSet.Where(c => c.Nodeid == req.Nodeid && c.Type == req.Type && c.Version == version.Version).FirstOrDefault() == null ? false : true;
            if (!agree)
            {
                Alert("未同意协议", -2);
            }
            return agree;
        }
        private readonly Cache _objCache = HttpRuntime.Cache;
        /// <summary>
        /// 设置同意协议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool AgreeAgreement(AgreementReq req)
        {
            var version = db.TnetUserProtocalSet.Where(c => c.Type == req.Type && c.Status == 1).FirstOrDefault();
            if (version == null)
            {
                Alert("协议不存在");
                return false;
            }

            var agree = db.TnetUserAgreementSet.Where(c => c.Nodeid == req.Nodeid && c.Type == req.Type && c.Version == version.Version).FirstOrDefault() == null ? false : true;
            if (!agree)
            {
                AgreeAgreement_Pro(req.Nodeid, req.Type, version.Version);
                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    return false;
                }
            }
            if (req.Type == 20003) {
                _objCache.Insert($"AuctionProtocol_{req.Nodeid}",1);
            }
            return true;
        }

        private void AgreeAgreement_Pro(int nodeid, int type, int version)
        {
            var agreement = new TnetUserAgreement
            {
                Agreed = 1,
                Createtime = DateTime.Now,
                Nodeid = nodeid,
                Type = type,
                Version = version
            };
            db.TnetUserAgreementSet.Add(agreement);
        }

        #endregion

        /// <summary>
        /// 设置加我为好友时是否需要验证
        /// </summary>
        public bool EditIsValidFriend(IsValidDto req)
        {
            var entity = db.TchatUserSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (entity == null)
            {
                Alert("用户参数错误");
                return false;
            }
            entity.IsValidfriend = req.IsValid;
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 通知设置：系统动态通知，1开，0关
        /// </summary>
        public bool EditIsSysNotice(IsOpenDto req)
        {
            var entity = db.TchatUserSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (entity == null)
            {
                Alert("用户参数错误");
                return false;
            }
            entity.IsSysNotice = req.IsOpen;
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 通知设置：通知显示详情，1是，0否
        /// </summary>
        public bool EditIsNoticeDetail(IsOpenDto req)
        {
            var entity = db.TchatUserSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (entity == null)
            {
                Alert("用户参数错误");
                return false;
            }
            entity.IsNoticeDetail = req.IsOpen;
            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 意见反馈
        /// </summary>
        public bool CreateFeedback(FeedbackReq req)
        {
            TappFeedback Tapp_Feedback = new TappFeedback();
            Tapp_Feedback.Nodeid = req.Nodeid;
            Tapp_Feedback.Message = req.Message;
            Tapp_Feedback.Image = req.ImageUrl;
            Tapp_Feedback.Client = req.Client;
            Tapp_Feedback.Version = req.Version;
            Tapp_Feedback.Createtime = DateTime.Now;
            Tapp_Feedback.Remark = "";
            Tapp_Feedback.Title = req.Title;
            Tapp_Feedback.Opnodeid = null;
            Tapp_Feedback.Status = 0;
            Tapp_Feedback.Note = "";
            Tapp_Feedback.Mobile = req.Mobile;
            db.TappFeedbackSet.Add(Tapp_Feedback);

            if (db.SaveChanges() <= 0)
            {
                log.Info(db.Message);
                Alert("操作失败");
                return false;
            }
            return true;
        }

        #region PCN账号绑定
        /// <summary>
        /// PCN账号绑定
        /// </summary>
        public bool BindPcnAcount(BindPcnAcountReq req)
        {
            string pwd = Convert.ToBase64String(Encoding.Default.GetBytes(req.Pwd));
            var myRet = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/user/Login", new { Nodecode = req.NodeCode, PassWord = pwd, Version = "1.0.0", Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid });
            var ret = JsonConvert.DeserializeObject<Respbase<PcnAcountInfoDto>>(myRet);
            if (ret.Result <= 0)
            {
                log.Info("BindPcnAcount失败，" + myRet);
                Alert(ret.Message);
                return false;
            }
            else
            {
                if (!GetPcnAuthStatusByCode(new PcnAuthStatusByCodeReq { NodeCode = req.NodeCode, Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid }))
                {
                    return false;
                }
                var isBind = CreateUserOpen(new ReqCreateUserOpen { Client = req.Client, Nodeid = req.Nodeid, Sid = req.Sid, Sign = req.Sign, Tm = req.Tm, Openid = ret.Data.NodeCode, Opentype = 4 });
                return isBind;
            }
        }

        /// <summary>
        /// PCN账号认证状态
        /// </summary>
        public bool GetPcnAuthStatusByCode(PcnAuthStatusByCodeReq req)
        {
            var myRet = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.PCNDomainUrl}/api/auth/GetAuthStatusByCodeFromPXin", new PcnAuthStatusByCodeReq { NodeCode = req.NodeCode, Client = req.Client, Tm = req.Tm, Sign = req.Sign, Sid = req.Sid, Nodeid = req.Nodeid });
            var ret = JsonConvert.DeserializeObject<Respbase>(myRet);
            if (ret.Result <= 0)
            {
                log.Info("GetPcnAuthStatusByCode，" + myRet);
                Alert("此账号未认证");
                return false;
            }
            //Alert("此账号已经实名认证");
            return true;
        }
        #endregion

        #region 消息中心
        /// <summary>
        /// 用户的站内信(消息中心)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<MailDto> GetMails(ReqMail req)
        {
            var query = from tue_message in db.TueMessageSet
                        where tue_message.Nodeid == req.Nodeid && tue_message.Typeid == 1 && tue_message.Status != -1
                        orderby tue_message.Createtime descending
                        select new MailDto
                        {
                            Hisid = tue_message.Hisid,
                            Title = tue_message.Title,
                            Content = tue_message.Content,
                            Status = tue_message.Status,
                            Createtime = tue_message.Createtime,
                            ClickUrl= tue_message.Jumpurl
                        };
            List<MailDto> Maillist = query.ToPagedList(req.PageNum, req.PageSize).ToList();
            foreach (var item in Maillist)
            {
                item.ClickUrl = item.ClickUrl.Replace("{sign}", GetQueryString(req.Nodeid));
            }

            return Maillist;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteMail(ReqDeleteMail req)
        {
            var mail = db.TueMessageSet.Where(c => c.Hisid == req.HisId).FirstOrDefault();
            if (mail == null)
            {
                Alert("消息不存在或已删除");
                return true;
            }

            db.TueMessageSet.Remove(mail);
            if (db.SaveChanges() <= 0)
            {
                Alert("删除失败");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 用户读站内信
        /// </summary>
        /// <param name="readMail"></param>
        /// <returns></returns>
        public bool ReadMail(ReqReadMail readMail)
        {
            //1.判断用户账号是否存在
            var tnet_reginfo = HttpContext.Current.GetRegInfo();// db.TnetReginfoSet.Where(c => c.Nodeid == readMail.Nodeid).FirstOrDefault();
            if (tnet_reginfo == null)
            {
                //未找到该用户
                Alert("获取用户基本信息失败");
                return false;
            }
            //2.判断该用户是否有站内信
            string[] subHisid = readMail.HisId.Split(',');
            for (int i = 0; i < subHisid.Length; i++)
            {
                int.TryParse(subHisid[i], out int hisid);
                if (hisid == -1)
                {
                    var tue_message = db.TueMessageSet.Where(c => c.Nodeid == readMail.Nodeid && c.Status == 0).ToArray();
                    Array.ForEach(tue_message, item => item.Status = 1);
                }
                else
                {
                    //单独一个一个处理
                    var tue_message2 = db.TueMessageSet.Where(c => c.Nodeid == readMail.Nodeid && c.Hisid == hisid && c.Status == 0).FirstOrDefault();
                    if (tue_message2 == null)
                    {
                        Alert("获取站内信失败");
                        return false;
                    }
                    tue_message2.Status = 1;
                }
            }
            db.SaveChanges();
            Alert("操作成功", 1);
            return true;
        }

        /// <summary>
        /// 是否存在未读消息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IsMailDto IsMails(Reqbase req)
        {
            IsMailDto dto = new IsMailDto();
            dto.Status= db.TueMessageSet.Where(c => c.Nodeid == req.Nodeid && c.Status == 0).Count() > 0 ? 1 : 0;
            return dto;
        }

        private string GetQueryString(int nodeid)
        {
            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            int sid = 81127;
            string sign = Helper.GetSign(nodeid, sid, tm, CommonConfig.ApiAuthString);
            return $"nodeid={nodeid}&sid={sid}&tm={tm}&sign={sign}";
        }

        #endregion
    }
}
