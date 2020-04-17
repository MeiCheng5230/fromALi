import http from "@/api/http.js";
import axios from "axios";
import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

let store = new Vuex.Store({
  issale: false
});

function callbackResult(list, userId, callback) {
  let result;
  for (let index = 0; index < list.length; index++) {
    const element = list[index];
    if (element.nodeid == userId) {
      result = element;
      break;
    }
  }
  return callback(result);
}
export const initCacheData = (data) => {
  console.log("store.state.isFirst==>"+store.state.isFirst);
  if (!store.state.isFirst) {
    store.state.isFirst = true;
    axios
      .post("/api/Chat/MyFriend", { ...data })
      .then(function(res) {
        localStorage.setItem("MyFriendList", JSON.stringify(res.data.data));
      })
      .catch();
  }
};
//根据好友会话id获取好友信息
export const getMyFriendByUserId = (data, callback) => {
  let friendList = localStorage.getItem("MyFriendList");
  if (friendList) {
    let friendListJson = JSON.parse(friendList);
    return callbackResult(friendListJson, data.userId, callback);
  } 
  else {
    http.post("/api/Chat/MyFriend", { ...data }, res => {
      localStorage.setItem("MyFriendList", JSON.stringify(res.data));
      return callbackResult(res.data, data.userId, callback);
    });
  }
};
//我的好友
export const MyFriend = (data, callback) => {
  let friendList = localStorage.getItem("MyFriendList");
  if (friendList) {
    return callback(JSON.parse(friendList));
  } else {
    http.post("/api/Chat/MyFriend", { ...data }, res => {
      localStorage.setItem("MyFriendList", JSON.stringify(res.data));
      return callback(res.data);
    });
  }
};
