<!-- 联系方式 -->
<template>
  <div class="Contact">
    <p>{{$t('m.phonenum')}}</p>
    <div class="ipt">
      <input :placeholder="$t('m.searchphonenumber')" readonly v-model="mobileno" type="text" />
    </div>
    <!-- 更换手机号 -->
    <div class="sub">
      <button @click="ChangeMobileNo()">{{$t('m.replacephone')}}</button>
    </div>
  </div>
</template>

<script>
import { GetUserInfo } from "@/api/myData.js";
export default {
  data() {
    return {
      mobileno: "" //手机号
    };
  },
  methods: {
    ChangeMobileNo: function() {
      this.$router.push({
        name: "TestPhone",
        query: { mobileno: this.mobileno }
      });
    }
  },
  created() {
    if (this.$route.query.mobileno) {
      this.mobileno = this.$route.query.mobileno;
      return;
    }
    GetUserInfo(null, data => {
      if (data.result < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.mobileno = data.data.mobileno;
    });
  }
};
</script>

<style scoped lang='scss'>
.sub {
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  display: flex;
  padding-top: 1.68rem;
  button {
    border: 0;
    width: 100%;
    height: 0.88rem;
    line-height: 0.88rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}

.ipt {
  display: flex;
}

input {
  width: 100%;
  height: 0.88rem;
  box-sizing: border-box;
  padding-left: 0.3rem;
  background: #fff;
  border: 0;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}

.Contact {
  height: 100%;
  background-color: #f2f2f2;
}

p {
  margin: 0;
  height: 0.75rem;
  line-height: 0.75rem;
  padding-left: 0.3rem;
  box-sizing: border-box;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}
</style>
