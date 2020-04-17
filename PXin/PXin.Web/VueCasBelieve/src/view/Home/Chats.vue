<template>
  <div class="chats">
    <main class="scrollWrapper">
      <div class="chatsTit">
        <i></i>
        <span>{{friendInfo.nodename}}</span>
        <img
          @click="$route.query.type==1 ? $router.push({path:'/SetChat',query:{
            targetId:$route.query.userId
          }}):$router.push('/GroupChatInfo')"
          src="@/assets/images/chat_more.png"
          alt
        />
      </div>
      <!-- 倍率 -->
      <div ref="chatsPow" class="chatsPow">
        <router-link
          :to="{name:'Power',query:{targetId:friendInfo.nodeid}}"
          tag="div"
          class="PowInfo"
        >
          <div class="dynamic">
            <img src="@/assets/images/dynamic_p_sel@2x.png" alt />
          </div>
          <div class="pow">
            <p>{{pDianBalance}}</p>
            <p>接收倍率：{{receiveRate}}x</p>
          </div>
          <div class="arrow">
            <img src="@/assets/images/arrow.png" alt />
          </div>
        </router-link>

        <div class="PowInfo PowInfoRig">
          <div class="dynamic">
            <img src="@/assets/images/dynamic_v_sel@2x.png" alt />
          </div>
          <div class="pow">
            <p>{{vDianBalance}}</p>
            <p>发送倍率：{{senderRate}}x</p>
          </div>
        </div>
      </div>
      <!-- 聊天框 -->
      <div class="chatsCont" ref="chatsCont" :style="{height:chatsContHeight+'px'}">
        <van-pull-refresh v-model="isLoading" @refresh="onRefresh">
          <h3 class="time">2018-10-22</h3>
          <div class="List" ref="List">
            <div :class="item.type==1?'my':'use'" v-for="(item,index) of chatMsgList" :key="index">
              <div v-if="item.type==1" class="useText">{{item.message}}</div>
              <router-link to="/Information" tag="div" class="useImg">
                <img
                  :src="item.type==1?(userInfo.appphoto?userInfo.appphoto:require('@/assets/images/invite_weichat@2x.png')):(friendInfo.appphoto?friendInfo.appphoto:require('@/assets/images/invite_weichat@2x.png'))"
                  alt
                />
              </router-link>
              <div v-if="item.type==2" class="useText">{{item.message}}</div>
            </div>
          </div>
        </van-pull-refresh>
      </div>
    </main>

    <!-- 底部 -->
    <div class="footer" ref="footer" :class="(bqDIV || open) ? 'animationTop':'animationBot'">
      <div class="footIpt" ref="footIpt">
        <img
          :src="isSpeak?require('@/assets/images/chat_chat_smallvoice@2x.png'):require('@/assets/images/chat_chat_write@2x.png')"
          alt
          @click="isSpeak = !isSpeak"
        />
        <!-- <textarea v-model="textareaVal" class='textarea' ref='textarea' rows="" cols="" @keydown="onTextareaKeyDown"  type="text" @focus="iptfocus" @blur="iptblur" id="message-box" >
        </textarea>-->
        <div
          ref="Speak"
          @touchmove="touchmove"
          @touchend="touchend"
          @touchstart="touchstart"
          class="Speak"
          v-show="isSpeak"
        >按住 说话</div>
        <div
          v-show="!isSpeak"
          id="chat-box"
          @keydown="onTextareaInput"
          @focus="onTextareaFocus"
          @blur="onTextareaBlur"
          class="needsclick"
          contenteditable="true"
        ></div>
        <img
          @click="SwipeBq"
          :src="bqDIV?require('@/assets/images/chat_chat_write@2x.png'):require('@/assets/images/chat_chat_face@2x.png')"
          alt
          style="margin-left:.1rem;"
        />
        <img
          ref="isShow"
          class="isShow"
          @click="SwipeDiv"
          src="@/assets/images/chat_chat_add@2x.png"
          alt
        />
        <div @click="sendMsg" class="isMsg" ref="isMsg">
          <button>发送</button>
        </div>
      </div>
      <!-- 右下角表情 -->
      <div class="botContent">
        <div class="bqDIV" v-show="bqDIV" id="emoji-box"></div>
        <!-- 右下角功能加号 -->
        <div class="openDIV" v-show="open">
          <div>
            <p>
              <img src="@/assets/images/chat_chat_photo@2x.png" alt />
            </p>
            <p>照片</p>
          </div>
          <div>
            <p>
              <img src="@/assets/images/chat_chat_camera@2x.png" alt />
            </p>
            <p>相机</p>
          </div>
          <div>
            <p>
              <img src="@/assets/images/chat_chat_voice@2x.png" alt />
            </p>
            <p>语音通话</p>
          </div>
          <div>
            <p>
              <img src="@/assets/images/chat_chat_video@2x.png" alt />
            </p>
            <p>视频通话</p>
          </div>
          <div>
            <p>
              <img src="@/assets/images/chat_chat_address@2x.png" alt />
            </p>
            <p>位置</p>
          </div>
          <div>
            <p>
              <img src="@/assets/images/chat_chat_redpacket@2x.png" alt />
            </p>
            <p>红包</p>
          </div>
        </div>
      </div>
    </div>
    <!-- 说话图片 -->
    <div class="SpeakImg" ref="SpeakImg">
      <img src="@/assets/images/chat_chat_voiceprompt@2x.png" alt />
    </div>
  </div>
