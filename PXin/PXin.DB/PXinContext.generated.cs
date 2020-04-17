using System;
using System.Data.Entity;
using Common.Mvc;
using PXin.Model;

namespace PXin.DB
{
    public partial class PXinContext : WinnerDbContext
    {
        static PXinContext()
        {
            Database.SetInitializer<PXinContext>(null);
        }

        public PXinContext()
            : base()
        {
        }
        public PXinContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<VpxinOctoberActivity> VpxinOctoberActivitySet { get; set; }

        /// <summary>
        /// UV充值记录表
        /// </summary>
        public DbSet<TfinBilltrans> TfinBilltransSet { get; set; }

        /// <summary>
        /// 充值商配车情况视图
        /// </summary>
        public DbSet<VblJxsPeiche> VblJxsPeicheSet { get; set; }

        /// <summary>
        /// 定时消息推送表
        /// </summary>
        public DbSet<TpxinPushData> TpxinPushDataSet { get; set; }

        /// <summary>
        /// 注册邀请码
        /// </summary>
        public DbSet<TnetReginfoCode> TnetReginfoCodeSet { get; set; }

        /// <summary>
        /// 直播送礼历史
        /// </summary>
        public DbSet<TchatLivegiftHis> TchatLivegiftHisSet { get; set; }

        /// <summary>
        /// 直播礼物表
        /// </summary>
        public DbSet<TchatLivegiftType> TchatLivegiftTypeSet { get; set; }

        /// <summary>
        /// 直播房间
        /// </summary>
        public DbSet<TchatLiveroom> TchatLiveroomSet { get; set; }

        /// <summary>
        /// 直播历史
        /// </summary>
        public DbSet<TchatLiveroomPeriodHis> TchatLiveroomPeriodHisSet { get; set; }

        /// <summary>
        /// 直播房间用户进出历史
        /// </summary>
        public DbSet<TchatLiveroomVisitHis> TchatLiveroomVisitHisSet { get; set; }

        /// <summary>
        /// 直播结算历史
        /// </summary>
        public DbSet<TchatLivesettleHis> TchatLivesettleHisSet { get; set; }

        /// <summary>
        /// P信我的好友
        /// </summary>
        public DbSet<TchatFriend> TchatFriendSet { get; set; }

        /// <summary>
        /// 好友昵称
        /// </summary>
        public DbSet<TchatFriendNick> TchatFriendNickSet { get; set; }

        /// <summary>
        /// P信群姐
        /// </summary>
        public DbSet<TchatGroup> TchatGroupSet { get; set; }

        /// <summary>
        /// 用户退群历史
        /// </summary>
        public DbSet<TchatGroupQuitlog> TchatGroupQuitlogSet { get; set; }

        /// <summary>
        /// P信群组用户
        /// </summary>
        public DbSet<TchatGroupUser> TchatGroupUserSet { get; set; }

        /// <summary>
        /// P信公众号
        /// </summary>
        public DbSet<TchatPublic> TchatPublicSet { get; set; }

        /// <summary>
        /// 聊天室
        /// </summary>
        public DbSet<TchatRoom> TchatRoomSet { get; set; }

        /// <summary>
        /// 聊天室日志
        /// </summary>
        public DbSet<TchatRoomLog> TchatRoomLogSet { get; set; }

        /// <summary>
        /// P信用户
        /// </summary>
        public DbSet<TchatUser> TchatUserSet { get; set; }

        /// <summary>
        /// P信用户
        /// </summary>
        public DbSet<TchatUserFull> TchatUserFullSet { get; set; }


        /// <summary>
        /// 用户等级表
        /// </summary>
        public DbSet<TnetUsergrade> TnetUsergradeSet { get; set; }

        /// <summary>
        /// P信群组禁言用户
        /// </summary>
        public DbSet<TchatGroupUsergag> TchatGroupUsergagSet { get; set; }


        /// <summary>
        /// 直播间礼物钱包
        /// </summary>
        public DbSet<TchatLivegiftPurse> TchatLivegiftPurseSet { get; set; }

