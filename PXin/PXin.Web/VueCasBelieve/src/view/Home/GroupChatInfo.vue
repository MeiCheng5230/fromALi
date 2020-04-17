<!-- 群聊信息 -->
<template>
  <div class="chatinfo">
    <!-- 点击跟换群头像 -->
    <div class="uploadHead">
      <div>
        <van-uploader :after-read="afterRead">
          <img :src="uploadImg" alt />
        </van-uploader>
        <span>点击可以更换/上传群头像</span>
      </div>
    </div>
    <!-- 群成员 -->
    <div class="groupList">
      <p>群成员（36人）</p>
      <div class="infoList">
        <!-- v-for = (item,index) of data.slice(0,16) 前16-->
        <div v-for="item of 16" :key="item">
          <img src="@/assets/images/invite_weichat@2x.png" alt />
        </div>

        <!-- v-for = (item,index) of data.slice(16) 后16-->
        <!-- <div v-show='?????????' v-for="item of 30" :key="item">
          <img src="@/assets/images/invite_weichat@2x.png" alt />
        </div>-->

        <div @click="$router.push('/NewGroupChat')">
          <img src="@/assets/images/groupchat_detail_add@2x.png" alt />
        </div>
        <div @click="$router.push('/NewGroupChat')">
          <img src="@/assets/images/groupchat_detail_delete@2x.png" alt />
        </div>
      </div>
      <!-- v-show='大于16 显示 ' -->
      <p class="more">
        查看更多群成员
        <span></span>
      </p>
    </div>

    <!-- 群聊名称 -->
    <div class="chatname">
      群聊名称
      <span></span>
    </div>
    <!-- 群聊描述 -->
    <div class="chatdesc">
      <p>群聊描述</p>
      <div class="desc">
        <div>描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行描述文字最多三行</div>
        <div></div>
      </div>
    </div>
    <!-- 新消息通知 -->
    <div style="margin-top: .2rem;" class="infos">
      <span>新消息通知</span>
      <img
        @click="isNotice=!isNotice"
        :src="isNotice?require('@/assets/images/setting_btn_sel@2x.png'):require('@/assets/images/setting_btn_nor@2x.png')"
        alt
      />
    </div>
    <!-- 置顶聊天 -->
    <div class="infos">
      <span>聊天置顶</span>
      <img
        @click="isZd=!isZd"
        :src="isZd?require('@/assets/images/setting_btn_sel@2x.png'):require('@/assets/images/setting_btn_nor@2x.png')"
        alt
      />
    </div>

    <!-- 发起对话、解散并退出 -->
    <div class="btn">
      <button @click="$router.push('/chats')">发起对话</button>
      <button @click="show = true">解散并退出</button>
    </div>

    <!-- 删除好友提示框 -->
    <van-popup v-model="show" position="bottom">
      <div class="deleteMod">
        <p>退出后不会通知群聊中的其他成员，且不会再接收此群聊消息</p>
        <p>确定</p>
        <p @click="show=false">取消</p>
      </div>
    </van-popup>
  </div>
</template>

<script>
export default {
  data() {
    return {
      //是否通知
      isNotice: true,
      //是否置顶
      isZd: false,
      //上传默认头像
      uploadImg: require("@/assets/images/groupchat_head@2x.png"),
      //
      show: false
    };
  },
  methods: {
    //上传头像
    afterRead(file) {
      this.uploadImg = file.content;
    }
  }
};
</script>

<style scoped lang='scss'>
.deleteMod {
  /* height: 3.5rem; */
  background: #f0f0f0;

  p {
    background: #fff;
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 0;
    padding: 0 0.36rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
  }

  p:nth-child(1) {
    border-bottom: 1px solid #d1d1d1;
    height: 1.35rem;
  }

  p:nth-child(2) {
    height: 1.08rem;
    color: #ff1541;
  }

  p:nth-child(3) {
    margin-top: 0.12rem;
    height: 0.98rem;
    color: #333333;
  }
}

.chatinfo {
  height: 100%;
  background-color: #f2f2f2;

  .btn {
    width: 100%;
    padding: 0 0.3rem;
    box-sizing: border-box;
    display: flex;
    flex-direction: column;
    padding-top: 1.25rem;
    padding-bottom: 0.6rem;
    background-color: #f2f2f2;

    button {
      width: 100%;
      height: 0.88rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      padding: 0;
      margin: 0;
      border: 0;
      background-color: #2ea2fa;
      border-radius: 0.1rem;
    }

    button:nth-child(2) {
      margin-top: 0.3rem;
      border: solid 0.02rem #ff1541;
      background: #fff;
      color: #ff1541;
    }
  }

  .infos {
    height: 0.93rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 0.3rem;
    background: #fff;
    margin-top: 0.04rem;

    img {
      width: 0.7rem;
      height: 0.4rem;
    }

    span {
      font-family: PingFang-SC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;
    }
  }

  .desc {
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;

    & > div:nth-child(1) {
      width: 85%;
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      max-height: 1.26rem;
      line-height: 0.42rem;
    }

    & > div:nth-child(2) {
      width: 0.2rem;
      height: 0.2rem;
      border-top: 0.04rem solid #999999;
      border-right: 0.04rem solid #999999;
      transform: rotate(45deg);
    }
  }

  .chatdesc {
    background: #fff;
    margin-top: 0.04rem;
    padding: 0 0.3rem;
    padding-bottom: 0.2rem;

    p {
      height: 0.88rem;
      line-height: 0.88rem;
      margin: 0;
      font-family: PingFang-SC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;
    }
  }

  .chatname {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 0.3rem;
    background: #fff;
    margin-top: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    height: 0.88rem;

    span {
      display: inline-block;
      width: 0.2rem;
      height: 0.2rem;
      border-top: 0.04rem solid #999999;
      border-right: 0.04rem solid #999999;
      transform: rotate(45deg);
    }
  }

  .more {
    text-align: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #cecece !important;
    display: flex;
    align-items: center;
    justify-content: center;

    span {
      display: inline-block;
      width: 0.15rem;
      height: 0.15rem;
      border-top: 0.02rem solid #999999;
      border-right: 0.02rem solid #999999;
      transform: rotate(45deg);
    }
  }

  .infoList {
    display: flex;
    flex-wrap: wrap;

    & > div {
      display: flex;

      margin-left: 0.42rem;

      width: 0.8rem;
      height: 0.8rem;

      img {
        width: 100%;
        height: 100%;
        border-radius: 50%;
      }
    }

    & > div:nth-child(1) {
      margin-left: 0;
    }

    & > div:nth-child(6n + 7) {
      margin-left: 0;
    }

    & > div:nth-of-type(n + 7) {
      margin-top: 0.25rem;
    }
  }

  .groupList {
    margin-top: 0.2rem;
    background: #fff;
    padding: 0 0.3rem;

    p {
      line-height: 0.88rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #666666;
      height: 0.88rem;
      margin: 0;
    }
  }

  .uploadHead {
    height: 2.58rem;
    display: flex;
    justify-content: center;
    background: #fff;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;

    /deep/ .van-uploader__wrapper {
      display: flex;
      justify-content: center;

      img {
        width: 1rem;
        height: 1rem;
        border-radius: 0.04rem;
      }
    }

    & > div {
      padding-top: 0.7rem;
      display: flex;
      flex-direction: column;

      span {
        margin-top: 0.1rem;
      }
    }
  }
}
</style>
