<template>
  <div class="register">
    <header></header>
    <div class="bgImg">
      <div :style="{backgroundImage: 'url(' + require('./../../static/img/47.png') + ')' }">
        <p>相信不仅能聊天</p>
        <p>发消息还能赚P点</p>
      </div>
      <p>现在注册可免费领取50V点</p>
    </div>

    <div class="reg">
      <div class="region">
        <span>国家或地区</span>
        <router-link :to="{path:'/region',query:{info:$global.userInfo}}" tag="div">
          {{this.$route.query.country||'中国'}}
          <span class="arrows"></span>
        </router-link>
      </div>
      <p class="phone">
        <span class="regionCode">+{{this.$route.query.code||86}}</span>
        <input v-model="mobileno" type="text" placeholder="请输入手机号" maxlength="11" @input="handelInputMobile($event)" />
        <span></span>
      </p>
      <div class="yqCode">
        <span>邀请码</span>
        <input maxlength="6" v-model="invitationcode" type="text" placeholder="请输入邀请码" />
      </div>
      <p class="testCode">
        <span>验证码</span>
        <input ref="code" type="text" placeholder="请输入验证码" v-model="smscode" @input="handelInputSms($event)"
               maxlength="6" />
        <span class="getcode time" v-show="isCountdownBtn" @click="GetSmsCodeBtn">获取验证码</span>
        <span class="time" style="color:#FF3030;" v-show="isCountdownBtn?false:true">{{Countdown60}}s</span>
      </p>
      <div>
        <p class="checked" @click="isAgreement=!isAgreement">
          <img :src="isAgreement?require('@/assets/img/select_sel.png'):require('@/assets/img/select_nor.png')"
               alt="" />
          <span>我已阅读并同意<a href="/html/register.html">《相信注册协议》</a></span>
        </p>
        <div>
          <button :disabled='isDisabled' @click="ValidateFrom">立即注册</button>
        </div>
      </div>
    </div>


    <!-- 提示图片 -->
    <div class='hintImg' v-if='phoneType>0'>
        <img :src="phoneType==1 ? require('@/assets/img/android.png') : require('@/assets/img/ios.png') " alt="" class='hintInfo' />
    </div>
  </div>
</template>

<script>

  export default {
    data() {
      return {
        //手机类型 0     ios 2 android 1
        phoneType:0,
        //手机号
        mobileno: '',
        //邀请码
        invitationcode: '',
        //短信验证码
        smscode: '',
        //地区
        region: "",
        //是否同意协议
        isAgreement: false,
        //倒计时按钮
        isCountdownBtn: true,
        //60秒倒计时
        Countdown60: 59,
        //区号
        areaCode: '+86',
        //判断用户第一次是否请求过验证码
        isPostSms: false,
        //控制多次请求
        isDisabled: false,
      };
    },
    created() {
      this.isQQBrowsers();
      let areNum;
      if (this.$route.query.code == 86 || !this.$route.query.code) {
        areNum = '';
      } else {
        areNum = '+' + this.$route.query.code;
      }
      this.areaCode = areNum;
    },
    beforeDestroy() {
      clearInterval(this.SetCountdown60);
    },
    methods: {
      //是否QQ 微信内置浏览器打开
      isQQBrowsers(){
        let ua = navigator.userAgent.toLowerCase();
        if (ua.match(/ qq/i) == ' qq' || ua.match(/MicroMessenger/i) == 'micromessenger') {
          //qq内置浏览器 或 微信内置浏览器
          this.phoneType = true ; 

          var u = navigator.userAgent;
          let isAndroid = u.indexOf("Android") > -1 || u.indexOf("Adr") > -1; //android终端
          let isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端

          if (!!isAndroid) {
              this.phoneType = 1 ; 
          }
          if (!!isiOS) {
              this.phoneType = 2 ; 
          }
          return;
        }   
      },
      //手机号实时验证输入
      handelInputMobile(e) {
        this.mobileno = e.target.value.replace(/[^\d]/g, "");
      },
      //邀请码实时验证输入
      handelInputSms(e) {
        this.smscode = e.target.value.replace(/[^\d]/g, "");
      },
      //获取邀请码
      GetSmsCodeBtn() {
        if (!this.mobileno) {
          this.Mint.Toast("请输入手机号");
          return;
        }
        if (!this.invitationcode.trim() || this.invitationcode.length < 6) {
          this.Mint.Toast("请输入6位邀请码");
          return;
        }
        this.PostRegValidateMobile();
      },
      //60秒倒计时
      SetCountdown60() {
        this.isCountdownBtn = false;
        let Interval = setInterval(() => {
          if (this.Countdown60 > 0) {
            this.Countdown60--
          } else {
            this.Countdown60 = 59;
            this.isCountdownBtn = true;
            clearInterval(Interval);
          }
        }, 1000);
      },
      //验证表单
      ValidateFrom() {
        if (!this.mobileno) {
          this.Mint.Toast("请输入手机号");
          return;
        }
        if (!this.invitationcode.trim() || this.invitationcode.length < 6) {
          this.Mint.Toast("请输入6位邀请码");
          return;
        }
        if (!this.isPostSms) {
          this.Mint.Toast("请获取验证码");
          return;
        }
        if (!this.smscode) {
          this.Mint.Toast("请输入验证码");
          return;
        }
        if (!this.isAgreement) {
          this.Mint.Toast("请阅读并同意相关协议");
          return;
        }
        this.PostReg();
      },
      //获取验证码ajax
      PostRegValidateMobile() {
        this.isPostSms = true;
        let url = "/api/User/RegValidateMobile";
        let data = {
          mobileno: this.areaCode + '' + this.mobileno,
          invitationcode: this.invitationcode,
          typeid: 9,
          content: "",
          ...this.$global.userInfo
        };
        this.axios.post(url, data)
          .then(res => {
            if (res.data.result < 1) {
              this.Mint.Toast(res.data.message);
              return;
            }
            this.Mint.Toast(res.data.message);
            this.SetCountdown60();

          })
      },
      //用户注册ajax
      PostReg() {
        this.isDisabled = true;
        this.isRegBtn = true;
        let url = '/api/User/Reg';
        let data = {
          mobileno: this.areaCode + '' + this.mobileno,
          invitationcode: this.invitationcode,
          smscode: this.smscode,
          ...this.$global.userInfo,
          ...this.$global.userInfoApp
        };
        this.axios.post(url, data).then(res => {
          this.isDisabled = false;
          if (res.data.result < 1) {
            this.Mint.Toast(res.data.message);
            return;
          }

          this.$router.push("/succReg");
        })
      }
    },
  };