        /// <summary>
        /// 红包使用钱包
        /// </summary>
        public DbSet<TchatRedenvePurse> TchatRedenvePurseSet { get; set; }

        /// <summary>
        /// 发送红包历史
        /// </summary>
        public DbSet<TchatRedenveSendhis> TchatRedenveSendhisSet { get; set; }

        /// <summary>
        /// 抢红包历史
        /// </summary>
        public DbSet<TchatRedenveOpenhis> TchatRedenveOpenhisSet { get; set; }


        /// <summary>
        /// 直播间粉丝关注历史
        /// </summary>
        public DbSet<TchatLiveFans> TchatLiveFansSet { get; set; }

        /// <summary>
        /// 激活码表
        /// </summary>
        public DbSet<TbtcActivation> TbtcActivationSet { get; set; }

        /// <summary>
        /// 用户等级变化历史
        /// </summary>
        public DbSet<TchatGradeChange> TchatGradeChangeSet { get; set; }

        /// <summary>
        /// 群组发送历史
        /// </summary>
        public DbSet<TchatGroupSendHis> TchatGroupSendHisSet { get; set; }

        /// <summary>
        /// p信过滤词
        /// </summary>
        public DbSet<TchatFilterword> TchatFilterwordSet { get; set; }
        /// <summary>
        /// 直播间冻结历史
        /// </summary>
        public DbSet<TchatLiveroomFreezeHis> TchatLiveroomFreezeHisSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<VchatGroup> VchatGroupSet { get; set; }


