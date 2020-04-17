<template>
    <div id="app">
        <router-view/>
    </div>
</template>

<style lang="scss">
    html,body{
        height: 100%;
        padding: 0;
        margin: 0;
        background-color: #f7f7fc;
        color: #333;
    }
    #app {
        height: 100%;
        overflow-y: scroll;
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        font-size: 14px;
    }
</style>
