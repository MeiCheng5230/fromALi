<template>
    <div class="bidsauction">

        <div class="topBx">
            <div style=" color: white;padding-bottom: 0.2rem;font-size: 0.32rem;padding-left: 0.2rem;">我的P点：{{myp}}</div>
            <div class="con" :style="{backgroundImage: 'url('+require('@/assets/images/paimai_details_bg.png')+')'}">竞拍底价（1A点）：{{minprice}}P点</div>
        </div>
        <div class="botBx">
            <div class="tit">竞拍数量</div>
            <div class="numBx">
                <div class="item" :class="{select:marknum==10}" @click="selectNum(10,10)">10个</div>
                <div class="item" :class="{select:marknum==50}" @click="selectNum(50,50)">50个</div>
                <div class="item" :class="{select:marknum==100}" @click="selectNum(100,100)">100个</div>
                <div class="item" :class="{select:marknum==500}" @click="selectNum(500,500)">500个</div>
                <div class="item" :class="{select:marknum==1000}" @click="selectNum(1000,1000)">1000个</div>
                <div class="item" :class="{select:marknum==0}" @click="selectNum(0,inputnum)">
                   <input type="number" @input="inpNumFn" :class="{inpselect:marknum==0}" placeholder="输入数量" v-model="inputnum"/>
                </div>
            </div>
           <div class="tit add" style="white-space: nowrap;">
                <span class="tt">加价幅度</span>
                <input type="text" @keyup="inputprice=inputprice.replace(/^(0+)|[^\d]+/g,'')" placeholder="输入加价幅度" v-model="inputprice"/>&nbsp;X{{multiple}}P
            </div>
            <div class="time">截止日期：{{endDate}} 12:00:00</div>
            <div class="btn" @click="pay">立即支付:{{payamount}}P点</div>
            <div class="rule"><router-link to="/auctionRule" tag="span">《竞拍规则》</router-link></div>
        </div>
      <keyboard theme="moneyMulti" :isKeyboard="isKeyboard" :payPrice="payamount+'P点'" @close="fatherHide"  @pay="fatherConfirm"></keyboard>
    </div>
</template>

<script>
import { PayAuction,GetAuctionConfig} from '@/api/getData';
import moment  from 'moment';
export default {
    data() {
        return {
            minprice:0,//底价
            num:10,//竞拍数量
            addprice:0,//选中的加价幅度
            marknum:10,//选中的数量标志
            noaddprice:0,//固定幅度价格
            isKeyboard:false,
            payamount:0,//支付金额
            inputnum:"",//输入框数量
            inputprice: '',//输入框浮动价格
            endDate:moment(new Date()).endOf('month').format("YYYY-MM-DD"),
            show:false,
            myp:0,
            multiple:0,//倍数
        }
    },
    watch:{
       num:function(){
            this.payamount=(this.minprice+this.addprice*this.multiple)*this.num;
       },
       addprice:function(){
           this.payamount=(this.minprice+this.addprice*this.multiple)*this.num;
       },
       inputnum:function(){
           var inputvalue= parseInt(this.inputnum);
           this.num=isNaN(inputvalue)?0:inputvalue;
       },
       inputprice:function(){
           var inputvalue= parseInt(this.inputprice);
           this.addprice=isNaN(inputvalue)?0:inputvalue;
       }

    },
    mounted(){
        this.getAuctionConfig();
    },
    methods:{
       selectNum(e,num){
             this.marknum=e;
             var inputvalue= parseInt(num);
             this.num=isNaN(inputvalue)?0:inputvalue;
       },
        //取消隐藏
        fatherHide(){
            this.isKeyboard=false;
        },
        pay(){
            if(this.num<=0){
                this.Toast("请选择竞拍数量");
                return;
            }
            if((this.minprice+this.addprice*this.multiple)*this.num!=this.payamount){
                  this.Toast("支付金额计算错误");
                  return;
             }
             if(this.payamount>this.myp){
                  this.Toast("p点余额不足");
                  return;
             }
             this.isKeyboard=true;
        },
        inpNumFn() {
            this.inputnum = parseInt(this.inputnum);
        },
        //支付确认（密码输入后子组件回调）
        async fatherConfirm(password){
            this.isKeyboard=false;
            let result= await PayAuction(JSON.parse(sessionStorage.userParam),this.num,password,(this.minprice+this.addprice*this.multiple));
            if(result.result>0){
               this.$router.push({name:'result',query:{typeonename:'竞拍',typetwoname:''}});
            }
            else{
               this.Toast(result.message);
                 setTimeout(() => {
                    this.getAuctionConfig();
                 },2000);

            }
        },
        async getAuctionConfig(){
            let result=await GetAuctionConfig(JSON.parse(sessionStorage.userParam));
            if(result.result>0){
                this.noaddprice=result.data.addprice;
                //this.changeaddprice=result.data.addprice;
                this.minprice=result.data.minprice;
                this.payamount=this.num*(this.minprice+this.addprice*this.multiple);
                this.myp=result.data.myp;
                this.multiple=result.data.multiple;
            }
            else{
                this.Toast(result.message);
            }
        },
    }
}
</script>

