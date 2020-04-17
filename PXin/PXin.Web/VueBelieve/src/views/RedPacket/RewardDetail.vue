<template>
    <div class="RewardDetail">
        <div class="list">
            <div class="item">
              <div :class="data.status==3?'typetit':'typetitin'">{{data.status==3?"已领取":"未领取"}}</div>
                <div class="number">A点个数：{{adian}}</div>
                <div class="botBx">
                    <div class="ops">
                      <div class="num">{{data.sv}}</div>
                        <div class="tit">SV</div>
                    </div>
                    <div class="ops">
                      <div class="num">{{data.svc}}</div>
                        <div class="tit">充值码(SVC)</div>
                    </div>
                    <div class="ops">
                      <div class="num">{{data.dos}}</div>
                        <div class="tit">专户DOS</div>
                    </div>
                </div>
            </div>
        </div>
        <div class="table" v-show="receiveamountdetails.length">
            <table border="0" cellpadding="0" cellspacing="0">
                <thead>
                    <tr>
                        <th>A点</th>
                        <th>SV</th>
                        <th>SVC</th>
                        <th>专户DOS</th>
                        <th>状态</th>
                    </tr>
                </thead>
                <tbody>
                  <tr :class="item.statusdesc=='待结算'? 'bgtr': ''"  v-for="item in receiveamountdetails" :key="item.index">
                    <td>{{item.adian}}</td>
                    <td>{{item.sv}}</td>
                    <td>{{item.svc}}</td>
                    <td>{{item.dos}}</td>
                    <td>{{item.statusdesc}}</td>
                  </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
  import { GetMyRedPacketDetail } from '@/api/getData'
  export default {
    data() {
      return {
        data: {},
        receiveamountdetails: [],
        adian: 0,
        hisid:0,
      }
    },
    mounted() {
      this.setdata();
    },
    methods: {
      async GetMyRedPacketDetail() {
        let result = await GetMyRedPacketDetail(JSON.parse(sessionStorage.userParam), this.hisid);
        if (result.result > 0) {
          this.adian = result.data.adian;
          this.receiveamountdetails = result.data.receiveamountdetails;
          this.data = result.data.receiveamount;
        } else {
          this.Toast(result.message);
        }
      },
      setdata: function () {
        this.hisid = this.$route.params.hisid;
        this.GetMyRedPacketDetail();

      },
    }
}
</script>

<style lang="scss" scoped>
.RewardDetail {
    .list {
        padding: 0.3rem;
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
            .typetitin {
                position: absolute;
                top: 0;
                left: 0;
                background: #e4e4e4;
                color: #fff;
                border-radius: 0.04rem 0 0.3rem 0;
                padding: 0.2rem 0.26rem;
            }
            .number {
                position: absolute;
                top: 0.2rem;
                right: 0.3rem;
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
        }
    }
    .table {
        width: 100%;
        padding: 0 0.3rem;
        box-sizing: border-box;
        font-size: 0.28rem;
        table {
            width: 100%;
            tr {
                th {
                    padding: 0.2rem 0;
                    background: #eaf6ff;
                }
                td {
                    padding: 0.2rem 0;
                    text-align: center;
                    border-bottom: 1px solid #e4e4e4;
                }
                &.bgtr{
                  td {
                       background: #f7f7fc;

                       &.dhk{
                              color:#999;
                            }
                     }
                }
            }
        }
    }
}
</style>
