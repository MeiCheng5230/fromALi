import Vue from 'vue'
import Router from 'vue-router'
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


const Home = () => import( /* webpackChunkName: "home" */ '@/view/Home.vue');
const NotFound = () => import(/* webpackChunkName: "404" */'@/components/common/404');
const NewGroupChat = () => import( /* webpackChunkName: "NewGroupChat" */ '@/view/Home/NewGroupChat');
const Chats = () => import( /* webpackChunkName: "Chats" */ '@/view/Home/Chats');
const Power = () => import( /* webpackChunkName: "Power" */ '@/view/Home/Power');
const Information = () => import( /* webpackChunkName: "Information" */ '@/view/Home/Information');
const TwoCode = () => import( /* webpackChunkName: "TwoCode" */ '@/view/Home/TwoCode');
const GroupChatInfo = () => import( /* webpackChunkName: "GroupChatInfo" */ '@/view/Home/GroupChatInfo');
const SetChat = () => import( /* webpackChunkName: "SetChat" */ '@/view/Home/SetChat');
const AddFriend = () => import( /* webpackChunkName: "AddFriend" */ '@/view/Home/AddFriend');
const AddressBookHome = () => import( /* webpackChunkName: "AddressBookHome" */ '@/view/AddressBook/Home');
const SetRemarks = () => import( /* webpackChunkName: "AddressBookHome" */ '@/view/AddressBook/SetRemarks');
const AddressSearch = () => import( /* webpackChunkName: "AddressSearch" */ '@/view/Home/AddressSearch');
const NewFriend = () => import( /* webpackChunkName: "NewFriend" */ '@/view/AddressBook/NewFriend');
const AddChat = () => import( /* webpackChunkName: "AddChat" */ '@/view/AddressBook/AddChat');
const AddGroupChat = () => import( /* webpackChunkName: "AddGroupChat" */ '@/view/AddressBook/AddGroupChat');
const AddressBookFriends = () => import( /* webpackChunkName: "AddressBookFriends" */ '@/view/AddressBook/AddressBookFriends');
const InviteFriend = () => import( /* webpackChunkName: "InviteFriend" */ '@/view/AddressBook/InviteFriend');
const MsgInvite = () => import( /* webpackChunkName: "MsgInvite" */ '@/view/AddressBook/MsgInvite');
const FindHome = () => import( /* webpackChunkName: "FindHome" */ '@/view/Find/Home');
const FindRock = () => import( /* webpackChunkName: "FindRock" */ '@/view/Find/FindRock');
const NearbyFriends = () => import( /* webpackChunkName: "NearbyFriends" */ '@/view/Find/NearbyFriends');
const Friends = () => import( /* webpackChunkName: "Friends" */ '@/view/Find/Friends');
const SomeonesFriends = () => import( /* webpackChunkName: "SomeonesFriends" */ '@/view/Find/SomeonesFriends');
const Report = () => import( /* webpackChunkName: "Report" */ '@/view/Find/Report');
const Publishs = () => import( /* webpackChunkName: "Publishs" */ '@/view/Find/Publishs');
const AddTest = () => import( /* webpackChunkName: "AddTest" */ '@/view/Find/AddTest');
const my = () => import( /* webpackChunkName: "my" */ '@/view/My/Home');
const PersonalInformation = () => import( /* webpackChunkName: "PersonalInformation" */ '@/view/My/PersonalInformation');
const SetNotes = () => import( /* webpackChunkName: "SetNotes" */ '@/view/My/SetNotes');
const AddV = () => import( /* webpackChunkName: "AddV" */ '@/view/My/AddV');
const AddDetail = () => import( /* webpackChunkName: "AddDetail" */ '@/view/My/AddDetail');
const Contact = () => import( /* webpackChunkName: "Contact" */ '@/view/My/Contact');
const TestPhone = () => import( /* webpackChunkName: "TestPhone" */ '@/view/My/TestPhone');
const ChangePhone = () => import( /* webpackChunkName: "ChangePhone" */ '@/view/My/ChangePhone');
const About = () => import( /* webpackChunkName: "About" */ '@/view/My/About');
const UE_Replace = () => import( /* webpackChunkName: "UE_Replace" */ '@/view/My/UE_Replace');
const UserSafety = () => import( /* webpackChunkName: "UserSafety" */ '@/view/My/UserSafety');
const ReplaceLoginPwd = () => import( /* webpackChunkName: "ReplaceLoginPwd" */ '@/view/My/ReplaceLoginPwd');
const ForgottenPwd = () => import( /* webpackChunkName: "ForgottenPwd" */ '@/view/My/ForgottenPwd');
const Wallet = () => import( /* webpackChunkName: "Wallet" */ '@/view/My/Wallet');
const WalletDetail = () => import( /* webpackChunkName: "WalletDetail" */ '@/view/My/WalletDetail');
const SwitchLang = () => import( /* webpackChunkName: "SwitchLang" */ '@/view/My/SwitchLang');
const Authorization = () => import( /* webpackChunkName: "Authorization" */ '@/view/login/Authorization');
const SwitchUser = () => import( /* webpackChunkName: "SwitchUser" */ '@/view/login/SwitchUser');
const ownAndOther = () => import( /* webpackChunkName: "ownAndOther" */ '@/view/Find/ownAndOther');

