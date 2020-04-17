<template>
  <div class="SVCexchange">
    <div class="exchange">
      <div class="tit">兑换类型</div>
      <div class="tab">
        <div :class="item.flag?'sel':''" v-for="(item,index) in typelist" :key='index' @click="tabFn(index)">{{item.name}}</div>
      </div>
      <div class="content">
        <div class="SVC" v-if="showFlag == 'SVC'">
          <div class="tit">选择规格</div>
          <div class="ops" v-for="(item1,index) in data.specs" :key='index'>
            <div class="lft">{{item1.showname}}SVC</div>
            <div class="rgt"><van-stepper v-model="item1.val" min="0" integer @change="onChange(item1)" /></div>
          </div>
          <div class="ops opbx">
            <div class="lft">{{rate}}DOS</div>
            <div class="rgt">
              <span style="margin-right: 0.1rem;">手续费</span>
              <img src="@/assets/images/red_packet_exchange_icon.png" alt="" @click="dialogFn">
            </div>
          </div>
          <div class="txt">充值码(SVC)：{{data.svc}}</div>
          <div class="confirmBtn" @click="confirmBtn">确定兑换</div>
        </div>

        <div class="SV" v-else-if="showFlag == 'SV'">
          <div class="tit">兑换数量</div>
          <div class="ops">
            <div class="lft"><input type="text" maxlength="10" v-model="inpNum" @input="clearNoNum" placeholder="请输入兑换数量"></div>
          </div>
          <div class="txt">充值码(SVC)：{{data.svc}}<span @click="all">全部兑换</span></div>
          <div class="confirmBtn" @click="confirmBtn1">确定兑换</div>
        </div>
      </div>
    </div>
    <keyboard theme="moneyMulti" :isKeyboard="payPwdFlag" :payPrice="sumprice" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
  </div>
</template>

<script>
  import { GetExchangeInfo, Exchange } from '@/api/getData'

import { Dialog } from 'vant';
export default {
    data () {
        return {
          showFlag: "SVC",      //tab类型切换标志
          type:1,               //当前兑换类型
          payPwdFlag: false,    //密码弹窗控制
          sumprice: 0,          //总计金额
          num:0,                //个数
          data: {},             //初始数据
          typelist: [],         //类型列表
          cards: [],            //回传卡表类型
          rate: 0,              //手续费
          inpNum:'',            //兑换数量
        }
    },
    mounted() {
      this.setdata();
    },
    methods: {
      //tab切换函数
      tabFn(index) {
        this.typelist.forEach(c => c.flag = false);
        this.typelist[index].flag = true;
        this.type = index + 1;
        this.showFlag = this.typelist[index].name =="SVC充值码"?"SVC":"SV";
        },
      dialogFn() {
          Dialog.alert({
              title: '服务费比例',
              message: '充值商1%DOS<br>代理人2%DOS<br>合伙人3%DOS',
              confirmButtonText: '知道了'
          }).then(() => {
              // on close
          });
      },
      //获取当前页面信息
      async GetExchangeInfo() {
        let result = await GetExchangeInfo(JSON.parse(sessionStorage.userParam));
        if (result.result > 0) {
          this.data = result.data;
          this.data.specs.forEach(c => c.val = 0);
          this.data.specs[0].val = 1;
          this.onChange();
          this.data.type.forEach(c => this.typelist.push({ name: c, flag: c == "SV" ? false : true }))
        } else {
          this.Toast(result.message);
        }
      },
      //兑换
      async Exchange(data) {
        let result = await Exchange(JSON.parse(sessionStorage.userParam), this.type, this.cards, data, this.inpNum == '' ? 0 : this.inpNum);
        if (result.result > 0) {
          this.Toast("兑换成功");
          this.rate = 0;
          this.data.svc -= this.type == 1 ? this.sumprice : this.inpNum == '' ? 0 : this.inpNum;
          this.data.specs.forEach(c => c.val = 0);
          this.inpNum = '';
        } else {
          this.Toast(result.message);
        }
      },
      setdata: function () {
        this.GetExchangeInfo();

      },
      //选择面额
      onChange: function (item) {
        let num = 0;
        let sum = 0;
        this.data.specs.forEach(function (item, index) {
          num = num + item.val;
          sum = sum + item.val * parseInt(item.showname);
        })

        this.sumprice = sum;
        this.rate = this.sumprice * this.data.rate/100;
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

        this.Exchange(data);
      },
      //确认兑换充值码(SVC)
      confirmBtn: function () {

        if (this.data.svc < this.sumprice) {
          this.Toast("库存不足");
          return false;
        }
        let cards = [];
        this.data.specs.forEach(function (item, index) {
          if (item.val > 0) {
            cards.push({ infoid: item.infoid, num: item.val });
          }
        })
        if (cards.length == 0) {
          this.Toast("至少兑换一张卡");
          return false;
        }

        this.cards = cards;
        this.payPwdFlag = true;
      },
      //兑换sv
      confirmBtn1: function () {
        if (this.inpNum == '') {
          this.Toast("请输入兑换数量");
          return false;
        }
        if (this.data.svc < this.inpNum) {
          this.Toast("库存不足");
          return false;
        }
        this.sumprice = this.inpNum;
        this.payPwdFlag = true;
      },
      //全部兑换
      all: function () {
        this.inpNum = this.data.svc;
        this.sumprice = this.inpNum;
        this.payPwdFlag = true;
      },
      clearNoNum:function (obj){
        this.inpNum = this.inpNum.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符
        this.inpNum = this.inpNum.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的
        this.inpNum = this.inpNum.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        this.inpNum = this.inpNum.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数
        if (this.inpNum.indexOf(".") < 0 && this.inpNum != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额
          this.inpNum = parseFloat(this.inpNum);
        }
     }
    }
}
</script>