</template>


<script>
import ImService from "@/config/imService";
import { getStore, getDate, GetSequence } from "@/config/utils.js";
import { getMyFriendByUserId } from "@/api/localStorageData";
import {
  executeServerMethod,
  getSignalrConnectionStatus
} from "@/config/signalr.js";
import { List } from "vant";
import { userInfo } from "os";
let interval;
export default {
  data() {
    return {
      isSpeak: false,
      arr: [],
      conversation: {}, //会话对象
      RongIMEmoji: {},
      RongIMVoice: {},
      chatMsgList: [],
      userInfo: {}, //用户信息
      friendInfo: {}, //好友信息
      //底部隐藏九宫格
      open: false, //右下角加号 隐藏显示
      bqDIV: false, //表情 隐藏显示
      //倍率
      power: 5,
      textareaVal: "",
      loading: true,
      finished: false,
      isLoading: false,
      isMounted: false,
      pDianBalance: 0,
      receiveRate: 1,
      vDianBalance: 0,
      senderRate: 1,
      retry: 0
    };
  },
  mounted() {
    /* 兼容 android*/
    const docmHeight = document.body.clientHeight; // 默认屏幕高度
    window.onresize = () => {
      // this.$toast("触发window.onresize");
      if (this.$refs) {
        var nowHeight = document.body.clientHeight; // 实时屏幕高度

        let top = this.$refs.chatsCont.offsetTop; //距离顶部距离

        let footIpt = this.$refs.footIpt.offsetHeight; //底部元素高度

        this.$refs.chatsCont.style.height = nowHeight - top - footIpt + "px";
        setTimeout(() => {
          this.$refs.chatsCont.scrollTop = this.$refs.List.offsetHeight;
        }, 100);
      }
    };

    this.isMounted = true;
    this.userInfo = JSON.parse(getStore("UserConfigInfo")).data;
    //好友信息
    let userId = this.$route.query.userId;
    getMyFriendByUserId(
      { ...JSON.parse(sessionStorage.userParam), userId: userId },
      res => {
        if (res) {
          this.friendInfo = res;
        }
      }
    );
    let _this = this;
    //开启会话
    _this.startConversation(userId);

    if (ImService.ConnectionStatus == 0) {
      _this.ReceiveMessage();
      //获取未读消息数
      _this.getUnreadCount();
      //获取单群聊历史消息
      _this.getHistoryMessages(10, 0);

      _this.getConversationInfo();
    }
    //绑定表情
    let emojis = _this.getEmojiDetailList();
    let emojiBox = document.getElementById("emoji-box");
    emojis.forEach(ele => {
      let emojiHtml = ele;
      emojiBox.append(emojiHtml);
      emojiHtml.addEventListener("click", _this.clickEmoji, { passive: false });
    });
  },
  watch: {
    chatMsgList(newVal, formVal) {
      this.$nextTick(() => {
        if (newVal.length <= 10) {
          this.scroll();
        }
      });
    },
    bqDIV(val) {
      val && this.SetChatsContScrollTop();
    },
    open(val) {
      val && this.SetChatsContScrollTop();
    }
  },
  computed: {
    chatsContHeight() {
      if (!this.isMounted) return;
      let bodyH, top, footer;
      if (this.bqDIV || this.open) {
        footer = this.$refs.footer.offsetHeight;
      } else {
        footer = this.$refs.footIpt.offsetHeight;
      }
      bodyH = document.body.clientHeight; //body高度
      if (this.$refs) {
        top = this.$refs.chatsCont.offsetTop; //距离顶部距离
      }
      return bodyH - top - footer;
    }
  },
  methods: {
    //按住说话
    touchstart() {
      this.$refs.Speak.classList.add("touchstart");
      this.$refs.SpeakImg.style.display = "block";
      console.log(">>>>>点击发送>>>>>>");
    },
    //按住移动
    touchmove(e) {
      console.log(e);
    },
    //松开发送
    touchend() {
      this.$refs.Speak.classList.remove("touchstart");
      this.$refs.SpeakImg.style.display = "none";
      console.log(">>>松开发送");
    },
    SetChatsContScrollTop() {
      //
      setTimeout(() => {
        if (this.$refs) {
          this.$refs.chatsCont.scrollTop = this.$refs.List.offsetHeight;
        }
        /*transition 0.1s */
      }, 200);
    },
    onRefresh() {
      setTimeout(() => {
        this.isLoading = false;
        this.getHistoryMessages(10, null);
      }, 500);
    },
    //右下角div切换
    SwipeDiv() {
      this.bqDIV = false;
      this.open = !this.open;

      this.scroll(); //切换到底部
    },
    //切换表情div
    SwipeBq() {
      this.open = false;
      this.bqDIV = !this.bqDIV;
      if (this.bqDIV) {
        this.isSpeak = false;
        // document.getElementById('chat-box').focus();
      }
      this.scroll(); //切换到底部
    },
    sendMsg() {
      if (
        ImService.ConnectionStatus != 0 ||
        getSignalrConnectionStatus() != 1
      ) {
        this.$notify({
          message: "服务器断开连接，发送消息失败,请稍后再试",
          color: "#333333",
          background: "#fff",
          duration: "3000"
        });
        return;
      }
      let chatBox = document.getElementById("chat-box");
      let reg = /^\s*$/g;
      if (reg.test(chatBox.textContent)) {
        chatBox.focus();
        return;
      }
      if (this.vDianBalance <= 0) {
        this.$toast("V点不足,请充值后再发送");
        return;
      }
      let data = {
        command_id: 0x00000004,
        sequence_id: GetSequence(),
        feeType: 1,
        BusinessType: 1,
        Num: chatBox.textContent.length,
        Rate: this.senderRate,
        ReceiveType: 1,
        Receiver: this.friendInfo.nodeid,
        feeTime: getDate()
      };
      executeServerMethod(data);
    },
    ReceiveMessage() {
      let _this = this;
      _this.$eventBus.$on("TextMessage", function(message) {
        console.log("TextMessage==>" + JSON.stringify(message));
        if (message && message.targetId == _this.friendInfo.nodeid) {
          let toEmojiMsg = RongIMLib.RongIMEmoji.symbolToEmoji(
            message.content.content
          );
          _this.chatMsgList.push({ type: 2, message: toEmojiMsg });
          _this.clearUnreadCount();
          if (_this.$refs && _this.$refs.List) {
            _this.$nextTick(() => {
              _this.$refs.chatsCont.scrollTop = _this.$refs.List.offsetHeight;
            });
          }
        }
      });
    },
    onTextareaFocus(e) {
      if (e.target.innerHTML) {
        this.$refs.isMsg.style.display = "block";
        this.$refs.isShow.style.display = "none";
      }
      this.bqDIV = false;
      this.open = false;
      window.scrollTo(0, 0);
    },
    onTextareaBlur() {
      this.$refs.isMsg.style.display = "none";
      this.$refs.isShow.style.display = "block";
      window.scrollTo(0, 0);
    },
    //键盘keycode=13
    onTextareaInput(e) {
      setTimeout(() => {
        if (e.target.innerHTML) {
          this.$refs.isMsg.style.display = "block";
          this.$refs.isShow.style.display = "none";
        } 
        //ios 空内容会出现"<br>"
        if(e.target.innerHTML == '<br>' || e.target.innerHTML == '') {
          this.$refs.isMsg.style.display = "none";
          this.$refs.isShow.style.display = "block";
        }
      }, 300);
    },
    //页面加载聊天窗口滚动到底部
    scroll() {
      if (this.$refs) {
        this.$refs.chatsCont.scrollTop = this.$refs.List.offsetHeight;
      }
    },
    //底部input获取焦点
    iptfocus() {
      this.open = false; //右下角加号 隐藏显示
      this.bqDIV = false; //表情 隐藏显示
    },
    //底部input失去焦点
    iptblur() {
      window.scrollTo(0, 0);
    },
    startConversation(id) {
      //开始会话
      this.openConversation(id);
    },
    openConversation(id) {
      let RongIMEmoji = RongIMLib.RongIMEmoji;
      RongIMEmoji.init();
      this.RongIMEmoji = RongIMEmoji;

      var RongIMVoice = RongIMLib.RongIMVoice;
      RongIMVoice.init();
      this.RongIMVoice = RongIMVoice;

      this.conversation.id = id;
      this.conversation.conversationType = RongIMLib.ConversationType.PRIVATE;
    },
    getConversationInfo() {
      if (getSignalrConnectionStatus() == 1) {
        let _this = this;
        //获取聊天倍率
        let data = {
          command_id: 0x00000007,
          sequence_id: GetSequence(),
          type: 1,
          Sender: _this.$route.query.userId,
          Receiver: _this.userInfo.nodeid
        };
        executeServerMethod(data);
        //倍率查询答复
        this.$eventBus.$on("chatFeeRateQueryResp", function(messageResp) {
          if (messageResp) {
            _this.pDianBalance = messageResp.PDianBalance;
            _this.vDianBalance = messageResp.VDianBalance;
            _this.senderRate = messageResp.SenderRate;
            _this.receiveRate = messageResp.ReceiverRate;
          }
        });
        //聊天计费答复
        _this.$eventBus.$on("ChatFeeResp", function(messageResp) {
          console.log("ChatFeeResp");
          if (messageResp.Status) {
            _this.pDianBalance = messageResp.PDianBalance;
            _this.vDianBalance = messageResp.VDianBalance;

            let chatBox = document.getElementById("chat-box");
            let reg = /^\s*$/g;
            if (reg.test(chatBox.textContent)) {
              chatBox.focus();
              return;
            }
            let message = new RongIMLib.TextMessage({
              content: chatBox.textContent,
              extra: "附加信息"
            });
            _this.sendMessage(message);
            window.scrollTo(0, 0);
          } else {
            this.$toast(messageResp.StatusDesc);
          }
        });
        //聊天计费推送
        _this.$eventBus.$on("ChatFeePush", function(messageResp) {
          console.log("ChatFeePush");
          _this.pDianBalance = messageResp.PDianBalance;
        });
        //聊天计费倍率设置推送
        _this.$eventBus.$on("ChatFeeRateSetPush", function(messageResp) {
          console.log("ChatFeeRateSetPush");
          console.log(messageResp);
          _this.senderRate = messageResp.Rate;
        });
      }
    },
    //获取全部表情
    getEmojiDetailList() {
      var shadowDomList = [];
      for (var i = 0; i < this.RongIMEmoji.list.length; i++) {
        var value = this.RongIMEmoji.list[i];
        shadowDomList.push(value.node);
      }
      return shadowDomList;
    },
    //click表情事件
    clickEmoji(event) {
      let e = event || window.event;
      let target = e.target || e.srcElement;
      if (document.all && !document.addEventListener === false) {
      }
      let chatBox = document.getElementById("chat-box");
      chatBox.textContent =
        chatBox.textContent +
        RongIMLib.RongIMEmoji.symbolToEmoji(target.getAttribute("name"));
      //发送按钮隐藏显示
      if (chatBox.innerHTML) {
        this.$refs.isMsg.style.display = "block";
        this.$refs.isShow.style.display = "none";
      }
    },
    //发送消息
    sendMessage(message) {
      let _this = this;
      let toEmojiMsg = RongIMLib.RongIMEmoji.symbolToEmoji(message.content);
      console.log("this.conversation.id" + this.conversation.id);
      RongIMClient.getInstance().sendMessage(
        this.conversation.conversationType,
        this.conversation.id,
        message,
        {
          onSuccess: function(message) {
            let showMsg;
            if ("TextMessage" == message.messageType) {
              showMsg = toEmojiMsg;
            }
            ImService.updateConversationCache(false, "");
            // else if ("VoiceMessage" == message.messageType) {
            //   showMsg =
            //     "<button class='voiceInfo' data-message=" +
            //     toEmojiMsg +
            //     " onclick='PXin.playVoice(this)'>播放测试语音 base64 </button>";
            // } else if ("ImageMessage" == message.messageType) {
            //   showMsg =
            //     "<image style='width:120px;height:60px' src=" +
            //     message.content.imageUri +
            //     " />";
            // }
            // message 为发送的消息对象并且包含服务器返回的消息唯一 Id 和发送消息时间戳
            _this.chatMsgList.push({ type: 1, message: showMsg });
            _this.$nextTick(() => {
              if (this.$refs) {
                this.$refs.chatsCont.scrollTop = this.$refs.List.offsetHeight;
              }
            });
            let chatBox = document.getElementById("chat-box");
            chatBox.textContent = "";
            chatBox.focus();
          },
          onError: function(errorCode, message) {
            var info = "";
            switch (errorCode) {
              case RongIMLib.ErrorCode.TIMEOUT:
                info = "超时";
                break;
              case RongIMLib.ErrorCode.UNKNOWN:
                info = "未知错误";
                break;
              case RongIMLib.ErrorCode.REJECTED_BY_BLACKLIST:
                info = "在黑名单中，无法向对方发送消息";
                break;
              case RongIMLib.ErrorCode.NOT_IN_DISCUSSION:
                info = "不在讨论组中";
                break;
              case RongIMLib.ErrorCode.NOT_IN_GROUP:
                info = "不在群组中";
                break;
              case RongIMLib.ErrorCode.NOT_IN_CHATROOM:
                info = "不在聊天室中";
                break;
            }
          }
        }
      );
    },
    //获取单群聊历史消息
    getHistoryMessages(count, ts) {
      if (ImService.ConnectionStatus != 0) {
        console.log("连接已断开");
        return;
      }
      // 每次获取的历史消息条数，范围 0-20 条，可以多次获取
      let _this = this;
      let timestrap = ts; // 默认传 null，若从头开始获取历史消息，请赋值为 0, timestrap = 0;
      let conversationType = _this.conversation.conversationType;
      let targetId = _this.conversation.id.toString();
      RongIMClient.getInstance().getHistoryMessages(
        conversationType,
        targetId,
        timestrap,
        count,
        {
          onSuccess: function(list, hasMsg) {
            console.log("聊天历史==>" + JSON.stringify(list));
            let msgArray = [];
            list.forEach(element => {
              let toEmojiMsg = RongIMLib.RongIMEmoji.symbolToEmoji(
                element.content.content
              );
              console.log(element.sentTime);
              msgArray.push({
                type: element.messageDirection,
                message: toEmojiMsg
              });
            });
            _this.chatMsgList = msgArray.concat(_this.chatMsgList);
          },
          onError: function(error) {
            console.log("GetHistoryMessages, errorcode:" + error);
          }
        }
      );
    },
    //获取指定会话的未读消息数
    getUnreadCount() {
      let _this = this;
      let conversationType = this.conversation.conversationType;
      let targetId = this.conversation.id;
      RongIMLib.RongIMClient.getInstance().getUnreadCount(
        conversationType,
        targetId,
        {
          onSuccess: function(count) {
            // count => 指定会话的总未读数
            console.log("指定会话的未读消息数==>" + count);
            if (count > 0) {
              _this.clearUnreadCount();
            }
            // _this.conversation.unreadCount = count;
          },
          onError: function() {
            // error => 获取指定会话未读数错误码
          }
        }
      );
    },
    //清除未读消息数
    clearUnreadCount() {
      let conversationType = this.conversation.conversationType;
      let targetId = this.conversation.id;
      RongIMClient.getInstance().clearUnreadCount(conversationType, targetId, {
        onSuccess: function() {
          // 更新会话
          ImService.updateConversationCache(false, "");
        },
        onError: function(error) {
          // error => 清除未读消息数错误码
        }
      });
    }
  },
  destroyed() {
    this.$eventBus.$off("ChatFeeResp");
    this.$eventBus.$off("chatFeeRateQueryResp");
    this.$eventBus.$off("TextMessage");
    this.$eventBus.$off("ChatFeePush");
    this.$eventBus.$off("ChatFeeRateSetPush");
  }
};
</script>

