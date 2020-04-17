import http from '@/api/http'

//充值码
/**
 *  获取充值码面额
 */
export const GetSvcConfig = (data) => {
  return http.post('/api/CZM/GetSvcConfig', { ...data });
};
/**
 *  获取充值码统计
 */
export const GetMySvcStatis = (data) => {
  return http.post('/api/CZM/GetMySvcStatis', { ...data });
};
/**
 *  获取充值码统计
 */
export const GetMySvc = (data) => {
  return http.post('/api/CZM/GetMySvc', { ...data });
};
/**
 *  获取充值码变动历史
 */
export const GetMySvchis = (data, pagenum, pagesize, typeid) => {
  return http.post('/api/CZM/GetMySvchis', { ...data, pagenum, pagesize, typeid });
};
/**
 *  使用充值码
 */
export const SvcToSvByOwner = (data, cards, nodecode, paypwd, isvdnow) => {
  return http.post('/api/CZM/SvcToSvByOwner', { ...data, cards, nodecode, paypwd, isvdnow });
};
/**
 *  使用无主充值码
 */
export const SvcToSvByCard = (data, cards) => {
  return http.post('/api/CZM/SvcToSvByCard', { ...data, cards });
};
/**
 *  购买充值码
 */
export const BuySvc = (data, paytype, amount, num, isusenow, pwd) => {
  return http.post('/api/CZM/BuySvc', { ...data, paytype, amount, num, isusenow, pwd});
};
/**
 *  转让充值码
 */
export const RecoverySvc = (data, nodecode, cards, paypwd) => {
  return http.post('/api/CZM/RecoverySvc', { ...data, nodecode, cards, paypwd });
};
/**
 *  获取用户信息
 */
export const GetUserInfo = (data, nodecode, isphoneemail) => {
  return http.post('/api/CZM/GetUserInfo', { ...data, nodecode, isphoneemail: 1 });
};
/**
 *  零售
 */
export const SaleSvc = (data, specifications, nodecode, paypwd) => {
  return http.post('/api/CZM/SaleSvc', { ...data, specifications, nodecode, paypwd });
};
/**
 *  微信
 */
export const Success = (data, orderno, metadata) => {
  return http.post('/api/CZM/Success', { ...data, orderno, metadata });
};


//B指数
/**
 *  获取大厅信息
 */
export const GetHomeInfo = (data) => {
  return http.post('/api/Bexponent/GetHomeInfo', { ...data });
};
/**
 *  获取回馈号列表
 */
export const GetUexponentHKList = (data, pagesize, pagenum) => {
  return http.post('/api/Bexponent/GetUexponentHKList', { ...data, pagesize, pagenum });
};
/**
 *  获取回馈号详情
 */
export const GetHKInfoDetail = (data, infoid) => {
  return http.post('/api/Bexponent/GetHKInfoDetail', { ...data, infoid });
};





/**
 *
 * 获取用户信息及专户dos余额
 */
export const GetExChangeDosInfo = (data) => {
  return http.post('/api/Exchange/GetDosInfo', { ...data });
};
/**
 *
 * 获取兑换商品列表
 */
export const GetExChangeList = (data) => {
  return http.post('/api/Exchange/GetChargeProductList', { ...data });
};
/**
 *
 * 获取PCN或优谷用户信息
 */
export const GetPCNUserByNodeCode = (data, nodeCode, typeId) => {
  return http.post('/api/User/GetYGPCNUEUserInfo', { ...data, nodeCode, typeId });
};
/**
 *
 * 兑换商品
 */
export const ProductRecharge = (data, productId, payPwd, num, pnodeCode) => {
  return http.post('/api/Exchange/ProductRecharge', { ...data, productId, payPwd, num, pnodeCode });
};
/**
 *
 * 获取兑换历史
 */
export const GetRechargeHisList = (data, pageNum, pageSize) => {
  return http.post('/api/Exchange/GetRechargeHisList', { ...data, pageNum, pageSize });
};
/**
 *
 * 开通专属账号
 */
export const ExchangeOpenInfo = (data) => {
  return http.post('/api/Exchange/OpenInfo', { ...data });
};
/**
 *
 * 获取ue用户信息
 */
