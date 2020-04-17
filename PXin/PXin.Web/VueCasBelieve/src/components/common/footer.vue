<template>
  <!-- 底部导航 -->
  <van-tabbar route v-model="active" active-color="#3399ff" inactive-color="#333333">
    <van-tabbar-item to="/">
      <span class="msg">
        {{$t('m.newchat')}}
        <i class="msgCount" v-show="chatNoticeMsgCount>0">{{chatNoticeMsgCount}}</i>
      </span>
      <img
        slot="icon"
        slot-scope="props"
        :src="props.active ?  require('@/assets/images/main_chat_sel@2x.png') : require('@/assets/images/main_chat_nor@2x.png') "
      />
    </van-tabbar-item>
    <van-tabbar-item to="/AddressBookHome">
      <span class="msg">
        {{$t('m.contacts')}}
        <i
          class="msgCount"
          v-show="contactNotificationMsgCount>0"
        >{{contactNotificationMsgCount}}</i>
      </span>
      <img
        slot="icon"
        slot-scope="props"
        :src="props.active ?   require('@/assets/images/main_list_sel@2x.png') :require('@/assets/images/main_list_nor@2x.png') "
      />
    </van-tabbar-item>
    <van-tabbar-item icon="search" :dot="hasNewMessage" to="/FindHome">
      <span>{{$t('m.discover')}}</span>
      <img
        slot="icon"
        slot-scope="props"
        :src="props.active ?   require('@/assets/images/main_discover_sel@2x.png') :require('@/assets/images/main_discover_nor@2x.png') "
      />
    </van-tabbar-item>
    <van-tabbar-item to="/my">
      <span>{{$t('m.me')}}</span>
      <img
        slot="icon"
        slot-scope="props"
        :src="props.active ?   require('@/assets/images/main_personal_sel@2x.png') :require('@/assets/images/main_personal_nor@2x.png') "
      />
    </van-tabbar-item>
  </van-tabbar>
</template>

<script>
import ImService from "@/config/imService";
import md5 from "js-md5";
import { encodeUtf8, GetSequence, setStore, getStore } from "@/config/utils.js";
import { executeServerMethod } from "@/config/signalr.js";
import { QueryFriend } from "@/api/getChatData";
import { getMyFriendByUserId } from "@/api/localStorageData";
export default {
  props: ["hasNewMessage"],
  data() {
    return {
      active: 0,
      chatNoticeMsgCount: 0,
      contactNotificationMsgCount: 0
    };
  },
  methods: {
    ListenerMessage() {
      let _this = this;
      _this.$eventBus.$on("TextMessage", function(message) {
        _this.chatNoticeMsgCount += 1;
      });
      _this.$eventBus.$on("ContactNotificationMessage", function(message) {
        //let msg = getStore("ContactNotificationMessage");
        // let messageString = JSON.stringify({
        //   sourceUserId: message.content.sourceUserId,
        //   targetUserId: message.content.targetUserId,
        //   operation: message.content.operation,
        //   messageName: message.content.messageName,
        //   conversationType: message.conversationType,
        //   messageType: message.messageType,
        //   objectName: message.objectName,
        //   senderUserId: message.senderUserId,
        //   targetId: message.targetId
        // });
        // if (msg != messageString) {
        //   setStore("ContactNotificationMessage", messageString);
        // }
        _this.contactNotificationMsgCount += 1;
        if (message.content.operation == "respfriendpass") {
          //添加好友确定推送消息,处理好友缓存
          let friendList = localStorage.getItem("MyFriendList");
          if (friendList) {
            let friendListObj = JSON.parse(friendList);
            let data = {
              ...JSON.parse(sessionStorage.userParam),
              pageIndex: 1,
              pageSize: 1
            };
            QueryFriend(data, resp => {
              if (resp.result > 0) {
                friendListObj.push(resp.data.item[0]);
                localStorage.setItem(
                  "MyFriendList",
                  JSON.stringify(friendListObj)
                );
              }
            });
          }
        }
      });
    },
    //获取会话列表
    getConversationList() {
      let _this = this;
      ImService.getConversationList(list => {
        // list => 会话列表集合
        let chatMsgCount = 0;
        let contactMsgCount = 0;
        list.forEach(element => {
          if (element.conversationType == 1) {
            getMyFriendByUserId(
              {
                ...JSON.parse(sessionStorage.getItem("userParam")),
                userId: element.targetId
              },
              res => {
                if (!res) {
                  return;
                }
                chatMsgCount += element.unreadMessageCount;
              }
            );
          } else if (
            element.conversationType == 6 &&
            element.objectName == "RC:ContactNtf" &&
            element.latestMessage.content.operation != "respfriendpass" &&
            element.unreadMessageCount > 0
          ) {
            contactMsgCount += 1;
          }
        });

        _this.chatNoticeMsgCount = chatMsgCount;
        _this.contactNotificationMsgCount = contactMsgCount;
      });
    }
  },
  mounted() {
    let _this = this;
    if (ImService.ConnectionStatus == 0) {
      _this.getConversationList();
    } else {
      _this.$eventBus.$on("ConnectStatus", function(message) {
        if (message) {
          _this.getConversationList();
        }
      });
    }
    _this.ListenerMessage();

    let loginInfo = getStore("SignalRLoginInfo");
    if (!loginInfo) {
      let userConfigInfo = getStore("UserConfigInfo");
      let nodecode = JSON.parse(userConfigInfo).data.nodecode;
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
      executeServerMethod(jsonData);
    }
  },
  destroyed() {
    this.$eventBus.$off("ContactNotificationMessage");
    this.$eventBus.$off("ConnectStatus");
    this.$eventBus.$off("TextMessage");
  }
};
</script>

<style scoped lang='scss'>
.msg {
  position: relative;

  .msgCount {
    position: absolute;
    right: -0.2rem;
    top: -0.07rem;
    width: 0.24rem;
    height: 0.24rem;
    background-color: #ff0000;
    font-family: PingFang-SC-Medium;
    font-size: 0.2rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #fffefe;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-style: normal;
  }
}
@supports (bottom: env(safe-area-inset-bottom)) {
  .van-tabbar {
    padding-bottom: env(safe-area-inset-bottom);
  }
}

.van-tabbar {
  z-index: 0 !important;
  height: 0.98rem;

  /deep/ .van-tabbar-item__icon {
    width: 0.42rem;
    height: 0.42rem;

    img {
      width: 100%;
      height: 100%;
    }
  }

  .van-tabbar-item__text {
    font-family: MicrosoftYaHei;
    font-size: 0.2rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
  }
}
</style>
