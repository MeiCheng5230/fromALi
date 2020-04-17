<template>
    <div class="result">
        <div class="comInfo">
            <img src="@/assets/images/icon_success.png" alt="">
            <div class="suc">提交成功</div>
            <div class="txt">后续可在已申请发票列表中查看申请发票进度</div>
        </div>
        <div class="btn">
            <button @click="$router.go(-2)">回到开票首页</button>
        </div>
    </div>
</template>

<script>
export default {
    name: 'result',
    data() {
        return {

        }
    }
}
</script>

<style lang="scss" scoped>
.result {
    .comInfo {
        background: #fff;
        text-align: center;
        img {
            width: 45px;
            height: 45px;
            margin-top: 30px;
        }
        .suc {
            color: #666;
            font-weight: bold;
            font-size: 18px;
            margin: 15px 0;
        }
        .txt {
            color: #999;
            font-size: 15px;
            padding-bottom: 44px;
        }
    }
    .btn {
        padding: 0 25px;
        margin-top: 60px;
        button {
            width: 100%;
            background-color: #2ea2fa;
	        border-radius: 2px;
            font-size: 15px;
	        color: #ffffff;
            padding: 15px 0;
            border: none;
        }
    }
}
</style>