</script>

<style scoped lang='scss'>
.hintImg{
  position: fixed;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 2000;
}
.hintImg img{
  width: 100%;
  height: 100%;
}
  .testCode {
    justify-content: flex-start !important;
  }

  .time {
    width: 1.1rem !important;
    text-align: right;
  }

  .checked {
    padding: 0 !important;
    margin-top: 0.35rem;
  }

  input {
    width: 3rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }

  .reg > div > span:first-child {
    width: 1.8rem;
  }

  .reg > p > span:first-child {
    width: 1.8rem;
  }

  .yqCode {
    height: 0.9rem;
    display: flex;
    align-items: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
    border-bottom: 0.02rem solid #f2f2f2;
  }

  .region {
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    border-bottom: 0.02rem solid #f2f2f2;
    height: 0.9rem;
    & > div

  {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-right: 0.2rem;
    span

  {
    width: 0.15rem;
    height: 0.15rem;
    border-top: 0.02rem solid #333;
    border-right: 0.02rem solid #333;
    transform: rotate(45deg);
  }

  }

  & > span {
    color: #666;
  }

  }

  .phone {
    justify-content: flex-start !important;
    padding: 0 !important;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }

  .register {
    width: 100%;
    overflow-x: hidden;
    height: 100%;
    background: #fff;
  }

  header {
    padding-left: 0.3rem;
    height: 1rem;
    display: flex;
    align-items: center;
    span

  {
    width: 0.2rem;
    height: 0.2rem;
    border-top: 0.03rem solid #333;
    border-right: 0.03rem solid #333;
    transform: rotate(225deg);
  }

  }

  .bgImg {
    padding: 0 0.3rem;
    text-align: center;
    & > p

  {
    color: #666;
    padding-top: 0.3rem;
    font-size: 0.3rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(102, 102, 102, 1);
  }

  div:first-child {
    height: 4rem;
    background-size: 100% 100%;
    p

  {
    font-family: PingFang-SC-Bold;
    color: #666;
    font-size: 0.36rem;
    font-weight: bold;
    color: rgba(102, 102, 102, 1);
    line-height: 0.6rem;
  }

  }
  }

  .reg {
    padding: 0 0.5rem;
    padding-top: 0.8rem;
    & > div

  {
    div

  {
    display: flex;
    justify-content: center;
    button

  {
    margin-top: 0.5rem;
    border: 0;
    width: 6.5rem;
    height: 0.88rem;
    background: rgba(46, 162, 250, 1);
    border-radius: 0rem;
    font-size: 0.3rem;
    font-family: PingFang-SC-Bold;
    font-weight: bold;
    color: rgba(255, 255, 255, 1);
    outline: none;
  }

  }

  & > p {
    display: flex;
    padding-left: 0.5rem;
    align-items: center;
    img

  {
    padding-right: 0.1rem;
    width: 0.28rem;
    height: 0.28rem;
  }

  span {
    font-size: 0.24rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(102, 102, 102, 1);
    color: #666;
  }

  }
  }

  input {
    border: 0;
    outline: none;
    height: 100%;
    padding: 0;
    font-size: 0.3rem;
    -webkit-tap-highlight-color: transparent;
    -webkit-appearance: none;
  }

    input::placeholder {
      font-size: 0.3rem;
      font-family: PingFang-SC-Medium;
      font-weight: 500;
      color: rgba(153, 153, 153, 1);
    }

  & > p {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 0.9rem;
    width: 6.5rem;
    margin: 0 auto;
    border-radius: 0rem;
    border-bottom: 0.02rem solid #f2f2f2;
    .getcode

  {
    margin-right: 0.3rem;
    white-space: nowrap;
    font-size: 0.28rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(46, 162, 250, 1);
  }

  span {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
  }

  }
  }
</style>
