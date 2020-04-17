<template>
  <div class="succReg">
    <!-- <header class="header"></header> -->

    <div class="succ">
      <p>
        <img src="@/assets/img/icon_success.png" alt />
      </p>
      <p>注册成功</p>
      <p>手机验证码为你的默认登录密码和支付密码，请登录后及时到‘安全中心’修改。</p>
    </div>

    <div class="download">
      <button @click="download">立即下载</button>
      <!-- <button>返回微信</button> -->
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
  methods: {
    hist() {
      this.$router.go(-1);
    },
    //下载
    download() {
      // location.href = 'http://m.xiang-xin.net';
      this.axios.post("/api/Sys/GetAppDownloadUrl").then(res => {
        if (res.data.result > 0) {
          //判断手机类型  android ios
          var u = navigator.userAgent;
          var isAndroid = u.indexOf("Android") > -1 || u.indexOf("Adr") > -1; //android终端
          var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
          if (!!isiOS) {
            //   ios下载地址
            window.location.href = res.data.data.ios;
          } else if (!!isAndroid) {
            //   android下载地址
            window.location.href = res.data.data.android;
          }
        }
      });
    }
  }
};
</script>

<style scoped lang='scss'>
.succReg {
  height: 100%;
  background: #f2f2f2;
  header {
    padding-left: 0.3rem;
    height: 1rem;
    display: flex;
    align-items: center;

    span {
      width: 0.2rem;
      height: 0.2rem;
      border-top: 0.03rem solid #333;
      border-right: 0.03rem solid #333;
      transform: rotate(225deg);
    }
  }

  .succ {
    height: 4.14rem;
    background: #fff;
    padding: 0.5rem 0;

    p {
      text-align: center;

      img {
        width: 0.9rem;
        height: 0.9rem;
      }
    }

    p:nth-child(2) {
      padding-top: 0.3rem;
      font-size: 0.36rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;
      color: rgba(102, 102, 102, 1);
      color: #666;
    }

    p:nth-child(3) {
      padding-top: 0.3rem;
      color: #999;
      font-size: 0.28rem;
      font-family: PingFang-SC-Medium;
      font-weight: 500;
      color: rgba(153, 153, 153, 1);
      margin: 0 0.5rem;
    }
  }
}

.download {
  background: rgba(242, 242, 242, 1);
  padding: 0 0.3rem;
  padding-top: 0.6rem;
  button {
    width: 100%;
    height: 0.88rem;
    border: 0;
    outline: none;
    border-radius: 0rem;
    font-size: 0.3rem;
    font-family: PingFang-SC-Bold;
    font-weight: bold;
    color: #fff;
    background-color: #2ea2fa !important;
    border-radius: 0.04rem;
    color: #ffffff;
  }
}
</style>
