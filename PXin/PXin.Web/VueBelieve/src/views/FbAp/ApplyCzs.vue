<template>
  <div class="apply">
    <div class="topIMG">
      <img src="@/assets/images/bg.png" alt />
    </div>
    <div class="applyCzs">
      <h3>{{$t('lang.ApplyCzs_Application')}}</h3>
      <div class="userInfo">
        <div>
          <span>{{$t('lang.ApplyCzs_userName')}}</span>
          <span>{{userinfo.nodename}}</span>
        </div>
        <div>
          <span>{{$t('lang.ApplyCzs_userAccount')}}</span>
          <span>{{userinfo.nodecode}}</span>
        </div>
        <div>
          <span>{{$t("lang.ApplyCzs_purchaseCost")}}</span>
          <span>5000DOS</span>
        </div>
      </div>
      <!-- 地区 -->
      <div class="region">
        <div>{{$t('lang.agent_region')}}</div>
        <div class="regionInfo" @click="show=true">
          <span v-show="!region.length">{{$t('lang.agent_selectregion')}}</span>
          <span
            v-if="region.length"
            style="text-align:right; width: 4rem; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;"
          >{{region[0].name}}{{region[1].name}}{{region[2].name}}</span>
          <span class="arrow"></span>
        </div>
      </div>
      <!-- 按钮 -->
      <div class="footer">
        <div class="submit">
          <button @click="applypay">{{$t('lang.ApplyCzs_determinePay')}}</button>
        </div>
        <div class="agree" @click="isChecked=!isChecked">
          <img
            :src="isChecked?require('@/assets/images/select_sel.png'):require('@/assets/images/select_nor.png')"
            alt
          />
          <span>{{$t('lang.ApplyCzs_readAgree')}}</span>
          <router-link to="/signing">《{{$t('lang.ApplyCzs_Agreement')}}》</router-link>
        </div>
      </div>
      <!-- 提示 -->
      <div class="hint">
        <h4>{{$t('lang.ApplyCzs_Reminder')}}:</h4>
        <p>1.{{$t('lang.ApplyCzs_hint1')}}</p>
        <p>2.{{$t('lang.ApplyCzs_hint2')}}</p>
      </div>
    </div>
    <van-popup position="bottom" v-model="show">
      <van-area :area-list="areaList" @cancel="show=false" @confirm="confirm" />
    </van-popup>
  </div>
