<!-- 商友圈 -->
<template>
  <div class="friends">
    <!-- :style=" backgroundImage:'url('+require('@/assets/images/chat_chat_camera@2x.png')+')'" -->
    <!-- 背景 -->
    <div class="abs">
      <div class="header">
        <div class="top" :style="isBg && 'backgroundImage:url('+isBg+');background-color:#fff'">
          <div v-show="isBg" class="shadeUp">
            <img src="@/assets/images/believe_xinyou_top_bg_up.png" alt />
          </div>
          <div v-show="isBg" class="shadeDown">
            <img src="@/assets/images/believe_xinyou_top_bg_down.png" alt />
          </div>
          <div class="titIMG">
            <img @click="$router.push('/Publishs')" src="@/assets/images/dynamic_publish.png" alt />
          </div>
          <div class="setBG">
            <van-uploader :after-read="afterRead">{{$t('m.changeback')}}</van-uploader>
          </div>
          <!-- 头像 名称 点赞 -->
          <div class="titImg">
            <img src="@/assets/images/login_bg@2x.png" alt />
            <span>木木日月</span>
          </div>
        </div>
        <!-- 头像 名称 点赞 -->
        <div class="titBott">
          <div>
            <div>
              <img src="@/assets/images/dynamic_zan_icon@2x.png" alt />
              <span>100</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_tan_icon@2x.png" alt />
              <span>100</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_v_sel@2x.png" alt />
              <span>100</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_p_sel@2x.png" alt />
              <span>100</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 占高 -->
    <!-- <div style='height: 4rem;'></div> -->
    <!-- 内容 -->
    <div class="content">
      <div class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div>
            <!-- 日 -->
            <p class="day">18</p>
            <p class="month">08月</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">{{IssuedText}}</div>
          <!-- 点赞，踩 其他 -->
          <div class="IssuedUp">
            <div>
              <div>
                <img src="@/assets/images/dynamic_zan_nor@2x.png" alt />
                <span>100</span>
              </div>
              <div>
                <img src="@/assets/images/dynamic_tan_nor@2x.png" alt />
                <span>100</span>
              </div>
            </div>
            <div>
              <!-- 评论 打赏 举报 -->
              <div class="comment" v-if="select">
                <span @click="comment">评论</span>
                <i></i>
                <span @click="show=true">打赏</span>
                <i></i>
                <router-link to="/Report" tag="span">举报</router-link>
              </div>
              <img @click="select=!select" src="@/assets/images/dynamic_more@2x.png" alt />
            </div>
          </div>
        </div>
      </div>
      <!-- type2 -->
      <div class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div>
            <!-- 日 -->
            <p class="day">18</p>
            <p class="month">08月</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">{{IssuedText}}</div>
          <!-- 点赞，踩 其他 -->
          <div class="IssuedUp">
            <div class="pay">
              <!-- 只需支付1个V点，可查看全文 -->
              <div>
                <img src="@/assets/images/dynamic_lock@2x.png" alt />只需支付1个V点，可查看全文
              </div>
              <div @click="isKeyboard=true;payTypeImg=false">
                <button>支付</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="year">2018</div>
      <!-- type3 -->
      <div class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div>
            <!-- 日 -->
            <p class="day">18</p>
            <p class="month">08月</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">{{IssuedText}}</div>
          <!-- 点赞，踩 其他 -->
          <div class="IssuedUp">
            <div>
              <div>
                <img src="@/assets/images/dynamic_zan_nor@2x.png" alt />
                <span>100</span>
              </div>
              <div>
                <img src="@/assets/images/dynamic_tan_nor@2x.png" alt />
                <span>100</span>
              </div>
            </div>
            <div>
              <!-- 评论 打赏 举报 -->
              <div class="comment">
                <span>评论</span>
                <i></i>
                <span>打赏</span>
                <i></i>
                <span>举报</span>
              </div>
              <img src="@/assets/images/dynamic_more@2x.png" alt />
            </div>
          </div>

          <!-- 评论列表 -->
          <div class="commentList">
            <div>
              <span class="color">林明 :</span>
              这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论
            </div>
            <div>
              <span class="color">李总</span>
              回复
              <span class="color">林明 :</span>
              这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论这里是评论
            </div>
          </div>
        </div>
      </div>
      <!-- type4 -->
      <div class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div>
            <!-- 日 -->
            <p class="day">18</p>
            <p class="month">08月</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">图片一排最多显示三张，一条商友圈最多可以发九章图片。图片大小如下所示</div>
          <div class="Issuedimg">
            <!-- <div v-for="(item,index) of 9" :key="index">
              <img src="@/assets/images/chat_chat_redpacket@2x.png" alt />
            </div>-->
            <video-player
              class="video-player vjs-custom-skin"
              ref="videoPlayer"
              :options="playerOptions"
            ></video-player>
          </div>
          <!-- 点赞，踩 其他 -->
          <div class="IssuedUp">
            <div>
              <div>
                <img src="@/assets/images/dynamic_zan_nor@2x.png" alt />
                <span>100</span>
              </div>
              <div>
                <img src="@/assets/images/dynamic_tan_nor@2x.png" alt />
                <span>100</span>
              </div>
            </div>
            <div>
              <!-- 评论 打赏 举报 -->
              <div class="comment">
                <span>评论</span>
                <i></i>
                <span>打赏</span>
                <i></i>
                <span>举报</span>
              </div>
              <img src="@/assets/images/dynamic_more@2x.png" alt />
            </div>
          </div>
        </div>
      </div>
      <!-- type5 -->
      <div class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div>
            <!-- 日 -->
            <p class="day">18</p>
            <p class="month">08月</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">图片一排最多显示三张，一条商友圈最多可以发九章图片。图片大小如下所示</div>
          <div class="Issuedimg">
            <div v-for="(item,index) of 9" :key="index">
              <img src="@/assets/images/chat_chat_redpacket@2x.png" alt />
            </div>
          </div>
          <!-- 点赞，踩 其他 -->
          <div class="IssuedUp">
            <div>
              <div>
                <img src="@/assets/images/dynamic_zan_nor@2x.png" alt />
                <span>100</span>
              </div>
              <div>
                <img src="@/assets/images/dynamic_tan_nor@2x.png" alt />
                <span>100</span>
              </div>
            </div>
            <div>
              <!-- 评论 打赏 举报 -->
              <div class="comment">
                <span>评论</span>
                <i></i>
                <span>打赏</span>
                <i></i>
                <span>举报</span>
              </div>
              <img src="@/assets/images/dynamic_more@2x.png" alt />
            </div>
          </div>
        </div>
      </div>

      <div class="empty">到底了~</div>
    </div>
    <!-- 打赏 -->
    <van-popup v-model="show">
      <div class="rewards">
        <div class="rewardstop">
          <span style="width: .22rem;"></span>
          <span>选择打赏金额</span>
          <img @click="show=false" src="@/assets/images/dynamic_delete@2x.png" alt />
        </div>

        <div class="rewardsMiddle">
          <div v-for="(item,index) of type" :key="index" @click="ischecked(item,index)">
            <span>{{item.count}}</span>
            <img v-show="item.ischecked" src="@/assets/images/dynamic_choices@2x.png" alt />
          </div>

          <div>
            <input @click="clear" ref="ipt" placeholder="请输入其他金额" type="text" />
            <span style="color: #2ea2fa;">P</span>
          </div>
        </div>

        <div class="rewardsBtn">
          <button @click="isKeyboard=true;show=false;payTypeImg=true">确认</button>
        </div>
      </div>
    </van-popup>

    <!-- 支付 -->
    <!-- <div class="password" v-if="isKeyboard">
      <div class="passwordList" ref="passwordList">
        <p>请输入您的支付密码</p>
        <div>
          <div v-for="(item,index) of passwordKey" :key="index">{{item.title}}</div>
        </div>
      </div>
      <van-number-keyboard
        :show="isKeyboard"
        extra-key="关闭"
        @blur="show = false"
        @input="onInput"
        @delete="onDelete"
      /> 
    </div>-->
    <!-- 评论 -->
    <div class="commentIpt" v-show="btIpt">
      <input placeholder="评论" type="text" ref="comment" @keydown="onTextareaKeyDown" />
    </div>
    <!-- 支付密码框 -->
    <keyboard
      :isKeyboard="isKeyboard"
      theme="money"
      :pay-price="200"
      :balance-icon="payTypeImg?'http://global.ckv-test.sulink.cn/images/web/beCasPayPwdP.png':'http://global.ckv-test.sulink.cn/images/web/beCasPayPwdV.png'"
      balance-name="UV"
      :balance="2000"
      @close="closeKeyboard"
      @pay="sendPayAjax"
    ></keyboard>
  </div>