        /// <summary>
        /// 省
        /// </summary>
        public DbSet<VnetProvince> VnetProvinceSet { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public DbSet<VnetCity> VnetCitySet { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public DbSet<VnetRegion> VnetRegionSet { get; set; }


        /// <summary>
        /// 信友圈评论历史
        ///</summary>
        public DbSet<TpxinCommentHis> TpxinCommentHisSet { get; set; }
        /// <summary>
        /// 评论用户推送表
        ///</summary>
        public DbSet<TpxinCommentHisUser> TpxinCommentHisUserSet { get; set; }
        /// <summary>
        /// 信友圈消息表
        ///</summary>
        public DbSet<TpxinMessage> TpxinMessageSet { get; set; }
        /// <summary>
        /// 朋友圈推送用户表
        ///</summary>
        public DbSet<TpxinMessageUesr> TpxinMessageUesrSet { get; set; }
        /// <summary>
        /// 信友圈V点变化历史表
        ///</summary>
        public DbSet<TpxinPayhis> TpxinPayhisSet { get; set; }
        /// <summary>
        /// 信友圈赞踩历史表
        ///</summary>
        public DbSet<TpxinPraise> TpxinPraiseSet { get; set; }
        /// <summary>
        /// 信友圈用户基本信息
        ///</summary>
        public DbSet<TpxinUserinfo> TpxinUserinfoSet { get; set; }

        /// <summary>
        /// 信友圈消息视图
        ///</summary>
        public DbSet<VpxinMessage> VpxinMessageSet { get; set; }

        /// <summary>
        /// P点支付历史
        /// </summary>
        public DbSet<VpxinPraise> VpxinPraiseSet { get; set; }

        /// <summary>
        /// v点支付历史
        /// </summary>
        public DbSet<VpxinPayhis> VpxinPayhisSet { get; set; }

        public DbSet<TnetUepayhis> TnetUepayhisSet { get; set; }

        public DbSet<TpcnUepayconfig> TpcnUepayconfigSet { get; set; }

        public DbSet<TpxinReport> TpxinReportSet { get; set; }

        /// <summary>
        /// 头像表
        /// </summary>
        public DbSet<TnetUserphoto> TnetUserphotoSet { get; set; }

        /// <summary>
        /// 附近的人
        /// </summary>
        public DbSet<TpxinNearby> TpxinNearbySet { get; set; }
        /// <summary>
        /// 摇一摇
        /// </summary>
        public DbSet<TpxinYaoyiyao> TpxinYaoyiyaoSet { get; set; }
        /// <summary>
        /// 用户注册验证码表
        /// </summary>
        public DbSet<TssoRegcode> TssoRegcodeSet { get; set; }
        /// <summary>
        /// 用户账号记录表
        /// </summary>
        public DbSet<TssoUsercode> TssoUsercodeSet { get; set; }
        /// <summary>
        /// 注册、签到、登录历史表
        /// </summary>
        public DbSet<TueLoginHis> TueLoginHisSet { get; set; }
        /// <summary>
        /// 开放平台用户注册信息
        /// </summary>
        public DbSet<TssoOpenUser> TssoOpenUserSet { get; set; }

        public DbSet<TnetLockuser> TnetLockuserSet { get; set; }
        /// <summary>
        /// 用户信息表扩展
        /// </summary>
        public DbSet<TnetReginfoExt> TnetReginfoExtSet { get; set; }
        /// <summary>
        /// 客服操作历史
        /// </summary>
        public DbSet<TcsChangeHis> TcsChangeHisSet { get; set; }
        /// <summary>
        /// 子账号关系表
        /// </summary>
        public DbSet<TblcUserPurseSub2> TblcUserPurseSub2Set { get; set; }
        /// <summary>
        /// 消息/站内信
        /// </summary>
        public DbSet<TueMessage> TueMessageSet { get; set; }
        /// <summary>
        /// 阿里短信模板
        /// </summary>
        public DbSet<TsmsAlitemplate> TsmsAlitemplateSet { get; set; }
        /// <summary>
        /// app配置信息表
        /// </summary>
        public DbSet<TappConfig> TappConfigSet { get; set; }
        /// <summary>
        /// 注册邀请历史
        /// </summary>
        public DbSet<TnetInvitehis> TnetInvitehisSet { get; set; }
        /// <summary>
        /// 注册邀请历史
        /// </summary>
        public DbSet<TpxinAmountChangeHis> TpxinAmountChangeHisSet { get; set; }
        /// <summary>
        /// 充值商/代理人信息表
        /// </summary>
        public DbSet<TblUserJxs> TblUserJxsSet { get; set; }
        /// <summary>
        /// 充值商/代理人进出货历史
        /// </summary>
        public DbSet<TblUserJxsStockhis> TblUserJxsStockhisSet { get; set; }
        /// <summary>
        /// 充值商/代理人续费历史表
        /// </summary>
        public DbSet<TblUserJxsXf> TblUserJxsXfSet { get; set; }
        /// <summary>
        /// 充值商/代理人进出货配置表
        /// </summary>
        public DbSet<TblUserJxsStockconfig> TblUserJxsStockconfigSet { get; set; }
        /// <summary>
        /// 相信可兑换商品
        /// </summary>
        public DbSet<TpxinChargeProduct> TpxinChargeProductSet { get; set; }
        /// <summary>
        /// 充值码历史表
        /// </summary>
        public DbSet<TblcCentcardHis> TblcCentcardHisSet { get; set; }
        /// <summary>
        /// 卡表
        /// </summary>
        public DbSet<TblcCentcard> TblcCentcardSet { get; set; }
        /// <summary>
        /// 通用钱包配置表
        /// </summary>
        public DbSet<TnetPurseConfig> TnetPurseConfigSet { get; set; }
        /// <summary>
        /// 相信APP SVC充值码配置表
        /// </summary>
        public DbSet<TblcCentcardConfig> TblcCentcardConfigSet { get; set; }
        /// <summary>
        /// btc充值日志
        /// </summary>
        public DbSet<TblcBtcChargeLog> TblcBtcChargeLogSet { get; set; }
        /// <summary>
        /// 零售码库存零售历史
        /// </summary>
        public DbSet<TblUserJxsStockhis2> TblUserJxsStockhis2Set { get; set; }
        /// <summary>
        /// 用户P指数历史表
        /// </summary>
        public DbSet<TpcnIndexUser> TpcnIndexUserSet { get; set; }
        /// <summary>
        /// 兑换历史表
        /// </summary>
        public DbSet<TpxinChargeHis> TpxinChargeHisSet { get; set; }
        /// <summary>
        /// 功能开通表
        /// </summary>
        public DbSet<TnetOpenInfo> TnetOpenInfoSet { get; set; }
        /// <summary>
        /// 竞拍历史
        /// </summary>
        public DbSet<TpxinPaiHis> TpxinPaiHisSet { get; set; }
        /// <summary>
        /// A点竞拍配置表
        /// </summary>
        public DbSet<TpxinPaiConfig> TpxinPaiConfigSet { get; set; }
        /// <summary>
        /// 相信红包信息表
        /// </summary>
        public DbSet<TbtcYdTransferHis> TbtcYdTransferHisSet { get; set; }
        /// <summary>
        /// 相信红包信息扩展表
        /// </summary>
        public DbSet<TbtcYdTransferHisExt2> TbtcYdTransferHisExt2Set { get; set; }
        /// <summary>
        /// 用户A点信息（数据来源于服务）
        /// </summary>
        public DbSet<TpxinPaiUser> TpxinPaiUserSet { get; set; }
        /// <summary>
        /// P指数基本信息表
        /// </summary>
        public DbSet<TpcnIndexInfo> TpcnIndexInfoSet { get; set; }
        /// <summary>
        /// P指数变化历史表
        /// </summary>
        public DbSet<TpcnIndexHis> TpcnIndexHisSet { get; set; }
        /// <summary>
        /// p指数结算表
        /// </summary>
        public DbSet<TpcnIndexSettle> TpcnIndexSettleSet { get; set; }

        /// <summary>
        /// url地址转换规则表
        /// </summary>
        public DbSet<TnetUrlJump> TnetUrlJumpSet { get; set; }

        /// <summary>
        /// 聊天倍率设置表
        /// </summary>
        public DbSet<TchatRate> TchatRateSet { get; set; }
        /// <summary>
        /// 电话号码国标代码表
        /// </summary>
        public DbSet<TnetAreacode> TnetAreacodeSet { get; set; }
        /// <summary>
        /// 用户协议
        /// </summary>
        public DbSet<TnetUserAgreement> TnetUserAgreementSet { get; set; }
        /// <summary>
        /// 用户协议分类
        /// </summary>
        public DbSet<TnetUserProtocal> TnetUserProtocalSet { get; set; }
        /// <summary>
        /// 会议信息表
        /// </summary>
        public DbSet<TbossMeetinfo> TbossMeetinfoSet { get; set; }
        /// <summary>
        /// 会议报名历史表
        /// </summary>
        public DbSet<TbossMeethis> TbossMeethisSet { get; set; }
        /// <summary>
        /// 更换充值商历史表
        /// </summary>
        public DbSet<TblUserJxsChange> TblUserJxsChangeSet { get; set; }
        /// <summary>
        /// 充值商新增代理人确认表
        /// </summary>
        public DbSet<TblUserJxsConfirm> TblUserJxsConfirmSet { get; set; }
        /// <summary>
        /// 聊天计费历史表
        /// </summary>
        public DbSet<TchatFeehis> TchatFeehisSet { get; set; }
        /// <summary>
        /// 提取码基本信息表
        /// </summary>
        public DbSet<Ttqm2Info> Ttqm2InfoSet { get; set; }

        /// <summary>
        /// 意见反馈表
        /// </summary>
        public DbSet<TappFeedback> TappFeedbackSet { get; set; }

        /// <summary>
        /// 相信A点竟拍抽奖
        /// </summary>
        public DbSet<TpxinLuckDraw> TpxinLuckDrawSet { get; set; }

        /// <summary>
        /// 用户中奖详情
        /// </summary>
        public DbSet<TpxinLuckDrawDetail> TpxinLuckDrawDetailSet { get; set; }

        /// <summary>
        /// 用户认证
        /// </summary>
        public DbSet<TzcAuthLog> TzcAuthLogSet { get; set; }

        /// <summary>
        /// 身份证识别返回数据表
        /// </summary>
        public DbSet<TzcIdcardrecLog> TzcIdcardrecLogSet { get; set; }

        /// <summary>
        /// 注册信息表，对应个人用户
        /// </summary>
        public DbSet<TnetNodeinfo> TnetNodeinfoSet { get; set; }

        /// <summary>
        /// 行驶证历史表
        ///</summary>
        public DbSet<TnetVehicleLicLog> TnetVehicleLicLogSet { get; set; }

        /// <summary>
        /// 驾使证识别历史
        ///</summary>
        public DbSet<TnetDriveLicLog> TnetDriveLicLogSet { get; set; }

        /// <summary>
        /// 达人信息表
        ///</summary>
        public DbSet<TpxinDarenInfo> TpxinDarenInfoSet { get; set; }

        /// <summary>
        /// 推荐达人列表
        ///</summary>
        public DbSet<TpxinDarenDefault> TpxinDarenDefaultSet { get; set; }

        /// <summary>
        /// 达人扩展表(类型)
        ///</summary>
        public DbSet<TpxinDarenExt1> TpxinDarenExt1Set { get; set; }

        /// <summary>
        /// 达人扩展表2(职业/教育)
        ///</summary>
        public DbSet<TpxinDarenExt2> TpxinDarenExt2Set { get; set; }

        /// <summary>
        /// 达人类型表
        ///</summary>

        public DbSet<TpxinDarenType> TpxinDarenTypeSet { get; set; }

        /// <summary>
        /// 热门搜索关键字
        ///</summary>
        public DbSet<TpxinDarenSearch> TpxinDarenSearchSet { get; set; }

        /// <summary>
        /// 相信十月活动
        ///</summary>
        public DbSet<TpxinOctoberActivity> TpxinOctoberActivitySet { get; set; }

        /// <summary>
        /// YG认证绑定PCN账号表
        /// </summary>
        public DbSet<TzcAuthBindpcn> TzcAuthBindpcnSet { get; set; }

        /// <summary>
        /// PCN合作者
        /// </summary>
        public DbSet<TpcnThirdPartner> TpcnThirdPartnerSet { get; set; }

        /// <summary>
        /// PCN合作者支付历史
        /// </summary>
        public DbSet<TpcnThirdPayhis> TpcnThirdPayhisSet { get; set; }

        /// <summary>
        /// 表情包订单
        /// </summary>
        public DbSet<TpxinEmoticonOrder> TpxinEmoticonOrderSet { get; set; }

        /// <summary>
        /// 表情包素材
        /// </summary>
        public DbSet<TpxinEmoticonMaterial> TpxinEmoticonMaterialSet { get; set; }

        /// <summary>
        /// 收货人信息管理表
        /// </summary>
        public DbSet<TnetNodeconsigneeaddr> TnetNodeconsigneeaddrSet { get; set; }
        /// <summary>
        /// 竞拍历史-存放历史数据
        /// </summary>
        public DbSet<TpxinPaiHisOld> TpxinPaiHisOldSet { get; set; }
        /// <summary>
        /// 每月活动表
        /// </summary>
        public DbSet<TpxinActivity> TpxinActivitySet { get; set; }
        /// <summary>
        /// 每月活动绑定pcn帐号表
        /// </summary>
        public DbSet<TpxinActivityThirdparty> TpxinActivityThirdpartySet { get; set; }
        
        /// <summary>
        /// 达人浏览历史表
        /// </summary>
        public DbSet<TpxinDarenBrowseHis> TpxinDarenBrowseHisSet { get; set; }
        /// <summary>
        /// 达人知识库表
        /// </summary>
        public DbSet<TpxinDarenKnowledge> TpxinDarenKnowledgeSet { get; set; }
        /// <summary>
        /// 达人知识库查看历史表
        /// </summary>
        public DbSet<TpxinDarenKnowledgeHis> TpxinDarenKnowledgeHisSet { get; set; }
        /// <summary>
        /// 达人视频信息表
        /// </summary>
        public DbSet<TpxinDarenVideo> TpxinDarenVideoSet { get; set; }
        /// <summary>
        /// 表情热门搜索
        /// </summary>
        public DbSet<TpxinEmoticonSearch> TpxinEmoticonSearchSet { get; set; }

        /// <summary>
        /// 生成SVC限制规则表
        /// </summary>
        public DbSet<TpxinSvcLimit> TpxinSvcLimitSet { get; set; }

        /// <summary>
        /// 发票增票资质审核表
        /// </summary>
        public DbSet<TpxinInvoiceLimit> TpxinInvoiceLimitSet { get; set; }
        /// <summary>
        /// SVC充值码发票
        /// </summary>
        public DbSet<TblcCentcardInvoice> TblcCentcardInvoiceSet { get; set; }
        /// <summary>
        /// 码库购买/出售SVC充值卡历史表
        /// </summary>
        public DbSet<TblcCentcardAuctionHis> TblcCentcardAuctionHisSet { get; set; }
        

        /// <summary>
        /// APP本地H5页面配置
        /// </summary>
        public DbSet<TappH5Config> TappH5ConfigsSet { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new TchatLivegiftHisMap());
            //modelBuilder.Configurations.Add(new TchatLivegiftTypeMap());
            //modelBuilder.Configurations.Add(new TchatLiveroomMap());
            //modelBuilder.Configurations.Add(new TchatLiveroomPeriodHisMap());
            //modelBuilder.Configurations.Add(new TchatLiveroomVisitHisMap());
            //modelBuilder.Configurations.Add(new TchatLivesettleHisMap());
            //modelBuilder.Configurations.Add(new TblcUserPurseMap());
            //modelBuilder.Configurations.Add(new TchatFriendMap());
            //modelBuilder.Configurations.Add(new TchatFriendNickMap());
            //modelBuilder.Configurations.Add(new TchatGroupMap());
            //modelBuilder.Configurations.Add(new TchatGroupQuitlogMap());
            //modelBuilder.Configurations.Add(new TchatGroupUserMap());
            //modelBuilder.Configurations.Add(new TchatPublicMap());
            //modelBuilder.Configurations.Add(new TchatRoomMap());
            //modelBuilder.Configurations.Add(new TchatRoomLogMap());
            //modelBuilder.Configurations.Add(new TchatUserMap());
            //modelBuilder.Configurations.Add(new TnetReginfoMap());
            //modelBuilder.Configurations.Add(new TnetUsergradeMap());
            //modelBuilder.Configurations.Add(new TchatGroupUsergagMap());
            //modelBuilder.Configurations.Add(new TblcFreezeHisMap());
            //modelBuilder.Configurations.Add(new TchatLivegiftPurseMap());
            //modelBuilder.Configurations.Add(new TchatLiveFansMap());
            //modelBuilder.Configurations.Add(new TchatRedenvePurseMap());
            //modelBuilder.Configurations.Add(new TchatRedenveOpenhisMap());
            //modelBuilder.Configurations.Add(new TchatRedenveSendhisMap());
            //modelBuilder.Configurations.Add(new TbtcActivationMap());
            //modelBuilder.Configurations.Add(new TchatGradeChangeMap());
            //modelBuilder.Configurations.Add(new TchatGroupSendHisMap());
            //modelBuilder.Configurations.Add(new TblcCurrencyMap());

            //modelBuilder.Configurations.Add(new VnetProvinceMap());
            //modelBuilder.Configurations.Add(new VnetCityMap());
            //modelBuilder.Configurations.Add(new VnetRegionMap());
            InitFunction(modelBuilder);
            ModelBuilderHelper.ModelMaps.ForEach(item =>
            {
                dynamic configMap = Activator.CreateInstance(item);
                modelBuilder.Configurations.Add(configMap);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
