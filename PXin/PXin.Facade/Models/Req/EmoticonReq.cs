using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PageBase : Reqbase
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 获取热门表情包请求
    /// </summary>
    public class HotEmoticonsReq : Reqbase
    {

    }

    /// <summary>
    /// 搜索表情包请求
    /// </summary>
    public class SearchEmoticonsReq : PageBase
    {
        /// <summary>
        /// 表情包名字
        /// </summary>
        public string EmoticonName { get; set; }
        /// <summary>
        /// 1=表情包 2=表情单品
        /// </summary>
        public int Typeid { get; set; }
    }

    /// <summary>
    /// 下载表情
    /// </summary>
    public class DownloadEmoticonReq : Reqbase
    {
        /// <summary>
        /// 表情包id
        /// </summary>
        public int ID { get; set; }
        ///// <summary>
        ///// 支付密码
        ///// </summary>
        //public string Pwd { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmoticonMaterialDetailReq : Reqbase
    {
        /// <summary>
        /// 表情包id
        /// </summary>
        public int ID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateShowReq : Reqbase
    {
        /// <summary>
        /// 表情包id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 表情名称类型 默认传0 01_aa或者aa_01(传0)  aa_1(传1)  1_aa(传2) 
        /// </summary>
        public int Typeid { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 查询ue是否支付成功
    /// </summary>
    public class VerifyPayReq : Reqbase
    {
        /// <summary>
        /// 返回ue参数中的 orderno
        /// </summary>
        public int OrderNo { get; set; }
    }
}
