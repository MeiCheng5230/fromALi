<template>
  <div class="home">
    <!-- <div class='fixed'>
     <div class='chat'>
      <div></div>
      <div>聊一聊</div>
      <div slot='right'>
        <img @click='popupShow=true;' src="@/assets/images/chat_add@2x.png" alt="">
         点击加号
        <div v-show='popupShow' class='popupContent' :style='{backgroundImage:"url("+require("@/assets/images/chat_add_open@2x.png")+")"}'>
          <div>
            <router-link to='/NewGroupChat' tag='div'>
              <img src="@/assets/images/chat_groupchat@2x.png" alt="">
              <span>发起群聊</span>
            </router-link>
            <router-link to='/AddFriend' tag='div'>
              <img src="@/assets/images/chat_addfriend@2x.png" alt="">
              <span>添加朋友</span>
            </router-link>
            <div>
              <img src="@/assets/images/chat_saoyisao@2x.png" alt="">
              <span>扫一扫</span>
            </div>
          </div>
        </div>
        <van-popup v-model="popupShow" :overlay-style='{"background-color": "rgba(0, 0, 0, 0.3)"}'></van-popup>
      </div>
    </div>
    </div>-->
    <van-pull-refresh
      v-model="isLoading"
      loading-text=" "
      loosing-text=" "
      pulling-text=" "
      @refresh="onRefresh"
      :head-height="0"
    >
      <div class="content paddingBottom">
        <!-- 左右滑动 -->
        <div class="slider" v-show="isLoading" ref="slider">
          <div class="slider-group">
            <div>
              <p>
                <img src="@/assets/images/home_auction@2x@2x.png" alt />
              </p>
              <p>A点竞拍</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_redpack@2x@2x.png" alt />
              </p>
              <p>领取红包</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_B@2x@2x.png" alt />
              </p>
              <p>B指数</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_exchange@2x@2x.png" alt />
              </p>
              <p>兑换专区</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_wallet@2x@2x.png" alt />
              </p>
              <p>钱包</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_rechargecode@2x.png" alt />
              </p>
              <p>充值码</p>
            </div>
            <div>
              <p>
                <img src="@/assets/images/home_businessman@2x.png" alt />
              </p>
              <p>充值商</p>
            </div>
            <div>
              <p></p>
            </div>
          </div>
        </div>
        <!-- 搜索组件 -->
        <mysearch @click.native="$router.push('/AddressSearch')" :placeholder="$t('m.search')"></mysearch>
        <!-- 对话列表 -->
        <div class="chatList">
          <!-- 聊天列表 -->
          <router-link
            :to="{path:'/Chats',query:{type:item.type,userId:item.userId}}"
            tag="div"
            class="chatItem"
            v-for="(item,index) of conversationList"
            :key="index"
          >
            <van-swipe-cell>
              <div class="item">
                <!-- 头像 -->
                <div class="chargroup">
                  <img :src="item.appphoto" alt />
                  <img
                    v-if="item.type==3"
                    class="imgFixed"
                    src="@/assets/images/groupchatting@2x.png"
                    alt
                  />
                </div>
                <!-- 对话人名称 内容 -->
                <div class="userChat">
                  <p>{{item.chatname}}</p>
                  <p>{{item.lastinfo}}</p>
                </div>
                <!-- 时间,未读数量 -->
                <div class="time">
                  <p>{{item.lasttime}}</p>
                  <p
                    v-show="item.message>0"
                    :class="item.message<10 &&'radius50'"
                  >{{item.message>99?'99+':item.message}}</p>
                </div>
              </div>
              <van-button
                square
                slot="right"
                type="danger"
                text="删除"
                @click="deleteItem($event,item.type,item.userId)"
              />
            </van-swipe-cell>
          </router-link>
        </div>
      </div>
    </van-pull-refresh>

    <!-- 底部 -->
    <myfooter></myfooter>
  </div>
</template>

