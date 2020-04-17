const getUrlParams = name => {
  return (
    decodeURIComponent(
      (new RegExp("[?|&]" + name + "=" + "([^&;]+?)(&|#|;|$)").exec(
        location.href
      ) || [, ""])[1].replace(/\+/g, "%20")
    ) || null
  );
};
//全局参数
const utils = {};
//用户信息
utils.userInfo = {
  "nodeid": getUrlParams('nodeid'),
  "sid": getUrlParams('sid'),
  "tm": getUrlParams('tm'),
  "sign": getUrlParams('sign')
};

export default utils;
