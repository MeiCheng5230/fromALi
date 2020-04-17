<template>
    <div class="Header" v-if="HeaderShow">
        <header>
            <div class="back" @click="backFn"></div>
            <div class="title">{{ title }}</div>
        </header>
    </div>
</template>

<script>
export default {
    props: ["title"],
    data() {
        return {
            yougu: undefined,
            HeaderShow: false,
        }
    },
    methods: {
        backFn() {
            this.$router.back(1);
        }
    },
    created() {
      if (!this.yougu) return;
      //对应组件 头部文字样式 颜色
      if (['我的会员码', '我的兑换码', '新增充值商'].some(item => {
        return this.title == item;
      })) {
        this.title2n = 1;
      }
      ;
      if (this.title == '充值商') {
        this.font2 = 1;
      }
      ;
      if (['我的经销商', '购买会员码', '经销商兑换码', '进货记录', '新增代理人'].some(item => {
        return this.title == item;
      })) {
        this.title1n = 1;
      }
      ;
      if (['公司资料认证', '认证资料', '个人资料认证', '拍卖详情', 'BTS管理系统'].some(item => {
        return this.title == item;
      })) {
        this.auctionDetails = true;
      }
    }
}
</script>

<style lang="scss" scoped>
.Header {
    height: 1rem;
    header {
        height: 1rem;
        position: fixed;
        top: 0;
        width: 100%;
        background: #2ea2fa;
        color: #fff;
        z-index: 9;
        .back {
            width: 0.2rem;
            height: 0.2rem;
            border-bottom: 2px solid #fff;
            border-left: 2px solid #fff;
            transform: rotate(45deg) translateY(-50%);
            right: 0px;
            position: absolute;
            top: 50%;
            left: 0.2rem;
        }
        .title {
            text-align: center;
            line-height: 1rem;
            font-size: 0.34rem;
        }
    }
}
</style>
