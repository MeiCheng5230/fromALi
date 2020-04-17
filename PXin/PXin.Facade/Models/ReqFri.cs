using Common.Facade.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PXin.Facade.Models
{
    /// <summary>
    /// 充值V点请求参数
    /// </summary>
    public class ReqChargeVDian : Reqbase
    {
        /// <summary>
        /// 充值数量
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public decimal Price { get; set; }
        /// <summary>
        /// 支付类型(0=UE,1=SV支付)
        /// </summary>
        [Required]
        public double PayType { get; set; }
        /// <summary>
        /// 支付密码,paytype=1时有效
        /// </summary>
        public string PayPwd { get; set; }
    }

    /// <summary>
    /// 获取历史V点交易记录请求参数
    /// </summary>
    public class ReqGetPVDianHis : Reqbase
    {
        /// <summary>
        /// 页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [Required]
        public int PageIndex { get; set; }
    }

    /// <summary>
    /// 获取历史V点交易记录返回参数
    /// </summary>
    public class PxinPayhisDto
    {
        /// <summary>
        /// 
        /// </summary>
        public PxinPayhisDto()
        {
            Hisid = 0;
            Price = 0;
            Typeid = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }

        /// <summary>
        ///  金额
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        ///  类型 1=充值 2=发布文章 3=查看文章
        ///</summary>
        public int Typeid { get; set; }

        /// <summary>
        /// 充值 发布文章 查看文章
        /// </summary>
        public string Typename
        {
            get
            {
                return (Typeid == 3 && Price < 0) ? "查看付费-" + Nodename + "" : (Typeid == 3 && Price > 0) ? "查看收款-" + Nodename + "" : (Typeid == 1) ? "充值" : "发布";
            }
        }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string Nodename { get; set; }
    }

    /// <summary>
    /// 获取历史P点交易记录返回参数
    /// </summary>
    public class PxinPraiseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public PxinPraiseDto()
        {
            Hisid = 0;
            Status = 0;
            Reward = 0;
        }
        /// <summary>
        /// 主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  状态 -1=踩 0=即没赞也没踩 1=赞
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  状态名称
        ///</summary>
        public string Statusname
        {
            get
            {
                return Type == 1 ? "踩-" + Nodename + "" : Type == 2 ? "赞-" + Nodename + "" : Type == 3 ? "打赏-" + Nodename + "" : "赏金-" + Nodename + "";
            }
        }
        /// <summary>
        ///  最后一次操作时间
        ///</summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        ///  打赏金额
        ///</summary>
        public decimal Reward { get; set; }

        /// <summary>
        /// 1=发布文章 2=查看文章 3=点赞文章 4=踩文章 5=聊天计费 6=充值
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string Nodename { get; set; }
    }

    /// <summary>
    /// 支付V点（查看文章）请求参数
    /// </summary>
    public class ReqPayVDian : Reqbase
    {
        ///// <summary>
        ///// 密码
        ///// </summary>
        //[Required]
        //public string Pwd { get; set; }
        /// <summary>
        /// 信友圈消息表主键id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int InfoID { get; set; }
    }


    /// <summary>
    /// 发布信友圈信息请求参数
    /// </summary>
    public class ReqPxinMessage : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqPxinMessage()
        {
            Price = 0;
        }
        /// <summary>
        ///  &gt;=0,查看需要多少P点,0表示免费
        ///</summary>
        [Required]
        [Range(0, 9999999999)]
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }

        /// <summary>
        ///  视频文件
        ///</summary>
        public string Video { get; set; }
        /// <summary>
        ///  音频文件
        ///</summary>
        public string Sound { get; set; }

        /// <summary>
        ///  图片URL,多张用逗号隔开
        ///</summary>
        public string Picurl { get; set; }
    }

    /// <summary>
    /// 删除信友圈请求参数
    /// </summary>
    public class ReqDeleteMsg : Reqbase
    {
        /// <summary>
        ///信友圈消息表主键id
        /// </summary>
        [Required]
        [Range(1, 9999999999)]
        public int Infoid { get; set; }
    }

    /// <summary>
    /// 获取信友圈用户基本信息的请求参数
    /// </summary>
    public class ReqUserInfo : Reqbase
    {
        /// <summary>
        /// 查询信友圈的Nodeid  ',', ';', '|'隔开,为空查询自己
        /// </summary>
        public string Snodeids { get; set; }
    }

    /// <summary>
    /// 查询信友圈用户基本信息的返回参数
    /// </summary>
    public class UserinfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public UserinfoDto()
        {
            Infoid = 0;
            Nodeid = 0;
            Up = 0;
            Down = 0;
            P = 0;
            V = 0;
        }

        /// <summary>
        ///  主键值
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  用户的NODEID,唯一
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  顶部大图片URL(空时有默认图片)
        ///</summary>
        public string Backpic { get; set; }
        /// <summary>
        ///  总赞数量
        ///</summary>
        public int Up { get; set; }
        /// <summary>
        ///  总踩数量
        ///</summary>
        public int Down { get; set; }
        /// <summary>
        ///  P点(来源于赞/踩/打赏) 可为负数
        ///</summary>
        public decimal P { get; set; }
        /// <summary>
        ///  V点(发布和看文章) &gt;=0
        ///</summary>
        public decimal V { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  备注
        ///</summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _Appphoto { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Appphoto
        {
            set
            {
                _Appphoto = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_Appphoto))
                {
                    return AppConfig.DefaultPhoto;
                }
                else if (!_Appphoto.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    return AppConfig.Userphoto + _Appphoto;    //新改的图片地址
                }
                return _Appphoto;
            }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Nodename { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Personalsign { get; set; }
    }


    /// <summary>
    /// 查询信友圈消息请求参数
    /// </summary>
    public class ReqGetMsg : Reqbase
    {
        /// <summary>
        /// 页大小范围1-30
        /// </summary>
        [Required]
        [Range(1, 30)]
        public int PageSize { get; set; }
        /// <summary>
        /// 页数范围1-100000
        /// </summary>
        [Required]
        [Range(1, 1000000)]
        public int PageIndex { get; set; }
        /// <summary>
        /// 查询信友圈的nodeid
        /// </summary>
        [Required]
        public int Snodeid { get; set; }
    }
    /// <summary>
    /// 获取信友圈首页消息请求参数
    /// </summary>
    public class ReqGetMsgHome : Reqbase
    {
        /// <summary>
        /// 页大小范围1-30
        /// </summary>
        [Required]
        [Range(1, 30)]
        public int PageSize { get; set; }
        /// <summary>
        /// 页数范围1000000
        /// </summary>
        [Required]
        [Range(1, 1000000)]
        public int PageIndex { get; set; }
        /// <summary>
        /// 获取消息的开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
    }

    /// <summary>
    /// 信友圈消息
    /// </summary>
    public class MessageDto
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageDto()
        {
            Infoid = 0;
            Hisid = 0;
            Nodeid = 0;
            Localnodeid = 0;
            Price = 0;
            Up = 0;
            Down = 0;
            Commentnum = 0;
            Ispay = 0;
            Reward = 0;
            IsUp = 0;
            IsDown = 0;
        }

        /// <summary>
        ///  消息Id
        ///</summary>
        public int Infoid { get; set; }

        /// <summary>
        ///  推送消息Id
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  发布用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  当前登录用户ID
        ///</summary>
        public int Localnodeid { get; set; }
        /// <summary>
        /// 查看需要多少v点,0表示免费
        ///</summary>
        public int Price { get; set; }
        /// <summary>
        ///  内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  视频文件
        ///</summary>
        public string Video { get; set; }
        /// <summary>
        ///  音频文件
        ///</summary>
        public string Sound { get; set; }
        /// <summary>
        ///  图片URL,多张用逗号隔开
        ///</summary>
        public string Picurl { get; set; }
        /// <summary>
        ///  赞
        ///</summary>
        public decimal Up { get; set; }
        /// <summary>
        ///  踩
        ///</summary>
        public decimal Down { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  评论数量
        ///</summary>
        public int Commentnum { get; set; }
        /// <summary>
        /// 是否已付款 1=已付款（自己的文章，已付款文章，免费文章）
        /// </summary>
        public int Ispay { get; set; }
        /// <summary>
        ///  已打赏金额
        ///</summary>
        public decimal Reward { get; set; }

        /// <summary>
        /// 是否已赞 1=已赞
        /// </summary>
        public int IsUp { get; set; }
        /// <summary>
        /// 是否已踩 1=已踩
        /// </summary>
        public int IsDown { get; set; }
    }
    /// <summary>
    /// 信友圈评论
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        ///  主键值
        ///</summary>
        public int Hisid { get; set; }
        /// <summary>
        ///  消息Id
        ///</summary>
        public int Infoid { get; set; }
        /// <summary>
        ///  发布评论人用户ID
        ///</summary>
        public int Nodeid { get; set; }
        /// <summary>
        ///  具体内容
        ///</summary>
        public string Content { get; set; }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime Createtime { get; set; }
        /// <summary>
        ///  回复评论,关联Hisid
        ///</summary>
        public int Phisid { get; set; }
        /// <summary>
        ///  回复人NODEID
        ///</summary>
        public int Pnodeid { get; set; }
    }
    /// <summary>
    /// 信友圈消息
    /// </summary>
    public class FriMessageCollection
    {
        /// <summary>
        /// 文章列表
        /// </summary>
        public IEnumerable<MessageDto> Messages { get; set; }
        /// <summary>
        /// 评论列表
        /// </summary>
        public IEnumerable<CommentDto> Comments { get; set; }
    }

    /// <summary>
    /// 评论的请求参数
    /// </summary>
    public class ReqCreateComment : Reqbase
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqCreateComment()
        {
            Phisid = 0;
        }
        /// <summary>
        ///  信友圈消息表TPXIN_MESSAGE.INFOID 
        ///</summary>
        [Required]
        [Range(1, 9999999999)]
        public int Infoid { get; set; }

        /// <summary>
        ///  具体内容
        ///</summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        ///  回复评论,自关联(不是回复别人的评论=0)
        ///</summary>
        public int Phisid { get; set; }

    }

    /// <summary>
    /// 点赞或踩
    /// </summary>
    public class ReqCreateAttitude : Reqbase
    {
        /// <summary>
        ///  信友圈消息表TPXIN_MESSAGE.INFOID 
        ///</summary>
        [Required]
        [Range(1, 9999999999)]
        public int Infoid { get; set; }
        /// <summary>
        /// 是点赞或踩 1=点赞，-1=踩
        /// </summary>
        [Required]
        public int Isupdown { get; set; }
    }

    /// <summary>
    /// 打赏
    /// </summary>
    public class ReqCreateReward : Reqbase
    {
        /// <summary>
        ///  信友圈消息表TPXIN_MESSAGE.INFOID 
        ///</summary>
        [Required]
        [Range(1, 9999999999)]
        public int Infoid { get; set; }
        /// <summary>
        /// 打赏金额
        /// </summary>
        [Required]
        public int Reward { get; set; }
    }

    /// <summary>
    /// 修改背景图片
    /// </summary>
    public class ReqUpdateBackgImg : Reqbase
    {
        /// <summary>
        ///  背景图片地址 
        ///</summary>
        [Required]
        public string BackImg { get; set; }
    }
    /// <summary>
    /// 创建数据返回值
    /// </summary>
    public class CreateDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int KeyId { get; set; }
    }


    /// <summary>
    /// 信友圈文章举报
    /// </summary>
    public class ReqReport : Reqbase
    {
        /// <summary>
        /// 举报信友圈文章id 
        ///</summary>
        [Required]
        [Range(1, 9999999999)]
        public int InfoId { get; set; }

        /// <summary>
        /// 举报原因
        ///</summary>
        [Required]
        public int Reason { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        public string Remarks { get; set; }

    }

    /// <summary>
    /// 信友圈文章举报返回
    /// </summary>
    public class ReportDto
    {
        /// <summary>
        /// 原因id
        /// </summary>
        public int ReasonId { get; set; }
        /// <summary>
        ///原因
        /// </summary>
        public string ReasonName { get; set; }
    }

    /// <summary>
    /// v点p点变动历史Dto
    /// </summary>
    public class PxinAmountChangeHisDto
    {
        /// <summary>
        /// 变动金额
        ///</summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public string BalanceAfter
        {
            set
            {
            }
            get
            {
                return _balanceafter.ToString("0.00");
            }

        }
        /// <summary>
        ///  创建时间
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        ///  1=发布文章 2=查看文章 3=点赞文章 4=踩文章 5=打赏 6-聊天计费 7=充值
        ///</summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 金额变动原因描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        [JsonIgnore]
        public decimal _balanceafter { get; set; }
    }
    /// <summary>
    /// v点p点变动历史Dto
    /// </summary>
    public class PxinAmountChangeHisReq : Reqbase
    {
        /// <summary>
        /// 页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [Required]
        public int PageIndex { get; set; }
        /// <summary>
        /// 类型 1=V点 2=P点 3=SVC
        /// </summary>
        [Required]
        public int TypeId { get; set; }
    }
}

