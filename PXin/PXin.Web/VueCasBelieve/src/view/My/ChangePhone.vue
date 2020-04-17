<!-- 绑定新手机号 -->
<template>
  <div class="test">
    <table></table>
    <div class="info">
      <input v-model="mobileno" type="text" placeholder="请输入您的手机号" />
      <button @click="ClickGetCode()" v-show="show">{{$t('m.getcode')}}</button>
      <button v-show="!show" class="timeout">已发送({{time}}s)</button>
    </div>
    <div class="info">
      <input class="code" v-model="code" type="text" :placeholder="$t('m.testcode')" />
      <div></div>
    </div>
    <!-- 按钮 -->
    <div class="btn">
      <button :class="code && 'active'" @click="Next()">{{$t('m.finish')}}</button>
    </div>
  </div>
</template>

<script>
import { SendSms } from "@/api/sysRequest.js";
import { EditMobileno } from "@/api/myData.js";
export default {
  data() {
    return {
      mobileno: "", //手机号
      code: "", //验证码
      show: true, //1显示获取验证码,0显示已发送xS
      time: 60 //
    };
  },
  watch: {
    show(Nval, Fval) {
      if (!Nval) {
        let time = setInterval(() => {
          this.time--;
          if (this.time == 0) {
            clearInterval(time);
            this.show = true;
            this.time = 60;
          }
        }, 1000);
      }
    }
  },
  methods: {
    //获取验证码
    ClickGetCode() {
      let reg = /^[1][3,4,5,6,7,8,9][0-9]{9}$/;
      if (!reg.test(this.mobileno)) {
        this.$toast("请输入有效手机号");
        return;
      }
      this.show = false;
      SendSms({ mobileno: this.mobileno, typeid: 9 }, data => {
        this.$toast("发送短信成功");
      });
    },
    Next: function() {
      let data = {
        oldmobileno: this.$route.query.oldMobileNo,
        newmobileno: this.mobileno,
        oldsmscode: this.$route.query.oldCode,
        newsmscode: this.code
      };
      EditMobileno(data, data => {
        if (data.result < 0) {
          this.$toast(data.message);
          return;
        }
        setTimeout(() => {
          this.$toast("修改手机号成功");
        }, 500);
        this.$router.go(-3);
      });
    }
  },
  created() {
    this.mobileno = this.$route.params.mobileno;
  }
};
</script>

<style scoped lang='scss'>
.code::placeholder {
  color: #999;
}

.code {
  color: #999999 !important;
}

.active {
  color: #fff !important;
  background: #2ea2fa !important;
}

.btn {
  display: flex;
  box-sizing: border-box;
  padding: 0 0.3rem;
  padding-top: 1.38rem;

  button {
    width: 100%;
    border: 0;
    height: 0.88rem;
    line-height: 0.88rem;
    background-color: rgba(46, 162, 250, 0.5);
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: rgba(255, 255, 255, 0.5);
  }
}

.test {
  height: 100%;
  background-color: #f2f2f2;
}

.info {
  margin-top: 0.2rem;
  width: 100%;
  box-sizing: border-box;
  background: #fff;
  height: 0.88rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-left: 0.98rem;
  padding-right: 0.89rem;

  button {
    padding: 0;
    padding: 0 0.1rem;
    border: 0;
    height: 0.52rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    border: solid 0.01rem #2ea2fa;
    font-family: PingFang-SC-Bold;
    font-size: 0.2rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }

  .timeout {
    border: solid 0.01rem #2ea2fa;
    background: none;
    color: #2ea2fa;
  }

  input {
    flex: 1;
    height: 80%;
    width: 70%;
    border: 0;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }
}
</style>
