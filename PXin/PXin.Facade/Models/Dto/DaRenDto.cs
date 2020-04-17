using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Dto
{
    /// <summary>
    /// 达人基本信息
    /// </summary>

    public class DaRenBase
    {
        /// <summary>
        /// 
        /// </summary>
        private string _appPhoto;
        /// <summary>
        /// 主键值
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户Nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;
                }
                return _appPhoto;
            }

        }
        /// <summary>
        /// 签名
        /// </summary>
        public string Autograph { get; set; }
        /// <summary>
        /// 达人达语
        /// </summary>
        public string Greetings { get; set; }

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class DaRenInfoDto
    {
        /// <summary>
        /// 主键值
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户Nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Autograph { get; set; }
        /// <summary>
        /// 达人达语
        /// </summary>
        public string Greetings { get; set; }
        /// <summary>
        /// 聊天接收倍率
        /// </summary>
        public decimal Rate { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 专业领域
        /// </summary>
        public List<string> Majors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Typename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public int Orderno { get; set; }
        
    }

    /// <summary>
    /// 达人个人信息
    /// </summary>
    public class DaRenInfoSelfDto: DaRenInfoDto
    {
        /// <summary>
        /// 自我介绍图片
        /// </summary>
        public List<string> Pic { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string SelfIntroduction { get; set; }
        /// <summary>
        /// 相信号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 自我介绍语音地址
        /// </summary>
        public string VoiceAddress { get; set; }
        /// <summary>
        /// 语音浏览量
        /// </summary>
        public int VoiceBrowseNum { get; set; }
        /// <summary>
        /// 我的视频地址
        /// </summary>
        public List<VideoDto> VideoAddress { get; set; }
        /// <summary>
        /// 浏览我的人头像列表
        /// </summary>
        public List<string> BrowsePeople { get; set; }
        /// <summary>
        /// 浏览我的人总数
        /// </summary>
        public int BrowseNum { get; set; }
        /// <summary>
        /// 点赞我的总数
        /// </summary>
        public int PraiseNum { get; set; }
        /// <summary>
        /// 当前是否已经点赞 0=否 1=是
        /// </summary>
        public int IsPraise { get; set; }
        /// <summary>
        /// 当前是否已经浏览过 0=否 1=是
        /// </summary>
        public int IsBrowse { get; set; }
        /// <summary>
        /// 当前是否已经浏览过语音 0=否 1=是
        /// </summary>
        public int IsVoice { get; set; }
        /// <summary>
        /// 是否上传了知识库 0=否 1=是
        /// </summary>
        public int IsKnowledge { get; set; }
        /// <summary>
        /// 是否是好友 0=否 1=是
        /// </summary>
        public int IsFriend { get; set; }
        /// <summary>
        /// 是否开启倍率保护 0=否 1=是
        /// </summary>
        public int ProtectRate { get; set; }
        /// <summary>
        /// 分享链接
        /// </summary>
        public string ShareUrl { get; set; }
        /// <summary>
        /// 教育经历
        /// </summary>
        public List<DaRenEduBase> Edu { get; set; }
        /// <summary>
        /// 职业经历
        /// </summary>
        public List<DaRenOccupationBase> Occupation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string PicBase { get; set; }
    }

    /// <summary>
    /// 达人个人信息(静态页面)
    /// </summary>
    public class DaRenInfoSelfStaticDto : DaRenInfoDto
    {
        /// <summary>
        /// 自我介绍图片
        /// </summary>
        public List<string> Pic { get; set; }
        /// <summary>
        /// 浏览我的人头像列表
        /// </summary>
        public List<string> BrowsePeople { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string SelfIntroduction { get; set; }
        /// <summary>
        /// 相信号
        /// </summary>
        public string NodeCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 浏览我的人总数
        /// </summary>
        public int BrowseNum { get; set; }
        /// <summary>
        /// 教育经历
        /// </summary>
        public List<DaRenEduBase> Edu { get; set; }
        /// <summary>
        /// 职业经历
        /// </summary>
        public List<DaRenOccupationBase> Occupation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string PicBase { get; set; }
    }

    /// <summary>
    /// 达人首页（二级分类）
    /// </summary>
    public class DarenHomeInfoDto
    {
        /// <summary>
        /// 是否是达人 0=否 1=是
        /// </summary>
        public int IsDaRen { get; set; }
        /// <summary>
        /// 不是达人时启用 申请达人界面地址
        /// </summary>
        public string DaRenUrl { get; set; }
        /// <summary>
        /// 一级分类列表
        /// </summary>
        public List<Classification> Classific { get; set; }
        /// <summary>
        /// 推荐达人列表
        /// </summary>
        public List<DaRenInfoDto> List { get; set; }
    }

    /// <summary>
    /// 填写的资料
    /// </summary>
    public class DaRenAbovementionedDataDto: DaRenBase
    {
        /// <summary>
        /// 达人状态  -1=未满足申请达人条件 0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否实名认证 0=否 1=是 
        /// </summary>
        public int IsAuth { get; set; }
        /// <summary>
        /// 手机号 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 专业领域
        /// </summary>
        public List<string> Majors { get; set; }
        /// <summary>
        /// 是否填写教育背景 0=否 1=是 
        /// </summary>
        public int IsEdu { get; set; }
        /// <summary>
        /// 是否填写职业背景 0=否 1=是 
        /// </summary>
        public int IsOccupation { get; set; }
        /// <summary>
        /// 专业资格认证图片
        /// </summary>
        public List<string> ProfessionalPics { get; set; }
        /// <summary>
        /// 是否上传了我的视频 0=否 1=是
        /// </summary>
        public int IsVideo { get; set; }
        /// <summary>
        /// 是否填写了我的知识库数据 0=否 1=是
        /// </summary>
        public int IsKnowledge { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string SelfIntroduction { get; set; }
        /// <summary>
        /// 自我介绍图片
        /// </summary>
        public List<string> Pic { get; set; }
        /// <summary>
        /// 自我介绍语音
        /// </summary>
        public string VoiceUrl { get; set; }
        /// <summary>
        /// 欢迎语
        /// </summary>
        public string Welcome { get; set; }
        /// <summary>
        /// 是否开启欢迎语 0=否 1=是
        /// </summary>
        public int IsWelcome { get; set; }
        /// <summary>
        /// 拒绝原因(status=2时显示)
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 当前已经使用SV累计
        /// </summary>
        public decimal SV { get; set; }
        /// <summary>
        /// 当STATUS=3时是否允许修改1次 0=不允许 1=允许(这种都是人工数据)
        /// </summary>
        public int IsChange { get; set; }
        /// <summary>
        /// 是否关闭倍率保护 0=否 1=是
        /// </summary>
        public int IsProtectRate{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Specialized { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string PicBase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Typename { get; set; }
    }


    /// <summary>
    /// 一级分类
    /// </summary>
    public class Classification
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 分类名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Pic { get; set; }
    }

    /// <summary>
    /// 热门关键字
    /// </summary>
    public class HotKeyWordDto
    {
        /// <summary>
        /// 关键字id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }

    /// <summary>
    /// 分类列表
    /// </summary>
    public class ClassificationDto
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 分类名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 二级分类详情
        /// </summary>
        public List<Classification> List { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DaRenEduBase
    {
        /// <summary>
        /// 学校名
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
    }

    /// <summary>
    /// 教育领域数据
    /// </summary>
    public class DaRenEduDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 学校名
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 资料图片
        /// </summary>
        public List<string> Pics { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DaRenOccupationBase
    {
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
    }

    /// <summary>
    /// 职业领域数据
    /// </summary>
    public class DaRenOccupationDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Fromtime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 资料图片
        /// </summary>
        public List<string> Pics { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ChatDarenInfoDto
    {
        private string _appPhoto;
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Personalsign { get; set; }
        /// <summary>
        /// 专业领域
        /// </summary>
        public List<string> Majors { get; set; }
        /// <summary>
        /// 欢迎语
        /// </summary>
        public string Welcome { get; set; }
        /// <summary>
        /// 是否是达人 0=否 1=是
        /// </summary>
        public int IsDaRen  { get; set; }
        /// <summary>
        /// 是否开启倍率保护 0=否 1=是
        /// </summary>
        public int Protectrate { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;
                }
                return _appPhoto;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Typename { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class AdminDaRenInfoDto
    {
        /// <summary>
        /// 用户Nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 专业区域(一级)
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Occupation { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdminDaRenInfoDetailDto:DaRenBase
    {
        /// <summary>
        /// 手机号 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 专业领域
        /// </summary>
        public List<string> Majors { get; set; }
        /// <summary>
        /// 教育背景
        /// </summary>
        public List<DaRenEduDto> Edu { get; set; }
        /// <summary>
        /// 职业背景
        /// </summary>
        public List<DaRenOccupationDto> Occupation { get; set; }
        /// <summary>
        /// 专业资格认证图片
        /// </summary>
        public List<string> ProfessionalPics { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string SelfIntroduction { get; set; }
        /// <summary>
        /// 自我介绍图片
        /// </summary>
        public List<string> Pic { get; set; }
        /// <summary>
        /// 欢迎语
        /// </summary>
        public string Welcome { get; set; }
        /// <summary>
        /// 达人专区自己是否是默认推荐
        /// </summary>
        public int IsDefultHome { get; set; }
        /// <summary>
        /// 聊一聊自己是否是默认推荐
        /// </summary>
        public int IsDefultChat { get; set; }
        /// <summary>
        /// -1=未满足申请达人条件 0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Specialized { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string PicBase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string Typename { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KnowledgeBase
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 浏览人数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 支付类型 0=V点 1=UV
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KnowledgeDto: KnowledgeBase
    {
        /// <summary>
        /// 当前是否已经查看 0=否 1=是
        /// </summary>
        public int IsShow { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string ClickUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DaRenKnowledgeDto: KnowledgeBase
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 语音列表
        /// </summary>
        public string Voice { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        private string _appPhoto;
        /// <summary>
        /// 用户头像
        /// </summary>
        public string AppPhoto
        {
            set
            {
                _appPhoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_appPhoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_appPhoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _appPhoto;
                }
                return _appPhoto;
            }

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KnowledgeLists
    {
        /// <summary>
        /// 知识库数据列表
        /// </summary>
        public List<KnowledgeBase> List { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VideoBase
    {
        /// <summary>
        /// 视频主键id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 视频第一帧图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VideoDto: VideoBase
    {
        /// <summary>
        /// 点赞数
        /// </summary>
        public int Praisenum { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int Browsenum { get; set; }
        /// <summary>
        /// 是否点赞 0=否 1=是
        /// </summary>
        public int IsBrowse { get; set; }
        /// <summary>
        /// 是否浏览 0=否 1=是
        /// </summary>
        public int IsPraise { get; set; }
        /// <summary>
        /// 视频时长(秒)
        /// </summary>
        public int Duration { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateVideoDto
    {
        /// <summary>
        /// 视频id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 视频第一帧图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
