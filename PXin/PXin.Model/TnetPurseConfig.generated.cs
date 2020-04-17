using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 通用钱包配置表
    /// </summary>
    public partial class TnetPurseConfig
    {
        public TnetPurseConfig()
        { 
Infoid = 0;                                         
Showname = string.Empty;                                         
Pursetype = 0;                                         
Subid = 0;                                         
Currencytype = 0;                                         
Showunit = 0;                                         
Isshow = 0;                                         
Islocal = 0;                                         
Sortnum = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  显示名
        ///</summary>
        public string Showname { get; set; }
        /// <summary>
        ///  小图标
        ///</summary>
        public string Picurl { get; set; }
        /// <summary>
        ///  tblc_user_purse.pursetype
        ///</summary>
        public int Pursetype { get; set; }
        /// <summary>
        ///  tblc_user_purse.subid
        ///</summary>
        public int Subid { get; set; }
        /// <summary>
        ///  tblc_user_purse.currencytype
        ///</summary>
        public int Currencytype { get; set; }
        /// <summary>
        ///  对外的单位 tblc_currency.currencyid
        ///</summary>
        public int Showunit { get; set; }
        /// <summary>
        ///  对外的单位中文名
        ///</summary>
        public string Showunitname { get; set; }
        /// <summary>
        ///  数据库的查询SQL
        ///</summary>
        public string Sqldata { get; set; }
        /// <summary>
        ///  c#中的代码写法
        ///</summary>
        public string Codedata { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  是否显示 (优谷我的资产1=显示,2-提供给app但不显示在钱包列表，0=不显示）
        ///</summary>
        public int Isshow { get; set; }
        /// <summary>
        ///  对外显示的文字描述
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  是否内部货币
        ///</summary>
        public int Islocal { get; set; }
        /// <summary>
        ///  排序，数字越小越靠前
        ///</summary>
        public int Sortnum { get; set; }
        /// <summary>
        ///  背景图片
        ///</summary>
        public string Bgpic { get; set; }
    
        
    }
}