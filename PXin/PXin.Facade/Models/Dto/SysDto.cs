using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// TCP服务相关内容
    /// </summary>
    public class TCPServerConfigDto
    {
        /// <summary>
        /// TCP服务地址(192.168.1.1:12345)
        /// </summary>
        public List<string> IPs { get; set; }
        /// <summary>
        /// 心跳包发送间隔(单位：s)
        /// </summary>
        public int KeepAliveInterval { get; set; }
    }
    /// <summary>
    /// 获取手机国际区号Dto
    /// </summary>
    public class AreaCodeDto
    {
        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  区域
        ///</summary>
        public string Areaname { get; set; }
        /// <summary>
        ///  国家
        ///</summary>
        public string Country { get; set; }
        /// <summary>
        ///  区号
        ///</summary>
        public string Code { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  国家首字母拼音
        ///</summary>
        public string EnCountry { get; set; }
        /// <summary>
        ///  常用=1,默认=0
        ///</summary>
        public int Commonuse { get; set; }
    }
}
