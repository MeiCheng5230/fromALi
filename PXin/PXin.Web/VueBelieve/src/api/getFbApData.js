import http from "@/api/http";
/*
 * 充值商代理人
 */

//上传临时文件
export const UploadFile = data => {
  return http.post("/api/Sys/UploadFile", data);
};
//获取(充值商/代理人)信息
export const GetUserJxs = data => {
  return http.post("/api/FbAp/GetUserJxs", data);
};
//修改(充值商/代理人)名称
export const UpdateUserJxsName = (data, ruleid, isbind) => {
  return http.post("/api/FbAp/UpdateUserJxsName", data);
};
//获取90天累积进货列表
export const Get90DaysPurchases = (data, infoid) => {
  return http.post("/api/FbAp/Get90DaysPurchases", { ...data, infoid });
};
//我的代理人
export const GetMyUserJxs = (data, infoid) => {
  return http.post("/api/FbAp/GetMyUserJxs", { ...data, infoid });
};
//我的充值商
export const GetMyUserCzs = (userInfo, infoid) =>
  http.post("/api/FbAp/GetMyUserCzs", { ...userInfo, infoid });
//获取认证资料
export const GetAuthData = data => http.post("/api/FbAp/GetAuthData", data);
//审核我的代理人资料
export const Audit = data => http.post("/api/FbAp/AuditJxsInfo", data);
//获取审核状态
export const GetAuditStatus = (data, infoid) =>
  http.post("/api/FbAp/GetAuditStatus", { ...data, infoid });
//获取续费信息
export const GetRenewInfo = (data, infoid) =>
  http.post("/api/FbAp/GetRenewInfo", { ...data, infoid });
//续费
export const Renew = (data, infoid) =>
  http.post("/api/FbAp/Renew", { ...data, infoid });
//获取用户信息,头像
export const GetUserInfo = (data, infoid) =>
  http.post("/api/FbAp/GetUserInfo", { ...data, infoid });
//获取兑换类型信息
export const GetExchangeTypeInfo = (data, infoid) =>
  http.post("/api/FbAp/GetExchangeTypeInfo", { ...data, infoid });
//进货
export const ExChangeRechargeCode = data =>
  http.post("/api/FbAp/ExChangeRechargeCode", data);
//获取DOS余额
export const GetStockBalance = data =>
  http.post("/api/FbAp/GetStockBalance", { ...data });
//查询用户
export const SearchUser = (data, key, type) =>
  http.post("/api/FbAp/SearchUser", { ...data, key, type });
//开通经销商
export const AddUserJxs = (data, newnodeid, infoid) =>
  http.post("/api/FbAp/AddUserJxs", { ...data, newnodeid, infoid });
//验证密码
export const VerifyPwd = data => http.post("/api/FbAp/VerifyPwd", { ...data });
//是否为充值商/代理人
export const GetFbapInitPage = data =>
  http.post("/api/FbAp/GetFbapInitPage", { ...data });
//充值商申请
export const ApplyFbap = data => http.post("/api/FbAp/ApplyFbap", { ...data });
//上传认证资料
export const UploadAuthData = data =>
  http.post("/api/FbAp/UploadAuthData", { ...data });
//获取会议列表
export const GetMeetInfos = data =>
  http.post("/api/FbAp/GetMeetInfos", { ...data });
//获取会议详情
export const GetMeetInfoDetail = (data, callback) =>
  http.post("/api/FbAp/GetMeetInfoDetail", { ...data }, callback);
//参加会议
export const JoinMeeting = (data, callback) =>
http.post("/api/FbAp/JoinMeeting", { ...data }, callback);
//查询充值商信息
export const GetFbapInfo = data =>
  http.post("/api/FbAp/GetFbapInfo", { ...data });
//更换充值商
export const ChangeFbap = data =>
http.post("/api/FbAp/ChangeFbap", { ...data });
//开通充值商
export const OpenCzs = data =>
http.post("/api/FbAp/OpenCzs", { ...data });
//开通充值商检查
export const CheckOpenCzs = data =>
http.post("/api/FbAp/CheckOpenCzs", { ...data });
//查询充值商添加代理人请求列表
export const GetUserJxsConfirms = data =>
http.post("/api/FbAp/GetUserJxsConfirms", { ...data });
//同意充值商添加代理人请求
export const AgreeUserJxsRequst = data =>
http.post("/api/FbAp/AgreeUserJxsRequst", { ...data });
//SV生成充值码
export const SvToSvcCard = data =>
http.post("/api/CZM/SvToSvcCard", { ...data });

/*
/api/User/GetUserOpens 查询用户所有第三方账号绑定状态
*/
export let GetUserOpens = data => http.post('/api/User/GetUserOpens',{...data});

/*
/api/Activity/GetOctoberActivityCount 获取十月送手机活动的领取手机和支付服务费的数量
*/
export let GetOctoberActivityCount = (data,activityid) => http.post('/api/Activity/GetOctoberActivityCount',{...data,activityid});


/*
/api/Activity/GetOctoberActivityList 获取十月送手机活动的领取手机和支付服务费的列表
*/
export let GetOctoberActivityList = (data,activityid) => http.post('/api/Activity/GetOctoberActivityList',{...data,activityid});

/*
/api/Activity/OctoberActivityDosUEPrepare 调用ue支付
*/
export let OctoberActivityDosUEPrepare = data => http.post('/api/Activity/OctoberActivityDosUEPrepare',{...data});

/*
/api/Activity/GetExpressInfo 查询快递
*/
export let GetExpressInfo = data => http.post('/api/Activity/GetExpressInfo',{...data});

/*
/api/User/BindPcnAcount 登录绑定pcn账号
*/
export let BindPcnAcount = data => http.post('/api/User/BindPcnAcount',{...data});
/**
 * 获取配车状态
 * @param data
 * @param infoid
 * @returns {*}
 * @constructor
 */
export let GetJxsPeiche = (data,infoid) => http.post('/api/FbAp/GetJxsPeiche',{...data,infoid});



//POST /api/Activity/HasBindActivityThirdparty  检查每月活动是否绑定pcn帐号
export let HasBindActivityThirdparty = data => http.post('/api/Activity/HasBindActivityThirdparty',{...data});

// POST /api/Activity/BindActivityThirdparty 每月活动绑定pcn帐号
export let BindActivityThirdparty = data => http.post('/api/Activity/BindActivityThirdparty',{...data});

//POST /api/FbAp/GetStockRecord  获取库存记录列表
export let GetStockRecord = data => http.post('/api/FbAp/GetStockRecord',{...data});
