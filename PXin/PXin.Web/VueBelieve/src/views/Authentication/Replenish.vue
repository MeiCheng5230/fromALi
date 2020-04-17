<template>
    <div class="replenish">
        <div class="uploadImg" v-if="!verify">
            <div class="top">
                <div class="tit">请上传您的补充资料</div>
                <div class="num">（{{ fileList.length }}/9）</div>
            </div>
            <van-uploader v-model="fileList" multiple :max-count="9"/>
        </div>

        <div class="uploadImg" v-else>
            <div class="top">
                <div class="tit">您已上传的补充资料</div>
                <div class="num">（{{ ImgList.length }}/9）</div>
            </div>
            <div class="ImgList">
                <div class="Img" v-for="item in ImgList" :key="item.index">
                    <img :src="item.content" alt="补充资料"/>
                </div>
            </div>
        </div>
        
        <div class="btnbx">
            <button class="upload-btn" v-if="!verify">上传</button>
            <button class="verify-btn" v-else>审核中</button>
        </div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            fileList: [],       // 要上传的补充资料图片
            verify: true,       // 审核中——上传
            ImgList: [          // 正在审核中的补充资料
                {content: require("@/assets/images/ic_wechat_pay.png")},
                {content: require("@/assets/images/ic_zfb_pay.png")},
                {content: require("@/assets/images/ic_wechat_pay.png")},
                {content: require("@/assets/images/ic_zfb_pay.png")},
                {content: require("@/assets/images/ic_wechat_pay.png")},
                {content: require("@/assets/images/ic_zfb_pay.png")},
                {content: require("@/assets/images/ic_wechat_pay.png")},
            ],
        }
    }
}
</script>

<style lang="scss" scoped>
.replenish {
    height: 100%;
    display: flex;
    flex-direction: column;
    background: #f7f7fc;
    font-size: 0.28rem;
    .uploadImg {
        background: #fff;
        padding: 0.3rem;
        padding-bottom: 0.1rem;
        .top {
            display: flex;
            padding-bottom: 0.3rem;
            .tit {
                flex: auto;
            }
            .num {
                color: #999;
            }
        }
        .ImgList {
            .Img {
                display: inline-block;
                margin: 0 0.2rem 0.2rem 0;
                img {
                    width: 2.1rem;
                    height: 2.1rem;
                }
            }
        }
        /deep/ .van-uploader {
            .van-uploader__wrapper {
                .van-uploader__preview {
                    margin: 0 0.2rem 0.2rem 0;
                    .van-uploader__preview-image {
                        width: 2.1rem;
                        height: 2.1rem;
                    }
                }
                .van-uploader__upload {
                    width: 2.1rem;
                    height: 2.1rem;
                    margin: 0 0.2rem 0.2rem 0;
                }
            }
        }
        .uploadBtn {
            margin: 0.5rem 0.3rem 0 0.3rem;
            background: #2ea2fa;
            text-align: center;
            border-radius: 0.06rem;
            padding: 0.21rem 0;
            color: #fff;
        }
    }
    .btnbx {
        flex: auto;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        button {
            margin: 0.3rem;
            border: none;
            outline: none;
            border-radius: 0.04rem;
            background: #2ea2fa;
            color: #fff;
            padding: 0.21rem 0;
            &.verify-btn {
                background: #cdcdcd;
            }
        }
    }
}
</style>
