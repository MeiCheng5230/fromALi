<template>
        <!-- 搜索 -->
  <div class='search' >
    <span  :style="{backgroundImage:'url('+require('@/assets/images/addfriend_search@2x.png')+')',marginLeft:marginLeft}"></span>
    <input type="text" :placeholder="placeholder" :style='{background:iptBG}' :class='marginLeft && "SpanMarginLeft"'>
  </div>
</template>

<script>
  export default{
    props:['placeholder','marginLeft','iptBG']
  }
</script>

<style scoped lang='scss'>
  .search {
    height: 1.18rem;
    width: 100%;
    box-sizing: border-box;
    padding: .3rem;
    position: relative;
    padding-bottom: .2rem;
    display: flex;
    
    span {
      position: absolute;
      width: .38rem;
      height: .42rem;
      top: .42rem;
      left: .5rem;
      background-size: 100% 100%;
    }
  
    input {
      background-color: #f2f2f2;
      border-radius: 0.1rem;
      border: 0;
      padding: 0;
      margin: 0;
      outline: none;
      flex:1;
      display: flex;
      align-items: center;
      padding-left: .83rem;
      
      font-family: MicrosoftYaHei;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }
    .SpanMarginLeft{
      padding-left: 1.4rem;
    }
  }
</style>
