using Common.Facade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.Models.Req
{
    /// <summary>
    /// 搜索达人请求
    /// </summary>
    public class SearchWiseManReq:Reqbase
    {
        /// <summary>
        /// 搜索类型 1=二级列表id搜索 2=达人名字搜索 3=一级列表id搜索
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 搜索名  type=1时传0
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 二级列表id 搜索全部的时候type传3，此项传一级id，type=1时传二级id type=2时传0
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 添加扩展信息(专业领域)
    /// </summary>
    public class CreateDaRenExt1Req : Reqbase
    {
        /// <summary>
        /// 专业领域id逗号分割
        /// </summary>
        public string Majorid { get; set; }
    }

    /// <summary>
    /// 添加扩展信息(教育领域)
    /// </summary>
    public class CreateDaRenEduReq : Reqbase
    {
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
        public string Pics { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetClassificasReq : Reqbase
    {
        /// <summary>
        /// 一级列表的id
        /// </summary>
        public int ID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateDaRenEdu: CreateDaRenEduReq
    {
        /// <summary>
        /// 主键id，获取教育数据接口返回的主键id
        /// </summary>
        public int ExtId { get; set; }
    }

    /// <summary>
    /// 删除扩展信息
    /// </summary>
    public class DeleteDaRenExtReq : Reqbase
    {
        /// <summary>
        /// 主键id，获取经历接口中的主键id
        /// </summary>
        public int ID { get; set; }
    }

    /// <summary>
    /// 获取推荐达人
    /// </summary>
    public class GetDefaultDaRenReq : Reqbase
    {
        /// <summary>
        /// 获取类型 0=聊一聊 1=达人专区
        /// </summary>
        public int Type { get; set; }
        
    }

    /// <summary>
    /// 自我介绍
    /// </summary>
    public class UpdateSelfIntroductionReq : Reqbase
    {
        /// <summary>
        /// 自我介绍文字
        /// </summary>
        public string Introduce { get; set; }
        /// <summary>
        /// 自我介绍语音地址
        /// </summary>
        public string VoiceUrl { get; set; }
        /// <summary>
        /// 图片地址 逗号分割
        /// </summary>
        public string Pics { get; set; }
    }

    /// <summary>
    /// 达人达语
    /// </summary>
    public class UpdateGreetingsReq : Reqbase
    {
        /// <summary>
        /// 达人达语
        /// </summary>
        public string Greetings { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateWelcomeReq : Reqbase
    {
        /// <summary>
        /// 欢迎语
        /// </summary>
        public string Welcome { get; set; }
    }

    /// <summary>
    /// 专业领域
    /// </summary>
    public class UpdateMajorsReq : Reqbase
    {
        /// <summary>
        /// 专业领域id 逗号分割
        /// </summary>
        public string Majors { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateDaRenOccupations : Reqbase
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
        /// <summary>
        /// 资料图片地址，逗号分割
        /// </summary>
        public string Pics { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateDaRenOccupations: CreateDaRenOccupations
    {
        /// <summary>
        /// 主键id，获取职业数据接口返回的主键id
        /// </summary>
        public int ExtId { get; set; }
        /// <summary>
        /// 是否显示为默认 0=否 1=是
        /// </summary>
        public int IsDefult { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetDaRenInfoSelf : Reqbase
    {
        /// <summary>
        /// 目标用户的nodeid
        /// </summary>
        public int PNodeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetDaRenInfoSelfNew : Reqbase
    {
        /// <summary>
        /// 目标用户的nodeid或者nodecode
        /// </summary>
        public string PNodeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetClassifications : Reqbase
    {
        /// <summary>
        /// 获取类型，达人主页传入1，申请资料页面传入2
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateSpecializedPics : Reqbase
    {
        /// <summary>
        /// 专业技能图片地址 逗号分割
        /// </summary>
        public string Pics { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateVideo : Reqbase
    {
        /// <summary>
        /// 视频地址
        /// </summary>
        public string Pics { get; set; }
        /// <summary>
        /// 视频时长(秒)
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 视频第一帧图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BrowseSomeOneReq : Reqbase
    {
        /// <summary>
        /// 目标用户的nodeid
        /// </summary>
        public int PNodeid { get; set; }
        /// <summary>
        /// 1=浏览 2=点赞 5=语音浏览
        /// </summary>
        public int Typeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BrowseVideoReq : Reqbase
    {
        /// <summary>
        /// 目标用户的nodeid
        /// </summary>
        public int PNodeid { get; set; }
        /// <summary>
        /// 目标视频的id
        /// </summary>
        public int Pinfoid { get; set; }
        /// <summary>
        /// 3=浏览 4=点赞
        /// </summary>
        public int Typeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DaRenKnowledgeReq : Reqbase
    {
        /// <summary>
        /// 知识库主键id
        /// </summary>
        public int ID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetDaRenKnowledgesReq : PageBase
    {
        /// <summary>
        /// 0=草稿 1=发布
        /// </summary>
        public int Typeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SeeDaRenKnowledgeReq : Reqbase
    {
        /// <summary>
        /// 知识库id
        /// </summary>
        public int ID { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class SetProtectRateReq : Reqbase
    {
        /// <summary>
        /// 是否开启倍率保护 0=否 1=是
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetDaRenKnowledgesByOneReq : PageBase
    {
        /// <summary>
        /// 对方nodeid
        /// </summary>
        public int Pnodeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateKnowledgeReq : Reqbase
    {
        /// <summary>
        ///  主键id
        ///</summary>
        public int ID { get; set; }
        /// <summary>
        ///  主题
        ///</summary>
        public string Title { get; set; }
        /// <summary>
        ///  支付类型 0=V点 1=UV
        ///</summary>
        public int Paytype { get; set; }
        /// <summary>
        ///  金额
        ///</summary>
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  状态 0=草稿 1=已发布
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  语音地址,逗号分割
        ///</summary>
        public string VoiceUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UploadVoiceReq : Reqbase
    {
        /// <summary>
        ///  音频文件base64
        ///</summary>
        public string Base64 { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeleteVideoReq : Reqbase
    {
        /// <summary>
        /// 视频主键id
        /// </summary>
        public int ID { get; set; }
    }



    /// <summary>
    /// 
    /// </summary>
    public class GetAwaitVerifyDaRenReq
    {
        /// <summary>
        /// 搜索类型 0=全部 1=按名字搜索
        /// </summary>
        public int type { get; set; }       
        /// <summary>
        /// 姓名 type=0时传0
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetVerifyDaRenDetailReq
    {
        /// <summary>
        /// 用户nodeid
        /// </summary>
        public int Nodeid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetDaRenStaticReq
    {
        /// <summary>
        /// 用户nodecode
        /// </summary>
        public string NodeCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdminVerifyDaRenReq
    {
        /// <summary>
        /// 用户nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 是否通过审核 0=否 1=是
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string Note { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateDefultDaRenReq
    {
        /// <summary>
        /// 用户nodeid
        /// </summary>
        public int Nodeid { get; set; }
        /// <summary>
        /// 推荐类型 0=聊一聊 1=达人专区
        /// </summary>
        public int Type { get; set; }
    }

}

