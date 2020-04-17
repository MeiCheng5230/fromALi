<template>
    <div class="Transfer">
        <div class="mainCom">
            <div class="topBg" :style="{backgroundImage:'url('+require('@/assets/images/bg_SVC.png')+')'}">
                <div class="cardTit">已选充值码</div>
                <div class="cardNum"><span>4</span>张</div>
            </div>
            <div class="infoBx" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
                <div class="tit">账号信息</div>
                <div class="info ID"><span>账号：1510238195（充值商）</span></div>
                <div class="info">姓名：李**</div>
                <div class="info">手机号：131***000</div>
            </div>
            <div class="separate" :style="{backgroundImage:'url('+require('@/assets/images/bg_2.png')+')'}"></div>
            <div class="codeBx" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
                <div class="tit">充值码</div>
                <div class="num">
                    <span>SV</span>
                    <div>1000*3</div>
                </div>
            </div>
            <div class="separate" :style="{backgroundImage:'url('+require('@/assets/images/bg_2.png')+')'}"></div>
            <div class="total" :style="{backgroundImage:'url('+require('@/assets/images/bg_1.png')+')'}">
                <div>总计</div>
                <span>SV：3000</span>
            </div>
            <div class="final" :style="{backgroundImage:'url('+require('@/assets/images/bg_3.png')+')'}"></div>
        </div>
        <div class="transfer_btn">转让</div>
        <div class="remarks">
            <p>温馨提示：<br>1. 充值码(SVC)只能转让给充值商</p>
        </div>
        <div class="pop" v-if="inputFlag">
            <div class="popcom">
                <div class="tit">提取码转让</div>
                <div class="close" @click="changeFn"><img src="@/assets/images/ic_cancel.png" alt=""></div>
                <div class="inp">
                    <input type="number" placeholder="请输入对方账号">
                </div>
                <div class="Btn">
                    <button @click="changeFn('change')">确定</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            inputFlag: false
        }
    },
    methods: {
        changeFn(data) {  // 点击转让
            if (data == "input") {
                this.inputFlag = true;
            } else if (data == "change") {
                // 修改对方账号
            } else {
                this.inputFlag = false;
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.Transfer {
    min-height: 100%;
    background: #f7f7fc;
    .mainCom {
        padding: 0.3rem;
        font-family: PingFangSC-Regular;
        .separate {
            background-repeat: no-repeat;
            background-size: 100% auto;
            height: 0.4rem;
        }
        .final {
            background-repeat: no-repeat;
            background-size: 100% auto;
            height: 0.4rem;
        }
        .topBg {
            background-repeat: no-repeat;
            background-size: 100% 100%;
            padding: 0.3rem 0.5rem;
            margin: 0 0.1rem;
            .cardTit {
                color: rgba(51, 51, 51, 0.5);
                font-size: 0.2rem;
            }
            .cardNum {
                color: #fff;
                font-size: 0.24rem;
                font-family: PingFangSC-Regular;
                padding-bottom: 0.7rem;
                span {
                    font-size: 1rem;
                }
            }
        }
        .infoBx {
            background-repeat: no-repeat;
            background-size: 100% 100%;
            padding: 0.2rem 0.4rem;
            font-size: 0.28rem;
            .info {
                color: #999;
                padding: 0.05rem 0;
            }
            .ID {
                display: flex;
                align-items: center;
                span {
                    flex: auto;
                }
                button {
                    background: #fff;
                    padding: 0.1rem 0.2rem;
                    font-size: 0.24rem;
                    color: #2ea2fa;
                    border: 1px #2ea2fa solid;
                    outline: none;
                    border-radius: 0.05rem;
                }
            }
        }
        .codeBx {
            background-size: 100% 100%;
            font-size: 0.28rem;
            padding: 0 0.4rem;
            .num {
                display: flex;
                color: #999;
                padding: 0.05rem 0;
                span {
                    flex: auto;
                }
            }
        }
        .total {
            background-size: 100% 100%;
            display: flex;
            font-size: 0.28rem;
            padding: 0 0.4rem;
            div {
                flex: auto;
                padding-bottom: 0.2rem;
            }
            span {
                color: #999;
            }
        }
    }
    .transfer_btn {
        margin: 0.8rem;
        text-align: center;
        font-size: 0.32rem;
        color: #fff;
        background: #2ea2fa;
        border-radius: 0.06rem;
        padding: 0.22rem;
        font-family: PingFangSC-Medium;
    }
    .remarks {
        padding: 0 0.8rem;
        font-size: 0.24rem;
        color: #999;
        letter-spacing: 1px;
        line-height: 0.35rem;
    }
    .pop {
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        z-index: 3;
        .popcom {
            background: #fff;
            position: fixed;
            bottom: 0;
            width: 100%;
            border-radius: 0.2rem 0.2rem 0 0;
            padding: 0 0.3rem;
            box-sizing: border-box;
            .tit {
                padding-top: 0.2rem;
                font-size: 0.32rem;
                color: #000;
            }
            .close {
                position: absolute;
                top: 0.2rem;
                right: 0.3rem;
                img {
                    width: 0.32rem;
                    height: 0.32rem;
                }
            }
            .inp {
                padding: 0.3rem 0;
                border-bottom: 1px solid #dedede;
                box-sizing: border-box;
                input {
                    border: none;
                    padding: 0 0.3rem;
                    width: 100%;
                    height: 0.8rem;
                    background: #f7f7fc;
                    box-sizing: border-box;
                    border-radius: 0.06rem;
                }
            }
            .Btn {
                width: 100%;
                padding: 0.6rem 0 0.3rem 0;
                button {
                    width: 100%;
                    padding: 0.2rem 0;
                    background: #ffb90f;
                    border: none;
                    outline: none;
                    border-radius: 0.06rem;
                    color: #fff;
                }
            }
        }
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .popcom {
        padding-bottom: env(safe-area-inset-bottom);
    }
}
</style>
