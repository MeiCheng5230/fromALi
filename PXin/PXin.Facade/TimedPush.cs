using System;
using System.Linq;
using Common.Facade;
using Common.Mvc;
using Newtonsoft.Json;
using PXin.DB;
using PXin.Facade.ApiFacade;

namespace PXin.Facade
{
    /// <summary>
    /// 定时推送，业务类型
    /// </summary>
    public enum BusinessCategoryEnum
    {
        /// <summary>
        /// A点出局推送
        /// </summary>
        APonitOut = 20001,

        /// <summary>
        /// 租车
        /// </summary>
        RentCar = 20011
    }
    /// <summary>
    /// 定时推送
    /// </summary>
    public class TimedPush
    {
        public static TimedPush Instance = new TimedPush();
        private readonly Log log = new Log(typeof(TimedPush));
        private ChatFacade facade;
        public TimedPush()
        {
            facade = new ChatFacade();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            using (var db = new PXinContext())
            {
                var pushList = from push in db.TpxinPushDataSet
                               join chat in db.TchatUserSet on push.Nodeid equals chat.Nodeid
                               join pushkey in db.TnetReginfoExtSet on push.Nodeid equals pushkey.Nodeid
                               where push.Expecttime < DateTime.Now && push.Status == 0 && chat.IsSysNotice == 1
                               select new
                               {
                                   ID = push.Id,
                                   Typeid = push.Typeid,
                                   Nodeid = push.Nodeid,
                                   Title = push.Title,
                                   Content = push.Content,
                                   Url = push.Url,
                                   GTClientid = pushkey.Gtclientid,
                                   DeviceToken = pushkey.Devicetoken,
                                   IsNoticeDetail = chat.IsNoticeDetail,
                               };
                string content = "";
                foreach (var item in pushList)
                {
                    string cnt = item.Content;
                    if (item.IsNoticeDetail == 0)
                    {
                        cnt = "您有一条新消息";
                    }
                    content = JsonConvert.SerializeObject(new
                    {
                        Type = "APonitOut",//((BusinessCategoryEnum)item.Typeid).ToString(),
                        Title = item.Title,
                        Content = cnt,
                        Url = item.Url.Replace("{sign}", GetQueryString(item.Nodeid))
                    });
                    facade.GtPush(item.Nodeid, item.GTClientid, item.DeviceToken, item.Title, content);
                    facade.AddMessage(db,item.Nodeid, item.Content, item.Url,item.Title);

                    var pushData = db.TpxinPushDataSet.FirstOrDefault(f => f.Id == item.ID);
                    pushData.Pushtime = DateTime.Now;
                    pushData.Status = 1;
                    if (db.SaveChanges() < 0)
                    {
                        log.Info("修改推送状态失败：ID=" + pushData.Id);
                    }
                }
            }
        }

        private string GetQueryString(int nodeid)
        {
            string tm = DateTime.Now.ToString("yyyyMMddHHmmss");
            int sid = 81127;
            string sign = Helper.GetSign(nodeid, sid, tm, CommonConfig.ApiAuthString);
            return $"nodeid={nodeid}&sid={sid}&tm={tm}&sign={sign}";
        }
    }
}
