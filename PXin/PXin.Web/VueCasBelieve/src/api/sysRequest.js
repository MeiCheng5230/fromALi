import http from '@/api/http.js';
import { getStore } from "@/config/utils";
let userParam = null;
function UserParam() {
    if (!userParam) {
        userParam = JSON.parse(getStore("userParam"));
    }
    return userParam;
}
function invoke(path, data, callback) {
    if (data) {
        data = { ...data, ...UserParam() };
    } else {
        data = { ...UserParam() };
    }
    return http.post(path, { ...data }, callback);
}
export const Go = (id) => {
    let origin = window.location.origin;
    if (origin.indexOf('localhost') > -1) {
        origin = 'http://client.be.sulink.cn';
    }
    window.location.href = origin + "/go?id=" + id + "&nodeid=" + UserParam().nodeid + "&sid=" + UserParam().sid + "&tm=" + UserParam().tm + "&sign=" + UserParam().sign;
    // window.location.href = 'http://client.be.sulink.cn' + "/go?id=" + id + "&nodeid=" + UserParam().nodeid + "&sid=" + UserParam().sid + "&tm=" + UserParam().tm + "&sign=" + UserParam().sign;
}
/**
 * 跳转链接，带四个验证参数
 * @param {*} url 
 */
export const GoWithParam = (url) => {
    if (url == null || url == "") {
        return;
    }
    let firstSign = url.indexOf('?') > 0 ? "&" : "?";
    window.location.href = url + firstSign + "nodeid=" + UserParam().nodeid + "&sid=" + UserParam().sid + "&tm=" + UserParam().tm + "&sign=" + UserParam().sign;
}

export const Invoke = (path, data, callback) => {
    return invoke(path, data, callback);
}

export const SendSms = (data, callback) => {
    return invoke('/api/Sys/SendSms', data, callback);
}
export const CheckVerificationCode = (data, callback) => {
    return invoke('/api/Sys/CheckVerificationCode', data, callback);
}
export const GetConfig = (data, callback) => {
    return invoke('/api/Sys/GetConfig', data, callback);
}
export const UploadFile = (data, callback) => {
    return invoke('/api/Sys/UploadFile', data, callback);
}
