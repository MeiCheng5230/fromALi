<!-- 生成提取码 -->
<template>
  <div class="generate">
    <table></table>
    <div class="SVgenerate">SV生成充值码</div>
    <!-- @click="show=true" -->

    <!-- 选项 -->
    <div class="generateDetails">
      <div>
        <p>
          <span>帐号</span>
          <span>SV余额: {{ svnum }}</span>
        </p>
      </div>
      <div>
        <p>
          <span>手续费</span>
        </p>
        <p>{{ charge }} DOS</p>
      </div>
      <div>
        <p>
          <span>选择规格</span>
        </p>
        <div class="count" >
          <span>1000 SV</span>
          <van-stepper v-model="sv1000" @change="setconfig" min="0" disable-input integer />
        </div>
        <div class="count" >
          <span>5000 SV</span>
          <van-stepper v-model="sv5000" @change="setconfig" min="0" disable-input integer />
        </div>
        <div class="count" >
          <span>10000 SV</span>
          <van-stepper v-model="sv10000" @change="setconfig" min="0" disable-input integer />
        </div>
      </div>
    </div>
    <!-- 共张 -->
    <div class="sum">
      <p>
        共
        <span>{{ sumCount }}</span>张,
        <span>{{ total }}</span>SV
      </p>
    </div>
    <!-- 提示 -->
    <div class="instructions">
      <div>
        <p>温馨提示：</p>
        <p>1.生成充值码的额度最小为1000SV；</p>
        <p>2.生成充值码需要扣除相应的SV余额；</p>
        <p>3.生成充值码将收取生成额度的<span>5%</span>作为手续费，从绑定的码库账号内自动扣除；</p>
        <p>4.今天最多可生成<span style='color:#E73333;'>{{this.todaycount.toLocaleString()}}</span>SVC充值码；</p>
      </div>
    </div>
    <!-- 按钮 -->
    <div class="btn" @click="setpay">生成</div>
    <keyboard
      :isKeyboard="isKeyboard"
      theme="money"
      :pay-price="total"
      balance-name="SV余额"
      :balance="svbalance"
      @close="isKeyboard = false"
      @pay="sendPayAjax"
    ></keyboard>

    <!-- 遮罩层 -->
    <div class="model" v-if="show">
      <div class="modelInfo">
        <div class="photo">1提取码</div>
        <div class="photo">2提取码</div>
        <div @click="show=false">取消</div>
      </div>
    </div>
  </div>
</template>

<script>
import { SvToSvcCard } from '@/api/getFbApData';
import { getStore } from "@/config/utils";
export default {
  data() {
    return {
      isKeyboard: false, //支付界面开关
      show: false,  // 选择下拉框
      svbalance: null, //sv余额
      nodecode: null, //账号
      password: null, //密码
      sv1000: 0, // sv1000数量
      sv5000: 0, // sv5000数量
      sv10000: 0, // sv10000数量
      config: '',   // 生成数量规格 例："1000|1,5000|1,10000|2" ,
      todaycount: 0,  // 每天生成数量
    };
  },
  created() {
    var FbApInfo = JSON.parse(getStore('FbApInfo'));
    this.todaycount = parseInt(this.$route.query.todaycount);
    this.svbalance = FbApInfo.svbalance;
  },
  computed: {
    total() {
      // 支付总额
      return this.sv1000*1000 + this.sv5000*5000 + this.sv10000*10000;
    },
    charge() {
      // 手续费
      return (this.sv1000*1000 + this.sv5000*5000 + this.sv10000*10000) * 0.05;
    },
    sumCount() {
      //总张数
      return this.sv1000 + this.sv5000 + this.sv10000 ;
    },
    svnum() {
      var num = this.svbalance;
        num = num.toString() ;
        let index = num.indexOf('.') ;
        if (index !== -1) {
          num = num.substring(0, 2 + index + 1) ;
        } else {
          num = num.substring(0) ;
        }
        return parseFloat(num).toFixed(2) ;
    }
  },
  methods: {
    setconfig() {
      // 设置参数
      this.config = '1000|' + this.sv1000+',5000|' + this.sv5000 + ',10000|' + this.sv10000;
    },
    setpay() {
      // 点击生成按钮
      if (this.total > this.svbalance) {
        this.Toast('您的SV余额不足！');
        return;
      };
      
      if (this.total > this.todaycount) {
        this.Toast(`今天最多可以生成${this.todaycount.toLocaleString()}充值码(SVC)!`);
        return;
      }

      if (this.sv1000 || this.sv5000 || this.sv10000) {
        this.isKeyboard = true;
      } else {
        this.Toast('请至少选择一种规格！') ;
      }
    },
    async sendPayAjax(paypwd) {
      // sv生成充值码
      this.isKeyboard = false;
      let result = await SvToSvcCard({config:this.config, paypwd, ...JSON.parse(sessionStorage.userParam)});
      if (result.result > 0) {
        this.Toast.success(result.message);
        setTimeout(() => {
          this.$router.go(-1);
        }, 500)
      } else {
        this.Toast.fail(result.message);
      }
    }
  }
};
</script>

