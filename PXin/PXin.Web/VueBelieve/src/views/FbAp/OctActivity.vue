<template>
  <div class="content">
    <div class="banner">
      <img src="@/assets/images/banner1.png" alt />
    </div>
    <div class="container">
      <div class="info">
        <div class="titleImg">
          <img src="@/assets/images/image_title1.png" alt />
        </div>
        <h3>合伙人/代理人</h3>
        <p>1.1 活动期间充值商使用5万SV零售码库存代合伙人/代理人开通充值商；</p>
        <p>1.2 新开通的充值商可获得2万SV批发码库存+3万SV零售码库存；</p>
        <p>1.3 缴费期间原充值商支付200DOS服务费​，新开通的充值商支付100DOS服务费即可获得手机一部；</p>
        <h3>代理人</h3>
        <p>1.1 活动期间代理人向上级充值商进货SV零售码库存1万以上；</p>
        <p>1.2 缴费期间充值商支付200DOS服务费，代理人支付100DOS服务费即可获得手机一部；</p>
        <h3>新注册的合伙人</h3>
        <p>
          1.1 活动期间充值商或代理人给活动期间注册的合
          伙人零售充值码(SVC)1万以上且合伙人充值SV余额
          1万以上；
        </p>
        <p>
          1.2 缴费期间充值商或代理人支付200DOS服务费，
          合伙人支付100DOS即可获得手机一部
        </p>
      </div>
    </div>

    <div class="container" style="margin-top:.5rem; margin-bottom: 2rem;">
      <div class="info" style="border-radius:.12rem;">
        <div class="titleImg">
          <img src="@/assets/images/image_title2.png" alt />
        </div>
        <p>1. 活动时间：10月11-10月31；</p>
        <p>2. 缴费时间：10月21-11月05；</p>
        <p>
          3. 本次活动每个代理人/合伙人账号只能获得一部
          手机，如参与多个活动，只能选择一个活动支付服
          务费。充值商/代理人可为所有满足条件的代理人/
          合伙人支付服务费，人数不做限制；
        </p>
        <p>
          4. 缴费期未支付服务费的用户，将视为自动放弃，
          如其中一方已支付服务费，将在缴费时间结束后退
          回原支付账号；
        </p>
        <p>5. 领取手机的账号必须绑定已实名认证的PCN账号；</p>
        <p>6. 本活动最终解释权归相信所有；</p>
      </div>
    </div>

    <!-- <div class="submit">
      <button @click="pay">开始缴费时间：10月21日</button>
    </div>-->
    <div class="footer">
      <div class="hint">
        <div>
          <div>已满足条件：{{paycount}}</div>
          <img @click="hint(0)" src="@/assets/images/ic_help_tab.png" alt />
        </div>
        <div>
          <div>已获得资格：{{receivecount}}</div>
          <img @click="hint(1)" src="@/assets/images/ic_help_tab.png" alt />
        </div>
      </div>
      <!-- 按钮 -->
      <div class="btns">
        <div>
          <button @click="GetUserOpens(0)">支付服务费</button>
        </div>
        <div>
          <button @click="GetUserOpens(1)" class="getPhone">领取手机</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { Dialog, Divider } from "vant";
