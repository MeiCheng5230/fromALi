<template>
    <div class="explain">
        <div class="inp">
            <div class="tit">达人达语</div>
            <div class="textarea">
                <div class="inpbx">
                    <div style="display: flex; align-items: center;">
                        <span>&</span><input type="text" v-model="explainText1" id="inp1" autocomplete="off" @keyup="SetInput('inp1', $event)" maxlength="24" placeholder="点击输入属于你的语录"/>
                    </div>
                    <div style="display: flex; align-items: center;">
                        <span>&</span><input type="text" v-model="explainText2" id="inp2" autocomplete="off" @keyup="SetInput('inp2', $event)" maxlength="24" placeholder="一行最多输入24个字符"/>
                    </div>
                    <div style="display: flex; align-items: center;">
                        <span>&</span><input type="text" v-model="explainText3" id="inp3" autocomplete="off" @keyup="SetInput('inp3', $event)" maxlength="24"  placeholder="最多输入三行"/>
                    </div>
                </div>
            </div>
        </div>
        <div class="btn">
            <button @click="UpdateGreetings">保存</button>
        </div>
    </div>
</template>

<script>
import { UpdateGreetings } from '@/api/getData';
export default {
    data() {
        return {
            explainText1: '',
            explainText2: '',
            explainText3: '',
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        if(userinfo.greetings) {
            this.explainText1 = userinfo.greetings.split(',')[0];
            this.explainText2 = userinfo.greetings.split(',')[1];
            this.explainText3 = userinfo.greetings.split(',')[2];
        }
    },
    methods: {
        async UpdateGreetings() {
            var explain1 = this.explainText1?this.explainText1+',':'';
            var explain2 = this.explainText2?this.explainText2+',':'';
            var explain3 = this.explainText3?this.explainText3:'';
            var greetings = explain1 + explain2 + explain3;

            let result = await UpdateGreetings({greetings: greetings, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
        SetInput(type, e) {
            if (type == 'inp1') {
                if (this.explainText1.length == 24 || e.keyCode == 13) {
                    e.target.blur();
                    document.querySelector("#inp2").focus();
                }
            } else if (type == 'inp2') {
                if (this.explainText2.length == 24 || e.keyCode == 13) {
                    e.target.blur();
                    document.querySelector("#inp3").focus();
                } else if (e.keyCode == 8 && !this.explainText2.length) {
                    e.target.blur();
                    document.querySelector("#inp1").focus();
                }
            } else if (type == 'inp3') {
                if (e.keyCode == 8 && !this.explainText3.length) {
                    e.target.blur();
                    document.querySelector("#inp2").focus();
                }
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.explain {
    min-height: 100%;
    padding: 0.3rem;
    box-sizing: border-box;
    .inp {
        .tit {
            margin-bottom: 0.3rem;
        }
        .textarea {
            position: relative;
            .remark {
                position: absolute;
                top: 0.3rem;
                left: 0.3rem;
                p {
                    color: #999;
                    margin: 0;
                    span {
                        color: #000;
                        margin-right: 0.1rem;
                    }
                }
            }
        }
        .inpbx {
            background: #f7f7fc;
            padding: 0.3rem;
            input {
                flex: auto;
                margin-left: 0.2rem;
                background: #f7f7fc;
                border: none;
                font-size: 0.26rem;
                padding: 0.1rem 0;
                &::placeholder {
                    color: #999;
                }
            }
        }
        textarea {
            width: 100%;
            border: none;
            background: #f7f7fc;
            padding: 0.3rem;
            box-sizing: border-box;
            resize: none;
        }
    }

    .btn {
        margin-top: 5rem;
        button {
            background: #2ea2fa;
            border-radius: 0.04rem;
            color: #fff;
            width: 100%;
            border: none;
            padding: 0.28rem 0;
        }
    }
}
</style>