<style scoped lang='scss'>
.SpeakImg {
  width: 2rem;
  height: 2rem;
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  margin: auto;
  z-index: 1000;
  display: none;
  img {
    max-width: 100%;
    max-height: 100%;
  }
}
.Speak {
  width: 4.8rem;
  height: 0.7rem;
  margin-left: 0.2rem;
  line-height: 0.7rem;
  text-align: center;
  max-height: 1.6rem;
  overflow-y: scroll;
  box-sizing: border-box;
  background-color: #f2f2f2;
  border-radius: 0.04rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;
}
.touchstart {
  color: #000;
  // font-weight: bold;
  background: #c5c5c5;
}
.isMsg {
  // margin-left: 0.1rem;
  display: none;
  align-items: center;
  white-space: nowrap;
  position: relative;
  flex: 1;
  button {
    position: absolute;
    width: 100%;
    height: 100%;
    height: 0.46rem;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    margin: auto;
    border: 0;
    font-size: 0.28rem;
    color: #fff;
    background-color: #2ea2fa;
    border-radius: 0.06rem;
    display: flex;
    justify-content: center;
    align-items: center;
  }
}
.List::-webkit-scrollbar {
  display: none;
}
/deep/ .van-pull-refresh__track {
  height: 100%;
}

.animationTop {
  bottom: 0rem !important;
  // transition: all 1s linear;
  -webkit-transition: bottom 0.1s linear;
}
.animationBot {
  bottom: -3.3rem !important;
  // transition: all 1s linear;
  -webkit-transition: bottom 0.1s linear;
}
.ListTop {
  height: 3.3rem !important;
  -webkit-transition: all 0.1s linear;
}
.ListBot {
  height: 0 !important;
  -webkit-transition: all 0.1s linear;
}

