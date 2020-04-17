/**
 * 临时存储localStorage
 * @param name 名称
 * @param content 内容
 */
export const setStore = (name, content) => {
    if (!name) return;
    if (typeof content !== 'string') {
      content = JSON.stringify(content);
    }
    window.sessionStorage.setItem(name, content);
  };
  
  /**
   * 获取localStorage
   * @param name 名称
   */
  export const getStore = name => {
    if (!name) return;
    return window.sessionStorage.getItem(name);
  };
  
  /**
   * 删除localStorage
   * @param name 名称
   */
  export const removeStore = name => {
    if (!name) return;
    window.sessionStorage.removeItem(name);
  };
  /**
   * 清空所有缓存
   */
  export const clearStore = () => {
    window.sessionStorage.clear();
  };
  
  /**
   * 获取url上的参数
   * @param name 名称
   */
  export const getUrlKey = (name) => {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null
  };
  
  /**
   * 获取手机型号 ios或安卓
   * @returns {number}
   */
  export const getPhoneType = () => {
    let type = 0;
    if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
      type = 1;
    }
    if (/android/i.test(navigator.userAgent)) {
      type = 2;
    }
    return type;
  };
  
  
  /**
   * 保留两位小数
   * @param value
   * @returns {string}
   * @constructor
   */
  export const FiltersPrice = function (val) {
    let Price = parseFloat(val);
    if(Price==0){
      return '0.00';
    }
    if(!Price){
      return '';
    }
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
  
  export const isEqual=(a, b)=>{
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


  export const formatSeconds = (value) => {
    // 时间格式
    var theTime = parseInt(value);// 秒
    var middle= 0;// 分
    var hour= 0;// 小时
    if(theTime >= 60) {
        middle= parseInt(theTime/60);
        theTime = parseInt(theTime%60);
        if(middle>= 60) {
            hour= parseInt(middle/60);
            middle= parseInt(middle%60);
        }
    }
    var result = ''
    if(parseInt(theTime)>=10) {
        result = "0"+":"+parseInt(theTime);
    } else {
        result = "0"+":"+"0"+parseInt(theTime);
    };
    if(middle >= 0 && parseInt(theTime)>=10) {
        result = parseInt(middle)+":"+parseInt(theTime);
    } else {
        result = parseInt(middle)+":"+"0"+parseInt(theTime);
    };
    if(hour> 0) {
        result = ""+parseInt(hour)+":"+result;
    };
    return result;
  }

  export const canvasDataURL=(img,type)=> {
    //生成canvas
    let canvas = document.createElement('canvas');
    let ctx = canvas.getContext('2d');
    // 指定canvas画布大小，该大小为最后生成图片的大小
    canvas.width = img.width/2;
    canvas.height = img.height/2;
    /* drawImage画布绘制的方法。(0,0)表示以Canvas画布左上角为起点，400，300是将图片按给定的像素进行缩小。
    如果不指定缩小的像素图片将以图片原始大小进行绘制，图片像素如果大于画布将会从左上角开始按画布大小部分绘制图片，最后的图片就是张局部图。*/ 
    ctx.drawImage(img, 0, 0, img.width/2, img.height/2);
    // 将绘制完成的图片重新转化为base64编码，file.file.type为图片类型，0.92为默认压缩质量
    let base64 = canvas.toDataURL(type, 0.92) 
    // 最后将base64编码的图片保存到数组中，留待上传。
    if(base64.length/1024>1024){
      canvasDataURL(img,type);
    }
    return base64;
}
