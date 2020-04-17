/*
（id ，参数1，参数2）
1001：
1002：扫描二维码；空；空；         回调：callBackScanResult；
1003：UE支付，参数1，参数2；
1009：实名认证原生页面；空；空；
1010：已实名认证原生页面；空；空；
1012：证件管理页；空；空；
1013：跳PCN绑定原生页面；空；空；；
1014：设置原生APP导航；参数1（0:白色背景,黑色标题,黑色按钮、1：深色（主题色）背景，白色标题，白色按钮、2：透明背景，白色标题，白色按钮）；参数2；
1015：JS调用原生人脸对比功能；参数1；参数2； 回调：nativeFaceCompareCompletion
1016：实名认证驾驶证原生页面；空；空；
1017：获取导航高度；空；空；   回调：nativeNaviBarHeight；
1019：获取用户当前定位；空；空；  回调：nativeLocationCompletion
*/
/**
 * 原生Sdk
 * @param id 方法id
 * @param body 传递内容
 * @param sign 用户签名
 * @param callback
 * @constructor
 */
const NativeMethod = (id, body, sign, callback) => {
    //设置导航专用签名
    let navJsonStr = [
        {"PStr": "eyJuYXZpdHlwZSI6MH0=", "Sign": "B45473F9D8711AB7E1B4B8413AFD5D91"},
        {"PStr": "eyJuYXZpdHlwZSI6MX0=", "Sign": "D1A97BBB39A4B528B295BDF1E9276FA5"},
        {"PStr": "eyJuYXZpdHlwZSI6Mn0=", "Sign": "131850A5C3530A2BCC988D2E47403250"}
    ];

    if (typeof (body) == 'number') {
        body = navJsonStr[body].Sign;
        sign = navJsonStr[body].PStr;
    }

    try {
        let appName = window.AppNative.AppName;
        if (appName == 'xiangxin') {
            AppNative.blJsTunedupNativeWithTypeParamSign(id, body, sign);
            callback;
        }
        if (appName == 'pcn') {
            AppNative.jsTunedupNativeWithTypeParamSign(id, body, sign);
            callback;
        }
        if (appName == 'yougu') {
            AppNative.ygJsTunedupNativeWithTypeParamSign(id, body, sign);
            callback;
        }
    } catch (e) {
        vant.Toast.fail(e);
    }

};

export default NativeMethod;
