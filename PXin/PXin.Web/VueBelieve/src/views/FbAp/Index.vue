<template>
  <div class="home" :style="$global.isYouGu?'padding-top:-46px':''" v-if="permission">
    <table></table>
    <div
      class="bgBox pt46"
      :style="{backgroundImage:'url('+require('@/assets/images/chongzhi_bg.png')+')'}"
    >
      <!-- 通告 -->
      <div class="notice">
        <div class="noticeIMG">
          <img src="@/assets/images/icon_notice.png" alt />
        </div>
        <div class="swipeImg">
          <van-swipe
            :autoplay="noticeList.length>1?2000:0"
            :loop="true"
            :vertical="true"
            :show-indicators="false"
          >
            <van-swipe-item v-for="(item,index) of noticeList" :key="index">
              <router-link to="/notice">{{item.title}}</router-link>
            </van-swipe-item>
          </van-swipe>
        </div>
        <div class="arrows">
          <img src="@/assets/images/icon_arrows.png" alt />
        </div>
      </div>

      <div class="address">{{userJxs.regiondesc}}</div>
      <div class="username">
        {{userJxs.name==null?'':userJxs.name.length>6?userJxs.name.slice(0,6)+"...":userJxs.name}}
        <img
          @click="$router.push({path:'/rename',query:{name:userJxs.name,infoid:userJxs.infoid}})"
          src="@/assets/images/fbap_revise.png"
          alt
        />
      </div>
      <div class="dealer">
        {{userJxs.typeiddesc}}
        <span v-show="userJxs.statusdesc">({{userJxs.statusdesc}}）</span>
      </div>
      <div class="time">{{$t('lang.FBAP_INDEX')}} : {{userJxs.endtime}}</div>
      <div class="btnBox">
        <router-link
          v-show="userJxs.isshowrenew"
          v-if="userJxs.isczs"
          :to="{path:'/Renew',query:{userJxs:JSON.stringify(userJxs),infoid:JSON.stringify(userJxs.infoid)}}"
          tag="div"
          :class="userJxs.isshowrenew?'bt1bor bt1':'bt1bor'"
        >{{$t('lang.FBAP_Renewals')}}</router-link>
        <div @click="stock(userJxs)" class="bt2">{{$t('lang.FBAP_purchase')}}</div>
      </div>
    </div>
    <div class="content">
      <div class="relative">
        <div class="SVcount">
          <router-link
            :to="{path:'/RetailStockRecord',query:{infoId:JSON.stringify(userJxs.infoid)}}"
          >
            <span>SV{{$t('lang.FBAP_RetailInventory')}}</span>
            <span>({{userJxs.retailcodestock}})</span>
          </router-link>
          <div>
            <button @click="$router.push({path:'/Retail'})">{{$t('lang.FBAP_Retail')}}</button>
          </div>
        </div>
        <div class="SVcount" v-if="userJxs.isczs">
          <router-link
            :to="{path:'/WholesaleStockRecord',query:{infoId:JSON.stringify(userJxs.infoid)}}"
          >
            <span>SV{{$t('lang.FBAP_WholesaleInventory')}}</span>
            <span>({{userJxs.wholesalecodestock}})</span>
          </router-link>
        </div>
        <div @click="togenerate" class="SVcount" v-if="userJxs.isczs">
          <div>
            <span>SV{{$t('lang.FBAP_generateRechargeCode')}}</span>
            <span>({{ svnum }})</span>
          </div>
        </div>
        <router-link
          :to="{path:'/StockRecord',query:{infoId:JSON.stringify(userJxs.infoid)}}"
          tag="div"
          class="stock"
        >
          <div class="t">
            <div class="title">{{$t('lang.FBAP_90DaysCumulativePurchase')}}</div>
          </div>
          <div class="n">
            <span>{{userJxs.inputstock}}次</span>
          </div>
        </router-link>
        <!-- <p>温馨提示：每90天内最少进货{{userJxs.leaststock}}个零售码，否则将会冻结</p> -->
        <p
          style="padding:0 .3rem;padding-top:.1rem;font-size: .2rem; color: #ff3030;"
        >温馨提示：90天内最低进货SV零售码库存{{userJxs.leaststock}}次,不足3次将被冻结</p>

        <router-link
          v-if="userJxs.isczs"
          :to="{path:'/FbApList',query:{infoid: userJxs.infoid}}"
          tag="div"
          class="ops"
        >
          <div class="tit">{{$t('lang.FBAP_myAgent')}}</div>
        </router-link>
        <router-link
          v-if="!userJxs.isczs"
          :to="{path:'/changeInfo',query:{key: userJxs.pmobileno, infoid: userJxs.infoid}}"
          tag="div"
          class="ops"
        >
          <div class="tit">{{$t('lang.FBAP_myRechargeableQuotient')}}</div>
        </router-link>
        <div class="ops">
          <router-link
            tag="div"
            :to="{path:'/IdentityIndex',query:{infoid:JSON.stringify(userJxs.infoid)}}"
            class="tit"
          >{{$t('lang.FBAP_Authentication')}}</router-link>
        </div>
        <div class="ops" v-if="userJxs.isczs" @click="toAgent">
          <div class="tit">{{$t('lang.FBAP_openRechargeQuotient')}}</div>
        </div>
        <div class="ops" v-if="!userJxs.isczs">
          <router-link
            tag="div"
            :to="{path:'/ApplyCzs'}"
            class="tit"
          >{{$t('lang.FBAP_upgradeToRecharger')}}</router-link>
        </div>
        <div class="ops" v-if="userJxs.isczs">
          <router-link
            tag="div"
            :to="{path:'/Car',query:{infoId:userJxs.infoid,statusdesc:userJxs.statusdesc}}"
            class="tit"
          >充值商配车</router-link>
        </div>
      </div>
    </div>

    <!-- 预审通过 -->

    <van-popup v-model="show">
      <div class="audit">
        <div class="close">
          <img @click="show=false" src="@/assets/images/pop_up_delete.png" alt />
        </div>
        <div class="hintIMG">
          <img src="@/assets/images/icon_success.png" alt />
        </div>
        <h3>{{$t('lang.FBAP_Congratulations')}}</h3>
        <div class="hinttext">{{$t('lang.FBAP_hinttext')}}</div>
        <div class="submit">
          <button
            @click="$router.push({path:'/IdentityIndex',query:{infoid:JSON.stringify(userJxs.infoid)}})"
          >{{$t('lang.FBAP_perfectInformation')}}</button>
        </div>
      </div>
    </van-popup>
  </div>
</template>

<script>
import {
  GetUserJxs,
  GetFbapInitPage,
  GetMeetInfos,
  CheckOpenCzs
} from "@/api/getFbApData";
import { setStore } from "@/config/utils";
export default {
  data() {
    return {
      //通告List
      noticeList: [],
      show: false,
      userJxs: {},
      isexpire: false,
      pisexpire: false,
      permission: false
    };
  },
  async created() {
    await this.GetFbapInitPage();
    await this.GetMeetInfos();
  },
  computed: {
    svnum() {
      var num = this.userJxs.svbalance;
      num = num.toString();
      let index = num.indexOf(".");
      if (index !== -1) {
        num = num.substring(0, 2 + index + 1);
      } else {
        num = num.substring(0);
      }
      return parseFloat(num).toFixed(2);
    }
  },
  methods: {
    async GetMeetInfos() {
      let res = await GetMeetInfos({
        ...JSON.parse(sessionStorage.userParam),
        typeid: 1
      });
      if (res.result > 0) {
        this.noticeList = res.data;
      }
    },
    async GetFbapInitPage() {
      let res = await GetFbapInitPage(JSON.parse(sessionStorage.userParam));
      if (res.result > 0) {
        if (res.data.type == 1) {
          this.$router.push("/ApplyCzs");
        } else if (res.data.type == 2) {
          this.$router.push("/AgentReq");
        } else {
          this.permission = true;
          this.GetUserJxs();
        }
      } else {
        this.Toast(res.message);
      }
    },
    //获取(充值商/代理人)信息
    async GetUserJxs() {
      let result = await GetUserJxs(JSON.parse(sessionStorage.userParam));
      if (result.result > 0) {
        this.userJxs = result.data;
        if (result.data.typeid == 4 && result.data.statusdesc == "未上传资料")
          this.show = true;
        //存储充值商信息
        setStore("FbApInfo", result.data);
        this.pisexpire = result.data.pisexpire;
        if (result.data.isexpire) {
          this.isexpire = result.data.isexpire;
          if (result.data.typeid == 5) {
            this.Toast(this.$t("lang.FBAP_exceed"));
          } else if (result.data.typeid <= 4) {
            this.Toast(this.$t("lang.FBAP_Beoverdue"));
          }
        }
      } else {
        this.Toast(result.message);
        setTimeout(() => {
          this.$router.go(-1);
        }, 2000);
      }
    },
    stock(user) {
      let status = this.userJxs.status + this.userJxs.statustwo;
      if (status != 1) {
        let msg = this.$t("lang.FBAP_noauthority");
        if (this.userJxs.status == 2) {
          msg = "您当前状态为审核未通过,不能操作";
        } else {
          if (this.userJxs.statustwo != 0) {
            msg = "您当前状态为冻结,不能操作";
          } else {
            if (this.userJxs.typeid == 4) {
              msg = this.$t("lang.FBAP_msg");
            }
          }
        }
        this.Dialog.alert({
          message: msg
        }).then(() => {});
        return;
      }
      if (this.isexpire) {
        this.Toast.fail(this.userJxs.typeiddesc + this.$t("lang.FBAP_overdue"));
        return;
      }
      if (this.pisexpire) {
        this.Toast.fail("上级充值商已过期,请联系充值商续费");
        return;
      }
      this.$router.push({
        path: "/Stockin",
        query: {
          statusdesc: JSON.stringify(this.userJxs.statusdesc),
          infoid: JSON.stringify(this.userJxs.infoid)
        }
      });
    },
    copyFn(e) {
      var copyDOM = document.getElementById("txt"); //要复制文字的节点
      var range = document.createRange(); //创建一个range
      window.getSelection().removeAllRanges(); //清楚页面中已有的selection
      range.selectNode(copyDOM); // 选中需要复制的节点
      window.getSelection().addRange(range); // 执行选中元素
      var successful = document.execCommand("copy"); // 执行 copy 操作
      if (successful) {
        this.Toast("复制成功");
      }
      // 移除选中的元素
      window.getSelection().removeAllRanges();
    },
    async toAgent() {
      let result = await CheckOpenCzs(JSON.parse(sessionStorage.userParam));
      if (result.result > 0) {
        this.$router.push("/agent");
      } else {
        this.Toast.fail(result.message);
      }
    },
    togenerate() {
      let status = this.userJxs.status + this.userJxs.statustwo;
      if (status != 1) {
        let msg = this.$t("lang.FBAP_noauthority");
        if (this.userJxs.status == 2) {
          msg = "您当前状态为审核未通过,不能操作";
        } else {
          if (this.userJxs.statustwo != 0) {
            msg = "您当前状态为冻结,不能操作";
          } else {
            if (this.userJxs.typeid == 4) {
              msg = this.$t("lang.FBAP_msg");
            }
          }
        }
        this.Toast.fail(msg);
        return;
      }
      if (this.isexpire) {
        this.Toast.fail(this.userJxs.typeiddesc + this.$t("lang.FBAP_overdue"));
        return;
      }
      if (this.userJxs.binduestatus == -2) {
        this.$router.push("/BindAccount");
        return;
      }
      !this.userJxs.isgeneratecode
        ? this.Toast.fail(this.$t("lang.FBAP_exceed"))
        : this.$router.push({
            path: "/generate",
            query: { todaycount: this.userJxs.todaycount }
          });
    }
  }
};
</script>

<style lang="scss" scoped>
.notice {
  display: flex;
  box-sizing: border-box;
  padding: 0 0.3rem;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  height: 0.68rem;
  margin-bottom: 0.3rem;
  background: rgba(139, 205, 255, 0.5);

  img {
    width: auto;
    height: auto;
    max-width: 100%;
    max-height: 100%;
  }
  .noticeIMG {
    width: 0.48rem;
    height: 0.4rem;
  }
  .arrows {
    width: 0.16rem;
    height: 0.32rem;
  }
  .swipeImg {
    flex: 1;
    padding: 0 0.25rem;
    height: 100%;
    .van-swipe {
      height: 100%;
      .van-swipe-item {
        line-height: 0.68rem;
        height: 0.68rem;
        a {
          width: 100%;
          display: inline-block;
          overflow: hidden;
          white-space: nowrap;
          text-overflow: ellipsis;
          font-family: PingFang-SC-Bold;
          font-size: 0.3rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0.01rem;
          color: #ffffff;
        }
      }
    }
  }
}

.audit .submit {
  padding-top: 1.2rem;
  margin: 0 auto;
  width: 3.6rem;
  height: 0.74rem;
  button {
    width: 100%;
    height: 100%;
    border: 0;
    border-radius: 0.04rem;
    background-color: #2ea2fa;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}
.hinttext {
  padding: 0 0.35rem;
  box-sizing: border-box;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.36rem;
  letter-spacing: 0rem;
  color: #999999;
}
.audit h3 {
  margin-bottom: 0.2rem;
  text-align: center;
  font-family: PingFang-SC-Bold;
  font-size: 0.36rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}
.hintIMG {
  display: flex;
  justify-content: center;
  img {
    margin: 0 auto;
    width: 0.9rem;
    height: 0.9rem;
  }
}
.close {
  width: 100%;
  padding-top: 0.2rem;
  padding-right: 0.2rem;
  box-sizing: border-box;
  text-align: right;
  img {
    width: 0.36rem;
    height: 0.36rem;
  }
}
/deep/ .van-overlay {
  background-color: rgba(0, 0, 0, 0.5);
}
.van-popup {
  border-radius: 0.12rem;
}
.audit {
  // padding: 0 0.35rem;
  width: 6.5rem;
  box-sizing: border-box;
  height: 6.65rem;
  background: #fff;
}
.relative {
  position: relative;
  top: -0.6rem;
}

.t {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.t p {
  margin: 0 !important;
}

.SVcount {
  width: 100%;
  height: 1.28rem;
  background-color: #ffffff;
  border-radius: 0.06rem;
  box-sizing: border-box;
  display: flex;
  padding: 0 0.3rem;
  justify-content: space-between;
  align-items: center;
  position: relative;

  span {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #666666;
  }

  span:first-child {
    color: #2ea2fa;
  }

  button {
    width: 1.2rem;
    border: 0;
    height: 0.58rem;
    line-height: 0.58rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
  }
}
.SVcount:nth-of-type(n + 2) {
  margin-top: 0.2rem;
}

[class*="van-hairline"]::after {
  border: none;
}
.home::-webkit-scrollbar {
  display: none;
}
.home {
  height: 100%;
  // padding-bottom: 20px;
  overflow-y: scroll;
  background: #f4f4f4;
  -webkit-overflow-scrolling: touch;
  .bgBox {
    color: #fff;
    padding-top: 0.3rem;
    background-size: 100% 100%;
    display: flex;
    flex-direction: column;
    align-items: center;

    // padding-top: 50px;
    .header {
      width: 100%;
      box-sizing: border-box;
      background: rgba(255, 255, 255, 0);
    }

    .address {
      font-size: 0.2rem;
      font-family: PingFang-SC-Medium;
      font-weight: 500;
      color: rgba(255, 255, 255, 1);
    }

    .username {
      font-size: 0.36rem;
      padding: 0.2rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;

      img {
        width: 0.3rem;
        height: 0.3rem;
        margin-left: 0.2rem;
      }
    }

    .dealer {
      font-size: 0.36rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;

      span {
        color: #ff3030;
        font-weight: normal;
        margin-left: 0.1rem;
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(255, 48, 48, 1);
      }
    }

    .time {
      padding: 0.2rem 0 0.3rem 0;
      font-size: 0.2rem;
      font-family: PingFang-SC-Medium;
      font-weight: 500;
      color: rgba(255, 255, 255, 1);
    }

    .btnBox {
      width: 100%;
      display: flex;
      justify-content: center;
      padding-bottom: 0.57rem;

      div {
        width: 1.8rem;
        height: 0.58rem;
        border-radius: 0.9rem;
        outline: none;
        text-align: center;
        line-height: 0.58rem;
        border-radius: 0.9rem;
      }

      .bt1bor {
        border: 1px solid #fff;
      }

      .bt1 {
        margin-right: 1.6rem;
      }

      .bt2 {
        background: #fff;
        color: #2ea2fa;
      }
    }
  }

  .content {
    background: #f4f4f4;
    padding: 0.3rem;
    padding-bottom: 0;

    .numBox {
      background: #fff;
      display: flex;
      text-align: center;
      margin-top: -0.6rem;
      border-radius: 0.06rem;

      .lft {
        width: 50%;
        box-sizing: border-box;
        border-right: 1px solid #d1d1d1;
        margin: 0.3rem 0 0.2rem 0;

        .tit {
          color: #999;
          margin-top: 0.2rem;
        }

        .num {
          font-size: 0.36rem;
        }
      }

      .rgt {
        width: 50%;
        margin: 0.3rem 0 0.2rem 0;

        .tit {
          color: #999;
          margin-top: 0.2rem;
        }

        .num {
          font-size: 0.36rem;
        }
      }
    }

    .stock {
      display: flex;
      background: #fff;
      margin-top: 0.2rem;
      padding: 0.34rem 0.3rem;

      .t {
        flex: auto;

        .title {
          color: #5194e7;
        }
      }

      .n {
        display: flex;
        justify-content: center;
        align-items: center;
        color: #666666;

        span {
          white-space: nowrap;
        }
      }
    }

    .ops {
      background: #fff;
      margin-top: 0.2rem;
      padding: 0.34rem 0.3rem;
      font-size: 0.3rem;
      color: #5194e7;
    }

    .copyBox {
      display: flex;
      background: #fff;
      margin-top: 0.2rem;
      border-radius: 0.1rem;
      overflow: hidden;

      .txt {
        flex: auto;
        padding: 0.2rem 0.24rem;
        color: #999999;

        .text {
          font-size: 0.24rem;
        }

        p {
          margin-top: 0.1rem;
        }
      }

      .copyBtn {
        background: #5194e7;
        display: flex;
        align-items: center;
        color: #fff;
        font-size: 0.24rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(255, 255, 255, 1);
        padding: 0 0.2rem;
        text-align: center;
      }
    }
  }
}
</style>
