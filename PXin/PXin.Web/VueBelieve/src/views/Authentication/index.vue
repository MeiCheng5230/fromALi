<template>
    <div class="authentication">
        <div class="option">
            <router-link to="/personal" tag="div" class="ops">
                <div class="lft">
                    <div class="tp">个人资料认证</div>
                    <div class="bt">身份证正反面、手持身份证照片</div>
                </div>
                <div class="rgt"><span>去认证</span><img src="@/assets/images/ic_enter@2x.png" alt=""></div>
            </router-link>
            <router-link to="/company" tag="div" class="ops">
                <div class="lft">
                    <div class="tp">公司资料认证</div>
                    <div class="bt">公司照、营业执照原件</div>
                </div>
                <div class="rgt"><span>已认证</span><img src="@/assets/images/ic_enter@2x.png" alt=""></div>
            </router-link>
            <router-link to="/contract" tag="div" class="ops">
                <div class="lft">
                    <div class="tp">合同照片</div>
                    <div class="bt">签字后的合同原价</div>
                </div>
                <div class="rgt"><span>审核未通过</span><img src="@/assets/images/ic_enter@2x.png" alt=""></div>
            </router-link>
            <router-link to="/replenish" tag="div" class="ops">
                <div class="lft">
                    <div class="tp">补充资料</div>
                    <div class="bt">需要补充的相关资料</div>
                </div>
                <div class="rgt"><span>审核中</span><img src="@/assets/images/ic_enter@2x.png" alt=""></div>
            </router-link>
        </div>
        <div class="explain">
            <p>说明：</p>
            <p>1.请上传真实有效的资料，若有虚假将导致账号被冻结无法使用</p>
            <p>2.请下载合同样本，根据真实资料填好并签署后，再拍照上传，可上传多张合同照片</p>
            <p>3.充值商由“相信官方”审核，代理人资料由“充值商”审核</p>
        </div>
        <div class="copy">
            <div class="text">合同下载链接：<span>{{ coypURL }}</span></div>
            <button class="copyBtn">复制</button>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            coypURL: "http://www.123456789.com",
        }
    }
}
</script>

<style lang="scss" scoped>
.authentication {
    min-height: 100%;
    background: #f7f7fc;
    font-family: PingFangSC-Regular;
    padding-bottom: 0.8rem;
    box-sizing: border-box;
    .option {
        .ops {
            background: #fff;
            display: flex;
            padding: 0.2rem 0.3rem;
            margin-bottom: 0.2rem;
            .lft {
                flex: auto;
                .tp {
                    font-size: 0.32rem;
                    margin-bottom: 0.05rem;
                }
                .bt {
                    font-size: 0.24rem;
                    color: #999;
                }
            }
            .rgt {
                display: flex;
                align-items: center;
                font-size: 0.26rem;
                span {
                    color: #999;
                }
                img {
                    height: 0.3rem;
                    width: auto;
                    margin-left: 0.05rem;
                }
            }
        }
    }
    .explain {
        padding: 0.3rem;
        font-size: 0.24rem;
        color: #999;
        margin-top: 2rem;
        p {
            margin: 0;
            line-height: 0.4rem;
            letter-spacing: 0.02rem;
        }
    }
    .copy {
        margin: 0 0.3rem;
        background: #fff;
        font-size: 0.24rem;
        display: flex;
        .text {
            flex: auto;
            color: #999;
            padding: 0.15rem 0.2rem;
            border-radius: 0.06rem 0 0 0.06rem;
        }
        .copyBtn {
            background: #2ea2fa;
            color: #fff;
            border: none;
            outline: none;
            border-radius: 0 0.06rem 0.06rem 0;
            padding: 0 0.35rem;
            letter-spacing: 0.02rem;
        }
    }
}
</style>
