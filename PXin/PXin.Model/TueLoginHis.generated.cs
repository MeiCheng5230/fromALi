using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 注册/登录/签到历史表
    /// </summary>
    public partial class TueLoginHis
    {
        public TueLoginHis()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Typeid = 0;                                         
Clientid = 0;                                         
Version = string.Empty;                                         
Sid = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  类型 1=注册 2=登录 3=签到
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  客户端 1=IOS 2=安卓
        ///</summary>
        public int Clientid { get; set; }
        /// <summary>
        ///  APP版本号
        ///</summary>
        public string Version { get; set; }
        /// <summary>
        ///  APPID唯一标记
        ///</summary>
        public int Sid { get; set; }
        /// <summary>
        ///  手机唯一标记,每个手机都不同/个推ID
        ///</summary>
        public string Token { get; set; }
        /// <summary>
        ///  系统分配
        ///</summary>
        public string Accesskeyid { get; set; }
        /// <summary>
        ///  时间戳
        ///</summary>
        public string Timestamp { get; set; }
        /// <summary>
        ///  GUID
        ///</summary>
        public string Signaturenonce { get; set; }
        /// <summary>
        ///  最近签名结果
        ///</summary>
        public string Signature { get; set; }
        /// <summary>
        ///  经度
        ///</summary>
        public string Longitude { get; set; }
        /// <summary>
        ///  维度
        ///</summary>
        public string Latitude { get; set; }
        /// <summary>
        ///  IP地址,APP用不到,以后WEB会用到
        ///</summary>
        public string Ip { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
    
        
    }
}