</template>

<script>
import { videoPlayer } from "vue-video-player";
import "video.js/dist/video-js.css";
export default {
  data() {
    return {
      //支付类型 P?V
      payTypeImg: "",
      //支付框
      isKeyboard: false,
      //底部评论
      btIpt: false,
      select: false, // 评论 打赏 举报
      isBg: "", //背景图
      show: false, //打赏模态框

      IssuedText:
        "这里是用户发布的内容，字数最多显示约100个字符，超出的折叠隐藏。这里是用户发布的内容，字数最多显示约100。这里是用户发布的内容，字数最多显示约100个字符，超出的折叠隐藏。这里是用户发布的内容，字数最多显示约100。这里是用户发布的内容，字数最多显示约100个字符，超出的折叠隐藏。这里是用户发布的内容，字数最多显示约100。",
      //打赏选项
      type: [
        {
          count: "1P",
          ischecked: false
        },
        {
          count: "5P",
          ischecked: false
        },
        {
          count: "10P",
          ischecked: false
        }
      ],
      playerOptions: {
        playbackRates: [0.7, 1.0, 1.5, 2.0], //播放速度
        autoplay: false, //如果true,浏览器准备好时开始回放。
        controls: false,
        muted: false, // 默认情况下将会消除任何音频。
        loop: false, // 导致视频一结束就重新开始。
        preload: "auto", // 建议浏览器在<video>加载元素后是否应该开始下载视频数据。auto浏览器选择最佳行为,立即开始加载视频（如果浏览器支持）
        language: "zh-CN",
        aspectRatio: "16:9", // 将播放器置于流畅模式，并在计算播放器的动态大小时使用该值。值应该代表一个比例 - 用冒号分隔的两个数字（例如"16:9"或"4:3"）
        fluid: true, // 当true时，Video.js player将拥有流体大小。
        sources: [
          {
            src: require("@/assets/mp4/20190826150418.mp4"), // 路径
            type: "video/mp4" // 类型
          }
        ],
        poster: require("@/assets/images/waller_dos@2x.png"), //你的封面地址
        // width: document.documentElement.clientWidth,
        notSupportedMessage: "此视频暂无法播放，请稍后再试" //允许覆盖Video.js无法播放媒体源时显示的默认信息。
      }
    };
  },
  mounted() {
    //绑定滚动事件,
    document
      .getElementsByClassName("friends")[0]
      .addEventListener("scroll", this.scroll);
  },

  components: {
    videoPlayer
  },

  methods: {
    autoPlay() {
      console.log(333);
    },

    //键盘keycode=13
    onTextareaKeyDown() {
      if (event.keyCode == 13) {
        alert("发送评论");
      }
    },
    //页面滚动事件
    scroll() {
      this.btIpt = false;
      this.$refs.comment.blur();
    },

    //更换背景图片
    afterRead(file) {
      this.isBg = file.content;
      console.log(file);
    },

    //打开支付密码框
    closeKeyboard() {
      this.isKeyboard = false;
    },
    //输入完6位密码 Ajax
    async sendPayAjax(password) {
      //let res = await xxx()
      console.log(password);
    },

    //打赏 选择P币
    ischecked(item, index) {
      for (let item of this.type) {
        item.ischecked = false;
      }
      item.ischecked = true;
    },
    //打赏 选择其他金额
    clear() {
      for (let item of this.type) {
        item.ischecked = false;
      }
    },
    //点击评论
    comment() {
      this.btIpt = true;
    }
  },
  updated() {
    //底部评论框出现 自动获取焦点
    if (this.btIpt) {
      this.$refs.comment.focus();
    }
  }
};
</script>
<style scoped lang='scss'>
.video-player {
  width: 2.41rem !important;
  width: 100% !important;
  height: 3.88rem !important;
}
/deep/ .video-js {
  height: 100% !important;
  padding: 0;
  background-color: rgba(255, 255, 255, 1) !important;
}
/deep/ .vjs-poster {
  background-color: rgba(255, 255, 255, 1) !important;
  background-size: 100% 100%;
}
/deep/ .vjs-big-play-button {
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  margin: auto;
  width: 1rem;
  height: 1rem;
  background: url("./../../assets/images/dynamic_pay@2x.png") no-repeat;
  background-size: 100% 100%;
  border: 0 !important;
  z-index: 100;
  background-color: transparent !important ;
}
// /deep/ .vjs-paused,/deep/ .vjs-tech{
//   // width: auto;
//   // max-width: 100%;
//   // height: auto;
//   // max-height: 100%;
//   // width: 100%;
//   // height: 100%;
// }
/deep/ .vjs-volume-panel {
  display: none;
}
/deep/ .vjs-control-bar {
  display: flex;
  // display: none;
  align-items: center;
  justify-content: space-between;
}
.year {
  padding: 0 0.3rem;
  padding-top: 0.7rem;
  font-family: PingFang-SC-Bold;
  font-size: 0.48rem;
  line-height: 0.48rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}
