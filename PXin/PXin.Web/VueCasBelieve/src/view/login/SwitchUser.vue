<!-- 切换用户 -->
<template>
  <div class="SwitchUser">
    <div
      class="bgImg"
      :style="{backgroundImage:'url('+require('@/assets/images/login_bg@2x.png')+')'}"
    >
      <div class="content">
        <div v-show="!isPhoneLogin" class>
          <img src="@/assets/images/login_person@2x.png" alt />
          <input placeholder="请输入手机号" type="text" />
        </div>
        <div v-show="!isPhoneLogin" class>
          <img src="@/assets/images/login_passwork@2x.png" alt />
          <input placeholder="请输入密码" type="password" />
        </div>

        <!-- 手机登录 -->
        <div v-show="isPhoneLogin" class>
          <img src="@/assets/images/login_person@2x.png" alt />
          <input v-model="phone" placeholder="请输入手机号" type="text" />
          <span v-show="!isCode" @click="clickGetCode">获取验证码</span>
          <span class="active" v-show="isCode">{{time}}s后重发</span>
        </div>
        <div v-show="isPhoneLogin" class>
          <img src="@/assets/images/login_code@2x.png" alt />
          <input placeholder="请输入验证码" type="password" />
        </div>

        <!-- btn -->
        <div class="btn">
          <button>登录</button>
        </div>
        <div v-show="!isPhoneLogin" @click="isPhoneLogin=true" class="phone">手机快捷登录</div>
        <div v-show="isPhoneLogin" @click="isPhoneLogin=false" class="phone">密码登录</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      //手机号
      phone: "",
      // 0 密码登录 1 手机快捷登录
      isPhoneLogin: false,
      //0 获取验证码 1 s后重发
      isCode: false,
      time: 60
    };
  },
  methods: {
    //获取验证码
    clickGetCode() {
      //验证
      let reg = /^[1][3,4,5,6,7,8,9][0-9]{9}$/;
      if (!reg.test(this.phone)) {
        this.$toast("请输入有效手机号");
        return;
      }
      //倒计时
      this.time = 60;
      this.isCode = true;
      let time = setInterval(() => {
        this.time--;
        if (this.time == 0) {
          this.isCode = false;
          clearInterval(time);
        }
      }, 1000);
    }
  }
};
</script>

<style scoped lang='scss'>
.active {
  color: #ff3366 !important;
}
.phone {
  font-family: PingFang-SC-Medium;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;
  padding: 0.2rem !important;
  border: 0 !important;
  justify-content: center;
}

.btn {
  margin-top: 0.85rem;
  border: 0 !important;
  padding-bottom: 0 !important;

  button {
    width: 100%;
    height: 0.82rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
  }
}

.content {
  position: relative;
  top: 2.45rem;
  width: 100%;
  box-sizing: border-box;
  height: 6.3rem;
  background-color: #ffffff;
  box-shadow: 0rem 0.02rem 0.1rem 0rem #a6d9ff;
  border-radius: 0.1rem;
  padding: 0 0.5rem;
  padding-top: 1.2rem;

  & > div {
    padding: 0.3rem 0;
    display: flex;
    align-items: center;
    border-bottom: 0.01rem solid #f2f2f2;

    span {
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #2ea2fa;
    }

    input {
      flex: 1;
      border: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }

    input::placeholder {
      color: #999999;
    }

    img {
      width: 0.26rem;
      height: 0.26rem;
      margin-right: 0.15rem;
    }
  }
}

.bgImg {
  height: 5.2rem;
  background-size: 100% 100%;
  box-sizing: border-box;
  padding: 0 0.5rem;
}
</style>
