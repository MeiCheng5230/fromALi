<!-- 忘记密码 -->

<template>
  <div class="ForgottenPwd">
    <table></table>
    <div class="ipt">
      <div>
        <p>
          <span>手机号码：</span>
          <span>{{mobileno}}</span>
        </p>
      </div>
      <div>
        <input v-model="smscode" type="text" placeholder="请输入验证码" />
        <span v-show="!isTime" @click="setTime">获取验证码</span>
        <span v-show="isTime">{{time}}s</span>
      </div>
      <div>
        <input v-model="newpwd" type="password" :placeholder="type==1?'请设置新密码':'请设置新的支付密码'" />
      </div>
    </div>
    <!-- 提示 -->
    <div class="hint" v-if="type==1">登录密码由6-20位字母、数字、字符任意组合,请注意区分大小写</div>
    <!-- 提示 -->
    <div class="hint" v-if="type==2">支付密码由6数字组成</div>
    <!-- 确定 -->
    <div class="btn">
      <button class="active" @click="submit">确定</button>
    </div>
  </div>
</template>

<script>
import { GetUserInfo, ForgetPwd } from "@/api/myData.js";
import { SendSms } from "@/api/sysRequest.js";

export default {
  data() {
    return {
      user: {},
      smscode: "",
      newpwd: "",
      time: 60, //倒计时
      isTime: false, //是否获取验证码
      type: 0 //登录密码1，支付密码2
    };
  },
  methods: {
    //获取验证码
    setTime: function() {
      this.$toast("发送短信成功");
      this.isTime = true;
      let time = setInterval(() => {
        this.time--;
        if (this.time == 0) {
          this.time = 60;
          this.isTime = false;
          clearInterval(time);
        }
      }, 1000);
      SendSms({ mobileno: this.user.mobileno, typeid: 9 }, data => {});
    },
    submit: function() {
      ForgetPwd(
        {
          type: this.type,
          mobileno: this.user.mobileno,
          smscode: this.smscode,
          newpwd: btoa(this.newpwd)
        },
        data => {
          this.$toast(data.message);
          if (data.result > 0) {
            setTimeout(() => {
              this.$router.go(-1);
            }, 500);
          }
        }
      );
    }
  },
  created() {
    GetUserInfo(null, data => {
      this.user = data.data;
    });
    this.type = this.$route.query.type;
  },
  computed: {
    mobileno: function() {
      if (this.user.mobileno) {
        return (
          this.user.mobileno.substring(0, 3) +
          "****" +
          this.user.mobileno.substring(7, this.user.mobileno.length)
        );
      } else {
        ("***********");
      }
    }
  }
};
</script>

<style scoped lang='scss'>
.btn {
  margin-top: 1.4rem;
  padding: 0 0.3rem;
  display: flex;

  button {
    width: 100%;
    border: 0;
    height: 0.88rem;
    line-height: 0.88rem;
    background-color: #90caf6;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
  }

  .active {
    background-color: #2ea2fa;
  }
}

.hint {
  margin-top: 0.3rem;
  padding: 0 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #666666;
}

.ipt {
  margin-top: 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;

  & > div {
    height: 0.88rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #fff;
    margin-top: 0.2rem;
    padding: 0 0.3rem;

    & > input {
      flex: 1;
      border: 0;
      height: 75%;
    }

    & > input::placeholder {
      color: #999999;
    }

    & > span {
      height: 100%;
      /* line-height: .88rem; */
      display: flex;
      align-items: center;
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #2ea2fa;
    }
  }
}

.ForgottenPwd {
  height: 100%;
  background: #f2f2f2;
}
</style>
