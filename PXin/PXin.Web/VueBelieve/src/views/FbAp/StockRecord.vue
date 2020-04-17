<!-- 进货记录 -->
<template>
  <div class='stock' >
    <div class='hint' v-if='targettimestocknum'>{{targettimestocknum}}</div>
    <!-- 无进货记录 -->
    <div class="empty" v-if='empty'>
      暂无进货记录...
    </div>
    <!-- 有进货记录 -->
    <div class='stockList'>
      <div class='stockInfo' v-for='(item,index) of list' :key="index">
        <div>
          <p>{{(item.stocktype==1||item.stocktype==3)?"SV零售码库存":"SV批发码库存"}}</p>
          <p>{{item.createtime}}</p>
        </div>
        <div>
          <p :class="(item.stocktype==3||item.stocktype==4)?'stockAmount':''">{{(item.stocktype==3||item.stocktype==4)?-item.stocknum: item.stocknum}}</p>
          <p></p>
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
        targettimestocknum: null,
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
          this.targettimestocknum = result.data.targettimestocknum;
        } else {
          this.Toast(result.message);
        };
      }
    },
    GetDesc(type){
      switch(type){
        case 1:
          return 'SV零售码进货';
        case 2:
          return 'SV批发码进货';
        case 3:
          return 'SV零售码出货';
        case 4:
          return 'SV批发码出货';
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
      justify-content: space-between;
      padding:  .3rem;
      box-sizing: border-box;
      p{
        margin: 0 ;
      }

      &>div:nth-child(1) {
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        &>p:nth-child(1) {
          font-size: 0.3rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(51, 51, 51, 1);
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
    .stockAmount{
      color: #333 !important;
    }
  }
</style>
