<!-- 钱包 -->
<template>
  <div class="wallet">
    <div class="card">
      <div class="bgImg"></div>
      <div
        class="bgFooter"
        :style="{backgroundImage:'url('+require('@/assets/images/personal_wallet_bg@2x.png')+')'}"
      ></div>
      <div class="content">
        <van-swipe
          :show-indicators="false"
          :loop="false"
          :width="width"
          @change="SwipeChange"
          ref="purseSwipe"
        >
          <table></table>
          <van-swipe-item v-for="(item,index) of purses" :key="index">
            <div ref="swipe" class="cardInfo" :style="{backgroundImage:'url('+item.bgpic+')'}">
              <!-- left -->
              <div class="left">
                <p>
                  {{$t('m.accountbalance')}}
                  <span>(</span>
                  {{item.pursename}}
                  <span>)</span>
                  <img @click="show=!show" src="@/assets/images/personal_wallet_p_issue@2x.png" alt />
                </p>
                <p class="balance">{{item.balance}}</p>
              </div>
              <!-- right -->
              <div :class="item.pursetype==4?'right SV':'right'">
                <button
                  v-if="item.pursetype==65"
                  @click="RightBottonOnClick('personal_exchargecode')"
                >{{$t('m.exchange')}}</button>
                <button
                  v-if="item.pursetype==4"
                  @click="RightBottonOnClick('home_exchange')"
                >{{$t('m.charge')}}</button>
                <button
                  v-if="item.pursetype==4"
                  @click="RightBottonOnClick('personal_tqmcode')"
                >{{$t('m.extract')}}</button>
                <button v-if="item.pursetype==-1" @click="ChargeP()">{{$t('m.charge')}}</button>
              </div>
            </div>
          </van-swipe-item>
          <table></table>
        </van-swipe>
      </div>
    </div>
    <!-- 全部 收入 支出 -->
    <div class="detail">
      <div class="detailTab">
        <span
          v-for="(item,index) of detail"
          :class="screenIndex==index &&'active'"
          @click="ScreenOnClick(index)"
          :key="index"
        >{{$t('m.'+item)}}</span>
      </div>
      <div
        v-if="!ScreenPurseHisList||ScreenPurseHisList.length==0"
        class="empty"
        :style="{backgroundImage:'url('+require('@/assets/images/nodata@2x.png')+')'}"
      >当前页没有内容</div>

      <!-- 列表 -->
      <van-list
        v-if="ScreenPurseHisList&&ScreenPurseHisList.length>0"
        class="detailList"
        v-model="loading"
        :finished="finished"
        finished-text="没有更多了"
        @load="onLoad()"
        :offset="20"
      >
        <div class="detailInfo" v-for="(item,index) of ScreenPurseHisList" :key="index">
          <div class="detailImg">
            <img :src="GetPurseHisTypeIcon(item.typeid)" alt />
            <span>{{item.remark}}</span>
          </div>
          <div class="detailTime">
            <p :class="item.amount < 0 && 'priceColor'">{{Amount(item.amount)}}</p>
            <p>{{item.createtime}}</p>
          </div>
        </div>
      </van-list>
    </div>

    <van-popup v-model="show">
      <div class="model">
        <div class="modelT">
          <img src alt />
          {{$t('m.whatvp')}}
          <img
            @click="show=false"
            src="@/assets/images/dynamic_delete@2x.png"
            alt
          />
        </div>
        <p>{{$t('m.V')}}</p>
        <p>{{$t('m.P')}}</p>
      </div>
    </van-popup>
  </div>
</template>

