using Common.Facade;
using Common.Facade.Models;
using PXin.DB;
using PXin.Facade.CommonService;
using PXin.Facade.Models.UserDto;
using PXin.Facade.Models.UserReq;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 实名认证
        /// </summary>
        public bool IdCardAuth(IdAuthReq req)
        {
            req.Pic1 = "/" + req.Pic1.Replace(AppConfig.ImageBaseUrl, "").TrimStart('/');
            req.Pic3 = "/" + req.Pic3.Replace(AppConfig.ImageBaseUrl, "").TrimStart('/');
            var regInfo = db.TnetReginfoSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            var authLog = db.TzcAuthLogSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            var nodeInfo = db.TnetNodeinfoSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (regInfo.Isconfirmed == 1 && authLog != null && nodeInfo != null && (authLog.Status == 1 || authLog.Status == 5))
            {
                return Fail("已通过实名认证，不要重复认证");
            }
            IdCardService idCardService = new IdCardService();
            IdentResult recResult1 = idCardService.GetRecResult(req.Pic1);
            if (recResult1 == null)
            {
                return Fail("未找到身份证正面照识别结果");
            }
            IdentResult recResult2 = idCardService.GetRecResult(req.Pic3);
            if (recResult2 == null)
            {
                return Fail("未找到身份证反面照识别结果");
            }

            var res = ValidateAuthRegInfo(req.Nodeid, recResult1, authLog);//验证信息
            if (!res)
            {
                return false;
            }

            DateTime myDate = DateTime.Now;
            TzcAuthLog newAuthLog = new TzcAuthLog();
            if (authLog != null)
            {
                newAuthLog = authLog;
                myDate = authLog.Createtime;
            }
            newAuthLog.Nodeid = regInfo.Nodeid;
            newAuthLog.Createtime = DateTime.Now;
            newAuthLog.Remarks = "用户认证";
            newAuthLog.Realname = req.IdCardName.IsNullOrWhiteSpace() ? recResult1.cards[0].name : req.IdCardName;

            try
            {
                newAuthLog.Birthday = Convert.ToDateTime(recResult1.cards[0].birthday);
            }
            catch (Exception err)
            {
                log.Info(recResult1.cards[0].birthday + "转换为生日失败，" + err.ToString());
            }

            newAuthLog.Race = recResult1.cards[0].race;
            newAuthLog.Sex = recResult1.cards[0].gender == "女" ? 2 : 1;
            newAuthLog.Idcard = req.IdCardNum.IsNullOrWhiteSpace() ? recResult1.cards[0].id_card_number : req.IdCardNum;
            newAuthLog.Address = recResult1.cards[0].address;
            newAuthLog.IssuedBy = recResult2.cards[0].issued_by;
            newAuthLog.ValidDate = recResult2.cards[0].valid_date;

            //分解身份证身日性别
            if (newAuthLog.Idcard.Length == 18 && !newAuthLog.Birthday.HasValue)
            {
                //（1）前1、2位数字表示：所在省份的代码； 
                //（2）第3、4位数字表示：所在城市的代码； 
                //（3）第5、6位数字表示：所在区县的代码； 
                //（4）第7~14位数字表示：出生年、月、日； 
                //（5）第15、16位数字表示：所在地的派出所的代码； 
                //（6）第17位数字表示性别：奇数表示男性，偶数表示女性； 
                //（7）第18位数字是校检码：也有的说是个人信息码，用来检验身份证的正确性。校检码可以是0~9的数字，有时也用x表示(尾号是10，那么就得用X来代替)。 一般是随计算机的随机产生。
                newAuthLog.Sex = Convert.ToInt32(newAuthLog.Idcard.Substring(16, 1)) % 2 == 0 ? 2 : 1;
                newAuthLog.Birthday = new DateTime(Convert.ToInt32(newAuthLog.Idcard.Substring(6, 4)),
                    Convert.ToInt32(newAuthLog.Idcard.Substring(10, 2)),
                    Convert.ToInt32(newAuthLog.Idcard.Substring(12, 2)));
            }
            FileService fileService = new FileService();
            newAuthLog.Idcardpic1 = fileService.CombinePicUrl(req.Pic1, myDate, FileActionType.身份证正面图片);
            newAuthLog.Idcardpic3 = fileService.CombinePicUrl(req.Pic3, myDate, FileActionType.身份证反面图片);

            newAuthLog.Isidentify = 1;
            newAuthLog.Baiduapiret = "0";
            newAuthLog.Status = 0; //通过实人认证，不需要再审核了
            //添加或更新认证日志
            if (authLog == null)
            {
                db.TzcAuthLogSet.Add(newAuthLog);
            }
            else
            {
                db.TzcAuthLogSet.Attach(newAuthLog);
                db.Entry(newAuthLog).State = EntityState.Modified;
            }
            //更新tnet_nodeinfo表
            bool isExistnodeInfo = nodeInfo != null;
            if (nodeInfo == null)
            {
                nodeInfo = new TnetNodeinfo();
            }
            BuildNodeInfo(nodeInfo, newAuthLog);
            if (!isExistnodeInfo)
            {
                db.TnetNodeinfoSet.Add(nodeInfo);
            }
            else
            {
                db.TnetNodeinfoSet.Attach(nodeInfo);
                db.Entry(nodeInfo).State = EntityState.Modified;
            }
            if (regInfo.Isconfirmed == 0)
            {
                regInfo.Isconfirmed = 1;
                regInfo.Nodename = newAuthLog.Realname;
                regInfo.Authtime = DateTime.Now;

                if (db.Entry(regInfo).State == EntityState.Detached)
                {
                    db.TnetReginfoSet.Attach(regInfo);
                    db.Entry(regInfo).State = EntityState.Modified;
                }
            }
            int scnt = db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 获取实名认证信息
        /// </summary>
        /// <returns></returns>
        public IdCardAuthDto GetIdCardAuth(Reqbase req)
        {
            var authLog = db.TzcAuthLogSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (authLog != null)
            {
                IdCardAuthDto idCardAuthDto = new IdCardAuthDto();
                idCardAuthDto.Pic1 = authLog.Idcardpic1;
                idCardAuthDto.Pic3 = authLog.Idcardpic3;
                idCardAuthDto.IdCardName = authLog.Realname;
                idCardAuthDto.IdCardNum = authLog.Idcard;
                idCardAuthDto.Status = authLog.Status;
                idCardAuthDto.Reason = authLog.Remarks;
                return idCardAuthDto;
            }
            else
            {
                Alert("用户上传身份证照片");
                return null;
            }
        }

        private void BuildNodeInfo(TnetNodeinfo nodeInfo, TzcAuthLog authLog)
        {
            nodeInfo.Nodeid = authLog.Nodeid;
            nodeInfo.Idcardno = authLog.Idcard;
            nodeInfo.Name = authLog.Realname;
            nodeInfo.Birthday = authLog.Birthday.Value;
            nodeInfo.Sex = authLog.Sex == 1 ? 1 : 0;
            if (!string.IsNullOrEmpty(authLog.Race))
            {
                nodeInfo.Nation = authLog.Race;
                nodeInfo.Idcardaddr = authLog.Address;
                nodeInfo.Issuing = authLog.IssuedBy;
                string[] valid_date = authLog.ValidDate.Split('-');
                if (valid_date != null && valid_date.Length == 2)
                {
                    nodeInfo.Beginvalidity = Convert.ToDateTime(valid_date[0].Replace(".", "-"));
                    if (valid_date[1].Trim() == "长期")
                    {
                        nodeInfo.Endvalidity = new DateTime(2099, 1, 1);
                    }
                    else
                    {
                        nodeInfo.Endvalidity = Convert.ToDateTime(valid_date[1].Replace(".", "-"));
                    }
                }
            }
        }

        private bool ValidateAuthRegInfo(int nodeid, IdentResult recResult1, TzcAuthLog authLog)
        {
            string idcardnumber = recResult1.cards[0].id_card_number;
            var cntExistIdcard = db.TzcAuthLogSet.Count(x => x.Idcard == idcardnumber && x.Nodeid != nodeid);
            if (cntExistIdcard > 0)
            {
                log.Info("身份证[" + idcardnumber + "]已存在,nodeid=" + nodeid);
                return Fail("身份证[" + GetMaskIdCard(idcardnumber) + "]已存在");
            }
            return true;
        }

        private string GetMaskIdCard(string idcard, bool isMask = true)
        {
            if (string.IsNullOrEmpty(idcard) || idcard.Length < 15)
            {
                return string.Empty;
            }
            string retidcard = idcard;
            if (isMask)
            {
                retidcard = idcard.Substring(0, 6) + "********" + idcard.Substring(idcard.Length - 4, 4);
            }
            return retidcard;
        }

        /// <summary>
        /// 保存驾驶证认证数据
        /// </summary>
        public bool SaveDriveLicense(DriveLicenseReq req)
        {
            req.Pic1 = "/" + req.Pic1.Replace(AppConfig.ImageBaseUrl, "").TrimStart('/');
            req.Pic2 = "/" + req.Pic2.Replace(AppConfig.ImageBaseUrl, "").TrimStart('/');
            //1.判断用户账号是否存在
            var tnet_reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (req.Fileno == "0")
            {
                return Fail("参数错误");
            }
            BaiduLicense license = new BaiduLicense();
            bool isSuccess = license.DrivLicense(tnet_reginfo, req.Pic1, req.Pic2, req.Fileno);
            Alert(license.PromptInfo.Message);
            return isSuccess;
        }

        /// <summary>
        /// 获取驾驶证认证信息
        /// </summary>
        /// <returns></returns>
        public DriveLicenseDto GetDriveLicense(Reqbase req)
        {
            var driveLicense = db.TnetDriveLicLogSet.FirstOrDefault(x => x.Nodeid == req.Nodeid);
            if (driveLicense != null)
            {
                DriveLicenseDto driveLicenseDto = new DriveLicenseDto();
                driveLicenseDto.Pic1 = driveLicense.Cardimg;
                driveLicenseDto.Pic2 = driveLicense.CardimgAppendix;
                driveLicenseDto.Fileno = driveLicense.Fileno;
                driveLicenseDto.Status = driveLicense.Status;
                return driveLicenseDto;
            }
            else
            {
                Alert("用户未上传驾驶证认证信息");
                return null;
            }
        }
    }
}
