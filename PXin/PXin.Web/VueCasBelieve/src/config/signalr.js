import Vue from "vue";
import md5 from "js-md5";
import { encodeUtf8, GetSequence, getStore } from "./utils.js";

const HUBNAME = "chatHub";
function getConnectionUrl() {
  let host = window.location.host;
  if (
    process.env.NODE_ENV == "production" &&
    (host != "client.be.sulink.cn" && host != "pxin.ckv-test.sulink.cn")
  ) {
    return signalrUrl; //"http://cas.51shumeng.com/signalr";
  }
  return process.env.VUE_APP_SIGNALR_URL;
}
function Login(nodecode) {
  let pwd = Math.random() * (999999 - 100000) + 100000;
  let sign = md5(encodeUtf8(nodecode + pwd + "DvUZIrmKXs"));
  let jsonData = {
    command_id: 0x00000001,
    sequence_id: GetSequence(),
    total_length: 0,
    clientId: 1001,
    version: 1.0,
    nodeCode: nodecode,
    pwd: pwd,
    sign: sign
  };
  signalrInfo.proxy
    .invoke("receiveChatMessage", JSON.stringify(jsonData))
    .done(function() {
      console.log("Invocation of receiveChatMessage succeeded");
    })
    .fail(function(error) {
      console.log("Invocation of receiveChatMessage failed. Error: " + error);
    });
}
function LoginResp(message) {
  console.log("LoginResp==>" + message);
  if (message.Status == 1) {
    sessionStorage.setItem("SignalRLoginInfo", JSON.stringify(message));
    Vue.prototype.$eventBus.$emit("Login", "messageBody");
  }
}
const receiveMessage = {
  name: "receiveMessage",
  method: function(message) {
    console.log("receiveMessage==>" + JSON.stringify(message));
    let messageResp = JSON.parse(message);
    let messageHander = messageResp.Header;
    let messageBody = messageResp.Body;
    let commandType = "0x" + messageHander.Command_Id.toString(16);
    console.log(commandType);
    switch (commandType) {
      case "0x80000001":
        LoginResp(messageBody);
        break;
      case "0x80000004": //聊天计费返回结果
        Vue.prototype.$eventBus.$emit("ChatFeeResp", messageBody);
        break;
      case "0x5": //聊天计费推送
        Vue.prototype.$eventBus.$emit("ChatFeePush", messageBody);
        break;
      case "0x80000006": //聊天计费倍率设置返回结果
        Vue.prototype.$eventBus.$emit("ChatFeeRateSetResp", messageBody);
        break;
      case "0x80000007": //查询倍率返回结果
        Vue.prototype.$eventBus.$emit("chatFeeRateQueryResp", messageBody);
        break;
      case "0x8": //聊天计费倍率设置推送
        Vue.prototype.$eventBus.$emit("ChatFeeRateSetPush", messageBody);
        break;
      default:
        break;
    }
    // Vue.$eventBus.$emit('chatFeeRateQuery','data-1232131232132131232');
  }
};
const addFriendMessage = {
  name: "addFriendMessage",
  method: function(message) {
    console.log("addFriendMessage==>" + message);
  }
};
//客户端的方法
const clientMethods = [receiveMessage, addFriendMessage]; //将需要注册的方法放进集合

let signalrInfo = {};
export function startConnection(nodecode) {
  signalrInfo.connection = $.hubConnection(getConnectionUrl());
  signalrInfo.proxy = createHubProxy(signalrInfo.connection);
  signalrInfo.connection.stop();
  signalrInfo.connection.start().done(function(connection) {
    signalrInfo.connection.state = connection.state;
    Login(nodecode);
  });

  signalrInfo.connection.stateChanged(function(s) {
    console.log("状态发生改变==>" + JSON.stringify(s));
    console.log(signalrInfo.connection.state);
  });

  signalrInfo.connection.disconnected(function() {
    console.log("断开连接");
    console.log(signalrInfo.connection.state);
    setTimeout(() => {
      signalrInfo.connection.start().done(function(connection) {
        Login(nodecode);
      });
    }, 5000);
  });

  signalrInfo.connection.error(function(error) {
    console.log("错误信息：==>" + error);
  });
}

export function disConnection() {
  // signalrInfo.connection.closed();
  signalrInfo.connection.state = 0;
}

export function createHubProxy(hub) {
  let proxy = hub.createHubProxy(HUBNAME);

  // 注册客户端方法
  clientMethods.map(item => {
    proxy.on(item.name, item.method);
  });
  return proxy;
}

export function executeServerMethod(data) {
  console.log("executeServerMethod==>" + JSON.stringify(data));
  console.log("connectionStatus==>" + signalrInfo.connection.state);
  signalrInfo.proxy
    .invoke("receiveChatMessage", JSON.stringify(data))
    .done(function() {
      console.log("Invocation of receiveChatMessage succeeded");
    })
    .fail(function(error) {
      console.log("Invocation of receiveChatMessage failed. Error: " + error);
    });
  return signalrInfo.proxy;
}

export function getSignalrConnectionStatus() {
  if (signalrInfo.connection.state) {
    return signalrInfo.connection.state;
  } else {
    return 0;
  }
}
