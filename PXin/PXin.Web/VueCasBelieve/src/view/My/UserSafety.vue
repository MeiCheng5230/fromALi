<!-- 账户安全 -->

<template>
  <div class="safety">
    <table></table>
    <div class="ipt">
      <router-link :to="{path:'/ReplaceLoginPwd',query:{type:'1'}}" tag="div" class="iptInfo">
        <span>修改登录密码</span>
        <span class="arrows"></span>
      </router-link>
      <router-link :to="{path:'/ReplaceLoginPwd',query:{type:'2'}}" tag="div" class="iptInfo">
        <span>修改支付密码</span>
        <span class="arrows"></span>
      </router-link>
      <router-link :to="{path:'/ForgottenPwd',query:{type:'2'}}" tag="div" class="iptInfo">
        <span>忘记支付密码</span>
        <span class="arrows"></span>
      </router-link>
    </div>
    <!-- 绑定第三方 -->
    <!-- <div class="bind">
      <p>绑定第三方账户</p>
      <div class="bindInfo" @click="toThreeBind">
        <div>
          <img src="@/assets/images/login_pcn@2x.png" alt />
          <span style="letter-spacing: 0.01rem;">PCN</span>
        </div>
        <div>
          <span style="color:#999;margin-right: .1rem;">立即去绑定</span>
          <span class="arrows"></span>
        </div>
      </div>
    </div>-->
    <!-- <van-popup v-model="show">内容</van-popup> -->
    <!-- <van-popup v-model="show">
      <div class="model">
        <div class="modelTop">
          <p>
            <img src="@/assets/images/icon_reminder@2x.png" alt />
          </p>
          <p>你是否要解除关联？</p>
          <p>解除绑定之后将不能使用PCN账号登录</p>
        </div>
        <div class="modelBottom">
          <div @click="show=false">
            <button>取消</button>
          </div>
          <div>
            <button>解除关联</button>
          </div>
        </div>
      </div>
    </van-popup>-->
  </div>
</template>


<style scoped lang='scss'>
.van-popup {
  border-radius: 0.12rem;

  .model {
    width: 5.3rem;
    height: 3.6rem;
    background-color: #ffffff;

    .modelBottom {
      display: flex;

      & > div {
        display: flex;
        align-items: center;
        width: 50%;
        height: 0.8rem;

        button {
          background: #fff;
          width: 100%;
          height: 0.6rem;
          border: 0;
          font-family: PingFang-SC-Medium;
          font-size: 0.3rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0.01rem;
          color: #2ea2fa;
        }

        button:first-child {
          border-right: 0.02rem solid #f2f2f2;
        }
      }
    }

    .modelTop {
      height: 2.8rem;
      box-sizing: border-box;
      border-bottom: 0.02rem solid #f2f2f2;
      padding-top: 0.48rem;
      padding-bottom: 0.44rem;

      p {
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;

        img {
          width: 0.63rem;
          height: 0.63rem;
        }
      }

      p:nth-child(2) {
        color: #333333;
        font-size: 0.3rem;
        padding-top: 0.48rem;
        padding-bottom: 0.24rem;
      }
    }
  }
}

.arrows {
  width: 0.15rem;
  height: 0.15rem;
  border-top: 0.02rem solid #676767;
  border-right: 0.02rem solid #676767;
  transform: rotate(45deg);
}

.bindInfo {
  padding: 0 0.3rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 1.48rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  background: #fff;

  & > div {
    display: flex;
    align-items: center;
  }

  img {
    margin-right: 0.3rem;
    border-radius: 50%;
    width: 0.8rem;
    height: 0.8rem;
    box-shadow: 0rem -0.03rem 0.08rem 0rem #ededed,
      0.01rem 0.03rem 0.08rem 0rem #ededed;
  }
}

.bind {
  p {
    height: 1.18rem;
    padding-left: 0.3rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #333333;
    margin: 0;
    box-sizing: border-box;
    padding-bottom: 0.3rem;
    display: flex;
    align-items: flex-end;
  }
}

.iptInfo {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 0.88rem;
  background-color: #ffffff;
  padding: 0 0.3rem;
  border-bottom: 0.02rem solid #f3f3f3;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
}

.ipt {
  margin-top: 0.3rem;
}

.safety {
  background: #f2f2f2;
  height: 100%;
}
</style>
