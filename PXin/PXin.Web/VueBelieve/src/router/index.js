
import {
  setStore,
  getUrlParams,
  getPhoneType,
  isEqual,
  getStore
} from "@/config/utils";
import Axios from 'axios';
import {
  Toast
} from 'vant';

// 充值码
const RechargeCode = () => import(/* webpackChunkName: "RechargeCode" */ '@/views/RechargeCode/RechargeCode');
const Mid = () => import(/* webpackChunkName: "Mid" */'@/views/RechargeCode/Mid');
const MyCode = () => import(/* webpackChunkName: "MyCode" */'@/views/RechargeCode/MyCode');
const Record = () => import(/* webpackChunkName: "Record" */'@/views/RechargeCode/Record');
const BuyCode = () => import(/* webpackChunkName: "BuyCode" */'@/views/RechargeCode/BuyCode');
const Retail = () => import(/* webpackChunkName: "Retail" */'@/views/RechargeCode/Retail');
const Recharge = () => import(/* webpackChunkName: "Recharge" */'@/views/RechargeCode/Recharge');
const Agreement = () => import(/* webpackChunkName: "Agreement" */'@/views/RechargeCode/agreement');
const Transfer = () => import(/* webpackChunkName: "Transfer" */'@/views/RechargeCode/Transfer');

// 认证资料
// const Authentication = () => import("@/views/authentication/index");
// const Personal = () => import("@/views/authentication/Personal");
// const Company = () => import("@/views/authentication/Company");
// const Replenish = () => import("@/views/authentication/Replenish");
// const Contract = () => import("@/views/authentication/Contract");

  // A点竞拍
const Aauction = () => import(/* webpackChunkName: "Aauction" */"@/views/A_auction/index");
const auctionRecord = () => import(/* webpackChunkName: "auctionRecord" */"@/views/A_auction/Record");
const MyRecord = () => import(/* webpackChunkName: "MyRecord" */"@/views/A_auction/MyRecord");
const auctionRule = () => import(/* webpackChunkName: "auctionRule" */"@/views/A_auction/Rule");
const MypointA = () => import(/* webpackChunkName: "MypointA" */"@/views/A_auction/MypointA");
const BidsAuction = () => import(/* webpackChunkName: "BidsAuction" */"@/views/A_auction/BidsAuction");

  // 兑换专区
const Exchange = () => import(/* webpackChunkName: "Exchange" */"@/views/Exchange/index");
const TransferInto = () => import(/* webpackChunkName: "TransferInto" */"@/views/Exchange/TransferInto");
const TransferOut = () => import(/* webpackChunkName: "TransferOut" */"@/views/Exchange/TransferOut");
const YGtransfer = () => import(/* webpackChunkName: "YGtransfer" */"@/views/Exchange/YGtransfer");
const SVCtransfer = () => import(/* webpackChunkName: "SVCtransfer" */"@/views/Exchange/SVCtransfer");
const Result = () => import(/* webpackChunkName: "Result" */"@/views/Exchange/Result");
const ChangeHis = () => import(/* webpackChunkName: "ChangeHis" */"@/views/Exchange/changeHis");
const BindAccount = () => import(/* webpackChunkName: "BindAccount" */"@/views/Exchange/BindAccount");

  // 红包
const Opening = () => import(/* webpackChunkName: "Opening" */"@/views/RedPacket/Opening");
const Receive = () => import(/* webpackChunkName: "Receive" */"@/views/RedPacket/Receive");
const Explain = () => import(/* webpackChunkName: "Explain" */"@/views/RedPacket/Explain");
const SVCexchange = () => import(/* webpackChunkName: "SVCexchange" */"@/views/RedPacket/Exchange");
const RewardList = () => import(/* webpackChunkName: "RewardList" */"@/views/RedPacket/RewardList");
const RewardDetail = () => import(/* webpackChunkName: "RewardDetail" */"@/views/RedPacket/RewardDetail");

