<!-- 群聊 -->
<template>
  <div class="groupChat">
    <!-- 左右滑块 -->
    <div class="fixed">
      <van-tabs
        v-model="active"
        color="#2ea2fa"
        line-height="0.02rem"
        title-active-color="#2ea2fa"
        title-inactive-color="#333"
      >
        <van-tab :title="$t('m.groupcreated')">
          <div class="chatInfoContent">
            <div v-for="(item,index) of createdGroup" :key="index" style="padding-left: .2rem;" @click="$router.push('/Chats')">
              <div class="chatInfo" >
                <div>
                  <img :src="item.grouppicfull" alt />
                </div>
                <div>{{item.groupname}}</div>
              </div>
            </div>
          </div>
        </van-tab>
        <van-tab :title="$t('m.groupjoin')">
          <div class="chatInfoContent">
            <div  v-for="(item,index) of joinGroup" :key="index" style="padding-left: .2rem;">
              <div class="chatInfo">
                <div>
                  <img :src="item.grouppicfull" alt />
                </div>
                <div>{{item.groupname}}</div>
              </div>
            </div>
          </div>
        </van-tab>
      </van-tabs>
    </div>
    <!-- footer 创建群聊按钮 -->
    <div class="AddGroupChatBtn">
      <button @click="$router.push('/AddChat')">{{$t('m.creategroup')}}</button>
    </div>
  </div>
</template>

<script>
import { MyGroup } from "@/api/getChatData";

export default {
  data() {
    return {
      active: 0,
      createdGroup:[],
      joinGroup:[]
    };
  },
  mounted() {
    //固定在顶部
    let fixed = document.getElementsByClassName("van-tabs__wrap")[0];
    fixed.style.position = "fixed";

    //列表高度
    document.getElementsByClassName('van-tabs__content')[0].style.height=document.body.offsetHeight - getComputedStyle(window.document.documentElement)['font-size'].slice(0,-2)*2.24 +'px'
  },
  created(){
    let param={...JSON.parse(sessionStorage.userParam)};
    MyGroup(param, res => {
        if (res.result > 0) {
            let data=res.data;
            data.forEach(ele => {
              if(ele.creater == param.nodeid){
                this.createdGroup.push(ele);
              }else{
                this.joinGroup.push(ele);
              }
            });
        } else {
             this.$toast(res.message);
        }
    });
    
  }
};
</script>

<style scoped lang='scss'>
.groupChat {
  height: 100%;
  
  // .fixed {
  //   /* position: fixed; */
  // }

  /deep/ .right {
    display: none;
  }

  /deep/ .van-nav-bar__title {
    font-size: 0.34rem !important;
  }

  .van-hairline--top-bottom::after {
    border: 0;
  }
  /deep/ .van-tabs__content{
    background: #f4f4f4;
    padding-bottom: 1.5rem;
    overflow-y: scroll;
    -webkit-overflow-scrolling: touch;
  }
  /deep/ .van-tab{
    display: flex;
    justify-content: center;

  }
  .van-tabs {
    padding-top: .74rem;
  }

  /deep/ .van-tabs__wrap {
    height: .74rem;
    border-bottom: .02rem solid  #f2f2f2;
  }

  .chatInfo {
    display: flex;
    height: 1.68rem;
    align-items: center;
    border-bottom: 0.01rem solid #d1d1d1;

    div:nth-child(1) {
      width: 1rem;
      height: 1rem;

      img {
        width: 100%;
        height: 100%;
        border-radius: 50%;
      }
    }

    div:nth-child(2) {
      padding-left: 0.3rem;
      width: 4.96rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;
      overflow: hidden;
      white-space: nowrap;
      text-overflow: ellipsis;
    }
  }
  // .chatInfoContent {
  //   // padding-bottom: 1.5rem;
  // }
    .chatInfoContent>div{
      background:#fff;
    }
  .chatInfoContent > div:last-child .chatInfo {
    border-bottom: 0;
  }

  .AddGroupChatBtn {
    position: fixed;
    width: 100%;
    box-sizing: border-box;
    padding: 0.3rem;
    bottom: 0;
    height: 1.5rem;
    display: flex;
    background: #fff;
    button {
      display: flex;
      justify-content: center;
      align-items: center;
      padding: 0;
      width: 100%;
      height: 100%;
      background-color: #2ea2fa;
      border-radius: 0.04rem;
      border: 0;
      font-family: PingFang-SC-Bold;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
    }
  }
}
</style>
