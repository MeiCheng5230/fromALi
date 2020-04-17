<template>
    <div class="reward">
        <div class="content">
          <div class="topbx">
            <div class="tit">可兑换</div>
            <div class="num">充值码(SVC)：{{svc}}</div>
            <router-link to="/SVCexchange" tag="button">去兑换</router-link>
          </div>
          <div class="list">
            <div class="item" v-for="item in settledList" :key="item.index">
              <div :class="item.status==1?'typetit':'typetitred'">累计{{item.status==1?"已结算":"已失效"}}</div>
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
            </div>
          </div>
          <div class="title">领取列表</div>
          <div class="list">
            <div class="item" v-for="item in receivedList" :key="item.index">
              <div :class="item.status==3?'typetit':'typetitin'">{{item.status==3?'已领取':'未领取'}}</div>
              <div class="detail" @click="detail(item.hisid)">查看详情<img src="@/assets/images/ic_enter@2x.png" alt=""></div>
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
              <div>
                <div style="margin: 0.2rem;padding-bottom: 0.2rem;">红包日期：{{item.time.split(' ')[0]}}</div>
              </div>
            </div>
          </div>
          <noData v-if="flag" text="您没有领取记录！"></noData>
        </div>
    </div>
</template>

<script>
  const noData = () => import("@/components/noData");
  import { GetMyRedPacket} from '@/api/getData'
  export default {
    data() {
      return {
        settledList: [],
        receivedList: [],
        svc: 0,
        flag:false
      }
    },
    mounted() {
      this.setdata();
    },
    methods: {
      async GetMyRedPacket() {
        let result = await GetMyRedPacket(JSON.parse(sessionStorage.userParam));
        if (result.result > 0) {
          this.svc = result.data.svc;
          this.settledList = result.data.settleamounts;
          this.receivedList = result.data.receiveamounts;
          if (this.receivedList.length > 0) {
            this.flag = false;
          } else {
            this.flag = true;
          }

        } else {
          this.flag = true;
          this.Toast(result.message);
        }
      },
      setdata: function () {
        this.GetMyRedPacket();

      },
      detail: function (hisid) {
        this.$router.push({
          name:"RewardDetail",
          params: {
            hisid: hisid,
          }
        })
      },
    },
    components: {
      noData
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
                        flex: 1;
                        text-align: center;
                        .num {
                          width:1.4rem;
                          margin:0 auto;
                          margin-bottom: 0.3rem;
                          white-space:nowrap;
                          overflow:hidden;
                          text-overflow:ellipsis;
                        }
                    }
                }
                .timebx {
                    font-size: 0.24rem;
                    color: #999;
                    padding: 0.22rem;
                }
            }
        }
    }
}
</style>
