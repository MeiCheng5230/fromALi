using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PXin.DB;
using PXin.Facade.Models;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Enum;
using PXin.Facade.Models.Helper.FbAp;
using PXin.Facade.Models.Req;
using PXin.Facade.UEPay;
using PXin.Model;
using Winner.CU.Balance.GlobalCurrency;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 充值商(fb)代理人(ap)逻辑
    /// </summary>
    public class FbApFacade : FacadeBase<PXinContext>
    {
        #region PublicMethods
        /// <summary>
        /// 获取(充值商/代理人)信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<UserJxsDto> GetUserJxs(Reqbase req)
        {
            var targetTime = DateTime.Now.AddDays(-90);

            var result = from dlr in db.TblUserJxsSet
                         join czs in db.TblUserJxsSet on dlr.Pinfoid equals czs.Infoid into temp_czs
                         from czs in temp_czs.DefaultIfEmpty()
                         join user in db.TnetReginfoSet on czs.Nodeid equals user.Nodeid into temp_user
                         from user in temp_user.DefaultIfEmpty()
                         join dlrUser in db.TnetReginfoSet on dlr.Nodeid equals dlrUser.Nodeid into dlrUser_join
                         from dlrUser in dlrUser_join.DefaultIfEmpty()
                         join purse in db.TblcUserPurseSet.Where(p => p.Pursetype == 4 && p.Subid == 0 && p.Currencytype == 2) on dlr.Nodeid equals purse.Ownerid into purse_join
                         from purse in purse_join.DefaultIfEmpty()
                             //join his in db.TblUserJxsStockhisSet.Where(his => his.Stocktype == 1 && his.Createtime >= targetTime) on dlr.Infoid equals his.Jsxid into stockHis
                         where dlr.Nodeid == req.Nodeid && dlr.Sid == req.Sid
                         select new UserJxsDto
                         {
                             Nodeid = dlr.Nodeid,
                             InfoId = dlr.Infoid,
                             TypeId = dlr.Typeid,
                             TempEndTime = dlr.Endtime,
                             Status = dlr.Status,
                             StatusTwo = dlr.Status2,
                             WholesaleCodeStock = dlr.Stocknum,
                             RetailCodeStock = dlr.Stocknum2,
                             Name = dlr.Jsxname,
                             Nodename = dlrUser.Nodename,
                             PicCompany = dlr.PicCompany,
                             PicContract = dlr.PicContract,
                             PicHold = dlr.PicHold,
                             PicIdentback = dlr.PicIdentback,
                             PicIdentfront = dlr.PicIdentfront,
                             PicLicense = dlr.PicLicense,
                             Province = dlr.Province,
                             City = dlr.City,
                             Region = dlr.Region,
                             ChgTypeDate = dlr.Chgtypedate,
                             PChgTypeDate = czs.Chgtypedate,
                             IsShowStock = (dlr.Status + dlr.Status2) == 1,
                             IsShowRenew = dlr.Typeid < 5,
                             IsExpire = dlr.Endtime < DateTime.Now,
                             PIsExpire = czs.Endtime < DateTime.Now,
                             PNodeName = user.Nodename,
                             PMobileno = user.Mobileno,
                             SVBalance = purse == null ? 0 : purse.Balance - purse.Freezevalue,
                             IsGenerateCode = !(dlr.Status != 1 || dlr.Status2 != 0)
                         };
            var resultDto = await result.FirstOrDefaultAsync();
            if (resultDto == null)
            {
                Alert("获取数据失败,请联系管理员!");
                return null;
            }
            resultDto.IsShowStock = resultDto.ChgTypeDate.AddDays(30) >= DateTime.Now ? true : resultDto.IsShowStock;
            resultDto.IsExpire = resultDto.ChgTypeDate.AddDays(30) >= DateTime.Now ? false : resultDto.IsExpire;
            resultDto.PIsExpire = resultDto.PChgTypeDate?.AddDays(30) >= DateTime.Now ? false : resultDto.PIsExpire;
            resultDto.IsGenerateCode = resultDto.ChgTypeDate.AddDays(30) >= DateTime.Now ? true : resultDto.IsGenerateCode;
            resultDto.BindUEStatus = GetUeInfo(resultDto.Nodeid, 1);
            resultDto.EndTime = resultDto.TempEndTime.ToString("yyyy-MM-dd");
            DateTime time = DateTime.Now.Date;
            resultDto.TodayCount = GetTodaySvcLimit(req.Nodeid);

            var stockHis = await db.TblUserJxsStockhisSet.Where(his => his.Stocktype == 1 && his.Createtime >= targetTime && his.Jsxid == resultDto.InfoId && his.Isshow == 1 && his.Pcnnodecode == null).ToListAsync();
            resultDto.InputStock = stockHis.Sum(h => h.Rate);
            resultDto.Name = string.IsNullOrWhiteSpace(resultDto.Name) ? resultDto.Nodename : resultDto.Name;
            return resultDto;
        }
        /// <summary>
        /// 修改(充值商/代理人)名称
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<UpdateUserJxsNameDto> UpdateUserJxsName(UpdateUserJxsNameReq req)
        {
            var userJxs = await db.TblUserJxsSet.SingleOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (userJxs == null)
            {
                Alert($"找不到该{GetFbApName(userJxs)}");
                return null;
            }
            if (req.JxsName.Length > 20)
            {
                Alert("名称最大长度为20");
                return null;
            }
            userJxs.Jsxname = req.JxsName;
            var result = await db.SaveChangesAsync();
            if (result <= 0)
            {
                Alert("修改失败");
                return null;
            }
            return new UpdateUserJxsNameDto() { JxsName = userJxs.Jsxname };
        }
        /// <summary>
        /// 获取90天累积进货列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PurchaseWith90DaysDto> Get90DaysPurchases(PurchaseWith90DaysReq req)
        {
            var jxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (jxs == null)
            {
                Alert($"您不是{GetFbApName(jxs)}");
                return null;
            }
            var targetTime = DateTime.Now.AddDays(-90);
            //进货历史列表
            var jxsStockHis = await db.TblUserJxsStockhisSet
                .Where(p => p.Jsxid == req.InfoId && p.Createtime >= targetTime && p.Isshow == 1 && p.Stocktype == 1 && p.Pcnnodecode == null)
                .OrderByDescending(p => p.Createtime).ToListAsync();

            var result = new PurchaseWith90DaysDto
            {
                IsShowNoCheck = jxs.Nochecktime.AddDays(90) < DateTime.Now ? false : true,
                NoCheckTime = jxs.Nochecktime.AddDays(90).ToString("yyyy-MM-dd"),
            };
            result.TargetTimeStockNum = PXinContext.Get90DaysStockInfo(db, req.InfoId);
            jxsStockHis.ForEach(p =>
            {
                result.StockHis.Add(new StockhisWith90Days
                {
                    CreateTime = p.Createtime.ToString("yyyy-MM-dd HH:mm:ss"),
                    StockNum = p.Num,
                    Stocktype = p.Stocktype
                });
            });
            return result;
        }
        /// <summary>
        /// 获取库存记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<StockRecordDto>> GetStockRecord(StockRecordReq req)
        {
            var jxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (jxs == null)
            {
                Alert($"您不是{GetFbApName(jxs)}");
                return null;
            }
            var result = new List<StockRecordDto>();
            var stockhis = new List<TblUserJxsStockhis>();
            var stockhis2 = new List<TblUserJxsStockhis2>();
            if (req.Type == 1)
            {
                stockhis = await db.TblUserJxsStockhisSet.Where(p => p.Jsxid == req.InfoId && p.Isshow == 1 && (p.Stocktype == 1 || p.Stocktype == 3)).ToListAsync();
                stockhis2 = await db.TblUserJxsStockhis2Set.Where(p => p.Infoid == req.InfoId).ToListAsync();
            }
            else if (req.Type == 2)
            {
                stockhis = await db.TblUserJxsStockhisSet.Where(p => p.Jsxid == req.InfoId && p.Isshow == 1 && (p.Stocktype == 2 || p.Stocktype == 4)).ToListAsync();
            }
            return stockhis.Select(p => new StockRecordInfo
            {
                CreateTime = p.Createtime,
                Number = p.Num,
                TypeId = p.Stocktype
            }).Union(stockhis2.Select(s => new StockRecordInfo
            {
                CreateTime = s.Createtime,
                Number = s.Num * s.Amount,
                TypeId = s.Typeid + 5
            })).Select(p => new StockRecordDto
            {
                CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Number = p.Number,
                TypeId = p.TypeId
            }).OrderByDescending(p => p.CreateTime).ToList();
        }
        /// <summary>
        /// 我的代理人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<MyUserJxsDto>> GetMyUserJxs(MyUserJxsReq req)
        {
            var jxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId && p.Sid == req.Sid);
            if (jxs == null)
            {
                Alert($"您不是{GetFbApName(jxs)},没有开通此功能");
                return null;
            }
            var result = await (from j in db.TblUserJxsSet.Where(p => p.Pinfoid == req.InfoId)
                                join reg in db.TnetReginfoSet on j.Nodeid equals reg.Nodeid
                                join a in db.TbtcActivationSet
                                            .Where(a => a.Typeid == 4 && a.Codetype == 12 && a.NodeId == 0).GroupBy(p => p.Chgnodeid)
                                            .Select(p => new
                                            {
                                                ChgNodeId = p.Key,
                                                Counts = p.Count()
                                            })
                                            on j.Nodeid equals a.ChgNodeId into a_join
                                from activation in a_join.DefaultIfEmpty()
                                select new MyUserJxsDto
                                {
                                    InfoId = j.Infoid,
                                    Name = j.Jsxname,
                                    NodeName = reg.Nodename,
                                    StartTime = j.Starttime,
                                    EndTime = j.Endtime,
                                    Status = j.Status,
                                    StatusTwo = j.Status2,
                                    Counts = activation == null ? 0 : activation.Counts,
                                    PicCompany = j.PicCompany,
                                    PicContract = j.PicContract,
                                    PicHold = j.PicHold,
                                    PicIdentback = j.PicIdentback,
                                    PicIdentfront = j.PicIdentfront,
                                    PicLicense = j.PicLicense,
                                }).OrderByDescending(p => p.InfoId).ToListAsync();
            //foreach (var item in result)
            //{
            //    var userJxs = db.TblUserJxsSet.FirstOrDefault(p => p.Infoid == item.InfoId);
            //    item.StartTime = userJxs.Starttime.ToString("yyyy-MM-dd");
            //    item.EndTime = userJxs.Endtime.ToString("yyyy-MM-dd");
            //}
            return result;
        }
        /// <summary>
        /// 我的充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<MyUserCzsDto> GetMyUserCzs(MyUserCzsReq req)
        {
            var result = await (from jxs in db.TblUserJxsSet.Where(p => p.Infoid == req.InfoId)
                                join zys in db.TblUserJxsSet on jxs.Pinfoid equals zys.Infoid into zys_join
                                from zys in zys_join.DefaultIfEmpty()
                                join u in db.TnetReginfoSet on zys.Nodeid equals u.Nodeid into regInfo
                                from user in regInfo.DefaultIfEmpty()
                                select new MyUserCzsDto
                                {
                                    Mobileno = user.Mobileno,
                                    NodeName = user.Nodename,
                                    ParentName = zys.Jsxname
                                }).SingleOrDefaultAsync();

            var userJxsChange = await db.TblUserJxsChangeSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId && p.Typeid == 5);
            result.HsChanged = userJxsChange != null;
            return result;
        }
        /// <summary>
        /// 获取审核状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<AuditStatusDto> GetAuditStatus(AuditStatusReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (userJxs == null)
            {
                Alert($"找不到您的{GetFbApName(userJxs)}信息!");
                return null;
            }
            return new AuditStatusDto
            {
                TypeId = userJxs.Typeid,
                PicCompany = userJxs.PicCompany,
                PicContract = userJxs.PicContract,
                PicHold = userJxs.PicHold,
                PicIdentback = userJxs.PicIdentback,
                PicIdentfront = userJxs.PicIdentfront,
                PicLicense = userJxs.PicLicense,
                PicExt = userJxs.PicExt,
                Status = userJxs.Status,
                StatusTwo = userJxs.Status2,
                StatusThree = userJxs.Status3,
                Reason = userJxs.Remarks
            };
        }
        /// <summary>
        /// 上传认证资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<UploadAuthDataDto> UploadAuthData(UploadAuthDataReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid);
            if (!Validation(userJxs, req)) return null;
            if (req.AuthDatas.Where(a => a.AuthDataType == AuthDataType.Supplement).Any()) userJxs.Status3 = 0;
            db.BeginTransaction();
            try
            {
                foreach (var authData in req.AuthDatas)
                {
                    var index = 0;
                    ImageActionType type = default;
                    foreach (var file in authData.UploadFiles)
                    {
                        index = type == file.ImageActionType ? ++index : 1;
                        type = file.ImageActionType;
                        (string physicsFileName, string authFilePhysicsPath, string fullUrl) = GenerateFilePath(req, file);

                        (bool isSucc, string oldFilePath) = GetImageUrl(authData.AuthDataType, file.ImageActionType, userJxs, fullUrl, index);
                        if (!isSucc)
                        {
                            db.Rollback();
                            return null;
                        }
                        if (!Common.Facade.Helper.CopyFile(physicsFileName, authFilePhysicsPath))
                        {
                            db.Rollback();
                            return null;
                        }
                        else
                        {
                            Thread thread = new Thread(() =>
                            {
                                var directory = Path.GetDirectoryName(authFilePhysicsPath);
                                var filename = Path.GetFileNameWithoutExtension(authFilePhysicsPath);
                                var ext = Path.GetExtension(authFilePhysicsPath);
                                var descFile = Path.Combine(directory, $"{filename}_thumbnail{ext}");
                                var retry = 3;
                                var result = true;
                                {
                                    result = Common.Facade.Helper.GetPicThumbnail(authFilePhysicsPath, descFile, 90, 100);
                                    retry--;
                                } while (retry > 0 && !result) ;
                            });
                            thread.Start();
                        }
                    }
                }
                if (req.AuthDatas.Where(p => p.AuthDataType == AuthDataType.Supplement).Count() > 0)
                    userJxs.Status3 = 0;
                else
                    userJxs.Status = 0;

                if (!(await db.SaveChangesAsync() > 0))
                {
                    db.Rollback();
                    Alert("上传认证资料失败");
                    return null;
                }
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                Alert("上传认证资料失败");
                return null;
            }
            var resultDto = new UploadAuthDataDto()
            {
                TypeId = userJxs.Typeid,
                //PicCompany = userJxs.PicCompany,
                PicContract = userJxs.PicContract,
                PicHold = userJxs.PicHold,
                PicIdentback = userJxs.PicIdentback,
                PicIdentfront = userJxs.PicIdentfront,
                PicLicense = userJxs.PicLicense,
                PicExt = userJxs.PicExt,
                Status = userJxs.Status,
                StatusTwo = userJxs.Status2,
                StatusThree = userJxs.Status3
            };
            if (req.AuthDatas.Where(a => a.AuthDataType == AuthDataType.Supplement).Any())//补充
                resultDto.IsExt = true;
            return resultDto;
        }
        /// <summary>
        /// 审核代理人资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> AuditJxsInfo(AuditJxsInfoReq req)
        {
            if (req.Status != 1 && req.Status != 2)
            {
                Alert("参数错误");
                return false;
            }
            var userJxs = await db.TblUserJxsSet.SingleOrDefaultAsync(j => j.Infoid == req.InfoId);
            if (userJxs == null)
            {
                Alert($"该{GetFbApName(userJxs)}不存在！");
                return false;
            }
            userJxs.Status = req.Status;
            userJxs.Remarks = req.Remarks;
            var result = await db.SaveChangesAsync() > 0;
            if (!result)
            {
                Alert("审核失败");
                return false;
            }
            return result;
        }
        /// <summary>
        /// 获取认证资料
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<AuthDataDto> GetAuthData(AuthDataReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (userJxs == null)
            {
                Alert("找不到您的信息,请联系上级充值商或管理员");
                return null;
            }
            var result = new AuthDataDto()
            {
                TypeId = userJxs.Typeid,
                //PicCompany = userJxs.PicCompany,
                //PicContract = userJxs.PicContract,
                PicHold = userJxs.PicHold,
                PicIdentback = userJxs.PicIdentback,
                PicIdentfront = userJxs.PicIdentfront,
                PicLicense = userJxs.PicLicense,
                PicExt = userJxs.PicExt,
                Status = userJxs.Status,
                StatusTwo = userJxs.Status2,
                StatusThree = userJxs.Status3,
                Remark = userJxs.Remarks,
                SupplementRemark = userJxs.Remarks3
            };
            if (req.AuthDataType == AuthDataType.Personal)//个人
            {
                GetPersonalData(userJxs, result);
            }
            else if (req.AuthDataType == AuthDataType.Company)//公司资料
            {
                GetCompanyData(userJxs, result);
            }
            else if (req.AuthDataType == AuthDataType.Contract)//合同
            {
                GetContractDataa(userJxs, result);
            }
            else if (req.AuthDataType == AuthDataType.Supplement)//补充
            {
                GetSupplementData(userJxs, result);
            }
            else
            {
                GetPersonalData(userJxs, result);
                GetCompanyData(userJxs, result);
                GetContractDataa(userJxs, result);
                GetSupplementData(userJxs, result);
            }
            //判断有没有压缩图片
            CheckThumbnail(result.ImageUrls);

            return result;
        }
        /// <summary>
        /// 获取续费信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<RenewInfoDto> GetRenewInfo(RenewInfoReq req)
        {
            var userJxs = db.TblUserJxsSet
                 .Where(p => p.Infoid == req.InfoId);
            if (userJxs.Count() == 0)
            {
                Alert($"您不是充值商");
                return null;
            }
            var result = await (from jxs in userJxs
                                join u in db.TnetReginfoSet on jxs.Nodeid equals u.Nodeid into u_join
                                from user in u_join.DefaultIfEmpty()
                                select new RenewInfoDto
                                {
                                    Mobileno = user.Mobileno,
                                    Nodename = user.Nodename,
                                    TypeId = jxs.Typeid,
                                    Amount = jxs.Typeid == 5 ? 500 : 1500
                                }).FirstOrDefaultAsync();
            result.EndTime = userJxs.FirstOrDefault().Endtime.ToString("yyyy-MM-dd");
            return result;
        }
        /// <summary>
        /// 续费
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<RenewDto> Renew(RenewReq req)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户基本信息失败");
                return null;
            }
            var czs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (czs.Typeid > 4)
            {
                Alert("您不是充值商不能续费");
                return null;
            }
            if (userJxs.Status + userJxs.Status2 != 1)
            {
                Alert($"{userJxs.Jsxname}当前审核状态为{GetStatusDesc(userJxs)},不能续费");
                return null;
            }
            var uePayResult = await UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, userJxs.Typeid == 5 ? 500 : 1500, 20005, userJxs.Infoid.ToString(), "充值商续费", "充值商续费");
            if (!uePayResult.IsSuccess)
            {
                Alert(uePayResult.Message);
                return null;
            }
            return new RenewDto
            {
                ChargeStr = uePayResult.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = uePayResult.OrderNo
            };
        }
        /// <summary>
        /// 续费回调
        /// </summary>
        /// <param name="uePayHis"></param>
        /// <returns></returns>
        public async Task<bool> Renew_Notice(TnetUepayhis uePayHis)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = uePayHis.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            db.BeginTransaction();
            (bool isSucc, int businessId) tupleData = await JxsRenew_Pro(uePayHis);
            if (!tupleData.isSucc)
            {
                db.Rollback();
                return false;
            }
            uePayHis.BusinessId = tupleData.businessId;
            uePayHis.Status = 2;
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("orderid=" + uePayHis.Id + ",更新订单状态失败");
                db.Rollback();
                return false;
            }
            db.Commit();
            return true;
        }
        /// <summary>
        /// 获取sv库存/dos余额 
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetStockBalance(StockBalanceReq req)
        {
            var result = 0m;
            if (req.Type == 1)
            {
                var userPUrse = await db.TblcUserPurseSet.FirstOrDefaultAsync(p => p.Ownerid == req.Nodeid && p.Ownertype == 1 && p.Pursetype == 3 && p.Subid == 15 && p.Currencytype == 8);
                result = userPUrse == null ? 0 : userPUrse.Balance - userPUrse.Freezevalue;
            }
            else if (req.Type == 2)
            {
                var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == req.Nodeid);
                result = userJxs == null ? 0 : userJxs.Stocknum2;
            }
            return result;
        }
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        public async Task<SearchUserDto> SearchUser(SearchUserReq req)
        {
            TnetReginfo userInfo = null;
            if (req.Type == 1)
                userInfo = await db.TnetReginfoSet.FirstOrDefaultAsync(p => p.Mobileno == req.Key || p.Email == req.Key || p.Nodecode == req.Key);
            else if (req.Type == 2 || req.Type == 4)
                userInfo = await db.TnetReginfoSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            else if (req.Type == 3)
                userInfo = await db.TnetReginfoSet.FirstOrDefaultAsync(p => p.Mobileno == req.Key || p.Nodecode == req.Key);
            if (userInfo == null)
            {
                Alert("用户不存在");
                return null;
            }
            if (req.Type != 2)
            {
                var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == req.Nodeid);
                if (userJxs == null)
                {
                    Alert("找不到您当前信息,请重试");
                    return null;
                }
            }
            var targetUserJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == userInfo.Nodeid);
            if (req.Type == 1 || req.Type == 2)
            {
                if (targetUserJxs != null)
                {
                    var name = targetUserJxs.Typeid == 4 ? "充值商" : "代理人";
                    Alert($"用户已经是{name}");
                    return null;
                }
            }
            else if (req.Type == 3)
            {
                if (targetUserJxs != null && targetUserJxs.Typeid == 4)
                {
                    Alert($"用户{req.Key}已经是充值商");
                    return null;
                }
            }
            return new SearchUserDto
            {
                NodeId = userInfo.Nodeid,
                NodeCode = userInfo.Nodecode,
                NodeName = userInfo.Nodename,
                Mobileno = userInfo.Mobileno
            };

        }
        /// <summary>
        /// 新增代理人
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public AddDealerDto AddUserJxs(AddDealerReq req)
        {
            string lockStr = string.Intern(req.Nodeid.ToString());
            lock (lockStr)
            {
                var result = new AddDealerDto();
                var amount = Convert.ToDecimal(AppConfig.OpenDlrAmount);
                (bool isSuccess, TnetReginfo userInfo) = AddUserJxsCheck(req).GetAwaiter().GetResult();
                if (!isSuccess)
                {
                    return null;
                }
                if (amount > 0)
                {
                    var uePayResult = UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, amount, 20002, $"{req.Sid}|{req.NewNodeId}", "新增代理人", "新增代理人").GetAwaiter().GetResult();
                    if (!uePayResult.IsSuccess)
                    {
                        Alert(uePayResult.Message);
                        return null;
                    }
                    result.ChargeStr = uePayResult.ChargeStr;
                    result.Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper();
                    result.OrderNo = uePayResult.OrderNo;
                }
                else
                {
                    var userJxsConfirm = db.TblUserJxsConfirmSet.FirstOrDefault(p => p.Nodeid == req.NewNodeId && p.Opnodeid == req.Nodeid && p.Status == 0);
                    if (userJxsConfirm != null)
                    {
                        Alert("您已添加过该代理人,请耐心等待");
                        return null;
                    }
                    userJxsConfirm = new TblUserJxsConfirm();
                    userJxsConfirm.Nodeid = req.NewNodeId;
                    userJxsConfirm.Opnodeid = req.Nodeid;
                    userJxsConfirm.Status = 0;
                    userJxsConfirm.Createtime = DateTime.Now;
                    userJxsConfirm.Remarks = "新增代理人待确认";
                    db.TblUserJxsConfirmSet.Add(userJxsConfirm);
                    var saveResult = db.SaveChanges() > 0;
                    if (!saveResult)
                    {
                        Alert("添加代理人失败");
                        return null;
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 新增代理人回调
        /// </summary>
        /// <param name="uePayHis"></param>
        /// <returns></returns>
        public async Task<bool> AddUserJxs_Notice(TnetUepayhis uePayHis)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = uePayHis.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            db.BeginTransaction();

            var sid = Convert.ToInt32(uePayHis.BusinessParams.Split('|')[0]);
            var newNodeId = Convert.ToInt32(uePayHis.BusinessParams.Split('|')[1]);
            (bool isSucc, int businessId) tupleData = await AddUserJxs_Pro(userInfo, sid, newNodeId);
            if (!tupleData.isSucc)
            {
                db.Rollback();
                return false;
            }
            uePayHis.BusinessId = tupleData.businessId;
            uePayHis.Status = 2;
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("orderid=" + uePayHis.Id + ",更新订单状态失败");
                db.Rollback();
                return false;
            }
            db.Commit();
            return true;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<FbApUserInfoDto> GetUserInfo(UserInfoReq req)
        {
            var userJxs = await db.TblUserJxsSet
                .Where(p => p.Infoid == req.InfoId && p.Nodeid == req.Nodeid)
                .Select(p => new
                {
                    NodeId = p.Nodeid,
                    Typeid = p.Typeid,
                    IdentityDesc = $"您当前是" + p.City + p.Region + (p.Typeid == 5 ? "代理人" : "充值商"),
                }).FirstOrDefaultAsync();
            if (userJxs == null)
            {
                Alert("你还未成为代理人或充值商");
                return null;
            }
            var tssoOpenUser = await db.TssoOpenUserSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Opentype == 5);
            return new FbApUserInfoDto
            {
                Typeid = userJxs.Typeid,
                NodeName = db.TnetReginfoSet.FirstOrDefault(u => u.Nodeid == userJxs.NodeId)?.Nodename,
                IdentityDesc = userJxs.IdentityDesc,
                AppPhoto = db.TnetUserphotoSet.FirstOrDefault(u => u.Nodeid == userJxs.NodeId)?.Appphoto,
                IsBind = tssoOpenUser != null,
                Nodecode = tssoOpenUser != null ? tssoOpenUser.Openid : "",
                IsActivity = AppConfig.FbapStockinActivity == DateTime.Now.Month.ToString()
            };
        }
        /// <summary>
        /// 获取兑换类型信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<ExchangeTypeInfoDto>> GetExchangeTypeInfo(ExchangeTypeInfoReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == req.InfoId);
            if (userJxs == null)
            {
                Alert($"找不到您的信息,请刷新后重试");
                return null;
            }
            var rechargeCodeRule = GetRechargeCodeRule(userJxs);

            return rechargeCodeRule.Select(p => new ExchangeTypeInfoDto
            {
                Code = p.Code,
                Dos = p.Dos,
                Id = p.Id,
                RetailCodeNum = p.RetailCodeStock,
                SubTitle = p.Subtitle,
                Title = p.Title,
                WholesaleCodeNum = p.WholesaleCodeStock,
                IsPromotion = p.IsPromotion
            }).ToList();
        }
        /// <summary>
        /// 兑换充值码(进货)
        /// </summary>
        /// <returns></returns>
        public async Task<ExChangeRechargeCodeDto> ExChangeRechargeCode(ExChangeRechargeCodeReq req)
        {
            (bool isSucc, TnetReginfo userInfo, TblUserJxs userJxs, RechargeCodeRuleInfo rule) = await CheckData(req);
            if (!isSucc) return null;
            if (req.IsPromotion)
            {
                //检查是否为vip
                var remoteReq = new { nodecode = req.Nodecode, nodeid = req.Nodeid, sid = req.Sid, tm = req.Tm, sign = req.Sign, client = req.Client };
                string resultStr = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.YGDomain}/api/UserVip/CheckUserIsVip", remoteReq);
                if (string.IsNullOrEmpty(resultStr) || !resultStr.Contains("result"))
                {
                    Alert("网络异常,请重试!");
                    return null;
                }
                var result = JObject.Parse(resultStr);
                var res = Convert.ToInt32(result.GetValue("result").ToString());
                if (res > 0)
                {
                    var data = result["data"]["isvip"].ToString();
                    if (!Convert.ToBoolean(data))
                    {
                        Alert("您当前的优谷帐号不是VIP,无法参与活动!");
                        return null;
                    }
                }
                else
                {
                    Alert(result.GetValue("message").ToString());
                    return null;
                }
            }
            //是否成功，上级NodeId,批发码库存
            (bool isSucced, int pNodeId, TblUserJxs jsxParent) = await GetParentJxsInfo(userJxs, rule, req);
            if (!isSucced)
            {
                return null;
            }
            string paramStr = $"{req.Sid.ToString()}|{req.RuleId.ToString()}|{req.IsPromotion}|{req.Nodecode }";
            var uePayResult = await UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, rule.DosPrice, 20003, paramStr, "进货SVC", "进货SVC", "", pNodeId);
            if (!uePayResult.IsSuccess)
            {
                Alert(uePayResult.Message);
                return null;
            }
            return new ExChangeRechargeCodeDto
            {
                ChargeStr = uePayResult.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = uePayResult.OrderNo,
            };
        }
        /// <summary>
        /// 兑换充值码回调
        /// </summary>
        /// <param name="uePayHis">优谷UE支付历史</param>
        /// <returns></returns>
        public async Task<bool> ExChangeRechargeCode_Notice(TnetUepayhis uePayHis)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = uePayHis.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            string[] paramArr = uePayHis.BusinessParams.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int sid = Convert.ToInt32(paramArr[0]);
            int ruleId = Convert.ToInt32(paramArr[1]);
            bool isPromotion = Convert.ToBoolean(paramArr[2]);
            string nodecode = paramArr.Length >= 4 ? paramArr[3] : "";

            int freezeId = Convert.ToInt32(uePayHis.Freezeids);
            var province = "";
            var city = "";
            var region = "";
            db.BeginTransaction();
            if (nodecode == "ApplyFbap")//判断是否是充值商申请
            {
                province = paramArr[4];
                city = paramArr[5];
                region = paramArr[6];
                if (!await ApplyFbap_Pro(userInfo, userInfo.Nodeid, sid, province, city, region))//新增充值商
                {
                    db.Rollback();
                    return false;
                }
            }
            (bool isSucc, int businessId) tupleData = await ExChangeRechargeCode_Pro(userInfo, sid, ruleId, isPromotion, nodecode);
            if (!tupleData.isSucc)
            {
                db.Rollback();
                return false;
            }
            uePayHis.BusinessId = tupleData.businessId;
            uePayHis.Status = 2;
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("orderid=" + uePayHis.Id + ",更新订单状态失败");
                db.Rollback();
                return false;
            }
            db.Commit();
            return true;
        }
        /// <summary>
        /// 获取充值商转向初始页面的值
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<FbapInitPageDto> GetFbapInitPage(FbapInitPageReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid);
            var userJxsConfirms = await db.TblUserJxsConfirmSet.Where(p => p.Nodeid == req.Nodeid && p.Status == 0).ToListAsync();
            return new FbapInitPageDto
            {
                Type = userJxs != null ? 3 : (userJxsConfirms.Count > 0 ? 2 : 1)
            };
        }
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool VerifyPwd(VerifyPwdReq req)
        {
            string resultStr = HttpSimulation.Instance.RequestByJsonOrQueryString($"{AppConfig.YGDomain}/api/User/VerifyPwd", req);
            if (string.IsNullOrEmpty(resultStr) || !resultStr.Contains("result"))
            {
                Alert("用户帐号或密码错误!");
                return false;
            }
            var result = JObject.Parse(resultStr);
            var res = Convert.ToInt32(result.GetValue("result").ToString());
            if (res > 0)
            {
                var lockStr = string.Intern(req.Nodecode);
                lock (lockStr)
                {
                    var flag = false;
                    var tssoOpenUser = db.TssoOpenUserSet.FirstOrDefault(p => p.Opentype == 5 && p.Openid == req.Nodecode);
                    if (tssoOpenUser != null)
                    {
                        Alert("对不起，该帐号已被绑定");
                        return false;
                    }
                    else
                    {
                        db.TssoOpenUserSet.Add(new TssoOpenUser
                        {
                            Nodeid = req.Nodeid,
                            Opentype = 5,
                            Openid = req.Nodecode,
                            Createtime = DateTime.Now,
                            Remarks = "绑定优谷帐号"
                        });
                        flag = db.SaveChanges() > 0;
                    }
                    if (flag)
                        return true;
                    else
                    {
                        Alert("绑定优谷帐号出现错误!");
                        return false;
                    }
                }
            }
            else
            {
                Alert(result.GetValue("message").ToString());
                return false;
            }

        }
        /// <summary>
        /// 充值商申请
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ApplyFbapDto> ApplyFbap(ApplyFbapReq req)
        {
            (bool isSuccess, TnetReginfo userInfo) = await ApplyFbapCheck(req);
            if (isSuccess)
            {
                var curTime = DateTime.Now;
                var jxsStockConfig = await db.TblUserJxsStockconfigSet
                                  .Where(p => p.Fromtime <= curTime && p.Endtime > curTime && p.Typeid == 4 && p.Isfirst == 1 && p.Isallowcua == 0).ToListAsync();
                if (jxsStockConfig.Count == 0 || jxsStockConfig.Count > 1)
                {
                    Alert("申请失败,请联系管理员!");
                    return null;
                }
                var ruleId = jxsStockConfig.FirstOrDefault().Infoid;
                var nodecode = "ApplyFbap";
                var uePayResult = await UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, 5000, 20007, $"{req.Sid}|{ruleId}|{false}|{nodecode}|{req.Province}|{req.City}|{req.Region}", "充值商申请", "充值商申请");
                if (!uePayResult.IsSuccess)
                {
                    Alert(uePayResult.Message);
                    return null;
                }
                var result = new ApplyFbapDto();
                result.ChargeStr = uePayResult.ChargeStr;
                result.Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper();
                result.OrderNo = uePayResult.OrderNo;
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取会议列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<List<MeetInfoDto>> GetMeetInfos(MeetInfoReq req)
        {
            var meetInfoList = new List<TbossMeetinfo>();
            var curTime = DateTime.Now;
            if (req.Type == 1)
            {
                meetInfoList = await db.TbossMeetinfoSet.AsNoTracking().Where(p => p.Starttime > curTime).OrderByDescending(p => p.Createtime).ThenByDescending(p => p.Starttime).ToListAsync();
            }
            else
            {
                meetInfoList = await db.TbossMeetinfoSet.AsNoTracking().OrderByDescending(p => p.Createtime).ThenByDescending(p => p.Starttime).ToListAsync();
            }
            var meethis = await db.TbossMeethisSet.AsNoTracking().Where(p => p.Nodeid == req.Nodeid).ToListAsync();
            var resultDto = meetInfoList.Select(p => new MeetInfoDto
            {
                Infoid = p.Infoid,
                Title = p.Title,
                Starttime = p.Starttime,
                Address = p.Address,
                Detail = p.Detail,
                Status = p.Starttime < curTime ? -1 : meethis.FirstOrDefault(s => s.Infoid == p.Infoid) == null ? 0 : 1
            }).ToList();
            return resultDto;
        }
        /// <summary>
        /// 获取会议详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<MeetInfoDetailDto> GetMeetInfoDetail(MeetInfoDetailReq req)
        {
            var meetInfo = await db.TbossMeetinfoSet.Where(p => p.Infoid == req.Infoid).FirstOrDefaultAsync();
            if (meetInfo == null)
            {
                Alert("会议信息不存在");
                return null;
            }
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            var meethis = await db.TbossMeethisSet.AsNoTracking().FirstOrDefaultAsync(p => p.Infoid == meetInfo.Infoid && p.Nodeid == req.Nodeid);
            var curTime = DateTime.Now;

            var content = "";
            var flag = true;
            var retry = 0;
            {
                retry += 1;
                try
                {
                    HttpClient httpClient = new HttpClient();
                    content = await httpClient.GetStringAsync(meetInfo.Detail);
                }
                catch
                {
                    flag = false;
                }
            }
            while (retry < 3 && !flag) ;
            var resultDto = new MeetInfoDetailDto();
            resultDto.Title = meetInfo.Title;
            resultDto.Starttime = meetInfo.Starttime;
            resultDto.Address = meetInfo.Address;
            resultDto.Name = meethis == null ? userInfo.Nodename : meethis.Name;
            resultDto.Mobileno = meethis == null ? userInfo.Mobileno : meethis.Mobileno;
            resultDto.Detail = content;
            resultDto.Status = meetInfo.Starttime < curTime ? -1 : meethis == null ? 0 : 1;
            if (meethis != null && !string.IsNullOrWhiteSpace(meethis.JoinPersons))
            {
                resultDto.JoinMeetingPersons = JsonConvert.DeserializeObject<List<JoinMeetingPersonDto>>(meethis.JoinPersons);
            }
            return resultDto;
        }
        /// <summary>
        /// 参加会议
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> JoinMeeting(JoinMeetingReq req)
        {
            var curTime = DateTime.Now;
            var meetInfo = await db.TbossMeetinfoSet.Where(p => p.Infoid == req.Infoid).FirstOrDefaultAsync();
            if (meetInfo == null)
            {
                Alert("会议信息不存在");
                return false;
            }
            if (meetInfo.Starttime < curTime)
            {
                Alert("对不起,会议已开始,停止报名");
                return false;
            }
            var meethis = await db.TbossMeethisSet.FirstOrDefaultAsync(p => p.Infoid == req.Infoid && p.Nodeid == req.Nodeid);
            if (meethis != null)
            {
                Alert("您已参加该会议,请耐心等待");
                return false;
            }

            meethis = new TbossMeethis();
            meethis.Infoid = req.Infoid;
            meethis.Nodeid = req.Nodeid;
            meethis.Name = req.Name;
            meethis.Mobileno = req.Mobileno;
            meethis.Createtime = DateTime.Now;
            meethis.JoinPersons = JsonConvert.SerializeObject(req.JoinMeetingPersons);
            db.TbossMeethisSet.Add(meethis);

            var result = await db.SaveChangesAsync();
            if (result <= 0)
            {
                Alert("报名失败,联系管理员!");
                return false;
            }
            return result > 0;
        }
        /// <summary>
        /// 查询充值商信息
        /// </summary>
        /// <returns></returns>
        public async Task<FbapInfoDto> GetFbapInfo(FbapInfoReq req)
        {
            TnetReginfo userInfo = await db.TnetReginfoSet.FirstOrDefaultAsync(p => p.Mobileno == req.Key || p.Nodecode == req.Key);
            if (userInfo == null)
            {
                Alert("用户不存在");
                return null;
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == req.Nodeid && p.Typeid == 5);
            if (userJxs == null)
            {
                Alert("找不到您的代理人信息,请重试");
                return null;
            }
            var targetUserJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == userInfo.Nodeid);
            if (targetUserJxs == null)
            {
                Alert("充值商不存在");
                return null;
            }
            else
            {
                if (targetUserJxs.Typeid == 5)
                {
                    Alert("该用户为代理人,不能更改");
                    return null;
                }
                if (targetUserJxs.Infoid == userJxs.Infoid)
                {
                    Alert("您要更改的充值商不能为自己");
                    return null;
                }
                if (targetUserJxs.Infoid == userJxs.Pinfoid)
                {
                    Alert("您要更改的充值商不能和之前一样");
                    return null;
                }
            }
            return new FbapInfoDto
            {
                Name = targetUserJxs.Jsxname,
                NodeName = userInfo.Nodename,
                Mobileno = userInfo.Mobileno
            };
        }
        /// <summary>
        /// 更换充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> ChangeFbap(ChangeFbapReq req)
        {
            var dResult = DateTime.TryParse(AppConfig.BetaEndTime, out DateTime betaEndTime);
            if (dResult && DateTime.Now <= betaEndTime)
            {
                var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Infoid == req.Infoid && p.Nodeid == req.Nodeid && p.Typeid == 5);
                if (userJxs == null)
                {
                    Alert("找不到您的代理人信息,请重试");
                    return false;
                }
                TnetReginfo userInfo = await db.TnetReginfoSet.FirstOrDefaultAsync(p => p.Mobileno == req.Key || p.Nodecode == req.Key);
                if (userInfo == null)
                {
                    Alert("您要更改的充值商用户不存在");
                    return false;
                }
                var targetUserJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Sid == req.Sid && p.Nodeid == userInfo.Nodeid);
                if (targetUserJxs == null)
                {
                    Alert("您要更改的充值商不存在");
                    return false;
                }
                else
                {
                    if (targetUserJxs.Typeid == 5)
                    {
                        Alert("该用户为代理人,不能更改");
                        return false;
                    }
                    if (targetUserJxs.Infoid == userJxs.Infoid)
                    {
                        Alert("您要更改的充值商不能为自己");
                        return false;
                    }
                    if (targetUserJxs.Infoid == userJxs.Pinfoid)
                    {
                        Alert("您要更改的充值商不能和之前一样");
                        return false;
                    }
                }
                var userJxsChange = await db.TblUserJxsChangeSet.FirstOrDefaultAsync(p => p.Infoid == req.Infoid && p.Typeid == userJxs.Typeid);
                if (userJxsChange == null)
                {
                    userJxsChange = new TblUserJxsChange();
                    userJxsChange.Infoid = userJxs.Infoid;
                    userJxsChange.Typeid = userJxs.Typeid;
                    userJxsChange.Fromstatus = userJxs.Pinfoid ?? 0;
                    userJxsChange.Endstatus = targetUserJxs.Infoid;
                    userJxsChange.Note = "代理人更改上级充值商";
                    userJxsChange.Createtime = DateTime.Now;
                    userJxsChange.Remarks = "代理人更改上级充值商";

                    userJxs.Pinfoid = targetUserJxs.Infoid;
                    userJxs.Province = targetUserJxs.Province;
                    userJxs.City = targetUserJxs.City;
                    userJxs.Region = targetUserJxs.Region;

                    db.TblUserJxsChangeSet.Add(userJxsChange);
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        Alert("更改充值商失败,请重试");
                        return false;
                    }
                }
                else
                {
                    Alert("对不起,您已修改过一次充值商!");
                    return false;
                }
            }
            else
            {
                Alert("对不起,内测期间已过,您暂无权限更改上级充值商!");
                return false;
            }
        }
        /// <summary>
        /// 开通充值商检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> CheckOpenCzs(CheckOpenCzsReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid && p.Typeid == 4);
            if (userJxs == null)
            {
                Alert("充值商不存在");
                return false;
            }
            if (userJxs.Status + userJxs.Status2 != 1)
            {
                if (userJxs.Status == 2)
                {
                    Alert($"当前账号审核未通过,没有开通权限");
                    return false;
                }
            }
            if (userJxs.Status2 != 0)
            {
                Alert($"当前账号已冻结,没有开通权限");
                return false;
            }
            if (userJxs.Endtime < DateTime.Now)
            {
                Alert($"充值商已过期,请续费");
                return false;
            }
            if (userJxs.Stocknum2 < 50000)
            {
                Alert($"当前账号零售码库存不足5万");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 开通充值商
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<OpenCzsDto> OpenCzs(OpenCzsReq req)
        {
            (bool isSuccess, TnetReginfo userInfo) = await OpenCzsCheck(req);
            if (!isSuccess)
            {
                return null;
            }
            var dResult = DateTime.TryParse(AppConfig.BetaEndTime, out DateTime betaEndTime);
            if (dResult && DateTime.Now < betaEndTime)//内测期间
            {
                var curTime = DateTime.Now;
                var jxsStockConfig = await db.TblUserJxsStockconfigSet
                                  .Where(p => p.Fromtime <= curTime && p.Endtime > curTime && p.Typeid == 4 && p.Isfirst == 1 && p.Isallowcua == 0).ToListAsync();
                if (jxsStockConfig.Count == 0 || jxsStockConfig.Count > 1)
                {
                    Alert("申请失败,请联系管理员!");
                    return null;
                }
                var ruleId = jxsStockConfig.FirstOrDefault().Infoid;
                var uePayResult = await UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, 1500, 20010, $"{req.Sid}|{ruleId}|{req.NewNodeId}|{req.Province}|{req.City}|{req.Region}", "代开充值商", "代开充值商");
                if (!uePayResult.IsSuccess)
                {
                    Alert(uePayResult.Message);
                    return null;
                }
                var result = new OpenCzsDto();
                result.ChargeStr = uePayResult.ChargeStr;
                result.Sign = Common.Mvc.Md5.SignString(uePayResult.ChargeStr + AppConfig.AppSecurityString).ToUpper();
                result.OrderNo = uePayResult.OrderNo;
                return result;
                //return await OpenCzsLogic_Beta(req, userInfo);
            }
            else
            {
                Alert("该功能开发中...");
                return null;
            }
        }
        /// <summary>
        /// 代开充值商回调
        /// </summary>
        /// <returns></returns>
        public async Task<bool> OpenCzs_Notice(TnetUepayhis uePayHis)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = uePayHis.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            string[] paramArr = uePayHis.BusinessParams.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int sid = Convert.ToInt32(paramArr[0]);
            int ruleId = Convert.ToInt32(paramArr[1]);
            int newNodeId = Convert.ToInt32(paramArr[2]);
            var province = paramArr[3];
            var city = paramArr[4];
            var region = paramArr[5];
            db.BeginTransaction();
            var targetUser = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = newNodeId.ToString() });
            if (!await ApplyFbap_Pro(targetUser, userInfo.Nodeid, sid, province, city, region))//开通充值商
            {
                db.Rollback();
                return false;
            }
            var curTime = DateTime.Now;
            (bool isSucc, int businessId) tupleData = await ExChangeRechargeCode_Pro(targetUser, sid, ruleId, false, "OpenFbap");
            if (!tupleData.isSucc)
            {
                db.Rollback();
                return false;
            }
            //生成充值码，在截至时间内赠送svc充值码
            if (DateTime.Now <= new DateTime(2019, 12, 31, 23, 59, 59))
            {
                if (!await GenerateCzm(userInfo.Nodeid, uePayHis.Id))
                {
                    db.Rollback();
                    return false;
                }
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == userInfo.Nodeid && p.Sid == sid && p.Typeid == 4);
            userJxs.Stocknum2 -= 50000;

            var userJxsStockhis2 = new TblUserJxsStockhis2();
            userJxsStockhis2.Infoid = userJxs.Infoid;
            userJxsStockhis2.Typeid = 2;
            userJxsStockhis2.Nodeid = newNodeId;
            userJxsStockhis2.Amount = 50000;//sv库存 
            userJxsStockhis2.Num = 1;
            userJxsStockhis2.Totalamount = userJxsStockhis2.Amount * userJxsStockhis2.Num;
            userJxsStockhis2.Opnodeid = userInfo.Nodeid;
            userJxsStockhis2.Createtime = DateTime.Now;
            userJxsStockhis2.Remarks = "充值商代开充值商";
            db.TblUserJxsStockhis2Set.Add(userJxsStockhis2);
            if (await db.SaveChangesAsync() <= 0)
            {
                Alert("新增代开充值商进货历史失败");
                db.Rollback();
                return false;
            }
            uePayHis.BusinessId = tupleData.businessId;
            uePayHis.Status = 2;
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("orderid=" + uePayHis.Id + ",更新订单状态失败");
                db.Rollback();
                return false;
            }
            return db.Commit();
        }
        /// <summary>
        /// 查询充值商添加代理人请求列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserJxsConfirmsDto>> GetUserJxsConfirms(UserJxsConfirmsReq req)
        {
            var resultDto = await (from confirm in db.TblUserJxsConfirmSet
                                   join czsUser in db.TnetReginfoSet on confirm.Opnodeid equals czsUser.Nodeid into czsUser_join
                                   from czsUser in czsUser_join.DefaultIfEmpty()
                                   join userPhoto in db.TnetUserphotoSet on czsUser.Nodeid equals userPhoto.Nodeid
                                   join userJxs in db.TblUserJxsSet on confirm.Opnodeid equals userJxs.Nodeid
                                   where confirm.Nodeid == req.Nodeid && confirm.Status == 0
                                   select new UserJxsConfirmsDto
                                   {
                                       AppPhoto = userPhoto.Appphoto,
                                       CzsNodeid = czsUser.Nodeid,
                                       Nodename = czsUser.Nodename,
                                       Name = userJxs.Jsxname
                                   }).ToListAsync();
            return resultDto;
        }
        /// <summary>
        /// 同意充值商添加代理人请求
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AgreeUserJxsRequst(UserJxsRequstReq req)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid);
            if (userJxs != null)
            {
                var name = userJxs.Typeid == 4 ? "充值商" : "代理人";
                Alert($"对不起,您已经是{userJxs}");
                return false;
            }
            var userJxsConfirm = await db.TblUserJxsConfirmSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Opnodeid == req.CzsNodeid && p.Status == 0);
            if (userJxsConfirm == null)
            {
                Alert("对不起,您要同意的请求不存在,请重试");
                return false;
            }
            var pUserJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.CzsNodeid && p.Sid == req.Sid);
            if (pUserJxs == null)
            {
                Alert($"对不起,您要同意的充值商不存在");
                return false;
            }
            userJxsConfirm.Status = 1;
            var userJxsConfirms = await db.TblUserJxsConfirmSet.Where(p => p.Nodeid == req.Nodeid && p.Opnodeid != req.CzsNodeid && p.Status == 0).ToListAsync();
            if (userJxsConfirms.Count > 0)
            {
                foreach (var item in userJxsConfirms)
                {
                    item.Status = -1;
                }
            }
            var newJxs = new TblUserJxs
            {
                Pinfoid = pUserJxs.Infoid,
                Nodeid = req.Nodeid,
                Province = pUserJxs.Province,
                City = pUserJxs.City,
                Region = pUserJxs.Region,
                Note = "您当前是" + pUserJxs.City + pUserJxs.Region + "代理人",
                Opnodeid = req.Nodeid,
                Createtime = DateTime.Now,
                Typeid = 5,
                Sid = req.Sid,
                Status = 0,
                Istrain = 1,
                Isfirst = 1,
                Starttime = DateTime.Now,
                Endtime = DateTime.Now.AddDays(90)// new DateTime(2019, 11, 1, 0, 0, 0)
            };
            db.TblUserJxsSet.Add(newJxs);
            if (db.SaveChanges() <= 0)
            {
                Alert("新增代理人失败!");
                return false;
            }
            return true;
        }
        #endregion
        #region PrivateMethods
        /// <summary>
        /// 获取充值商/代理人名称
        /// </summary>
        /// <param name="jxs"></param>
        /// <returns></returns>
        private string GetFbApName(TblUserJxs jxs)
        {
            return jxs.Pinfoid == 0 ? "充值商" : "代理人";
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool Validation(TblUserJxs userJxs, UploadAuthDataReq req)
        {
            if (userJxs == null)
            {
                Alert($"您不是{GetFbApName(userJxs)}，没有开通此功能");
                return false;
            }
            if (userJxs.Status != 1 && req.AuthDatas.Where(a => a.AuthDataType == AuthDataType.Supplement).Any())
            {
                Alert("您暂时不能上传补充资料");
                return false;
            }
            return true;
        }
        private ValueTuple<string, string, string> GenerateFilePath(UploadAuthDataReq req, UploadFile file)
        {
            string fileName = file.Url;
            var physicsFileName = System.Web.Hosting.HostingEnvironment.MapPath(fileName);
            var suffix = Path.GetExtension(physicsFileName);
            string authFilePath = $"/images/authFile/{ req.Nodeid.ToString() + "-" + Guid.NewGuid().ToString()}{suffix}";
            var authFilePhysicsPath = System.Web.Hosting.HostingEnvironment.MapPath(authFilePath);
            var uploadFileDto = new UploadFileDto { Url = authFilePath };
            return (physicsFileName, authFilePhysicsPath, uploadFileDto.FullUrl);
        }
        /// <summary>
        /// 获取上传资料的Url
        /// </summary>
        /// <param name="authDataType">认证资料类型</param>
        /// <param name="imageActionType">图片类型</param>
        /// <param name="userJxs">代理人信息表</param>
        /// <param name="fullUrl">上传资料的Url</param>
        /// <param name="index">多张图片的字段，第一次清空该字段</param>
        /// <returns></returns>
        private ValueTuple<bool, string> GetImageUrl(AuthDataType authDataType, ImageActionType imageActionType, TblUserJxs userJxs, string fullUrl, int index)
        {
            string oldFilePath = "";
            if (authDataType == AuthDataType.Personal)//个人
            {
                if (imageActionType == ImageActionType.IdentFront)//身份证正面照
                {
                    var identFront = string.IsNullOrWhiteSpace(userJxs.PicIdentfront) ? "" : userJxs.PicIdentfront.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(identFront) ? "" : identFront.Substring(identFront.IndexOf(@"/images/authFile"));
                    userJxs.PicIdentfront = fullUrl;//tupleData.fullUrl;
                }
                else if (imageActionType == ImageActionType.IdentBack)//身份证反面照
                {
                    var identBack = string.IsNullOrWhiteSpace(userJxs.PicIdentback) ? "" : userJxs.PicIdentback.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(identBack) ? "" : identBack.Substring(identBack.IndexOf(@"/images/authFile"));
                    userJxs.PicIdentback = fullUrl;//tupleData.fullUrl;
                }
                else if (imageActionType == ImageActionType.Hold)//手持照
                {
                    var hold = string.IsNullOrWhiteSpace(userJxs.PicHold) ? "" : userJxs.PicHold.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(hold) ? "" : hold.Substring(hold.IndexOf(@"/images/authFile"));
                    userJxs.PicHold = fullUrl;//tupleData.fullUrl;
                }
            }
            else if (authDataType == AuthDataType.Company)//公司资料
            {
                if (imageActionType == ImageActionType.Company)//公司照片
                {
                    userJxs.PicCompany = index == 1 ? "" : userJxs.PicCompany;//第一次清空该字段
                    var picCompany = string.IsNullOrWhiteSpace(userJxs.PicCompany) ? "" : userJxs.PicCompany.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(picCompany) ? "" : picCompany.Substring(picCompany.IndexOf(@"/images/authFile"));
                    userJxs.PicCompany += string.IsNullOrWhiteSpace(userJxs.PicCompany) ? fullUrl : "," + fullUrl;//多张照片
                }
                else if (imageActionType == ImageActionType.License)//营业执照
                {
                    var license = string.IsNullOrWhiteSpace(userJxs.PicLicense) ? "" : userJxs.PicLicense.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(license) ? "" : license.Substring(license.IndexOf(@"/images/authFile"));
                    userJxs.PicLicense = fullUrl;
                }
                else if (imageActionType == ImageActionType.Lease)//租赁合同
                {
                    var lease = string.IsNullOrWhiteSpace(userJxs.PicLease) ? "" : userJxs.PicLease.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                    oldFilePath = string.IsNullOrWhiteSpace(lease) ? "" : lease.Substring(lease.IndexOf(@"/images/authFile"));
                    userJxs.PicLease = fullUrl;
                }
            }
            else if (authDataType == AuthDataType.Contract)//合同
            {
                userJxs.PicContract = index == 1 ? "" : userJxs.PicContract;//第一次清空该字段
                var picContract = string.IsNullOrWhiteSpace(userJxs.PicContract) ? "" : userJxs.PicContract.Replace(@"\", @"/").Replace("imgs", "/images/authFile");
                oldFilePath = string.IsNullOrWhiteSpace(picContract) ? "" : picContract.Substring(picContract.IndexOf(@"/images/authFile"));
                userJxs.PicContract += string.IsNullOrWhiteSpace(userJxs.PicContract) ? fullUrl : "," + fullUrl;//多张照片
            }
            else if (authDataType == AuthDataType.Supplement)//补充
            {
                userJxs.PicExt = index == 1 ? "" : userJxs.PicExt;//第一次清空该字段
                var picExt = string.IsNullOrWhiteSpace(userJxs.PicExt) ? "" : userJxs.PicExt.Replace(@"\", @"/").Replace("imgs", "/images/authFile"); ;
                oldFilePath = string.IsNullOrWhiteSpace(picExt) ? "" : picExt.Substring(picExt.IndexOf(@"/images/authFile"));
                userJxs.PicExt += string.IsNullOrWhiteSpace(userJxs.PicExt) ? fullUrl : "," + fullUrl;//多张照片
            }
            else
            {
                Alert("对不起，你上传的资料类型不对，请联系管理员!");
                return (false, "");
            }
            return (true, oldFilePath);
        }
        /// <summary>
        /// 续费流程
        /// </summary>
        /// <param name="uePayHis"></param>
        /// <returns></returns>
        private async Task<ValueTuple<bool, int>> JxsRenew_Pro(TnetUepayhis uePayHis)
        {
            int infoid = int.Parse(uePayHis.BusinessParams);
            var jxs = await db.TblUserJxsSet.FirstOrDefaultAsync(j => j.Infoid == infoid);
            if (jxs == null)
            {
                Alert("获取商家信息失败");
                log.Info(@"JxsRenew_Pro 代理人/充值商续费 获取商家信息失败,infoid={0}", infoid);
                return (false, 0);
            }
            jxs.Starttime = jxs.Endtime < DateTime.Now ? DateTime.Now : jxs.Starttime;
            jxs.Endtime = jxs.Endtime < DateTime.Now ? DateTime.Now.AddYears(1) : jxs.Endtime.AddYears(1);
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("修改商家信息失败");
                log.Info(@"JxsRenew_Pro 代理人/充值商续费,修改tbl_user_jxs失败");
                return (false, 0);
            }
            var userJsxXf = new TblUserJxsXf();
            userJsxXf.Nodeid = uePayHis.Nodeid;
            userJsxXf.Infoid = infoid;
            userJsxXf.Amount = uePayHis.Amount;
            userJsxXf.Createtime = DateTime.Now;
            userJsxXf.Remarks = "续费";
            db.TblUserJxsXfSet.Add(userJsxXf);

            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("记录续费历史失败");
                log.Info(@"JxsRenew_Pro 代理人/充值商续费 记录续费历史失败,infoid={0},nodeid={1)", infoid, uePayHis.Nodeid);
                return (false, 0);
            }
            return (true, jxs.Infoid);
        }
        private async Task<ValueTuple<bool, TnetReginfo>> AddUserJxsCheck(AddDealerReq req)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("用户不存在");
                return (false, null);
            }
            var newUserInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.NewNodeId.ToString() });
            if (newUserInfo == null)
            {
                Alert("要增加的代理人账号不存在");
                return (false, null);
            }
            var newJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.NewNodeId && p.Sid == req.Sid);
            if (newJxs != null && newJxs.Infoid > 0)
            {
                Alert($"{newUserInfo.Nodecode}已经是代理人");
                return (false, null);
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid);
            if (userJxs == null || userJxs.Typeid > 4)
            {
                Alert("充值商不存在");
                return (false, null);
            }
            else if (!(userJxs.Status != 2 && userJxs.Status2 == 0))
            {
                Alert("没有新增代理人权限，请联系客服人员");
                return (false, null);
            }
            return (true, userInfo);
        }
        private async Task<ValueTuple<bool, TnetReginfo>> OpenCzsCheck(OpenCzsReq req)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("用户不存在");
                return (false, null);
            }
            var newUserInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.NewNodeId.ToString() });
            if (newUserInfo == null)
            {
                Alert("要开通的充值商账号不存在");
                return (false, null);
            }
            var newJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.NewNodeId && p.Sid == req.Sid && p.Typeid == 4);
            if (newJxs != null)
            {
                Alert($"{newUserInfo.Nodecode}已经是充值商");
                return (false, null);
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid && p.Typeid == 4);
            if (userJxs == null)
            {
                Alert("充值商不存在");
                return (false, null);
            }
            if (userJxs.Status + userJxs.Status2 != 1)
            {
                Alert($"当前充值商状态是{GetStatusDesc(userJxs)},没有权限");
                return (false, null);
            }
            if (userJxs.Stocknum2 < 50000)
            {
                Alert($"当前充值商零售码不足,请充值");
                return (false, null);
            }
            return (true, userInfo);
        }
        private async Task<ValueTuple<bool, TnetReginfo>> ApplyFbapCheck(ApplyFbapReq req)
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("用户不存在");
                return (false, null);
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid && p.Typeid == 4);
            if (userJxs != null && userJxs.Infoid > 0)
            {
                Alert($"{userInfo.Nodecode}已经是充值商");
                return (false, null);
            }
            return (true, userInfo);
        }
        /// <summary>
        /// 新增代理人流程
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="sid"></param>
        /// <param name="newNodeId"></param>
        /// <returns></returns>
        public async Task<ValueTuple<bool, int>> AddUserJxs_Pro(TnetReginfo userInfo, int sid, int newNodeId)
        {
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == newNodeId && p.Sid == sid);
            if (userJxs != null && userJxs.Infoid > 0)
            {
                Alert("用户已经是代理人");
                return (false, 0);
            }
            userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == userInfo.Nodeid && p.Sid == sid);
            if (userJxs == null || userJxs.Typeid > 4)
            {
                Alert("充值商不存在");
                return (false, 0);
            }
            if (userJxs.Status == 2 || userJxs.Status == 3)
            {
                Alert("充值商已冻结或已锁定");
                return (false, 0);
            }
            var newJxs = new TblUserJxs
            {
                Pinfoid = userJxs.Infoid,
                Nodeid = newNodeId,
                Province = userJxs.Province,
                City = userJxs.City,
                Region = userJxs.Region,
                Note = "您当前是" + userJxs.City + userJxs.Region + "代理人",
                Opnodeid = userInfo.Nodeid,
                Createtime = DateTime.Now,
                Typeid = 5,
                Sid = userJxs.Sid,
                Status = 0,
                Istrain = 1,
                Isfirst = 1,
                Starttime = DateTime.Now,
                Endtime = DateTime.Now.AddDays(90)
            };
            db.TblUserJxsSet.Add(newJxs);
            var saveResult = await db.SaveChangesAsync() > 0;
            if (!saveResult)
            {
                Alert("写代理人失败");
                log.Info("写代理人失败");
                return (false, 0);
            }
            return (true, newJxs.Infoid);
        }
        /// <summary>
        /// 代理人过期状态,0-未过期，1-自已过期，2-上级过期
        /// </summary>
        /// <param name="userJxs"></param>
        /// <returns></returns>
        private int JxsOverdueStatus(TblUserJxs userJxs)
        {
            if (DateTime.Now >= AppConfig.BtsContinueStart)
            {
                var zysModel = db.TblUserJxsSet.FirstOrDefault(p => p.Infoid == userJxs.Pinfoid);
                if (zysModel == null) return 0;
                if (userJxs.Endtime < DateTime.Now)
                {
                    Alert($"{GetFbApName(userJxs)}已过期，请续费后再来兑换");
                    return 1;
                }
                else if (zysModel.Endtime != null && Convert.ToDateTime(zysModel.Endtime) < DateTime.Now)
                {
                    var userInfp = db.TnetReginfoSet.FirstOrDefault(u => u.Nodeid == zysModel.Nodeid);
                    Alert("上级充值商已过期，请联系上级充值商[" + userInfp.Mobileno + "]续费");
                    return 2;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取充值码兑换规则
        /// </summary>
        /// <param name="userJxs"></param>
        /// <returns></returns>
        private List<RechargeCodeRuleInfo> GetRechargeCodeRule(TblUserJxs userJxs)
        {
            var results = new List<RechargeCodeRuleInfo>();
            var targetTime = Convert.ToDateTime("2019-1-1");
            var curTime = DateTime.Now;
            var jxsStockConfig = db.TblUserJxsStockconfigSet
                                   .Where(p => p.Fromtime <= curTime && p.Endtime > curTime && p.Typeid == userJxs.Typeid && p.Isfirst == userJxs.Isfirst).ToList();
            if (jxsStockConfig.Count > 0)
            {
                foreach (var item in jxsStockConfig)
                {
                    results.Add(new RechargeCodeRuleInfo
                    {
                        Id = item.Infoid,
                        RetailCodeStock = item.Lsm,
                        WholesaleCodeStock = item.Pfm,
                        DosPrice = item.Dos,
                        Rate = item.Rate,
                        Title = item.Title,
                        Subtitle = item.Subtitle,
                        IsPromotion = item.Isallowcua > 0,
                    });
                }
            }
            return results;
        }
        private async Task<ValueTuple<bool, TnetReginfo, TblUserJxs, RechargeCodeRuleInfo>> CheckData(ExChangeRechargeCodeReq req, string nodecode = "")
        {
            var userInfo = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.Nodeid.ToString() });
            if (userInfo == null)
            {
                Alert("获取用户基本信息失败");
                return (false, null, null, null);
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid);
            if (userJxs == null)
            {
                Alert($"您不是{GetFbApName(userJxs)},不可以兑换充值值码");
                return (false, null, null, null);
            }
            if (userJxs.Chgtypedate.AddDays(30) < DateTime.Now)//超过新开30天内
            {
                if (nodecode != "ApplyFbap" && nodecode != "OpenFbap")//正常进货时判断状态,applyfbap=申请 ，openfbap=代开
                {
                    var statusDesc = GetStatusDesc(userJxs);
                    if (userJxs.Status != 1 || userJxs.Status2 > 0)
                    {
                        Alert("当前状态是：" + statusDesc + "，不可以兑换充值码，请向充值商咨询。");
                        return (false, null, null, null);
                    }
                }
                if (JxsOverdueStatus(userJxs) != 0)
                {
                    return (false, null, null, null);
                }
            }
            var chgRules = GetRechargeCodeRule(userJxs);
            var rule = chgRules.FirstOrDefault(c => c.Id == req.RuleId);
            if (rule == null)
            {
                Alert("找不到兑换规则,请联系管理员");
                return (false, null, null, null);
            }
            rule.Rate = nodecode == "OpenFbap" ? 0 : rule.Rate;
            return (true, userInfo, userJxs, rule);
        }
        private string GetStatusDesc(TblUserJxs userJxs)
        {
            //状态，Status = 0-未审核，1-审核通过，2-审核拒绝，
            // Status2 = 3 -冻结,4-锁定
            switch (userJxs.Status + userJxs.Status2)
            {
                case 0:
                    {
                        if (!IsUploaded(userJxs))
                        {
                            return "未上传资料";
                        }
                        else
                        {
                            return "等待审核";
                        }
                    }
                case 1:
                    return "审核通过";
                case 2:
                    return "审核拒绝";
                case 3:
                    return "冻结";
                case 4:
                    return "冻结";
                default:
                    return "审核拒绝";
            }
        }
        private bool IsUploaded(TblUserJxs jxs)
        {
            return !(string.IsNullOrEmpty(jxs.PicCompany)
                || string.IsNullOrEmpty(jxs.PicContract)
                || string.IsNullOrEmpty(jxs.PicHold)
                || string.IsNullOrEmpty(jxs.PicIdentback)
                || string.IsNullOrEmpty(jxs.PicLicense)
                || string.IsNullOrEmpty(jxs.PicIdentfront));
        }
        /// <summary>
        /// 获取上级充值商信息
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="rule"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private async Task<ValueTuple<bool, int, TblUserJxs>> GetParentJxsInfo(TblUserJxs userJxs, RechargeCodeRuleInfo rule, ExChangeRechargeCodeReq req)
        {
            var pNodeId = 0;
            var pStockNum = 0;
            var parentJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Infoid == userJxs.Pinfoid);
            pNodeId = parentJxs != null ? parentJxs.Nodeid : -1;
            pStockNum = parentJxs != null ? parentJxs.Stocknum : 999999999;
            if (rule.WholesaleCodeStock + rule.RetailCodeStock > pStockNum)
            {
                log.Info(string.Format("您所属的充值商批发码库存不足:Num={0},Num2={1},StockNum={2},NodeId={3}", rule.RetailCodeStock, rule.WholesaleCodeStock, pStockNum, req.Nodeid));
                Alert("您所属的充值商批发码库存不足，无法兑换");
                return (false, 0, null);
            }
            return (true, pNodeId, parentJxs);
        }
        private async Task<ValueTuple<bool, int>> ExChangeRechargeCode_Pro(TnetReginfo userInfo, int sid, int ruleid, bool isPromotion, string nodecode)
        {
            int nodeId = userInfo.Nodeid;
            log.Info(string.Format("兑换充值码(new):nodeid={0},sid={1},ruleid={2}", nodeId, sid, ruleid));
            var req = new ExChangeRechargeCodeReq() { Nodeid = nodeId, Sid = sid, RuleId = ruleid };
            (bool isSucc, TnetReginfo regInfo, TblUserJxs userJxs, RechargeCodeRuleInfo rule) = await CheckData(req, nodecode);
            if (!isSucc) return (false, 0);
            (bool isSucced, int pNodeId, TblUserJxs jsxParent) = await GetParentJxsInfo(userJxs, rule, req);
            if (!isSucced)
            {
                return (false, 0);
            }
            Business.ExChangeRechargeCode exChangeRechargeCode = new Business.ExChangeRechargeCode(userJxs, userInfo, rule, jsxParent, sid, pNodeId, isPromotion, nodecode);
            if (!exChangeRechargeCode.Execute())
            {
                return (false, 0);
            }
            return (true, exChangeRechargeCode.BusinessId);
        }

        /// <summary>
        /// 获取补充资料
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="result"></param>
        private void GetSupplementData(TblUserJxs userJxs, AuthDataDto result)
        {
            var extPicArray = (string.IsNullOrWhiteSpace(userJxs.PicExt) ? "" : userJxs.PicExt).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/images").Split(',');
            if (!string.IsNullOrWhiteSpace(extPicArray[0]))
            {
                foreach (var extPic in extPicArray)
                {
                    result.ImageUrls.Add(new ImageUrlInfo { Url = extPic });
                }
            }
        }
        /// <summary>
        /// 获取合同认证资料
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="result"></param>
        private void GetContractDataa(TblUserJxs userJxs, AuthDataDto result)
        {
            var contractPicArray = (string.IsNullOrWhiteSpace(userJxs.PicContract) ? "" : userJxs.PicContract).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs").Split(',');
            if (!string.IsNullOrWhiteSpace(contractPicArray[0]))
            {
                foreach (var contractPic in contractPicArray)
                {
                    result.ImageUrls.Add(new ImageUrlInfo { Url = contractPic });
                }
            }
        }
        /// <summary>
        /// 获取公司认证资料
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="result"></param>
        private void GetCompanyData(TblUserJxs userJxs, AuthDataDto result)
        {
            var companyPicArray = (string.IsNullOrWhiteSpace(userJxs.PicCompany) ? "" : userJxs.PicCompany).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs").Split(',');
            if (!string.IsNullOrWhiteSpace(companyPicArray[0]))
            {
                foreach (var companyPic in companyPicArray)
                {
                    result.ImageUrls.Add(new ImageUrlInfo { Url = companyPic, ImageActionType = ImageActionType.Company });
                }
            }
            result.ImageUrls.AddRange(new List<ImageUrlInfo> {
                    new ImageUrlInfo{Url= (string.IsNullOrWhiteSpace(userJxs.PicLicense) ? "" : userJxs.PicLicense).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs"), ImageActionType= ImageActionType.License},
                    new ImageUrlInfo{Url= (string.IsNullOrWhiteSpace(userJxs.PicLease) ? "" : userJxs.PicLease).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs"), ImageActionType= ImageActionType.Lease},
                });
        }
        /// <summary>
        /// 获取个人认证资料
        /// </summary>
        /// <param name="userJxs"></param>
        /// <param name="result"></param>
        private void GetPersonalData(TblUserJxs userJxs, AuthDataDto result)
        {
            result.ImageUrls.AddRange(new List<ImageUrlInfo> {
                    new ImageUrlInfo{Url=  (string.IsNullOrWhiteSpace(userJxs.PicIdentfront) ? "" : userJxs.PicIdentfront).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs"), ImageActionType= ImageActionType.IdentFront},
                    new ImageUrlInfo{Url= (string.IsNullOrWhiteSpace(userJxs.PicIdentback) ? "" :  userJxs.PicIdentback).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs"), ImageActionType= ImageActionType.IdentBack},
                    new ImageUrlInfo{Url=  (string.IsNullOrWhiteSpace(userJxs.PicHold) ? "" : userJxs.PicHold).Replace("imgs", $"{Common.Facade.Helper.DomainUrl}/Bts/imgs"), ImageActionType= ImageActionType.Hold},
                });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="nodeid"></param>
        /// <param name="sid"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private async Task<bool> ApplyFbap_Pro(TnetReginfo targetUser, int nodeid, int sid, string province, string city, string region)
        {
            var curUser = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = nodeid.ToString() });
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == targetUser.Nodeid && p.Sid == sid);
            if (userJxs != null && userJxs.Typeid == 4)
            {
                Alert($"{targetUser.Nodecode}已经是充值商");
                return false;
            }
            if (userJxs != null)
            {
                userJxs.Typeid = 4;
                userJxs.Pinfoid = 0;
                userJxs.Status = 0;
                userJxs.Status2 = 0;
                userJxs.Province = province;
                userJxs.City = city;
                userJxs.Region = region;
                userJxs.Note = "您当前是" + city + region + "充值商";
                userJxs.Opnodeid = curUser.Nodeid;
                userJxs.Isfirst = 1;
                userJxs.Nochecktime = DateTime.Now;
                userJxs.Starttime = DateTime.Now;
                userJxs.Endtime = DateTime.Now.AddDays(90);
                userJxs.Remarks = "";
                userJxs.Chgtypedate = DateTime.Now;
            }
            else
            {
                userJxs = new TblUserJxs();
                userJxs.Pinfoid = 0;
                userJxs.Nodeid = targetUser.Nodeid;
                userJxs.Province = province;
                userJxs.City = city;
                userJxs.Region = region;
                userJxs.Note = "您当前是" + city + region + "充值商";
                userJxs.Opnodeid = curUser.Nodeid;
                userJxs.Createtime = DateTime.Now;
                userJxs.Typeid = 4;
                userJxs.Sid = sid;
                userJxs.Status = 0;
                userJxs.Istrain = 1;
                userJxs.Isfirst = 1;
                userJxs.Starttime = DateTime.Now;
                userJxs.Endtime = DateTime.Now.AddDays(90);
                db.TblUserJxsSet.Add(userJxs);
            }

            if (await db.SaveChangesAsync() <= 0)
            {
                Alert("充值商申请失败!");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查是否有压缩图
        /// </summary>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        private void CheckThumbnail(List<ImageUrlInfo> imageUrls)
        {
            List<Task> taskList = new List<Task>();
            foreach (var item in imageUrls)
            {
                if (!string.IsNullOrWhiteSpace(item.Url))
                {
                    TaskFactory taskFactory = new TaskFactory();
                    taskList.Add(taskFactory.StartNew(() =>
                    {
                        try
                        {
                            Uri baseUri = new Uri(item.Url);
                            var authFilePhysicsPath = System.Web.Hosting.HostingEnvironment.MapPath(baseUri.AbsolutePath);

                            var directory = Path.GetDirectoryName(authFilePhysicsPath);
                            var filename = Path.GetFileNameWithoutExtension(authFilePhysicsPath);
                            var ext = Path.GetExtension(authFilePhysicsPath);
                            var descFile = Path.Combine(directory, $"{filename}_thumbnail{ext}");
                            if (File.Exists(authFilePhysicsPath) && !File.Exists(descFile))
                            {
                                var retry = 3;
                                var result = true;
                                {
                                    result = Common.Facade.Helper.GetPicThumbnail(authFilePhysicsPath, descFile, 90, 100);
                                    retry--;
                                } while (retry > 0 && !result) ;
                            }
                        }
                        catch (FormatException ex)
                        {
                            log.Error($"加载认证资料时压缩出现异常，错误消息：" + ex.Message);
                        }
                    }));
                }
            }
            Task.WaitAll(taskList.ToArray());
        }
        /// <summary>
        /// 代开充值商logic-内测期间
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private async Task<bool> OpenCzsLogic_Beta(OpenCzsReq req, TnetReginfo userInfo)
        {
            //if (!CheckPayPwd(userInfo, req.PayPassword))
            //{
            //    return false;
            //}
            db.BeginTransaction();
            var targetUser = CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = req.NewNodeId.ToString() });
            if (!await ApplyFbap_Pro(targetUser, req.Nodeid, req.Sid, req.Province, req.City, req.Region))//开通充值商
            {
                db.Rollback();
                return false;
            }
            var curTime = DateTime.Now;
            var jxsStockConfig = await db.TblUserJxsStockconfigSet
                              .Where(p => p.Fromtime <= curTime && p.Endtime > curTime && p.Typeid == 4 && p.Isfirst == 1 && p.Isallowcua == 0).ToListAsync();
            if (jxsStockConfig.Count == 0 || jxsStockConfig.Count > 1)
            {
                Alert("申请失败,请联系管理员!");
                return false;
            }
            var ruleId = jxsStockConfig.FirstOrDefault().Infoid;
            (bool isSucc, int businessId) tupleData = await ExChangeRechargeCode_Pro(targetUser, req.Sid, ruleId, false, "OpenFbap");
            if (!tupleData.isSucc)
            {
                db.Rollback();
                return false;
            }
            var userJxs = await db.TblUserJxsSet.FirstOrDefaultAsync(p => p.Nodeid == req.Nodeid && p.Sid == req.Sid && p.Typeid == 4);
            userJxs.Stocknum2 -= 50000;

            var userJxsStockhis2 = new TblUserJxsStockhis2();
            userJxsStockhis2.Infoid = userJxs.Infoid;
            userJxsStockhis2.Typeid = 2;
            userJxsStockhis2.Nodeid = req.NewNodeId;
            userJxsStockhis2.Amount = 50000;//sv库存 
            userJxsStockhis2.Num = 1;
            userJxsStockhis2.Totalamount = userJxsStockhis2.Amount * userJxsStockhis2.Num;
            userJxsStockhis2.Opnodeid = req.Nodeid;
            userJxsStockhis2.Createtime = DateTime.Now;
            userJxsStockhis2.Remarks = "充值商代开充值商";
            db.TblUserJxsStockhis2Set.Add(userJxsStockhis2);
            if (!(await db.SaveChangesAsync() > 0))
            {
                Alert("开通失败!");
                db.Rollback();
                return false;
            }
            return db.Commit();
        }

        /// <summary>
        /// 获取ue账号信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="payType"></param>
        /// <returns>1:成功，-1：异常，-2：未绑定</returns>
        private int GetUeInfo(int nodeId, int payType)
        {
            try
            {
                var payConfig = db.TpcnUepayconfigSet.Where(w => w.Typeid == payType).FirstOrDefault();
                if (payConfig == null)
                {
                    log.Info($"没有查询到此支付配置,payType:{payType}");
                    Alert("支付配置不正确");
                    return -1;
                }
                var user = PxinCache.GetRegInfo(nodeId);
                if (user == null)
                {
                    Alert("获取用户基本信息失败");
                    return -1;
                }
                var result = Common.UEPay.UeApi.GetUeUserInfo(payConfig.Accesskeyid, payConfig.Accesssecret, "6", user.Nodecode);
                log.Info("ue返回的用户信息：" + JsonConvert.SerializeObject(result));
                return result.Result > 0 ? 1 : -2;
            }
            catch (Exception e)
            {
                log.Error("获取ue用户基本信息失败：" + e);
                Alert("获取ue用户基本信息失败");
                return -1;
            }
        }
        /// <summary>
        /// 代开充值商生成2张10000额度的SVC充值码
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="uePayHisId"></param>
        /// <returns></returns>
        private async Task<bool> GenerateCzm(int nodeId, int uePayHisId)
        {
            for (int i = 0; i < 2; i++)
            {
                TblcCentcard tc = new TblcCentcard
                {
                    Cardno = PXinContext.GetCentcard(db),
                    Cardpwd = "0",
                    Ispwdrequired = 0,
                    Amount = 10000,
                    Expiredtime = DateTime.Now.AddYears(10),
                    Createdtime = DateTime.Now,
                    Areaid = "1",
                    Status = 1,
                    Usenodeid = nodeId,
                    Remarks = "代开充值商生成SVC充值码",
                    Fromid = 7,
                    Period = "0",
                    Productid = uePayHisId,
                };
                db.TblcCentcardSet.Add(tc);
                if (await db.SaveChangesAsync() <= 0)
                {
                    Alert("生成SVC充值码失败");
                    return false;
                }
                TblcCentcardHis his = new TblcCentcardHis
                {
                    Idno = tc.Idno,
                    Nodeid = nodeId,
                    Typeid = 9,
                    Note = "代开充值商生成SVC充值码",
                    Createtime = DateTime.Now,
                    Opnodeid = nodeId,
                    Remarks = "代开充值商生成SVC充值码"
                };
                db.TblcCentcardHisSet.Add(his);
                if (await db.SaveChangesAsync() <= 0)
                {
                    Alert("生成SVC充值码记录失败");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取今天还能生成充值码数量
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private decimal GetTodaySvcLimit(int nodeid)
        {
            var now = DateTime.Now.Date;
            var sum = db.TblcCentcardSet.Where(c => c.Createdtime >= now && c.Fromid == 6 && c.Assignnodeid == nodeid).ToList().Sum(c => c.Amount);
            var total = db.TpxinSvcLimitSet.Where(c => c.Nodeid == nodeid && c.Fromtime <= DateTime.Now && c.Endtime >= DateTime.Now).Select(c => c.Totalamount - c.Localamount1).FirstOrDefault();
            if (sum >= 10000)
            {
                return total;
            }
            else
            {
                return 10000 - (decimal)sum + total;
            }
        }
        #endregion
    }
}
