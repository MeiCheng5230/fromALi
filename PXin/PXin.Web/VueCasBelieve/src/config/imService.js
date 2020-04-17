import Vue from "vue";
import { getStore, setStore } from "@/config/utils.js";
const callback = {
  onSuccess: function(userId) {
    Vue.prototype.$eventBus.$emit("ConnectStatus", true);
    console.log("Connect successfully. " + userId);
  },
  onTokenIncorrect: function() {
    console.log("token无效");
  },
  onError: function(errorCode) {
    var info = "";
    switch (errorCode) {
      case RongIMLib.ErrorCode.TIMEOUT:
        info = "超时";
        break;
      case RongIMLib.ConnectionState.UNACCEPTABLE_PAROTOCOL_VERSION:
        info = "不可接受的协议版本";
        break;
      case RongIMLib.ConnectionState.IDENTIFIER_REJECTED:
        info = "appkey不正确";
        break;
      case RongIMLib.ConnectionState.SERVER_UNAVAILABLE:
        info = "服务器不可用";
        break;
    }
    console.log(info);
  }
};
const config = {
  // 默认 false, true 启用自动重连，启用则为必选参数
  auto: true,
  // 网络嗅探地址 [http(s)://]cdn.ronghub.com/RongIMLib-2.2.6.min.js 可选
  url: "http://cdn.ronghub.com/RongIMLib-2.4.0.min.js",
  // 重试频率 [100, 1000, 3000, 6000, 10000, 18000] 单位为毫秒，可选
  rate: [100, 1000, 3000, 6000, 10000]
};
const Believe = {
  ConnectionStatus: -1,
  //官方初始化
  sdkInit: function(params) {
    let appKey = params.appKey;
    let token = params.token;
    RongIMLib.RongIMClient.init(appKey);
    // 连接状态监听器
    RongIMClient.setConnectionStatusListener({
      onChanged: function(status) {
        Believe.ConnectionStatus = status;
        // status 标识当前连接状态
        switch (status) {
          case RongIMLib.ConnectionStatus.CONNECTED:
            console.log("链接成功");
            break;
          case RongIMLib.ConnectionStatus.CONNECTING:
            console.log("正在链接");
            break;
          case RongIMLib.ConnectionStatus.DISCONNECTED:
            console.log("断开连接");
            break;
          case RongIMLib.ConnectionStatus.KICKED_OFFLINE_BY_OTHER_CLIENT:
            console.log("其他设备登录");
            break;
          case RongIMLib.ConnectionStatus.DOMAIN_INCORRECT:
            console.log("域名不正确");
            break;
          case RongIMLib.ConnectionStatus.NETWORK_UNAVAILABLE:
            console.log("网络不可用");
            break;
        }
      }
    });
    // 消息监听器
    RongIMClient.setOnReceiveMessageListener({
      // 接收到的消息
      onReceived: function(message) {
        console.log("接收到新的消息" + JSON.stringify(message));
        //更新会话缓存
        Believe.updateConversationCache(true, message);
        switch (message.messageType) {
          case RongIMClient.MessageType.TextMessage:
            break;
          case RongIMClient.MessageType.VoiceMessage:
            // message.content.content => 格式为 AMR 的音频 base64
            break;
          case RongIMClient.MessageType.ImageMessage:
            // message.content.content => 图片缩略图 base64
            // message.content.imageUri => 原图 URL
            break;
          case RongIMClient.MessageType.LocationMessage:
            // message.content.latiude => 纬度
            // message.content.longitude => 经度
            // message.content.content => 位置图片 base64
            break;
          case RongIMClient.MessageType.RichContentMessage:
            // message.content.content => 文本消息内容
            // message.content.imageUri => 图片 base64
            // message.content.url => 原图 URL
            break;
          case RongIMClient.MessageType.InformationNotificationMessage:
            // do something
            break;
          case RongIMClient.MessageType.ContactNotificationMessage:
            Vue.prototype.$eventBus.$emit(
              "ContactNotificationMessage",
              message
            );
            // do something
            break;
          case RongIMClient.MessageType.ProfileNotificationMessage:
            // do something
            break;
          case RongIMClient.MessageType.CommandNotificationMessage:
            // do something
            break;
          case RongIMClient.MessageType.CommandMessage:
            // do something
            break;
          case RongIMClient.MessageType.UnknownMessage:
            // do something
            break;
          default:
          // do something
        }
      }
    });
    //开始连接
    RongIMClient.connect(token, callback);
  },
  reconnect: function name() {
    RongIMClient.reconnect(callback, config);
  },
  ImageUpload: function(event) {
    var thisTarget = event.target || event.srcElement;
    var _file = thisTarget.files;
    var file = _file[0];

    var fileReader = new FileReader();
    fileReader.readAsDataURL(file);
    fileReader.onabort = function(e) {
      console.log("文件读取异常" + file.name);
    };
    fileReader.onerror = function(e) {
      console.log("文件读取出现错误" + file.name);
    };

    fileReader.onload = function(e) {
      if (e.target.result) {
        let imageBase64Str;
        console.log("完成" + file.name);
        imageBase64Str = e.target.result;
        // console.log(imageBase64Str);
        // if (fileLength < file.length) {
        //     reader.readAsDataURL(file);
        // } else {
        // }
        // console.log(imageBase64Str);
        let imageBase64Array = imageBase64Str.split(",");
        let suffixStr = imageBase64Array[0];
        let index = suffixStr.indexOf("/") + 1;
        let suffix = suffixStr.substr(index, suffixStr.length - 7 - index);
        let imageBase64Content = imageBase64Array[1];

        // $.ajax({
        //   type: "post",
        //   url: "http://localhost:4778/api/Sys/UploadFile",
        //   data: {
        //     content: imageBase64Content,
        //     typeid: suffix,
        //     nodeid: 3434909,
        //     sid: 81123
        //   },
        //   dataType: "json",
        //   success: function(res) {
        //     if (res.result > 0) {
        //       var imageMsg = new RongIMLib.ImageMessage({
        //         content: "",
        //         imageUri: res.data.fullurl,
        //         isFull: false,
        //         extra: ""
        //       });
        //       PXin.sendMessage(imageMsg);
        //     }
        //   }
        // });
      }
    };
  },
  playVoice: function(t) {
    let voice = t.getAttribute("data-message");
    if (voice) {
      var duration = voice.length / 1024; // 音频持续大概时间(秒)
      PXin.RongIMVoice.preLoaded(voice, function() {
        PXin.RongIMVoice.play(voice, duration);
      });
    } else {
      console.error("请传入 amr 格式的 base64 音频文件");
    }
  },
  keyboard: function(event) {
    var thisTarget = event.target || event.srcElement;
    setTimeout(function() {
      thisTarget.scrollIntoView(true);
    }, 500);
  },
  keySend: function(event) {
    if (event.keyCode == "13" && !event.shiftKey) {
      event.preventDefault();
      let msg = $("#chatbox-message").val();
      if (!msg) {
        $("#chatbox-message").focus();
        return;
      }
      let message = new RongIMLib.TextMessage({
        content: msg,
        extra: "附加信息"
      });
      PXin.sendMessage(message);
    } else {
      //inputChange();
    }
  },
  init: function(config) {
    //初始化
    this.sdkInit(config);
  },
  //获取会话列表
  getConversationList(callback) {
    let conversationListString = getStore("ConversationList");
    if (conversationListString) {
      callback(JSON.parse(conversationListString));
    } else {
      RongIMClient.getInstance().getConversationList(
        {
          onSuccess: function(list) {
            setStore("ConversationList", list);
            callback(list);
          },
          onError: function(error) {
            console.log(error);
          }
        },
        null
      );
    }
  },
  //更新会话缓存
  updateConversationCache(isBus, message) {
    RongIMClient.getInstance().getConversationList(
      {
        onSuccess: function(list) {
          setStore("ConversationList", list);
          if (isBus) {
            Vue.prototype.$eventBus.$emit(message.messageType, message);
          }
        },
        onError: function(error) {
          console.log(error);
        }
      },
      null
    );
  }
};

export default Believe;
