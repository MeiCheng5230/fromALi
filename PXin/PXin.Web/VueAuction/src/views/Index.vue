<template>
  <div class="Aauction">
    <div class="bgbx">
      <div class="tptit">
        <div class="lft">本月竞拍</div>
        <div class="rgt">{{auctionData.anum}}个A点</div>
        <div class="rule">
          <router-link to="/rule" tag="span">《竞拍规则》</router-link>
        </div>
      </div>
    </div>

    <div class="banner">
      <a :href="draw">
        <img src="@/assets/images/A_banner.png" alt="">
      </a>
    </div>
    <div class="thisMonth" v-show="isdefault">

    </div>
    <div class="thisMonth" v-if="auctionData.myout>0 ||auctionData.myleading>0" v-show="!isdefault">
      <div class="topbx">
        <div class="num">{{auctionData.myleading+auctionData.myout}}A点</div>
        <div class="tit">我本月竞拍总数</div>
      </div>
      <div class="botbx">
        <div class="lft">
          <div class="num"><span style="color: #ff5b1b">{{auctionData.myleading}}</span>A点</div>
          <div class="tit"><img src="@/assets/images/paimai_ahead.png"/>领先</div>
        </div>
        <div class="rgt">
          <div class="num"><span style="color: #bfbfbf">{{auctionData.myout}}</span>A点</div>
          <div class="tit"><img src="@/assets/images/paimai_out.png"/>出局</div>
        </div>
      </div>
    </div>
    <div class="thisMonth noRecord" v-show="!isdefault" v-else
         :style="{backgroundImage:'url('+require('@/assets/images/paimai_unrecorded_bg.png')+')'}">
      <div class="txt">咦~本月你还未有竞拍记录</div>
      <div @click="myauction" tag="div" class="jump">立即去参加竞拍</div>
    </div>
    <div class="mybx">
      <router-link to="/myPointA" tag="div" class="lft">我的A点：{{auctionData.mya}}</router-link>
      <router-link to="/myRecord" tag="div" class="rgt">我的竞拍记录</router-link>
    </div>
    <div class="record">
      <div class="top">
        <button class="bntRefresh" :disabled="disable" :class="{ codeGeting:isGeting }" tag="button"
                @click="saveRefreshNum">{{getCode}}
        </button>
      </div>
      <div class="top">
        <div class="lft">拍卖记录</div>
        <router-link to="/detail" tag="div" class="rgt">查看更多<img src="@/assets/images/ic_enter@2x.png" alt="">
        </router-link>
      </div>
      <div class="tit">
        <div class="lft">排名区间(名)</div>
        <div class="rgt">竞拍价(单价)</div>
      </div>
      <div class="ltbx" :class="Index < 10?'active':''" v-for="(item,index) in auctionData.auctionhis" :key="item.id">
        <div v-show="Index==10">
          <div class="item" v-for="tm in 3" :key="tm">
            <div class="idx1">.</div>
            <div class="num1">.</div>
          </div>
        </div>
        <div class="item">
          <div class="idx">{{item.befornum}}-{{item.afternum}}</div>
          <div class="num"><span class="lt">{{item.price}}</span>P点</div>
        </div>
      </div>
      <nodata v-if="noDataFlag" text='暂无拍卖记录'></nodata>

    </div>
    <div class="fixedbx">
      <div>
        <div>距离结束还剩<span style="font-size: 0.36rem;">{{ parseInt(time/1000/60/60/24) }}</span>天</div>
        <div>
          <!-- <van-count-down :time="time" format="HH 时 mm 分 ss 秒"/> -->
          <van-count-down :time="time">
            <template v-slot="timeData">
              <span
                class="item"><span>{{ String(timeData.hours).length == 1 ? '0'+ timeData.hours : timeData.hours  }}</span>小时</span>
              <span class="item"><span>{{ String(timeData.minutes).length == 1 ? '0'+ timeData.minutes : timeData.minutes }}</span>分钟</span>
              <span class="item"><span>{{ String(timeData.seconds).length == 1 ? '0'+ timeData.seconds : timeData.seconds }}</span>秒</span>
            </template>
          </van-count-down>
        </div>
      </div>

      <div class="btnbx">
        <div class="rmk" v-if="parseInt(time/1000/60/60/24) <= 0 && time>0"
             :style="{backgroundImage:'url('+require('@/assets/images/paimai_hint_bg.png')+')'}">今天出局仅退还一半P点哦，快来加价
        </div>
        <button class="addbtn" @click="setAddPrice" tag="button">加价</button>
        <button @click="myauction" tag="button">我要竞拍</button>
      </div>
    </div>
    <van-popup v-model="popFlag" round>
      <div class="popbx">
        <div class="poptxt">竞拍成功的A点，可连续100天每天领取SV、充值码(SVC)、专户DOS等随机大礼包</div>
        <div class="popbtn" @click="agreeok">同意并继续</div>
        <div class="agreebx">
          <div class="selIcon" @click="agree = !agree">
            <img v-if="agree" src="../assets/images/ic_unselected_pay.png" alt=""/>
            <img v-else src="../assets/images/ic_selected_pay.png" alt=""/>

          </div>

          <div class="file">我已同意并阅读<a href="/html/auction.html">《竞拍协议》</a></div>
        </div>
      </div>
    </van-popup>
  </div>