<script>
import myfooter from "@/components/common/footer";
import mysearch from "@/components/common/search";
import { initCacheData, getMyFriendByUserId } from "@/api/localStorageData";
import { getChatTime } from "@/config/utils";
import ImService from "@/config/imService";
export default {
  name: "HelloWorld",
  data() {
    return {
      isLoading: false, //下拉
      //点击加号弹出框  已改版 废除
      // popupShow: false,
      //聊天列表
      conversationList: [
        //type:1 群聊 2 单聊
        // {
        //   chatname:
        //     "群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊群聊",
        //   lasttime: "10:14",
        //   message: 989,
        //   lastinfo: "这是最后一条信息",
        //   imgUrl: require("@/assets/images/discover_dynamic@2x.png"),
        //   type: 1
        // },
        // {
        //   chatname:
        //     "单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊单聊",
        //   lasttime: "5:14",
        //   message: 9,
        //   lastinfo: "这是最后一条信息",
        //   imgUrl: require("@/assets/images/discover_dynamic@2x.png"),
        //   type: 2
        // }
      ]
    };
  },
  created() {
    initCacheData({ ...JSON.parse(sessionStorage.getItem("userParam")) });
  },
  mounted() {
    let _this = this;
    window.addEventListener("scroll", this.scroll);
    // this.getAppNativeHead();

    this.ListenerMessage();

    if (ImService.ConnectionStatus == 0) {
      _this.getConversationList(_this);
    } else {
      _this.$eventBus.$on("ConnectStatus", function(message) {
        if (message) {
          _this.getConversationList(_this);
        }
      });
    }
  },
  methods: {
    //调用原生方法改变头部
    getAppNativeHead() {
      let that = this;
      let Sign = [
        {
          PStr: "eyJuYXZpdHlwZSI6MH0=",
          Sign: "B45473F9D8711AB7E1B4B8413AFD5D91"
        }, //白色原生头部
        {
          PStr: "eyJuYXZpdHlwZSI6MX0=",
          Sign: "D1A97BBB39A4B528B295BDF1E9276FA5"
        }, //黑色原生头部
        {
          PStr: "eyJuYXZpdHlwZSI6Mn0=",
          Sign: "131850A5C3530A2BCC988D2E47403250"
        } //透明
      ];
      try {
        let appName = window.AppNative.AppName;
        if (appName == "pcn" || appName == "yougu") {
          AppNative.jsTunedupNativeWithTypeParamSign(
            1014,
            Sign[2].PStr,
            Sign[2].Sign
          );
        }
      } catch (e) {
        //根据ui框架自行修改Toast提示API
        this.$toast(e);
      }
    },

    scroll(e) {
      // 滚动高度
      let scrTopHeight =
        window.pageYOffset ||
        document.documentElement.scrollTop ||
        document.body.scrollTop;

      //元素高度
      let EleHeight = this.$refs.slider.offsetHeight;

      if (scrTopHeight > EleHeight + 10 && this.isLoading) {
        //滑动大于元素高度, 影藏
        this.isLoading = false;
      }
    },
    onRefresh() {
      // setTimeout(() => {
      //   this.$toast('刷新成功');
      //   this.isLoading = true;
      //   this.count++;
      // }, 500);
    },
    deleteItem(event, conversationType, targetId) {
      this.removeConversation(event, conversationType, targetId);

      // this.chatDataList.splice(index,1);
    },
    ListenerMessage() {
      let _this = this;
      _this.$eventBus.$on("TextMessage", function(message) {
        _this.getConversationList(_this);
      });
    },
    //获取会话列表
    getConversationList(_this) {
      ImService.getConversationList(list => {
        let chatMsgConversations = list.filter(function(item) {
          if (item.conversationType == 1) {
            return item;
          }
        });
        console.log("会话列表==>"+JSON.stringify(chatMsgConversations));
        _this.conversationList = [];
        chatMsgConversations.forEach(element => {
          getMyFriendByUserId(
            {
              ...JSON.parse(sessionStorage.getItem("userParam")),
              userId: element.targetId
            },
            res => {
              if (!res) {
                return;
              }
              let time = new Date(
                element.latestMessage.sentTime
              ).toDateString();
              _this.conversationList.push({
                userId: element.targetId,
                chatname: res.nodename,
                lasttime: getChatTime(element.latestMessage.sentTime),
                message: element.unreadMessageCount,
                lastinfo: RongIMLib.RongIMEmoji.symbolToEmoji(
                  element.latestMessage.content.content
                ),
                appphoto: res.appphoto
                  ? res.appphoto
                  : require("@/assets/images/discover_dynamic@2x.png"),
                type: element.conversationType
              });
            }
          );
        });
      });
    },
    //删除会话
    removeConversation(event, conversationType, targetId) {
      RongIMClient.getInstance().removeConversation(
        conversationType,
        targetId,
        {
          onSuccess: function(bool) {
            //删除当前元素
            document
              .getElementsByClassName("chatList")[0]
              .removeChild(
                event.target.parentNode.parentNode.parentNode.parentNode
              );
          },
          onError: function(error) {
            // error => 删除会话的错误码
          }
        }
      );
    }
  },
  components: {
    myfooter,
    mysearch
  },
  destroyed() {
    //离开页面,销毁scroll事件
    window.removeEventListener("scroll", this.scroll);
    this.$eventBus.$off("ConnectStatus");
    this.$eventBus.$off("TextMessage");
  }
};
</script>


