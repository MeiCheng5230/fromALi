<template>
    <div class="list">
        <van-tabs :sticky="true" @click="setTabActive" v-model="listActive">
            <van-tab title="已满足条件" name="0">

                <list-item v-for="item in satisfaction.list"
                           :item="item"
                           :key="item.hisid"
                           :listActive="listActive"
                           v-show="satisfaction.list.length>0"
                           @setChecked="setChecked"></list-item>
                <no-data v-if="satisfaction.list.length<1&&isAjaxFinished" text="咦，您还没有满足条件哟~"></no-data>
            </van-tab>
            <van-tab title="已获得资格" name="1">
                <list-item v-for="item in qualify.list"
                           :item="item"
                           :key="item.hisid"
                           :listActive="listActive"
                           :isQualify="isQualify"
                           v-show="qualify.list.length>0"
                           @setChecked="setChecked"></list-item>
                <no-data v-if="qualify.list.length<1&&isAjaxFinished" text="咦，您还没有获得资格哟~"></no-data>
            </van-tab>
        </van-tabs>
        <van-submit-bar
                v-show="isBtnShow"
                :price="totalPrice*100"
                button-text="去支付"
                @submit="postUePayData"
                :class="listActive==0?'':'active'"
        >
            <van-checkbox v-model="isAllChecked" @click="checkAll" shape="square"
                          v-if="listActive==0">全选
            </van-checkbox>
        </van-submit-bar>
    </div>
</template>