import { getUrlParams } from '@/config/utils'
import { HasBindActivityThirdparty, GetOctoberActivityCount ,BindActivityThirdparty} from "@/api/getFbApData";
export default {
  data() {
    return {
      //已满足条件支付服务费数量 ,
      paycount: "",
      //领取手机数量
      receivecount: "",
      activityid:getUrlParams('activityid')
    };
  },
  created() {
    this.GetOctoberActivityCount();
  },
  mounted() {
    //每月活动绑定pcn帐号
    if(sessionStorage.OctActivityPcnUser && sessionStorage.OctActivityPcnUser != 'null'){
      this.BindActivityThirdparty() ;
    }
  },
  methods: {
      //每月活动绑定pcn帐号
    async BindActivityThirdparty(){
      let data = {
        ...this.$global.userInfo,
        pcnaccount : sessionStorage.OctActivityPcnUser,
        activityid:this.activityid
      }
      let res = await BindActivityThirdparty(data);
      if( res.result > 0){
        sessionStorage.OctActivityPcnUser = null ;
      }
    },
    //获取十月送手机活动的领取手机和支付服务费的数量
    async GetOctoberActivityCount() {
      let res = await GetOctoberActivityCount(this.$global.userInfo,this.activityid);
      if (res.result <= 0) {
        this.$toast(res.message);
        return;
      }
      let { paycount, receivecount } = res.data;
      this.paycount = paycount;
      this.receivecount = receivecount;
    },
    //?提示
    hint(num) {
      let title, message;
      if (num) {
        title = "获得资格";
        message =
          "当前账号已获得资格的活动，您可选择其中一个支付100DOS服务费，缴费期间双方服务费支付完成，即可获得手机。";
      } else {
        title = "满足条件";
        message =
          "下级代理人/合伙人已满足活动条件，仅支付200DOS服务费，缴费期间双方服务费支付完成，代理人/合伙人即可获得手机。";
      }
      Dialog.alert({
        title,
        message,
        className: "OctAlert",
        confirmButtonText: "知道了"
      }).then(() => {
        // on close
      });
    },
    async GetUserOpens(index) {
      let data = {  
        ...this.$global.userInfo,
        activityid:this.activityid
      } 
      let res = await HasBindActivityThirdparty(data);

      if (res.result > 0) {
        //绑定
          // this.$toast("请于活动结束后，10.21至10.25前来支付服务费");
          this.$router.push({
            path: "/OctActivityList",
            query: { tabsIndex: index }
          });
      } else {
         Dialog.confirm({
            className: "OctAlert",
            title: "请绑定PCN账户",
            message: "<p> 缴费前需要先绑定PCN的账户</p>",
            confirmButtonText: "立即绑定",
            cancelButtonText: "取消"
          })
            .then(() => {
              // AppNative.blJsTunedupNativeWithTypeParamSign(1013,'','');
              this.$router.push('/bindpcn');
            })
            .catch(() => {
              // on cancel
            });
      }
    },
    //开始缴费按钮
    pay() {
      this.GetUserOpens();
    }
  },
  components: {
    [Dialog.Component.name]: Dialog.Component
  }
};
</script>
<style lang="scss" scoped>
  @supports (bottom: env(safe-area-inset-bottom)) {
    .footer {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
.footer {
  position: fixed;
  bottom: 0;
  // height: 1.9rem;
  width: 100%;
  padding: 0 0.4rem;
  padding-top: 0.3rem;
  box-sizing: border-box;
  border-radius: 0.4rem 0.4rem 0 0;
  background-image: linear-gradient(180deg, #ff9782 0%, #ff3240 100%);
  box-shadow: 0rem -0.03rem 0.08rem 0rem rgba(204, 36, 37, 0.25);
  .btns {

    padding-top: 0.15rem;
    padding-bottom:.15rem;
    display: flex;
    justify-content: space-between;
    & > div {
      width: 44%;
      display: flex;
      button {
        width: 100%;
        height: 0.8rem;
        background-image: linear-gradient(180deg, #ffe8a8 0%, #fed84b 100%);
        box-shadow: 0rem 0.04rem 0.15rem 0rem #a71d25;
        border-radius: 0.12rem;
        border: solid 0.03rem #ffffdd;
        font-family: PingFangSC-Medium;
        font-size: 0.32rem;
        font-weight: bold;
        font-stretch: normal;
        line-height: 0.36rem;
        letter-spacing: 0rem;
        color: #7a1c23;
      }
      .getPhone {
        background-image: none;
        background-color: #ff4453 !important;
        box-shadow: 0rem 0.04rem 0.15rem 0rem #a71d25;
        border-radius: 0.12rem;
        border: solid 0.03rem #ffffdd;
        color: #fff0b8;
      }
    }
  }
  .hint {
    display: flex;
    justify-content: space-between;
    & > div {
      width: 44%;
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-family: PingFangSC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #fff0b8;
      img {
        width: 0.32rem;
        height: 0.32rem;
      }
    }
  }
}
.content {
  height: 100%;
  overflow-y: scroll;
  background-color: #fe7d5a;
  -webkit-overflow-scrolling: touch;
  img {
    width: 100%;
    height: 100%;
  }
}
.content::-webkit-scrollbar {
  display: none;
}
.banner {
  height: 6.2rem;
}
.container {
  background-color: #fe7d5a;
  padding-left: 0.54rem;
  padding-right: 0.52rem;
  p {
    margin-top: 0.2rem;
    font-family: PingFangSC-Regular;
    font-size: 0.26rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #4a4a4a;
  }
  h3 {
    font-family: PingFangSC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.32rem;
    letter-spacing: 0rem;
    color: #fe7d5a;
  }
  .info {
    background: #fff;
    padding: 0.5rem 0.3rem 0.4rem 0.4rem;
    border-radius: 0 0 0.12rem 0.12rem;
  }
  .titleImg {
    width: 5rem;
    height: 0.5rem;
    margin: 0 auto;
  }
}
// .submit {
//   position: fixed;
//   bottom: 0;
//   width: 100%;
//   height: 1.2rem;
//   padding: 0 0.5rem;
//   box-sizing: border-box;
//   display: flex;
//   align-items: center;
//   background-image: linear-gradient(180deg, #ff9782 0%, #ff3240 100%);
//   box-shadow: 0rem -0.03rem 0.08rem 0rem rgba(204, 36, 37, 0.25);
//   button {
//     width: 100%;
//     height: 0.8rem;
//     background-image: linear-gradient(180deg, #ffe8a8 0%, #fed84b 100%);
//     box-shadow: 0rem 0.04rem 0.15rem 0rem #a71d25;
//     border-radius: 0.12rem;
//     border: solid 0.03rem #ffffdd;
//     font-family: PingFangSC-Medium;
//     font-size: 0.32rem;
//     font-weight: bold;
//     font-stretch: normal;
//     line-height: 0.36rem;
//     letter-spacing: 0rem;
//     color: #7a1c23;
//   }
// }
</style>