<template>
  <div class="buycode">
    <div class="topbx">
      <div class="bgbx">
        <img src="@/assets/images/bg_buy_sv.png" alt />
      </div>
      <div class="denomination">
        <div class="tit">充值码(SVC)面额</div>
        <div class="num">
          <div :class="item.select ? 'item select' : 'item'"
               @click="selNum(item)"
               v-for="item in denomination"
               integer
               :key="item.index">
            <span>{{ item.price }}</span>
          </div>
        </div>
      </div>
      <div class="number">
        <div class="txt">购买数量：</div>
        <van-stepper v-model="number" min="0" @change="onChange" />
      </div>
    </div>
    <div class="payment">
      <div class="litit">选择支付方式</div>
      <div class="paylist">
        <div class="paytype" @click='DCEPClick'>
          <div class="Img">
            <img src="@/assets/images/icon_pay_dcep.png" alt />
          </div>
          <div class="txt">DCEP支付</div>
          <div class="select selected">
            <img class="sel" :src="payIndex ==0 ?require('@/assets/images/ic_selected_pay.png'):require('@/assets/images/ic_unselected_pay.png')" alt />
          </div>
        </div>
        <!-- 微信 -->
        <div class="paytype" @click='payIndex=1'>
          <div class="Img">
            <img src="@/assets/images/ic_wechat_pay.png" alt />
          </div>
          <div class="txt">微信支付</div>
          <div class="select selected">
            <img class="sel" :src="payIndex == 1 ?require('@/assets/images/ic_selected_pay.png'):require('@/assets/images/ic_unselected_pay.png')" alt />
          </div>
        </div>
        <!-- UV支付 -->
        <div class="paytype" @click='payIndex=2'>
          <div class="Img">
            <img src="@/assets/images/ic_uv_pay.png" alt />
          </div>
          <div class="txt">UV支付</div>
          <div class="select selected">
            <img class="sel" :src="payIndex==2 ? require('@/assets/images/ic_selected_pay.png'):require('@/assets/images/ic_unselected_pay.png')" alt />
          </div>
        </div>

        <div v-show="false">
          <div class="more" v-if="!moreFlag">
            <span @click="moreFlag = true" style="color:#999">查看更多支付方式</span>
          </div>
          <div v-else>
            <div class="paytype">
              <div class="Img">
                <img src="@/assets/images/ic_zfb_pay.png" alt />
              </div>
              <div class="txt">支付宝</div>
              <div class="select" @click="selectPay($event,2)">
                <img class="sel" src="@/assets/images/ic_selected_pay.png" alt />
                <img class="nosel" src="@/assets/images/ic_unselected_pay.png" alt />
              </div>
            </div>
            <div class="paytype">
              <div class="Img">
                <img src="@/assets/images/ic_bank_pay.png" alt />
              </div>
              <div class="txt">银行卡</div>
              <div class="select" @click="selectPay($event,3)">
                <img class="sel" src="@/assets/images/ic_selected_pay.png" alt />
                <img class="nosel" src="@/assets/images/ic_unselected_pay.png" alt />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="option">
      <div class="lft">立即充值SV余额</div>
      <div class="rgt" @click="rechargeSV=!rechargeSV">
        <img v-if="rechargeSV" src="@/assets/images/ic_on.png" alt />
        <img v-else src="@/assets/images/ic_off.png" alt />
      </div>
    </div>
    <div class="agreement">
      <div class="sel" @click="agree = !agree">
        <img v-if="agree" src="@/assets/images/ic_agree.png" alt />
        <img v-else src="@/assets/images/ic_disagree.png" alt />
      </div>
      <div class="text">
        阅读并同意
        <a href="/html/recharge.html">《用户充值协议》</a>
      </div>
    </div>
    <div class="totalBx">
      <div class="txt">共{{sumamount}}{{rechargeSV?"SV":"SVC"}}，合计：</div>
      <div class="num">{{payIndex==2?rmbamount+"UV":"¥"+rmbamount}}</div>
      <button @click="buy">确认购买</button>
    </div>
    <keyboard theme="keyboard" :isKeyboard="payPwdFlag" @close="fatherHide" @pay="fatherConfirm"></keyboard>
  </div>
