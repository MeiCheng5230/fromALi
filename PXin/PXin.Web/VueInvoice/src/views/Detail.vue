<template>
    <div class="detail">
        <div class="detail-img">
            <img :src="imgUrl" alt="">
        </div>
        <div class="operate">
            <van-button class="down" size="large" @click="LoadInvoice">下载电子发票</van-button>
            <van-button class="email" size="large" @click="showEmail=true">发送到邮箱</van-button>
        </div>
        <div class="popup" v-show="showEmail">
            <div class="email">
                <div class="tit">收票人邮箱地址</div>
                <div class="inp"><input type="text" v-model="email" placeholder="请输入您的邮箱地址"/></div>
                <div class="err">{{ errorText }}</div>
                <div class="btn">
                    <div class="cancel" @click="HideEmail">取消</div>
                    <div class="send" @click="SetSendEmail">发送</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { SendEmail } from '@/api/api';
    export default {
        name: "Detail",
        data() {
            return{
                email: '',      // 邮箱地址
                showEmail: false,   // 发送邮箱显示隐藏
                errorText: '',   // 发送失败错误提示
                infoid: '', // 请求id
                imgUrl: '', // 发票详情图片
            }
        },
        created() {
            this.imgUrl = this.$route.query.imgUrl;
        },
        methods: {
            SetSendEmail() {
            // 点击发送
                const reg = new RegExp("^[a-z0-9A-Z]+[- | a-z0-9A-Z . _]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$");
                if(!this.email.length) {
                    this.errorText = '请输入您的邮箱地址';
                } else if(!reg.test(this.email)) {
                    this.errorText = '发送失败，请输入正确的邮箱地址';
                    this.email = '';
                } else {
                    this.errorText = '';
                    this.SendEmail(this.email);
                    this.email = '';
                    this.showEmail = false;
                }
            },
            HideEmail() {
            // 点击取消发送邮箱
                this.email = '';
                this.showEmail = false;
                this.errShow = false;
            },
            LoadInvoice() {
            // 下载电子发票
                var Url2 = this.imgUrl;
                var oInput = document.createElement("input");
                oInput.value = Url2;
                document.body.appendChild(oInput);
                oInput.select(); // 选择对象
                document.execCommand("Copy"); // 执行浏览器复制命令
                oInput.className = "oInput";
                oInput.style.display = "none";
                vant.Toast('电子发票PDF文件下载地址已复制，请至浏览器访问下载');
            },
            async SendEmail(email) {
            // 发送邮件
                let result = await SendEmail({...this.GLOBAL.USERINFO, email});
                if (result.result > 0) {
                    vant.Toast.success('发送成功，注意查收邮件!');
                } else {
                    vant.Toast.fail(result.message);
                }
            },
        }
    }
</script>

<style scoped lang="scss">
    .detail{
        padding: 15px;
        .detail-img{
            width: 100%;
            height: 223px;
            background-color: #fff;
            text-align: center;
            img {
                max-width: 100%;
                max-height: 100%;
            }
        }
        .operate{
            padding-top: 50px;
            .down{
                margin-bottom: 15px;
                background-color: #2ea2fa;
                color: #fff;
                font-size: 18px;
            }
            .email{
                font-size: 18px;
                color: #2ea2fa;
                border-color: #2ea2fa;
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
            padding: 0 53px;
            box-sizing: border-box;
            .email {
                font-size: 18px;
                background: #fff;
                width: 100%;
                border-radius: 5px;
                text-align: center;
                .tit {
                    padding: 20px 0;
                    font-weight: bold;
                }
                .inp {
                    border-radius: 2px;
                    padding: 0 18px;

                    input {
                        font-size: 14px;
                        padding: 12px 15px;
                        box-sizing: border-box;
                        width: 100%;
                        border: none;
                        background: #f7f7fc;
                    }
                }
                .err {
                    height: 18px;
                    padding: 10px 0 15px 18px;
                    text-align: left;
                    color: #ff1541;
                    font-size: 12px;
                }
                .btn {
                    display: flex;
                    border-top: 1px solid #ddd;
                    
                    .cancel {
                        flex: 1;
                        border-right: 1px solid #ddd;
                        color: #999;
                        padding: 18px 0 14px;
                    }
                    .send {
                        flex: 1;
                        color: #2ea2fa;
                        padding: 18px 0 14px;
                    }
                }
            }
        }
    }

</style>
