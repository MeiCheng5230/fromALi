 <template>
    <div class="agreement">
        <div class="topImg">
            <img src="@/assets/images/get_expert_top_bg.png" alt="">
        </div>
        <div class="cont">
            <div class="trans">
                <div class="tit">
                    <div class="bor"></div>
                    <div>关于达人</div>
                    <div class="bor"></div>
                </div>
                <div class="ops">
                    <div class="lftImg"><img src="@/assets/images/get_expert_bg_01.png"/></div>
                    <div class="rgtTxt">
                        <div class="ttx">最自由</div>
                        <div class="tx">不受地点限制，随时随地随心，接收倍率由你定</div>
                    </div>
                </div>
                <div class="ops">
                    <div class="lftImg"><img src="@/assets/images/get_expert_bg_02.png"/></div>
                    <div class="rgtTxt">
                        <div class="ttx">最便捷</div>
                        <div class="tx">只需一台手机，聊天、分享、解答即可赚P点</div>
                    </div>
                </div>
                <div class="ops">
                    <div class="lftImg"><img src="@/assets/images/get_expert_bg_03.png"/></div>
                    <div class="rgtTxt">
                        <div class="ttx">最专业</div>
                        <div class="tx">用自己的专业知识帮助他人，帮人赚钱两不误</div>
                    </div>
                </div>
            </div>
        </div>
        <router-link to="/agreement" tag="div" class="btn">我要成为达人</router-link>
    </div>
</template>

<script>
export default {
    
}
</script>

<style lang="scss" scoped>
.agreement {
    padding-bottom: 1rem;
    .topImg {
        img {
            width: 100%;
        }
    }
    .cont {
        padding: 0 0.3rem;
        .trans {
            width: 100%;
            background-color: #fdffff;
            box-shadow: 0px 2px 16px 0px 
                rgba(112, 148, 254, 0.34);
            border-radius: 0.22rem;
            transform: translateY(-1rem);
            .tit {
                display: flex;
                justify-content: center;
                align-items: center;
                color: #7a98f2;
                text-align: center;
                font-weight: bold;
                font-size: 0.42rem;
                padding: 0.46rem 0;
                .bor {
                    width: 0.2rem;
                    height: 0.2rem;
                    border: 2px solid #7a98f2;
                    transform: rotate(45deg);
                    margin: 0 0.4rem;
                }
            }
            .ops {
                padding: 0.3rem;
                display: flex;
                .lftImg {
                    img {
                        width: 2rem;
                        margin-right: 0.36rem;
                    }
                }
                .rgtTxt {
                    .ttx {
                        font-size: 0.36rem;
                        font-weight: bold;
                        padding: 0.2rem 0;
                    }
                    .tx {
                        line-height: 0.6rem;
                    }
                }
            }
        }
    }
    .btn {
        width: 100%;
        padding: 0.28rem;
        background: #66a7f8;
        color: #fff;
        text-align: center;
        position: fixed;
        bottom: 0;
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .btn {
       padding-bottom: env(safe-area-inset-bottom);
    }
}
</style>