//充值商代理人
const FbAp = () => import(/* webpackChunkName: "FbAp" */ "@/views/FbAp/Index");
const Rename = () => import(/* webpackChunkName: "Rename" */ "@/views/FbAp/Rename");
const StockRecord = () => import(/* webpackChunkName: "StockRecord" */ "@/views/FbAp/StockRecord");
const RetailStockRecord = () => import(/* webpackChunkName: "StockRecord" */ "@/views/FbAp/RetailStockRecord");
const WholesaleStockRecord = () => import(/* webpackChunkName: "StockRecord" */ "@/views/FbAp/WholesaleStockRecord");
const FbApList = () => import(/* webpackChunkName: "FbApList" */ "@/views/FbAp/FbApList");
const Audit = () => import(/* webpackChunkName: "Audit" */ "@/views/FbAp/Audit");
const IdentityIndex = () => import(/* webpackChunkName: "IdentityIndex" */ "@/views/FbAp/IdentityIndex");
const IdentityPerson = () => import(/* webpackChunkName: "IdentityPerson" */ "@/views/FbAp/IdentityPerson");
const IdentityCompany = () => import(/* webpackChunkName: "IdentityCompany" */ "@/views/FbAp/IdentityCompany");
const ContractPhotos = () => import(/* webpackChunkName: "ContractPhotos" */ "@/views/FbAp/ContractPhotos");
const Material = () => import(/* webpackChunkName: "Material" */ "@/views/FbAp/Material");
const Renew = () => import(/* webpackChunkName: "Renew" */ "@/views/FbAp/Renew");
const Stockin = () => import(/* webpackChunkName: "Stockin" */ "@/views/FbAp/Stockin");
const Add = () => import(/* webpackChunkName: "Add" */ "@/views/FbAp/Add");
const agree = () => import(/* webpackChunkName: "agree" */ "@/views/FbAp/agree");
const Signing = () => import(/* webpackChunkName: "Signing" */ "@/views/FbAp/Signing");
const ApplyCzs = () => import(/* webpackChunkName: "ApplyCzs" */ "@/views/FbAp/ApplyCzs");
const agentReq = () => import(/* webpackChunkName: "agentReq" */ "@/views/FbAp/agentReq");
const OctActivity = () => import(/* webpackChunkName: "OctActivity" */ "@/views/FbAp/OctActivity");
const OctActivityList = () => import(/* webpackChunkName: "OctActivityList" */ "@/views/FbAp/OctActivityList");
const logistics = () => import(/* webpackChunkName: "logistics" */ "@/views/FbAp/logistics");
const bindpcn = () => import(/* webpackChunkName: "bindpcn" */ "@/views/FbAp/bindpcn");
const Car = () => import(/* webpackChunkName: "Car" */ "@/views/FbAp/Car");

// 邀请码
const InvitationCode = () => import(/* webpackChunkName: "InvitationCode" */ "@/views/InvitationCode/index.vue");
const inviteList = () => import(/* webpackChunkName: "inviteList" */ "@/views/InvitationCode/inviteList.vue");


