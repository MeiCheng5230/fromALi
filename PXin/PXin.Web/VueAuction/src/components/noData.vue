<template>
    <div class="nodata">
        <img :src="img?img:require('@/assets/images/nocard@2x.png')" alt=""/>
        <div class="text">{{text?text:'暂无数据'}}</div>
    </div>
</template>

<script>
export default {
    props:['text','img']

}
</script>

<style lang="scss" scoped>
.nodata {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 2rem 0.3rem;
    img {
        width: 6rem;
        height: auto;
    }
    .text {
        padding-top:2rem;
        color: #999;
    }
}
</style>
