<!-- 进货记录 -->
<template>
  <div class='stock' >
      <div class='hint' v-if='nochecktime'>{{nochecktime}}前免考核</div>
    <div class='hint' v-if='targettime && stocknum'>{{targettime}}前最低进零售货{{stocknum}}套，否则将会冻结</div>
    <!-- 无进货记录 -->
    <div class="empty" v-if='empty'>
      暂无进货记录...
    </div>
    <!-- 有进货记录 -->
    <div class='stockList'>
      <div class='stockInfo' v-for='(item,index) of list' :key="index">
        <div>
          <p>零售码进货</p>
          <p>{{item.createtime}}</p>
        </div>
        <div>
          <p>{{item.stocknum}}</p>
          <p>套</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  // <!-- 进货记录 -->
  import { Get90DaysPurchases } from '@/api/getFbApData';
  export default {
    data() {
      return {
        targettime: null, //时间
        stocknum: null, //多少套
        list: [],
        nochecktime: null, //免考核日期
        empty:false,
      }
    },
    created() {
      this.Get90DaysPurchases(); //获取90天累积进货列表
    },
    watch:{
      list(newValue,formerValue){
        if(newValue.length==0 && formerValue.length==0){
          this.empty = true ;
        }
      }
    },
    methods: {
      //  /api/Bts/Get90DaysPurchases获取90天累积进货列表
      async Get90DaysPurchases() {
        let result = await Get90DaysPurchases(JSON.parse(sessionStorage.userParam), JSON.parse(this.$route.query.infoId));
        if (result.result > 0) {
          result.data.isshownocheck == true ? this.nochecktime = result.data.nochecktime : null;
          this.list = result.data.stockhis;
          this.targettime = result.data.targettime;
          this.stocknum = result.data.stocknum;
        } else {
          this.Toast(result.message);
        };
      }
    },
  }
</script>

<style scoped lang='scss'>
  /* <!-- 进货记录 --> */
  .empty{
    text-align: center;
    height: 2rem;
    line-height: 2rem;
    color:#666;
  }
  .stock {
    height: 100%;
    overflow-y: scroll;
    background: rgba(244, 244, 244, 1);

    .hint {
      padding-left: .2rem;
      height: .68rem;
      line-height: .68rem;
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
      margin-top: .3rem;
      align-items: center;
      padding: 0 .2rem;
      justify-content: space-between;

      &>div:nth-child(1) {
        &>p:nth-child(1) {
          font-size: 0.3rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(51, 51, 51, 1);
          padding-bottom: .1rem;
        }

        &>p:nth-child(2) {
          font-size: 0.24rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(153, 153, 153, 1);
        }
      }

      &>div:nth-child(2) {
        display: flex;
        align-items: center;
        &>p:nth-child(1) {
          font-size: 0.36rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(255, 185, 15, 1);
        }

        &>p:nth-child(2) {
          font-size: 0.3rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(51, 51, 51, 1);
          padding-left: .1rem;
        }
      }

    }
  }
</style>
