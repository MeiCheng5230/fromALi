<template>
  <div class="myrecord">
    <div class="timedelect" @click="changedate">
      <span>{{showDate}}</span>
      <span class="ico_btn"><van-icon name="arrow-down"/></span>
    </div>
    <div class="recordlist">
      <van-list v-model="loading"
                :finished="finished"
                :immediate-check=false
                :finished-text="hisListshow?'':'没有更多了'"
                :offset=10
                @load="onLoad">
        <div class="item" v-for="item in hisList" :key="item.id">
          <div class="top" :class="{out:item.status==-1}">
            <div class="lft">{{item.price}}P点×{{item.num}}</div>
            <div class="rgt" v-show="item.status==0">领先</div>
            <div class="rgt" v-show="item.status==1">已完成</div>
            <div class="rgt" v-show="item.status==-1">出局(P点已退)</div>
            <div class="rgt" v-show="item.status==-2">出局</div>
          </div>
          <div class="bot">
            <div class="lft">{{item.createtime}}</div>
            <!-- <div class="rgt">5558-5888</div> -->
          </div>
        </div>
      </van-list>
      <nodata v-show="hisListshow" :text="'咦！您目前还木有竞拍哟'"/>
    </div>
    <van-popup
      v-model="show"
      position="bottom">
      <van-datetime-picker
        v-model="currentDate"
        type="year-month"
        :formatter="formatter"
        @confirm="confirmDate"
        @cancel="cancelDate"/>
    </van-popup>
  </div>
</template>

<script>
    const nodata = () => import("@/components/noData");
    import {GetMyAuctionHis} from '@/api/api';
    import moment from 'moment';

    export default {
        data() {
            return {
                hisList: [],
                currentDate: new Date(),
                show: false,
                hisListshow: false,
                loading: false,
                finished: false,
                pageNum: 1,
                pagesize: 10,
                queryDate: moment(new Date()).format("YYYY-MM"),
                showDate: moment(new Date()).format("YYYY年M月"),
            }
        },
        components: {
            nodata
        },
        mounted() {
            this.getMyAuctionHis();
        },
        methods: {
            formatter(type, value) {
                if (type === 'year') {
                    return `${value}年`;
                } else if (type === 'month') {
                    return `${value}月`
                }
                return value;
            },
            changedate() {
                this.show = true;
            },
            confirmDate(value) {
                this.queryDate = moment(value).format("YYYY-MM");
                this.showDate = moment(value).format("YYYY年M月");
                this.show = false;
                this.hisList = [];
                this.pageNum = 1;
                this.finished = false;
                this.getMyAuctionHis();
            },
            cancelDate() {
                this.show = false;
            },
            async getMyAuctionHis() {
                let result = await GetMyAuctionHis(JSON.parse(sessionStorage.userParam), this.pageNum, this.pagesize, this.queryDate);
                if (result.result > 0) {
                    this.hisList = this.hisList.concat(result.data);
                    this.hisListshow = this.hisList.length > 0 ? false : true;
                    if (result.data.length <10) {
                        this.finished = true;
                        this.loading = false;
                    }
                } else {
                     vant.Toast(result.message);
                }
            },
            onLoad() {
                this.pageNum += 1;
                this.getMyAuctionHis();


            }
        },

    }
</script>

<style lang="scss" scoped>
  .myrecord {
    .timedelect {
      padding: 0.3rem;

      .ico_btn {
        font-size: 0.4rem;

        i {
          vertical-align: middle;
        }
      }
    }

    .recordlist {
      padding: 0 0.3rem;

      .item {
        padding: 0.3rem;
        box-shadow: -1px -2px 6px 0 #ececec, 1px 2px 6px 0 #ececec;
        border-radius: 0.12rem;
        margin-bottom: 0.3rem;

        .top {
          display: flex;
          margin-top: 0.1rem;

          .lft {
            flex: auto;
            font-size: 0.28rem;

            &::before {
              content: "";
              background: #2ea2fa;
              border: 2px solid #2ea2fa;
              border-bottom: none;
              border-top: none;
              border-radius: 4px;
              margin-right: 0.16rem;
            }
          }
        }

        .out {
          color: #E0E0E0;
        }

        .bot {
          display: flex;
          margin-top: 0.2rem;

          .lft {
            flex: auto;
            font-size: 0.24rem;
            color: #999;
          }
        }
      }

      .outitem {
        color: #999;
      }
    }
  }
</style>
