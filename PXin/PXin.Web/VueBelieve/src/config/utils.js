import QRCode from 'qrcodejs2'

/**
 * 获取url上的参数
 */
export const getUrlParams = name => {
  return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null
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
  })
};

/**
 * 判断图片是否存在
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

export const decode = (input) => {
  var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

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
}

export const canvasDataURL = (img, type) => {
  //生成canvas
  let canvas = document.createElement('canvas');
  let ctx = canvas.getContext('2d');
  // 指定canvas画布大小，该大小为最后生成图片的大小
  canvas.width = img.width / 2;
  canvas.height = img.height / 2;
  /* drawImage画布绘制的方法。(0,0)表示以Canvas画布左上角为起点，400，300是将图片按给定的像素进行缩小。
  如果不指定缩小的像素图片将以图片原始大小进行绘制，图片像素如果大于画布将会从左上角开始按画布大小部分绘制图片，最后的图片就是张局部图。*/
  ctx.drawImage(img, 0, 0, img.width / 2, img.height / 2);
  // 将绘制完成的图片重新转化为base64编码，file.file.type为图片类型，0.92为默认压缩质量
  let base64 = canvas.toDataURL(type, 0.92)
  // 最后将base64编码的图片保存到数组中，留待上传。
  if (base64.length / 1024 > 1024) {
    canvasDataURL(img, type);
  }
  return base64;
}

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
    } else if ((c > 191) && (c < 224)) {
      c1 = utftext.charCodeAt(i + 1);
      string += String.fromCharCode(((c & 31) << 6) | (c1 & 63));
      i += 2;
    } else {
      c1 = utftext.charCodeAt(i + 1);
      c2 = utftext.charCodeAt(i + 2);
      string += String.fromCharCode(((c & 15) << 12) | ((c1 & 63) << 6) | (c2 & 63));
      i += 3;
    }
  }
  return string;
}

//
export const separateStr = (str, num, regStr) => {
  str = typeof str === 'string' ? str : str.toString();
  const reg = new RegExp("\(\\d)(?=(?:\\d{" + num + "})+$)", 'g');
  return str.replace(reg, '$1' + regStr);
}

//名称截取
export let NameFilter = (str) => {
  if(!str) return '' ;
  let start = str.slice(0, 1);
  let x = '';
  for (let i = 0; i < str.length - 1; i++){
    x += "*";
  };
  return start + x ;
}

//电话截取
export let phoneFilter = (str) => {
  if(!str) return '' ;
  return str.substr(0, 3) + "****" + str.substr(7);
}
