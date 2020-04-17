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

// 获取首页开票统计
export const GetInvioceStatistics = (data) => Post('/api/Invioce/GetInvioceStatistics', {
    ...data,
});

// 获取可申请列表
export const GetMayApplyInvioceHis = (data) => Post('/api/Invioce/GetMayApplyInvioceHis', {
    ...data,
});

// 获取已申请列表
export const GetAlreadyApplyInvioceHis = (data) => Post('/api/Invioce/GetAlreadyApplyInvioceHis', {
    ...data,
});

// 提交增票资质申请
export const ApplyInvioceQualifica = (data) => Post('/api/Invioce/ApplyInvioceQualifica', {
    ...data,
});

// 获取增票资质
export const GetInvioceQualifica = (data) => Post('/api/Invioce/GetInvioceQualifica', {
    ...data,
});

// 开票申请
export const ApplyWriteInvioce = (data) => Post('/api/Invioce/ApplyWriteInvioce', {
    ...data,
});

// 发送邮件
export const SendEmail = (data) => Post('/api/Invioce/SendEmail', {
    ...data,
});

// 开票申请
export const GetElectronicInvioceDetail = (data) => Post('/api/Invioce/GetElectronicInvioceDetail', {
    ...data,
});

// 获取收获地址
export const GetShoppingAddrs = (data) => Post('/api/ShoppingAddr/GetShoppingAddrs', {
    ...data,
});
