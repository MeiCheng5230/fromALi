<!-- 钱包 -->
<template>
  <div class="wallet">
    <div class="top">
      <div class="topImg">
        <img src="@/assets/images/wallet_bg@2x.png" alt />
      </div>
      <!--各类钱包-->
      <div class="content">
        <div
          v-for="(item,index) of purses"
          @click="GoWalletDetail(item.purseid)"
          v-show="item.isshow==1"
          :key="index"
          tag="div"
          class="contentInfo"
        >
          <div>
            <p>{{item.pursename}}</p>
            <p>{{item.balance}}</p>
          </div>
        </div>
      </div>
    </div>
    <div
      class="unify"
      v-for="(item,index) of myPurseMenu"
      :key="index"
      v-show="item.isDisplay"
      @click="Goto(item.url)"
    >
      <img :src="item.logo" alt />
      <span>{{item.chName}}</span>
    </div>
  </div>
</template>

<script>
import { GetPurses } from "@/api/myData.js";
import { GetConfig, GoWithParam } from "@/api/sysRequest.js";
import { Base64 } from "@/config/utils.js";
export default {
  data() {
    return {
      purses: [],
      myPurseMenu: []
    };
  },
  methods: {
    GoWalletDetail: function(purseid) {
      this.$router.push({ name: "WalletDetail", query: { purseid: purseid } });
    },
    Goto: function(url) {
      if (!url || url == "") {
        return;
      }
      GoWithParam(url);
    }
  },
  created() {
    GetPurses(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.purses = data.data;
    });
    GetConfig(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.myPurseMenu = JSON.parse(Base64.decode(data.data.mypurse_menu));
    });
  }
};
</script>

<style scoped lang='scss'>
.unify {
  padding: 0 0.4rem;
  height: 1rem;
  display: flex;
  align-items: center;
  background-color: #ffffff;
  box-shadow: 0rem 0.04rem 0.08rem 0rem #e9e9e9;
  border-radius: 0.12rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  margin-top: 0.3rem;

  img {
    width: 0.5rem;
    height: 0.5rem;
    margin-right: 0.2rem;
  }
}

.contentInfo {
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 50%;
  border-right: 0.02rem solid #fafafa;
  border-bottom: 0.02rem solid #fafafa;
  box-sizing: border-box;
  padding-left: 0.6rem;

  & > div {
    /* height: .75rem; */
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }

  p {
    margin: 0;
  }

  p:nth-child(1) {
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
  }

  p:nth-child(2) {
    font-family: PingFang-SC-Bold;
    font-size: 0.34rem;
    margin-top: 0.1rem;
  }
}

.contentInfo:nth-child(1),
.contentInfo:nth-child(2) {
  height: 1.5rem;
}

.contentInfo:nth-child(3),
.contentInfo:nth-child(4) {
  height: 1.8rem;
}

.contentInfo:nth-child(2),
.contentInfo:nth-child(4) {
  border-right: 0;
}

.contentInfo:nth-child(3),
.contentInfo:nth-child(4) {
  border-bottom: 0;
}

/*  .contentInfo:nth-child(1) {
    border-bottom: .02rem solid red;
    border-right: .02rem solid red;
  }

  .contentInfo:nth-child(4) {
    border-top: .02rem solid red;
    border-left: .02rem solid red;
  } */

.content {
  height: 3.3rem;
  display: flex;
  position: relative;
  top: -0.12rem;
  flex-wrap: wrap;
  padding-top: 0.4rem;
}

.topImg {
  position: relative;
  top: -0.12rem;
  margin: 0 auto;
  width: 1.7rem;
  height: 0.52rem;
  display: flex;

  img {
    width: 100%;
    height: 100%;
  }
}

.top {
  width: 100%;
  height: 4.1rem;
  background-color: #ffffff;
  box-shadow: 0rem 0.04rem 0.08rem 0rem #e9e9e9;
  border-radius: 0.12rem;
}

.wallet {
  height: 100%;
  background: #f2f2f2;
  padding: 0.42rem 0.3rem;
  padding-bottom: 0;
  box-sizing: border-box;
}
</style>
