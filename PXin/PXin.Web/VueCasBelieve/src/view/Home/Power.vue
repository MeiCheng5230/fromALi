<!-- 倍率 -->
<template>
  <div class="Power">
    <div
      class="content"
      :style="{backgroundImage:'url('+require('@/assets/images/setting_bg@2x.png')+')'}"
    >
      <div class="count">
        <div class="Countimg" @click="setPower(-1)">-</div>
        <div
          :style="{backgroundImage:'url('+require('@/assets/images/number_bg@2x.png')+')'}"
          class="CountText"
        >
          <p class="powCoun">{{rate}}</p>
          <p>接收倍率</p>
        </div>
        <div class="Countimg" @click="setPower(1)">+</div>
      </div>

      <div class="save">
        <button @click="save">保存</button>
      </div>
    </div>
    <!-- 文字提示 -->
    <div class="hint">
      <p>设置说明：</p>
      <p>1.用户设置倍率所有私聊生效，默认为1</p>
      <p>2.群聊倍率默认为1，只有群主可设置</p>
      <p>3.扣发送者的V点，但扣了n个V点，会同时给发送者和接收者都加对应n个P点；(例如我发一条消息70个字给你，扣了我7V，但同时会给我加上7P点，你作为接收者也会加7P点)</p>
      <p>（群聊：扣减金额=消息金额*群成员人数，群里所有人增加 X个P,X=扣减数量）</p>
      <p>图片大小以压缩后实际传输的大小为准，如果用户选择发送原图，则以原图尺寸为准。扣V点的最小单位为0.01V</p>
    </div>
  </div>
</template>

<script>
import { getStore, GetSequence } from "@/config/utils.js";
import { executeServerMethod } from "@/config/signalr.js";
export default {
  data() {
    return {
      //倍率
      rate: 1,
      targetId: -1, //对方id
      userId: -1 //自己id
    };
  },
  mounted() {
    let _this = this;
    //对方Id
    _this.targetId = _this.$route.query.targetId;
    let userInfo = JSON.parse(getStore("UserConfigInfo")).data;
    _this.userId = userInfo.nodeid;
    //获取聊天倍率
    let data = {
      command_id: 0x00000007,
      sequence_id: GetSequence(),
      type: 1,
      Sender: _this.targetId,
      Receiver: _this.userId
    };
    executeServerMethod(data);
    //倍率查询答复
    _this.$eventBus.$on("chatFeeRateQueryResp", function(messageResp) {
      if (messageResp) {
        _this.rate = messageResp.ReceiverRate;
      }
    });
  },
  methods: {
    // 加减倍率
    setPower(num) {
      if (this.rate == 0 && num == -1) return;
      this.rate += num;
    },
    // 按钮 保存
    save() {
      let data = {
        command_id: 0x00000006,
        sequence_id: GetSequence(),
        Type: 1,
        Sender: this.targetId,
        Receiver: this.userId,
        Rate: this.rate
      };
      executeServerMethod(data);
      //倍率查询答复
      this.$eventBus.$on("ChatFeeRateSetResp", function(messageResp) {
        console.log("ChatFeeRateSetResp");
        console.log(messageResp);
      });
      this.$router.go(-1);
    }
  },
  destroyed() {
    this.$eventBus.$off("chatFeeRateQueryResp");
    this.$eventBus.$off("ChatFeeRateSetResp");
  }
};
</script>

<style scoped lang='scss'>
.hint {
  padding: 0.3rem;
  padding-top: 0.8rem;

  p {
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}

.Power {
  height: 100%;
  background: #2ea2fa;
  overflow-y: scroll;
}

.save {
  text-align: center;

  button {
    width: 2.6rem;
    height: 0.68rem;
    background-color: #ffffff;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #2ea2fa;
    border: 0;
  }
}

.CountText {
  width: 3.2rem;
  height: 3.2rem;
  background-size: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;

  p {
    margin: 0;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;
  }

  .powCoun {
    font-family: PingFang-SC-Medium;
    font-size: 0.72rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #ff3030;
    padding-bottom: 0.3rem;
  }
}

.count {
  margin: 0 auto;

  display: flex;
  width: 5.43rem;
  height: 3.2rem;
  justify-content: space-between;
  align-items: center;
}

.Countimg {
  width: 0.38rem;
  height: 0.38rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.72rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.36rem;
  letter-spacing: 0rem;
  color: #ffffff;

  img {
    width: 100%;
    height: 100%;
  }
}

.content {
  width: 100%;
  padding-top: 1rem;
  height: 5.7rem;
  background-color: #2ea2fa;
  background-repeat: no-repeat;
  background-size: 100%;
  background-position-y: 100%;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
</style>
