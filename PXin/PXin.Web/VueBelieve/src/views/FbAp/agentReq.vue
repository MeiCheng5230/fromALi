<template>
  <div style="height:100%;">
    <div class="userList">
      <div class="userInfo" v-for="(item,index) of userJxsReqList" :key="index">
        <div class="info">
          <div>
            <img :src="item.appphoto?item.appphoto:require('@/assets/images/bg_buy_sv.png')" alt />
          </div>
          <div class="sign">
            <span>{{item.name||item.nodename}}</span>
            <span>{{$t('lang.req_Request')}}</span>
          </div>
        </div>
        <div class="btn">
          <button @click="Hint(item.name,item.nodename,item.czsnodeid)">{{$t('lang.req_Agree')}}</button>
        </div>
      </div>
    </div>
    <div class="foot">
      <router-link :to="{path:'ApplyCzs',query:{flag:'agentReq'}}" tag="button">{{$t('lang.req_Application')}}</router-link>
    </div>
  </div>
</template>

<script>
import { Dialog } from "vant";
import { GetUserJxsConfirms, AgreeUserJxsRequst } from "@/api/getFbApData";
export default {
  data() {
    return {
      userJxsReqList: [],
      agent: false
    };
  },
  mounted() {
    this.GetUserJxsConfirms() ;
  },
  components: {
    [Dialog.Component.name]: Dialog.Component
  },
  methods: {
    async GetUserJxsConfirms(){
      let res = await GetUserJxsConfirms(
        JSON.parse(sessionStorage.getItem("userParam"))
      );
      if (res.result > 0) {
        this.userJxsReqList = res.data;
      } else {
        this.Toast(res.message);
      }
    },
    async AgreeUserJxsRequst(nodeid) {
      let _this = this;
      let res = await AgreeUserJxsRequst({
        ...JSON.parse(sessionStorage.getItem("userParam")),
        czsnodeid: nodeid
      });
      if (res.result > 0) {
        // _this.Toast("您已成为代理人");
        _this.Toast(this.$t('lang.req_becomeAgent'));
        _this.agent = true;
        setTimeout(() => {
          _this.$router.go(-1);
        }, 1000);
      } else {
        _this.Toast(res.message);
      }
    },
    Hint(name, nodename, nodeid) {
      let showname = name || nodename;
      Dialog.confirm({
        title:
          "同意后将成为" +showname+
          "的下级代理人，该操作不可逆。",
        message: ""
      })
        .then(() => {
          this.AgreeUserJxsRequst(nodeid);
          // on confirm
        })
        .catch(() => {
          // on cancel
        });
    }
  },
  beforeRouteLeave: function(to, from, next) {
    if (!this.agent && to.name == "FbAp") {
      AppNative.blJsTunedupNativeWithTypeParamSign(1001, "", "");
      return;
    }
    next();
  }
};
</script>
<style lang="scss">
.van-dialog__header {
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.36rem;
  letter-spacing: 0.01rem;
  color: #333333;
  text-align: left !important;
  box-sizing: border-box;
  padding-left: 0.3rem !important;
  padding-right: 0.3rem;
}
.foot {
  width: 100%;
  position: fixed;
  bottom: 0;
  button {
    height: 0.88rem;
    background-color: #2ea2fa;
    width: 100%;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
    border: 0;
  }
}
.btn {
  padding-right: 0.3rem;
  button {
    width: 1.48rem;
    height: 0.58rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
    outline: none;
  }
}
.userList::-webkit-scrollbar {
  display: none;
}
.userInfo:last-child {
  margin-bottom: 1rem;
}
.userList {
  padding-left: 0.3rem;
  height: 100%;
  overflow: scroll;
  -webkit-overflow-scrolling: touch;
}
.userInfo {
  border-bottom: 0.01rem solid #d1d1d1;
  height: 1.68rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  .info {
    display: flex;
    align-items: center;
    flex: 1;
    .sign {
      flex: 1;
      width: 3rem;
      padding-left: 0.2rem;
      display: flex;
      flex-direction: column;
      span:first-child {
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
      span:last-child {
        font-family: PingFang-SC-Medium;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
        margin-top: 0.3rem;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
    }
  }
  img {
    width: 1rem;
    height: 1rem;
    border-radius: 50%;
  }
}
</style>