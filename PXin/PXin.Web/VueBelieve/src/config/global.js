import {getUrlParams, getPhoneType, decode, canvasDataURL} from './utils';
import {Toast} from 'vant';

//全局参数
const global = {};

let lang = getUrlParams('lang');
if (lang == undefined || lang == null) {
  lang = 'chs'
}


//用户信息
global.userInfo = {
  "nodeid": getUrlParams('nodeid'),
  "sid": getUrlParams('sid'),
  "tm": getUrlParams('tm'),
  "sign": getUrlParams('sign'),
  "client": getPhoneType()
};
global.errorShopImgUrl = 'http://global.ckv-test.sulink.cn/images/err/store.png';
global.errorGoodImgUrl = 'http://global.ckv-test.sulink.cn/images/err/noneimg.png';

global.isYouGu = window.AppNative.AppName == 'yougu' ? true : false;
global.decode = decode;
global.canvasDataURL = canvasDataURL;
global.lang = lang;
global.setAppNative = (id, StrParam, SignParam, callback) => {
  //id :1013  status:0,1,2  StrParam:不固定 SignParam：不固定  callback:其他或回调函数

  //id | 1013 跳PCN绑定
  //id | 1014 导航
  //id | 1003 ue支付
  //id | 1017 window.nativeNaviBarHeight回调 获取导航高度
  //status| 0：白色  1：主题色  2：透明
  //1009-未实名认证
  // 1010-已实名认证
  // 1015-未实名驾驶证
  // 1016-已实名驾驶证

  let Sign = [
    {"PStr": "eyJuYXZpdHlwZSI6MH0=", "Sign": "B45473F9D8711AB7E1B4B8413AFD5D91"},
    {"PStr": "eyJuYXZpdHlwZSI6MX0=", "Sign": "D1A97BBB39A4B528B295BDF1E9276FA5"},
    {"PStr": "eyJuYXZpdHlwZSI6Mn0=", "Sign": "131850A5C3530A2BCC988D2E47403250"}
  ];

  if (typeof (StrParam) == 'number') {
    SignParam = Sign[StrParam].Sign;
    StrParam = Sign[StrParam].PStr;
  }

  try {
    let appName = window.AppNative.AppName;
    if (appName == 'xiangxin') {
      AppNative.blJsTunedupNativeWithTypeParamSign(id, StrParam, SignParam);
      callback;
    }
  } catch (e) {
    Toast(e);
  }
};
export default global;
