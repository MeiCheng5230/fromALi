<template>
    <div class="SVCtransfer">
        <div class="bgBx">
            <img class="arcbg" src="@/assets/images/excharge_bg.png" alt=""/>
            <img :src="details.pic" alt="">
        </div>
        <div class="content">
            <div class="ops">
                <div class="lft">{{details.name}}<span v-if="details.typeid==2">({{details.pdtvalue}}面额)</span></div>
                <div class="rgt">{{details.price}}{{details.priceunit}}</div>
            </div>
            <div class="infoBx num">
                <div class="lft">购买数量</div>
                <van-stepper v-model="value" integer />
            </div>
            <div class="infoBx txt">
                <div>商品说明</div>
                <dl v-html="details.note">
                </dl>
            </div>
        </div>
        <div class="botBx">
            <div class="total">总计：{{payamount}}{{details.priceunit}}</div>
            <button @click="exchange">确定兑换</button>
        </div>

      <keyboard theme="moneyMulti" :isKeyboard="isPay" :payPrice="payamount+''+details.priceunit" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
    </div>
</template>

<script>

import {getStore} from "@/config/utils";
import { ProductRecharge} from '@/api/getData';
export default {
    data () {
        return {
            value: 1,
            payamount:0,
            details:{},
            isPay:false
        }
    },
    mounted(){
        this.getDetails();
    },
    watch:{
       value:function(){
          this.payamount=this.value*this.details.price;
       }
    },
    methods:{
        getDetails(){
            let json= getStore("exchangeDetails");
            if(json==null){
              this.Toast("获取详情失败");
              return ;
            }
            var data=JSON.parse(json);
            this.details= data;
            this.payamount= data.price;
        },
        exchange(){
            this.isPay=true;
        },
        //取消隐藏
        fatherHide(){
            this.isPay=false;
        },
        //支付确认（密码输入后子组件回调）
        async fatherConfirm(password){
            this.isPay=false;
            let result= await ProductRecharge(JSON.parse(sessionStorage.userParam),this.details.id,password,this.value,'');
            if(result.result>0){
               let name=this.details.name;
               this.$router.push({name:'result',query:{typeonename:'兑换',typetwoname:'兑换类型：'+name}});
            }
            else{
               this.Toast(result.message);
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.SVCtransfer {
    .bgBx {
        width: 100%;
        min-height: 3.4rem;
        .arcbg {
          width: 100%;
          height: auto;
          left: 0;
        }
        img {
            width: 6.9rem;
            height: auto;
            position: absolute;
            left: 0.3rem;
        }
    }
    .content {
        padding: 0.3rem;
        .ops {
            display: flex;
            margin-top: 0.6rem;
            .lft {
                flex: auto;
            }
        }
        .infoBx {
            background: #f7f7fc;
            position: relative;
            padding: 0.3rem;
            margin-top: 0.3rem;
            .info {
                padding: 0.06rem 0;
            }
            .change {
                position: absolute;
                top: 50%;
                right: 0.3rem;
                background: #ff9000;
                transform: translateY(-50%);
                color: #fff;
                font-size: 0.28rem;
                padding: 0.1rem 0.3rem;
            }
            &.num {
                display: flex;
                padding: 0.18rem 0.3rem;
                .lft {
                    flex: auto;
                }
                /deep/ .van-stepper {
                    .van-stepper__minus, .van-stepper__plus {
                        width: 0.4rem;
                        height: 0.4rem;
                    }
                    .van-stepper__input {
                        background: #fff;
                        height: 0.4rem;
                    }
                }
            }
            &.txt {
                padding: 0 0.3rem;
                div {
                    padding: 0.24rem 0;
                }
                p {
                    margin: 0;
                    padding-bottom: 0.24rem;
                    font-size: 0.24rem;
                }
                overflow-y: auto;
                max-height: 2.8rem;
            }
        }
    }
    .botBx {
        position: fixed;
        width: 100%;
        bottom: 0;
        display: flex;
        box-shadow: -1px -2px 6px 0px #ededed;
        padding: 0.12rem 0.3rem;
        box-sizing: border-box;
        align-items: center;
        .total {
            flex: auto;
        }
        button {
            padding: 0 0.3rem;
            height: 0.74rem;
            border: none;
            background: #2ea2fa;
            color: #fff;
            border-radius: 0.04rem;
            white-space: nowrap;
        }
    }
}
</style>