</template>
<script>
import area from "@/api/Area";
import { ApplyFbap, SearchUser } from "@/api/getFbApData";
export default {
  data() {
    return {
      show: false,
      isChecked: false,
      areaList: area,
      region: [],
      userinfo: {},
      isapply: false, //是否已经申请
    };
  },
  mounted() {
    this.SearchUser() ;
    this.dosPayResult() ; //dos 回调
  },
  methods: {
    //dos 回调
    dosPayResult(){
      let _this = this;
      window.dosPayResult = function(obj) {
        try {
          var ret = JSON.parse(obj);
          if (ret.result == undefined) {
            throw new Error("解析错误");
            // throw new Error(this.$t('lang.add_Parsingerror'));
          }
          if (ret.result <= 0) {
            _this.Toast(ret.message);
            return;
          } else {
            _this.isapply = true;
            _this.Toast("申请成功");
            // _this.Toast(this.$t('lang.agent_success'));
            let returnCount=-1;
            if(_this.$route.query.flag){
              returnCount=-2;
            }
            setTimeout(() => {
              _this.$router.go(returnCount);
            }, 1000);
          }
        } catch (e) {
          _this.Toast("支付异常:" + obj);
          // _this.Toast(this.$t('lang.add_payabnormal') + obj);
        }
      };
    },
    async SearchUser(){
      let user = JSON.parse(sessionStorage.userParam);
      let fbapInfoString = sessionStorage.getItem("FbApInfo");
      let type = 2;
      if (fbapInfoString) {
        type = 4;
      }
      let result = await SearchUser(user, user.nodeid, type);
      if (result.result > 0) {
        this.userinfo = result.data;
      } else {
        this.Toast(result.message);
      }

      if (this.$store.state.region) {
        this.region = JSON.parse(this.$store.state.region);
      }
    },
    confirm(val) {
      this.region = val;
      this.show = false;
    },
    testCon(){
      if (this.region.length == 0) {
        // this.Toast("请选择所在地区!");
        this.Toast(this.$t('lang.agent_selectregion'));
        return -1 ;
      }
      if (!this.isChecked) {
        // this.Toast("请同意相信充值商服务协议!");
        this.Toast(this.$t('lang.ApplyCzs_hint3'));
        return -1 ;
      }
      return 1 ;
    },
    async applypay() {
      if(this.testCon() == -1) return ;
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        province: this.region[0].name,
        city: this.region[1].name,
        region: this.region[2].name
      };
      let res = await ApplyFbap(data);
      if (res.result > 0) {
        try {
          AppNative.blJsTunedupNativeWithTypeParamSign(
            1003,
            res.data.chargestr,
            res.data.sign
          );
        } catch (e) {
          // this.Toast(e);
          // this.Toast("调起码库支付失败");
          this.Toast(this.$t('lang.add_ueError'));
        }
      } else {
        this.Toast(res.message);
      }
    }
  },
  beforeRouteLeave: function(to, from, next) {
    this.$store.state.region = JSON.stringify(this.region);
    let fbapInfoString = sessionStorage.getItem("FbApInfo");
    if (!this.isapply && to.name == "FbAp" && !fbapInfoString) {
      AppNative.blJsTunedupNativeWithTypeParamSign(1001, "", "");
      return;
    }
    next();
  }
};
</script>
<style lang="scss" scoped>
.hint {
  h4 {
    margin-bottom: 0;
    font-family: PingFang-SC-Bold;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;

    text-overflow: ellipsis;
    white-space: nowrap;
  }
  p {
    padding-top: 0.1rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #999999;
  }
}
.agree {
  display: flex;
  align-items: center;
  justify-content: center;
  padding-top: 0.15rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.48rem;
  letter-spacing: 0rem;
  color: #999999;
  a {
    color: #2ea2fa;
  }
  span {
    padding-left: 0.1rem;
  }
  img {
    width: 0.28rem;
    height: 0.28rem;
  }
}
.submit {
  width: 100%;
  padding-top: 1rem;
  button {
    width: 100%;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    border: 0;
    outline: none;
    height: 0.88rem;
    line-height: 0.88rem;
    text-align: center;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}
.region {
  margin-top: 0.2rem;
  height: 0.88rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background-color: #f7f7fc;
  padding: 0 0.6rem;
  font-family: PingFang-SC-Bold;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  .regionInfo {
    height: 100%;
    display: flex;
    align-items: center;
  }
  .arrow {
    width: 0.15rem;
    height: 0.15rem;
    border-top: 0.04rem solid #999;
    border-right: 0.04rem solid #999;
    transform: rotate(45deg);
    margin-left: 0.1rem;
  }
  span {
    font-family: PingFang-SC-Medium;
    color: #999999;
    white-space: nowrap;
  }
}
.userInfo {
  background-color: #f7f7fc;
  border-radius: 0.12rem;
  padding: 0.6rem;
  padding-bottom: 0.5rem;
  height: 2.64rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  & > div {
    display: flex;
    justify-content: space-between;
    font-family: PingFang-SC-Bold;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;
    width: 100%;
    span {
      &:first-child {
        flex: auto;
      }
    }
  }
}
.apply::-webkit-scrollbar {
  display: none;
}
.apply {
  height: 100%;
  overflow: scroll;
  -webkit-overflow-scrolling: touch;
  .topIMG {
    width: 100%;
    height: 3.7rem;
    img {
      width: 100%;
      height: 100%;
    }
  }

  .applyCzs {
    padding: 0 0.3rem;
    background-color: #ffffff;
    border-radius: 0.36rem 0.36rem 0 0;
    position: relative;
    top: -0.3rem;
    padding-bottom: 1rem;
    h3 {
      margin: 0;
      font-family: PingFang-SC-Bold;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      // line-height: 0.36rem;
      letter-spacing: 0rem;
      color: #333333;
      height: 1.12rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
}
</style>