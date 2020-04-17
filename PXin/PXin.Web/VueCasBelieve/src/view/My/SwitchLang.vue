<template>
  <div class="lang">
    <div
      @click="switchLang(item,index)"
      v-for="(item,index) of lang"
      :key="index"
      class="common"
      style="border-bottom: 1px solid rgb(242, 242, 242);"
    >
      <span>{{item.lang}}</span>
      <img :src="item.isChecked? require('@/assets/images/dynamic_choices@2x.png'):''" alt />
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      lang: [
        { lang: "简体中文", isChecked: false },
        { lang: "繁体中文", isChecked: false },
        { lang: "English", isChecked: false }
      ]
    };
  },
  mounted() {
    //获取 存储
    if (this.$route.params.language) {
      sessionStorage.language = this.$route.params.language;
    }
    for (let item of this.lang) {
      item.lang == sessionStorage.language
        ? (item.isChecked = true)
        : (item.isChecked = false);
    }
  },
  methods: {
    //切换语言
    switchLang(item, index) {
      for (let item of this.lang) {
        item.isChecked = false;
      }
      item.isChecked = true;
      if (item.lang == "简体中文") {
        this.$i18n.locale = "zh-CN";
      }
      if (item.lang == "繁体中文") {
        this.$i18n.locale = "zh-F";
      }
      if (item.lang == "English") {
        this.$i18n.locale = "en-US";
      }
      this.$router.push("/");
    }
  }
};
</script>
<style lang="scss" scoped>
.lang {
  height: 100%;
  background: #f2f2f2;
}
.common {
  height: 0.88rem;
  background: #fff;
  display: flex;
  align-items: center;
  padding: 0 0.3rem;
  justify-content: space-between;
  span {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }
  img {
    width: 0.4rem;
  }
}
</style>
