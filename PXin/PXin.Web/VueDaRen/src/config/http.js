import axios from 'axios';
import {baseUrl} from './baseUrl'
import {Toast} from 'vant';
//配置接口域名 开发环境和生产环境

axios.interceptors.request.use(config => {
  Toast.loading({
    mask: true,
    message: '加载中...',
    duration: 0
  })
  // loading
  return config
}, error => {
  Toast.clear();
  Toast('数据异常');
  return Promise.reject(error)
});

axios.interceptors.response.use(response => {
  Toast.clear();
  return response
}, error => {
  Toast.clear();
  Toast(error.response.data.message);
  return Promise.resolve(error.response)
});

function checkStatus(response, callBack) {
  // loading
  // 如果http状态码正常，则直接返回数据
  if (response && (response.status === 200 || response.status === 304 || response.status === 400)) {
    // callBack(response.data);
    return response.data;
  }
  if (response && response.status ===500 ){
    return  {
      message: "服务器异常",
      result: 0
    };
  }
  // 异常状态下，把错误信息返回去
  return {
    message: "网络异常",
    result: 0
  }
}

function checkCode(res) {
  // 如果code异常(这里已经包括网络错误，服务器错误，后端抛出的错误)，可以弹出一个错误提示，告诉用户
  if (res.status === -404) {
    console.log(res.res)
  }
  if (res.data && (!res.data.success)) {
    console.log(res.data.error_msg)
  }
  return res
}

export default {
  post(url, data) {
    console.log(url);
    return axios({
      method: 'POST',
      baseURL: baseUrl,
      url,
      data: data,
      headers: {
        'X-Requested-With': 'XMLHttpRequest',
        'Content-Type': 'application/json; charset=UTF-8'
      }
    }).then(
      (response) => {
        return checkStatus(response)
      }
    ).catch(
      (res) => {
        return checkCode(res)
      }
    )
  }
}

