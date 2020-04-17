export const formatSearch = se => {
  if (typeof se !== "undefined") {
    se = se.substr(1);
    var arr = se.split("&"),
    obj = {},
    newarr = [];
    arr.forEach(function (item,index) {
      newarr = item.split("=");
      if (typeof obj[newarr[0]] === "undefined") {
        obj[newarr[0]] = newarr[1];
      }
    });
    return obj;
  }
};

/**
 * 获取手机类型IOS||安卓
 */
export const getPhoneType =()=> {
  let type = 0;
  if(/android/i.test(navigator.userAgent)){
    type = 2;
  }
  if(/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)){
    type = 1;
  }
  return type;
};

/**
 * 获取url上的参数
 */
export const getUrlParams = name => {
  return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null
};