.shadeUp,
.shadeDown {
  height: 1rem;
  width: 100%;
  position: absolute;

  img {
    width: 100%;
    height: 100%;
  }
}
.shadeUp {
  top: 0;
}
.shadeDown {
  bottom: 0;
}
.day {
  font-family: PingFang-SC-Bold;
  font-size: 0.48rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  height: 0.48rem;
  line-height: 0.48rem;
  display: flex;
  align-items: flex-start;
}
.month {
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem !important;
  height: 0.28rem;
  line-height: 0.28rem;
  padding-top: 0.15rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}
.usertype1 {
  padding: 0.3rem;
  padding-bottom: 0;
  display: flex;
}
.commentIpt {
  width: 100%;
  box-sizing: border-box;
  position: fixed;
  bottom: 0;
  height: 1rem;
  padding: 0 0.3rem;
  display: flex;
  background: #fff;
  input {
    border: 0;
    width: 100%;
    height: 100%;
    font-family: PingFang-SC-Regular;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666;
  }
}
/deep/ [class*="van-hairline"]::after {
  border: none !important;
}
/deep/ .van-key {
  font-size: 14px !important;
}

.passwordList {
  position: fixed;
  left: 0;
  width: 100%;
  height: 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: #fff;

  & > div {
    /* margin-top: .1rem; */
    width: 4.8rem;
    height: 0.8rem;
    box-sizing: border-box;
    display: flex;

    & > div {
      background: #f1f1f1;
      display: flex;
      justify-content: center;
      align-items: center;
      width: 0.8rem;
      border: 0.01rem solid #bbb;
      box-sizing: border-box;
      font-size: 0.28rem;
    }

    & > div:nth-of-type(n + 2) {
      border-left: 0;
    }
  }

  p {
    padding-bottom: 0.1rem;
    margin: 0;
    font-family: MicrosoftYaHei;
    font-size: 0.21rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.password {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.3);
}

.rewardsBtn {
  display: flex;
  height: 1.3rem;
  align-items: center;
  box-sizing: border-box;
  padding: 0 0.8rem;

  button {
    border: 0;
    width: 100%;
    height: 0.61rem;
    background-color: #2ea2fa;
    border-radius: 0.05rem;
    font-family: MicrosoftYaHei;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}

.rewardsMiddle {
  & > div {
    border-bottom: 0.01rem solid #ddd;
    display: flex;
    padding-left: 0.45rem;
    padding-right: 0.55rem;
    height: 1rem;
    align-items: center;
    justify-content: space-between;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #1a1a1a;

    img {
      width: 0.39rem;
      height: 0.28rem;
    }

    input {
      padding: 0;
      border: 0;
      width: 80%;
      height: 90%;
    }
  }
}

.rewardstop {
  height: 0.84rem;
  align-items: center;
  display: flex;
  justify-content: space-between;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
  padding: 0 0.32rem;
  border-bottom: 0.01rem solid #dddddd;

  img {
    width: 0.22rem;
    height: 0.22rem;
  }
}

.van-popup {
  border-radius: 0.1rem;
}

.rewards {
  width: 5.8rem;
  height: 6.24rem;
  background-color: #ffffff;
}

.empty {
  height: 2rem;
  line-height: 2rem;
  text-align: center;
  background: #f2f2f2;
  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;

  letter-spacing: 0rem;
  color: #999999;
}

.Issuedimg {
  display: flex;
  //   padding-left: 1.15rem;
  flex-wrap: wrap;
  padding: 0.2rem 0;

  & > div {
    margin-left: 0.04rem;
    margin-top: 0.04rem;
    width: 1.8rem;
    height: 1.8rem;

    img {
      width: 100%;
      height: 100%;
    }
  }
}

.commentList {
  margin-top: 0.25rem;
  /* width: 100%; */
  background-color: #f0f0f0;
  box-sizing: border-box;
  padding: 0.3rem;
  //   margin-left: .3rem;
  //   margin-right: 0.3rem;

  & > div:nth-of-type(n + 2) {
    margin-top: 0.2rem;
  }

  & > div {
    /* display: flex; */
    /* flex-wrap: wrap; */
    .color {
      color: #00297b;
    }
  }

  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #000000;
}

.pay {
  //   padding-left: 0.3rem;
  width: 100%;
  display: flex;
  justify-content: space-between;
  height: 0.65rem;
  margin-top: 0.25rem;

  & > div {
    margin: 0 !important;
  }

  & > div:nth-child(1) {
    flex: 1;
    height: 100%;
    background-color: #f0f0f0;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.48rem;
    letter-spacing: 0rem;
    color: #999999;

    img {
      margin-left: 0.3rem;
      margin-right: 0.15rem;
    }
  }
  & > div:nth-child(2) {
    width: 1rem;
  }
  button {
    width: 100%;
    border: 0;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.65rem;
    letter-spacing: 0rem;
    color: #ffffff;
    /* width: 1.01rem; */
    height: 0.65rem;
    padding: 0;
    background-color: #2ea2fa;
  }
}

.comment {
  position: absolute;
  right: 0.65rem;
  width: 3.35rem;
  height: 0.8rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  box-sizing: border-box;
  padding: 0 0.3rem;
  background-color: #2c2c2c;
  border-radius: 0.05rem;

  span {
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }

  i {
    width: 0.02rem;
    height: 0.25rem;
    background: #000;
  }
}

.IssuedUp {
  //   border-bottom: 0.02rem solid #f9f9f9;
  height: 1rem;
  display: flex;
  //   padding-left: 0.5rem;
  justify-content: space-between;
  width: 100%;
  box-sizing: border-box;
  //   padding: 0 0.3rem;
  //   padding-left: 0.85rem;

  & > div:first-child {
    display: flex;
    align-items: center;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.48rem;
    letter-spacing: 0rem;
    color: #999999;
    padding-left: 0.1rem;

    & > div {
      // margin-left: 0.35rem;
      display: flex;
      align-items: center;
    }
    & > div:last-child img {
      margin-left: 0.35rem;
    }
    img {
      width: 0.33rem;
      height: 0.33rem;
      vertical-align: middle;
      margin-right: 0.15rem;
    }
  }

  & > div:last-child {
    display: flex;
    align-items: center;
    position: relative;

    & > img {
      width: 0.4rem;
      height: 0.4rem;
    }
  }
}

.IssuedText {
  //   padding: 0 0.3rem;
  //   padding-left: 1.15rem;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.36rem;
  letter-spacing: 0rem;
  color: #000000;
}

.usertype1Head {
  display: flex;
  //   height: 1.1rem;
  align-items: flex-start;
  //   padding: 0 0.3rem;
  & > div:first-child {
    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
    }
  }

  & > div:last-child {
    width: 0.9rem;
    display: flex;
    flex-direction: column;

    //  margin-left: 0.25rem;

    //  p:nth-child(1) {
    //    font-family: PingFang-SC-Medium;
    //    font-size: 0.3rem;
    //    font-weight: normal;
    //    font-stretch: normal;
    //    letter-spacing: 0rem;
    //    color: #2ea2fa;
    //  }

    p:nth-child(2) {
      font-family: PingFang-SC-Regular;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }
  }
}

p {
  margin: 0;
}

.content {
  background: #fff;

  margin-top: 1rem;
}

.titBott {
  box-sizing: border-box;
  padding: 0 0.3rem;
  background: #fff;
  height: 0.86rem;

  & > div {
    display: flex;
    height: 100%;
    padding-left: 1.25rem;

    & > div {
      display: flex;
      align-items: center;
      height: 100%;
      margin-left: 0.3rem;

      span {
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
        padding-left: 0.15rem;
        height: 0.3rem;
        line-height: 0.3rem;
      }

      img {
        vertical-align: middle;
        width: 0.3rem;
        height: 0.3rem;
      }
    }
  }
}

.top {
  // display: flex;
  // flex-direction: column;
  // justify-content: space-between;
  // height: 3rem;
  // background-size: 100% 100%;

  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
  padding-top: 1rem;
  box-sizing: border-box;
  background-size: 100% 100%;
  position: relative;
  // background-color: #fff;
}

.titImg {
  padding: 0 0.3rem;
  height: 0.7rem;
  line-height: 0.7rem;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #ffffff;
  position: relative;

  span {
    margin-left: 1.55rem;
  }

  img {
    position: absolute;
    box-sizing: border-box;
    width: 1.33rem;
    height: 1.33rem;
    border-radius: 50%;
    border: solid 0.05rem rgba(255, 255, 255, 0.3);
    /* opacity: 0.3; */
  }
}

.setBG {
  margin-top: 0.45rem;
  display: flex;
  justify-content: center;
  align-self: center;
}

.titIMG {
  margin-top: 0.3rem;
  width: 100%;
  box-sizing: border-box;
  display: flex;
  justify-content: flex-end;
  padding: 0 0.47rem;
  height: 0.43rem;
  display: flex;

  img {
    width: 0.5rem;
    height: 0.43rem;
  }
}

.header {
  height: 4rem;
  width: 100%;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  background-color: #2c2c2c;
}

/deep/ .van-uploader__wrapper {
  height: 100%;
}

/deep/ .van-uploader__input-wrapper {
  display: flex;
  height: 100%;
  font-family: MicrosoftYaHei;
  font-size: 0.2rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.22rem;
  letter-spacing: 0rem;
  color: rgba(254, 254, 254, 0.6);
  /* opacity: 0.6; */
}

.friends {
  height: 100%;
  overflow-y: scroll;
  -webkit-overflow-scrolling: touch;
  background-color: #f2f2f2;
}
</style>
