using Common.Facade;
using Common.Facade.Models;
using Common.Mvc.Models;
using MvcPaging;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Facade.UEPay;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winner.CU.Balance.Entities;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.Balance.PurseFactory;
using Winner.CU.BalanceWcfClient;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 表情包
    /// </summary>
    public class EmoticonFacade : FacadeBase<PXinContext>
    {

        private int _transferid;

        /// <summary>
        /// 获取热门表情包名字
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<string> GetHotEmoticons(HotEmoticonsReq req)
        {
            return db.TpxinEmoticonSearchSet.OrderByDescending(c => c.Times).ThenBy(c => c.Infoid).Take(8).Select(c => c.Showname).ToList();
        }

        /// <summary>
        /// 获取表情单品
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<EmoticonsDto> GetSingleEmoticons(PageBase req)
        {
            var emoticon = GetEmoticonsEFResult(req.Nodeid, 2).ToPagedList(req.PageNum, req.PageSize).ToList();
            return emoticon;
        }

        /// <summary>
        /// 获取表情包
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<EmoticonsDto> GetEmoticons(PageBase req)
        {
            var emoticon = GetEmoticonsEFResult(req.Nodeid, 1).ToPagedList(req.PageNum, req.PageSize).ToList();
            return emoticon;
        }

        /// <summary>
        /// 搜索表情包
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<EmoticonsDto> SearchEmoticons(SearchEmoticonsReq req)
        {
            var emoticon = GetEmoticonsEFResult(req.Nodeid, req.Typeid).Where(c => c.Name.Contains(req.EmoticonName)).ToPagedList(req.PageNum, req.PageSize).ToList();
            AddDarenSearch(req.EmoticonName);
            return emoticon;
        }

        /// <summary>
        /// 下载表情
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ValueTuple<bool, string, string> DownloadEmoticon(int nodeid, int id)
        {
            var user = db.TnetReginfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            if (user == null)
            {
                Alert("用户不存在");
                return (false, "", null);
            }

            var emoticon = db.TpxinEmoticonMaterialSet.Where(c => c.Id == id).FirstOrDefault();
            if (emoticon == null)
            {
                Alert("表情包不存在");
                return (false, "", null);
            }

            var path = System.Web.Hosting.HostingEnvironment.MapPath(emoticon.Filedir);
            if (!File.Exists(path))
            {
                Alert("表情包不存在,请联系客服" + path);
                return (false, "", null);
            }

            var order = db.TpxinEmoticonOrderSet.Where(c => c.Nodeid == nodeid && c.Materialid == id).FirstOrDefault();
            
            if (order == null)
            {
                //Alert("未购买，请先购买再进行下载");
                if (emoticon.Price <= 0)
                {
                    db.BeginTransaction();
                    if (!DownloadEmoticon_Pro(nodeid, emoticon.Price, emoticon))
                    {
                        Alert("生成表情订单失败");
                        db.Rollback();
                        return (false, "", null);
                    }

                    db.Commit();
                }
                else
                {
                    Alert("请先购买再下载");
                    return (false, "", null);
                }

            }

            
            return (true, emoticon.Name, path);
        }

        /// <summary>
        /// 购买表情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ValueTuple<bool, OpenInfUeoDto> BuyEmoticon(DownloadEmoticonReq req)
        {
            var result = Check_BuyEmoticon(req);
            if (!result.Item1)
            {
                return (false,null);
            }

            var emoticon = result.Item2;
            if (emoticon.Configid == 11)
            {
                var uesign = GetUESignInfo(req.Nodeid,emoticon.Price, emoticon.Id);
                Alert("调起ueapp",99);
                return (true, uesign);
            }

            db.BeginTransaction();
            BeginTransfer();

            if (emoticon.Price > 0)
            {
                if (emoticon.Configid < 0)
                {
                    if (!Transfer_EmoticonPayPV(req.Nodeid, emoticon.Price, emoticon.Configid))
                    {
                        db.Rollback();
                        EndTransfer(false);
                        return (false, null);
                    }
                }
                else
                {
                    if (!Transfer_EmoticonPay(req.Nodeid, emoticon.Price, emoticon.Configid))
                    {
                        db.Rollback();
                        EndTransfer(false);
                        return (false, null);
                    }
                }

            }

            if (!DownloadEmoticon_Pro(req.Nodeid, emoticon.Price, emoticon))
            {
                db.Rollback();
                EndTransfer(false);
                return (false,null);
            }

            db.Commit();
            Alert("支付成功", 1);
            return (true, null);
        }

        /// <summary>
        /// 购买表情-支付回调
        /// </summary>
        /// <param name="uepayhis"></param>
        /// <returns></returns>
        public bool BuyEmoticon_Notice(TnetUepayhis uepayhis)
        {
            var userInfo = PxinCache.GetRegInfo(uepayhis.Nodeid);
            if (userInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            int.TryParse(uepayhis.BusinessParams, out int id);

            var emotion = db.TpxinEmoticonMaterialSet.Where(c => c.Id == id).FirstOrDefault();
            var order = db.TpxinEmoticonOrderSet.Where(c => c.Materialid == id && c.Nodeid == userInfo.Nodeid).FirstOrDefault();
            if (emotion == null)
            {
                Alert("要购买的表情不存在");
                return false;
            }
            if (order != null)
            {
                Alert("重复购买");
                return false;
            }

            try
            {
                db.BeginTransaction();
                if (!DownloadEmoticon_Pro(userInfo.Nodeid, uepayhis.Amount, emotion))
                {
                    Alert("生成表情订单失败");
                    db.Rollback();
                    return false;
                }

                db.Commit();
                return true;
            }
            catch (Exception e)
            {
                log.Error("购买表情-支付回调异常：" + e);
                Alert("购买表情-支付回调异常：" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取我的表情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<EmoticonsDto> GetMyEmoticons(Reqbase req)
        {
            return (from tm in db.TpxinEmoticonMaterialSet
                   join th in db.TpxinEmoticonOrderSet.Where(c => c.Nodeid == req.Nodeid) on tm.Id equals th.Materialid into thdata
                   from thd in thdata.DefaultIfEmpty()
                   orderby tm.Id descending 
                   select new EmoticonsDto
                   {
                       ID = tm.Id,
                       Author = tm.Author,
                       Filesize = tm.Filesize,
                       Intr = tm.Intr,
                       Name = tm.Name,
                       Price = tm.Price,
                       Url = tm.Url,
                       IsDownload = thd == null ? 1 : 0,
                       Typeid=tm.Typeid
                   }).ToList();
        }

        /// <summary>
        /// 获取表情包详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<EmoticonDetailDto> GetEmoticonMaterialDetail(EmoticonMaterialDetailReq req)
        {
            var emoticon = db.TpxinEmoticonMaterialSet.Where(c => c.Id == req.ID && c.Typeid == 1).FirstOrDefault();
            if (emoticon == null)
            {
                Alert("表情包不存在");
                return null;
            }

            var result= ImageList(emoticon.Filedir);
            if (!result.Item1)
            {
                Alert("未找到表情包内容");
                return null;
            }
            if(result.Item2.Count!= result.Item3.Count)
            {
                Alert("表情包数据有误");
                return null;
            }

            List<EmoticonDetailDto> dtos = new List<EmoticonDetailDto>();
            foreach (var item in result.Item2)
            {
                EmoticonDetailDto dto = new EmoticonDetailDto();
                dto.ImageGifUrl = item;
                dto.ImagePngUrl = item.Replace(".gif", "_bt.png");//result.Item3.Find(c => c.Contains(item.Split('.')[3]));
                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 查询ue支付是否成功
        /// </summary>
        /// <returns></returns>
        public bool VerifyPayResult(VerifyPayReq req)
        {

            var result = db.TnetUepayhisSet.Where(c => c.Id == req.OrderNo).FirstOrDefault();
            if (result==null || result.Status!=2)
            {
                Alert("订单不存在或未处理");
            }

            if(result.Status == 2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 生成描述文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateShowNameFile(CreateShowReq req)
        {
            var emoticon = db.TpxinEmoticonMaterialSet.Where(c => c.Id == req.ID && c.Typeid == 1).FirstOrDefault();
            if (emoticon == null)
            {
                Alert("表情包不存在");
                return false;
            }
            CreateShowNameFile_Pro(emoticon.Filedir, req.Name,req.Typeid);

            return true;
        }

        #region[Private]

        /// <summary>
        /// 表情库基础数据
        /// </summary>
        /// <param name="nodeid">用户的nodeid</param>
        /// <param name="typeid">-1=搜索都选择，1-表情包，2-表情单品</param>
        /// <returns></returns>
        private IQueryable<EmoticonsDto> GetEmoticonsEFResult(int nodeid, int typeid)
        {
            IQueryable<TpxinEmoticonMaterial> tmd;
            if (typeid == -1)
            {
                tmd = db.TpxinEmoticonMaterialSet;
            }
            else
            {
                tmd = db.TpxinEmoticonMaterialSet.Where(c => c.Typeid == typeid);
            }

            var result= from tm in tmd
                   join tc in db.TnetPurseConfigSet on tm.Configid equals tc.Infoid into tcdata
                   from tcd in tcdata.DefaultIfEmpty()
                   join th in db.TpxinEmoticonOrderSet.Where(c => c.Nodeid == nodeid) on tm.Id equals th.Materialid into thdata
                   from thd in thdata.DefaultIfEmpty()
                   orderby tm.Createtime descending, tm.Id ascending
                   select new EmoticonsDto
                   {
                       ID = tm.Id,
                       Typeid=tm.Typeid,
                       Author = tm.Author,
                       Filesize = tm.Filesize,
                       Intr = tm.Intr,
                       Name = tm.Name,
                       Price = tm.Price,
                       Url = tm.Url,
                       PurseName = tcd.Showname,
                       IsDownload = thd == null ? 0 : 1,
                       SendPrice=tm.Sendprice
                   };

            return result;
        }

        /// <summary>
        /// 购买表情包
        /// </summary>
        /// <param name="nodeid">用户nodeid</param>
        /// <param name="price">价格</param>
        /// <param name="config">购买钱包配置id</param>
        /// <returns></returns>
        private bool Transfer_EmoticonPay(int nodeid, decimal price, int config)
        {
            var purseconfig = db.TnetPurseConfigSet.Where(c => c.Infoid == config).FirstOrDefault();
            if (purseconfig == null)
            {
                Alert("支付配置不存在");
                return false;
            }

            CurrencyType type = new CurrencyType(purseconfig.Currencytype);
            Purse toPurse = purseFactory.SystemPurseRand(nodeid);
            Purse fromPurse = new Purse(OwnerType.个人钱包, nodeid, (PurseType)purseconfig.Pursetype, type, wcfProxy);
            Currency currency = new Currency(type, price);

            TransferResult tResult = wcfProxy.Transfer(fromPurse, toPurse, currency, 13, "购买表情包");
            if (!tResult.IsSuccess)
            {
                Alert("购买失败:" + tResult.Message);
                return false;
            }
            _transferid = tResult.TransferId;
            return true;
        }

        /// <summary>
        /// 购买表情-v点p点支付
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="price"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private bool Transfer_EmoticonPayPV(int nodeid, decimal price, int config)
        {
            var user = db.TpxinUserinfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            if (user == null)
            {
                Alert("用户不存在");
                return false;
            }

            if (config == -1)
            {
                if (!Transfer_PayV(nodeid, price, 5003, "购买表情包"))
                {
                    return false;
                }
            }
            else
            {
                Alert("暂不支持的支付方式");
            }



            //var before = 0M;
            //var after = 0M;
            //if (config == -1)
            //{
            //    if (user.V < price)
            //    {
            //        Alert("余额不足");
            //        return false;
            //    }
            //    before = user.V;
            //    user.V -= price;
            //    after = user.V;
            //}
            //else
            //{
            //    if(user.P< price)
            //    {
            //        Alert("余额不足");
            //        return false;
            //    }
            //    before = user.P;
            //    user.P -= price;
            //    after = user.P;
            //}

            //var his = new TpxinAmountChangeHis
            //{
            //    Amount = -price,
            //    Amountbefore = before,
            //    Amountafter = after,
            //    Createtime = DateTime.Now,
            //    Nodeid = nodeid,
            //    Reason = 5003,
            //    Transferid = Guid.NewGuid().ToString(),
            //    Typeid = config == -1 ? 1 : 2,
            //    Remarks = "购买表情包"
            //};
            //db.TpxinAmountChangeHisSet.Add(his);

            //if (db.SaveChanges() <= 0)
            //{
            //    Alert("操作失败");
            //    return false;
            //}

            //_transferid = his.Hisid;

            return true;
        }

        /// <summary>
        /// 订单记录
        /// </summary>
        /// <returns></returns>
        private bool DownloadEmoticon_Pro(int nodeid, decimal price, TpxinEmoticonMaterial emoticon)
        {
            db.TpxinEmoticonOrderSet.Add(new TpxinEmoticonOrder
            {
                Amount = price,
                Createtime = DateTime.Now,
                Nodeid = nodeid,
                Transferid = _transferid.ToString(),
                Materialid = emoticon.Id
            });

            emoticon.Buycount += 1;

            if (db.SaveChanges() <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取表情包下对应的列表文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ValueTuple<bool,List<string>, List<string>>  ImageList(string path)
        {

            try
            {
                string FilePath ="/" + string.Join("/",path.Replace("/images2/", "").Split('/').Take(2));
                string from = Path.GetDirectoryName(System.Web.Hosting.HostingEnvironment.MapPath(path));
                string urlbase = AppConfig.ImageBaseUrl;
                if (!Directory.Exists(from))
                {
                    Alert("表情包不存在");
                    return (false,null,null);
                }
                DirectoryInfo root = new DirectoryInfo(from);
                FileInfo[] files = root.GetFiles();
                List<string> file1 = new List<string>();
                List<string> file2 = new List<string>();

                foreach (var item in files)
                {
                    var filename = urlbase + FilePath + "/" + item.Name;
                    if (item.Extension == ".gif")
                    {
                        file1.Add(filename);
                    }
                    else if(item.Extension == ".png")
                    {
                        if (item.Name.IndexOf("bt.png")>0)
                        {
                            file2.Add(filename);
                        }
                    }

                }

                return (true,file1, file2);
            }
            catch (Exception e)
            {
                log.Info(e.ToString());
                return (false,null, null);
            }
        }

        #region[生成描述部分]
        /// <summary>
        /// 生成对应的描述文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">描述的名字字符串，逗号分割</param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        private void CreateShowNameFile_Pro(string path, string name,int typeid)
        {
            try
            {
                string from = Path.GetDirectoryName(System.Web.Hosting.HostingEnvironment.MapPath(path));
                if (!Directory.Exists(from))
                {
                    Alert("表情包不存在");
                    return;
                }
                DirectoryInfo root = new DirectoryInfo(from);
                FileInfo[] files = root.GetFiles();
                List<string> file3 = new List<string>();

                foreach (var item in files)
                {
                    if (item.Extension == ".gif")
                    {
                        file3.Add(item.Name.Replace(item.Extension, ""));
                    }
                }

                var dic = new Dictionary<string, string>();
                var ss = name.Split(',');
                int i = 0;
                if (typeid == 0)
                {
                    file3 = file3.OrderBy(c => c).ToList();
                }
                else if(typeid == 1)
                {
                    file3 = file3.OrderBy(c => int.Parse(c.Split('_')[0])).ToList();
                }
                else
                {
                    file3 = file3.OrderBy(c => int.Parse(c.Split('_')[1])).ToList();
                }

                
                foreach (var item in file3)
                {
                    dic.Add(item, ss[i]);
                    i++;
                }

                var data = JsonWinner.SerializeObject(dic);
                WriteFileEmoction(from + "/ShowNameJson.txt", data);
            }
            catch (Exception e)
            {
                log.Info(e.ToString());
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class FileDto
        {
            /// <summary>
            /// 
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ShowName { get; set; }
        }

        private bool WriteFileEmoction(string path, string content)
        {
            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
            using (FileStream fsWrite = new FileStream(path, FileMode.Create))
            {
                fsWrite.Write(myByte, 0, myByte.Length);
            };

            return true;
        }

        #endregion

        /// <summary>
        /// 获取调起ue参数
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="price"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private OpenInfUeoDto GetUESignInfo(int nodeid,decimal price,int id)
        {
            var userInfo = PxinCache.GetRegInfo(nodeid);
            if (userInfo == null)
            {
                Alert("获取用户基本信息失败");
                return null;
            }
            var result = UePayHelper.DosWithUePay(db, userInfo, 1, CurrencyType.DOS_矿沙, price, 20004, id.ToString(), "购买表情", "购买表情").Result;
            if (!result.IsSuccess)
            {
                Alert(result.Message);
                return null;
            }
            return new OpenInfUeoDto
            {
                ChargeStr = result.ChargeStr,
                Sign = Common.Mvc.Md5.SignString(result.ChargeStr + AppConfig.AppSecurityString).ToUpper(),
                OrderNo = result.OrderNo
            };
        }

        /// <summary>
        /// 购买表情检查
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private ValueTuple<bool, TpxinEmoticonMaterial> Check_BuyEmoticon(DownloadEmoticonReq req)
        {
            var emoticon = db.TpxinEmoticonMaterialSet.Where(c => c.Id == req.ID).FirstOrDefault();
            if (emoticon == null)
            {
                Alert("表情包不存在");
                return (false, null);
            }
            //if (emoticon.Configid != 11)
            //{
            //    if (!CheckPwd(req.Nodeid, req.Pwd))
            //    {
            //        return (false,null);
            //    }
            //}
            var path = System.Web.Hosting.HostingEnvironment.MapPath(emoticon.Filedir);
            if (!File.Exists(path))
            {
                Alert("表情包不存在");
                return (false, null);
            }
            var order = db.TpxinEmoticonOrderSet.Where(c => c.Nodeid == req.Nodeid && c.Materialid == req.ID).FirstOrDefault();
            if (order != null)
            {
                //重复下载直接通过
                Alert("再次下载", 2);
                return (false, null);
            }

            return (true, emoticon);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CheckPwd(int nodeid, string pwd)
        {
            var regInfo = PxinCache.GetRegInfo(nodeid);
            if (regInfo == null)
            {
                Alert("获取用户信息失败");
                return false;
            }
            if (!CheckPayPwd(regInfo, pwd, false))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 搜索增加热门关键字次数
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private bool AddDarenSearch(string keys)
        {
            var result = db.TpxinEmoticonSearchSet.Where(c => c.Showname == keys).FirstOrDefault();
            if (result == null)
            {
                result = new TpxinEmoticonSearch
                {
                    Createtime = DateTime.Now,
                    Showname = keys,
                    Times = 1
                };
                db.TpxinEmoticonSearchSet.Add(result);
            }
            else
            {
                result.Times++;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("操作失败");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 支付v点
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amonut"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool Transfer_PayV(int nodeid, decimal amonut, int reason, string remarks)
        {
            var vp = new VPHelper();
            var result = vp.SetV(new VPChargeVDian
            {
                Amount = -amonut,
                Nodeid = nodeid,
                Reason = reason,
                Remark = remarks,
                Transferid = Guid.NewGuid().ToString()
            });

            return result.Result > 0 ? true : false;
        }

        #endregion



    }
}
