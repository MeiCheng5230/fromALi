using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CodeFirstStoreFunctions;
using Common.Mvc;

namespace PXin.DB
{
    public partial class PXinContext
    {
        public void InitFunction(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new FunctionsConvention(DbContextHelper.GetOwnerByTableName("TSYS_ERRORLOG"), typeof(Functions)));
        }
        /// <summary>
        /// 同步PXIN用户数据
        /// </summary>
        /// <param name="nodeid"></param>
        public void SyncChatUserFull(int nodeid)
        {
            BeginTransaction();
            try
            {
                ExecuteSqlCommand($"delete from tchat_user_full where nodeid = {nodeid}");
                if (ExecuteSqlCommand($@"insert into tchat_user_full 
select * from vchat_user where nodeid = {nodeid}") < 1)
                {
                    Rollback();
                    return;
                }
            }
            catch (Exception)
            {
                Rollback();
                return;
            }
            Commit();
        }

        /// <summary>
        /// 获取充值码
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string GetCentcard(WinnerDbContext db)
        {
            string getRandSql = "select GetCZMNum czm from dual ";
            var rand = db.SqlQuery(getRandSql).FirstOrDefault();
            return rand["CZM"]?.ToString();
        }
        /// <summary>
        /// 获取充值码
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userJxsInfoId"></param>
        /// <returns></returns>
        public static string Get90DaysStockInfo(WinnerDbContext db, int userJxsInfoId)
        {
            string sql = string.Format("select F_GetJxsTip({0}) as tip from dual", userJxsInfoId);
            var tip = db.SqlQuery(sql).FirstOrDefault();
            return tip["TIP"].ToString();
        }

        /// <summary>
        /// 是否完成任务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nodeid"></param>
        /// <param name="typeid">0-上月，1-本月</param>
        /// <returns></returns>
        public static bool IsFinishTask(WinnerDbContext db, int nodeid, int typeid)
        {
            string sql = string.Format("select F_IsBlTask({0},{1}) as task from dual", nodeid, typeid);
            var num = db.SqlQuery(sql).FirstOrDefault();
            return Convert.ToInt32(num["TASK"].ToString()) > 0;
        }

    }
    public static class Functions
    {
        /// <summary>
        /// 返回一个0~1的随机数
        /// </summary>
        /// <returns></returns>
        [DbFunction("CodeFirstDatabaseSchema", "RANDOMGET")]
        public static string Randomget()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 获得2个经纬度的距离米
        /// </summary>
        /// <param name="lnga"></param>
        /// <param name="lata"></param>
        /// <param name="lngb"></param>
        /// <param name="latb"></param>
        /// <returns></returns>
        [DbFunction("CodeFirstDatabaseSchema", "GETJULI")]
        public static double GetJuLi(string lnga, string lata, string lngb, string latb)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 获取充值码
        /// <returns></returns>
        [DbFunction("CodeFirstDatabaseSchema", "GetCZMNum")]
        public static string GetCentcard()
        {
            throw new NotSupportedException();
        }



    }
}