.botContent {
  height: 3.3rem;
}

.chats {
  height: 100%;
}
.List > div:last-child {
  padding-bottom: 0.18rem;
}

.use:nth-of-type(n + 2) {
  margin-top: 0.18rem;
}

.scrollWrapper {
  position: absolute;
  /* 绝对定位，进行内部滚动 */
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  overflow-y: atuo;
  /* 或者scroll */
  -webkit-overflow-scrolling: touch;
  /* 解决ios滑动不流畅问题 */
}
.bqDIV {
  width: 100%;
  height: 3.3rem;
  padding: 0 0.3rem;
  overflow: scroll;
  font-size: 0.72rem;
  // display: flex;
  // flex-direction: column;
  // justify-content: space-between;
  box-sizing: border-box;
  /deep/ span {
    margin: 0 0.1rem;
  }
}
.openDIV {
  width: 100%;
  height: 3.3rem;
  padding: 0.3rem 0.6rem;
  box-sizing: border-box;
  display: flex;
  flex-wrap: wrap;

  & > div {
    width: 25%;

    img {
      width: 0.9rem;
      height: 0.9rem;
      border-radius: 0.04rem;
    }

    p {
      text-align: center;
      margin: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }
  }

  & > div:nth-of-type(n + 5) {
    margin-top: 0.2rem;
  }
}

