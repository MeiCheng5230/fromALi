
import store from "../store/index";
const qs = require("qs");

const service = axios.create({
    baseURL: process.env.VUE_APP_BASE_API,
    timeout: 100000
});

const pending = [];
const cancelToken = axios.CancelToken;

/**
 * 处理重复请求
 * @param  {} {config}={} AxiosRequestConfig
 */
const addPending = ({config}) => {

    const url =
        config.url + "&" + config.method + "&" + qs.stringify(config.data);
    config.cancelToken = new cancelToken(cancel => {
        if (!pending.some(item => item.url === url)) {
            pending.push({
                url,
                cancel
            });
        }
    });
};
const removePending = ({config}) => {
    const url =
        config.url + "&" + config.method + "&" + qs.stringify(config.data);
    pending.forEach((item, index) => {
        if (item.url === url) {
            item.cancel("取消重复请求:" + config.url);
            pending.splice(index, 1);
        }
    });
};
/**
 * 请求头预处理
 * @param  {} {config} AxiosRequestConfig
 */
const requestHeaders = ({config}) => {
    //1.时间戳
    const timestamp = new Date().getTime();
    config.headers.timestamp = timestamp;
    //2.token
    const token = sessionStorage.getItem("token");
    if (token) {
        config.headers.token = token;
    }
};
/**
 * 请求参数预处理
 * @param  {} {config}={} AxiosRequestConfig
 */
const requestParams = ({config} = {}) => {
    //配置分页
    if (config.headers.pagination) {
        const { pageNum, pageSize } = store.getters.getPagination;
        config.data = Object.assign({}, config.data, {
            pageNum,
            pageSize
        });
    }
};
/**
 * 请求开始&&loading=true
 * @param  {} requestHeaders 1.配置请求头
 * @param  {} requestParams 2.配置请求体
 * @param  {} removePending 3.处理重复请求
 */
const requestStart = ({config} = {}) => {
    requestHeaders({config});
    requestParams({config});
    removePending({config});
    addPending({config});
    vant.Toast.loading({
        duration: 0,
        forbidClick: true,
        mask: true
    });
};
/**
 * 请求结束&&loading=false
 * @param  {} {config}={} AxiosRequestConfig
 * @param  {} {config}={} response body
 */
const requestEnd = ({config, data} = {}) => {
    removePending({config});
    vant.Toast.clear();
    //配置分页
    if (config.headers.pagination) {
        const {data: content} = data;
        if (content) {
            // store.commit("setPageTotal", content.total);
        }
    }
};
/**
 * @param  {} {status HTTP状态码
 * @param  {} data 响应体
 * @param  {} config}={} AxiosRequestConfig
 */
const responseResolve = ({status, data, config} = {}) => {
    const {result, message} = data;
    if (status === 200) {
        switch (result) {
            case 1:
                return Promise.resolve(data);
            case 0:
                vant.Toast.fail(message || "登录超时，请重新登录！");
                // window.location.href = process.env.VUE_APP_AUTH_URL;
                return Promise.reject(data);
            case -1:
                vant.Toast.fail(message || "登录超时，请重新登录！");
                // window.location.href = process.env.VUE_APP_AUTH_URL;
                return Promise.reject(data);
            default:
                //可配置错误提醒
                if (!config.headers["hide-message"]) {
                    vant.Toast.fail(message || "操作失败！");
                }
                return Promise.reject(data);
        }
    } else {
        vant.Toast.fail(message || "操作失败！");
        // vant.Toast.clear();
        return Promise.reject(data);
    }
};
/**
 * 请求拦截器
 * @param  {} requestStart 请求开始
 */
service.interceptors.request.use(
    config => {
        requestStart({config});
        return config;
    },
    error => {
        vant.Toast.fail("请求出错");
        return Promise.reject(error);
    }
);
/**
 * 响应拦截器
 * @param  {} requestEnd 1.请求结束
 * @param  {} responseResolve 2.请求错误处理
 */
service.interceptors.response.use(
    response => {
        const {status, data, config} = response;
        requestEnd({config, data});
        return responseResolve({status, data, config});
    },
    error => {
        if (axios.isCancel(error)) {
            vant.Toast.fail("网络请求中，请不要重复操作！");
        } else {
            const {response} = error;
            console.log(response);
            vant.Toast.fail(response.data.message);
        }
        // vant.Toast.clear();
        // store.commit("setPageTotal", 0);
        return Promise.reject(error);
    }
);
export default service;
