<!-- 我的二维码 -->
<template>
  <div class="code">
    <div class="content">
      <div class="title">
        <img src="@/assets/images/invite_weichat@2x.png" alt />
        <span>小狐狸。</span>
      </div>
      <!-- 分割 -->
      <div class="br">
        <img src="@/assets/images/erweima_bg@2x.png" alt />
      </div>
      <div id="qrcode"></div>
      <div class="text">{{$t('m.scanaddfriend')}}</div>
    </div>
    <!-- 按钮 -->
    <div class="btn">
      <button @click="save">{{$t('m.saveimg')}}</button>
    </div>
  </div>
</template>

<script>
import QRCode from "qrcodejs2";
import { Toast } from "vant";
export default {
  data() {
    return {};
  },
  methods: {
    qrcode() {
      let qrcode = new QRCode("qrcode", {
        width: 200,
        height: 200, // 高度  [图片上传失败...(image-9ad77b-1525851843730)]
        text: "http://www.baidu.com" // 二维码内容
        // render: 'canvas' // 设置渲染方式（有两种方式 table和canvas，默认是canvas）
        // background: '#000'  ,
        //  foreground: '#ff0'
      });
      // console.log(qrcode)
    },
    save() {
      Toast("已保存至相册");
    }
  },
  mounted() {
    this.qrcode();
  }
};
</script>

<style scoped lang='scss'>
@media screen and (width: 320px) {
  .text {
    margin-top: 0.5rem !important;
    
  }
}
/deep/ .van-toast {
  top: 80% !important;
}

.btn {
  padding: 0.5rem;
  padding-top: 0.6rem;
  display: flex;

  button {
    display: flex;
    align-items: center;
    justify-content: center;
    border: 0;
    width: 100%;
    height: 0.8rem;
    background-color: #ffffff;
    border-radius: 0.4rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }
}

.text {
  font-family: PingFang-SC-Medium;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;
  text-align: center;
  margin-top: 0.79rem;
  // white-space: nowrap;
}

#qrcode {
  margin: 0 auto;
  display: flex;
  justify-content: center;
  margin-top: 0.5rem;

  /*  width: 4.6rem;
    height: 4.45rem; */
  /* background-color: #ebebeb; */
  img {
    width: 100% !important;
    height: 100% !important;
  }
}

.code {
  height: 100%;
  box-sizing: border-box;
  background-color: #2ea2fa;
  padding: 1rem 0.5rem 0 0.5rem;

  .content {
    width: 100%;
    box-sizing: border-box;
    padding: 0.5rem;
    height: 8.63rem;
    background-color: #ffffff;
    border-radius: 0.2rem;

    .br {
      margin-top: 0.5rem;
      display: flex;
      height: 0.1rem;
      width: 100%;
    }

    img {
      width: 100%;
      height: 100%;
    }

    .title {
      display: flex;

      span {
        margin-left: 0.25rem;
        padding-top: 0.18rem;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #666666;
      }

      img {
        vertical-align: middle;
        width: 1rem;
        height: 1rem;
        border-radius: 50%;
      }
    }
  }
}
</style>
