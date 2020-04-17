<template>
  <div class="RechargeCode">
    <div class="content">
      <div class="shadowBox">
        <div class="tp">充值码资产</div>
        <div class="bt">
          <router-link tag="div" to="/mycode" class="col">
            <div class="tit">总数额(SVC)</div>
            <div class="num">{{amount}}</div>
          </router-link>
          <router-link tag="div" to="/mycode" class="col">
            <div class="tit">数量(张)</div>
            <div class="num">{{count}}</div>
          </router-link>
        </div>
      </div>
      <div class="shadowBox">
        <div class="tp">充值码管理</div>
        <div class="bt">
          <router-link to="/mycode" tag="div" class="col mycode">
            <div class="logo">
              <img src="@/assets/images/ic_code.png" alt="">
            </div>
            <div class="tit">我的充值码</div>
          </router-link>
          <router-link to="/rechargerecord" tag="div" class="col mycode">
            <div class="logo">
              <img src="@/assets/images/ic_record.png" alt="">
            </div>
            <div class="tit">充值码记录</div>
          </router-link>
          <router-link to="/retail" tag="div" class="col mycode" v-if="issale">
            <div class="logo">
              <img src="@/assets/images/ic_retaily.png" alt="">
            </div>
            <div class="tit">零售</div>
          </router-link>
        </div>
      </div>
      <router-link to="/buycode" tag="div" class="btn buyBtn">购买充值码</router-link>
      <div class="btn rechargeBtn" @click="popup('卡号充值')">卡号充值</div>
      <div class="explain">
        <p>温馨提示：</p>
        <p>1. 充值码(SVC)可用充值SV</p>
        <p>2. 充值码购买后永久有效，充值本金不可提现</p>
      </div>
    </div>
    <popup :title="popTitle"
           v-if="popupFlag"
           :popFlag="popupFlag"
           @popupSubmit="submit"
           @popupFlag="popupFn" />
  </div>

</template>

<script>
  const popup = () => import("@/components/BotPopup");
  import { getUrlParams, setStore, getStore } from '@/config/utils'
  import { GetMySvcStatis, SvcToSvByCard} from '@/api/getData'
import { all } from 'q';
export default {
    data () {
        return {
          popupFlag: false,       // 弹出框显示
          cardno:'',
            popTitle: '',           // 弹出框标题
            amount:0,
            count: 0,
          issale: false,
        }
    },
    mounted() {
      this.setdata();
    },
    methods: {
        popup(poptit) {     // 弹出弹框
            this.popupFlag = true;
            this.popTitle = poptit;
        },
        popupFn(data) {     // 接受子传值，关闭弹窗
            this.popupFlag = data;
      },
      //确认充值
      submit: function (data) {
        if (data == '') {
          this.Toast("请输入卡号");
          return false;
        }
        this.cardno = data;
        //充值
        this.SvcToSvByCard();
        this.popupFlag = false;
      },
      async SvcToSvByCard() {
        let result = await SvcToSvByCard(JSON.parse(sessionStorage.userParam), this.cardno);
        if (result.result > 0) {
          this.Toast(result.message);
        } else {
          this.Toast(result.message);
        }
      },
      //获取充值码统计
        async GetMySvcStatis(){
            let result = await GetMySvcStatis(JSON.parse(sessionStorage.userParam));
            if (result.result > 0) {
                if (result.data.length == 0) this.empty = true;
                this.amount = result.data.amount;
                this.count = result.data.count;
                this.issale = result.data.issale;
                this.$store.issale = result.data.issale;
            } else {
                this.Toast(result.message);
            }
        },
        setdata:function(){
            this.GetMySvcStatis();

        },

    },
    components: {
      popup
    }
  }
</script>

<style lang="scss" scoped>
  .RechargeCode {
    min-height: 100%;
    .content

  {
    padding: 0 0.3rem;
    .shadowBox

  {
    padding: 0.3rem;
    margin: 0.5rem 0;
    box-shadow: 0 0.02rem 0.3rem 0 rgba(141, 171, 196, 0.15);
    border-radius: 0.04rem;
    .tp

  {
    font-size: 0.28rem;
    color: #000;
    font-weight: bold;
    font-family: PingFangSC-Medium;
  }

  .bt {
    display: flex;
    margin-top: 0.5rem;
    .col

  {
    flex: auto;
    .tit

  {
    font-size: 0.24rem;
    color: #999;
  }

  .num {
    margin-top: 0.1rem;
    font-size: 0.32rem;
    color: #333;
  }

  }

  .mycode {
    display: flex;
    flex-direction: column;
    align-items: center;
    .logo

  {
    margin-bottom: 0.2rem;
    img

  {
    height: 0.54rem;
    width: auto;
  }

  }
  }
  }
  }

  .btn {
    text-align: center;
    border-radius: 0.04rem;
    padding: 0.21rem 0;
    font-size: 0.32rem;
    margin: 0 0.5rem 0.4rem 0.5rem;
    &.buyBtn

  {
    margin-top: 1.7rem;
    background-color: #2ea2fa;
    color: #fff;
    box-shadow: 0 0.04rem 0.2rem 0 rgba(46, 162, 250, 0.35);
  }

  &.rechargeBtn {
    color: #2ea2fa;
    border: solid 1px #2ea2fa;
  }

  }

  .explain {
    font-size: 0.24rem;
    color: #999;
    line-height: 0.4rem;
    p

  {
    padding: 0 0.5rem;
    margin: 0;
  }

  }
  }
  }
</style>
