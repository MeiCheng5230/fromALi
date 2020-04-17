using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 竞拍历史
    /// </summary>
    public partial class TpxinPaiHis
    {
        /// <summary>
        /// 
        /// </summary>
        public TpxinPaiHis()
        { 
Hisid = 0;                                         
Nodeid = 0;                                         
Num = 0;                                         
Price = 0;                                         
Totalprice = 0;                                         
Status = 0;
Configid = 0;
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
        ///  数量
        ///</summary>
        public int Num { get; set; }
        /// <summary>
        ///  单价
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  总价=NUM*PRICE
        ///</summary>
        public decimal Totalprice { get; set; }
        /// <summary>
        ///  状态 -1=出局(p点已退还) 0=竞拍中 1=已完成 -2出局
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  显示的名次信息(服务每天更新一次)
        ///</summary>
        public string Rankinfo { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  A点竞拍配置表的主键（tpxin_pai_config.CONFIGID）
        ///</summary>
        public int Configid { get; set; }

    }
}