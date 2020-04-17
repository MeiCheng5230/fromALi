<template>
  <div class="no-list">
    <img src="../assets/images/nodata.png" alt="" height="222">
    <p>{{text}}</p>
  </div>
</template>

<script>
    export default {
        name: "listNoData",
        props: {
            text: {
                type: String,
                required: true,
            }
        }
    }
</script>

<style scoped lang="scss">
  .no-list {
    width: 100%;
    text-align: center;

    img {
      width: 100%;
    }

    p {
      color: #999;
    }
  }
</style>