</template>

<script>
import { GetSvcConfig, BuySvc, Success } from "@/api/getData";
export default {
  data() {
    return {
      denomination: [],
      number: 1, // 购买数量
      agree: false, // 阅读并同意《用户充值协议》
      payment: 1, // 支付方式
      moreFlag: false, // 查看更多支付方式
      rechargeSV: true, // 立即充值SV
      amount: 1000,
      sumamount: 1000,
      rmbamount: 1000,

      //支付index
      payIndex: 1,
      payPwdFlag: false,
    };
  },
  mounted() {
    this.setdata();
    //APP回调
    window.dosPayResult = function(obj) {
      try {
        var ret = JSON.parse(obj);
        if (ret.result <= 0) {
          Toast({
            message: ret.message,
            iconClass: "icon icon-error"
          });
          return;
        } else {
          Toast({
            message: "购买成功",
            iconClass: "icon icon-error"
          });
        }
      } catch (e) {
        alert("支付异常:" + e);
      }
    };
  },
  methods: {
    DCEPClick(){
      this.$toast("DCEP支付方式正在开发中！") ;
    },
    onChange() {
      this.sumamount = this.amount * this.number;
      this.rmbamount = this.fixnumber(this.sumamount * 1);
    },
    selNum(data) {
      // 选择面额
      if (!data.select) {
        for (const item of this.denomination) {
          this.$set(item, "select", false);
        }
        this.$set(data, "select", true);
      }
      this.amount = data.price;
      this.sumamount = data.price * this.number;
      this.rmbamount = this.fixnumber(this.sumamount * 1);
    },
    // selectPay(e, index) {
    //   var paytype = document
    //     .querySelector(".payment")
    //     .querySelectorAll(".select");
    //   for (const itm of paytype) {
    //     itm.className = "select";
    //   }
    //   e.currentTarget.className += " selected";
    //   this.payment = index;
    // },
    async GetSvcConfig() {
      let result = await GetSvcConfig(JSON.parse(sessionStorage.userParam));
      if (result.result > 0) {
        if (result.data.length == 0) this.empty = true;
        this.denomination = result.data[0].list;
        this.denomination[0].select = true;
        this.amount = this.denomination[0].price;
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
        }
        this.BuySvc(data);
    },
    async BuySvc(data) {
      let result = await BuySvc(
        JSON.parse(sessionStorage.userParam),
        this.payIndex,
        this.amount,
        this.number,
        this.rechargeSV ? 1 : 0,
        data
      );
      if (result.result > 0) {
        if (this.payIndex == 1) {
          try {
            AppNative.blJsTunedupNativeWithTypeParamSign(1005, result.data.chargestr, result.data.sign);
          } catch (e) {
            this.Toast("调用微信支付失败");
          }
        } else {
          this.Toast(result.message);
        }
 
      } else {
        this.Toast(result.message);
      }
    },
    setdata: function () {
      this.GetSvcConfig();
    },
    fixnumber: function(data) {
      let x = (data * 1000) % 10;
      data = data * 100;
      if (x > 0) {
        data = data * 100 + 1;
      }
      return Math.floor(data) / 100;
    },
    buy: function() {
      //this.Toast("暂未开放，敬请期待");
      //return false;

      if (!this.agree) {
        this.Toast("请先阅读并同意用户协议");
        return false;
      }
      //if (this.payment != 1 && this.payment != 2) {
      //  this.Toast("暂时只支持微信/UV支付");
      //  return false;
      //}
      if (this.number <= 0) {
        this.Toast("至少购买一个");
        return false;
      }
      if (this.payIndex == 0) {
        this.Toast("正在开发接入中...");
        return false;
      }
 
      if (this.payIndex == 1) {
        this.BuySvc("");
      } else {
        this.payPwdFlag = true;
      }
    },
    agreeFun: function() {
      this.$router.push({ path: "/agreement" });
    }
  }
};
</script>

