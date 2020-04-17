<template>
    <div class="home">
        <div class="popup" v-if="dialog">
            <div class="dialog">
                <div class="tit">您当前充值累计</div>
                <div class="num">{{ sv }}</div>
                <div class="txt">申请达人需三个月内使用SVC充值码、SVC提取码充值SV累计满1万，才可获取申请资格</div>
                <div class="btn" @click="SetClose">知道了</div>
            </div>
        </div>
    </div>
</template>

<script>
import { GetAbovementionedData } from '@/api/getData';
export default {
    data() {
        return {
            dialog: false,
            sv: 0,
        }
    },
    created() {
        this.GetAbovementionedData();
    },
    methods: {
         async GetAbovementionedData() {
            let result = await GetAbovementionedData(this.$global.userInfo);
            if(result.result > 0) {
                if (result.data.status == 0) {
                    this.$router.push('/open')
                } else if (result.data.status == -1) {
                    this.dialog = true;
                    this.sv = result.data.sv;
                } else {
                    this.$router.push('/info');
                }
            } else {
                this.Toast(result.message)
            }
        },
        SetClose() {
            try {
                AppNative.blJsTunedupNativeWithTypeParamSign(1001, '', '');
            } catch (error) {
                this.Toast.fail("请在相信App中打开");
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.popup {
    width: 100%;
    height: 100%;
    position: fixed;
    top: 0;
    left: 0;
    background: rgba($color: #000000, $alpha: 0.5);
    .dialog {
        width: 5rem;
        padding: 0.5rem;
        box-sizing: border-box;
        min-height: 5rem;
        border-radius: 0.1rem;
        background: #fff;
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        .tit {
            text-align: center;
            font-weight: bold;
        }
        .num {
            text-align: center;
            font-size: 0.5rem;
            padding: 0.3rem 0;
            font-weight: bold;
            color: #2ea2fa;
        }
        .txt {
            color: #999;
            line-height: 0.5rem;
        }
        .btn {
            margin-top: 0.3rem;
            background: #2ea2fa;
            text-align: center;
            padding: 0.1rem 0;
            border-radius: 0.04rem;
            color: #fff;
        }
    }
}
</style>
