import { Invoke } from '@/api/sysRequest.js'

export const GetUserInfo = (data, callback) => {
    return Invoke('/api/User/GetMy', { ...data }, callback);
}
export const GetYGPCNUEUserInfo = (data, callback) => {
    return Invoke('/api/User/GetYGPCNUEUserInfo', { ...data }, callback);
}
export const EditUserInfo = (data, callback) => {
    return Invoke('/api/User/EditUserInfo', { ...data }, callback);
}
export const EditMobileno = (data, callback) => {
    return Invoke('/api/User/EditMobileno', { ...data }, callback);
}
export const ChangePwd = (data, callback) => {
    return Invoke('/api/User/ChangePwd', { ...data }, callback);
}
export const ForgetPwd = (data, callback) => {
    return Invoke('/api/User/ForgetPwd', { ...data }, callback);
}
export const GetUserOpens = (data, callback) => {
    return Invoke('/api/User/GetUserOpens', { ...data }, callback);
}
export const GetPurses = (data, callback) => {
    return Invoke('/api/Purse/GetPurses', { ...data }, callback);
}
export const GetPurseHis = (data, callback) => {
    return Invoke('/api/Purse/GetPurseHis', { ...data }, callback);
}
export const GetPurseHisTypeLogo = (data, callback) => {
    return Invoke('/api/Purse/GetPurseHisTypeLogo', { ...data }, callback);
}
export const GetUserInfo_Fri = (data, callback) => {
    return Invoke('/api/Fri/GetUserInfo', { ...data }, callback);
}
export const ChargeVDian = (data, callback) => {
    return Invoke('/api/Fri/ChargeVDian', { ...data }, callback);
}
export const GetPxinAmountChangeHis = (data, callback) => {
    return Invoke('/api/Fri/GetPxinAmountChangeHis', { ...data }, callback);
}