<style lang="scss" scoped>
.buycode {
  min-height: 100%;
  background: #f7f7fc;
  padding-bottom: 1rem;
  .topbx {
    background: #fff;
    padding: 0.3rem 0.4rem 0 0.4rem;
    .bgbx {
      img {
        width: 100%;
        height: auto;
      }
    }
    .denomination {
      padding-bottom: 0.3rem;
      border-bottom: 1px solid #dedede;
      .tit {
        font-size: 0.28rem;
        padding: 0.1rem 0 0.3rem;
      }
      .num {
        .item {
          display: inline-block;
          box-sizing: border-box;
          width: 33%;
          span {
            width: 90%;
            display: block;
            color: #2ea2fa;
            border: 1px solid #2ea2fa;
            border-radius: 0.06rem;
            text-align: center;
            padding: 0.2rem 0;
          }
          &.select {
            span {
              background: #2ea2fa;
              color: #fff;
              border: 1px solid #2ea2fa;
            }
          }
        }
      }
    }
    .number {
      display: flex;
      align-items: center;
      padding: 0.25rem 0;
      .txt {
        flex: auto;
      }
    }
  }
  .payment {
    .litit {
      font-size: 0.24rem;
      color: #999;
      padding: 0.2rem 0 0.1rem 0.3rem;
    }
    .paylist {
      background: #fff;
      padding: 0 0.3rem;
      .paytype {
        display: flex;
        padding: 0.3rem 0;
        align-items: center;
        border-bottom: solid 1px #eeeeee;
        .Img {
          display: flex;
          img {
            width: 0.6rem;
            height: 0.6rem;
          }
        }
        .txt {
          flex: auto;
          margin-left: 0.3rem;
        }
        .select {
          img {
            width: 0.48rem;
            height: 0.48rem;
            &.sel {
              display: none;
            }
          }
          &.selected {
            .sel {
              display: block;
            }
            .nosel {
              display: none;
            }
          }
        }
      }
      .more {
        display: flex;
        justify-content: center;
        padding: 0.2rem 0;
        font-size: 0.24rem;
        color: #999;
      }
    }
  }
  .option {
    padding: 0.24rem 0.3rem;
    display: flex;
    align-items: center;
    margin-top: 0.2rem;
    background: #fff;
    .lft {
      flex: auto;
    }
    .rgt {
      display: flex;
      align-items: center;
      img {
        width: 0.92rem;
        height: auto;
      }
    }
  }
  .agreement {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 0.3rem 0;
    .sel {
      margin-right: 0.1rem;
      display: flex;
      img {
        width: 0.36rem;
        height: 0.36rem;
      }
    }
    .text {
      span {
        color: #2ea2fa;
      }
    }
  }
  .totalBx {
    width: 100%;
    position: fixed;
    bottom: 0;
    background: #fff;
    display: flex;
    align-items: center;
    padding: 0.13rem 0.3rem;
    box-sizing: border-box;
    .txt {
      flex: auto;
      text-align: right;
      font-size: 0.28rem;
    }
    .num {
      color: #2ea2fa;
      font-size: 0.32rem;
      font-weight: bold;
    }
    button {
      padding: 0.16rem 0.36rem;
      font-size: 0.28rem;
      color: #fff;
      background: #2ea2fa;
      border: none;
      outline: none;
      border-radius: 0.06rem;
      margin-left: 0.3rem;
    }
  }
}
@supports (bottom: env(safe-area-inset-bottom)) {
  .totalBx {
    padding-bottom: env(safe-area-inset-bottom);
  }
}
</style>
