<template>
    <div class="addprice">
        <div class="list">
            <div class="item" v-for="item in list" :key="item.index">
                <div class="sel" @click="setSelect(item)">
                    <img v-show="!item.select" src="@/assets/images/paimai_check_nor.png" alt="">
                    <img v-show="item.select" src="@/assets/images/paimai_check_sel.png" alt="">
                </div>
                <div class="top">
                    <div class="lft">{{ item.price }}P点×{{ item.num }}</div>
                    <div class="rgt" v-show="item.status==0">领先</div>
                    <div class="rgt" v-show="item.status==1">已完成</div>
                    <div class="rgt" v-show="item.status==-1">出局(P点已退)</div>
                    <div class="rgt" v-show="item.status==-2">出局</div>
                </div>
                <div class="bot">
                    <div class="lft">{{ item.createtime }}</div>
                    
                </div>
            </div>
        </div>
        <div class="handle">
            <div class="top">
                <span>加价幅度</span>
                <input type="text" @input="setInp" v-model="addnum" placeholder="请输入加价幅度"/>
                <span>×{{ multiple }}P</span>
            </div>
            <div class="bot">
                <div class="selImg" @click="setAllSelect">
                    <img v-show="!allselect" src="@/assets/images/all_check_nor.png" alt=""/>
                    <img v-show="allselect" src="@/assets/images/all_check_sel.png" alt=""/>
                    <span>全选</span>
                </div>
                <div class="total">合计：<span>{{ payAmount }}P</span></div>
                <button @click="pay">立即支付</button>
            </div>
             <keyboard theme="moneyMulti" :isKeyboard="isKeyboard" :payPrice="payAmount+'P点'" @close="isKeyboard=false" @pay="fatherConfirm"></keyboard>
        </div>
    </div>
</template>

<script>
import { GetAuctionAddprice, AuctionAddpricePay } from '@/api/getData';
import Vue from 'vue';
import vueKeyboard from 'vue-keyboard-su-link/dist/vueKeyboard';
Vue.use(vueKeyboard);
export default {
    data() {
        return {
            allselect: false,//全选
            addnum: '',//输入的加价幅度
            list: [],//数据列表
            isKeyboard:false,
            multiple:0,//倍率
        }
    },
    mounted(){
        this.GetAuctionAddpriceData();
    },
    computed: {
        payAmount() {
            let num = 0;
            for (const itm of this.list) {
                if (itm.select) {
                    num += itm.num;
                }
            }
            return this.addnum * this.multiple * num;
        }
    },
    methods: {
        setSelect(data) {
            this.$set(data, 'select', !data.select);
            var arr = [];
            for(const item of this.list) {
                if (item.select) {
                   arr.push(item); 
                }
            };
            if (arr.length == this.list.length) {
                this.allselect = true;
            } else {
                this.allselect = false;
            }
        },
        setAllSelect() {
            this.allselect = !this.allselect;
            for(const item of this.list) {
                this.$set(item, 'select', this.allselect);
            };
            if (!this.allselect) {
                this.$set(this.list[0], 'select', true);
            }
        },
        setInp() {
            if(this.addnum.length==1) {
                this.addnum = this.addnum.replace(/[^1-9]/g,'')
            } else {
                this.addnum = this.addnum.replace(/\D/g,'')
            }
        },
        pay() {
            let newArr = [];
            for (const item of this.list) {
                if (item.select) {
                    newArr.push(item);
                }
            };
            if (!this.addnum) {
                this.Toast.fail('请输入加价幅度！');
            } else if (!newArr.length) {
                this.Toast.fail('请至少选择一项');
            } else {
                this.isKeyboard = true;
            }
        },
        SetSortKey(array, key) {
          return array.sort(function(a, b) {
            var x = a[key];
            var y = b[key];
            return x > y ? -1 : x < y ? 1 : 0;
          });
        },
        //输入密码回调
        async fatherConfirm(password){
            this.isKeyboard = false;
            var auctionid = [];
            for (const item of this.list) {
                if (item.select) {
                    auctionid.push(item.id);
                }
            };
            let result= await AuctionAddpricePay(JSON.parse(sessionStorage.userParam), password, this.addnum * this.multiple, auctionid);
            if(result.result>0) {
               this.$router.push({name:'result',query:{typeonename:'竞拍加价',typetwoname:''}});
            } else {
                this.Toast(result.message);
                setTimeout(() => {
                    this.GetAuctionAddpriceData();
                },2000);            
            }
        },
        async GetAuctionAddpriceData() {
            let result = await GetAuctionAddprice(JSON.parse(sessionStorage.userParam));
            if(result.result>0) {
                this.list=result.data.myauctionhis;
                this.multiple=result.data.multiple;
                this.SetSortKey(this.list, 'createtime');
                this.$set(this.list[0], 'select', true);
            } else {
                this.Toast(result.message);
            }
        },
    }
}
</script>

<style lang="scss" scoped>
.addprice {
    .list {
        padding: 0 0.3rem 2rem 0.3rem;
        .item {
            box-shadow: 0 2px 10px 0#eaeaea;
	        border-radius: 0.12rem;
            padding: 0.48rem 0.3rem 0.3rem 0.3rem;
            margin-top: 0.3rem;
            position: relative;
            .sel {
                position: absolute;
                top: 0;
                right: 0;
                img {
                    height: 0.48rem;
                }
            }
            .top, .bot {
                font-size: 0.3rem;
                display: flex;
                .lft {
                    flex: auto;
                    font-size: 0.24rem;
                    color: #999;
                }
                &.top {
                    margin-bottom: 0.3rem;
                    font-size: 0.3rem;
                    .lft {
                        display: flex;
                        align-items: center;
                        font-size: 0.28rem;
                        color: #333;
                        &::before {
                            content: '';
                            width: 0.08rem;
                            height: 0.3rem;
                            background: #2ea2fa;
                            display: inline-block;
                            border-radius: 0.04rem;
                            margin-right: 0.16rem;
                        }
                    }
                }
            }
        }
    }
    .handle {
        position: fixed;
        bottom: 0;
        width: 100%;
        background: #fff;
        .top {
            box-shadow: 0 -0.02rem 0.2rem 0#eaeaea;
            padding: 0.1rem;
            span {
                padding: 0 0.2rem;
            }
            input {
                width: 4.2rem;
	            height: 0.68rem;
                background-color: #f7f7fc;
	            border-radius: 0.04rem;
                border: none;
                box-sizing: border-box;
                padding: 0.2rem 0.3rem;
                &::placeholder {
                    color: #999;
                }
            }
        }
        .bot {
            display: flex;
            align-items: center;
            padding: 0.12rem 0.3rem;
            .selImg {
                display: flex;
                align-items: center;
                img {
                    width: 0.36rem;
                    height: 0.36rem;
                    margin-right: 0.1rem;
                }
                span {
                    color: #999;
                    font-size: 0.24rem;
                }
            }
            .total {
                flex: auto;
                text-align: right;
                span {
                    color: #2ea2fa;
                }
            }
            button {
                width: 1.48rem;
                height: 0.64rem;
                background-color: #2ea2fa;
                border-radius: 0.04rem;
                border: none;
                margin-left: 0.2rem;
                color: #fff;
            }
        } 
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .handle {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
</style>
