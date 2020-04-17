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
        <div class="popup" v-show="showNoPwd">
            <div class="noPwd">
                <div class="tit">支付</div>
                <div class="close" @click="SetNoPassword(false)"><img src="@/assets/images/close1.png" alt=""></div>
                <div class="text">今天加价是否免输密码，点击"免密加价"今天竞拍加价将不需要输入支付密码，0点后免密失效</div>
                <div class="btn bt2" @click="SetNoPassword(true)">免密加价</div>
                <div class="btn bt1" @click="SetNoPassword(false)">不用了</div>
            </div>
        </div>
    </div>
</template>

<script>
import { GetAuctionAddprice, AuctionAddpricePay } from '@/api/api';
import { setLocalStorage, getLocalStorage } from "@/config/utils";
export default {
    data() {
        return {
            allselect: false,//全选
            addnum: '',//输入的加价幅度
            list: [],//数据列表
            isKeyboard:false,
            multiple:0,//倍率
            password: '',   // 密码
            showNoPwd: false,  // 设置免密支付
        }
    },
    mounted() {
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
        //输入密码回调
        async fatherConfirm(password){
            this.password = password;
            this.isKeyboard = false;
            var auctionid = [];
            for (const item of this.list) {
                if (item.select) {
                    auctionid.push(item.id);
                }
            };
            let result= await AuctionAddpricePay(JSON.parse(sessionStorage.userParam), password, this.addnum * this.multiple, auctionid);
            if(result.result>0) {
                setTimeout(() => {
                    this.showNoPwd = true;
                }, 500)
                // this.$router.push({path:'/nov/2'});
            } else {
                vant.Toast(result.message);
                setTimeout(() => {
                    this.GetAuctionAddpriceData();
                },2000);
            }
        },
        async GetAuctionAddpriceData() {
            let result = await GetAuctionAddprice(JSON.parse(sessionStorage.userParam));
            console.log(result.data)
            if(result.result>0) {
                this.list=result.data.myauctionhis;
                this.multiple=result.data.multiple;
                this.SetSortKey(this.list, 'createtime');
                this.$set(this.list[0], 'select', true);
            } else {
                vant.Toast(result.message);
            }
        },
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
        async pay() {
            let newArr = [];
            for (const item of this.list) {
                if (item.select) {
                    newArr.push(item);
                }
            };
            if (!this.addnum) {
                 vant.Toast.fail('请输入加价幅度！');
            } else if (!newArr.length) {
                 vant.Toast.fail('请至少选择一项');
            } else {
                var newDate = new Date(new Date().getTime() + 24*60*60*1000);
                var now = new Date().getTime();
                var next = newDate.getFullYear()+"/" + (newDate.getMonth()+1) + "/" + newDate.getDate();
                var time = new Date(next).getTime() - now;
                var password = getLocalStorage('password', time);
                if(password) {
                    var auctionid = [];
                    for (const item of this.list) {
                        if (item.select) {
                            auctionid.push(item.id);
                        }
                    }
                    let result= await AuctionAddpricePay(JSON.parse(sessionStorage.userParam), password, this.addnum * this.multiple, auctionid);
                    if(result.result>0) {
                        setTimeout(() => {
                            this.$router.push({name:'result',query:{typeonename:'竞拍加价',typetwoname:''}})
                        }, 500)
                        // this.$router.push({path:'/nov/2'});
                    } else {
                        vant.Toast(result.message);
                        setTimeout(() => {
                            this.GetAuctionAddpriceData();
                        },2000);
                    }
                } else {
                    this.isKeyboard = true;
                }
            }
        },
        SetSortKey(array, key) {
          return array.sort(function(a, b) {
            var x = a[key];
            var y = b[key];
            return x > y ? -1 : x < y ? 1 : 0;
          });
        },
        SetNoPassword(type) {
            if(type) {
                setLocalStorage('password', this.password);
                setTimeout(() => {
                    this.$router.push({name:'result',query:{typeonename:'竞拍加价',typetwoname:''}})
                }, 100)
            } else {
                this.$router.push({name:'result',query:{typeonename:'竞拍加价',typetwoname:''}})
            }
        }
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
                padding: 0.1rem 0.2rem;
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
.popup {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba($color: #000000, $alpha: 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    .noPwd {
        font-size: 0.3rem;
        color: #333;
        background: #fff;
        width: 5rem;
        padding: 0.3rem 0.3rem 1rem 0.3rem;
        position: relative;
        text-align: center;
        .close {
            position: absolute;
            right: 0.2rem;
            top: 0.2rem;
            img {
            width: 0.5rem;
            }
        }
        .text {
            text-align: left;
            margin-top: 0.5rem;
        }
        .btn {
            width: 90%;
            margin: auto;
            padding: 0.2rem 0;
            box-sizing: border-box;
            border-radius: 0.1rem;
            margin-top: 0.5rem;
            &.bt1 {
                background: #2ea2fa;
                color: #fff;
            }
            &.bt2 {
                border: 1px solid #2ea2fa;
                color: #2ea2fa;
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
