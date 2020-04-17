import http from '../service/http'

const Post = (url, data) => {
    return http({
        url: url,
        method: 'post',
        data: data,
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'Content-Type': 'application/json; charset=UTF-8'
        }
    }).then(
        (response) => {
            return response;
        }
    ).catch(
        (res) => {
            return res;
        }
    );
}

/**
 * 获取11月活动-迪拜见证之旅 服务费的数量
 * @param data
 * @returns {*}
 * @constructor
 */
export const GetNovemberActivityCount = (data) => Post('/api/Activity/GetNovemberActivityCount', {
    ...data,
});

/**
 * 已满足条件和已获得资格列表
 * @param data
 * @returns {*}
 * @constructor
 */
export const GetVpxinOctoberActivitys = (data,activityid) => Post('/api/Activity/GetVpxinOctoberActivitys', {
    ...data,
    activityid
});
/**
 * 调用ue支付
 * @param data
 * @returns {*}
 * @constructor
 */
export const NovemberActivityDosPay = (data) => Post('/api/Activity/NovemberActivityDosPay', {
    ...data,
});
/**
 * 获取活动列表
 * @param data
 * @returns {*}
 * @constructor
 */
export const GetActivitys = (data) => Post('/api/Activity/GetActivitys', {
    ...data,
});

/**
 * 检查每月活动是否绑定pcn帐号
 * @param data
 * @returns {*}
 * @constructor
 */
export const HasBindActivityThirdparty = (data,activityid) => Post('/api/Activity/HasBindActivityThirdparty', {
    ...data,
    activityid
});


/**
 * 每月活动绑定pcn帐号
 * @param data
 * @returns {*}
 * @constructor
 */
export const BindActivityThirdparty = (data, pcnaccount,activityid) => Post('/api/Activity/BindActivityThirdparty', {
    ...data,
    pcnaccount,
    activityid
});



