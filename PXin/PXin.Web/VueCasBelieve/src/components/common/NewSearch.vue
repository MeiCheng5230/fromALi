<template>
  <div class='search'>
    <div>
      <div><img src="@/assets/images/addfriend_search@2x.png" alt=""></div>
      <input :placeholder="placeholder" type="text">
    </div>
  </div>
</template>

<script>
  export default {
    props: ['placeholder']
  }
</script>

<style scoped lang='scss'>
  .search {
    width: 100%;
    box-sizing: border-box;
    padding: .3rem;
    background: #f2f2f2;

    &>div {
      height: .68rem;
      width: 100%;
      box-sizing: border-box;
      background: #fff;
      border-radius: 0.1rem;
      display: flex;

      div {
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        padding:0 .3rem;

        img {
          width: .38rem;
          height: .42rem;
        }
      }

      input {
        width: 70%;
        height: 100%;
        box-sizing: border-box;
        padding: 0;
        border: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.28rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
      }
    }
  }
</style>
