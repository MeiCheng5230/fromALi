<template>
    <div class="bindaccount" >
        <div v-if="initialize">
          <div class="inpbx" v-if="!Flag">
            <div class="ops">
              <div class="tit">码库账户</div>
              <div class="inp">{{ueinfo.nodecode}}</div>
            </div>
            <div class="ops">
              <div class="tit">昵称</div>
              <div class="inp">{{ueinfo.nodename}}</div>
            </div>
          </div>

          <div class="inpbx" v-else>
            <div class="ops">
              <div class="tit">账号</div>
              <div class="inp">
                <input type="text" v-model="account" maxlength="11" @input="$event.currentTarget.value=$event.currentTarget.value.replace(/\D/g, '')" placeholder="请输入账户/手机号" />
              </div>
            </div>
            <div class="ops">
              <div class="tit">密码</div>
              <div class="inp pwd">
                <input type="password" maxlength="16" v-show="showPwd" v-model="pwd" placeholder="请输入密码" />
                <input type="text" maxlength="16" v-show="!showPwd" v-model="pwd" placeholder="请输入密码" />
                <img v-show="showPwd" @click="showpwd()" src="@/assets/images/hidepwd.png" alt="" />
                <img v-show="!showPwd" @click="showpwd()" src="@/assets/images/showpwd.png" alt="" />
              </div>
            </div>
          </div>
          <!-- <div class="changeBtn"  v-if="!Flag" @click="updateBind">更换绑定</div> -->
          <div :class="bindFlag ? 'bindBtn' : 'nobind'" v-show="Flag"
               @click="bindFn">立即绑定</div>
        </div>

    </div>
</template>

<script>
import { Dialog } from 'vant';
import { BindingUeAccount,GetUeUserInfo} from '@/api/getData';
import {getStore} from "@/config/utils";
export default {
    data() {
      return {
            initialize: false,       //初始化
            Flag: true,              // 判断为绑定UE还是更换绑定， true为绑定， false为更换
            showPwd: true,           // 密码显示隐藏
            bindFlag: false,         // 输入的账户和密码不为空，绑定按钮样式改变
            pwd: '',                 // 密码
            account: '',             // 账户
            ueinfo:{
                nodecode:'',
                nodename:'',
            },
            popFlag:true,//提示框是否显示
            callbackurl:'',
        }
    },
    created(){
        this.callbackurl= getStore("bindingUecallbackurl");
        this.getUeUserInfo();//重新获取ue用户信息，主要避免绑定后刷新本页面造成继续绑定
    },  
    updated() {
        if (this.pwd && this.pwd) {
            this.bindFlag = true;
        } else {
            this.bindFlag = false;
        }
    },
    methods: {
        bindFn() {      // 点击立即绑定
            if (this.bindFlag) {
                this.bindingUeAccount();
            }
        },
        updateBind(){
             this.account='';
             this.pwd='';
             this.Flag=true;
        },
        async bindingUeAccount(){
            let result=await BindingUeAccount(JSON.parse(sessionStorage.userParam),this.account,this.pwd);
            if(result.result>0){
                  this.Toast("绑定成功");
                  this.ueinfo.nodecode=result.data.nodecode;
                  this.ueinfo.nodename=result.data.nodename;
                  this.Flag=false;
                  if(!isNaN(parseInt(this.callbackurl))){//h5过来的可以直接回调
                        setTimeout(() => {
                           this.$router.go(this.callbackurl);
                        }, 1000);
                  }
            }
            else{
               this.Toast(result.message);
            }
        },
        showpwd() {     // 显示隐藏密码
            this.showPwd = !this.showPwd;
        },
        async getUeUserInfo(){
             let result=await GetUeUserInfo(JSON.parse(sessionStorage.userParam),0);
             if(result.result<=0){
                if(result.result==-5){
                    this.Flag=true;
                    Dialog.alert({
                    title: '友情提示',
                    message: '在你使用第三方提供的用户账号，支付接口等数据使用时会产生一定的数据使用费，以码库的DOS支付，系统将自动从你绑定的码库账号内扣除，如余额不足，所支付的其它费用将会退回，为了更好的用户体验，请在使用前先绑定码库账号，绑定后暂不支持解绑，请谨慎填写。'
                    }).then(() => {
                    // on close
                    });
                }
                else{
                     this.Toast(result.message);
                }
             }
             else{
                  this.ueinfo.nodecode=result.data.nodecode;
                  this.ueinfo.nodename=result.data.nodename;
                  this.Flag=false;
             }
          this.initialize = true;
         },
    },
    beforeRouteLeave: function (to, from, next) {
        if(!isNaN(parseInt(this.callbackurl))&&this.Flag){//h5页面没有绑定直接点击返回
            if(to.name=="exchange"){
                next();
                return;
            }
            next({"name":"exchange"});
        }
        else{
            next();
        }
    },
}
</script>

<style lang="scss" scoped>
.van-dialog__header{
    text-align: center!important;

}
.bindaccount {
    min-height: 100%;
    background: #f7f7fc;
    .inpbx {
        .ops {
            background: #fff;
            margin-top: 0.3rem;
            padding: 0.2rem 0.3rem;
            display: flex;
            align-items: center;
            .tit {
                width: 1.5rem;
                flex-shrink: 0;
            }
            .inp {
                flex: auto;
                input {
                    border: none;
                }
                &.pwd {
                    display: flex;
                    align-items: center;
                    input {
                        width: 1rem;
                        flex: auto;
                    }
                    img {
                        width: 0.5rem;
                        height: auto;
                    }
                }
            }
        }
    }
    .bindBtn, .changeBtn {
        background: #2ea2fa;
        margin: 1rem 0.3rem;
        text-align: center;
        color: #fff;
        border-radius: 0.06rem;
        padding: 0.2rem 0;
    }
    .nobind {
        background: #2ea2fa;
        opacity: 0.5;
        margin: 1rem 0.3rem;
        text-align: center;
        color: #fff;
        border-radius: 0.06rem;
        padding: 0.2rem 0;
    }
    #main{
            text-align: center;
            margin-top:60px;
        }
        input[type=text],input[type=password]{
            width:260px;
            height:28px;
            display: inline-block;
        }
        span{
            margin-left:-30px;
            cursor: pointer;
        }
        input[type=checkbox]{
            cursor: pointer;
            opacity: 0;
            margin-left:-18px;
            display: inline-block;
        }
}
</style>
