(function () {
  //alert(typeof (window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign.postMessage));
  //var appName = navigator.appName.toLowerCase();
  //var appName = typeof (PCNWebInteration) == "undefined" ? "yougu" : "pcn";
  //屏幕截图      jsTunedupNativeWithTypeParamSign("1013","","") 回调 nativeScreenShot("图片base64")
  //修改导航风格  jsTunedupNativeWithTypeParamSign("1014",BASE64("{"navitype":1}"),SignStr) navitype 0 = 白，1 = 黑，2 = 透明
  var appNative = {
    AppName: "",
    jsTunedupNativeWithTypeParamSign: function (typeid, body, sign) {
      alert("不支持的方法");
    },
    ygJsTunedupNativeWithTypeParamSign: function (typeid, body, sign) {
      alert("不支持的方法-ygJsTunedupNativeWithTypeParamSign");
    },
    blJsTunedupNativeWithTypeParamSign: function (typeid, body, sign) {
      alert("不支持的方法-blJsTunedupNativeWithTypeParamSign");
    }
  };

  if (typeof (PCNWebInteration) != "undefined") {
    appNative.AppName = "pcn";
    appNative.jsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      PCNWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
    appNative.ygJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      //alert("ygJsTunedupNativeWithTypeParamSign");
      YGWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
    //alert("PCNWebInteration.jsTunedupNativeWithTypeParamSign");
  }
  else if (typeof (window.webkit) != "undefined"
    && typeof (window.webkit.messageHandlers) != "undefined"
    && typeof (window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign) != "undefined"
    && typeof (window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign.postMessage) != "undefined") {
    appNative.AppName = "pcn";
    appNative.jsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
    appNative.ygJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      //alert("ygJsTunedupNativeWithTypeParamSign");
      window.webkit.messageHandlers.ygJsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
    //alert("window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign.postMessage");
  }
  else if (typeof (YGWebInteration) != "undefined") {
    appNative.AppName = "yougu";
    appNative.jsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      YGWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
    appNative.ygJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      //alert("ygJsTunedupNativeWithTypeParamSign");
      if (typeid == 2001) {
        //alert(typeid);
        YGWebInteration.buyMembershipCard(atob(body));
      } else if (typeid == 1005) {
        var data = JSON.parse(atob(body));
        //YGWebInteration.shareWithShareTypeAndShareMsgAndShareContentAndShareUrlAndShareIconUrl(type, title, content, url, pic);
        YGWebInteration.shareWithShareTypeAndShareMsgAndShareContentAndShareUrlAndShareIconUrl(data.shareChannel, data.shareTitle, data.shareDes, data.shareUrl, "");
      }
      YGWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
    appNative.blJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      BLWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
    //alert("YGWebInteration.jsTunedupNativeWithTypeParamSign");
  }
  else if (typeof (window.webkit) != "undefined"
    && typeof (window.webkit.messageHandlers) != "undefined"
    && typeof (window.webkit.messageHandlers.ygJsTunedupNativeWithTypeParamSign) != "undefined"
    && typeof (window.webkit.messageHandlers.ygJsTunedupNativeWithTypeParamSign.postMessage) != "undefined") {
    appNative.AppName = "yougu";
    appNative.jsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      window.webkit.messageHandlers.jsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
    appNative.ygJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      window.webkit.messageHandlers.ygAppJsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
    appNative.blJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      window.webkit.messageHandlers.blAppJsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
  }
  else if (typeof (BLWebInteration) != "undefined") {
    appNative.AppName = "xiangxin";
    appNative.blJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      BLWebInteration.jsTunedupNativeWithTypeParamSign(typeid, body, sign);
    }
  }
  else if (typeof (window.webkit) != "undefined"
    && typeof (window.webkit.messageHandlers) != "undefined"
    && typeof (window.webkit.messageHandlers.blJsTunedupNativeWithTypeParamSign) != "undefined"
    && typeof (window.webkit.messageHandlers.blJsTunedupNativeWithTypeParamSign.postMessage) != "undefined") {
    appNative.AppName = "xiangxin";
    appNative.blJsTunedupNativeWithTypeParamSign = function (typeid, body, sign) {
      window.webkit.messageHandlers.blAppJsTunedupNativeWithTypeParamSign.postMessage({ "type": typeid, "data": body, "sign": sign });
    }
  }
  else {
    console.warn("未能识别的环境");
  }


  window.AppNative = appNative;
  window.nativeNaviMoreClick = function () {
    document.getElementById('moreBox').style.display = 'block';
  };
})();
