<template>
    <div class="company">
        <div class="uploadImg">
            <div class="opt">
                <div class="top">
                    <div class="tit">请上传您的营业执照</div>
                    <div class="num">（{{ businesslicense.length }}/1）</div>
                </div>
                <van-uploader v-model="businesslicense" multiple :max-count="1" />
            </div>
            <div class="opt">
                <div class="top">
                    <div class="tit">请上传您的开户许可证照</div>
                    <div class="num">（{{ openingPermit.length }}/1）</div>
                </div>
                <van-uploader v-model="openingPermit" multiple :max-count="1" />
            </div>
            <div class="opt">
                <div class="top">
                    <div class="tit">请上传您的公司照片</div>
                    <div class="num">（{{ companyImg.length }}/9）</div>
                </div>
                <van-uploader v-model="companyImg" multiple :max-count="9" />
            </div>
        </div>
        <div class="uploadBtn" v-if="!authentication">上传</div>
        <div class="uploadBtn authenticated" v-else>已认证</div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            businesslicense: [],        // 营业执照
            openingPermit: [],          // 开户许可证
            companyImg: [],             // 公司照片
            authentication: false,      // true——已认证，false——未认证
        }
    }
}
</script>
<style lang="scss" scoped>
.company {
    min-height: 100%;
    background: #f7f7fc;
    padding-bottom: 0.3rem;
    box-sizing: border-box;
    .uploadImg {
        font-size: 0.28rem;
        .opt {
            padding: 0.3rem;
            background: #fff;
            margin-bottom: 0.2rem;
            .top {
                display: flex;
                margin-bottom: 0.3rem;
                .tit {
                    flex: auto;
                }
                .num {
                    color: #999;
                }
            }
            /deep/ .van-uploader {
                .van-uploader__upload {
                    width: 2.2rem;
                    height: 2.2rem;
                    margin: 0 0.1rem 0.1rem 0;
                }
                .van-uploader__preview {
                    margin: 0 0.1rem 0.1rem 0;
                    .van-uploader__preview-image {
                        width: 2.2rem;
                        height: 2.2rem;
                    }
                }
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
        &.authenticated {
            background: #cdcdcd;
        }
    }
}
</style>
