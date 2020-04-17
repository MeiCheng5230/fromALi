<template>
    <div class="home">
        <div class="banner">
            <div class="banner-top">
                <div class="font-12">可申请开票金额</div>
                <div class="font-30 banner-price">{{ amount }}</div>
                <div class="font-12">已申请：{{ alreadyamount }}</div>
                <router-link class="banner-add" tag="div" :to="{path: '/add', query: {status: status}}">增票资质</router-link>
            </div>
            <div class="banner-bottom font-12">
                申请开票仅支持使用微信支付购买的SVC充值码，且开票金额不能大于充值V点的SV金额
            </div>
        </div>
        <div class="lists">
            <van-tabs :sticky="true"  v-model="active">
                <van-tab title="可申请">

                    <van-list
                            v-model="list[0].loading"
                            :finished="list[0].finished"
                            :finished-text="list[0].finishedTxt"
                            @load="onLoad(0)"
                    >
                        <van-cell v-for="item in list[0].items" :key="item.cardnum" @click="selectChecked(item)">
                            <template slot="title">
                                <span class="custom-title"><i class="dot"></i>{{ item.amount + item.showname }}</span>
                            </template>
                            <van-checkbox v-model="item.isSelected" slot="right-icon"/>
                            <template slot="label">
                                <div class="font-12">编号：NO.{{ item.cardnum }}</div>
                                <div class="font-12">购买时间：{{ item.createtime }}</div>
                            </template>
                        </van-cell>

                    </van-list>
                    <nodata v-if="list[0].nodata" text="暂无数据"/>
                </van-tab>

                <van-tab title="已申请">
                    <van-list
                            v-model="list[1].loading"
                            :finished="list[1].finished"
                            :finished-text="list[1].finishedTxt"
                            @load="onLoad(1)"
                    >
                        <van-cell v-for="item in list[1].items" :key="item.id">
                            <template slot="title">
                                <span class="custom-title"><i :class="active===1?'dot bg-gray':'dot'"></i>{{ item.amount + item.showname }}</span>
                            </template>
                            <template slot="default" v-if="item.status==2">开票完成</template>
                            <span v-if="item.status==1">审核中</span>
                            <span style="color: red; white-space: nowrap; position: absolute; right: 0; top: 0;" v-if="item.status==3">审核不通过</span>

                            <template slot="label">
                                <div class="font-12">编号：NO.{{ item.cardnum }}</div>
                                <div class="font-12">
                                    <div class="font-12">类型：{{ item.typeid==1?'增值税普通发票':'增值税专用发票' }} 

                                        <!-- <router-link tag="span" :to="{path: '/detail', query: {imgUrl: item.expressno}}" class="btn" v-if="item.status==2&&item.typeid==1">查看发票</router-link> -->
                                    <a class="btn" :href="expressUrl(item.expressno)" v-if="item.status==2">查看物流</a>
                                    <!-- <a class="btn" :href="expressUrl(item.expressno)" v-if="item.status==2&&item.typeid==2">查看物流</a> -->

                                    </div>
                                    <div class="font-12">抬头：{{ item.head }}</div>
                                    <div class="font-12" v-if="!(item.typeid==1 && item.isperson==1)">税号：{{ item.taxnum }}</div>
                                </div>
                            </template>
                        </van-cell>
                    </van-list>
                    <nodata v-if="list[1].nodata" text="暂无数据"/>
                </van-tab>
                
                
            </van-tabs>
        </div>
        <van-submit-bar
                v-show="active===0&&showBar"
                :price="totalPrice"
                :label="totalNum.text"
                button-text="开发票"
                @submit="jumpApplyPage"
        >
            <van-checkbox v-model="isAllSelected" @click="allChecked">全选</van-checkbox>

        </van-submit-bar>
    </div>
</template>

