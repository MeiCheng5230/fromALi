<template>
  <div>
    <!-- <van-nav-bar :title="title" fixed :left-text="leftText">
    <div class='left' slot='left' v-if='back' :class='leftText && "isleftText"'>
      <img @click='$router.go(-1)' src="@/assets/images/back.png" alt="" :class='leftText && "isleftimg"'>
      <span>{{leftText}}</span>
    </div>
    <div slot='right' class='right'>
      <img src="@/assets/images/right_icon.png" alt="">
    </div>
  </van-nav-bar> -->
  </div>

</template>

<script>
  export default {
    props: ['title', 'back', 'leftText'],
  }
</script>

<style scoped lang='scss'>
  [class*=van-hairline]::after {
    border-bottom-width: 0;
  }

  .van-nav-bar {
    height: 1rem;
    box-sizing: border-box;
    padding: 0 .2rem;
    background-color: #2ea2fa;
    display: flex;
    align-items: center;
    justify-content: space-between;

    .isleftText {
      display: flex;
      align-items: center;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
    }

    .isleftimg {
      padding-right: .2rem;
    }

    .van-nav-bar__left {
      height: 100%;
      display: flex;
      align-items: center;
    }

    .left {
      display: flex;
      align-items: center;
      img {

        width: 0.2rem;
        height: 0.35rem;
        vertical-align: middle;
      }
    }

    .van-nav-bar__title {
      font-family: PingFang-SC-Bold;
      font-size: 0.36rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
    }

    .van-nav-bar__right {
      bottom: auto;
      right: .2rem;
    }

    .right {
      width: 1.74rem;
      height: 0.62rem;

      img {
        width: 100%;
        height: 100%;
      }
    }
  }
</style>
