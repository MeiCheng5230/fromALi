/**
 * 获取url上的参数
 * @param name
 * @returns {string | null}
 */
export const getUrlParams = name => {
  return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null
};


/**
 * 获取用户手机类型 0:pc 1:ios 2:安卓
 * @returns {number}
 */
export const getPhoneType = () => {
  let type = 0;
  if (/android/i.test(navigator.userAgent)) {
    type = 2;
  }
  if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
    type = 1;
  }
  return type;
};

/**
 * 判断图片是否存在
 * @param pathImg 图片路径
 * @returns {string}
 */
export const validateImage = (pathImg) => {
  let ImgObj = new Image();
  ImgObj.src = pathImg;
  let url;
  if (ImgObj.fileSize > 0 || (ImgObj.width > 0 && ImgObj.height > 0)) {
    url = pathImg;
  } else {
    url = 'http://global.ckv-test.sulink.cn/images/err/noneimg.png';

  }
  return url;
};

/**
 * 保留两位小数
 * @param val
 * @returns {string|boolean}
 * @constructor
 */
export const filtersPrice = function (val) {
  let Price = parseFloat(val);
  if (isNaN(Price)) {
    return false;
  }
  Price = Math.round(val * 100) / 100;
  Price = Price.toString();
  let res = Price.indexOf('.')
  if (res < 0) {
    res = Price.length;
    Price += '.';
  }
  while (Price.length <= res + 2) {
    Price += '0';
  }
  return Price;
};

/**
 *单数+前缀'0'
 * @param time
 * @returns {string}
 */
export const padDate = (time) => {
  return time < 10 ? '0' + time : time;
};


/**
 * 格式化为hh:mm:ss
 * @param date
 * @returns {string|number}
 */
export const formatDuring = (date) => {
  let time = date * 1000;
  if (time < 0) return 0;
  if (time === 0) return 1;
  let hours = parseInt(time / (1000 * 60 * 60));
  hours = padDate(hours);
  let minutes = parseInt((time % (1000 * 60 * 60)) / (1000 * 60));
  minutes = padDate(minutes);
  let seconds = (time % (1000 * 60)) / 1000;
  seconds = parseInt(seconds, 10);
  seconds = padDate(seconds);
  return hours + ":" + minutes + ":" + seconds;
}

/**
 * 存储sessionStorage
 */
export const setStore = (name, content) => {
  if (!name) return;
  if (typeof content !== 'string') {
    content = JSON.stringify(content);
  }
  window.sessionStorage.setItem(name, content);
};

/**
 * 获取sessionStorage
 */
export const getStore = name => {
  if (!name) return;
  return window.sessionStorage.getItem(name);
};

/**
 * 删除sessionStorage
 */
export const removeStore = name => {
  if (!name) return;
  window.sessionStorage.removeItem(name);
};

/**
 * 清空所有存储
 */
export const clearStore = () => {
  window.sessionStorage.clear();
};

export const isEqual = (a, b) => {
  //如果a和b本来就全等
  if (a === b) {
    //判断是否为0和-0
    return a !== 0 || 1 / a === 1 / b;
  }
  //判断是否为null和undefined
  if (a == null || b == null) {
    return a === b;
  }
  //接下来判断a和b的数据类型
  var classNameA = toString.call(a),
    classNameB = toString.call(b);
  //如果数据类型不相等，则返回false
  if (classNameA !== classNameB) {
    return false;
  }
  //如果数据类型相等，再根据不同数据类型分别判断
  switch (classNameA) {
    case '[object RegExp]':
    case '[object String]':
      //进行字符串转换比较
      return '' + a === '' + b;
    case '[object Number]':
      //进行数字转换比较,判断是否为NaN
      if (+a !== +a) {
        return +b !== +b;
      }
      //判断是否为0或-0
      return +a === 0 ? 1 / +a === 1 / b : +a === +b;
    case '[object Date]':
    case '[object Boolean]':
      return +a === +b;
  }
  //如果是对象类型
  if (classNameA == '[object Object]') {
    //获取a和b的属性长度
    var propsA = Object.getOwnPropertyNames(a),
      propsB = Object.getOwnPropertyNames(b);
    if (propsA.length != propsB.length) {
      return false;
    }
    for (var i = 0; i < propsA.length; i++) {
      var propName = propsA[i];
      //如果对应属性对应值不相等，则返回false
      if (a[propName] !== b[propName]) {
        return false;
      }
    }
    return true;
  }
  //如果是数组类型
  if (classNameA == '[object Array]') {
    if (a.toString() == b.toString()) {
      return true;
    }
    return false;
  }
};
/**
 * 设置本地缓存
 */
export const setLocalStorage=(key, value) =>{
  var curtime = new Date().getTime(); // 获取当前时间 ，转换成JSON字符串序列 
  var valueDate = JSON.stringify({
      val: value,
      timer: curtime
  });
  try {
      localStorage.setItem(key, valueDate);
  } catch(e) {
      /*if(e.name === 'QUOTA_EXCEEDED_ERR' || e.name === 'NS_ERROR_DOM_QUOTA_REACHED') {
          console.log("Error: 本地存储超过限制");
          localStorage.clear();
      } else {
          console.log("Error: 保存到本地存储失败");
      }*/
      // 兼容性写法
      if(isQuotaExceeded(e)) {
          console.log("Error: 本地存储超过限制");
          localStorage.clear();
      } else {
          console.log("Error: 保存到本地存储失败");
      }
  }
}

function isQuotaExceeded(e) {
  var quotaExceeded = false;
  if(e) {
      if(e.code) {
          switch(e.code) {
              case 22:
                  quotaExceeded = true;
                  break;
              case 1014: // Firefox 
                  if(e.name === 'NS_ERROR_DOM_QUOTA_REACHED') {
                      quotaExceeded = true;
                  }
                  break;
          }
      } else if(e.number === -2147024882) { // IE8 
          quotaExceeded = true;
      }
  }
  return quotaExceeded;
}
/**
 * 获取本地缓存
 * @param {*} key 
 * @param {*过期时间 毫秒} exp 
 */
export const getLocalStorage=(key,exp)=> {
  if(localStorage.getItem(key)) {
      var vals = localStorage.getItem(key); // 获取本地存储的值 
      var dataObj = JSON.parse(vals); // 将字符串转换成JSON对象
      // 如果(当前时间 - 存储的元素在创建时候设置的时间) > 过期时间 
      var isTimed = (new Date().getTime() - dataObj.timer) > exp;
      if(isTimed) {
          console.log("存储已过期");
          localStorage.removeItem(key);
          return null;
      } else {
          var newValue = dataObj.val;
      }
      return newValue;
  } else {
      return null;
  }
}

