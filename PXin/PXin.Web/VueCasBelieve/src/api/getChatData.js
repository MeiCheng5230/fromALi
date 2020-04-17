import http from "@/api/http.js";
import { getStore } from "@/config/utils.js";
import { Invoke } from "@/api/sysRequest.js";

//上传临时图片
export const UploadFile = (data, callback) => {
  return http.post("/api/Sys/UploadFile", { ...data }, callback);
};
//P信账号登录
export const Login = (data, callback) => {
  let userConfigInfo = getStore("UserConfigInfo");
  let userInfo = JSON.parse(userConfigInfo);
  console.log("data==>"+data);
  if (userConfigInfo && data.nodeid == userInfo.data.nodeid)
    return callback(userInfo);
  else return http.post("/api/Chat/Login", { ...data }, callback);
};
//我的好友
export const MyFriend = (data, callback) => {
  return Invoke("/api/Chat/MyFriend", { ...data }, callback);
};
//根据userid获取用户信息
export const QueryUserInfo = (data, callback) => {
  return http.post("/api/Chat/QueryUserInfo", { ...data }, callback);
};
//获取商友圈-根据用户查询
export const GetMsg = (data, callback) => {
  return http.post("/api/Fri/GetMsg", { ...data }, callback);
};
//修改好友信息
export const UpdateMyFriendInfo = (data, callback) => {
  return http.post("/api/Chat/UpdateMyFriendInfo", { ...data }, callback);
};
//我的群组
export const MyGroup = (data, callback) => {
  return http.post("/api/Chat/MyGroup", { ...data }, callback);
};
//创建群组
export const CreateGroup = (data, callback) => {
  return http.post("/api/Chat/CreateGroup", { ...data }, callback);
};
//查找好友
export const QueryFriend = (data, callback) => {
  return http.post("/api/Chat/QueryFriend", { ...data }, callback);
};
//添加好友
export const AddFriend = (data, callback) => {
  return http.post("/api/Chat/AddFriend", { ...data }, callback);
};
//添加好友确认
export const AddFriendConfirm = (data, callback) => {
  return http.post("/api/Chat/AddFriendConfirm", { ...data }, callback);
};
//删除联系人
export const DeleteFriend = (data, callback) => {
  return http.post("/api/Chat/DeleteFriend", { ...data }, callback);
};
