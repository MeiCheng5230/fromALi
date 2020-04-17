import { formatSearch,getUrlParams, getPhoneType } from './utils';

//全局参数
const global = {};

//用户信息
global.userInfo={
  "nodeid": getUrlParams('nodeid'),
  "sid": getUrlParams('sid'),
  "tm": getUrlParams('tm'),
  "sign": getUrlParams('sign'),
  "client":getPhoneType()
};
//用户其他信息
global.userInfoApp={
  // 绑定的第三方账号类型 ：1-QQ、2-微信、3-微博、4-Pcn、5-优谷
  "opentype":getUrlParams('opentype')||0,
  //APP版本号
  "version":getUrlParams('version')||'web',
  // 推荐人账号/手机/邮箱,可为空 ,
  "pnodecode":getUrlParams('r')||'',
  // 第三方账号 没有就是 QQ 123456789
  "openid":getUrlParams('openid')||'',
};
export default global;
