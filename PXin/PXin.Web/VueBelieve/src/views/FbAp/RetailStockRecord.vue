<!-- 进货记录 -->
<template>
  <div class="stock">
    <div class="hint" v-if="targettimestocknum">{{targettimestocknum}}</div>
    <!-- 无进货记录 -->
    <div class="empty" v-if="empty">暂无记录...</div>
    <!-- 有进货记录 -->
    <div class="stockList">
      <div class="stockInfo" v-for="(item,index) of list" :key="index">
        <div>
          <p>{{item.typeid==1?"SV零售码库存进货":(item.typeid==3?"SV零售码库存出货":(item.typeid==5?"零售":(item.typeid==6?"回收":"代开充值商")))}}</p>
          <p>{{item.createtime}}</p>
        </div>
        <div>
          <p
            :class="(item.typeid==3||item.typeid==5||item.typeid==7)?'stockAmount':''"
          >{{(item.typeid==3||item.typeid==5||item.typeid==7)?-item.number: '+'+item.number}}</p>
          <p></p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { GetStockRecord } from "@/api/getFbApData";
export default {
  data() {
    return {
      list: [],
      empty: false
    };
  },
  created() {
    this.GetStockRecord();
  },
  watch: {
    list(newValue, formerValue) {
      if (newValue.length == 0 && formerValue.length == 0) {
        this.empty = true;
      }
    }
  },
  methods: {
    async GetStockRecord() {
      let param = {
        ...JSON.parse(sessionStorage.userParam),
        infoId: this.$route.query.infoId,
        type: 1
      };
      let result = await GetStockRecord(param);
      if (result.result > 0) {
        this.list = result.data;
      } else {
        this.Toast(result.message);
      }
    }
  }
};
</script>

<style scoped lang='scss'>
/* <!-- 进货记录 --> */
.empty {
  text-align: center;
  height: 2rem;
  line-height: 2rem;
  color: #666;
}
.stock {
  height: 100%;
  overflow-y: scroll;
  background: rgba(244, 244, 244, 1);

  .hint {
    padding-left: 0.2rem;
    height: 0.68rem;
    line-height: 0.68rem;
    background: rgba(255, 234, 210, 1);
    font-size: 0.24rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(255, 48, 48, 1);
  }

  .stockInfo {
    height: 1.42rem;
    display: flex;
    background: #fff;
    margin-top: 0.3rem;
    align-items: center;
    justify-content: space-between;
    padding: 0.3rem;
    box-sizing: border-box;
    p {
      margin: 0;
    }

    & > div:nth-child(1) {
      height: 100%;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      & > p:nth-child(1) {
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(51, 51, 51, 1);
      }

      & > p:nth-child(2) {
        font-size: 0.24rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(153, 153, 153, 1);
      }
    }

    & > div:nth-child(2) {
      display: flex;
      align-items: center;
      & > p:nth-child(1) {
        font-size: 0.36rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(255, 185, 15, 1);
      }

      & > p:nth-child(2) {
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(51, 51, 51, 1);
        padding-left: 0.1rem;
      }
    }
  }
  .stockAmount {
    color: #333 !important;
  }
}
</style>
