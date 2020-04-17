import { getUrlParam, getPhoneType, } from '../src/utils/utils';

const GLOBAL = {};

//用户信息
GLOBAL.USERINFO = {
  "nodeid": getUrlParam('nodeid'),
  "sid": getUrlParam('sid'),
  "tm": getUrlParam('tm'),
  "sign": getUrlParam('sign'),
  "client": getPhoneType(),
};

export default GLOBAL;

