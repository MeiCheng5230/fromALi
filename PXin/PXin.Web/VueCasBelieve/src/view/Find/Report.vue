<!-- 举报 -->
<template>
  <div class="report">
    <div class="detail">
      <div class="reportname">
        投诉
        <span>{{nodename}}</span>的信友圈:
      </div>
      <div class="text">
        <div class>
          <div>
            <img src="@/assets/images/invite_note@2x.png" alt />
          </div>
          <div>
            <p>{{nodename}}</p>
            <p>{{message.createtime}}</p>
          </div>
        </div>
        <!--  -->
        <div>{{message.content}}</div>
        <div>
          <!-- <video-player
            v-if="message.video!=null"
            class="video-player vjs-custom-skin"
            ref="videoPlayer"
            :options="GetPlayerOptions(message.video)"
          ></video-player>-->
        </div>
      </div>
      <!-- 投诉原因 -->
      <div class="reason">
        <h3>投诉原因</h3>
        <div>
          <div @click="ReasonOnClick(reason)" v-for="(reason,index) of reasonList " :key="index">
            <img
              :src="!reason.isSelected?require('@/assets/images/dynamic_radio_nor@2x.png'):require('@/assets/images/dynamic_radio_sel@2x.png')"
              alt
            />
            {{reason.reasonname}}
          </div>
        </div>
      </div>
      <!-- 补充说明 -->
      <div class="addreport">
        <h3>补充说明</h3>
        <div class="textarea">
          <textarea
            v-model="reportContent"
            placeholder="请尽量详细描写，以确保举报能尽快被受理，可留下联系方式以便客服人员联系您"
            name
            id
            cols="30"
            rows="10"
          ></textarea>
        </div>
      </div>
      <!-- 提交 -->
      <div class="sub">
        <button @click="Submit()">确认提交</button>
        <button @click="$router.go(-1)">取消</button>
      </div>
    </div>

    <!-- 弹出框 -->
    <van-popup v-model="afterSubmitBoxShow">
      <div class="model">
        <div class="modelIMG">
          <img src="@/assets/images/icon_success.png" alt />
        </div>
        <div>提交成功，感谢您的支持</div>
        <div>
          <button @click="$router.go(-1)">返回</button>
        </div>
      </div>
    </van-popup>
  </div>
</template>

<script>
import { Base64 } from "@/config/utils.js";
import { GetReasonList, CreateReport } from "@/api/findData.js";
import { videoPlayer } from "vue-video-player";
export default {
  data() {
    return {
      message: {},
      nodename: "",
      reasonList: [], //投书原因
      reportContent: "", //投诉文字
      afterSubmitBoxShow: false //提交成功后弹框显示
    };
  },
  methods: {
    GetPlayerOptions: function(videourl) {
      return {
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
            src: videourl, // 路径
            type: "video/mp4" // 类型
          }
        ],
        poster: require("@/assets/images/waller_dos@2x.png"), //你的封面地址
        // width: document.documentElement.clientWidth,
        notSupportedMessage: "此视频暂无法播放，请稍后再试", //允许覆盖Video.js无法播放媒体源时显示的默认信息。
        controlBar: {
          timeDivider: true,
          durationDisplay: true,
          remainingTimeDisplay: false,
          fullscreenToggle: true //全屏按钮
        }
      };
    },
    ReasonOnClick: function(reason) {
      this.reasonList.forEach(element => {
        if (element.reasonid != reason.reasonid) {
          element.isSelected = false;
        }
      });
      reason.isSelected = !reason.isSelected;
    },
    //确认提交
    Submit: function() {
      let reason = null;
      for (let index = 0; index < this.reasonList.length; index++) {
        if (this.reasonList[index].isSelected) {
          reason = this.reasonList[index];
          break;
        }
      }
      if (reason == null) {
        this.$toast("至少选择一项投诉原因");
        return;
      }
      CreateReport(
        {
          infoid: this.message.infoid,
          reason: reason.reasonid,
          remarks: this.reportContent
        },
        data => {
          if (data.result > 0) {
            this.afterSubmitBoxShow = true;
          } else {
            this.$toast(data.message);
          }
        }
      );
    }
  },
  created() {
    let query = this.$route.query;
    this.nodename = query.nodename;
    this.message = JSON.parse(Base64.decode(query.message));
    GetReasonList(null, data => {
      if (data.result < 1) {
        this.$toast(data.message);
        return;
      }
      data.data.forEach(element => {
        element.isSelected = false;
      });
      this.reasonList = data.data;
      this.reasonList[0].isSelected = true;
    });
  },
  components: {
    videoPlayer
  }
};
</script>

<style scoped lang='scss'>
.modelIMG {
  width: 0.98rem;
  height: 0.98rem;

  img {
    width: 100%;
    height: 100%;
  }
}

.van-popup {
  border-radius: 0.1rem;
}

.model {
  box-sizing: border-box;
  padding-top: 0.57rem;
  padding-bottom: 0.73rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  width: 5.8rem;
  height: 4.13rem;
  background-color: #ffffff;

  div {
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.42rem;
    letter-spacing: 0rem;
    color: #1a1a1a;
  }

  button {
    width: 4.2rem;
    height: 0.6rem;
    line-height: 0.6rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: MicrosoftYaHei;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
  }
}

.sub {
  padding-top: 1.3rem;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;

  button {
    margin-top: 0.3rem;
    width: 100%;
    border: 0;
    height: 0.88rem;
    background-color: #2ca1f9;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }

  button:nth-child(2) {
    background-color: #fff;
    color: #2ca1f9;
    border: solid 0.02rem #2ca1f9;
  }
}

textarea::placeholder {
  color: #999999;
}

textarea {
  width: 100%;
  box-sizing: border-box;
  padding: 0.2rem;
  height: 1.28rem;
  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
  resize: none;
}

.addreport {
  h3 {
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #000000;
  }
}

.reason {
  & > div {
    display: flex;
    flex-wrap: wrap;

    & > div:nth-of-type(n + 3) {
      margin-top: 0.25rem;
    }

    & > div {
      img {
        width: 0.4rem;
        height: 0.4rem;
        margin-right: 0.25rem;
      }

      display: flex;
      align-items: center;
      width: 50%;
      font-family: PingFang-SC-Regular;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #000000;
    }
  }

  h3 {
    margin-top: 0.6rem;
    font-family: PingFang-SC-Regular;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.text {
  & > div:nth-child(1) {
    display: flex;
    align-items: center;
    padding-top: 0.2rem;

    & > div:nth-child(1) {
      display: flex;
      width: 0.6rem;
      height: 0.6rem;

      img {
        width: 100%;
        height: 100%;
      }
    }

    & > div:nth-child(2) {
      margin-left: 0.23rem;

      p {
        margin: 0;
        font-family: PingFang-SC-Regular;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
      }

      p:nth-child(1) {
        color: #00297b;
        font-size: 0.3rem;
      }
    }
  }

  & > div:nth-child(2) {
    padding-top: 0.25rem;
    padding-left: 0.8rem;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #000000;
  }
}

.detail {
  padding: 0 0.3rem;
}

.reportname {
  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
  height: 0.77rem;
  display: flex;
  align-items: center;

  span {
    color: #00297b;
  }
}

.report {
  height: 100%;
}
</style>
