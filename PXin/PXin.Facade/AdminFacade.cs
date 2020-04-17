using Common.Mvc;
using Common.Mvc.HttpHelper;
using Common.Mvc.Models;
using io.rong;
using MvcPaging;
using PXin.DB;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminFacade
    {

        private Log log = new Log(typeof(AdminFacade));
        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 获取系统群组
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="nodecode"></param>
        /// <param name="groupid"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IPagedList<GroupDto> GroupQuery(string groupname,string nodecode, int groupid, int page, int pagesize)
        {
            PXinContext db = HttpContext.Current.GetDbContext<PXinContext>();
            var query = from gro in db.TchatGroupSet
                        join user in db.TnetReginfoSet on gro.Creater equals user.Nodeid into user_join
                        from user in user_join.DefaultIfEmpty()
                        where gro.Groupstate == 1 && gro.Grouptype == 2
                        select new GroupDto
                        {
                            Creater = gro.Creater,
                            Creatername = user.Nodename,
                            Createtime = gro.Createtime,
                            Groupname = gro.Groupname,
                            Id = gro.Id,
                            Creatercode = user.Nodecode
                        };
            if (groupid > 0)
            {
                query = query.Where(a => a.Id == groupid);
            }
            if (!string.IsNullOrEmpty(groupname))
            {
                string name = groupname.Trim();
                query = query.Where(a => a.Groupname.Contains(name));
            }
            if (!string.IsNullOrEmpty(nodecode)) {
                string code = nodecode.Trim();
                query = query.Where(a => a.Creatercode.Contains(code));
            }
            #region 时间筛选
            //if (!string.IsNullOrEmpty(ComSTime) && !string.IsNullOrEmpty(ComETime))
            //{
            //    DateTime start = Convert.ToDateTime(ComSTime + " 00:00:00");
            //    DateTime end = Convert.ToDateTime(ComETime + " 23:59:59");
            //    query = query.Where(a => a.Createtime >= start && a.Createtime <= end);
            //}
            //else if (!string.IsNullOrEmpty(ComSTime))
            //{
            //    DateTime start = Convert.ToDateTime(ComSTime + " 00:00:00");
            //    query = query.Where(a => a.Createtime >= start);
            //}
            //else if (!string.IsNullOrEmpty(ComETime))
            //{
            //    DateTime end = Convert.ToDateTime(ComETime + " 23:59:59");
            //    query = query.Where(a => a.Createtime <= end);
            //}
            #endregion
            query = query.OrderByDescending(a => a.Createtime).ThenByDescending(a => a.Id);
            return query.ToPagedList<GroupDto>(page, pagesize);
        }


        /// <summary>
        /// 管理员后台调用 改变群组创建者
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="groupid"></param>
        /// <param name="destNodecode"></param>
        /// <returns></returns>
        public bool ChangeGroupCreator(int nodeid, int groupid, string destNodecode)
        {
            PXinContext ctx = HttpContext.Current.GetDbContext<PXinContext>();
            TnetReginfo destReginfo = ctx.TnetReginfoSet.FirstOrDefault(c => c.Nodecode == destNodecode);
            if (destReginfo == null || destReginfo.Nodeid == 0)
            {
                Msg = $"{destNodecode}不存在";
                return false;
            }
            log.Info($"ChangeGroupCreator Start:nodeid={nodeid}&groupid={groupid}&destnodeid={destReginfo.Nodeid}");
            if (nodeid != 3434909 && nodeid != 3435064)
            {
                Msg = "用户权限不足";
                return false;
            }

            TchatGroup group = ctx.TchatGroupSet.FirstOrDefault(c => c.Id == groupid);
            if (group == null)
            {
                Msg = "群组不存在";
                return false;
            }

            if (group.Creater == destReginfo.Nodeid)
            {
                Msg = "群组创建者已是当前用户";
                return false;
            }

            TchatUser destUser = ctx.TchatUserSet.FirstOrDefault(c => c.Nodeid == destReginfo.Nodeid);
            if (destUser == null)
            {
                Msg = "目标用户nodeid不存在," + destReginfo.Nodeid;
                log.Info(Msg);
                return false;
            }

            TchatGroupUser groupUser = ctx.TchatGroupUserSet.FirstOrDefault(c => c.Groupid == groupid && c.Userid == group.Creater);
            if (groupUser != null)
            {
                RongResult result = RongCloudServer.QuitGroup(AppConfig.AppKey, AppConfig.AppSecret, group.Creater.ToString(), group.Id.ToString());
                if (!result.Result)
                {
                    Msg = "退出群组失败,code=1";
                    log.Info(result.errorMessage);
                    return false;
                }
                ctx.TchatGroupUserSet.Remove(groupUser);
            }

            if (ctx.TchatGroupUserSet.Count(c => c.Groupid == groupid && c.Userid == destReginfo.Nodeid) == 0)
            {
                RongResult result = RongCloudServer.JoinGroup(AppConfig.AppKey, AppConfig.AppSecret, destReginfo.Nodeid.ToString(), group.Id.ToString(), group.Groupname);
                if (!result.Result)
                {
                    Msg = "加入群组失败,code=1";
                    log.Info(result.errorMessage);
                    return false;
                }
                ctx.TchatGroupUserSet.Add(new TchatGroupUser { Groupid = group.Id, Userid = destReginfo.Nodeid, Creattime = DateTime.Now });
                if (group.Grouptype == 3)
                {
                    CommonApiTransfer.Instance.SyncMyGroup3Info(destReginfo.Nodeid);//目标用户如果是新加入群组 需要同步
                }
                else
                {
                    CommonApiTransfer.Instance.SyncMyGroupInfo(destReginfo.Nodeid);
                }
            }

            group.Creater = destReginfo.Nodeid;
            ctx.SaveChanges();
            //改变群组创建者 先移除群主  在修改群组的创建者
            CommonApiTransfer.Instance.SyncChatGroupInfo(group.Id);
            if (group.Grouptype == 3)
            {
                CommonApiTransfer.Instance.SyncMyGroup3Info(group.Creater);//同步群主
            }
            else
            {
                CommonApiTransfer.Instance.SyncMyGroupInfo(group.Creater);//同步群主
            }
            log.Info($"ChangeGroupCreator Finish:nodeid={nodeid}&groupid={groupid}&destnodeid={ destReginfo.Nodeid}");
            return true;

        }

        /// <summary>
        /// 返回的群组信息
        /// </summary>
        public class GroupDto
        {
            /// <summary>
            /// 群id
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 群名
            /// </summary>
            public string Groupname { get; set; }
            /// <summary>
            /// 群主id
            /// </summary>
            public int Creater { get; set; }
            /// <summary>
            /// 群主名称
            /// </summary>
            public string Creatername { get; set; }
            /// <summary>
            /// 群组code
            /// </summary>
            public string Creatercode { get; set; }
            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime Createtime { get; set; }

        }

    }
}
