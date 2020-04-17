<template>
  <div class="NewFriend">
    <div class="NewFriendBar">
      <div></div>
      <div>新的朋友</div>
      <div @click="$router.push('/AddFriend')">
        <img src="@/assets/images/list_add-friends@2x.png" alt />
      </div>
    </div>
    <div style="height: .6rem;"></div>
    <!-- 搜索 -->
    <router-link to="/AddressSearch" tag="div" class="search">
      <div :style="{backgroundImage:'url('+require('@/assets/images/addfriend_search@2x.png')+')'}">
        <input placeholder="手机号" type="text" />
      </div>
    </router-link>
    <!-- 历史数据 -->
    <div class="history">
      <!--  -->
      <div v-for="(item,index) of NewFriendDataList" :key="index">
        <h3>{{item.time}}</h3>
        <!-- 列表 -->
        <div class>
          <div class="historyUser" v-for="(item2,index) of item.NewFrined" :key="index">
            <!-- 左 -->
            <div>
              <div>
                <img :src="item2.appPhoto" alt />
              </div>
              <div>
                <span>{{item2.NodeName}}</span>
                <span>{{item2.message}}</span>
              </div>
            </div>
            <!-- 右 -->
            <div>
              <button
                :disabled="item2.status!=='同意' && true"
                @click="agentAddFriend(item2)"
                :class="item2.status!=='同意' && 'btnColor'"
              >{{item2.status}}</button>
            </div>
          </div>
        </div>
      </div>
      <!--  -->
    </div>
  </div>
</template>

<script>
import ImService from "@/config/imService";
import { AddFriendConfirm, QueryFriend } from "@/api/getChatData";
import { timeout } from "q";

export default {
  data() {
    return {
      NewFriendDataList: [
        // {
        //   time: "昨天",
        //   NewFrined: [
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       title2: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       status: "同意"
        //     },
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       title2: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       status: "已添加"
        //     }
        //   ]
        // },
        // {
        //   time: "三天以前",
        //   NewFrined: [
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       title2: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       status: "已过期"
        //     },
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       title2: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       status: "已过期"
        //     }
        //   ]
        // },
        // {
        //   time: "昨天",
        //   NewFrined: [
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       title2: "这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1这是昵称1",
        //       status: "同意"
        //     },
        //     {
        //       url: require("@/assets/images/login_bg@2x.png"),
        //       title1: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       title2: "这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2这是昵称2",
        //       status: "已添加"
        //     }
        //   ]
        // }
      ]
    };
  },
  mounted() {
    this.getConversationList();
  },
  methods: {
    //点击同意
    agentAddFriend(item) {
      let _this = this;
      AddFriendConfirm(
        {
          ...JSON.parse(sessionStorage.getItem("userParam")),
          usercode: item.nodeCode,
          status: 1,
          remark: ""
        },
        res => {
          if (res.result > 0) {
            this.$toast("添加成功");
            item.status = "已添加";
            let friendList = localStorage.getItem("MyFriendList");
            if (friendList) {
              let friendListObj = JSON.parse(friendList);
              let data = {
                ...JSON.parse(sessionStorage.userParam),
                key: item.nodeCode,
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
                  _this.$router.replace("/AddressBookHome");
                } else {
                  this.$toast(resp.message);
                }
              });
              setStore("ContactNotificationMessage", "");
            }
          } else {
            this.$toast(res.message);
          }
          _this.clearUnreadCount(item.targetId);
        }
      );
    },
    //获取会话列表
    getConversationList() {
      let _this = this;
      _this.NewFriendDataList = [];
      ImService.getConversationList(list => {
        let chatMsgConversations = list.filter(function(item) {
          if (
            item.conversationType == 6 &&
            item.objectName == "RC:ContactNtf" &&
            item.latestMessage.content.operation != "respfriendpass" &&
            item.unreadMessageCount > 0
          ) {
            return item;
          }
        });
        let newFriend = [];
        chatMsgConversations.forEach(ele => {
          let content = ele.latestMessage.content;
          let userInfo = JSON.parse(content.extra);
          newFriend.push({
            appPhoto: userInfo.AppPhoto,
            NodeName: userInfo.NodeName,
            message: content.message,
            status:
              ele.latestMessage.content.operation == "addfriend"
                ? ele.unreadMessageCount > 0
                  ? "同意"
                  : "已添加"
                : "已通过",
            targetId: ele.targetId,
            nodeCode: userInfo.NodeCode
          });
          // if (ele.latestMessage.content.operation == "respfriendpass") {
          //   _this.clearUnreadCount(ele.targetId);
          // }
        });
        _this.NewFriendDataList.push({
          time: "今天",
          NewFrined: newFriend
        });
      });
    },
    //清除未读消息数
    clearUnreadCount(targetId) {
      let _this = this;
      let conversationType = RongIMLib.ConversationType.SYSTEM;
      RongIMClient.getInstance().clearUnreadCount(conversationType, targetId, {
        onSuccess: function() {
          // 清除未读消息成功
          ImService.updateConversationCache(false, "");
          setTimeout(() => {
            _this.getConversationList();
          }, 500);
        },
        onError: function(error) {
          // error => 清除未读消息数错误码
        }
      });
    }
  }
};
</script>

