<template>
  <div class="retail" >
    <div class="content">
      <div class="topbx">
        <div class="bgtxt">
          <div class="tt">充值码(SVC)</div>
          <span>类型</span>
        </div>
        <div class="tit">{{typename}}库存（零售）余额</div>
        <div class="num">{{stocknum}}</div>
      </div>
      <div class="infobx">
        <div class="tit">零售账号</div>
        <div class="ops account"><span>账号：{{user.nodecode}}</span><button @click="userChange">更改</button></div>
        <div class="ops">姓名：{{user.name}}</div>
        <div class="ops">手机号：{{user.phone}}</div>
      </div>
      <div class="selectNum">
        <div class="tit">选择规格</div>
        <div class="opt" v-for="(item,index) in list" :key='index'>
          <div class="lft">{{item.price}}{{typename}}</div>
          <van-stepper v-model="item.val" min="0" integer @change="onChange(item)" />
        </div>
        <div class="totalbx">共 <span>{{count}}</span> 张，<span>{{sumprice}}</span> {{typename}}</div>
      </div>
      <div class="retailbtn" @click="sale">零售</div>
      <div class="bgtxt">
        <p>温馨提示：</p>
        <p>1.零售的充值码额度最小为1000SV</p>
        <p>2.零售需要扣除相应的SV余额</p>
        <p>3.零售后的充值码将直接转让给别人</p>
      </div>
    </div>
    <popup title="零售"
           v-if="popupFlag"
           :popFlag="popupFlag"
           @popupSubmit="submit"
           @popupFlag="popupFn" />
    <keyboard theme="money" :isKeyboard="payPwdFlag" :payPrice="sumprice+'SV'" balanceName="SV" :balance='stocknum' :balanceIcon="require('@/assets/images/icon_sv.png')" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
  </div>
</template>

<script>
  const popup = () => import("@/components/BotPopup");
  import { GetSvcConfig, GetUserInfo, SaleSvc } from '@/api/getData'
export default {
    data () {
      return {
        configlist: [],
        list:[],
        count: 0,
        sumprice: 0,
        typeid: 0,
        popupFlag: false,
        payPwdFlag: false,
        typename: "SV",
        stocknum:0,
        user: {}
        }
    },
    mounted() {
      this.setdata();
    },
    methods: {
      popupFn(data) {     // 接受子传值，关闭弹窗
        this.popupFlag = data;
      },
      submit: function (data) {
        if (data == "") {
          this.Toast("请输入账号");
          return false;
        }
        this.popupFlag = false;
        this.GetUserInfo(data);
      },
      async GetSvcConfig() {
        let result = await GetSvcConfig(JSON.parse(sessionStorage.userParam));
        if (result.result > 0) {
          if (result.data.length == 0) this.empty = true;
          let _this = this;
          _this.configlist = result.data;
          result.data.forEach(function (item, index) {
            item.list.forEach(function (item1,index1) {
              _this.configlist[index].list[index1].val = 0;
            })
          })
          this.typename = this.configlist[this.typeid].typename;
          this.stocknum = this.configlist[this.typeid].stocknum;
          this.list = this.configlist[this.typeid].list;
        } else {
          this.Toast(result.message);
        }
      },
      async SaleSvc(data, cards) {
        let result = await SaleSvc(JSON.parse(sessionStorage.userParam), cards, this.user.nodecode, data);
        if (result.result > 0) {
          this.Toast(result.message);
          this.stocknum = this.stocknum - this.sumprice;
          this.list.forEach(c => c.val = 0);
        } else {
          this.Toast(result.message);
        }
      },
      async GetUserInfo(data) {
        let result = await GetUserInfo(JSON.parse(sessionStorage.userParam), data,0);
        if (result.result > 0) {
          this.user = result.data;
        } else {
          this.Toast(result.message);
        }
      },
      fatherHide: function () {
        this.payPwdFlag = false;
      },
      fatherConfirm: function (data) {
        this.payPwdFlag = false;
        if (data.length < 6) {
          this.Toast("密码错误");
          return false;
        }

        let cards = '';
        let i = 0;
        this.configlist[0].list.forEach(function (item, index) {
          if (item.val > 0) {
            if (i==0) {
              cards = item.configid + "|" + item.val;
              i++;
            } else {
              cards = cards + "," + item.configid + "|" + item.val;
            }
          }
        })
        if (cards == '') {
          this.Toast("至少零售一张卡");
          return false;
        }

        this.SaleSvc(data, cards);
      },
      setdata: function () {
        this.popupFlag = true;
        this.GetSvcConfig();

      },
      sale: function () {
        if (this.user.nodecode == null || this.user.nodecode == undefined) {
          this.Toast("请先选择目标用户");
          this.popupFlag = true;
          return false;
        }
        if (this.sumprice > this.stocknum) {
          this.Toast("库存不足");
          return false;
        }
        if (this.count <= 0) {
          this.Toast("请至少选择一种规格");
          return false;
        }
        this.payPwdFlag = true;
      },
      onChange: function () {
        let num = 0;
        let sum = 0;
        this.configlist[0].list.forEach(function (item,index) {
          num = num + item.val;
          sum = sum + item.val * item.price;
        })
        this.count = num;
        this.sumprice = sum;
      },
      userChange: function () {
        this.popupFlag = true;
      }

    },
    components: {
      popup
    }
}
</script>

<style lang="scss" scoped>
.retail {
    .content {
        padding: 0 0.3rem;
        .topbx {
            border-bottom: 1px solid #dedede;
            .bgtxt {
                display: flex;
                font-size: 0.24rem;
                color: #999;
                background: #f7f7fc;
                margin-top: 0.4rem;
                padding: 0.25rem 0.3rem;
                .tt {
                    flex: auto;
                    color: #000;
                    font-size: 0.28rem;
                }
            }
            .tit {
                font-size: 0.24rem;
            }
            .num {
                font-size: 0.28rem;
                padding: 0.2rem 0;
            }
        }
        .infobx {
            padding: 0.4rem 0 0.2rem;
            border-bottom: 1px solid #dedede;
            .tit {
                font-size: 0.24rem;
                padding-bottom: 0.2rem;
            }
            .ops {
                font-size: 0.24rem;
                color: #999;
                padding-bottom: 0.1rem;
                &.account {
                    display: flex;
                    span {
                        flex: auto;
                    }
                    button {
                        border: solid 1px #2ea2fa;
                        outline: none;
                        color: #2ea2fa;
                        background: #fff;
                        border-radius: 0.04rem;
                    }
                }
            }
        }
        .selectNum {
            padding-top: 0.4rem;
            .tit {
                font-size: 0.24rem;
            }
            .opt {
                display: flex;
                align-items: center;
                padding: 0.14rem;
                border-bottom: 1px solid #dedede;
                font-size: 0.28rem;
                .lft {
                    flex: auto;
                }
            }
            .totalbx {
                text-align: right;
                font-size: 0.28rem;
                padding: 0.2rem;
                span {
                    color: #2ea2fa;
                }
            }
        }
        .retailbtn {
            background: #2ea2fa;
            color: #fff;
            font-size: 0.32rem;
            border-radius: 0.04rem;
            text-align: center;
            padding: 0.2rem;
            margin-top: 0.6rem;
        }
        .bgtxt {
            background: #f7f7fc;
            font-size: 0.24rem;
            color: #999;
            padding: 0.2rem;
            margin: 0.5rem 0;
            p {
                padding: 0;
                margin: 0;
                line-height: 0.4rem;
            }
        }
    }
}
</style>