<style scoped lang='scss'>
.chargroup {
  position: relative;
  .imgFixed {
    width: 0.28rem !important;
    height: 0.24rem !important;
    position: absolute;
    border-radius: 0 !important;
    right: 0;
    bottom: 0;
  }
}
.slider-group::-webkit-scrollbar {
  display: none !important;
}
.radius50 {
  width: 0.28rem !important;
  height: 0.28rem !important;
  padding: 0 !important;
  border-radius: 50% !important;
}
.slider-group {
  /* height: 100%; */
  display: flex;
  overflow-x: scroll;

  & > div {
    display: flex;
    flex-direction: column;
    /* justify-content: space-between; */
    width: 0.9rem;
    margin-left: 0.7rem;

    p {
      text-align: center;
      white-space: nowrap;
      margin: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;

      img {
        width: 0.9rem;
        height: 0.9rem;
      }
    }

    p:nth-child(2) {
      margin-top: 0.05rem;
    }
  }

  & > div:nth-child(1) {
    margin-left: 0.5rem;
  }

  & > div:last-child {
    p {
      width: 0.5rem;
    }

    margin-left: 0;
  }
}

.slider {
  /* padding: .6rem 0 0 .5rem; */
  padding-top: 0.6rem;
  /* height: 1.28rem; */
}

/deep/ .van-overlay {
  top: 1.56rem;
}

.van-popup {
  width: 0 !important;
}

.home {
  .fixed {
    width: 100%;
    position: fixed;
    z-index: 1;
  }

  .content {
    position: relative;
    /* top: .56rem; */
  }

  .chat {
    width: 100%;
    box-sizing: border-box;

    height: 0.56rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    background-color: #2ea2fa;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 0.48rem;

    div:nth-child(2) {
      flex: 1;
      text-align: center;
    }

    div:nth-child(1),
    div:nth-child(3) {
      width: 0.32rem;
      height: 0.32rem;

      img {
        width: 100%;
        height: 100%;
      }
    }

    .popupContent {
      z-index: 200999;
      position: absolute;

      right: 0.34rem;
      top: 0.45rem;
      box-sizing: border-box;
      padding: 0 0.15rem;
      padding-top: 0.2rem;
      width: 2.8rem;
      height: 3.5rem;
      background-size: 100% 100%;
      overflow: hidden;

      & > div {
        font-size: 0.24rem;
        width: 100%;
        height: 100%;

        & > div {
          width: 100%;
          height: 1.1rem;
          display: flex;
          align-items: center;
          font-family: PingFang-SC-Medium;
          font-size: 0.24rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #ffffff;
          border-bottom: 0.01rem solid #fff;

          span {
            padding-left: 0.43rem;
          }

          img {
            padding-left: 0.13rem;
            width: 0.42rem;
            height: 0.42rem;
          }
        }
      }
    }

    .popup {
      width: 100rem !important;
      height: 100% !important;
      position: fixed;
      top: 1.56rem;
      left: 0;
      right: 0;
      bottom: 0;
      background-color: rgba(0, 0, 0, 0.3);
      z-index: 0;
    }
  }

  .chatList {
    .chatItem {
      height: 1.68rem;
      padding-left: 0.3rem;

      .item {
        display: flex;
        height: 100%;
        align-items: center;

        p {
          margin: 0;
        }

        div:nth-child(1) {
          width: 1rem;
          height: 1rem;

          img {
            width: 100%;
            height: 100%;
            border-radius: 50%;
          }
        }

        .userChat {
          padding: 0.05rem 0 0.05rem 0.2rem;
          height: 1rem;
          box-sizing: border-box;
          display: flex;
          flex-direction: column;
          justify-content: space-between;

          p {
            width: 4rem;
            font-family: PingFang-SC-Medium;
            font-size: 0.3rem;
            font-weight: normal;
            font-stretch: normal;
            letter-spacing: 0rem;
            color: #333333;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
            vertical-align: top;
            /* height: .3rem; */
          }

          p:nth-child(2) {
            color: #999;
            font-size: 0.24rem;
          }
        }

        .time {
          flex: 1;
          text-align: center;
          display: flex;
          flex-direction: column;
          height: 1rem;
          justify-content: space-between;
          box-sizing: border-box;
          padding: 0.05rem 0;

          p {
            font-family: PingFang-SC-Medium;
            font-size: 0.24rem;
            font-weight: normal;
            font-stretch: normal;
            letter-spacing: 0rem;
            color: #999999;
          }

          p:nth-child(2) {
            // width: 0.28rem;
            // height: 0.24rem;
            line-height: 0.28rem;
            background-color: #ff1541;
            margin: 0 auto;
            padding: 0.03rem 0.1rem;
            color: #fff;
            font-size: 0.24rem;
            border-radius: 0.12rem;
            text-align: center;
          }
        }
      }

      /deep/ .van-swipe-cell__wrapper,
      .van-swipe-cell {
        height: 100%;
      }

      /deep/ .van-swipe-cell {
        border-bottom: 0.01rem solid #d1d1d1;
      }

      /deep/ .van-swipe-cell__right {
        width: 0.98rem;
        display: flex;

        .van-button {
          padding: 0;
          width: 100%;
          height: 100%;
          background-color: #ff1541;
        }

        .van-button__text {
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

  @supports (bottom: env(safe-area-inset-bottom)) {
    .chatList {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }

  /*  */
}
</style>
