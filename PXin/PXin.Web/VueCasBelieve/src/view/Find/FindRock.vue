<!-- 摇一摇 -->
<template>
  <div class="Findshake">
    <div ref="shakeTop" class="shakeTop">
      <img src="@/assets/images/discover_shake_white_up@2x.png" alt />
    </div>

    <div ref="shakeBot" class="shakeBot">
      <img src="@/assets/images/discover_shake_white_down@2x.png" alt />
      <!-- <div class="hint">{{$t('m.shake')}}</div> -->
      <div @click="GoAddFriend()" v-show="isShowRockUserBox" class="search">
        <img :src="rockUser.photo" alt />
        <div class="title">
          <p>{{rockUser.nickname}}</p>
          <p class="dist">相距{{rockUser.distance}}m</p>
        </div>
        <div class="arrows"></div>
      </div>
    </div>
  </div>
</template>

<script>
import { GetYaoyiyao } from "@/api/findData.js";
import Shake from "shake.js";
export default {
  data() {
    return {
      isShowRockUserBox: false,
      searchOver: false,
      rockUser: {}
    };
  },
  created() {
    try {
      PCNWebInteration.jsTunedupNativeWithTypeParamSign(1006, "", "");
    } catch (e) {
      getDefaultMap(".siteCity", "#siteVal", ".site");
    }
  },
  mounted() {
    this.imgAnimation();
    // 实例化一个 shake 对象
    let myShakeEvent = new Shake({
      threshold: 20, // 默认摇动阈值
      timeout: 1200 // 默认两次事件间隔时间
    });
    // 监听设备的动作
    myShakeEvent.start();
    // 添加一个事件监听
    window.addEventListener("shake", this.shakeEventDidOccur, false);
    window["nativeLocationCompletion"] = (latitude, longitude) => {
      this.NativeLocationCompletion(latitude, longitude);
    };
  },
  methods: {
    GoAddFriend: function() {
      if (this.rockUser) {
        this.$router.push({
          name: "Information",
          query: { userid: this.rockUser.nodeid }
        });
      }
    },
    NativeLocationCompletion: function(latitude, longitude) {
      if (
        (latitude === "0" && longitude === "0") ||
        (latitude === "4.9E-324" && longitude === "4.9E-324")
      ) {
        this.$toast("获取当前位置失败，请打开GPS");
      } else {
        this.GetYaoyiyao(latitude, longitude);
      }
    },
    GetYaoyiyao: function(latitude, longitude) {
      console.info("latitude:" + latitude);
      console.info("longitude:" + longitude);
      GetYaoyiyao({ longitude: longitude, latitude: latitude }, resp => {
        if (resp.result < 1 || resp.data == null) {
          this.$toast("附近暂无其他人");
          return;
        }
        console.info(resp);
        this.rockUser = resp.data;
        this.isShowRockUserBox = true;
      });
    },
    imgAnimation() {
      let self = this;
      let top = 0;
      //是否到顶
      let isTop = false;
      let time = setInterval(() => {
        if (!isTop) {
          top--;
          if (top < -50) {
            isTop = true;
          }
        } else {
          top++;
          if (top == 0) {
            clearInterval(time);
          }
        }
        if (this.$refs.shakeTop) {
          this.$refs.shakeTop.style.top = top + "px";
        }
      }, 10);

      let bot = 0;
      //是否到顶
      let isBot = false;
      let time1 = setInterval(() => {
        if (!isBot) {
          bot++;
          if (top < 50) {
            isBot = true;
          }
        } else {
          bot++;
          if (bot == 0) {
            console.log(this);
            self.searchOver = true;
            console.log(1);
            clearInterval(time1);
          }
        }
        if (this.$refs.shakeBot) {
          this.$refs.shakeBot.style.bottom = top + "px";
        }
      }, 10);
      console.log(123);
    },

    // 摇动的回调函数
    shakeEventDidOccur() {
      // alert("ios版本不兼容，android支持摇一摇");
      this.imgAnimation();
      if (window.navigator.vibrate) {
        navigator.vibrate(500);
      }
    }
  }
};
</script>

<style scoped lang='scss'>
.Findshake {
  height: 100%;
  overflow: hidden;
  .shakeTop,
  .shakeBot {
    background: #2ea2fa;
    width: 100%;
    height: 50%;
    display: flex;
    align-items: flex-end;
    justify-content: center;
    position: relative;
    img {
      width: 1.68rem;
      height: 0.84rem;
    }
  }
  .shakeBot {
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
    .search {
      border: 0.05rem solid rgba(46, 162, 250, 0.5);
      margin-top: 0.8rem;
      background-color: #ffffff;
      border-radius: 0.1rem;
      padding: 0.24rem 0.3rem;
      background: #fff;
      display: flex;
      justify-content: space-between;
      align-items: center;
      .title {
        height: 100%;
        padding: 0.1rem 0.2rem;
        padding-right: 1rem;
        box-sizing: border-box;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
      }
      .dist {
        font-family: PingFang-SC-Medium;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
      }
      .arrows {
        width: 0.2rem;
        height: 0.2rem;
        border-top: 0.04rem solid #ccc;
        border-right: 0.04rem solid #ccc;
        transform: rotate(45deg);
      }
      img {
        width: 1rem;
        height: 1rem;
        border-radius: 50%;
      }
      p {
        margin: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.28rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
      }
    }
  }
  .hint {
    margin-top: 1.6rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}
</style>