<style scoped lang='scss'>
.btnColor {
  background: #fff !important;
  color: #999 !important;
}

.NewFriend {
  padding-bottom: 1rem;
  .history {
    h3 {
      padding-left: 0.3rem;
      line-height: 0.88rem;
      margin: 0;
      height: 0.88rem;
      background: #f2f2f2;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #666666;
    }

    & > div > div {
      padding-left: 0.3rem;
    }

    .historyUser {
      padding-right: 0.3rem;
      display: flex;
      justify-content: space-between;
      border-bottom: 0.02rem solid #d1d1d1;

      div:nth-child(1) {
        display: flex;
        height: 1.68rem;
        align-items: center;

        div:nth-child(1) {
          width: 1rem;
          height: 1rem;

          img {
            width: 100%;
            height: 100%;
            border-radius: 50%;
          }
        }

        div:nth-child(2) {
          width: 3.8rem;
          display: flex;
          flex-direction: column;
          font-family: PingFang-SC-Medium;
          height: 1rem;
          box-sizing: border-box;
          padding: 0.05rem 0;
          padding-left: 0.2rem;
          justify-content: space-between;
          font-size: 0.3rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #333333;
          align-items: flex-start;

          span {
            width: 100%;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
          }

          span:nth-child(2) {
            font-size: 0.24rem;
            color: #999;
          }
        }
      }

      div:nth-child(2) {
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
        display: flex;

        align-items: center;

        button {
          width: 1.48rem;
          height: 0.58rem;
          background-color: #2ea2fa;
          border-radius: 0.04rem;
          border: 0;
          outline: none;
          font-family: PingFang-SC-Medium;
          font-size: 0.28rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #ffffff;
        }
      }
    }
  }

  .search {
    width: 100%;
    box-sizing: border-box;
    padding: 0.3rem;
    background: #f2f2f2;

    & > div {
      height: 0.68rem;
      width: 100%;
      box-sizing: border-box;
      background: #fff;
      border-radius: 0.1rem;
      background-repeat: no-repeat;
      background-size: 0.38rem 0.42rem;
      background-position: 0.6rem center;
      padding-left: 1.14rem;
      display: flex;

      input {
        width: 100%;
        height: 100%;
        box-sizing: border-box;
        padding: 0;
        border: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.28rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
      }
    }
  }

  .NewFriendBar {
    height: 0.6rem;
    box-sizing: border-box;
    padding: 0 0.48rem;
    display: flex;
    align-items: center;
    background-color: #2ea2fa;
    position: fixed;
    width: 100%;
    div {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      width: 0.4rem;
      height: 0.4rem;

      img {
        width: 100%;
        height: 100%;
      }
    }

    div:nth-child(2) {
      flex: 1;
      text-align: center;
    }
  }
}

.history > div > div > div:last-child {
  border-bottom: 0 !important;
}
</style>
