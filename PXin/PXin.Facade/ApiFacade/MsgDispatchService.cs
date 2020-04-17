using Common.Facade;
using PXin.DB;
using PXin.Model;
using System;
using System.Linq;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 消息分发服务
    /// </summary>
    public class MsgDispatchService : FacadeBase<PXinContext>
    {
        private TpxinMessage message;
        /// <summary>
        /// 
        /// </summary>
        public int MsgId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MsgDispatchService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgid"></param>
        public MsgDispatchService(int msgid)
        {
            MsgId = msgid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MsgDispatchService(TpxinMessage message)
        {
            this.message = message;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgid"></param>
        public void Execute(int msgid)
        {
            MsgId = msgid;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Execute(TpxinMessage message)
        {
            this.message = message;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            string guidStr = Guid.NewGuid().ToString();
            log.Info($"开始调用消息分发{guidStr}");
            if (!Try(CheckParam))
            {
                return;
            }
            db.Configuration.AutoDetectChangesEnabled = false;
            if (Try(ExecuteCore))
            {
                log.Info($"调用消息分发成功-结束{guidStr}");
            }
            else
            {
                log.Info($"调用消息分发失败-结束{guidStr}");
            }
            db.Configuration.AutoDetectChangesEnabled = true;
            MsgId = 0;
            message = null;
        }
        private bool ExecuteCore()
        {
            db.TpxinMessageUesrSet.Add(new TpxinMessageUesr
            {
                Typeid = 0,
                Infoid = message.Infoid,
                Nodeid = message.Nodeid,
                Status = 0,
                Createtime = DateTime.Now,
                Remarks = ""
            });

            foreach (var item in db.TchatFriendSet.AsNoTracking().Where(c => c.Friendstatus == 1 && (c.Mynodeid == message.Nodeid || c.Friendnodeid == message.Nodeid)))
            {
                db.TpxinMessageUesrSet.Add(new TpxinMessageUesr
                {
                    Typeid = 0,
                    Infoid = message.Infoid,
                    Nodeid = item.Mynodeid == message.Nodeid ? item.Friendnodeid : item.Mynodeid,
                    Status = 0,
                    Createtime = DateTime.Now,
                    Remarks = ""
                });
            }
            int result = db.SaveChanges();
            return true;
        }
        private bool CheckParam()
        {
            if (MsgId > 0)
            {
                message = db.TpxinMessageSet.FirstOrDefault(c => c.Infoid == MsgId);
            }
            if (message == null)
            {
                log.Info($"{nameof(MsgId)}={MsgId}:消息不存在");
                return false;
            }
            return true;
        }
    }
}

