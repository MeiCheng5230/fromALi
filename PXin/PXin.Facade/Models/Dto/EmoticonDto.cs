using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 热门表情包
    /// </summary>
    public class HotEmoticonsDto
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchEmoticonsDto
    {
        /// <summary>
        ///  表情包
        ///</summary>
        public List<EmoticonsDto> Emoticon { get; set; }
        /// <summary>
        ///  表情单品
        ///</summary>
        public List<EmoticonsDto> SingleEmoticon { get; set; }
    }

    /// <summary>
    /// 表情包，表情单品
    /// </summary>
    public class EmoticonsDto
    {
        /// <summary>
        ///  表情包主键id
        ///</summary>
        public int ID { get; set; }
        /// <summary>
        ///  表情包名称
        ///</summary>
        public string Name { get; set; }
        /// <summary>
        ///  类型 1=表情包 2=表情单品
        ///</summary>
        public int Typeid { get; set; }
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
        ///  支付类型
        ///</summary>
        public string PurseName { get; set; }
        /// <summary>
        ///  显示主图地址
        ///</summary>
        public string Url { get; set; }
        /// <summary>
        ///  当前用户是否已经购买/下载
        ///</summary>
        public int IsDownload { get; set; }
        /// <summary>
        ///  发送价格
        ///</summary>
        public decimal SendPrice { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class EmoticonDetailDto
    {
        /// <summary>
        ///  完整gif图
        ///</summary>
        public string ImageGifUrl { get; set; }
        /// <summary>
        ///  静态png图
        ///</summary>
        public string ImagePngUrl { get; set; }
    }
}
