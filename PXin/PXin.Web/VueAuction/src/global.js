import { getUrlParams, getPhoneType, } from '@/config/utils.js';

const clobal = {};

//用户信息
clobal.userInfo = {
  "nodeid": getUrlParams('nodeid'),
  "sid": getUrlParams('sid'),
  "tm": getUrlParams('tm'),
  "sign": getUrlParams('sign'),
  "client": getPhoneType(),
};

export default clobal;