const router = new VueRouter({
  routes: [
// 充值码
    {   // 首页
      path: '/RechargeCode',
      name: 'RechargeCode',
      component: RechargeCode,
      meta:{
        title:'充值码'
      }

    },{   // 我的充值码
      path: '/MyCode',
      name: 'MyCode',
      component: MyCode,
      meta:{
        title:'我的充值码'
      }
    },{   // 充值记录
      path: '/rechargerecord',
      name: 'rechargerecord',
      component: Record,
      meta:{
        title:'充值码记录'
      }
    },{   // 购买充值码
      path: '/buycode',
      name: 'buycode',
      component: BuyCode,
      meta:{
        title:'购买充值码'
      }
    },{   // 零售
      path: '/retail',
      name: 'retail',
      component: Retail,
      meta:{
        title:'零售'
      }
    },{   // 充值
      path: '/recharge',
      name: 'recharge',
      component: Recharge,
      meta:{
        title:'充值'
      }
    },{   // 转让
      path: '/transfer',
      name: 'transfer',
      component: Transfer,
      meta:{
        title:'转让'
      }
    },{   // 协议
      path: '/agreement',
      name: 'agreement',
      component: Agreement,
      meta:{
        title:'协议'
      }
    },

// A点竞拍
    {   // A点竞拍首页
      path: '/auction',
      name: 'auction',
      component: Aauction,
      meta:{
        title:'A点竞拍'
      }
    },{   // 拍卖记录
      path: '/auctionRecord',
      name: 'auctionRecord',
      component: auctionRecord,
      meta:{
        title:'拍卖记录'
      }
    },{   // 我的竞拍记录
      path: '/myrecord',
      name: 'myrecord',
      component: MyRecord,
      meta:{
        title:'我的竞拍记录'
      }
    },{   // 竞拍规则
      path: '/auctionRule',
      name: 'auctionRule',
      component: auctionRule,
      meta:{
        title:'竞拍规则'
      }
    },{   // 我的A点
      path: '/MypointA',
      name: 'MypointA',
      component: MypointA,
      meta:{
        title:'我的A点'
      }
    },{   // 竞拍
      path: '/bidsAuction',
      name: 'bidsauction',
      component: BidsAuction,
      meta:{
        title:'A点竞拍'
      }
    },{
      path: '/addprice',
      name: 'addprice',
      component: () => import('@/views/A_auction/AddPrice'),
      meta:{
        title:'竞拍加价'
      }
    },{
      path: '/charts',
      name: 'charts',
      component: () => import('@/views/A_auction/Charts'),
      meta:{
        title:'拍卖记录详情'
      }
    },

// 兑换专区
    {   // 兑换专区首页
      path: '/exchange',
      name: 'exchange',
      component: Exchange,
      meta:{
        title:'兑换专区'
      }
    },{   // 转入
      path: '/transferInto',
      name: 'transferInto',
      component: TransferInto,
      meta:{
        title:'转入'
      }
    },{   // 转出
      path: '/transferOut',
      name: 'transferOut',
      component: TransferOut,
      meta:{
        title:'转出'
      }
    },{   // 优谷VIP兑换
      path: '/YGtransfer',
      name: 'YGtransfer',
      component: YGtransfer,
      meta:{
        title:'优谷VIP兑换'
      }
    },{   // 兑换成功
      path: '/result',
      name: 'result',
      component: Result,
      meta:{
        title:'兑换成功'
      }
    },{   // SVC兑换
      path: '/SVCtransfer',
      name: 'SVCtransfer',
      component: SVCtransfer,
      meta:{
        title:'SVC兑换'
      }
    },{
      //兑换历史
      path:'/ChangeHis',
      name:'ChangeHis',
      component:ChangeHis,
      meta:{
        title:'兑换记录'
      }
    },
    {
      path: '/bindaccount',
      name: 'binfaccount',
      component: BindAccount,
      meta:{
        title:'绑定码库账号'
      }
    },

// 红包
    {   // 开通专属账户
      path: '/opening',
      name: 'Opening',
      component: Opening,
      meta:{
        title:'开通账户'
      }
    },{   // 领取红包
      path: '/receive',
      name: 'receive',
      component: Receive,
      meta:{
        title:'领取红包'
      }
    },{   // 红包活动说明
      path: '/explain',
      name: 'explain',
      component: Explain,
      meta:{
        title:'领取红包活动说明'
      }
    },{   // 兑换
      path: '/SVCexchange',
      name: 'SVCexchange',
      component: SVCexchange,
      meta:{
        title:'兑换'
      }
    },{   // 奖励列表
      path: '/rewardlist',
      name: 'rewardlist',
      component: RewardList,
      meta:{
        title:'我的红包奖励'
      }
    },{   // 奖励明细
      path: '/RewardDetail',
      name: 'RewardDetail',
      component: RewardDetail,
      meta:{
        title:'奖励明细'
      }
    },

    //充值商代理人
    {
      path: '/FbAp',
      name: 'FbAp',
      component: FbAp,
      meta:{
        title:'充值商'
      }
    },
    {
      path: '/OctActivity',
      name: 'OctActivity',
      component: OctActivity,
      meta:{
        title:'10月送手机活动'
      }
    },
    {
      path: '/OctActivityList',
      name: 'OctActivityList',
      component: OctActivityList,
      meta:{
        title:'10月送手机活动'
      }
    },
    {
      path: '/Car',
      name: 'Car',
      component: Car,
      meta:{
        title:'充值商配车'
      }
    },
    {
      path: '/bindpcn',
      name: 'bindpcn',
      component: bindpcn,
      meta:{
        title:'绑定PCN'
      }
    },
    {
      path: '/logistics',
      name: 'logistics',
      component: logistics,
      meta:{
        title:'查看物流'
      }
    },
    {
      path: '/agentReq',
      name: 'agentReq',
      component: agentReq,
      meta:{
        title:'代理人请求'
      }
    },

    {
      path: '/ApplyCzs',
      name: 'ApplyCzs',
      component: ApplyCzs,
      meta:{
        title:'申请充值商'
      }
    },
    {
      path: '/Signing',
      name: 'Signing',
      component: Signing,
      meta:{
        title:'相信充值商服务协议'
      }
    },
    {
      path: '/Rename',
      name: 'Rename',
      component: Rename,
      meta:{
        title:'修改昵称'
      }
    },{
      path: '/StockRecord',
      name: 'StockRecord',
      component: StockRecord,
      meta:{
        title:'进货记录'
      }
    },{
      path: '/RetailStockRecord',
      name: 'RetailStockRecord',
      component: RetailStockRecord,
      meta:{
        title:'零售库存记录'
      }
    },{
      path: '/WholesaleStockRecord',
      name: 'WholesaleStockRecord',
      component: WholesaleStockRecord,
      meta:{
        title:'批发库存记录'
      }
    }
    ,{
      path: '/FbApList',
      name: 'FbApList',
      component: FbApList,
      meta:{
        title:'我的代理人'
      }
    },
    {
      path: '/Audit',
      name: 'Audit',
      component: Audit,
      meta:{
        title:'审核'
      }
    },{
      path: '/IdentityIndex',
      name: 'IdentityIndex',
      component: IdentityIndex,
      meta:{
        title:'认证资料'
      }
    },{
      path: '/IdentityPerson',
      name: 'IdentityPerson',
      component: IdentityPerson,
      meta:{
        title:'认证资料'
      }
    },{
      path: '/IdentityCompany',
      name: 'IdentityCompany',
      component: IdentityCompany,
      meta:{
        title:'公司资料认证'
      }
    },{
      path: '/ContractPhotos',
      name: 'ContractPhotos',
      component: ContractPhotos,
      meta:{
        title:'合同照片'
      }
    },{
      path: '/Material',
      name: 'Material',
      component: Material,
      meta:{
        title:'补充资料'
      }
    },{
      path: '/Renew',
      name: 'Renew',
      component: Renew,
      meta:{
        title:'续费'
      }
    },{
      path: '/Stockin',
      name: 'Stockin',
      component: Stockin,
      meta:{
        title:'进货'
      }
    },{
      path: '/agree',
      name: 'agree',
      component: agree,
      meta:{
        title:'2019相信促销活动服务协议'
      }
    },{
      path: '/Add',
      name: 'Add',
      component: Add,
      meta:{
        title:'新增代理人'
      }
    },{
      path: '/changeInfo',
      name: 'changeInfo',
      component: () => import("@/views/FbAp/changeInfo"),
      meta: {
        title: '我的充值商'
      }
    },{
      path: '/agent',
      name: 'agent',
      component: () => import("@/views/FbAp/Agent"),
      meta: {
        title: '代开充值商'
      }
    },

    // 邀请码
    {
      path: '/InvitationCode',
      name: 'InvitationCode',
      component: InvitationCode,
      meta: {
        title: '邀请码'
      }
    },{
      path: '/invitelist',
      name: 'invitelist',
      component: inviteList,
      meta: {
        title: '我邀请的好友'
      }
    },
    {
      path: '/notice',
      name: 'notice',
      component: () => import('@/views/Notice/index'),
      meta: {
        title: '公告列表'
      }
    },{
      path: '/noticeDetail',
      name: 'noticeDetail',
      component: () => import('@/views/Notice/notice'),
      meta: {
        title: '公告详情'
      }
    },

    {
      path: '/generate',
      name: 'generate',
      component: () => import('@/views/Fbap/Generate'),
      meta: {
        title: 'SV生成充值码'
      }
    }
  ]
})


