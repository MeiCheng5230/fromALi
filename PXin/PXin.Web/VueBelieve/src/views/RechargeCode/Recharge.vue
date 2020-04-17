<template>
  <div class="Recharge" >
    <div class="mainCom">
      <div class="topBg" :style="{backgroundImage:'url('+require('@/assets/images/bg_SVC.png')+')'}">
        <div class="cardTit">已选充值码</div>
        <div class="cardNum"><span>{{data.num}}</span>张</div>
      </div>
      <div class="infoBx" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
        <div class="tit">账号信息</div>
        <div class="info ID"><span>账号：{{user.nodecode}}{{user.nodecode?user.iszys==1?"(充值商)":"(其他)":""}}</span><button @click="changeFn('input')">更改</button></div>
        <div class="info">姓名：{{user.name}}</div>
        <div class="info">手机号：{{user.phone}}</div>
      </div>
      <div class="separate" :style="{backgroundImage:'url('+require('@/assets/images/bg_2.png')+')'}"></div>
      <div class="codeBx" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
        <div class="tit">充值码</div>
        <div class="num" v-for="(item,index) in data.amountlist" :key="index">
          <span>SVC</span>
          <div>{{item.amount}}*{{item.num}}</div>
        </div>
      </div>
      <div class="separate" v-if="data.title==0" :style="{backgroundImage:'url('+require('@/assets/images/bg_2.png')+')'}"></div>
      <div class="codeBx ckBx" v-if="data.title==0" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
        <div class="tit">直接充值V点</div>
        <van-switch v-model="checked" size="0.42rem"/>
      </div>
      <div class="separate" :style="{backgroundImage:'url('+require('@/assets/images/bg_2.png')+')'}"></div>
      <div class="total" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
        <div>总计</div>
        <span>{{ checked? 'V点：': 'SV：'}}{{checked? data.sumamount*10: data.sumamount}}</span>
      </div>
      <div class="final" :style="{backgroundImage:'url('+require('@/assets/images/bg_3.png')+')'}"></div>
    </div>
    <div class="transfer_btn" @click="transfer">{{data.title==0?'充值':'转让'}}</div>
    <div class="remarks">
      <p>温馨提示：<br>1. 充值码(SVC)可用来充值SV余额，支持直接充值V点<br>2.直接充值V点比例：1SV=10V</p>
    </div>
    <div class="pop" v-if="inputFlag">
      <div class="popcom">
        <div :class="data.title==0?'tit':'titl'">{{title}}</div>
        <div class="close" @click="changeFn"><img src="@/assets/images/ic_cancel.png" alt=""></div>
        <div class="inp">
          <input type="text" v-model="nodecode" @input="nodein" placeholder="请输入账号/手机号">
        </div>
        <div class="Btn">
          <button @click="changeFn('change')">确定</button>
        </div>
      </div>
    </div>
    <keyboard theme="keyboard" :isKeyboard="payPwdFlag" @close="fatherHide" @pay="fatherConfirm"></keyboard>
  </div>
</template>

<script>
  import { RecoverySvc, GetUserInfo, SvcToSvByOwner } from '@/api/getData';
