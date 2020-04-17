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
    }
    #app {
        height: 100%;
        overflow-y: scroll;
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;

    }
    .van-dialog {
        border-radius: 6px;
        .van-dialog__header{
            font-size: 18px;
            font-weight: 700;
        }
        .van-dialog__message{
            font-size: 16px;
        }
    }

</style>
