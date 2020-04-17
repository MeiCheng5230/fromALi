<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
    export default {
        name: 'App'
    }
</script>

<style>
  body, html,#app {
    margin: 0;
    padding: 0;
    height: 100%;
    font-family: PingFangSC-Regular,sans-serif,'Avenir', Helvetica, Arial;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    background-color: #fff;
    color: #333;
    font-size: 0.3rem;
  }
  h5,p{
    padding: 0;
    margin: 0;
  }

</style>