//全局路由守卫

router.beforeEach((to, from, next) => {
if(to.name==="FbApList"){
    let zys = JSON.parse(getStore('FbApInfo'));
    if (zys.isczs===false) {
      to.meta.title = '我的充值商';
    }
  }
  //修改title
  if(to.name==="YGtransfer"||to.name==="SVCtransfer"||to.name==="recharge"){
     to.meta.title= getStore("nextTitle");
  }
  if(to.meta.title){
    document.title=to.meta.title;
  }

  if (to.name === "Mid" && (from.name === "exchange" || from.name === "receive" || from.name === "auction" || from.name === "FbAp" || from.name === 'binfaccount')) {
    setStore("home", "home");
  }

  if (to.name) {
    let usreInfo = {
      "nodeid": getUrlParams('nodeid'),
      "sid": getUrlParams('sid'),
      "tm": getUrlParams('tm'),
      "sign": getUrlParams('sign'),
      "client": getPhoneType()
    };
    Vue.prototype.i18n.locale =Vue.prototype.$global.lang;
     if (!usreInfo.nodeid || !usreInfo.sid || !usreInfo.tm || !usreInfo.sign) {
       next('/404');
       return;
     }
     let userParam = getStore('userParam');

     if (usreInfo && isEqual(JSON.parse(userParam), usreInfo)) { // 第一次验证通过后，存储，如果url参数不变不发请求
       next();
       return;
     }
    let url = '/api/Sys/CheckSign';
     Axios.post(url, usreInfo).then(res => {
       if (res.data.result > 0) {
         setStore('userParam', usreInfo); //存储
         next();
       } else {
         next('/404'); //验证不通过
       }
     }).catch(err => { //验证不通过 报错
       Toast('签名不正确');
       next('/404');
     })

   } else {
     next();
  }
});

export default router;
