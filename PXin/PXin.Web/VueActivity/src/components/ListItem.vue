<template>
    <van-cell class="item" @click="setChecked(item)">
        <div slot="title">
            <div>
                <span class="custom-title">{{titleStr[item.typeid]}}</span>
                <span class="primary" v-if="item.paystatus==1&&item.nodeid!=1">对方已支付</span>
                <span class="default" v-if="item.paystatus==0&&item.nodeid!=1">对方尚未支付</span>
            </div>
            <div :class="item.mystatus ==0?'status':'status active'" v-if="item.mystatus>0">
                {{myStatusStr[item.mystatus]}}
            </div>
        </div>
        <div slot="label">
            <div class="userInfo">
                <div class="name">
                    {{setCodeName(item.nodename)}}{{item.nodeid==1?'':'（'+item.nodecode+'）'}}
                </div>
                <div>{{item.createtime}}</div>
            </div>
        </div>
        <div slot="right-icon" v-if="item.mystatus==0&&listActive==0" class="checked">
            <img :src="item.checked? require('@/assets/images/ic_seleted_pay.png'):require('@/assets/images/ic_unseleted_pay.png')"
                 alt height="24"/>
        </div>
        <div slot="right-icon" v-if="listActive==1&&item.mystatus!=1" class="checked">
            <img :src="item.checked? require('@/assets/images/ic_seleted_pay.png'):require('@/assets/images/ic_unseleted_pay.png')"
                 alt height="24"/>
        </div>
    </van-cell>
</template>

<script>
    export default {
        name: "ListItme",
        props: {
            item: {
                type: Object,
                required: true,
            },
            listActive: {
                type: Number,
                required: true,
            },
            isQualify: {
                type: Blob,
                required: false,
            }
        },
        data() {
            return {
                titleStr: ['', '充值商进货', '代理人进货', '零售SVC充值码并充值SV'],
                myStatusStr: ['未支付', '已支付', '已发货', '已退款'],
            }
        },
        methods: {
            //隐藏名称
            setCodeName(str) { // start从前往后第几位，end从后往前第几位
                let leg = str.length;
                let text = str.substr(0, 1) + ('*'.repeat(leg));
                return text;
            },
            //单选
            setChecked(item) {
                this.$emit('setChecked', item)
            },
        }
    }
</script>

<style scoped lang="scss">
    .van-cell {
        .custom-title {
            font-weight: 700 !important;
            display: inline-block;

        }

        .primary, .default {
            display: inline-block;
            font-size: 10px;
            padding: 2px;
            border: 1px solid #999;
            border-radius: 2px;
            line-height: normal;
            margin-left: 5px;
        }

        .primary {
            color: #2ea2fa;
            border-color: #2ea2fa;
        }
        .default{
            color: #999;
            border-color: #999;
        }

        background-color: #ffffff;
        box-shadow: 0px 1px 15px 0px rgba(141, 171, 196, 0.15);
        border-radius: 6px;
        margin-bottom: 20px;

        &.disable {
            background-color: #eeeef1;

            .van-cell__title {
                color: #999;
            }
        }

        .van-cell__title {
            position: relative;
            text-align: left;
            font-size: 16px;
            color: #333;

            .van-tag {
                margin-left: 10px;

                &.van-tag--primary {
                    color: #2ea2fa;
                    border-color: #2ea2fa;
                }
            }

            .status {
                color: #2ea2fa;
                font-size: 12px;
                display: inline-block;
                position: absolute;
                right: -20px;
                top: 0px;

                &.active {
                    right: 0;
                }
            }

            .van-cell__label {
                margin-top: 15px;

                .userInfo {
                    .name {
                        font-size: 14px;
                    }
                }
            }
        }

        .checked {
            display: flex;
            align-items: center;
        }
    }

</style>
