using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// 信友圈V点变化历史表
    /// </summary>
    public partial class TpxinPayhis
    {
        public TpxinPayhis()
        { 
Hisid = 0;                                         
Infoid = 0;                                         
Nodeid = 0;                                         
Price = 0;                                         
Typeid = 0;
            Tonodeid = 0;
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  TPXIN_MESSAGE.infoid,typeid=1时为0无意义
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  付款人
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  类型 1=充值 2=发布文章 3=查看文章
        ///</summary>
        public int Typeid { get; set; }

        /// <summary>
        /// 付款对象ID TYPEID！=3时 为0
        ///</summary>
        public int Tonodeid { get; set; }


    }
}