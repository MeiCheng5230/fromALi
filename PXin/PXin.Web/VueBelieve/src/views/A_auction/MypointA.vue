<template>
    <div class="reward">
        <div class="content">
            <div class="list">
                <div class="item" v-for="item in settledList" :key="item.index">
                  <div :class="item.status==1?'typetit':'typetitred'">累计{{item.status==1?"奖励已结算":"奖励未结算"}}</div>
                   <div class="number">数量：{{item.num}}</div>
                    <div class="botBx">
                        <div class="ops">
                            <div class="num">{{item.sv}}</div>
                            <div class="tit">SV</div>
                        </div>
                        <div class="ops">
                          <div class="num">{{item.svc}}</div>
                            <div class="tit">充值码(SVC)</div>
                        </div>
                        <div class="ops">
                          <div class="num">{{item.dos}}</div>
                            <div class="tit">专户DOS</div>
                        </div>
                    </div>
                    <div class="timebx">有效期：{{item.begindate|shortDate}}至{{item.enddate|shortDate}}</div>
                </div>
                 <nodata v-show="hisListshow"/>
            </div>
        </div>
    </div>
</template>

<script>
  import { GetMyAuctionA} from '@/api/getData';
  const nodata = () => import("@/components/noData");
  import moment  from 'moment';
  export default {
    data() {
      return {
        settledList: [],
        hisListshow:false,
      }
    },
    mounted() {
      this.setdata();
    },
    methods: {
      async getMyAuctionAData() {
        let result = await GetMyAuctionA(JSON.parse(sessionStorage.userParam));
        if (result.result > 0) {
          this.settledList=result.data;
          this.hisListshow=result.data.length>0?false:true;
        } else {
          this.Toast(result.message);
        }
      },
      setdata () {
        this.getMyAuctionAData();

      },
    },
    filters:{
       shortDate(data){
           var longdata= moment(data).format("YYYY-MM-DD");
           return longdata;
       }
    },
    components: {
        nodata
    }
}
</script>

<style lang="scss" scoped>
.reward {
    min-height: 100%;
    background: #f7f7fc;
    .content {
        padding: 0.3rem;
        .topbx {
            box-shadow: 0px 2px 8px 0px #e4e4e4;
	        border-radius: 0.04rem;
            padding: 0.2rem;
            position: relative;
            background: #fff;
            .tit {
                padding-bottom: 0.3rem;
                &::before {
                    content: '';
                    display: inline-block;
                    width: 0.08rem;
                    height: 0.3rem;
                    background: #2ea2fa;
                    border-radius: 0.04rem;
                    margin-right: 0.15rem;
                }
            }
            .num {
                padding-bottom: 0.2rem;
            }
            button {
                position: absolute;
                right: 0.3rem;
                top: 50%;
                transform: translateY(-50%);
                background: #2ea2fa;
                border-radius: 0.04rem;
                border: none;
                color: #fff;
                padding: 0.16rem 0.24rem;
                font-size: 0.24rem;
            }
        }
        .list {
            padding: 0.3rem 0;
            .item {
                background: #fff;
                position: relative;
                padding: 1.2rem 0 0;
                margin-bottom: 0.3rem;
                box-shadow: 0 0.02rem 0.08rem 0 #e4e4e4;
                border-radius: 0.04rem;
                .typetit {
                    position: absolute;
                    top: 0;
                    left: 0;
                    background: #2ea2fa;
                    color: #fff;
                    border-radius: 0.04rem 0 0.3rem 0;
                    padding: 0.2rem 0.26rem;
                }
                .typetitred {
                    position: absolute;
                    top: 0;
                    left: 0;
                    background: #f92d51;
                    color: #fff;
                    border-radius: 0.04rem 0 0.3rem 0;
                    padding: 0.2rem 0.26rem;
                }
                .typetitin{
                    position: absolute;
                    top: 0;
                    left: 0;
                    background: #e4e4e4;
                    color: #fff;
                    border-radius: 0.04rem 0 0.3rem 0;
                    padding: 0.2rem 0.26rem;
                }
                .detail {
                    color: #999;
                    position: absolute;
                    top: 0.2rem;
                    right: 0.3rem;
                    display: flex;
                    align-items: center;
                    img {
                        height: 0.36rem;
                        width: auto;
                    }
                }
                .botBx {
                    display: flex;
                    border-bottom: 1px solid #f2f2f2;
                    padding-bottom: 0.46rem;
                    .ops {
                        flex: auto;
                        text-align: center;
                        .num {
                            margin-bottom: 0.3rem;
                        }
                    }
                }
                .timebx {
                    font-size: 0.24rem;
                    color: #999;
                    padding: 0.22rem;
                }
                 .number {
                    position: absolute;
                    top: 0.2rem;
                    right: 0.3rem;
                    width: 1.8rem;
                    white-space: nowrap;
                    overflow: hidden;
                    text-overflow: ellipsis;
              }
            }
        }
    }
}
</style>
