/**
 * 获取时间
 * @param date
 * @returns {Date}
 */
export const getDate = (date) => {
    if (date instanceof Date) {
        return date
    }
    if (typeof date === 'string') {
        date = date.replace(/\-/g, '/')
        return new Date(date)
    }
    if (typeof date === 'number') {
        return new Date(date)
    }
    return new Date()
};

/**
 * 返回 YYYY-MM-DD hh:mm:ss
 * @param date
 * @returns {string}
 */
export const getYMDhms = date => {
    date = getDate(date)
    date.setHours(date.getHours() + 8)
    return date
        .toISOString()
        .match(/\d{4}-\d{2}-\d{2}(.)\d{2}:\d{2}:\d{2}/)[0]
        .replace(/T/g, ' ')
};

/**
 * 返回 YYYY-MM-DD
 * @param date
 * @returns {string}
 */
export const getYMD = date => {
    date = getDate(date)
    date.setHours(date.getHours() + 8)
    return date.toISOString().match(/\d{4}-\d{2}-\d{2}/)[0]
};

/**
 * 返回 hh:mm:ss
 * @param date
 * @returns {string}
 */
export const gethms = date => {
    date = getDate(date)
    return date.toTimeString().match(/\d{2}:\d{2}:\d{2}/)[0]
};

/**
 * 单数+前缀'0'
 * @param time
 * @returns {string}
 */
export const padDate = (time) => time < 10 ? '0' + time : time;

/**
 * 获取倒计时
 * @param date
 * @returns {string|number}
 */
export const formatDuring = date => {
    date = getDate(date)
    let time = new Date(date).getTime() - new Date().getTime();
    if (time < 0) return 0;
    if (time === 0) return 1;
    const days = parseInt(time / (1000 * 60 * 60 * 24));
    const hours = parseInt((time % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = parseInt((time % (1000 * 60 * 60)) / (1000 * 60));
    let seconds = (time % (1000 * 60)) / 1000;
    seconds = parseInt(seconds, 10);
    if (days > 0) {
        return days + "天" + hours + "小时" + minutes + "分钟" + seconds + "秒";
    } else if (hours > 0) {
        return hours + "小时" + minutes + "分钟" + seconds + "秒";
    } else if (minutes > 0) {
        return hours + "小时" + minutes + "分钟" + seconds + "秒";
    } else {
        return seconds + "秒";
    }
};
/**
 * 以某字符分隔字符串 separateStr(10000000000000, 3, ',') // 10,000,000,000,000
 * @param str
 * @param num
 * @param regStr
 * @returns {string}
 */
export const separateStr = (str, num, regStr) => {
    str = typeof str === 'string' ? str : str.toString();
    const reg = new RegExp("\(\\d)(?=(?:\\d{" + num + "})+$)", 'g');

    return str.replace(reg, '$1' + regStr);
};
/**
 * 隐藏/替换字符串中间几位 hideMidStr(18255558888, 3, 4, '****') // 182****8888
 * @param str
 * @param start
 * @param last
 * @param regStr
 * @returns {string}
 */
export const hideMidStr = (str, start, last, regStr) => { // start从前往后第几位，end从后往前第几位
    console.log(str, start, last)
    str = typeof str === 'string' ? str : str.toString();
    const reg = new RegExp("\^( . {" + start + "})(?:\\d+)(.{" + last + "})$");
    console.log(str.replace(reg, "$1" + regStr + "$2"))
    return str.replace(reg, "$1" + regStr + "$2");
}


/**
 * 金额每三位正数添加逗号(支持保留小数) sepMoney("18255178737", 2) // 10,000,000,000.00
 * @param str
 * @param len
 * @returns {string|number}
 */
export const sepMoney = (str, len) => {
    if (/[^0-9\.]/.test(str) || str == null || str == "") return 0;
    str = str.toString().replace(/^(\d*)$/, "$1.");
    str = (str + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    str = str.replace(".", ",");
    const re = /(\d)(\d{3},)/;
    while (re.test(str))
        str = str.replace(re, "$1,$2");
    str = str.replace(/,(\d\d)$/, ".$1");
    if (len == 0) { // 不带小数位(默认是有小数位)
        var a = str.split(".");
        if (a[1] == "00") {
            str = a[0];
        }
    }
    return str;
};
/**
 * 去掉字符之前的空格
 * @param string
 * @returns {string}
 */
export const trim = string => {
    return (string || '').replace(/^[\s\uFEFF]+|[\s\uFEFF]+$/g, '')
}
/**
 * 获取url参数的值
 * @param name
 * @returns {any}
 */
export const getUrlParam = (name) => {
    const reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    const r = window.location.search.substr(1).match(reg);
    return r ? decodeURIComponent(r[2]) : null;
};


/**
 * 返回链接中所有键值数组 [[key1, value1],[key2, value2]]
 * @param url
 * @returns {[]}
 */
export const getUrlParams = (url) => {
    url = url.substr(url.indexOf('?') + 1).split("&");
    let result = [];
    for (var i = 0; i < url.length; i++) {
        url[i] = url[i].split("=");
        result[i] = url[i];
    }
    return result;
};

/**
 * 返回链接中所有键值对象
 */
export const getUrlObj = () => {
    let qs = (location.search.length > 0 ? location.search.substring(1) : ""),
        args = {},
        items = qs.length ? qs.split("&") : [],
        item = null,
        name = null,
        value = null,
        i = 0,
        len = items.length;
    for (i = 0; i < len; i++) {
        item = items[i].split("=");
        name = decodeURIComponent(item[0]);
        value = decodeURIComponent(item[1]);
        if (name.length) {
            args[name] = value;
        }
    }
    return args;
}

/**
 * 判断是否是微信浏览器
 * @returns {boolean}
 */
export const is_weixin = () => {
    const ua = navigator.userAgent.toLowerCase();
    return ua.match(/MicroMessenger/i) == "micromessenger";
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
 * 判断图片是否存在并设置默认地址
 * @param pathImg 图片路径
 * @returns {string}
 */
export const validateImage = (pathImg, defaultUrl) => {
    let ImgObj = new Image();
    ImgObj.src = pathImg;
    let url;
    if (ImgObj.fileSize > 0 || (ImgObj.width > 0 && ImgObj.height > 0)) {
        url = pathImg;
    } else {
        url = 'http://global.ckv-test.sulink.cn/images/err/noneimg.png' || defaultUrl;
    }
    return url;
};