.footer {
  position: fixed;
  width: 100%;
  bottom: -3.3rem;
  z-index: 1000;
  background: #fff;
  box-sizing: border-box;
}

@supports (bottom: env(safe-area-inset-bottom)) {
  .footer {
    padding-bottom: env(safe-area-inset-bottom);
  }
}

.footIpt {
  width: 100%;
  min-height: 1.08rem;
  display: flex;
  padding-left: 0.2rem;
  padding-right: 0.2rem;
  align-items: center;
  // justify-content: space-between;
  box-sizing: border-box;
  border-top: solid 0.01rem #f2f2f2;
  #chat-box::-webkit-scrollbar {
    display: none;
  }
  #chat-box {
    outline: none;
    padding: 0.2rem;
    border: 0;
    width: 4.8rem;
    height: auto;
    max-height: 1.6rem;
    overflow-y: scroll;
    box-sizing: border-box;
    background-color: #f2f2f2;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
    -webkit-user-select: text;
    padding-left: 0.2rem;
    box-sizing: border-box;
    margin-left: 0.2rem;
  }
  #chat-box + img {
    margin: 0 0.2rem !important;
  }

  img {
    width: 0.46rem;
    height: 0.46rem;
  }
}

.my .useText {
  background: #66ccff;
  margin-right: 0.2rem;
}

