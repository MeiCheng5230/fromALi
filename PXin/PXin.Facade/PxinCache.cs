using System;
using System.Linq;
using System.Web;
using Common.Mvc;
using System.Web.Caching;
using System.Collections;
using PXin.Model;
using PXin.DB;
using Common.Mvc.Models;
using Common.Mvc.HttpHelper;

namespace PXin.Facade
{
    /// <summary>
    /// 
    /// </summary>
    public class PxinCache
    {
        private static Log log = new Log(typeof(PxinCache));
        private const string Prefix_Reginfo = "reginfo_";

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static TnetReginfo GetRegInfo(int nodeId)
        {
           return  CommonApiTransfer.Instance.GetTnetReginfo(new GetRegInfoReq { RegInfoKey = nodeId.ToString() });
        }
        /// <summary>
        /// 移除用户缓存
        /// </summary>
        /// <param name="nodeId"></param>
        public static void RemoveRegInfo(int nodeId)
        {
            CommonApiTransfer.Instance.RemoveTnetReginfoCache(new GetRegInfoReq {  RegInfoKey = nodeId.ToString() });
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearAll()
        {
            IDictionaryEnumerator cacheEnmu = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnmu.MoveNext())
            {
                HttpRuntime.Cache.Remove(cacheEnmu.Key.ToString());
            }
        }
    }
}