Vue.use(Router);

const router = new Router({
  routes: [{
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      title: '相信'
    }
  },
  {
    path: '/404:id',
    name: 'NotFound',
    component: NotFound
  },
  {//切换语言
    path: '/SwitchLang',
    name: 'SwitchLang',
    component: SwitchLang,
    meta: {
      title: '切换语言'
    }
  },
  { //忘记登录密码
    path: '/ForgottenPwd',
    name: 'ForgottenPwd',
    component: ForgottenPwd,
    meta: {
      title: '忘记登录密码'
    }
  },
  { //修改登录密码
    path: '/ReplaceLoginPwd',
    name: 'ReplaceLoginPwd',
    component: ReplaceLoginPwd,
    meta: {
      title: '修改登录密码'
    }
  },
  { //账户安全
    path: '/UserSafety',
    name: 'UserSafety',
    component: UserSafety,
    meta: {
      title: '账户安全'
    }
  },
  { //UE账户
    path: '/UE_Replace',
    name: 'UE_Replace',
    component: UE_Replace,
    meta: {
      title: 'UE账号'
    }
  },
  { //朋友验证
    path: '/AddTest',
    name: 'AddTest',
    component: AddTest,
    meta: {
      title: '朋友验证'
    }
  },
  { //授权
    path: '/Authorization',
    name: 'Authorization',
    component: Authorization,
    meta: {
      title: '授权'
    }
  },
  { //切换用户
    path: '/SwitchUser',
    name: 'SwitchUser',
    component: SwitchUser,
    meta: {
      title: '切换用户'
    }
  },
  { //发起群聊
    path: '/NewGroupChat',
    name: 'NewGroupChat',
    component: NewGroupChat,
    meta: {
      title: '发起群聊'
    }
  },
  { //添加朋友
    path: '/AddFriend',
    name: 'AddFriend',
    component: AddFriend,
    meta: {
      title: '添加朋友'
    }
  },
  { //搜索页面
    path: '/AddressSearch',
    name: 'AddressSearch',
    component: AddressSearch,
    meta: {
      title: '搜索'
    }
  },
  { //通讯录首页
    path: '/AddressBookHome',
    name: 'AddressBookHome',
    component: AddressBookHome,
    meta: {
      title: '通讯录'
    }
  },
  { //设置备注
    path: '/SetRemarks',
    name: 'SetRemarks',
    component: SetRemarks,
    meta: {
      title: '备注'
    }
  },
  { //新的朋友
    path: '/NewFriend',
    name: 'NewFriend',
    component: NewFriend,
    meta: {
      title: '新的朋友'
    }
  },
  { //群聊
    path: '/AddGroupChat',
    name: 'AddGroupChat',
    component: AddGroupChat,
    meta: {
      title: '群聊'
    }
  },
  { //通讯录朋友
    path: '/AddressBookFriends',
    name: 'AddressBookFriends',
    component: AddressBookFriends,
    meta: {
      title: '通讯录朋友'
    }
  },
  { //邀请好友
    path: '/InviteFriend',
    name: 'InviteFriend',
    component: InviteFriend,
    meta: {
      title: '邀请好友'
    }
  },
  { //短信邀请
    path: '/MsgInvite',
    name: 'MsgInvite',
    component: MsgInvite,
    meta: {
      title: '短信邀请'
    }
  },
  { //发现首页
    path: '/FindHome',
    name: 'FindHome',
    component: FindHome,
    meta: {
      title: '发现'
    }
  },
  { //摇一摇
    path: '/FindRock',
    name: 'FindRock',
    component: FindRock,
    meta: {
      title: '摇一摇'
    }
  },
  { //附近的人
    path: '/NearbyFriends',
    name: 'NearbyFriends',
    component: NearbyFriends,
    meta: {
      title: '附近的人'
    }
  },
  { //我的
    path: '/my',
    name: 'my',
    component: my,
    meta: {
      title: '我的'
    }
  },
  { //个人信息
    path: '/PersonalInformation',
    name: 'PersonalInformation',
    component: PersonalInformation,
    meta: {
      title: '个人信息'
    }
  },
  { //修改备注
    path: '/SetNotes',
    name: 'SetNotes',
    component: SetNotes,
    meta: {
      title: '修改备注'
    }
  },
  { //钱包
    path: '/Wallet',
    name: 'Wallet',
    component: Wallet,
    meta: {
      title: '钱包'
    }
  },
  { //钱包详情
    path: '/WalletDetail',
    name: 'WalletDetail',
    component: WalletDetail,
    meta: {
      title: '钱包详情'
    }
  },
  { //
    path: '/Chats',
    name: 'Chats',
    component: Chats
  },
  { // 设置接收倍率
    path: '/Power',
    name: 'Power',
    component: Power,
    meta: {
      title: '接收倍率'
    }
  },
  { // 详细信息
    path: '/Information',
    name: 'Information',
    component: Information,
    meta: {
      title: '详细信息'
    }
  },
  { // 聊天设置
    path: '/SetChat',
    name: 'SetChat',
    component: SetChat,
    meta: {
      title: '聊天设置'
    }
  },
  { // 我的二维码
    path: '/TwoCode',
    name: 'TwoCode',
    component: TwoCode,
    meta: {
      title: '我的二维码'
    }
  },
  { // 群聊信息
    path: '/GroupChatInfo',
    name: 'GroupChatInfo',
    component: GroupChatInfo,
    meta: {
      title: '群聊信息'
    }
  },
  { // 创建普通群聊
    path: '/AddChat',
    name: 'AddChat',
    component: AddChat,
    meta: {
      title: '创建普通群聊'
    }
  },
  { // 充值
    path: '/AddV',
    name: 'AddV',
    component: AddV,
    meta: {
      title: '充值'
    }
  },
  { // 充值明细
    path: '/AddDetail',
    name: 'AddDetail',
    component: AddDetail,
    meta: {
      title: '充值明细'
    }
  },
  { // 联系方式
    path: '/Contact',
    name: 'Contact',
    component: Contact,
    meta: {
      title: '联系方式'
    }
  },
  { // 验证原手机号
    path: '/TestPhone',
    name: 'TestPhone',
    component: TestPhone,
    meta: {
      title: '验证原手机号'
    }
  },
  { // 绑定新手机号
    path: '/ChangePhone',
    name: 'ChangePhone',
    component: ChangePhone,
    meta: {
      title: '绑定新手机号'
    }
  },
  { // 关于我们
    path: '/About',
    name: 'About',
    component: About,
    meta: {
      title: '关于我们'
    }
  },
  { // 信友圈
    path: '/Friends',
    name: 'Friends',
    component: Friends,
    meta: {
      title: '信友圈'
    }
  },
  { // 朋友信友圈
    path: '/SomeonesFriends',
    name: 'SomeonesFriends',
    component: SomeonesFriends,
    meta: {
      title: '信友圈'
    }
  },
  { // 举报
    path: '/Report',
    name: 'Report',
    component: Report,
    meta: {
      title: '举报'
    }
  },
  { // 发布
    path: '/Publishs',
    name: 'Publishs',
    component: Publishs,
    meta: {
      title: '发布朋友圈'
    }
  },
  { // 看自己和看别人的信友圈
    path: '/ownAndOther',
    name: 'ownAndOther',
    component: ownAndOther,
    meta: {
      title: '信友圈'
    }
  },
  ]
})
//全局路由守卫

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title;
  }
  if (to.name) {
    let userInfo = {
      "nodeid": getUrlParams('nodeid'),
      "sid": getUrlParams('sid'),
      "tm": getUrlParams('tm'),
      "sign": getUrlParams('sign'),
      "client": getPhoneType()
    };
    Vue.prototype.$global.setAppNativeHead(2);
    if (!userInfo.nodeid || !userInfo.sid || !userInfo.tm || !userInfo.sign) {
      next('/404/1');
      return;
    }
    let userParam = getStore('userParam');

    if (userInfo && isEqual(JSON.parse(userParam), userInfo)) { // 第一次验证通过后，存储，如果url参数不变不发请求
      next();
      return;
    }
    let url = '/api/Sys/CheckSign';
    Axios.post(url, userInfo).then(res => {
      if (res.data.result > 0) {
        setStore('userParam', userInfo); //存储
        next();
        
      } else {
        next('/404/1'); //验证不通过
      }
    }).catch(err => { //验证不通过 报错
      Toast.error('签名不正确');
      next('/404/1');
    })
  } else {
    next();
  }
});

export default router;
