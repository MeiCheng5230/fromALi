<template>
    <div class="transferInto">
        <div class="option">
            <div class="lft">到账钱包</div>
            <div class="rgt">码库的DOS</div>
        </div>
        <div class="bgbx">
            <div class="top">
                <div class="lft">转出数量</div>
                <input type="number" placeholder="转出数量必须大于等于1" @input="dbpointNum()" v-model="balance" />
                <div class="rgt">专户DOS</div>
            </div>
            <div class="bot">
                <span>专户DOS余额：{{dos}}</span>
                <span class="all" @click="balance=dos">全部转出</span>
            </div>
        </div>
        <div class="option">
            <div class="lft">服务费</div>
            <div class="rgt">{{actualbalance}}专户DOS</div>
        </div>
        <div class="option">
            <div class="lft">到账数量</div>
            <div class="rgt">{{actualbalance}}码库的DOS</div>
        </div>
        <div class="remake">注:从专户DOS转账到码库主账号收取50%服务费，以专户DOS计算</div>
        <div class="btn" @click="transfer">确定</div>
      <keyboard theme="moneyMulti" :isKeyboard="isPay" :payPrice="balance" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
    </div>
</template>

<script>
    import { Dialog } from 'vant';

import { DosTransferOutUe,GetUeUserInfo} from '@/api/getData';
import {getStore,setStore} from "@/config/utils";
export default {
    data(){
        return{
            dos:0,
            balance:'',
            actualbalance:0,
            isPay:false,
            show:false,
        }
    },
    mounted(){
      this.dos= getStore("exchangeUserDos");
    },
    watch:{
       balance:function(){
           this.actualbalance=this.isNumber(this.balance)?this.balance/2:0;
       }
    },
    methods:{
        isNumber(test){
           var reg = /^\d+(?=\.{0,1}\d+$|$)/;
           return reg.test(test);
        },
        transfer(){
             if(this.balance<=0){
               this.Toast("转出金额必须大于0");
               return;
            }
            if(!this.isNumber(this.balance)){
                this.Toast("请输入正确的金额");
                return;
            }
            if(parseFloat(this.balance)>parseFloat (this.dos)){
                 this.Toast("转出金额不能大于余额");
                 return;
            }
            this.isPay=true;
        },
        async fatherConfirm(password){
             let result=await DosTransferOutUe(JSON.parse(sessionStorage.userParam),password,this.balance);
             if(result.result>0){
                this.$router.push({name:'result',query:{typeonename:'转出',typetwoname:''}});
             }
             else{
                 this.Toast(result.message);
             }
         },
         fatherHide(){
             this.isPay=false;
         },
         async getUeUserInfo(){
             let result=await GetUeUserInfo(JSON.parse(sessionStorage.userParam),0);
             if(result.result<=0){
                // this.Toast(result.message);
                 Dialog.alert({
                     title: '提示',
                     message: result.message
                 }).then(() => {
                     if(result.result==-5){
                         setStore("bindingUecallbackurl",-1);//回调页面地址
                         this.$router.push({path:"/bindaccount"});
                     }
                 });


             }
         },
        dbpointNum() {
            this.balance = Number(this.balance);
            if (this.balance <= Number(this.dos)) {
              this.balance = parseInt(this.balance);
            } else {
                this.Toast('转出金额不能大于余额');
                this.balance = 0;
             }
        }
    }
}
</script>

<style lang="scss" scoped>
.transferInto {
    min-height: 100%;
    background: #f2f2f2;
    .option {
        display: flex;
        background: #fff;
        padding: 0.3rem;
        margin-top: 0.3rem;
        .lft {
            flex: auto;
        }
    }
    .bgbx {
        margin-top: 0.3rem;
        background: #fff;
        padding: 0 0.3rem;
        .top {
            display: flex;
            padding: 0.3rem 0;
            border-bottom: 1px solid #e1e1e1;
            // white-space: nowrap;
            box-sizing: border-box;
            .lft {
                padding-right: 0.3rem;
            }
            input {
                flex: auto;
                padding-left: 0;
                box-sizing: border-box;
                border: none;
            }
        }
        .bot {
            color: #999;
            padding: 0.3rem 0;
            .all {
                color: #2ea2fa;
                margin-left: 0.2rem;
            }
        }
    }
    .remake {
        font-size: 0.24rem;
        color: #666;
        padding: 0.3rem;
    }
    .btn {
        background: #2ea2fa;
        border-radius: 0.04rem;
        text-align: center;
        padding: 0.22rem 0;
        margin: 0.9rem 0.3rem 0 0.3rem;
        color: #fff;
    }
}
</style>
