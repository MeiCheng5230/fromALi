<template>
    <div class="result" >
        <div class="content">
            <img src="@/assets/images/pay_success.png" alt="">
            <div>{{typeonename}}成功</div>
            <span>{{typetwoname}}</span>
        </div>
    </div>
</template>

<script>
export default {
    data(){
       return{
         typeonename:'',
         typetwoname:'',
       }
    },
    mounted(){
        this.typeonename=this.$route.query.typeonename;
        this.typetwoname=this.$route.query.typetwoname;
        let _this=this;
        setTimeout(() => {
            _this.$router.go(-2);
        }, 1000);
   }
}
</script>

<style lang="scss" scoped>
.result {
    min-height: 100%;
    background: #f2f2f2;
    .content {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 0.7rem 0;
        background: #fff;
        img {
            width: 0.9rem;
            height: 0.9rem;
        }
        div {
            padding: 0.3rem;
            font-size: 0.36rem;
            color: #666;
        }
        span {
            padding-bottom: 0.3rem;
            color: #999;
        }
    }
    .btnBx {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding-top: 0.6rem;
        button {
            width: 6.5rem;
            height: 0.88rem;
            border: none;
            border-radius: 0.04rem;
            margin-bottom: 0.3rem;
            &.bt1 {
                background: #2ea2fa;
                color: #fff;
            }
            &.bt2 {
                background: #f2f2f2;
                border: 1px solid #2ea2fa;
                color: #2ea2fa;
            }
        }
    }
}
</style>
