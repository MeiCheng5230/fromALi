using Common.Facade;
using Common.Facade.Models;
using MvcPaging;
using PXin.DB;
using PXin.Facade.CommonService;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 发票专区
    /// </summary>
    public class InvioceFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 获取首页开票统计
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public InvioceStatisticsDto GetInvioceStatistics(Reqbase req)
        {
            var result = GetAvailableAmount(req.Nodeid);

            var apply = db.TpxinInvoiceLimitSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();

            var user = HttpContext.Current.GetRegInfo();

            return new InvioceStatisticsDto { 
                Amount= result.Item1,
                AlreadyAmount= result.Item2,
                Status = apply==null?0: apply.Status,
                Name= user?.Nodename
            };
        }

        /// <summary>
        /// 获取可申请列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<InvioceMayApplyDto> GetMayApplyInvioceHis(PageBase req)
        {
            var s =string.Join(",", db.TblcCentcardInvoiceSet.Where(c => c.Nodeid == req.Nodeid).Select(c => c.Idno.ToString()).ToArray());
            var query = from tch in db.TblcCentcardHisSet.Where(c => c.Nodeid == req.Nodeid && c.Typeid == 0)
                        join tc in db.TblcCentcardSet on tch.Idno equals tc.Idno into tcdata
                        from tcd in tcdata.DefaultIfEmpty()
                        where !s.Contains(tch.Idno.ToString())
                        select new InvioceMayApplyDto
                        {
                            IdNo=tch.Idno,
                            Amount = (decimal)tcd.Amount,
                            CardNum = tcd.Cardno,
                            ShowName = "充值码（SVC）",
                            CreateTime = tch.Createtime
                        };
            return query.OrderByDescending(c=>c.CreateTime).ToPagedList(req.PageNum, req.PageSize).ToList();
        }

        /// <summary>
        /// 获取已申请列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<InvioceAlreadyApplyDto> GetAlreadyApplyInvioceHis(PageBase req)
        {
            var query=(from tci in db.TblcCentcardInvoiceSet
                      join tch in db.TblcCentcardHisSet.Where(c=>c.Nodeid==req.Nodeid && c.Typeid==0) on tci.Idno equals tch.Idno
                      join tc in db.TblcCentcardSet on tch.Idno equals tc.Idno into tcdata
                      from tcd in tcdata.DefaultIfEmpty()
                      orderby tci.Createtime descending
                      select new InvioceAlreadyApplyDto { 
                        Amount=(decimal)tcd.Amount,
                        Infoid=tci.Infoid,
                        CardNum =tcd.Cardno,
                        Head= tci.Head,
                        ShowName= "充值码（SVC）",
                        Status=tci.Status,
                        TaxNum= tci.Code,
                        Typeid=tci.Typeid,
                        IdNo=tci.Infoid,
                        Expressno=tci.Expressno,
                        IsPerson=tci.Isperson
                      }).ToPagedList(req.PageNum, req.PageSize).ToList();
            return query;
        }

        /// <summary>
        /// 提交增票资质申请
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ApplyInvioceQualifica(ApplyInvioceQualificaReq req)
        {
            var data = db.TpxinInvoiceLimitSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (data != null)
            {
                if (data.Status == 1)
                {
                    Alert("审核中请勿重复提交");
                    return false;
                }
                if(data.Status == 2)
                {
                    Alert("已经通过审核不可修改");
                    return false;
                }

                data.Address = req.Address;
                data.Bank = req.Bank;
                data.Cardno = req.Cardno;
                data.Company = req.Company;
                data.Mobile = req.Mobile;
                data.Status = 1;
                data.Taxnum = req.Taxnum;
                data.Note = "";
            }
            else
            {
                db.TpxinInvoiceLimitSet.Add(new TpxinInvoiceLimit
                {
                    Address = req.Address,
                    Bank = req.Bank,
                    Cardno = req.Cardno,
                    Company = req.Company,
                    Createtime = DateTime.Now,
                    Mobile = req.Mobile,
                    Nodeid = req.Nodeid,
                    Status = 1,
                    Taxnum = req.Taxnum
                });
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }


            return true;
        }

        /// <summary>
        /// 获取增票资质
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public InvioceQualificaDto GetInvioceQualifica(Reqbase req)
        {
            WriteInvioce_Pro(1);
            var data = db.TpxinInvoiceLimitSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (data == null)
            {
                Alert("未申请",1);
                return null;
            }

            return new InvioceQualificaDto
            {
                Address = data.Address,
                Bank = data.Bank,
                Cardno = data.Cardno,
                Company = data.Company,
                Mobile = data.Mobile,
                Taxnum = data.Taxnum,
                Status = data.Status,
                Note=data.Note
            };
        }

        /// <summary>
        /// 开票申请
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool ApplyWriteInvioce(ApplyWriteInvioceReq req)
        {
            if (req.Typeid != 1 && req.Typeid != 2)
            {
                Alert("参数错误");
                return false;
            }
            //if (req.Typeid == 1&&!IsEmail(req.Email))
            //{
            //    Alert("邮箱格式错误");
            //    return false;
            //}

            var list = Array.ConvertAll(req.IdNo.Split(','), int.Parse);
            var sumprice = 0M;
            TpxinInvoiceLimit limit = new TpxinInvoiceLimit(); 
            if (req.Typeid == 2)
            {
                limit = db.TpxinInvoiceLimitSet.Where(c => c.Nodeid == req.Nodeid && c.Status == 2).FirstOrDefault();
                if (limit == null)
                {
                    Alert("只有增票资质通过审核才能申请此类型发票");
                    return false;
                }
            }
            var result = GetAvailableAmount(req.Nodeid);

            db.BeginTransaction();
            foreach (var item in list)
            {
                var cardhis = db.TblcCentcardHisSet.Where(c => c.Nodeid == req.Nodeid && c.Typeid == 0 && c.Idno == item).FirstOrDefault();
                if (cardhis == null)
                {
                    Alert("只能操作自己的充值卡");
                    db.Rollback();
                    return false;
                }
                var card = db.TblcCentcardSet.Where(c => c.Idno == item).FirstOrDefault();

                var data = db.TblcCentcardInvoiceSet.Where(c => c.Idno == card.Idno).Count();
                if (data > 0)
                {
                    Alert("已经开票的卡不能再次操作");
                    db.Rollback();
                    return false;
                }
                sumprice += (decimal)card.Amount;

                var Address = "";
                var Code = "";
                var Head = "";
                var status = 1;

                if (req.Typeid == 1)
                {
                    //Address = req.Email;
                    //Code = req.Code;
                    //Head = req.Head;
                    //status = 2;
                    if (!WriteInvioce_Pro(req.Nodeid))
                    {
                        Alert("电子发票开具失败");
                        db.Rollback();
                        return false;
                    }

                    Address = req.Address.ToString();
                    Code = req.Code;
                    Head = req.Head;
                    status = 1;
                }
                else
                {
                    Address = req.Address.ToString();
                    Code = limit.Taxnum;
                    Head = limit.Company;
                    status = 1;
                }


                db.TblcCentcardInvoiceSet.Add(new TblcCentcardInvoice
                {
                    Address = Address,
                    Code = Code,
                    Createtime = DateTime.Now,
                    Head = Head,
                    Idno = card.Idno,
                    Isperson = req.Isperson,
                    Modifytime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Status = status,
                    Typeid = req.Typeid
                });

                if (db.SaveChanges() <= 0)
                {
                    Alert("操作失败");
                    db.Rollback();
                    return false;
                }
            }

            if (sumprice> result.Item1)
            {
                Alert("开票金额不能超过当前可开票金额");
                db.Rollback();
                return false;
            }

            db.Commit();
            return true;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="req"></param>
        public bool SendEmail(SendEmailReq req)
        {
            if (!IsEmail(req.Email))
            {
                Alert("请输入正确的邮箱");
                return false;
            }

            var path = System.Web.Hosting.HostingEnvironment.MapPath("/images2" + "http://images2.be.sulink.cn/daren/20191120/0ec6e55e-4b2a-4ace-8682-a71d877a416b.jpg".Replace(AppConfig.ImageBaseUrl, ""));
            SendEmail(req.Email, "相信发票", "这是您申请的电子发票，请在附件查看", "相信官方", path);

            return true;
        }

        /// <summary>
        /// 获取电子发票详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public InvioceDetailDto GetElectronicInvioceDetail(InvioceDetailReq req)
        {
            var data = db.TblcCentcardInvoiceSet.Where(c => c.Nodeid == req.Nodeid && c.Infoid == req.ID).FirstOrDefault();
            if (data == null)
            {
                Alert("未找到数据");
                return null;
            }

            if (data.Typeid != 1)
            {
                Alert("只能查看电子发票详情");
                return null;
            }

            return new InvioceDetailDto
            {
                Url = "http://images2.be.sulink.cn/daren/20191120/0ec6e55e-4b2a-4ace-8682-a71d877a416b.jpg"
            };

        }

        #region[Private]

        /// <summary>
        /// 获取已开票金额
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private decimal GetAlreadyAmount(int nodeid)
        {
            return (decimal)(from tci in db.TblcCentcardInvoiceSet.Where(c => c.Nodeid == nodeid)
                             join tc in db.TblcCentcardSet on tci.Idno equals tc.Idno into tcdata
                             from tcd in tcdata.DefaultIfEmpty()
                             select new { tcd.Amount }).ToList().Sum(c => c.Amount);
        }

        /// <summary>
        /// 获取微信购买的充值码统计
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private decimal GetAmount(int nodeid)
        {
            return (decimal)(from tch in db.TblcCentcardHisSet.Where(c => c.Nodeid == nodeid && c.Typeid == 0)
                             join tc in db.TblcCentcardSet on tch.Idno equals tc.Idno into tcdata
                             from tcd in tcdata.DefaultIfEmpty()
                             select new { tcd.Amount }).ToList().Sum(c => c.Amount);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="Receiver">邮件接收人</param>
        /// <param name="Subject">邮件主题</param>
        /// <param name="content">邮件内容</param>    
        /// <param name="userName">用户名</param>  
        /// <param name="path">附件地址</param>    
        private void SendEmail(string Receiver, string Subject, string content, string userName, string path)
        {
            string userEmailAddress = AppConfig.SendEmailNodeCode;
            string password = AppConfig.SendEmailPwd;

            SmtpClient client = null;
            if (string.IsNullOrEmpty(Receiver) || string.IsNullOrEmpty(Subject)
                || string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("SendEmail参数空异常！");
            }
            if (client == null)
            {
                try
                {
                    if (AppConfig.SendEmailType.Equals("163"))
                    {
                        //163发送配置                    
                        client = new System.Net.Mail.SmtpClient();
                        client.Host = "smtp.163.com";
                        client.Port = 25;
                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = true;
                    }
                    else
                    {
                        //qq发送配置的参数//切EnableSsl必须设置为true  
                        client = new System.Net.Mail.SmtpClient();
                        client.Host = "smtp.qq.com";
                        client.Port = 25;
                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                    }

                    client.Credentials = new System.Net.NetworkCredential(userEmailAddress, password);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            try
            {
                var attachFile = new Attachment(path);

                System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
                Message.SubjectEncoding = System.Text.Encoding.UTF8;
                Message.BodyEncoding = System.Text.Encoding.UTF8;
                Message.Priority = System.Net.Mail.MailPriority.Normal;

                Message.From = new System.Net.Mail.MailAddress(userEmailAddress, userName);
                //添加邮件接收人地址
                string[] receivers = Receiver.Split(new char[] { ',' });
                Array.ForEach(receivers.ToArray(), ToMail => { Message.To.Add(ToMail); });
                Message.Attachments.Add(attachFile);
                Message.Subject = Subject;
                Message.Body = content;
                Message.IsBodyHtml = true;
                client.Send(Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验输入的内容是否为邮箱
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        private bool IsEmail(string inputData)
        {
            Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 开具电子发票
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private bool WriteInvioce_Pro(int nodeid)
        {
            //var detail = new ElectroniceDetail
            //{
            //    HSBZ = "0",
            //    SL = "1",
            //    XMDJ = 1000,
            //    XMJE = 3000,
            //    XMMC = "测试",
            //    XMSL = 3,
            //    SPBM = "330300",
            //    YHZCBS = "0"
            //};

            //var dd = new ElectroniceDetail[2];
            //dd[0] = detail;

            //var sss = new ElectroniceInfo()
            //{
            //    BMB_BBH = "30.0",
            //    CZDM = "10",
            //    DDH = MakeOrderId(3434903),
            //    details = dd,
            //    DKBZ = "0",
            //    FPQQLSH = MakeOrderId(3434909),//Guid.NewGuid().ToString(),
            //    GHF_MC = "测试",
            //    KPHJJE = 3000,
            //    KPLX = "1",
            //    KPR = "测试企业名称",
            //    KP_NSRMC = "测试名称",
            //    KP_NSRSBH = "440002999999441",
            //    XHF_MC = "测试",
            //    XHF_DH = "13982300983",
            //    XHF_DZ = "测试地址",
            //    XHF_NSRSBH = "440002999999441",//"440002999999441",
            //    XHF_YHZH = "621345798124576",
            //    HJSE = 2,
            //    HJBHSJE = 2,
            //};

            //IEliWebService webService = new IEliWebService();
            //var ss = webService.invEli(sss);

            //var scs = webService.queryEliStock("440002999999441");
            return true;
        }

        /// <summary>
        /// 获取可开票金额
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private ValueTuple<decimal,decimal> GetAvailableAmount(int nodeid)
        {
            var amount = db.TpxinAmountChangeHisSet.Where(c => c.Nodeid == nodeid && c.Reason == 7).ToList().Sum(c => c.Amount);
            amount = amount > 0 ? amount / 10 : 0;
            var amount1 = GetAmount(nodeid);

            var amount2 = GetAlreadyAmount(nodeid);
            var result = amount > amount1 ? amount1 - amount2 : amount - amount2;
            return (result < 0?0: result, amount2);
        }

        private string MakeOrderId(int nodeid)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random(nodeid).Next(10000, 99999);
        }

        #endregion

        #region[后台管理]

        /// <summary>
        /// 获取增票资质申请列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public QualificaAdminDto GetInvioceQualificaList(GetInvioceQualificaListReq req)
        {
            var query = db.TpxinInvoiceLimitSet.ToList();
            if (req.Status != -1)
            {
                query = query.Where(c => c.Status == req.Status).ToList();
            }
            int num = query.Count();
            var result = query.Select(c=>new InvioceQualificaAdminDto
            { 
                Address=c.Address,
                Bank=c.Bank,
                Cardno=c.Cardno,
                Company=c.Company,
                Mobile=c.Mobile,
                Taxnum=c.Taxnum,
                Status=c.Status,
                Nodeid=c.Nodeid
            }).ToPagedList(req.PageNum, req.PageSize).ToList();

            return new QualificaAdminDto { 
                List= result,
                Num= num
            };
        }

        /// <summary>
        /// 获取开票列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public AlreadyApplyAdminDto GetWriteInvioceList(GetWriteInvioceListReq req)
        {
            var query = (from tci in db.TblcCentcardInvoiceSet
                          join tc in db.TblcCentcardSet on tci.Idno equals tc.Idno into tcdata
                          from tcd in tcdata.DefaultIfEmpty()
                         join tr in db.TnetReginfoSet on tci.Nodeid equals tr.Nodeid into trdata
                         from trd in trdata.DefaultIfEmpty()
                         orderby tci.Createtime descending
                          select new InvioceAlreadyApplyAdminDto
                          {
                              Amount = (decimal)tcd.Amount,
                              Infoid = tci.Infoid,
                              CardNum = tcd.Cardno,
                              Head = tci.Head,
                              ShowName = "充值码（SVC）",
                              Status = tci.Status,
                              TaxNum = tci.Code,
                              Typeid = tci.Typeid,
                              IdNo = tci.Infoid,
                              Expressno = tci.Expressno,
                              IsPerson = tci.Isperson,
                              NodeCode = trd.Nodecode
                          }).ToList();

            if (req.Status != -1)
            {
                query = query.Where(c => c.Status == req.Status).ToList();
            }
            if (req.Typeid != -1)
            {
                query = query.Where(c => c.Typeid == req.Typeid ).ToList();
            }
            if (req.IsPerson != -1)
            {
                query = query.Where(c => c.IsPerson == req.IsPerson).ToList();
            }
            int num = query.Count();

            return new AlreadyApplyAdminDto
            {
                List = query.ToPagedList(req.PageNum, req.PageSize).ToList(),
                Num = num
            };
        }

        /// <summary>
        /// 审核增票资质
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool VerifyInvioceQualifica(VerifyInvioceReq req)
        {
            var data = db.TpxinInvoiceLimitSet.Where(c => c.Nodeid == req.ID).FirstOrDefault();
            if (data == null)
            {
                Alert("记录不存在");
                return false;
            }

            if (data.Status == 2)
            {
                Alert("已经通过审核不可修改状态");
                return false;
            }

            if (req.Status == 0)
            {
                data.Status = 3;
                data.Note = req.Note;
            }
            else
            {
                data.Status = 2;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 开票审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool VerifyWriteInvioce(VerifyWriteInvioceReq req)
        {
            var data = db.TblcCentcardInvoiceSet.Where(c => c.Infoid == req.ID).FirstOrDefault();
            if (data == null)
            {
                Alert("记录不存在");
                return false;
            }

            if (data.Status == 2)
            {
                Alert("已经通过审核不可修改状态");
                return false;
            }

            if (req.Status == 0)
            {
                data.Status = 3;
            }
            else
            {
                data.Status = 2;
                data.Expressno = req.ExpressNo;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }

            return true;
        }

        #endregion

    }
}
