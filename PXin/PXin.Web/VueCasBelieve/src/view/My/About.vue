<!-- 关于我们 -->
<template>
  <div class="about">
    <table></table>
    <!-- <div class="common" style="margin-top: .3rem;">
      <span>{{$t('m.version')}}</span>
      <span>3.3.3</span>
    </div>-->
    <div @click="IntroductionOnClick()" class="common">
      <span>{{$t('m.function')}}</span>
      <span class="arrows"></span>
    </div>
    <!-- <div class="common" style="border-bottom:1px solid #F2F2F2;">
      <span>检查更新</span>
      <span class="arrows"></span>
    </div>-->
    <div class="common">
      <span>{{$t('m.lang')}}</span>
      <router-link
        tag="div"
        :to="{name:'SwitchLang',params:{language:language}}"
        style="color: #999999;"
      >
        {{language}}
        <span class="arrows"></span>
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  computed: {
    //计算当前是哪种语言
    language() {
      if (this.$i18n.locale == "en-US") return "English";
      if (this.$i18n.locale == "zh-CN") return "简体中文";
      if (this.$i18n.locale == "zh-F") return "繁体中文";
    }
  },
  methods: {
    //功能介绍 跳转
    IntroductionOnClick: function() {
      let origin =
        window.location.origin.indexOf("localhost") < 0
          ? window.location.origin
          : "http://client.be.sulink.cn";
      window.location.href = origin + "/html/about.html";
    }
  }
};
</script>

<style scoped lang='scss'>
.about {
  height: 100%;
  background-color: #f2f2f2;

  .common {
    height: 0.88rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    box-sizing: border-box;
    padding: 0 0.3rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    background: #fff;
  }

  .arrows {
    margin-left: 0.3rem;
    display: inline-block;
    width: 0.2rem;
    height: 0.2rem;
    border-top: 0.02rem solid #999;
    border-right: 0.02rem solid #999;
    transform: rotate(45deg);
  }
}
</style>
