using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 表情包素材
    /// </summary>
    public partial class TpxinEmoticonMaterial
    {
        public TpxinEmoticonMaterial()
        { 
Id = 0;                                         
Typeid = 0;                                         
Name = string.Empty;                                         
Author = string.Empty;                                         
Intr = string.Empty;                                         
Filesize = string.Empty;                                         
Price = 0;                                         
Configid = 0;                                         
Url = string.Empty;                                         
Filedir = string.Empty;                                         
Buycount = 0;                                         
Sendprice = 0;                                         
            }

        /// <summary>
        ///  PK
        ///</summary>
        public int Id { get; set; }
        /// <summary>
        ///  1-表情包，2-表情单品
        ///</summary>
        public int Typeid { get; set; }
        /// <summary>
        ///  表情包名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  作者
        ///</summary>
        public string Author { get; set; }
        /// <summary>
        ///  简介
        ///</summary>
        public string Intr { get; set; }
        /// <summary>
        ///  表情包文件大小
        ///</summary>
        public string Filesize { get; set; }
        /// <summary>
        ///  价格，0-表示免费
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  钱包配置ID,支付用,tnet_purse_config.infoid
        ///</summary>
        public int Configid { get; set; }
        /// <summary>
        ///  typeid=1时表示表情包URL根目录，typeid=2时表示表情单品完整URL地址
        ///</summary>
        public string Url { get; set; }
        /// <summary>
        ///  typeid=1时表示表情包物理文件存放地址根目录，typeid=2时表示表情单品完整地址
        ///</summary>
        public string Filedir { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  购买次数
        ///</summary>
        public int Buycount { get; set; }
        /// <summary>
        ///  发送单价
        ///</summary>
        public decimal Sendprice { get; set; }
    
        
    }
}