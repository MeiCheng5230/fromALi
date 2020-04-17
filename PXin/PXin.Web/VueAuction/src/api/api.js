/**
 * jqPromiseAjax
 */
function jqPromiseAjax(url, data) {
  return new Promise(function (resolve, reject) {
    vant.Toast.loading({
      duration: 0,
      mask: true
    });
    $.ajax({
      url: GetBaseurl() + url,
      type: 'POST',
      dataType: 'json',
      headers: {},
      data,
      success(res) {
        resolve(res)
      },
      error(err) {
        reject(err)
      }
    })
  })
};


/**
 * 成功
 */
function responseSuccess(data) {
  vant.Toast.clear();
  return data;
};

/**
 * 失败
 */
function responseError(err) {
  console.log(err.status)
  if (err && err.status) {
    switch (err.status) {
      case 400: err.message = '请求错误(400)'; break;
      case 401: err.message = '未授权，请重新登录(401)'; break;
      case 403: err.message = '拒绝访问(403)'; break;
      case 404: err.message = '请求出错(404)'; break;
      case 408: err.message = '请求超时(408)'; break;
      case 500: err.message = '服务器错误(500)'; break;
      case 501: err.message = '服务未实现(501)'; break;
      case 502: err.message = '网络错误(502)'; break;
      case 503: err.message = '服务不可用(503)'; break;
      case 504: err.message = '网络超时(504)'; break;
      case 505: err.message = 'HTTP版本不受支持(505)'; break;
      default: err.message = `连接出错(${err.status})!`;
    }
  } else {
    err.message = '连接服务器失败'
  }
  vant.Toast.fail(err.message);
  return err;
};


/**
 *
 * 本月竞拍数据（我的竞拍总数和竞拍记录）
 */
export const GetThisMonthData = (data) => {
  return jqPromiseAjax('/api/AuctionA/GetThisMonthData', {...data}).then(function (res) {
    return responseSuccess(res)
  }).catch(function (err) {
    return responseError(err)
  });
};
/**
 *
 * 竞拍支付
 */
export const PayAuction = (data, num, payPwd, minPrice) => {
  return jqPromiseAjax('/api/AuctionA/PayAuction', {...data, num, payPwd, minPrice}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 我的竞拍历史
 */
export const GetMyAuctionHis = (data, pageNum, pageSize, queryDate) => {
  return jqPromiseAjax('/api/AuctionA/GetMyAuctionHis', {...data, pageNum, pageSize, queryDate}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 我的A点
 */
export const GetMyAuctionA = (data) => {
  return jqPromiseAjax('/api/AuctionA/GetMyAuctionA', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};
/**
 *
 * 竞拍配置
 */
export const GetAuctionConfig = (data) => {
  return jqPromiseAjax('/api/AuctionA/GetAuctionConfig', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};
//红包
/**
 *
 * 获取领取红包页面信息
 */
export const GetRedPacketInfo = (data) => {
  return jqPromiseAjax('/api/Redpacket/GetRedPacketInfo', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 领取红包
 */
export const ReceiveRedPacket = (data, infoid) => {
  return jqPromiseAjax('/api/Redpacket/ReceiveRedPacket', {...data, infoid}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 获取我的红包奖励
 */
export const GetMyRedPacket = (data) => {
  return jqPromiseAjax('/api/Redpacket/GetMyRedPacket', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 红包奖励领取详情
 */
export const GetMyRedPacketDetail = (data, hisid) => {
  return jqPromiseAjax('/api/Redpacket/GetMyRedPacketDetail', {...data, hisid}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 获取兑换页面信息
 */
export const GetExchangeInfo = (data) => {
  return jqPromiseAjax('/api/Redpacket/GetExchangeInfo', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 兑换
 */
export const Exchange = (data, ExchangeType, Specs, Password, num) => {
  return jqPromiseAjax('/api/Redpacket/Exchange', {...data, ExchangeType, Specs, Password, num}).then(function (res) {
    return responseSuccess(res)
  });
};


/**
 *
 * 获取用户邀请码
 */
export const InviteesList = (data) => {
  return jqPromiseAjax('/api/User/GetInviteesList', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};

/**
 *
 * 设置阅读用户协议
 */
export const SetAgreeAgreement = (data, type) => {
  return jqPromiseAjax('/api/User/SetAgreeAgreement', {...data, type}).then(function (res) {
    return responseSuccess(res)
  });
};

export const CheckSign = (data) => {
  return jqPromiseAjax('/api/Sys/CheckSign', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};
/**
 *
 * 竞拍加价页面数据
 */
export const GetAuctionAddprice = (data) => {
  return jqPromiseAjax('/api/AuctionA/GetAuctionAddprice', {...data}).then(function (res) {
    return responseSuccess(res)
  });
};
/**
 *
 * 竞拍加价支付
 */
export const AuctionAddpricePay = (data, payPwd, price, auctionid) => {
  return jqPromiseAjax('/api/AuctionA/AuctionAddpricePay', {...data, payPwd, price, auctionid}).then(function (res) {
    return responseSuccess(res)
  });
};


export const GetAuctionDetails = (data, querytimetype) => {
  return jqPromiseAjax('/api/AuctionA/GetAuctionDetails ', {...data, querytimetype}).then(function (res) {
    return responseSuccess(res)
  });
};
/**
 * 竞拍排名
 *
 */
export const GetAuctionRanking = (data, num) => {
  return jqPromiseAjax('/api/AuctionA/GetAuctionRanking ', {...data, num}).then(function (res) {
    return responseSuccess(res)
  });
};