export default {
    data () {
        return {
          inputFlag: false,
          payPwdFlag: false,
          nodecode:'',
          data: {},
          user: {},
          title:'',
          checked: false, 
          isKeyboard: true,
          isvdnow: 0,        // 是否立即充值v点
        }
    },
    mounted() {
      this.setdata();
    },
    watch: {
      checked(val) {
        if (val) {
          this.isvdnow = 1
        } else {
          this.isvdnow = 0
        }
      }
    },
    methods: {
        changeFn(data) {  // 点击转让
            if (data == "input") {
                this.inputFlag = true;
            } else if (data == "change") {
              if (this.nodecode == "") {
                this.Toast("请输入账号");
                return false;
              }
              this.GetUserInfo();
            } else {
                this.inputFlag = false;
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
        if (this.data.title == 1) {
          this.RecoverySvc(data);
        } else if (this.data.title == 0) {
          this.SvcToSvByOwner(data);
        }
      },
      async SvcToSvByOwner(paypwd) {
        let result = await SvcToSvByOwner(JSON.parse(sessionStorage.userParam), this.data.cards, this.nodecode, paypwd,this.checked?1:0);
        if (result.result > 0) {
          this.Toast(result.message);
          setTimeout(() => {
            this.$router.go(-2);
          }, 1000);
        } else {
          this.Toast(result.message);
        }
      },
      async RecoverySvc(paypwd) {
        let result = await RecoverySvc(JSON.parse(sessionStorage.userParam), this.nodecode, this.data.cards, paypwd, this.isvdnow);
        if (result.result > 0) {
          this.Toast(result.message);
          setTimeout(() => {
            this.$router.go(-2);
          }, 1000);
        } else {
          this.Toast(result.message);
        }
      },
      async GetUserInfo() {
        let result = await GetUserInfo(JSON.parse(sessionStorage.userParam), this.nodecode, this.data.title==0?1:0);
        if (result.result > 0) {
          if (this.data.title == 1) {
            if (result.data.iszys == 1) {
                this.user = result.data;
                this.nodecode = result.data.nodecode;
                this.inputFlag = false;
              //if (result.data.isself == 0) {
              //  this.user = result.data;
              //  this.nodecode = result.data.nodecode;
              //  this.inputFlag = false;
              //} else {
              //  this.Toast("不能转让给自己");
              //  this.nodecode = "";
              //}
            } else {
              this.Toast("该用户不是充值商");
              this.nodecode = "";
            }
          } else {
            this.user = result.data;
            this.nodecode = result.data.nodecode;
            this.inputFlag = false;
          }
        } else {
          this.Toast(result.message);
        }
      },
      transfer: function () {
        if (this.data.cards == "") {
          this.Toast("至少选择一张卡");
          return false;
        }
        if (this.nodecode == "") {
          this.Toast("请先选择目标用户");
          this.inputFlag = true;
          return false;
        }
        if (this.data.title == 1 && this.user.iszys != 1) {
          this.Toast("只能转让给充值商");
          return false;
        }
        this.payPwdFlag = true;
      },
      setdata: function () {
        this.data = this.$route.params;
        if (this.data.title == 0) {
          this.title = "充值账号更改";
          this.nodecode = JSON.parse(sessionStorage.userParam).nodeid;
          this.GetUserInfo();
        } else {
          this.title = "*充值码(SVC)只能转让给充值商";
          this.inputFlag = true;
        }
      },
      nodein: function () {
        if (this.data.title == 0) {
          this.nodecode = this.nodecode.replace(/[^\w@.]/g, '');
        } else {
          this.nodecode = this.nodecode.replace(/[^\d]/g, '');
        }

      }
    }
}
</script>

<style lang="scss" scoped>
.Recharge {
    min-height: 100%;
    background: #f7f7fc;
    .mainCom {
        padding: 0.3rem;
        font-family: PingFangSC-Regular;
        .separate {
            background-repeat: no-repeat;
            background-size: 100% auto;
            height: 0.4rem;
        }
        .final {
            background-repeat: no-repeat;
            background-size: 100% auto;
            height: 0.4rem;
        }
        .topBg {
            background-repeat: no-repeat;
            background-size: 100% 100%;
            padding: 0.3rem 0.5rem;
            margin: 0 0.1rem;
            .cardTit {
                color: rgba(51, 51, 51, 0.5);
                font-size: 0.2rem;
            }
            .cardNum {
                color: #fff;
                font-size: 0.24rem;
                font-family: PingFangSC-Regular;
                padding-bottom: 0.7rem;
                span {
                    font-size: 1rem;
                }
            }
        }
        .infoBx {
            background-repeat: no-repeat;
            background-size: 100% 100%;
            padding: 0.2rem 0.4rem;
            font-size: 0.28rem;
            .info {
                color: #999;
                padding: 0.05rem 0;
            }
            .ID {
                display: flex;
                align-items: center;
                span {
                    flex: auto;
                }
                button {
                    background: #fff;
                    padding: 0.1rem 0.2rem;
                    font-size: 0.24rem;
                    color: #2ea2fa;
                    border: 1px #2ea2fa solid;
                    outline: none;
                    border-radius: 0.05rem;
                }
            }
        }
        .codeBx {
            background-size: 100% 100%;
            font-size: 0.28rem;
            padding: 0 0.4rem;
            .num {
                display: flex;
                color: #999;
                padding: 0.05rem 0;
                span {
                    flex: auto;
                }
            }
            &.ckBx {
              display: flex;
              align-items: center;
              .tit {
                flex: auto;
              }
            }
        }
        .total {
            background-size: 100% 100%;
            display: flex;
            font-size: 0.28rem;
            padding: 0 0.4rem;
            div {
                flex: auto;
                padding-bottom: 0.2rem;
            }
            span {
                color: #999;
            }
        }
    }
    .transfer_btn {
        margin: 0.8rem;
        text-align: center;
        font-size: 0.32rem;
        color: #fff;
        background: #2ea2fa;
        border-radius: 0.06rem;
        padding: 0.22rem;
        font-family: PingFangSC-Medium;
    }
    .remarks {
        padding: 0 0.8rem;
        font-size: 0.24rem;
        color: #999;
        letter-spacing: 1px;
        line-height: 0.35rem;
        padding-bottom: 0.3rem;
    }
    .pop {
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        z-index: 3;
        .popcom {
            background: #fff;
            position: fixed;
            bottom: 0;
            width: 100%;
            border-radius: 0.2rem 0.2rem 0 0;
            padding: 0 0.3rem;
            box-sizing: border-box;
            .tit {
                padding-top: 0.2rem;
                font-size: 0.32rem;
                color: #000;
            }
            .titl{
                padding-top: 0.2rem;
                font-size: 0.32rem;
                color: #b7b0b0;
            }
            .close {
                position: absolute;
                top: 0.2rem;
                right: 0.3rem;
                img {
                    width: 0.32rem;
                    height: 0.32rem;
                }
            }
            .inp {
                padding: 0.3rem 0;
                border-bottom: 1px solid #dedede;
                box-sizing: border-box;
                input {
                    border: none;
                    padding: 0 0.3rem;
                    width: 100%;
                    height: 0.8rem;
                    background: #f7f7fc;
                    box-sizing: border-box;
                    border-radius: 0.06rem;
                }
            }
            .Btn {
                width: 100%;
                padding: 0.6rem 0 0.3rem 0;
                button {
                    width: 100%;
                    padding: 0.2rem 0;
                    background: #2ea2fa;
                    border: none;
                    outline: none;
                    border-radius: 0.06rem;
                    color: #fff;
                }
            }
        }
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .popcom {
        padding-bottom: env(safe-area-inset-bottom);
    }
}
</style>
