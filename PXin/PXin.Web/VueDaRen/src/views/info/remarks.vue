<template>
    <div class="remarks">
        <div class="switch">
            <div class="tit">开启欢迎语</div>
            <div class="rgt" @click="SetOpen">
                <img v-show="open" src="@/assets/images/setting_btn_sel@2x.png" alt="">
                <img v-show="!open" src="@/assets/images/setting_btn_nor@2x.png" alt="">
            </div>
        </div>
        <div class="set">
            <div class="tit">设置欢迎语</div>
            <div class="inp">
                <textarea name="" id="" cols="30" v-model="welcome" rows="10" maxlength="100" placeholder="请输入10-100字欢迎语"></textarea>
            </div>
        </div>
        <div class="btn">
            <button :class="handle?'' : 'fillbtn'" @click="UpdateWelcome">保存</button>
        </div>
    </div>
</template>

<script>
import { UpdateWelcome } from '@/api/getData';
export default {
    data() {
        return {
            open: true,
            welcome: '',
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        if (userinfo.welcome) {
            this.welcome = userinfo.welcome;
        }

        var status = userinfo.iswelcome;
        if (status == 0) {
            this.open = false;
        } else {
            this.open = true;
        }; 
    },
    computed: {
        handle() {
            if (!this.open) {
                return true;
            } else {
                if(this.welcome.length >= 10) {
                    return true
                }
                return false;
            }
        }
    },
    methods: {
        async UpdateWelcome() {
            if(!this.handle) {
                return false;
            }
            var text = this.welcome;
            if (this.open) {
                text = '1|' + text;
            } else {
                text = '0|' + text;
            }
            let result = await UpdateWelcome({welcome: text, ...this.$global.userInfo});
            if (result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
        SetOpen() {
            this.open = !this.open;
        }
    }
}
</script>

<style lang="scss" scoped>
.remarks {
    .switch {
        background: #f7f7fc;
        display: flex;
        align-items: center;
        padding: 0.25rem 0.3rem;
        .tit {
            flex: auto;
        }
        .rgt {
            img {
                height: 0.38rem;;
            }
        }
    }
    .set {
        padding: 0.3rem;
        .inp {
            background: #f7f7fc;
            margin-top: 0.3rem;
            textarea {
                background: #f7f7fc;
                width: 100%;
                box-sizing: border-box;
                border: none;
                padding: 0.3rem 0.25rem;
                resize: none;
            }
        }
    }
    .btn {
        margin-top: 3.7rem;
        padding: 0 0.3rem;
        button {
            background: #2ea2fa;
            color: #fff;
            border-radius: 0.04rem;
            border: none;
            width: 100%;
            padding: 0.28rem 0;
            &.fillbtn {
                background: rgba($color: #2ea2fa, $alpha: 0.5);
            }
        }
    }
}
</style>