<script>
import { GetPurses, GetPurseHis, GetPurseHisTypeLogo } from "@/api/myData.js";
import { GetConfig } from "@/api/sysRequest.js";
import { Base64 } from "@/config/utils.js";
export default {
  data() {
    return {
      list: [],
      loading: false,
      finished: false,

      purses: [],
      purseHisList: [],
      purseHisTypeLogo: [],
      // PV是什么? 弹出框
      show: false,
      //轮播item宽度
      width: 0,
      //全部,收入,支出,
      detail: ["all", "income", "pay"],
      purseid: 0,
      pageindex: 1,
      pagesize: 20,
      screenIndex: 0, //全部，收入，支出索引
      myPurseMenu: []
    };
  },
  methods: {
    onLoad() {
      // 异步更新数据
      setTimeout(() => {
        this.pageindex = this.pageindex + 1;
        this.GetPurseHis(() => {
          // 加载状态结束
          this.loading = false;
        });
      }, 500);
    },

    RightBottonOnClick: function(myPurseMenu_FunName) {
      for (let index = 0; index < this.myPurseMenu.length; index++) {
        if (this.myPurseMenu[index].funName == myPurseMenu_FunName) {
          if (this.myPurseMenu[index].url) {
            window.location.href = this.myPurseMenu[index].url;
          }
          break;
        }
      }
    },
    ChargeP: function() {
      this.$router.push({ name: "AddV" });
    },
    //全部，收入，支出点击事件
    ScreenOnClick: function(index) {
      this.screenIndex = index;
    },
    SwipeChange: function(index) {
      //index 为当前轮播图片下标
      this.purseid = this.purses[index].purseid;
      this.pageindex = 1;
      this.finished = false;
      this.GetPurseHis();
    },
    GetPurseHis: function(callback) {
      GetPurseHis(
        {
          purseid: this.purseid,
          pageindex: this.pageindex,
          pagesize: this.pagesize
        },
        data => {
          if (data.data < 1) {
            this.$toast("数据加载失败");
            setTimeout(() => {
              // this.$router.go(-1);
            }, 500);
            return;
          }
          if (this.pageindex == 1) {
            this.purseHisList = data.data;
          } else {
            this.purseHisList = this.purseHisList.concat(data.data);
          }
          if (data.data.length < this.pagesize) {
            this.finished = true;
          }
          if (callback) {
            callback();
          }
        }
      );
    },
    GetPurseHisTypeIcon: function(typeid) {
      for (let index = 0; index < this.purseHisTypeLogo.length; index++) {
        if (this.purseHisTypeLogo[index].typeid == typeid) {
          return this.purseHisTypeLogo[index].iconurl;
        }
      }
    },
    Amount: function(amount) {
      return amount > 0 ? "+" + amount : amount;
    }
  },
  mounted() {
    //图片宽度6.4
    this.width =
      getComputedStyle(window.document.documentElement)["font-size"].slice(
        0,
        -2
      ) * 6.7;
  },
  computed: {
    ScreenPurseHisList: function() {
      if (this.screenIndex == 0) {
        return this.purseHisList;
      }
      if (this.screenIndex == 1) {
        return this.purseHisList.filter(function(e) {
          return e.amount >= 0;
        });
      }
      if (this.screenIndex == 2) {
        return this.purseHisList.filter(function(e) {
          return e.amount < 0;
        });
      }
    }
  },
  updated() {
    if (document.getElementsByClassName("detailList ")[0]) {
      document.getElementsByClassName("detailList ")[0].style.height =
        document.body.offsetHeight -
        document.getElementsByClassName("detailList ")[0].offsetTop +
        "px";
    }
  },
  created() {
    this.purseid = this.$route.query.purseid;
    let swipIndex = 0;
    GetPurses(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.purses = data.data;
      for (let index = 0; index < this.purses.length; index++) {
        if (this.purses[index].purseid == this.purseid) {
          swipIndex = index;
          break;
        }
      }
      this.purseid = this.purses[swipIndex].purseid;
      if (swipIndex == 0) {
        this.GetPurseHis();
      } else {
        this.$refs.purseSwipe.swipeTo(swipIndex);
      }
    });
    GetPurseHisTypeLogo(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.purseHisTypeLogo = data.data;
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
.empty {
  height: 4.2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  background-size: 100% 100%;
  margin-top: 1rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.61rem;
  letter-spacing: -0.01rem;
  color: #666666;
}

.cardInfo {
  width: 6.4rem;
  height: 3.4rem;
  background-color: #ff9650;
  border-radius: 0.2rem;
  background-size: 100% 100%;
  padding: 0 0.5rem;
  display: flex;
  justify-content: space-between;
  box-sizing: border-box;

  .right {
    display: flex;
    margin-bottom: 1rem;
    align-items: flex-end;
    position: absolute;
    right: 0.5rem;
    bottom: 0;
    button {
      display: flex;
      border: 0;
      height: 0.48rem;
      background-color: #ffffff;
      border-radius: 0.24rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ff9650;
      padding: 0 0.35rem;
      white-space: nowrap;
    }
  }

  .left {
    padding-top: 0.85rem;

    p {
      margin: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      display: flex;
      align-items: center;
      white-space: nowrap;
      span {
        display: flex;
        align-items: center;
      }

      span:first-child {
        margin-left: 0.1rem;
      }

      img {
        position: relative;
        top: -0.15rem;
        /* left:.15rem; */
        width: 0.3rem;
        height: 0.3rem;
      }
    }

    .balance {
      padding-top: 0.3rem;
      font-family: PingFang-SC-Regular;
      font-size: 0.48rem;
    }
  }
}

.SV {
  flex-direction: column;
  justify-content: flex-end;

  button {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 1.25rem;
    font-size: 0.24rem !important;
    height: 0.48rem;
    border-radius: 0.24rem;
    border: solid 0.02rem #ffffff;
    color: #333333 !important;
    background-color: #ffffff;
    padding: 0 !important;
  }

  button:last-child {
    margin-top: 0.2rem;
    border: solid 0.02rem #ffffff;
    color: #ffffff !important;
    background: none;
  }
}

/deep/ .van-swipe__track {
  display: flex;
  padding: 0 0.6rem;
  box-sizing: border-box;
}

/deep/ .van-swipe-item {
  display: flex;
  /* margin: 0 .15rem; */
  width: 6.4rem !important;
  align-items: flex-end;
  justify-content: center;
  /* margin-left: .3rem; */
}

/deep/ .van-swipe-item:nth-of-type(n + 2) {
  margin-left: 0.3rem;
}

/deep/ .van-swipe-item:last-child {
  margin-right: 0.3rem;
}

.priceColor {
  color: #ff9000 !important;
}

.active {
  color: #2ea2fa !important;
}

.detailList,
.van-list {
  overflow: scroll;
  -webkit-overflow-scrolling: touch;
}
.detailList::-webkit-scrollbar,
.van-list::-webkit-scrollbar {
  display: none;
}
.modelT {
  padding: 0 0.2rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  display: flex;
  justify-content: space-between;
  align-items: center;

  img {
    width: 0.22rem;
    height: 0.22rem;
  }
}

.van-popup {
  border-radius: 0.1rem;
}

.model {
  width: 5.8rem;
  height: 3.55rem;
  background-color: #ffffff;
  border-radius: 0.1rem;
  padding: 0.32rem 0.24rem;
  box-sizing: border-box;
  p {
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;
  }
}

.detailImg {
  display: flex;
  align-items: center;

  img {
    width: 0.5rem;
    height: 0.5rem;
    margin-right: 0.25rem;
  }
  span {
    white-space: nowrap;
    width: 2.2rem;
    text-overflow: ellipsis;
    overflow: hidden;
  }
}

.detailTime {
  justify-content: center;
  display: flex;
  flex-direction: column;
  height: 100%;
  padding-right: 0.3rem;

  p {
    margin: 0;
    color: #000;
    text-align: right;
  }

  p:last-child {
    font-family: PingFang-SC-Regular;
    font-size: 0.2rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.detailInfo {
  border-bottom: 1px solid #ddd;
  height: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #1a1a1a;
}

.detail {
  padding: 1rem 0.3rem;
  padding-bottom: 0;
  .detailTab {
    height: 0.7rem;
    display: flex;
    align-items: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;

    span {
      padding-left: 0.8rem;
    }

    span:nth-child(1) {
      padding-left: 0;
    }
  }
}

.card {
  position: relative;

  .cardList {
    width: 10000px;
    display: flex;
    overflow-x: scroll;
  }

  .content {
    overflow-x: scroll;
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    display: flex;
    flex-direction: column;
    justify-content: flex-end;
    /* padding-left: .55rem; */
    box-sizing: border-box;
  }

  .bgImg {
    height: 3.34rem;
    width: 100%;
    box-sizing: border-box;
    background-color: #2ea2fa;
  }

  .bgFooter {
    width: 100%;
    background-size: 100% 100%;
    height: 0.77rem;
  }
}
</style>
