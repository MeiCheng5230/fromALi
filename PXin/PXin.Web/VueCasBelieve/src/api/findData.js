import { Invoke } from '@/api/sysRequest.js';
export const QueryUserInfo = (data, callback) => {
    return Invoke('/api/Chat/QueryUserInfo', { ...data }, callback);
}
export const IsNewMessage = (data, callback) => {
    return Invoke('/api/Fri/IsNewMessage', { ...data }, callback);
}
export const GetNearby = (data, callback) => {
    return Invoke('/api/Chat/GetNearby', { ...data }, callback);
}
export const GetYaoyiyao = (data, callback) => {
    return Invoke('/api/Chat/GetYaoyiyao', { ...data }, callback);
}
export const GetUserInfo_Fri = (data, callback) => {
    return Invoke('/api/Fri/GetUserInfo', { ...data }, callback);
}
export const GetMsgHome = (data, callback) => {
    return Invoke('/api/Fri/GetMsgHome', { ...data }, callback);
}
export const GetMsg = (data, callback) => {
    return Invoke('/api/Fri/GetMsg', { ...data }, callback);
}
export const MyFriend = (data, callback) => {
    return Invoke('/api/Chat/MyFriend', { ...data }, callback);
}
/**
 * 点赞或踩
 * @param {*} data 
 * @param {*} callback 
 */
export const CreateAttitude = (data, callback) => {
    return Invoke('/api/Fri/CreateAttitude', { ...data }, callback);
}
/**
 * 支付V点阅读消息
 * @param {请求数据} data 
 * @param {回调} callback 
 */
export const PayVDianForRead = (data, callback) => {
    return Invoke('/api/Fri/PayVDian', { ...data }, callback);
}
/**
 * 文章打赏
 * @param {*} data 
 * @param {*} callback 
 */
export const CreateReward = (data, callback) => {
    return Invoke('/api/Fri/CreateReward', { ...data }, callback);
}
/**
 * 删除消息
 * @param {*} data 
 * @param {*} callback 
 */
export const DeleteMsg = (data, callback) => {
    return Invoke('/api/Fri/DeleteMsg', { ...data }, callback);
}
/**
 * 评论
 * @param {*} data 
 * @param {*} callback 
 */
export const CreateComment = (data, callback) => {
    return Invoke('/api/Fri/CreateComment', { ...data }, callback);
}
/**
 * 获取举报投诉原因
 * @param {*} data 
 * @param {*} callback 
 */
export const GetReasonList = (data, callback) => {
    return Invoke('/api/Fri/GetReasonList', { ...data }, callback);
}
/**
 * 修改背景图片
 * @param {*} data 
 * @param {*} callback 
 */
export const EditBackgImg = (data, callback) => {
    return Invoke('/api/Fri/EditBackgImg', { ...data }, callback);
}
/**
 * 发布信友圈消息
 * @param {*} data 
 * @param {*} callback 
 */
export const CreateMsg = (data, callback) => {
    return Invoke('/api/Fri/CreateMsg', { ...data }, callback);
}
/**
 * 消息举报
 * @param {*} data 
 * @param {*} callback 
 */
export const CreateReport = (data, callback) => {
    return Invoke('/api/Fri/CreateReport', { ...data }, callback);
}