<style lang="scss" scoped>
  .van-stepper {
    margin-right: 0.1rem;
  }
  .van-number-keyboard {
    color: initial;
  }
.bidsauction {
    .topBx {
        background: #2ea2fa;
        padding: 0.3rem 0.3rem 0.85rem 0.3rem;
        .con {
            background: #fff;
            color: #2ea2fa;
            font-size: 0.36rem;
            padding: 0.45rem 0.3rem;
            border-radius: 0.04rem;
            background-repeat: no-repeat;
            background-position: top right;
            background-size: auto 100%;
        }
    }
    .botBx {
        height: 1rem;
        background: #fff;
        border-radius: 0.36rem 0.36rem 0 0;
        padding: 0 0.3rem;
        margin-top: -0.4rem;
        .tit {
            padding-top: 0.6rem;
            &.add {
                display: flex;
                align-items: center;
                background: #f7f7fc;
                margin-top: 0.3rem;
                padding: 0.2rem 0.3rem;
                input {
                    border: none;
                    padding: 0.2rem;
                    flex: auto;
                    margin-left: 0.3rem;
                    width: 1rem;
                }
                /deep/ .van-stepper {
                    .van-stepper__minus, .van-stepper__plus {
                        width: 0.4rem;
                        height: 0.4rem;
                    }
                    .van-stepper__input {
                        height: 0.4rem;
                    }
                }
            }
        }
        .numBx {
            background: #f7f7fc;
            border-radius: 0.04rem;
            padding: 0.3rem 0 0.1rem;
            margin-top: 0.3rem;
            .item {
                display: inline-block;
                width: 1.98rem;
                background: #fff;
                padding: 0.3rem 0;
                margin: 0 0 0.3rem 0.2rem;
                text-align: center;
                box-shadow: 1px 2px 6px 0px #ececec;
                border-radius: 0.04rem;
                color: #666;
                overflow: hidden;
                input {
                    width: 100%;
                    height: 100%;
                    padding: 0;
                    margin: 0;
                    border: none;
                    text-align: center;
                    &::placeholder {
                        color: #999;
                        text-align: center;
                    }
                }
                &.select {
                    background: #2ea2fa;
                     color: #fff;
                }
                .inpselect{
                    background: #2ea2fa;
                    color: #fff;
                    &::placeholder {
                      color: #fff;
                        text-align: center;
                    }
                }
            }
        }
        .time {
            color: #ff2020;
            text-align: center;
            margin: 0.4rem 0 0.2rem;
        }
        .btn {
            text-align: center;
            background: #2ea2fa;
            color: #fff;
            padding: 0.22rem 0;
            border-radius: 0.04rem;
        }
        .rule {
            color: #2ea2fa;
            text-align: center;
            padding: 0.3rem 0;
        }
    }
}
</style>
