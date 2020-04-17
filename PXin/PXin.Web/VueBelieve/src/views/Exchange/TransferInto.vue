<template>
    <div class="transferInto">
        <div class="option">
            <div class="lft">转入方式</div>
            <div class="rgt">码库的DOS</div>
        </div>
        <div class="bgbx">
            <div class="top">
                <div class="lft">转入数量</div>
                <input type="number" placeholder="转入数量必须大于等于1" @input="dbpointNum()" v-model="balance"/>
                <div class="rgt">码库的DOS</div>
            </div>
            <div class="bot">
                <span>码库的DOS余额：{{allueBalance}}</span>
                <span class="all" @click="balance=allueBalance">全部转入</span>
            </div>
        </div>
        <div class="option">
            <div class="lft">到账数量</div>
            <div class="rgt">{{balance}}专户DOS</div>
        </div>
        <div class="remake">注:从码库主账号转账到专户DOS不收服务费，比例为1:1</div>
        <div class="btn" @click="transfer">确定</div>
    </div>
</template>

<script>
import { Dialog } from 'vant';
import { GetUeUserInfo,UeTransferInDos} from '@/api/getData';
import {setStore} from "@/config/utils";
export default {
     data () {
        return {
          allueBalance:0,//全部ue dos余额
          balance: '',
          show:false,
        }
    },
    mounted(){
        this.getUeDosBalance();
        var _this=this;
        window.dosPayResult= function(obj) {
            try {
                var ret = JSON.parse(obj);
                if (ret.result == undefined) {
                    throw new Error("解析错误");
                }
                if (ret.result <= 0) {
                    _this.Toast(ret.message);
                    return;
                } else {
                    _this.Toast("支付成功");
                     _this.$router.go(-1);
                }
            } catch (e) {
                _this.Toast("支付异常:" + obj);
            }
        }
    },
    methods:{
        async getUeDosBalance(){
             let result=await GetUeUserInfo(JSON.parse(sessionStorage.userParam),0);
             if(result.result>0){
                 this.allueBalance=result.data.uebalance.toString();
             }
             else{
                // this.Toast(result.message);
                 Dialog.alert({
                     title: '提示',
                     message: result.message
                 }).then(() => {
                     if(result.result==-5){
                         //没有绑定ue账号，跳转到绑定页面
                         setStore("bindingUecallbackurl",-1);//回调页面地址
                         this.$router.push({path:"/bindaccount"});
                     }
                 });

             }
         },
        isNumber(test){
           var reg = /^\d+(?=\.{0,1}\d+$|$)/;
           return reg.test(test);
        },
        async transfer(){
            if(this.balance<=0){
               this.Toast("转入金额必须大于0");
               return;
            }
            if(!this.isNumber(this.balance)){
                this.Toast("请输入正确的金额");
                return;
            }
            if(parseFloat(this.balance)>parseFloat(this.allueBalance)){
                this.Toast("转入金额不能大于余额");
                return;
            }
             let result=await UeTransferInDos(JSON.parse(sessionStorage.userParam),this.balance);
             if(result.result>0){
                 try {
                   AppNative.blJsTunedupNativeWithTypeParamSign(1003, result.data.chargestr, result.data.sign);
              } catch (error) {
                  this.Toast("调起码库支付失败");
              }
             }
             else{
                 this.Toast(result.message);
             }
         },
        dbpointNum() {
            this.balance = Number(this.balance);
            if(this.balance <= Number(this.allueBalance)) {
              this.balance = parseInt(this.balance);
             } else {
                this.Toast('转入金额不能大于余额');
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
            height: 1rem;
            align-items: center;
            border-bottom: 1px solid #e1e1e1;
            // white-space: nowrap;
            box-sizing: border-box;
            .lft {
                padding-right: 0.3rem;
            }
            input {
                flex: 1;
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
