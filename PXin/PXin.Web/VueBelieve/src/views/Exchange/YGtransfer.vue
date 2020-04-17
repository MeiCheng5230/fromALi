<template>
    <div class="YGtransfer" >
        <div class="bgBx">
            <img class="arcbg" src="@/assets/images/excharge_bg.png" alt=""/>
            <img :src="details.pic" alt="">
        </div>
        <div class="content">
            <div class="ops">
                <div class="lft">{{details.name}}</div>
                <div class="rgt">{{details.price}}{{details.priceunit}}</div>
            </div>
            <div class="infoBx">
                <div class="info">账&#12288;号：{{accountinfo.nodecode}}</div>
                <div class="info">姓&#12288;名：{{accountinfo.nodename}}</div>
                <div class="info">手机号：{{accountinfo.phone}}</div>
                <div class="change" @click="changeUserInfo">更改</div>
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
      <keyboard theme="moneyMulti" :isKeyboard="isPay" :payPrice="payamount" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
         <popup :title="popuptitle"
            v-if="popupFlag"
            :popFlag="popupFlag"
            @popupFlag="popupFn" @popupSubmit='popupSubmit'/>
    </div>
</template>

<script>
const popup = () => import("@/components/BotPopup");
import {getStore,setStore} from "@/config/utils";
import { ProductRecharge,GetPCNUserByNodeCode} from '@/api/getData';
export default {
     data () {
        return {
            value: 1,
            payamount:0,
            details:{},//兑换商品详情
            isPay:false,//支付组件
            title:'',//详情标题
            accountinfo:{},
            popupFlag:false,//输入账号组件
            popuptitle:'',//账号组件的标题
        }
    },
    mounted(){
        this.getDetails();
        let userinfo=getStore("exchangeYGorPcnUserInfo");
        if(userinfo==null){
          this.Toast("获取用户数据失败");
          return ;
        }
        this.accountinfo=JSON.parse(userinfo);
     },
    components: {
        popup
    },
    watch:{
       value:function(){
          this.payamount=this.value*this.details.price;
       }
    },
    methods:{
        popupFn(data) {     // 接受子传值，关闭弹窗
            this.popupFlag = data;
        },
         //组件确定按钮回调
       async popupSubmit(nodeCode){
            var typeid=this.details.typeid==3?5:4;
            let result= await GetPCNUserByNodeCode(JSON.parse(sessionStorage.userParam),nodeCode,typeid);
            if(result.result>0){
                this.popupFlag=false;
                this.accountinfo=result.data;
                setStore("exchangeYGorPcnUserInfo",JSON.stringify(result.data));
            }
            else{
                this.Toast(result.message);
            }
        },
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
            let result= await ProductRecharge(JSON.parse(sessionStorage.userParam),this.details.id,password,this.value,this.accountinfo.nodecode);
            if(result.result>0){
               let name=this.details.name;
               this.$router.push({name:'result',query:{typeonename:'兑换',typetwoname:'兑换类型：'+name}});
            }
            else{
               this.Toast(result.message);
            }
        },
        //更改账号
        changeUserInfo(){
            switch(this.details.typeid){
                case 3:
                     this.popuptitle='请输入要兑换的优谷账号\\手机号';
                     this.popupFlag=true;
                    break;
                case 4:
                     this.popuptitle='请输入要兑换的pcn账号\\手机号';
                     this.popupFlag=true;
                    break;
                default:
                 this.Toast("选择错误");
                 break;
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.YGtransfer {
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
            width: 1.8rem;
            height: 0.74rem;
            border: none;
            background: #2ea2fa;
            color: #fff;
            border-radius: 0.04rem;
        }
    }
}
</style>
