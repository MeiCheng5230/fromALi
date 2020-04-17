import QRCode from "qrcodejs2";

/**
 * 获取url上的参数
 */
export const getUrlParams = name => {
  return (
    decodeURIComponent(
      (new RegExp("[?|&]" + name + "=" + "([^&;]+?)(&|#|;|$)").exec(
        location.href
      ) || [, ""])[1].replace(/\+/g, "%20")
    ) || null
  );
};

/**
 * 获取手机类型IOS||安卓
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
 * 存储sessionStorage
 */
export const setStore = (name, content) => {
  if (!name) return;
  if (typeof content !== "string") {
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
    case "[object RegExp]":
    case "[object String]":
      //进行字符串转换比较
      return "" + a === "" + b;
    case "[object Number]":
      //进行数字转换比较,判断是否为NaN
      if (+a !== +a) {
        return +b !== +b;
      }
      //判断是否为0或-0
      return +a === 0 ? 1 / +a === 1 / b : +a === +b;
    case "[object Date]":
    case "[object Boolean]":
      return +a === +b;
  }
  //如果是对象类型
  if (classNameA == "[object Object]") {
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
  if (classNameA == "[object Array]") {
    if (a.toString() == b.toString()) {
      return true;
    }
    return false;
  }
};

/**
 * 生成二维码
 */
export const createQRCode = (id, url) => {
  let $codeDiv = document.getElementById(id);
  new QRCode($codeDiv, {
    text: url,
    width: 60,
    height: 60,
    render: "canvas",
    colorDark: "#333333",
    colorLight: "#ffffff",
    correctLevel: QRCode.CorrectLevel.L
  });
};

/**
 * 判断图片是否存在
 */
export const validateImage = pathImg => {
  let ImgObj = new Image();
  ImgObj.src = pathImg;
  let url;
  if (ImgObj.fileSize > 0 || (ImgObj.width > 0 && ImgObj.height > 0)) {
    url = pathImg;
  } else {
    url = "http://global.ckv-test.sulink.cn/images/err/noneimg.png";
  }
  return url;
};

export const decode = input => {
  var _keyStr =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
  var output = "";
  var chr1, chr2, chr3;
  var enc1, enc2, enc3, enc4;
  var i = 0;
  input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
  while (i < input.length) {
    enc1 = _keyStr.indexOf(input.charAt(i++));
    enc2 = _keyStr.indexOf(input.charAt(i++));
    enc3 = _keyStr.indexOf(input.charAt(i++));
    enc4 = _keyStr.indexOf(input.charAt(i++));
    chr1 = (enc1 << 2) | (enc2 >> 4);
    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
    chr3 = ((enc3 & 3) << 6) | enc4;
    output = output + String.fromCharCode(chr1);
    if (enc3 != 64) {
      output = output + String.fromCharCode(chr2);
    }
    if (enc4 != 64) {
      output = output + String.fromCharCode(chr3);
    }
  }
  output = _utf8_decode(output);
  return output;
};
// private method for UTF-8 decoding
function _utf8_decode(utftext) {
  var string = "";
  var i = 0;
  var c = 0;
  var c1 = 0;
  var c2 = 0;
  while (i < utftext.length) {
    c = utftext.charCodeAt(i);
    if (c < 128) {
      string += String.fromCharCode(c);
      i++;
    } else if (c > 191 && c < 224) {
      c1 = utftext.charCodeAt(i + 1);
      string += String.fromCharCode(((c & 31) << 6) | (c1 & 63));
      i += 2;
    } else {
      c1 = utftext.charCodeAt(i + 1);
      c2 = utftext.charCodeAt(i + 2);
      string += String.fromCharCode(
        ((c & 15) << 12) | ((c1 & 63) << 6) | (c2 & 63)
      );
      i += 3;
    }
  }
  return string;
}

/**
 *
 *  Base64 encode / decode
 *  http://www.webtoolkit.info
 *
 **/
export const Base64 = {
  // private property
  _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

  // public method for encoding
  encode: function(input) {
    var output = "";
    var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
    var i = 0;

    input = Base64._utf8_encode(input);

    while (i < input.length) {
      chr1 = input.charCodeAt(i++);
      chr2 = input.charCodeAt(i++);
      chr3 = input.charCodeAt(i++);

      enc1 = chr1 >> 2;
      enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
      enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
      enc4 = chr3 & 63;

      if (isNaN(chr2)) {
        enc3 = enc4 = 64;
      } else if (isNaN(chr3)) {
        enc4 = 64;
      }

      output =
        output +
        this._keyStr.charAt(enc1) +
        this._keyStr.charAt(enc2) +
        this._keyStr.charAt(enc3) +
        this._keyStr.charAt(enc4);
    } // Whend

    return output;
  }, // End Function encode

  // public method for decoding
  decode: function(input) {
    var output = "";
    var chr1, chr2, chr3;
    var enc1, enc2, enc3, enc4;
    var i = 0;

    input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
    while (i < input.length) {
      enc1 = this._keyStr.indexOf(input.charAt(i++));
      enc2 = this._keyStr.indexOf(input.charAt(i++));
      enc3 = this._keyStr.indexOf(input.charAt(i++));
      enc4 = this._keyStr.indexOf(input.charAt(i++));

      chr1 = (enc1 << 2) | (enc2 >> 4);
      chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
      chr3 = ((enc3 & 3) << 6) | enc4;

      output = output + String.fromCharCode(chr1);

      if (enc3 != 64) {
        output = output + String.fromCharCode(chr2);
      }

      if (enc4 != 64) {
        output = output + String.fromCharCode(chr3);
      }
    } // Whend

    output = Base64._utf8_decode(output);

    return output;
  }, // End Function decode

  // private method for UTF-8 encoding
  _utf8_encode: function(string) {
    var utftext = "";
    string = string.replace(/\r\n/g, "\n");

    for (var n = 0; n < string.length; n++) {
      var c = string.charCodeAt(n);

      if (c < 128) {
        utftext += String.fromCharCode(c);
      } else if (c > 127 && c < 2048) {
        utftext += String.fromCharCode((c >> 6) | 192);
        utftext += String.fromCharCode((c & 63) | 128);
      } else {
        utftext += String.fromCharCode((c >> 12) | 224);
        utftext += String.fromCharCode(((c >> 6) & 63) | 128);
        utftext += String.fromCharCode((c & 63) | 128);
      }
    } // Next n

    return utftext;
  }, // End Function _utf8_encode

  // private method for UTF-8 decoding
  _utf8_decode: function(utftext) {
    var string = "";
    var i = 0;
    var c, c1, c2, c3;
    c = c1 = c2 = 0;

    while (i < utftext.length) {
      c = utftext.charCodeAt(i);

      if (c < 128) {
        string += String.fromCharCode(c);
        i++;
      } else if (c > 191 && c < 224) {
        c2 = utftext.charCodeAt(i + 1);
        string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
        i += 2;
      } else {
        c2 = utftext.charCodeAt(i + 1);
        c3 = utftext.charCodeAt(i + 2);
        string += String.fromCharCode(
          ((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63)
        );
        i += 3;
      }
    } // Whend

    return string;
  } // End Function _utf8_decode
};

export const getChatTime = time => {
  let targetTime = new Date(time);
  let targetLocaleString = targetTime.toLocaleString().substring(0, 10);
  if (getYYYYMMdd(0) == targetLocaleString) {
    //今天
    return targetTime.toLocaleTimeString();
  } else if (getYYYYMMdd(-1) == targetLocaleString) {
    //昨天
    return "昨天";
  } else if (
    getYYYYMMdd(-2) == targetLocaleString ||
    getYYYYMMdd(-3) == targetLocaleString ||
    getYYYYMMdd(-4) == targetLocaleString ||
    getYYYYMMdd(-5) == targetLocaleString ||
    getYYYYMMdd(-6) == targetLocaleString
  ) {
    let weekArray = ["日", "一", "二", "三", "四", "五", "六"];
    var week = new Date().getDay();
    return "星期" + weekArray[week];
  } else if (new Date().getFullYear() == targetTime.getFullYear()) {
    return (
      targetTime.getFullYear() +
      "/" +
      (targetTime.getMonth() + 1) +
      "/" +
      targetTime.getDate()
    );
  }
};

export const encodeUtf8 = text => {
  const code = encodeURIComponent(text);
  const bytes = [];
  for (var i = 0; i < code.length; i++) {
    const c = code.charAt(i);
    if (c === "%") {
      const hex = code.charAt(i + 1) + code.charAt(i + 2);
      const hexVal = parseInt(hex, 16);
      bytes.push(hexVal);
      i += 2;
    } else bytes.push(c.charCodeAt(0));
  }
  return bytes;
};

export const getDate = () => {
  var myDate = new Date();
  //获取当前年
  var year = myDate.getFullYear();
  //获取当前月
  var month = myDate.getMonth() + 1;
  //获取当前日
  var date = myDate.getDate();
  var h = myDate.getHours(); //获取当前小时数(0-23)
  var m = myDate.getMinutes(); //获取当前分钟数(0-59)
  var s = myDate.getSeconds();
  //获取当前时间
  return (
    year +
    "-" +
    conver(month) +
    "-" +
    conver(date) +
    " " +
    conver(h) +
    ":" +
    conver(m) +
    ":" +
    conver(s)
  );
};

export const GetSequence = () => {
  var date = new Date();
  let temp =
    date.getFullYear().toString() +
    conver(date.getMonth() + 1) +
    conver(date.getDate()) +
    conver(date.getHours()) +
    conver(date.getMinutes()) +
    conver(date.getSeconds()) +
    "00";
  return Number(temp) + Math.floor(Math.random() * 100);
};
//日期时间处理
function conver(s) {
  return s < 10 ? "0" + s : s;
}
function getYYYYMMdd(addDayCount) {
  var dd = new Date();
  dd.setDate(dd.getDate() + addDayCount); //addDayCount
  var y = dd.getFullYear();
  var m = dd.getMonth() + 1; //获取当前月份的日期
  var d = dd.getDate();
  return y + "/" + m + "/" + d;
}
