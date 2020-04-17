import { getUrlKey, getPhoneType } from './utils'
// import { imgUrl } from "./baseUrl";
import { FiltersPrice } from './utils'

const global = {};

/**
 * 用户信息
 * @type {{nodeid: string, sid: string, tm: string, sign: string, client: *}}
 */
global.userInfo = {
  "nodeid":getUrlKey('nodeid'),
  "sid": getUrlKey('sid'),
  "tm": getUrlKey('tm'),
  "sign": getUrlKey('sign'),
  "client": getPhoneType()
};

//图片前缀地址链接
// global.imgUrl=imgUrl;

//保留两位小数
global.FiltersPrice=FiltersPrice;

global.setAppNativeHead = (status, callback) => {
  let Sign = [
    {"PStr": "eyJuYXZpdHlwZSI6MH0=", "Sign": "B45473F9D8711AB7E1B4B8413AFD5D91"},
    {"PStr": "eyJuYXZpdHlwZSI6MX0=", "Sign": "D1A97BBB39A4B528B295BDF1E9276FA5"},
    {"PStr": "eyJuYXZpdHlwZSI6Mn0=", "Sign": "131850A5C3530A2BCC988D2E47403250"}
  ];
  try {
    let appName = window.AppNative.AppName;
    if (appName == 'xiangxin') {
      AppNative.blJsTunedupNativeWithTypeParamSign (1014, Sign[status].PStr, Sign[status].Sign);
      callback;
    }
  } catch (e) {
    Toast(e);
  }
};

export default global;