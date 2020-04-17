fnResize();
window.onresize = function () {
    fnResize();
};
function fnResize() {
    var deviceWidth = document.documentElement.clientWidth || window.innerWidth;
    if (deviceWidth >= 750) {
        deviceWidth = 750;
    }
    if (deviceWidth <= 320) {
        deviceWidth = 320;
    }
    document.documentElement.style.fontSize = (deviceWidth / 7.5) + 'px';
};

$(function() {
    //判断手机类型  android ios
    var u = navigator.userAgent;
    var isAndroid = u.indexOf("Android") > -1 || u.indexOf("Adr") > -1; //android终端
    var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端

    var ImgUrl;
    if (!!isAndroid) {
        ImgUrl = './images/android.png';
    }
    if (!!isiOS) {
        ImgUrl = './images/ios.png';
    }
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/ qq/i) == ' qq' || ua.match(/MicroMessenger/i) == 'micromessenger') {
        //qq内置浏览器 或 微信内置浏览器
        $('.hintImg').fadeIn(200).find('img').attr('src', ImgUrl);
        return;
    };

    $("#download").click(function() {
        window.location.href = "http://client.xiang-xin.net/download/index.html";
    });
    $("#openapp").click(function() {
        if(isiOS) {
            window.location.href = "blChat://talentShare?userId="+getQueryVariable('nodeid');
        } else if(isAndroid) {
            window.location.href = "believe://talentShare?userId="+getQueryVariable('nodeid');
        }
    })
})
