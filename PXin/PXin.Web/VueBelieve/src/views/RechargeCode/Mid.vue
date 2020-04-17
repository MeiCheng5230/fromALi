<template>
<!-- v-title -->
  <div  >
    </div>
</template>

<script>
  import { getUrlParams, setStore, getStore } from '@/config/utils'
export default {
    data () {
        return {
        }
    },
    beforeCreate() {
      let page = getUrlParams('router');
      if (page === undefined || page === null || page === '' || page.toLowerCase() === 'rechargecode') {
        return;
      }
      let xx = getStore("home");
      if (xx=="home") {
        setStore("home", "home1");
        BLWebInteration.jsTunedupNativeWithTypeParamSign(1001,"","");
      } else {
        setStore("home", "home1");
        this.$router.push({ path: '/' + page });
      }

    }
  }
</script>

<style lang="scss" scoped>
  .RechargeCode {
    min-height: 100%;
    .content

  {
    padding: 0 0.3rem;
    .shadowBox

  {
    padding: 0.3rem;
    margin: 0.5rem 0;
    box-shadow: 0 0.02rem 0.3rem 0 rgba(141, 171, 196, 0.15);
    border-radius: 0.04rem;
    .tp

  {
    font-size: 0.28rem;
    color: #000;
    font-weight: bold;
    font-family: PingFangSC-Medium;
  }

  .bt {
    display: flex;
    margin-top: 0.5rem;
    .col

  {
    flex: auto;
    .tit

  {
    font-size: 0.24rem;
    color: #999;
  }

  .num {
    margin-top: 0.1rem;
    font-size: 0.32rem;
    color: #333;
  }

  }

  .mycode {
    display: flex;
    flex-direction: column;
    align-items: center;
    .logo

  {
    margin-bottom: 0.2rem;
    img

  {
    height: 0.54rem;
    width: auto;
  }

  }
  }
  }
  }

  .btn {
    text-align: center;
    border-radius: 0.04rem;
    padding: 0.21rem 0;
    font-size: 0.32rem;
    margin: 0 0.5rem 0.4rem 0.5rem;
    &.buyBtn

  {
    margin-top: 1.7rem;
    background-color: #2ea2fa;
    color: #fff;
    box-shadow: 0 0.04rem 0.2rem 0 rgba(46, 162, 250, 0.35);
  }

  &.rechargeBtn {
    color: #2ea2fa;
    border: solid 1px #2ea2fa;
  }

  }

  .explain {
    font-size: 0.24rem;
    color: #999;
    line-height: 0.4rem;
    p

  {
    padding: 0 0.5rem;
    margin: 0;
  }

  }
  }
  }
</style>