<script>
const nodata = () => import('@/components/NoData');
import { GetInvioceStatistics, GetMayApplyInvioceHis, GetAlreadyApplyInvioceHis } from '@/api/api';
import { getUrlObj } from '@/utils/utils';
    export default {
        name: "home",
        data() {
            return {
                amount: '', // 可开票金额
                alreadyamount: '',  // 已开票金额 
                status: '',    // 是否通过增票资质审核 0-未填写， 1-审核中， 2-审核通过， 3-审核拒绝
                active: 0,  // 
                name: '',   // 姓名
                showBar: true,  // 底部显示
                list: [{
                    items: [],  // 可申请数据
                    loading: false,
                    finished: false,
                    nodata: false,
                    finishedTxt: '没有更多了'
                }, {
                    items: [],  // 已申请数据
                    loading: false,
                    finished: false,
                    nodata: false,
                    finishedTxt: '没有更多了'
                }],
                hasAllChecked:false,
                pagesize: 10,    // 每页数量
                canPagenum: 0,  // 可申请页码
                hasPagenum: 0,  // 已申请页码
            }
        },
        created() {
            this.GetInvioceStatistics();
        },
        updated() {
            if (!this.list[0].items.length) {
                this.showBar = false;
            } else {
                this.showBar = true;
            }
        },
        computed:{
            isAllSelected() {
            // 全选/全不选
                let isSelected = true;
                this.list[0].items.map(item => {
                    if(!item.isSelected) {
                        isSelected = false;
                    }
                });
                return isSelected;
            },
            totalPrice() {
            // 总金额
                let total = 0;
                this.list[0].items.map(item => {
                    if(item.isSelected) {
                        total += item.amount;
                    }
                });
                return total * 100;
            },
            totalNum() {
            // 总计数量
                let num = 0;
                this.list[0].items.map(item => {
                    if(item.isSelected) {
                        num ++;
                    }
                });
                return {
                    num,
                    text: "总计："+ num +"张，共"
                }
            }
        },
        methods: {
            expressUrl(num) {
            // 快递查询
                return window.location.protocol+"//"+window.location.host+"/App/Believe/index.html"+window.location.search+"#/logistics?expressno="+ num;
            },
            jumpApplyPage() {
            // 申请开票
                if (this.totalNum.num <= 0) {
                    vant.Toast('请至少选择一条！');
                    return;
                }
                if (this.totalPrice/100 > this.amount) {
                    vant.Toast('您的可开票金额不足!');
                    return;
                }
                let arr = [];
                for (const item of this.list[0].items) {
                    if (item.isSelected) {
                        arr.push(item.idno);
                    }
                }
                this.$router.push({path: '/apply', query: {name: this.name, status: this.status, idno: arr.join(',')}})
            },
            onLoad(index, isRefresh) {
            // 上拉加载
                if(index == 0) {
                    this.canPagenum ++;
                    this.GetMayApplyInvioceHis(this.canPagenum);
                } else if (index == 1) {
                    this.hasPagenum ++;
                    this.GetAlreadyApplyInvioceHis(this.hasPagenum);
                }
            },
            selectChecked(item) {
            // 选择充值码
                this.$set(item, 'isSelected', !item.isSelected);
            },
            allChecked() {
            // 点击全选/不全选
                this.hasAllChecked = !this.hasAllChecked;
                this.list[0].items.map(item => {
                    this.$set(item, 'isSelected', this.hasAllChecked);
                })
            },
            async GetInvioceStatistics() {
            // 获取首页开票
                let result = await GetInvioceStatistics(this.GLOBAL.USERINFO);
                if(result.result > 0) {
                    let { alreadyamount, amount, status, name } = result.data;
                    this.alreadyamount = alreadyamount;
                    this.amount = amount;
                    this.status = status;
                    this.name = name;
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async GetMayApplyInvioceHis(pagenum) {
            // 获取可申请列表
                let result = await GetMayApplyInvioceHis({...this.GLOBAL.USERINFO, pagenum, pagesize: this.pagesize});
                this.list[0].loading = false;
                if(result.result > 0) {
                    if (pagenum==1&&!result.data.length) {
                        this.list[0].nodata = true;
                        this.list[0].finishedTxt = '';
                    } else if (pagenum==1&&result.data.length) {
                        this.$set(result.data[0], 'isSelected', true);
                    }
                    if (result.data.length < this.pagesize) {
                        this.list[0].finished = true;
                    }
                    this.list[0].items = this.list[0].items.concat(result.data);
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async GetAlreadyApplyInvioceHis(pagenum) {
            // 获取已申请列表
                let result = await GetAlreadyApplyInvioceHis({...this.GLOBAL.USERINFO, pagenum, pagesize: this.pagesize});
                this.list[1].loading = false;
                if(result.result > 0) {
                    if (pagenum==1&&!result.data.length) {
                        this.list[1].nodata = true;
                        this.list[1].finishedTxt = '';
                    }
                    if (result.data.length < this.pagesize) {
                        this.list[1].finished = true;
                    }
                    this.list[1].items = this.list[1].items.concat(result.data);
                } else {
                    vant.Toast.fail(result.message);
                }
            }
        },
        components: {
            nodata
        }
    }
</script>

<style scoped lang="scss">
    .font-12 {
        font-size: 12px;
    }

    .font-30 {
        font-size: 30px;
    }

    .home {
        padding-bottom: 50px;

        .banner {
            color: #fff;
            background: url("../assets/images/bill_top_bg.png") no-repeat;
            background-size: 100% 100%;

            .banner-top {
                padding: 14px 15px 14px 20px;
                position: relative;

                .banner-price {
                    padding: 10px 0;
                }

                .banner-add {
                    position: absolute;
                    right: 0;
                    display: inline-block;
                    background-color: #fff6aa;
                    border-radius: 20px 0px 0px 20px;
                    padding: 10px 8px 10px 14px;
                    color: #2ea2fa;
                    top: 50%;
                    margin-top: -15px;
                    font-size: 12px;
                }
            }

            .banner-bottom {
                padding: 10px 15px 10px 20px;
                border-top: 1px solid rgba(255, 255, 255, .2);
                line-height: 18px;
            }
        }

        .lists {

            /deep/ .van-tabs {
                .van-tab {
                    font-size: 16px;
                    color: #999;

                    &.van-tab--active {
                        color: #2ea2fa;
                    }


                }

                .van-tabs__line {
                    background-color: #30a6fa;
                    max-width: 50px;
                }

                .van-tabs__content {
                    padding: 20px;

                    .van-cell {
                        margin-bottom: 20px;
                        border-radius: 6px;

                        .custom-title {
                            font-size: 14px;
                            padding-bottom: 20px;

                            .dot {
                                background-color: #30a6fa;
                                display: inline-block;
                                font-style: normal;
                                width: 7px;
                                height: 7px;
                                border-radius: 50%;
                                margin-right: 6px;
                            }
                        }

                        .van-cell__value {
                            color: #30a6fa;
                            flex: 0.3;
                            position: relative;
                            overflow: initial;
                        }

                        .van-cell__label {
                            .btn {
                                padding: 2px 5px;
                                display: inline-block;
                                font-size: 12px;
                                color: #30a6fa;
                                border-radius: 2px;
                                border: solid 1px #2ea2fa;
                                margin-left: 10px;
                            }
                        }
                    }
                }

            }
        }

        .van-submit-bar {
            padding-left: 15px;
            right: 0;
            box-sizing: border-box;

            .van-submit-bar__text {
                font-size: 15px;
                color: #2ea2fa;

                .van-submit-bar__price {
                    color: #2ea2fa;
                    font-size: 15px;
                }
            }

            .van-button {
                margin: 6px 15px;
                height: auto;
                background-color: #2ea2fa;
                border: none;
                padding: 8px 16px;
                line-height: normal;
                width: auto;
                font-size: 15px;
                border-radius: 2px;
            }
        }
        .bg-gray{
            background: #cbcbcb!important;
        }
    }
</style>
