using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXin.Facade.SignalR.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BelieveCasClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public BelieveCasClient(dynamic client)
        {
            this.LastActiveTime = DateTime.Now;
            this.Client = client;
        }
        /// <summary>
        /// 
        /// </summary>
        public dynamic Client { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastActiveTime { get; private set; }
    }
}
