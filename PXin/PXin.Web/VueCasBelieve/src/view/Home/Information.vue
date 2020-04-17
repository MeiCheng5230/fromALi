<!-- 用户信息 -->
<template>
  <div class="info">
    <table></table>
    <div class="header">
      <div>
        <img @click='SetBigImg(userInfo.appphoto)' :src="userInfo.appphoto" alt />
      </div>
      <div>
        <p>备注名：{{userInfo.remarks==null?'暂未设置':userInfo.remarks}}</p>
        <p>相信昵称：{{userInfo.nodename}}</p>
      </div>
    </div>
    <!-- 设置备注 -->
    <router-link
      v-show="type==2"
      :to="{path:'/SetRemarks',query:{usercode:userInfo.nodecode,remarks:userInfo.remarks}}"
      tag="div"
      class="setInfo"
    >
      设置备注
      <span></span>
    </router-link>

    <!-- 他的信友圈 -->
    <div @click="xinYouQuan" tag="div" class="syquan">
      <div v-if="type==1">我的信友圈</div>
      <div v-else>他的信友圈</div>
      <div class="syqimgs">
        <img
          v-for="(item,index) of msgList"
          :key="index"
          :src="item.picurl!=null?item.picurl.split(',')[0]:item.picurl!=null"
          v-show="item.picurl!=null"
          alt
        />
      </div>
      <div></div>
    </div>

    <!-- 添加朋友 -->
    <div v-if="type==1" class="add">
      <!-- <button @click="$router.push('/Chats')">发消息</button> -->
    </div>
    <div v-else-if="type==2" class="add">
      <button @click="$router.push({path:'/Chats',query:{type:1,userId: userInfo.nodeid}})">发消息</button>
    </div>
    <div v-else-if="type==3" class="add">
      <button @click="$router.push({path:'/AddTest',query:{userCode:userInfo.nodecode}})">添加为好友</button>
    </div>

    <!-- 放大头像 -->
    <van-image-preview
      v-model="show"
      :images="bugImg"
      :showIndex='false'
    >
    </van-image-preview>
  </div>
</template>

<script>
import { ImagePreview } from 'vant';
import { GetMsg, QueryUserInfo } from "@/api/getChatData";
export default {
  data() {
    return {
      //show
      show:false,
      //点击图片方法
      bugImg:[],
      msgList: [],
      userInfo: {},
      type: 1 //1=自己 ，2=好友  ，3=不是好友
    };
  },
  methods: {
    //图片放大
    SetBigImg(src){
      this.show = true ;
      this.bugImg[0]=src ; 
    },
    xinYouQuan() {
      if (this.type == 3) {
        this.$toast("请添加好友再查看该用户信友圈!");
        return;
      }
      this.$router.push({
        path: "/SomeonesFriends",
        query: { snodeid: this.userInfo.nodeid }
      });
    }
  },
  created() {
    let userId = this.$route.query.userId;
    let userParam = JSON.parse(sessionStorage.userParam);
    let data = {
      ...userParam,
      pageindex: 1,
      pagesize: 20,
      snodeid: userId
    };
    GetMsg(data, res => {
      if (res.result > 0) {
        this.msgList = res.data.messages;
        QueryUserInfo(
          { ...JSON.parse(sessionStorage.userParam), userids: userId },
          res => {
            if (res.result > 0) {
              if (userId == userParam.nodeid) {
                //自己
                this.type = 1;
              } else {
                //好友
                this.type = 2;
              }
              this.userInfo = res.data[0];
            } else {
              this.$toast(res.message);
            }
          }
        );
      } else {
        if (res.message == "请添加好友再查看该用户信友圈") {
          //不是好友
          this.type = 3;
          QueryUserInfo(
            { ...JSON.parse(sessionStorage.userParam), userids: userId },
            res => {
              if (res.result > 0) {
                this.userInfo = res.data[0];
              } else {
                this.$toast(res.message);
              }
            }
          );
        }
      }
    });
  }
};
</script>

<style scoped lang='scss'>
.syqimgs {
  flex: 1;
  img{
    margin-left:.2rem;
  }
}
.add {
  margin-top: 1.23rem;
  padding: 0 0.3rem;
  display: flex;

  button {
    width: 100%;
    height: 0.88rem;
    line-height: 0.88rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
  }
}

.syquan {
  width: 100%;
  height: 1.68rem;
  background: #fff;
  margin-top: 0.4rem;
  padding: 0 0.3rem;
  box-sizing: border-box;
  display: flex;
  align-items: center;
  justify-content: space-between;

  div {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    line-height: 1.2rem;
    height: 1.2rem;
    overflow: hidden;

    img {
      width: 1.2rem;
      height: 1.2rem;
      border-radius: 0.04rem;
    }

    img:nth-of-type(n + 2) {
      margin-left: 0.3rem;
    }
  }

  div:last-child {
    width: 0.2rem;
    height: 0.2rem;
    border-top: 0.04rem solid #d2d2d2;
    border-right: 0.04rem solid #d2d2d2;
    transform: rotate(45deg);
  }
}

.setInfo {
  background: #fff;
  display: flex;
  align-items: center;
  width: 100%;
  height: 0.88rem;
  margin-top: 0.4rem;
  padding: 0 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  box-sizing: border-box;
  justify-content: space-between;

  span {
    width: 0.2rem;
    height: 0.2rem;
    border-top: 0.04rem solid #d2d2d2;
    border-right: 0.04rem solid #d2d2d2;
    transform: rotate(45deg);
  }
}

.info {
  height: 100%;
  background: #f2f2f2;

  .header {
    margin-top: 0.3rem;
    width: 100%;
    height: 1.53rem;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    background: #fff;
    padding: 0 0.3rem;

    & > div:nth-child(1) {
      width: 1.2rem;
      height: 1.2rem;

      img {
        width: 100%;
        height: 100%;
        border-radius: 0.04rem;
      }
    }

    & > div:nth-child(2) {
      padding: 0.1rem 0.3rem;

      p:nth-child(1) {
        color: #333333;
        padding-bottom: 0.1rem;
      }

      p {
        margin: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #999999;
      }
    }
  }
}
</style>
