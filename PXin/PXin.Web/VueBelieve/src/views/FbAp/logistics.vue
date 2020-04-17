<template>
  <div class="logistics">
    <table></table>
    <div class="top" v-show="!noDataFlag">
      <div class="logisticsImg">
        <img :src="logisticsInfo.logo" alt />
      </div>
      <div class="logisticsTitle">
        <div>{{logisticsInfo.name}}</div>
        <div>{{logisticsInfo.type}}{{logisticsInfo.no}}</div>
      </div>
    </div>
    <div class="info" v-if="!noDataFlag">
      <van-steps
        direction="vertical"
        :active="0"
        :active-icon="require('@/assets/images/ic_post_1.png')"
        :inactive-icon="require('@/assets/images/ic_post_2.png')"
      >
        <van-step v-for="(item,index) of logisticsInfo.list" :key="index">
          <h3 :class="index==0 && 'status'">{{item.content}}</h3>
          <p>{{item.time}}</p>
        </van-step>
      </van-steps>
    </div>
    <nodata v-else text="暂无快递信息"/>
  </div>
</template>
<script>
import { GetExpressInfo } from "@/api/getFbApData";
const nodata = () => import("@/components/noData");
export default {
  data() {
    return {
      active: 1,
      //物流信息对象
      logisticsInfo: {},
      noDataFlag: false,
    };
  },
  created() {
    this.GetExpressInfo();
  },
  methods: {
    async GetExpressInfo() {
      let data = {
        ...this.$global.userInfo,
        expressno: this.$route.query.expressno
      };
      let res = await GetExpressInfo(data);
      if (res.result <= 0) {
        this.noDataFlag = true;
        this.$toast(res.message);
        return;
      }
      this.logisticsInfo = res.data;
    }
  },
  components: {
    nodata
  }
};
</script>
<style lang="scss" scoped>
/deep/ .van-step--vertical .van-step__circle-container {
  top: 0.41rem;
}
/deep/ .van-icon__image {
  width: 0.28rem;
  height: 0.28rem;
}
/deep/ .van-step--process .van-icon__image {
  width: 0.36rem;
  height: 0.36rem;
}
/deep/ .van-steps--vertical {
  padding-top: 0.3rem;
  padding-left: 0.86rem;
}
.van-step--vertical {
  padding: 0.25rem 0;
  // padding-right: 0.9rem;
}
.status {
  font-family: PingFangSC-Regular;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.4rem;
  letter-spacing: 0rem;
  color: #333333;
}
h3 {
  margin: 0;
  font-family: PingFangSC-Regular;
  font-size: 0.26rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.3rem;
  letter-spacing: 0rem;
  color: #999999;
  padding-bottom: 0.15rem;
}
p {
  font-family: PingFangSC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.28rem;
  letter-spacing: 0rem;
  color: #999999;
}
.info {
  margin-top: 0.4rem;
}

.logistics {
  height: 100%;
  overflow: scroll;
  -webkit-overflow-scrolling: touch;
  background: #f7f7fc;
  padding: 0 0.3rem;
  .top {
    margin-top: 0.4rem;
    padding: 0.25rem 0.3rem;
    display: flex;
    background: #fff;
    .logisticsImg {
      width: 0.8rem;
      height: 0.8rem;
      margin-right: 0.3rem;
      img {
        width: 100%;
        height: 100%;
        border-radius: 50%;
      }
    }
    .logisticsTitle {
      font-family: PingFangSC-Medium;
      font-size: 0.32rem;
      font-weight: bold;
      font-stretch: normal;
      line-height: 0.36rem;
      letter-spacing: 0rem;
      color: #333333;
      height: 0.8rem;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      & > div:last-child {
        line-height: 0.32rem;
        font-family: PingFangSC-Regular;
        font-size: 0.28rem;
        font-weight: normal;
      }
    }
  }
}
.logistics::-webkit-scrollbar { display: none }
</style>