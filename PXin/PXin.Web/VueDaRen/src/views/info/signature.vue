<template>
    <div class="signature">
        <div class="ops">
            <div class="tit">签名</div>
            <input type="text" placeholder="签名可输入表达话题相关的词" />
        </div>
        <div class="btn" v-if="showBtn">
            <button>保存</button>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            showBtn: false,
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        if(userinfo.status == 0 || userinfo.status == 2) {
            this.showBtn = true;
        }
    }
}
</script>

<style lang="scss" scoped>
.signature {
    min-height: 100%;
    background: #f7f7fc;
    .ops {
        display: flex;
        background: #fff;
        padding: 0.3rem;
        input {
            border: none;
            outline: none;
            margin-left: 0.3rem;
            flex: auto;
        }
    }
    .btn {
        padding: 2.6rem 0.3rem 0 0.3rem;
        button {
            width: 100%;
            border: none;
            border-radius: 0.04rem;
            background: #2ea2fa;
            color: #fff;
            padding: 0.28rem 0;
        }
    }
}
</style>
