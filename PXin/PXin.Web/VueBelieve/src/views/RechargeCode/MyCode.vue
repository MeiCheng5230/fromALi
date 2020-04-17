<template>
    <div class="mycode" >
        <div class="codelist">
          <div class="item" v-for="item in codelist" :key="item.index" :style="{backgroundImage:'url('+require('@/assets/images/bg_SVC.png')+')'}">
            <div class="tit">充值码(SVC)</div>
            <div class="num">{{ item.num || 0 }}</div>
            <div class="tit">编号</div>
            <div class="codeid">{{ item.codeid || 0 }}</div>
            <div class="select" @click="selectFn(item)">
              <img v-if="item.select" src="@/assets/images/ic_selected_code.png" alt="">
              <img v-else src="@/assets/images/ic_unselected_code.png" alt="">
            </div>
          </div>
          <nodata v-if="nodataFlag" />
        </div>
        <div class="botBtn">
            <div class="btn recharge" @click="recharge(0)">充值</div>
            <div class="btn transfer" @click="recharge(1)">转让</div>
            <div class="btn recharge" @click="setAllSelect"><span v-show="!allselect">全选</span><span v-show="allselect">全不选</span></div>
        </div>
        <popup title="*充值码(SVC)只能转让给充值商"
            v-if="popupFlag"
            :popFlag="popupFlag"
            @popupFlag="popupFn"/>
    </div>
</template>

<script>
const popup = () => import("@/components/BotPopup");
const nodata = () => import("@/components/noData");
import { GetMySvc } from '@/api/getData';
import { setStore } from '@/config/utils';
export default {
    data () {
        return {
            popupFlag: false,   // 弹出框显示
            nodataFlag: false,
            codelist: [],
            num: 0,
            sumamount: 0,
            amountlist:[],
            allselect: false,
        }
    },
    mounted(){
      this.setdata();
    },
    methods: {
        selectFn(data) {    // 选择/取消充值码
          this.amountlist = [];
          this.sumamount = 0;
          this.num = 0;
          this.$set(data, 'select', !data.select);
          for (const item of this.codelist) {
            if (item.select) {
              this.sumamount += item.num;
              this.num ++;
              this.amountlist.push({
                amount: item.num,
                num:1
              })
            }
          }
        },
        popupFn(data) {     // 接受子传值，关闭弹窗
            this.popupFlag = data;
        },
        setdata:function(){
            this.GetMySvc();
        },
        async GetMySvc(){
            let result = await GetMySvc(JSON.parse(sessionStorage.userParam));
            if (result.result > 0) {
              if (result.data.length == 0) this.empty = true;
              let _this = this;
              result.data.forEach((item, index) => {
                _this.codelist.push({
                  num:item.amount,
                  codeid: 'NO. ' + item.cardno,
                  code: item.cardno,
                });
              });
              if (this.codelist.length == 0) {
                this.nodataFlag = true;
              } else {
                this.nodataFlag = false;
                this.selectFn(this.codelist[0]);
              }
            } else {
                this.Toast(result.message);
            }
      },
      recharge: function (type) {
        let cardno = '';
        let i = 0;
        let _this = this;
        this.amountlist.forEach(function (item,index) {
          if (item.num <= 0) {
            _this.amountlist.splice(index,1);
          }
        })
        this.codelist.forEach(function (item,index) {
          if (item.select) {
            if (i == 0) {
              cardno = item.code;
            } else {
              cardno = cardno + "," + item.code
            }
            i++;
          }
        })
        if (cardno == "") {
          this.Toast("至少选择一张卡");
          return false;
        }
        if (type == 0) {
          setStore("nextTitle", "充值");
        } else {
          setStore("nextTitle", "转让");
        }
        this.$router.push({
          name: 'recharge',
          params: {
            num: this.num,
            sumamount: this.sumamount,
            cards: cardno,
            amountlist: this.amountlist,
            title: type
          }
        })
      },
      setAllSelect() {
        this.amountlist = [];
        this.allselect = !this.allselect;
        for (const item of this.codelist) {
          this.$set(item, 'select', this.allselect);
        };
        if (!this.allselect) {
          this.$set(this.codelist[0], 'select', true);
        };

        this.amountlist = [];
        this.num = 0;
        this.sumamount = 0;
        for (const item of this.codelist) {
          if (item.select) {
            this.sumamount += item.num;
            this.num ++;
            this.amountlist.push({
                amount: item.num,
                num:1
            })
          }
        }
      }
    },
    components: {
      popup, nodata
    }
}
</script>

<style lang="scss" scoped>
.mycode {
    min-height: 100%;
    background: #f7f7fc;
    .codelist {
        padding: 0.3rem 0.4rem 1rem 0.4rem;
        .item {
            background-size: 100% 100%;
            background-repeat: no-repeat;
            padding: 0.3rem 0.3rem 0.3rem 0.5rem;
            margin-bottom: 0.3rem;
            position: relative;
            .tit {
                font-size: 0.2rem;
                color: rgba(51, 51, 51, 0.5);
                font-family: PingFangSC-Regular;
            }
            .num {

                font-size: 0.6rem;
                color: #fff;
                margin-bottom: 0.6rem;
            }
            .codeid {
                font-size: 0.2rem;
                color: #fff;
                margin-top: 0.1rem;
            }
            .select {
                position: absolute;
                right: 0.3rem;
                top: 50%;
                transform: translateY(-50%);
                img {
                    width: 0.44rem;
                    height: 0.44rem;
                }
            }
        }
    }
    .botBtn {
        display: flex;
        position: fixed;
        bottom: 0;
        width: 100%;
        .btn {
            flex: auto;
            text-align: center;
            font-size: 0.32rem;
            color: #fff;
            background: #fff;
            padding: 0.26rem 0;
            &.transfer {
                color: #2ea2fa;
            }
            &.recharge {
                background: #2ea2fa;
            }
        }

    }
}

@supports (bottom: env(safe-area-inset-bottom)) {
    .botBtn {
      padding-bottom: env(safe-area-inset-bottom);
    }
    .popup {
       padding-bottom: env(safe-area-inset-bottom);
    }
}
</style>