.my {
  padding-top: 0.18rem;
  display: flex;
  justify-content: flex-end;
}

.use .useText {
  margin-left: 0.23rem;
  border: solid 0.02rem #e5e5e5;
}

.useText {
  word-wrap: break-word;
  word-break: break-all;
  margin-left: 0.23rem;
  word-wrap: break-word;
  word-break: break-all;
  max-width: 4.3rem;
  box-sizing: border-box;
  padding: 0.24rem 0.18rem;
  background-color: #fff;
  border-radius: 0rem 0.2rem 0rem 0.2rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}

.chatsCont {
  padding: 0 0.27rem;
  /* height: 1.2rem; */
  width: 100%;
  box-sizing: border-box;
  /* padding-bottom: .5rem; */
  overflow-y: scroll;
  -webkit-overflow-scrolling: touch;
  -webkit-transition: height 0.1s linear;
}
.chatsCont::-webkit-scrollbar {
  display: none;
}

@supports (bottom: env(safe-area-inset-bottom)) {
  .chatsCont {
    padding-bottom: env(safe-area-inset-bottom);
  }
}

.useImg {
  width: 1rem;
  height: 1rem;
  border-radius: 50%;

  img {
    width: 100%;
    height: 100%;
    border-radius: 50%;
  }
}

.use {
  display: flex;
}

