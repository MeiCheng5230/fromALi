<!-- ue账户 -->
<template>
  <div class="ue">
    <table></table>
    <div class="ipt">
      <div class="iptInfo">
        <span>UE账户</span>
        <input type="text" v-model="user" />
      </div>

      <div class="iptInfo">
        <span>昵称</span>
        <input type="text" v-model="name" />
      </div>
    </div>

    <!-- 更换绑定 -->
    <div class="btn">
      <button>更换绑定</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      user: 12345678901,
      name: "昵称昵称"
    };
  }
};
</script>

<style scoped lang='scss'>
.btn {
  box-sizing: border-box;
  padding: 0 0.3rem;
  padding-top: 2rem;
  button {
    width: 100%;
    height: 0.88rem;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    border: 0;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
  }
}

.iptInfo {
  background: #fff;
  display: flex;
  align-items: center;
  height: 0.88rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  padding-left: 0.3rem;
  margin-top: 0.3rem;

  span {
    width: 1.7rem;
    white-space: nowrap;
  }

  input {
    border: 0;
    margin: 0;
    white-space: nowrap;
    overflow: hidden;
  }
}

.iptInfo:last-child {
  margin-top: 0.2rem;
}

.ue {
  height: 100%;
  background: #f2f2f2;
}
</style>
