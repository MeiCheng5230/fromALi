<!-- 授权 -->

<template>
  <div class="authorization">
    <div class="logo">
      <div>
        <img src="@/assets/images/logo.png" alt />
      </div>
      <div>相信</div>
    </div>
    <!-- 账号 -->
    <div
      class="user"
      :style="{backgroundImage:'url('+require('@/assets/images/shouquan_bg.png')+')'}"
    >
      <div>
        <img src="@/assets/images/chat_chat_address@2x.png" alt />
        <span>dddd(16000000)</span>
      </div>
      <router-link to="/SwitchUser" tag="div">切换账号</router-link>
    </div>
    <!-- 授权信息 -->
    <div class="info">
      <p>使用XX账号授权以下信息</p>
      <p>·获得莫得公开信息(昵称、头像等)</p>
      <p>·允许好友看到你在该应用上的使用信息</p>
    </div>
    <!-- 提交 -->
    <div class="btn">
      <button>授权并绑定账号</button>
    </div>
  </div>
</template>


<style lang='scss' scoped>
.btn {
  padding: 0 0.35rem;
  box-sizing: border-box;
  display: flex;
  padding-top: 0.35rem;
  button {
    border: 0;
    width: 100%;
    height: 0.72rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: MicrosoftYaHei;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.3rem;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}

.info {
  p {
    margin: 0;
    padding: 0 0.35rem;
    font-family: MicrosoftYaHei;
    font-size: 0.24rem;
    line-height: 0.3rem;
    height: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.3rem;
    letter-spacing: 0rem;
    color: #999999;
  }

  p:nth-child(1) {
    color: #000000;
    margin-top: 0.3rem;
    margin-bottom: 0.4rem;
  }
}

.user {
  border-top: 0.01rem solid #f4f4f4;
  background-size: 100% 100%;
  background-repeat: no-repeat;
  height: 0.9rem;
  padding-left: 0.45rem;
  padding-right: 0.35rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: #fff;

  & > div {
    font-family: MicrosoftYaHei;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }

  & > div:nth-child(1) {
    font-family: ArialMT;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
    display: flex;
    align-items: center;

    img {
      margin-right: 0.25rem;
      width: 0.72rem;
      height: 0.72rem;
      border-radius: 50%;
    }
  }
}

.logo {
  height: 2.2rem;
  display: flex;
  align-items: center;
  flex-direction: column;
  background: #fff;

  div {
    margin: 0;
    margin-top: 0.4rem;
    font-family: MicrosoftYaHei;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;

    img {
      width: 0.9rem;
      height: 0.9rem;
    }
  }

  div:nth-child(2) {
    margin-top: 0.1rem;
  }
}

.authorization {
  height: 100%;
  background: #e1e5ef;
}
</style>
