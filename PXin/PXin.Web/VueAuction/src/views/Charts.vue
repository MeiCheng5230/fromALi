<template>
    <div class="charts">
        <div class="btit">拍卖价格区间人数分布散点图</div>
        <ve-scatter :data="chartData" :extend="chartExtend" :settings="chartSettings" :data-empty="dataEmpty" :legend="legend" :cancel-resize-check="true" :resizeable="false" :data-zoom="dataZoom">
        </ve-scatter>
        <div class="timbx">
          <div class="time select" @click="SetTime($event, 1)">近七天</div>
          <div class="time" @click="SetTime($event, 2)">七天之前</div>
          <div class="time" @click="SetTime($event, 3)">本月</div>
        </div>
    </div>
</template>

<script>
import scatter from 'v-charts/lib/scatter.common'
import 'echarts/lib/component/dataZoom'
Vue.component(scatter.name, scatter)
import { GetAuctionDetails } from '@/api/api'
export default {
    data () {
      this.dataZoom = [
        {
          type: 'slider',
          start: 0,
          end: 100
        }
      ],
      this.legend = {
        selectedMode: false,
      }
      this.chartExtend = {
        xAxis: {
          splitLine: {
            show: true,
            lineStyle:{
              width: 1,
              type: 'dashed'
            }
    　　  },
        },
        yAxis: {
          splitLine: {
            show: true,
            lineStyle:{
              width: 1,
              type: 'dashed'
            }
    　　  },
        },
      };
      this.chartSettings = {
        dimension: 'createdate',
        metrics: ['price', 'num'],
        tooltipTrigger: 'none',
        scale: true,
      };
      return {
        chartData: {
          columns: ['createdate', 'price', 'num',],
          rows: {
            '散点面积越大则代表该价格区间人数越多': []
          }
        },
        dataEmpty: false,
      }
    },
    created() {
        this.GetAuctionDetails(1)
    },
    mounted() {
      this.$nextTick(()=> {
        setTimeout(()=> {
          document.getElementsByTagName('canvas')[0].style.width = '95%';
        })
      })
    },
    methods: {
        async GetAuctionDetails(querytimetype) {
            let result = await GetAuctionDetails(this.$global.userInfo, querytimetype);
            if (result.result > 0) {
              let newArr = result.data;
              let obj = {};
              obj['createdate'] = '';
              obj['price'] = 0;
              obj['num'] = 0;
              newArr.push(obj);
              this.chartData.rows['散点面积越大则代表该价格区间人数越多'] = newArr;
              if (newArr.length <= 0) {
                this.dataEmpty = true;
              } else {
                this.dataEmpty = false;
              }
            } else {
               vant.Toast.fail(result.message);
            }
        },
        SetTime(e, n) {
          this.GetAuctionDetails(n);
          document.querySelector(".timbx").querySelector(".select").classList.remove("select");
          e.target.className += ' select';
        },
        SetSortKey(array, key) {
          return array.sort(function(b, a) {
            var x = a[key];
            var y = b[key];
            return x > y ? -1 : x < y ? 1 : 0;
          });
        }
    }
}
</script>

<style lang="scss" scoped>
.charts {
  /deep/ .ve-scatter {
    position: relative;
    .v-charts-data-empty {
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      color: #999;
    }
  }

  .btit {
    font-size: 0.36rem;
    font-weight: bold;
    text-align: center;
    padding: 1rem;
  }
  .timbx {
    position: fixed;
    bottom: 0;
    width: 100%;
    display: flex;
    .time {
      flex: 1;
      text-align: center;
      padding: 0.14rem 0;
      background: #f7f7fc;
      font-size: 0.24rem;
      &.select {
        background: #2ea2fa;
        color: #fff;
      }
    }
  }
}
</style>
