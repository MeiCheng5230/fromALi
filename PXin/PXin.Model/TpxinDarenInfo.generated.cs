using System;
using System.Collections.Generic;

namespace PXin.Model
{ 
    /// <summary>
    /// 达人信息表
    /// </summary>
    public partial class TpxinDarenInfo
    {
        public TpxinDarenInfo()
        { 
Infoid = 0;                                         
Nodeid = 0;                                         
Status = 0;                                         
Modifytime = new DateTime();                                         
Ischange = 0;                                         
Protectrate = 0;                                         
Praisenum = 0;                                         
Browsenum = 0;                                         
Voicebrowsenum = 0;                                         
            }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户ID,唯一
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  自我介绍
        ///</summary>
        public string Introduce { get; set; }
        /// <summary>
        ///  专业资格认证图片完整URL地址，多个用逗号隔开
        ///</summary>
        public string Specializedpic { get; set; }
        /// <summary>
        ///  达人达语
        ///</summary>
        public string Greetings { get; set; }
        /// <summary>
        ///  欢迎语，有值表示已开启欢迎语
        ///</summary>
        public string Welcome { get; set; }
        /// <summary>
        ///  状态  0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  最后一次修改状态时间
        ///</summary>
        public DateTime Modifytime { get; set; }
        /// <summary>
        ///  拒绝原因,管理员填写,用户可见
        ///</summary>
        public string Note { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        ///  自我介绍图片列表，多个用逗号隔开
        ///</summary>
        public string Introducepic { get; set; }
        /// <summary>
        ///  类型,即tpxin_daren_ext1的汇总
        ///</summary>
        public string Typename { get; set; }
        /// <summary>
        ///  职位
        ///</summary>
        public string Position { get; set; }
        /// <summary>
        ///  公司
        ///</summary>
        public string Company { get; set; }
        /// <summary>
        ///  当STATUS=3时是否允许修改1次 0=不允许 1=允许(这种都是人工数据)
        ///</summary>
        public int Ischange { get; set; }
        /// <summary>
        ///  自我介绍的语音地址
        ///</summary>
        public string Voiceaddress { get; set; }
        /// <summary>
        ///  是否保护倍率,0=不保护，1=保护（默认1）
        ///</summary>
        public int Protectrate { get; set; }
        /// <summary>
        ///  点赞我的数量
        ///</summary>
        public int Praisenum { get; set; }
        /// <summary>
        ///  浏览的我的数量
        ///</summary>
        public int Browsenum { get; set; }
        /// <summary>
        ///  我的语音介绍浏览数量
        ///</summary>
        public int Voicebrowsenum { get; set; }
    
        
    }
}