using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXin.SignalR.Models
{
    /// <summary>
    /// 客户端类
    /// </summary>
    public class BelieveCasClient
    {
        public BelieveCasClient(dynamic client)
        {
            this.Client = client;
        }
        /// <summary>
        /// 客户端对象
        /// </summary>
        public dynamic Client { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int Nodeid { get; set; }
    }
}