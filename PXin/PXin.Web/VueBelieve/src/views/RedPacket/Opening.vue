<template>
    <div class="opening">
        <div class="bg">
            <img src="@/assets/images/open_account.png" alt=""/>
        </div>
        <div class="infoBx">
            <div class="title">开通专属账户</div>
            <div class="info">
                <div class="ops">
                    <div class="lft">用户名称</div>
                    <div class="rgt">{{nodename}}</div>
                </div>
                <div class="ops">
                    <div class="lft">用户账号</div>
                    <div class="rgt">{{nodecode}}</div>
                </div>
                <div class="ops">
                    <div class="lft">开通时长</div>
                    <div class="rgt">1年</div>
                </div>
                <div class="ops">
                    <div class="lft">应付金额</div>
                    <div class="rgt">50DOS</div>
                </div>
            </div>
        </div>
        <div class="botbx">
            <div class="total">总计：50DOS</div>
            <button @click="openinfo">确定开通</button>
        </div>
    </div>
</template>

<script>
import { ExchangeOpenInfo } from '@/api/getData';
import {getStore} from "@/config/utils";
export default {
    data() {
      return {
          nodename:'',
          nodecode:'',
          callbackurl:-1,
          isopen:false,
      }
    },
    mounted(){
        let openinfo=getStore("openInfo");
        if(openinfo==null){
           this.Toast("获取用户信息失败");
           return;
        }
        let json=JSON.parse(openinfo);
        this.nodename=json.nodename;
        this.nodecode=json.nodecode;
        this.callbackurl=json.callbackurl;
        var _this=this;
      window.dosPayResult = function (obj) {
            try {
              var ret = JSON.parse(obj);
                if (ret.result == undefined) {
                    throw new Error("解析错误");
                }
                if (ret.result <= 0) {
                    _this.Toast(ret.message);
                    return;
                } else {
                    _this.isopen=true;
                    _this.Toast("支付成功");
                     _this.$router.go(_this.callbackurl);
                }
            } catch (e) {
                _this.Toast("支付异常:" + obj);
            }
        }
    },
    methods:{
       async openinfo(){
          let result=await ExchangeOpenInfo(JSON.parse(sessionStorage.userParam));
          if(result.result>0){
            try {
                AppNative.blJsTunedupNativeWithTypeParamSign(1003,result.data.chargestr,result.data.sign);
                //this.$router.go(-1);
              } catch (error) {
                this.Toast("调起码库支付失败");
              }
          }
          else{
            this.Toast(result.message);
          }
       }
    },
     beforeRouteLeave: function (to, from, next) {
       if (!this.isopen && (to.name == "exchange" || to.name == "receive")) {
            AppNative.blJsTunedupNativeWithTypeParamSign(1001,"","");
            return;
         }
         next();
    }
}
</script>

<style lang="scss" scoped>
.opening {
    .bg {
        img {
            width: 100%;
            height: auto;
        }
    }
    .infoBx {
        background: #fff;
        border-radius: 0.36rem 0.36rem 0 0;
        box-sizing: border-box;
        position: relative;
        top: -0.4rem;
        padding: 0 0.3rem;
        .title {
            padding: 0.5rem 0;
            text-align: center;
        }
        .info {
            background: #f7f7fc;
            padding: 0.6rem 0.6rem 0 0.6rem;
            font-size: 0.28rem;
            .ops {
                display: flex;
                padding-bottom: 0.5rem;
                .lft {
                    flex: auto;
                }
            }
        }
    }
    .botbx {
        position: fixed;
        bottom: 0;
        width: 100%;
        box-shadow: 0px -1px 6px 0px #f3f3f3;
        display: flex;
        align-items: center;
        box-sizing: border-box;
        padding: 0.13rem 0.3rem;
        .total {
            flex: auto;
        }
        button {
            padding: 0.2rem 0.3rem;
            background: #2ea2fa;
            border-radius: 0.04rem;
            color: #fff;
            border: none;
        }
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .botbx {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
</style>
