using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Common.Facade;
using Common.Mvc.Models;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Model;
using SmsCenter.SmsServiceClient;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 
    /// </summary>
    public class SmsFacade : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="bTypeid">业务类型，0-全部，1-手机号已注册发送短信，2-手机号未注册发送短信</param>
        /// <param name="mobileno"></param>
        /// <param name="content"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public bool SendSms(int typeId, int bTypeid, string mobileno, string content, int sid = 0)
        {
            log.Info("类型：" + typeId + ",业务类型:" + bTypeid + ",手机号：" + mobileno + ",短信内容：" + content);
            if (bTypeid > 0)
            {
                int counter = db.TnetReginfoSet.Count(c => c.Mobileno == mobileno);
                if (counter > 0 && bTypeid == 2)
                {
                    Alert("该手机号码已注册");
                    return false;
                }
                else if (counter == 0 && bTypeid == 1)
                {
                    Alert("该手机号码未注册");
                    return false;
                }
            }
            if (typeId == 0)
            {
                return SendCommonSms(mobileno, content, null);
            }
            if (mobileno.Substring(0, 1) == "+")
            {
                //国际短信
                typeId = typeId - 1;
                mobileno = mobileno.Substring(1);
            }
            TsmsAlitemplate smsAlitemplate = db.TsmsAlitemplateSet.AsNoTracking().FirstOrDefault(c => c.Id == typeId);
            if (smsAlitemplate == null)
            {
                Alert("短信类型错误,找不到发送短信模板");
                return false;
            }
            if (string.IsNullOrEmpty(content))
            {
                content = BuilderSmsContent(typeId, mobileno, sid, smsAlitemplate.Tmpval);
            }
            if (string.IsNullOrEmpty(content))
            {
                Alert("生成短信内容失败");
                return false;
            }
            if (!SendAliYunSms(mobileno, smsAlitemplate.Tmpcode, content))
            {
                return false;
            }
            Alert("发送短信成功", 1);
            return true;
        }
        /// <summary>
        /// 生成短信内容
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="mobileno"></param>
        /// <param name="sid"></param>
        /// <param name="tmpVal"></param>
        /// <returns></returns>
        private string BuilderSmsContent(int typeId, string mobileno, int sid, string tmpVal)
        {
            if (typeId == 9 || typeId == 8 || typeId == 1)
            {
                return BuilderSmsVerificationCode(mobileno, sid, tmpVal);
            }
            return string.Empty;
        }
        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="sid"></param>
        /// <param name="tmpVal"></param>
        /// <returns></returns>
        private string BuilderSmsVerificationCode(string mobileNo, int sid, string tmpVal)
        {
            TssoRegcode tssoRegcode = db.TssoRegcodeSet.FirstOrDefault(c => c.Indate >= DateTime.Now && c.Regcode == mobileNo && c.Status == 0);
            if (tssoRegcode == null)
            {
                Random rd = new Random();
                tssoRegcode = new TssoRegcode
                {
                    Regcode = mobileNo,
                    Authcode = new Random().Next(100000, 999999).ToString(),
                    Codetype = 2,
                    Status = 0,
                    Indate = DateTime.Now.AddMinutes(10),
                    Remarks = "手机验证码",
                    Appstoreid = sid,
                    Regtype = 2,
                    Sourceip = Helper.GetClientIp()
                };
                db.TssoRegcodeSet.Add(tssoRegcode);
                if (db.SaveChanges() <= 0)
                {
                    log.Info(db.Message);
                    Alert("生成验证码失败");
                    return string.Empty;
                }
            }
            dynamic paramObject = new ExpandoObject();
            IDictionary<string, object> dict = paramObject as IDictionary<string, object>;
            dict.Add(tmpVal, tssoRegcode.Authcode);
            return JsonConvert.SerializeObject(paramObject);
        }

        /// <summary>
        /// 发送阿里云短信
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="tmpCode">模板CODE</param>
        /// <param name="paramString">模板变量,JSON字符串</param>
        /// <returns></returns>
        private bool SendAliYunSms(string mobile, string tmpCode, string paramString)
        {
            log.Info($"发送短信[mobile={mobile}&tmpCode={tmpCode}&paramString={paramString}]");
            try
            {
                SmsClient smsClient = new SmsClient(AppConfig.SmsCode, AppConfig.SmsPwd);
                smsClient.ServiceUrl = AppConfig.SmsServiceUrl;
                if (!smsClient.SendAliYunSms(mobile, tmpCode, paramString))
                {
                    log.Info($"发送短信失败[mobile={mobile}&tmpCode={tmpCode}&paramString={paramString}]:{Environment.NewLine}" +
                        $"BatchID={smsClient.Result.BatchID}&Code={smsClient.Result.Code}&Description={smsClient.Result.Description}");
                    Alert(smsClient.Result.Description);
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile">手机号码，多个号码使用半角逗号分割</param>
        /// <param name="content">短信内容</param>
        /// <param name="sendTime">定时发送，若为null则立即发送</param>
        /// <returns></returns>
        private bool SendCommonSms(string mobile, string content, DateTime? sendTime)
        {
            if (string.IsNullOrEmpty(content))
            {
                Alert("短信内容为空");
                return false;
            }
            SmsClient smsClient = new SmsClient(AppConfig.SmsCode, AppConfig.SmsPwd);
            smsClient.ServiceUrl = AppConfig.SmsServiceUrl;
            if (!smsClient.SendSms(mobile, content, sendTime))
            {
                if (smsClient.Result == null)
                {
                    log.Info($"发送短信失败[mobile={mobile}&content={content}&sendTime={sendTime}]:{Environment.NewLine},smsClient.Result结果为空 ");
                    Alert("发送短信失败");
                    return false;
                }
                log.Info($"发送短信失败[mobile={mobile}&content={content}&sendTime={sendTime}]:{Environment.NewLine}" +
                    $"BatchID={smsClient.Result.BatchID}&Code={smsClient.Result.Code}&Description={smsClient.Result.Description}");
                Alert(smsClient.Result.Description);
                return false;
            }
            return true;
        }
    }
}
