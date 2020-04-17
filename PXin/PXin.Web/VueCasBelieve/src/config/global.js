import axios from 'axios';
import { getUrlParams, getPhoneType, decode } from './utils';

//全局参数
const global = {};

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
global.setAppNativeHead = (status, callback) => {
  let Sign = [
    {"PStr": "eyJuYXZpdHlwZSI6MH0=", "Sign": "B45473F9D8711AB7E1B4B8413AFD5D91"},
    {"PStr": "eyJuYXZpdHlwZSI6MX0=", "Sign": "D1A97BBB39A4B528B295BDF1E9276FA5"},
    {"PStr": "eyJuYXZpdHlwZSI6Mn0=", "Sign": "131850A5C3530A2BCC988D2E47403250"}
  ];
  try {
    let appName = window.AppNative.AppName;
    console.log('>>>>>>>>>>>>>>>>>>>>>');
    // alert(appName)
    // if (appName == 'xiangxin') {
      AppNative.blJsTunedupNativeWithTypeParamSign (1014, Sign[status].PStr, Sign[status].Sign);
      callback;
    // }
  } catch (e) {
    Toast(e);
  }
};



export default global;
