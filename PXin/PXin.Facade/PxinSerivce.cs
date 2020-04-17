using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Timers;
using Common.Mvc;
using PXin.DB;
using PXin.Facade.ApiFacade;

namespace PXin.Facade
{
    /// <summary>
    /// UE服务
    /// </summary>
    public class PxinSerivce
    {
        private static ConcurrentQueue<UeServiceData> serviceQueue = new ConcurrentQueue<UeServiceData>();
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private static Log log = new Log(typeof(PxinSerivce));
        private static System.Threading.Timer timer1 = null;
        /// <summary>
        /// 注册通知服务
        /// </summary>
        public static void Register()
        {
            Thread thread = new Thread(() =>
            {
                log.Info($"Register {nameof(PxinSerivce)} Success");
                while (true)
                {
                    manualResetEvent.WaitOne();
                    while (serviceQueue.TryDequeue(out UeServiceData data))
                    {
                        log.Info($"Dequeue:{data.ServiceType + "_" + data.Id}");
                        if (data.ServiceType == PxinServiceType.MsgDispatch)
                        {
                            new MsgDispatchService().Execute(data.Id);
                        }
                        else if (data.ServiceType == PxinServiceType.CommentDispath)
                        {
                            new CommentDispatchService().Execute(data.Id);
                        }
                        else
                        {
                            new NoticeService().Execute(data.Id);
                        }
                    }
                    manualResetEvent.Reset();
                }
            })
            { IsBackground = true };
            thread.Start();
            System.Timers.Timer timer = new System.Timers.Timer
            {
                Enabled = true,
                Interval = AppConfig.TimedPushTimeInterval * 60 * 1000
            };
            timer.Elapsed += Push;
            timer.Start();

            //定时器检查通知失败的数据
            int period = AppConfig.TimedPushTimeInterval * 60 * 1000;
            timer1 = new System.Threading.Timer((obj) =>
            {
                log.Info("定时器回调");
                timer1.Change(Timeout.Infinite, Timeout.Infinite);
                try
                {
                    DateTime dtToday = DateTime.Now;
                    DateTime dtYestDay = DateTime.Now.AddDays(-2);
                    using (PXinContext db = new PXinContext())
                    {
                        var hisIds = db.TpcnThirdPayhisSet.Where(x => x.Storequest != 2 && x.Createtime >= dtYestDay && x.Nextnotifytime < dtToday).Select(x => x.Hisid)
                                                            .OrderByDescending(x => x).ToList();
                        foreach (int hisId in hisIds)
                        {
                            EnqueueNotice(Convert.ToInt32(hisId));
                        }
                    }
                }
                catch (Exception exp)
                {
                    log.Info(exp.ToString());
                }
                timer1.Change(period, period);
            }, null, period, period);

            timer1.Change(0, period);
        }

        private static void Push(object source, ElapsedEventArgs e)
        {
            if (AppConfig.IsOpenTimedPush)
            {
                log.Info("-------------------------------定时推送-------------------------------");
                TimedPush.Instance.Execute();
            }
        }

        /// <summary>
        /// 加入文章队列
        /// </summary>
        /// <param name="msgid"></param>
        public static void EnqueueMsg(int msgid)
        {
            log.Info($"EnqueueMsg:{msgid}");
            serviceQueue.Enqueue(new UeServiceData { Id = msgid, ServiceType = PxinServiceType.MsgDispatch });
            manualResetEvent.Set();
        }
        /// <summary>
        /// 加入评论队列
        /// </summary>
        /// <param name="commentid"></param>
        public static void EnqueueComment(int commentid)
        {
            log.Info($"EnqueueComment:{commentid}");
            serviceQueue.Enqueue(new UeServiceData { Id = commentid, ServiceType = PxinServiceType.CommentDispath });
            manualResetEvent.Set();
        }

        /// <summary>
        /// 加入通知队列
        /// </summary>
        /// <param name="hisid"></param>
        public static void EnqueueNotice(int hisid)
        {
            log.Info($"NoticeEnqueue:{hisid}");
            serviceQueue.Enqueue(new UeServiceData { Id = hisid, ServiceType = PxinServiceType.Notice });
            manualResetEvent.Set();
        }

        struct UeServiceData
        {
            public int Id;
            public PxinServiceType ServiceType;
        }
        enum PxinServiceType : short
        {
            /// <summary>
            /// 消息分发
            /// </summary>
            MsgDispatch = 0,
            CommentDispath = 1,
            Notice = 2,
        }
    }
}
