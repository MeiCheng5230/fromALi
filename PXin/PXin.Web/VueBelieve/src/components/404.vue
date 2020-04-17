<template>
    <div class="content">
        <vheader title="页面异常"/>
        <div class="bgImg">
            <img :src="showObj.img" onerror="this.src='images/1.png'" alt=""/>
        </div>
        <div class="text">{{ showObj.txt }}</div>
    </div>
</template>

<script>
const vheader = () => import("@/components/Header")
export default {
    data() {
        return {
            state: [{
                img: require("@/assets/images/1.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/2.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/3.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/4.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/5.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/6.png"),
                txt: '页面错误，努力完善中'
            },{
                img: require("@/assets/images/7.png"),
                txt: '页面错误，努力完善中'
            }],
            showObj: {}
        }
    },
    created() {
        let idx = this.$route.params.id;
        this.showObj = this.state[idx]
    },
    components: {
        vheader
    }
}
</script>

<style lang="scss" scoped>
.content {
    .bgImg {
        text-align: center;
        padding-top: 2rem;
        img {
            width: 6rem;
            height: auto;
        }
    }
    .text {
        text-align: center;
        margin-top: 1rem;
        color: #666;
    }
}
</style>