<script>
    import ListItem from "../../components/ListItem";
    import NoData from "../../components/NoData";
    import {GetVpxinOctoberActivitys, NovemberActivityDosPay} from '@/api/api'

    export default {
        name: "List",
        data() {
            return {
                //ajax加载是否完成
                isAjaxFinished: false,
                //当前列表id
                listActive: 0,
                //已满足条件列表
                satisfaction: {
                    list: [],
                },
                //已获得资格列表
                qualify: {
                    list: [],
                },
                //是否全选
                isAllChecked: false,
                // 是否已获得资格
                isQualify: false,
                //0=未到缴费时间,1=缴费时间已过,2=缴费时间
                status: 2
            }
        },
        components: {
            ListItem,
            NoData
        },
        created() {
            this.listActive = this.$route.params.id;
            this.getListsData();
        },
        computed: {
            //总价
            totalPrice() {
                let price = 0;
                let list = this.listActive == 0 ? this.satisfaction.list : this.qualify.list;
                list.map(item => {
                    if (item.checked && item.mystatus === 0) {
                        price += Number(item.amount);
                    }
                });
                return price
            },
            isBtnShow() {
                let isBtn = true;
                if (this.listActive == 0 && this.satisfaction.list.length < 1) {
                    isBtn = false;
                }
                if (this.listActive == 0 && this.satisfaction.list.length > 0) {
                    isBtn = this.getPayStatus(this.satisfaction.list);
                }

                if (this.listActive == 1 && this.qualify.list.length < 1) {
                    isBtn = false;
                }
                if (this.listActive == 1 && this.qualify.list.length > 0) {
                    isBtn = this.getPayStatus(this.qualify.list);
                }
                return isBtn;
            }
        },
        methods: {
            //获取列表
            async getListsData() {
                let res = await GetVpxinOctoberActivitys(this.GLOBAL.USERINFO, this.$route.params.activityid);
                if (res.result > 0) {
                    let {qualifylist, satisfycondilist, status} = res.data;
                    this.satisfaction.list = this.initList(satisfycondilist);
                    this.qualify.list = this.initList(qualifylist);
                    this.status = status;
                    if (this.listActive == 1) {
                        this.getIsQualify()
                    }
                    this.isAjaxFinished = true;
                }
            },
            //调用ue支付
            async postUePayData() {
                if (this.status == 0) {
                    vant.Toast.fail("未到缴费时间");
                    return;
                }
                if (this.status == 1) {
                    vant.Toast.fail("缴费时间已过");
                    return;
                }
                if (this.isQualify && this.listActive == 1) {
                    vant.Toast.fail("您已获得资格");
                    return;
                }
                let params = this.getPayParamsObj();
                if (params.price === 0 || params.dataid === '') {
                    vant.Toast.fail("请选择需要支付的列表");
                    return;
                }

                let res = await NovemberActivityDosPay(params);
                if (res.result > 0) {
                    let that = this;
                    try {
                        window.dosPayResult = function (res) {
                            let result = JSON.parse(res);
                            if (result.result > 0) {
                                setTimeout(() => {
                                    that.getListsData()
                                }, 100);
                            } else {
                                vant.Toast.fail(res.message)
                            }
                        };
                        AppNative.blJsTunedupNativeWithTypeParamSign(1003, res.data.chargestr, res.data.sign);
                    } catch (e) {
                        vant.Toast.fail(e)
                    }
                }
            },
            //tab切换
            setTabActive(name) {
                this.listActive = name;
                this.isAllChecked = false;
                this.checkAll();
                if (name == 0 && this.satisfaction.list > 0) {
                    this.isQualify = false;
                } else {
                    this.getIsQualify()
                }
            },
            //获取参数信息
            getPayParamsObj() {
                let list = this.listActive == 0 ? this.satisfaction.list : this.qualify.list;
                let payDataObj = {
                    price: 0,
                    businessidstr: '',
                    hisidstr: '',
                    paytype: Number(this.listActive) + 1,
                    activityid: this.$route.params.activityid,
                    ...this.GLOBAL.USERINFO
                };
                list.map(item => {
                    if (item.mystatus == 0 && item.checked) {
                        payDataObj.price += item.amount;
                        payDataObj.businessidstr += item.id + '_';
                        payDataObj.hisidstr += item.hisid + '_';
                    }
                });
                payDataObj.businessidstr = payDataObj.businessidstr.substr(0, payDataObj.businessidstr.length - 1);
                payDataObj.hisidstr = payDataObj.hisidstr.substr(0, payDataObj.hisidstr.length - 1);
                return payDataObj;
            },
            //列表增加checked选项
            initList(arr) {
                arr.map(item => {
                    if (item.mystatus == 0) {
                        this.$set(item, 'checked', false);
                    }
                });
                return arr;
            },
            //单选
            setChecked(item) {
                if (this.listActive == 0) {
                    item.checked = !item.checked;
                    let isAll = true;
                    this.satisfaction.list.map(i => {
                        if (i.checked == false) {
                            isAll = false;
                        }
                    });
                    this.isAllChecked = isAll;
                    return
                }
                this.qualify.list.map(i => {
                    if (i.hisid == item.hisid) {
                        i.checked = !i.checked;
                    } else {
                        i.checked = false;
                    }
                });
            },
            //全选
            checkAll() {
                this.isAllChecked = !this.isAllChecked;
                this.satisfaction.list.map(item => {
                    item.checked = this.isAllChecked;
                });
            },
            //是否已获得资格
            getIsQualify() {
                let list = this.qualify.list;
                list.map(item => {
                    if (item.mystatus == 1) {
                        this.isQualify = true
                    }
                });
            },
            getPayStatus(arr) {
                let flag = false;
                arr.map((item) => {
                    if (item.mystatus == 0) {
                        flag = true;
                    }
                })
                return flag;
            }
        }

    }
</script>

<style scoped lang="scss">
    .list {
        height: 100%;
        padding-bottom: 80px;
        background-color: #f7f7fc;

        /deep/ .van-tabs {
            background-color: #f7f7fc;
            padding-bottom: 60px;
        }

        /deep/ .van-tabs__line {
            background-color: #2ea2fa !important;
        }

        /deep/ .van-tab--active {
            color: #2ea2fa !important;
        }

        /deep/ .van-tabs__content {
            padding: 15px;
        }

        /deep/ .van-submit-bar {
            &.active {
                /deep/ .van-submit-bar__text {
                    text-align: left;
                }
            }

            .van-submit-bar__bar {

                padding-left: 15px;

                .van-submit-bar__price {
                    color: #2ea2fa;
                }

                .van-button--danger {
                    background-color: #2ea2fa;
                    color: #fff;
                    border-color: #2ea2fa;
                }

            }

        }


    }

</style>
