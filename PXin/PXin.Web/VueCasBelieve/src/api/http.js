import axios from 'axios';
import { Toast } from 'vant';
//配置接口域名 开发环境和生产环境
const baseUrl = process.env.NODE_ENV === 'development' ? '' : '';

axios.interceptors.request.use(config => {
  Toast.loading({
    message: '数据加载中...',
    mask:true
  });
  return config;
}, error => {
  Toast.clear();
  Toast.fail('数据异常');
  return Promise.reject(error)
});

axios.interceptors.response.use(response => {
  Toast.clear();
  return response;
}, error => {
  Toast.fail(error.response.data.message);
  return Promise.resolve(error.response);
});

function checkStatus(response, callback) {
  // loading
  // 如果http状态码正常，则直接返回数据
  if (response && (response.status === 200 || response.status === 304 || response.status === 400)) {
    if (callback) {
      callback(response.data);
    }
    return response.data;
  }

  // 异常状态下，把错误信息返回去
  return {
    status: -404,
    msg: '网络异常'
  }
}

function checkCode(res) {
  // 如果code异常(这里已经包括网络错误，服务器错误，后端抛出的错误)，可以弹出一个错误提示，告诉用户
  if (res.status === -404) {
    console.log(res.msg);
  }
  if (res.data && (!res.data.success)) {
    console.log(res.data.error_msg);
  }
  return res;
}

export default {
  post(url, data, callback = null) {
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
        return checkStatus(response, callback);
      }
    ).catch(
      (res) => {
        return checkCode(res);
      }
    )
  }
}