.time {
  margin: 0;
  text-align: center;
  height: 0.75rem;
  line-height: 0.75rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
}

.PowInfoRig {
  justify-content: flex-start !important;

  .pow {
    padding-left: 0.16rem;
  }
}

.pow {
  height: 80%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  p {
    width: 2.2rem;
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
  }

  p:last-child {
    font-size: 0.2rem !important;
  }
}

.chatsPow {
  height: 0.88rem;
  background-color: #2ea2fa;
  border-radius: 0rem 0rem 0.2rem 0.2rem;
  padding: 0 0.3rem;
  display: flex;
  align-items: center;
  justify-content: space-between;

  .PowInfo {
    display: flex;
    justify-content: space-between;
    width: 3.3rem;
    height: 0.6rem;
    align-items: center;
    background-color: rgba(255, 255, 255, 0.3);
    border-radius: 0.06rem;
    /* opacity: 0.7; */
    padding: 0 0.1rem;
    box-sizing: border-box;

    p {
      margin: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      height: 0.24rem;
      line-height: 0.24rem;
    }

    img:nth-child(1) {
      width: 0.36rem;
      height: 0.36rem;
    }

    .dynamic {
      width: 0.36rem;
      height: 0.36rem;
      display: flex;

      img {
        width: 100%;
        height: 100%;
      }
    }

    .arrow {
      width: 0.24rem;
      height: 0.24rem;
      display: flex;

      img {
        width: 100%;
        height: 100%;
      }
    }
  }
}

.chatsTit {
  height: 0.88rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #2ea2fa;
  padding: 0 0.44rem;
  box-sizing: border-box;

  span {
    width: 3rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #fefefe;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    text-align: center;
  }

  img {
    height: 0.12rem;
    width: 0.56rem;
  }
  i {
    width: 0.56rem;
  }
}
</style>