export const GetUeUserInfo = (data, typeId) => {
  return http.post('/api/User/GetYGPCNUEUserInfo', { ...data }, typeId);
};
/**
 *
 * 绑定ue账号
 */
export const BindingUeAccount = (data, ueNodeCode, ueNodePwd) => {
  return http.post('/api/Exchange/BindingUeAccount', { ...data, ueNodeCode, ueNodePwd });
};
/**
 *
 * 转入（UEDOS转入到专户DOS） 调起ue
 */
export const UeTransferInDos = (data, amount) => {
  return http.post('/api/Exchange/UeTransferInDos', { ...data, amount });
};
/**
 *
 * 转出（专户DOS转出到UEDOS）
 */
export const DosTransferOutUe = (data, payPwd, amount) => {
  return http.post('/api/Exchange/DosTransferOutUe', { ...data, payPwd, amount });
};
/**
 *
 * 本月竞拍数据（我的竞拍总数和竞拍记录）
 */
export const GetThisMonthData = (data) => {
  return http.post('/api/AuctionA/GetThisMonthData', { ...data });
};
/**
 *
 * 竞拍支付
 */
export const PayAuction = (data, num, payPwd, minPrice) => {
  return http.post('/api/AuctionA/PayAuction', { ...data, num, payPwd, minPrice });
};

/**
 *
 * 我的竞拍历史
 */
export const GetMyAuctionHis = (data, pageNum, pageSize, queryDate) => {
  return http.post('/api/AuctionA/GetMyAuctionHis', { ...data, pageNum, pageSize, queryDate });
};

/**
 *
 * 我的A点
 */
export const GetMyAuctionA = (data) => {
  return http.post('/api/AuctionA/GetMyAuctionA', { ...data });
};
/**
 *
 * 竞拍配置
 */
export const GetAuctionConfig = (data) => {
  return http.post('/api/AuctionA/GetAuctionConfig', { ...data });
};
//红包
/**
 *
 * 获取领取红包页面信息
 */
export const GetRedPacketInfo = (data) => {
  return http.post('/api/Redpacket/GetRedPacketInfo', { ...data });
};

/**
 *
 * 领取红包
 */
export const ReceiveRedPacket = (data, infoid) => {
  return http.post('/api/Redpacket/ReceiveRedPacket', { ...data, infoid });
};

/**
 *
 * 获取我的红包奖励
 */
export const GetMyRedPacket = (data) => {
  return http.post('/api/Redpacket/GetMyRedPacket', { ...data });
};

/**
 *
 * 红包奖励领取详情
 */
export const GetMyRedPacketDetail = (data, hisid) => {
  return http.post('/api/Redpacket/GetMyRedPacketDetail', { ...data, hisid });
};

/**
 *
 * 获取兑换页面信息
 */
export const GetExchangeInfo = (data) => {
  return http.post('/api/Redpacket/GetExchangeInfo', { ...data });
};

/**
 *
 * 兑换
 */
export const Exchange = (data, ExchangeType, Specs, Password, num) => {
  return http.post('/api/Redpacket/Exchange', { ...data, ExchangeType, Specs, Password, num });
};


/**
 *
 * 获取用户邀请码
 */
export const InviteesList = (data) => {
  return http.post('/api/User/GetInviteesList', { ...data });
};

/**
 *
 * 设置阅读用户协议
 */
export const SetAgreeAgreement = (data,type) => {
  return http.post('/api/User/SetAgreeAgreement', { ...data,type });
};

export const CheckSign=(data)=>{
  return http.post('/api/Sys/CheckSign', { ...data });
};
/**
 * 
 * 竞拍加价页面数据
 */
export const GetAuctionAddprice=(data)=>{
  return http.post('/api/AuctionA/GetAuctionAddprice', { ...data });
};
/**
 * 
 * 竞拍加价支付
 */
export const AuctionAddpricePay=(data,payPwd,price,auctionid)=>{
  return http.post('/api/AuctionA/AuctionAddpricePay', { ...data,payPwd,price,auctionid});
};


export const GetAuctionDetails =(data, querytimetype)=>{
  return http.post('/api/AuctionA/GetAuctionDetails ', { ...data, querytimetype });
};
/**
 * 竞拍排名
 * 
 */
export const GetAuctionRanking =(data)=>{
  return http.post('/api/AuctionA/GetAuctionRanking ', { ...data });
};
