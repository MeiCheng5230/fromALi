var baseUrl = 'http://client.xiang-xin.net';//抽奖的域名地址
var arrUrl = ['http://client.be.sulink.cn', 'http://client.be.sulink.cn'];//接口域名地址
function GetBaseurl() {
    var index = Math.round(Math.random());
    console.log(index);
    return arrUrl[index];
}
document.write("<script src='http://global.xiang-xin.net/js/appnative.js'></script>");