using Common.Facade;
using PXin.DB;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 评论分发服务
    /// </summary>
    public class CommentDispatchService : FacadeBase<PXinContext>
    {
        private TpxinCommentHis comment;
        /// <summary>
        /// 
        /// </summary>
        public int CommentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CommentDispatchService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentid"></param>
        public CommentDispatchService(int commentid)
        {
            CommentId = commentid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public CommentDispatchService(TpxinCommentHis comment)
        {
            this.comment = comment;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgid"></param>
        public void Execute(int msgid)
        {
            CommentId = msgid;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void Execute(TpxinCommentHis comment)
        {
            this.comment = comment;
            Execute();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            string guidStr = Guid.NewGuid().ToString();
            log.Info($"开始执行评论分发{guidStr}");
            if (!Try(CheckParam))
            {
                return;
            }
            db.Configuration.AutoDetectChangesEnabled = false;
            if (Try(ExecuteCore))
            {
                log.Info($"执行评论分发成功-结束{guidStr}");
            }
            else
            {
                log.Info($"执行评论分发失败-结束{guidStr}");
            }
            db.Configuration.AutoDetectChangesEnabled = true;
            CommentId = 0;
            comment = null;
        }
        private bool ExecuteCore()
        {
            int msgNodeid = db.TpxinMessageSet.Find(comment.Infoid).Nodeid;//发布信友圈作者
            List<int> friList = new List<int>();

            //发布信友圈作者的好友
            foreach (var item in db.TchatFriendSet.Where(c => c.Friendstatus == 1 && (c.Mynodeid == msgNodeid || c.Friendnodeid == msgNodeid)).Select(a => new { a.Mynodeid, a.Friendnodeid }))
            {
                friList.Add(item.Friendnodeid);
                friList.Add(item.Mynodeid);
            }
            if (msgNodeid != comment.Nodeid)
            {
                //评论者好友
                List<int> commentFriend = new List<int>();
                foreach (var item in db.TchatFriendSet.Where(c => c.Friendstatus == 1 && (c.Mynodeid == comment.Nodeid || c.Friendnodeid == comment.Nodeid)).Select(a => new { a.Mynodeid, a.Friendnodeid }))
                {
                    commentFriend.Add(item.Friendnodeid);
                    commentFriend.Add(item.Mynodeid);
                }
                //共同的朋友
                friList = friList.Intersect(commentFriend).ToList();
            }
            if(comment.Pnodeid > 0)
            {
                //回复人好友
                List<int> pFriend = new List<int>();
                foreach (var item in db.TchatFriendSet.Where(c => c.Friendstatus == 1 && (c.Mynodeid == comment.Pnodeid || c.Friendnodeid == comment.Pnodeid)).Select(a => new { a.Mynodeid, a.Friendnodeid }))
                {
                    pFriend.Add(item.Friendnodeid);
                    pFriend.Add(item.Mynodeid);
                }
                //共同的朋友
                friList = friList.Intersect(pFriend).ToList();
                friList.Add(comment.Pnodeid);
            }
            friList.Add(msgNodeid);
            friList.Add(comment.Nodeid);
            foreach (var item in friList.Distinct())
            {
                db.TpxinMessageUesrSet.Add(new TpxinMessageUesr
                {
                    Typeid = 1,
                    Infoid = comment.Hisid,
                    Nodeid = item,
                    Status = 0,
                    Createtime = DateTime.Now,
                    Remarks = ""
                });
            }
            db.SaveChanges();
            return true;
        }
        private bool CheckParam()
        {
            if (CommentId > 0)
            {
                comment = db.TpxinCommentHisSet.FirstOrDefault(c => c.Hisid == CommentId);
            }
            else
            {
                CommentId = comment.Hisid;
            }
            if (comment == null)
            {
                log.Info($"{nameof(CommentId)}={CommentId}:评论不存在");
                return false;
            }
            return true;
        }
    }
}