</template>

<script>

    const nodata = () => import("@/components/noData");
    import {GetThisMonthData, SetAgreeAgreement, GetAuctionAddprice, GetAuctionRanking} from '@/api/api';
    import moment from 'moment';
    import {setLocalStorage,getLocalStorage} from "@/config/utils";
    export default {
        data() {
            return {
                auctionData: {
                    anum: 0,
                    myleading: 0,
                    myout: 0,
                    mya: 0,
                },
                time: 0,
                popFlag: false,
                agree: true,
                isdefault: true,//默认边框
                noDataFlag: false,
                isagreement: false,
                draw: '',     // 抽奖链接
                timer: '',
                isGeting: false,
                count: 5,
                disable: false,
                getCode: '刷新拍卖记录',
            }
        },
        components: {
            nodata
        },
        created() {
            var lastdate = moment(moment(new Date()).endOf('month').format("YYYY-MM-DD") + " 12:00:00");
            var now = moment(new Date());
            this.time = lastdate.diff(now, "millisecond");
            this.draw = baseUrl+'/App/Luckdraw/index.html' + location.search;
        },
        mounted() {
            // this.$once('hook:beforeDestroy', () => {//离开当前页面
            //    debugger;

            // })
            //获取缓存
            var auctionIndexData=getLocalStorage("AuctionIndexData",1000*5);
            if(auctionIndexData==null){
               this.getThisMonthData();
               this.disable=false;
               this.isGeting = false;
            }else{
              this.refreshBut();
              this.isdefault = false;
              this.auctionData = auctionIndexData;
              this.isagreement = auctionIndexData.isagreement == 0 ? false : true;
              if (!this.auctionData.auctionhis.length) {
                this.noDataFlag = true;
              }
            }
        },
        methods: {
            async getThisMonthData() {
                let result = await GetThisMonthData(JSON.parse(sessionStorage.userParam));
                this.isdefault = false;
                if (result.result > 0) {
                    this.auctionData = result.data;
                    this.isagreement = result.data.isagreement == 0 ? false : true;
                    if (!this.auctionData.auctionhis.length) {
                        this.noDataFlag = true;
                    }
                    setLocalStorage("AuctionIndexData",result.data);
                } else {
                    vant.Toast(result.message);
                }
            },
            async GetAuctionAddprice() {
                let result = await GetAuctionAddprice(this.$global.userInfo);
                if (result.result > 0) {
                    if (result.data.myauctionhis.length > 0) {
                        this.$router.push('/markUp');
                    } else {
                        vant.Dialog.alert({
                            title: '温馨提示',
                            message: '没有可加价的竞拍记录',
                            confirmButtonText: '知道了'
                        })
                    }
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async SetAgreeAgreement() {
                let result = await SetAgreeAgreement(JSON.parse(sessionStorage.userParam), 20003);
                if (result.result > 0) {

                } else {
                    vant.Toast(result.message);
                }
            },
            agreeok() {
                if (this.agree) {
                    vant.Toast("请同意竞拍协议");
                    return;
                }
                let nodeid = JSON.parse(sessionStorage.userParam).nodeid;
                this.SetAgreeAgreement();
                this.popFlag = false;
                this.$router.push({name: 'auction'});
            },
            myauction() {
                if (this.time < 0) {
                    vant.Dialog.alert({
                        message: '本月竞拍已截止，请关注下个月A点竞拍',
                        confirmButtonText: '知道了'
                    })
                    return false;
                }
                if (this.auctionData.anum == 0) {
                    vant.Toast("本月没有竞拍");
                    return;
                }
                let nodeid = JSON.parse(sessionStorage.userParam).nodeid;
                if (this.isagreement) {
                    this.$router.push({name: 'auction'});
                } else {
                    this.popFlag = true;
                }

            },
            setAddPrice() {
                if (this.time < 0) {
                    vant.Dialog.alert({
                        message: '本月竞拍已截止，请关注下个月A点竞拍',
                        confirmButtonText: '知道了'
                    })
                    return false;
                }
                if (this.auctionData.anum == 0) {
                    vant.Toast("本月没有竞拍");
                    return;
                }
                this.GetAuctionAddprice();

            },

        saveRefreshNum() {
            this.GetAuctionRanking();
        },
        async GetAuctionRanking(){
          let result = await GetAuctionRanking(JSON.parse(sessionStorage.userParam));
          if (result.result > 0 && result.data.length > 0) {
              this.noDataFlag = false;
              this.auctionData.auctionhis = result.data;
          }
          this.refreshBut();
        },
        refreshBut() {
           this.isGeting = true;
           this.disable = true;  
           var countDown = setInterval(() => {
                if (this.count < 1) {
                  this.isGeting = false;
                  this.disable = false;
                  this.getCode = '刷新拍卖记录';
                  this.count = 5;
                  clearInterval(countDown);
                } else {
                  this.isGeting = true;
                  this.disable = true;
                  this.getCode = this.count-- + 's后可点击';
                }
            }, 1000);
          }
        },

    }
</script>

<style lang="scss" scoped>
  .van-count-down, .van-divider {
    color: red !important;
  }

  .banner {
    width: 100%;
    padding: 0 0.3rem;
    box-sizing: border-box;
    margin-top: 0.4rem;

    img {
      width: 100%;
    }
  }

  .input {
    width: 4.2rem;
    height: 0.68rem;
    background-color: #f7f7fc;
    border-radius: 0.04rem;
    border: none;
    box-sizing: border-box;
    padding: 0.2rem 0.3rem;

    &::placeholder {
      color: #999;
    }
  }

  .bntRefresh {
    height: 0.64rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    border: none;
    color: #fff;
  }

  .codeGeting {
    background: #cdcdcd;
    border-color: #cdcdcd;
  }

  .popbx {
    width: 4rem;
    padding: 0.3rem 0.5rem;

    .poptxt {
      padding: 0.3rem 0;
      font-size: 0.24rem;
      color: #666;
    }

    .popbtn {
      background: #2ea2fa;
      color: #fff;
      text-align: center;
      padding: 0.1rem 0;
      border-radius: 0.06rem;
    }

    .agreebx {
      display: flex;
      align-items: center;
      margin-top: 0.3rem;

      .selIcon {
        margin-right: 0.2rem;

        img {
          width: 0.3rem;
          height: auto;
        }
      }

      .file {
        font-size: 0.24rem;

        span {
          color: #2ea2fa;
        }
      }
    }
  }

  .Aauction {
    min-height: 100%;
    background: #f7f7f7;
    padding-bottom: 70px;
    font-size: 0.28rem;

    .bgbx {
      background: #2ea2fa;
      height: 1.2rem;
      border-radius: 0 0 50% 50%;
      position: relative;

      .tptit {
        display: flex;
        align-items: center;
        background: #fff;
        position: absolute;
        left: 50%;
        bottom: -0.2rem;
        transform: translateX(-50%);
        width: 6.9rem;
        padding: 0.3rem 0.6rem;
        box-sizing: border-box;
        box-shadow: 1px 2px 6px 0px #ececec;
        border-radius: 4px;
        white-space: nowrap;

        .rgt {
          color: #2ea2fa;
          font-size: 0.36rem;
          margin-left: 0.54rem;
        }
      }
    }

    .rule {
      color: #2ea2fa;
      margin-left: 0.5rem;
    }

    .thisMonth {
      background: #fff;
      height: 2.7rem;
      margin: 0.3rem;
      margin-top: 0.1rem;
      font-size: 0.28rem;
      box-shadow: 0 0.02rem 0.06rem 0 #ececec;
      border-radius: 0.04rem;

      .topbx {
        text-align: center;
        padding: 0.3rem 0;
      }

      .botbx {
        display: flex;
        padding: 0.15rem 0;

        .lft, .rgt {
          width: 50%;
          text-align: center;
          padding-bottom: 0.1rem;

          .tit {
            margin-top: 0.16rem;
            display: flex;
            align-items: center;
            justify-content: center;

            img {
              width: 0.3rem;
              margin-right: 0.1rem;
            }
          }

          .num {
            width: 100%;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;

            span {
              font-weight: bolder;
              font-size: 0.36rem;
            }
          }

          &.rgt {
            border-left: 1px solid #ececec;
          }
        }

      }

      &.noRecord {
        height: 2.7rem;
        background-size: auto 2.7rem;
        background-repeat: no-repeat;
        background-position: -0.6rem 0.1rem;
        display: flex;
        flex-direction: column;
        align-items: flex-end;
        justify-content: center;
        font-size: 0.24rem;

        .txt {
          color: #999;
          margin-right: 0.56rem;
        }

        .jump {
          color: #2ea2fa;
          margin-top: 0.24rem;
          margin-right: 1.09rem;
          text-decoration: underline;
        }
      }
    }

    .mybx {
      display: flex;
      padding: 0 0.3rem;

      .lft, .rgt {
        width: 50%;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
        box-shadow: 0 0.02rem 0.06rem 0 #ececec;
        border-radius: 0.04rem;
        margin-right: 0.3rem;
        text-align: center;
        padding: 0.3rem 0;
        background: #fff;
      }

      .rgt {
        margin: 0;
      }
    }

    .record {
      padding: 0.3rem 0.3rem 1.2rem 0.3rem;
      background: #fff;
      margin-top: 0.6rem;

      .ltbx {
        &.active {
          .item {
            .idx {
              color: #ff5b1b;
              font-weight: bolder;
            }

            .num {
              .lt {
                color: #ff5b1b;
                font-weight: bolder;
              }
            }
          }
        }
      }

      .top, .tit {
        display: flex;
        margin-bottom: 0.3rem;

        .lft {
          flex: auto;
        }

        .rgt {
          display: flex;
          align-items: center;

          img {
            width: 0.34rem;
            height: auto;
          }
        }
      }

      .top {
        .lft {
          font-weight: bold;
        }

        .rgt {
          color: #999;
        }
      }

      .tit {
        font-size: 0.28rem;
      }

      .item {
        display: flex;
        align-items: center;
        margin-bottom: 0.3rem;
        font-size: 0.28rem;

        .idx {
          flex: auto;
          // font-weight: bolder;
          // color: #ff5b1b;
          // font-size: 0.3rem;
        }

        .num {
          // font-size: 0.2rem;
          // .lt {
          //     font-weight: bolder;
          //     color: #ff5b1b;
          //     font-size: 0.3rem;
          // }
        }

        .idx1 {
          padding-left: 0.2rem;
          width: 50%;
          font-weight: 800;
        }

        .num1 {
          width: 50%;
          padding-right: 0.2rem;
          text-align: right;
          font-weight: 800;
        }
      }
    }

    .fixedbx {
      position: fixed;
      bottom: 0;
      width: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.13rem 0.3rem;
      box-sizing: border-box;
      font-size: 0.24rem;
      background: #fff;
      box-shadow: -0.01rem -0.02rem 0.06rem 0 #ececec;

      .btnbx {
        position: relative;

        .rmk {
          background-size: 100% 100%;
          position: absolute;
          right: 1.26rem;
          bottom: 0.8rem;
          width: 5.2rem;
          height: 0.78rem;
          line-height: 0.6rem;
          font-size: 0.28rem;
          color: #fff;
          text-align: center;
        }
      }

      span {
        flex: auto;
        color: #ff2020;
      }

      button {
        font-size: 0.3rem;
        background: #2ea2fa;
        outline: none;
        color: #fff;
        border: none;
        border-radius: 0.06rem;
        padding: 0.21rem 0.3rem;
        box-sizing: border-box;
        border: 1px solid #2ea2fa;

        &.addbtn {
          background: #fff;
          border: 1px solid #2ea2fa;
          color: #2ea2fa;
          margin-right: 0.3rem;
        }
      }
    }
  }

  .fixedbx {
    .van-count-down {
      .item {
        color: #333;
        font-size: 0.24rem;

        span {
          display: inline-block;
          border: 1px solid #ff1541;
          margin: 0 0.05rem;
          padding: 0.1rem 0.05rem;
          border-radius: 0.04rem;
          font-size: 0.28rem;
        }
      }
    }
  }
  @supports (bottom: env(safe-area-inset-bottom)) {
    .fixedbx {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
</style>