<style lang="scss" scoped>
.SVCexchange {
    min-height: 100%;
    background: #f7f7fc;
    display: flex;
    flex-direction: column;
    .exchange {
        flex: auto;
        width: 100%;
        display: flex;
        flex-direction: column;
        .tit {
            padding: 0.3rem;
        }
        .tab {
            display: flex;
            div {
                width: 1.8rem;
	            height: 0.8rem;
                text-align: center;
                line-height: 0.8rem;
                border-radius: 0.04rem;
	            border: solid 1px #999999;
                margin-left: 0.3rem;
                box-sizing: border-box;
                &.sel {
                    border: none;
                    background: #2ea2fa;
                    color: #fff;
                }
            }
        }
        .content {
            flex: auto;
            background: #fff;
            border-radius: 0.6rem 0.6rem 0 0;
            margin-top: 0.6rem;
            padding-bottom: 0.5rem;
            box-sizing: border-box;
            display: flex;
            flex-direction: column;
        }
        .SVC {
            flex: auto;
            padding: 0 0.3rem;
            display: flex;
            flex-direction: column;
            .tit {
                padding-left: 0;
            }
            .txt {
                flex: auto;
                padding: 0.3rem 0;
            }
            .ops {
                display: flex;
                align-items: center;
                border-bottom: 1px solid #dedede;
                font-size: 0.28rem;
                padding: 0.1rem 0;
                .lft {
                    flex: auto;

                }
                &:last-of-type {
                    border: none;
                }
                &.opbx {
                    background: #f7f7fc;
                    margin-top: 1.2rem;
                    padding: 0.3rem;
                    border-radius: 0.04rem;
                    border: none;
                    .rgt {
                        display: flex;
                        align-items: center;
                        img {
                            width: 0.3rem;
                            height: 0.3rem;
                        }
                    }
                }
            }
            .confirmBtn {
                background: #2ea2fa;
                color: #fff;
                border-radius: 0.06rem;
                text-align: center;
                padding: 0.2rem;
            }
        }
        .SV {
            flex: auto;
            display: flex;
            flex-direction: column;
            padding: 0.3rem;
            .ops {
                background: #f7f7fc;
                padding: 0.3rem;
                border-radius: 0.04rem;
                color: #999;
                margin-top: 0.3rem;
                .lft{
                      input{
                           border:none;
                           background-color:#f7f7fc;
                           color:black;
                           &::placeholder {
                                            color:#999;
                                          }
                         }
                    }
            }
            .txt {
                padding: 0.2rem;
                flex: auto;
                span {
                    color: #2ea2fa;
                    margin-left: 0.3rem;
                }
            }
            .confirmBtn {
                background: #2ea2fa;
                color: #fff;
                border-radius: 0.06rem;
                text-align: center;
                padding: 0.2rem;
                margin-top: 2rem;
            }
        }
    }
}
</style>
