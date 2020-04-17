<template>
    <div class="hasIdentify">
        <div class="tit">认证信息</div>
        <div class="info">
            <div class="ops">
                <div class="lft">证件姓名</div>
                <div class="rgt">{{ info.username || '' }}</div>
            </div>
            <div class="ops">
                <div class="lft">证件类型</div>
                <div class="rgt">身份证</div>
            </div>
            <div class="ops">
                <div class="lft">证件号码</div>
                <div class="rgt">{{ info.idcardno || '' }}</div>
            </div>
            <div class="ops">
                <div class="lft">证件照片</div>
                <div class="rgt">已上传</div>
            </div>
            <div class="ops">
                <div class="lft">证件审核</div>
                <div class="rgt">已通过</div>
            </div>
        </div>
    </div>
</template>

<script>
import { GetUserAuthInfo } from '@/api/getData';
export default {
    data() {
        return {
            info: {}
        }
    },
    created() {
        this.GetUserAuthInfo();
    },
    methods: {
        async GetUserAuthInfo() {
            let result = await GetUserAuthInfo(this.$global.userInfo);
            if(result.result > 0) {
                this.info = result.data;
            } else {
                this.Toast.fail(result.message);
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.hasIdentify {
    min-height: 100%;
    background: #f7f7fc;
    .tit {
        padding: 0.2rem 0.3rem;
        color: #999;
    }
    .info {
        background: #fff;
        .ops {
            display: flex;
            padding: 0.2rem 0.3rem;
            border-bottom: 1px solid #f7f7fc;
            .lft {
                margin-right: 0.5rem;
            }
        }
    }
}
</style>