<style scoped lang='scss'>
/* 遮罩层 */
.model {
  box-sizing: border-box;
  height: 100%;
  width: 100%;
  position: fixed;
  background: rgba(0, 0, 0, 0.5);
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
}

.modelInfo {
  width: 100%;
  height: 3rem;
  background: #fff;
  position: fixed;
  bottom: 0;
  background: #f7f7fc;
}
/deep/ .van-stepper .van-stepper__input {
  color: #000!important;
  opacity: 1!important;
}
.modelInfo div {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 0.96rem;
  font-family: PingFangSC-Medium;
  font-size: 0.34rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  background: #fff;
}

.modelInfo div:nth-child(2) {
  margin-bottom: 0.14rem;
}

.sum {
  padding: 0 0.3rem;

  p {
    float: right;
    padding-top: 0.2rem;
    font-size: 0.28rem;
    color: #353535;

    span {
      color: #2ea2fa;
      padding: 0 0.1rem;
      font-weight: bold;
    }
  }
}

.generate {
  height: 100%;
  background: #fff;
  overflow-y: scroll;
  .btn {
    position: fixed;
    left: 0;
    right: 0;
    margin: 0 auto;
    bottom: 0.3rem;
    width: 6.9rem;
    line-height: 0.88rem;
    text-align: center;
    height: 0.88rem;
    background-color: #2ea2fa;
    border-radius: 0.06rem;
    font-size: 0.32rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }

  .instructions {
    margin: 0 auto;
    width: 6.9rem;
    /* height: 2.4rem; */
    padding: 0.2rem;
    background-color: #f7f7fc;
    border-radius: 0.06rem;
    margin-top: 1.1rem;
    margin-bottom: 2rem;
    div {
      width: 100%;
      height: auto;

      p {
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        line-height: 0.4rem;
        letter-spacing: 0.01rem;
        color: #999999;
        width: 100%;

        span {
          color: #2ea2fa;
        }
      }
    }
  }

  .SVgenerate {
    width: 6.9rem;
    height: 0.9rem;
    line-height: 0.9rem;
    padding-left: 0.3rem;
    font-size: 0.28rem;
    font-weight: bold;
    color: #333;
    margin: 0.4rem auto 0;
    background-color: #f7f7fc;
    border-radius: 0.06rem;
  }

  .generateDetails {
    padding: 0rem 0.3rem 0;

    & > div {
      border-bottom: solid 0.02rem #dedede;
      padding-bottom: 0.2rem;

      p:nth-child(1) {
        padding-top: 0.41rem;
      }

      p {
        display: flex;
        justify-content: space-between;

        span:nth-child(1) {
          font-size: 0.24rem;
          color: #333333;
        }

        span:nth-child(2) {
          font-size: 0.24rem;
          color: #2ea2fa;
        }
      }

      p:last-child {
        padding-top: 0.2rem;
        font-size: 0.28rem;
        color: #333;
        font-weight: bold;

        input {
          width: 0.68rem;
          vertical-align: middle;
        }
      }

      .count {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding-top: 0.2rem;

        span {
          font-weight: bold;
          font-size: 0.2
        }
      }
    }
  }
}
</style>
