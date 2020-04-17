<template>
    <div class="personal">
        <div class="uploadImg">
            <div class="title">请上传您的身份证</div>
            <div class="opt">
                <van-uploader :after-read="afterRead1" />
                <img class="showImg" :src="frontImg" alt="身份证正面"/>
            </div>
            <div class="opt">
                <van-uploader :after-read="afterRead2" />
                <img class="showImg" :src="reverseImg" alt="身份证正面"/>
            </div>
            <div class="opt">
                <van-uploader :after-read="afterRead3" />
                <img class="showImg" :src="inhandImg" alt="身份证正面"/>
            </div>
        </div>
        <div class="uploadBtn">上传</div>
    </div>
</template>

<script>
export default {
    data () {
        return {
            frontImg: require('@/assets/images/image_card_1@2x.png'),   // 身份证正面照片
            reverseImg: require('@/assets/images/image_card_2@2x.png'), // 身份证反面照片
            inhandImg: require('@/assets/images/image_card_3@2x.png'),  // 手持身份证照片
        }
    },
    methods: {
        afterRead1(file) {      // 身份证正面
           this.frontImg = file.content
        },
        afterRead2(file) {      // 身份证反面
           this.reverseImg = file.content
        },
        afterRead3(file) {      // 手持身份证
           this.inhandImg = file.content
        },
    }
}
</script>

<style lang="scss" scoped>
.personal {
    background: #f7f7fc;
    min-height: 100%;
    padding-bottom: 0.3rem;
    box-sizing: border-box;
    .uploadImg {
        padding: 0 0.65rem;
        .title {
            color: #999;
            font-size: 0.24rem;
            padding: 0.3rem 0;
        }
        .opt {
            position: relative;
            margin-bottom: 0.2rem;
            /deep/ .van-uploader {
                position: absolute;
                width: 100%;
                height: 100%;
                opacity: 0;
                .van-uploader__wrapper {
                    width: 100%;
                    height: 100%;
                    .van-uploader__upload {
                        width: 100%;
                        height: 100%;
                    }
                }
            }
            .showImg {
                width: 100%;
                height: 3rem;
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
</style>
