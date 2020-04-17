<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
export default {
  name: "App"
};
</script>

<style lang='scss'>
* {
  touch-action: pan-y;
}
html,
body {
  width: 100%;
  height: 100%;
}
#app {
  font-family: "PingFangSC-Medium", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 100%;
  color: #333;
  font-size: 0.3rem;
}
.van-dialog .van-dialog__header,
.van-dialog .van-dialog__content .van-dialog__message {
  text-align: center ;
  padding-left: 0.5rem;
}
.van-dialog .van-dialog__header {
  text-align: center !important;
  padding-left: 0;
}
p {
  margin: 0;
}
.OctAlert {
  width: 5.4rem;
  .van-dialog__header {
    font-weight: bold;
    padding-top: 0.4rem;
    font-family: PingFangSC-Medium;
    font-size: 0.36rem;
    font-stretch: normal;
    line-height: 0.5rem;
    letter-spacing: 0rem;
    color: #333333;
  }
  .van-dialog__message {
    text-align: left !important;
    padding-bottom: 0.2rem;
    font-family: PingFangSC-Regular;
    font-size: 0.32rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.44rem;
    letter-spacing: 0rem;
    color: #aeaeae;
    p{
      width: 100%;
      text-align: center ;
      padding-bottom:.2rem;
    }
  }
}
</style>
