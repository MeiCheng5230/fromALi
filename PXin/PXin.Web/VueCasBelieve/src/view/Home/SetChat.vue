<!-- 聊天设置 -->
<template>
  <div class="setchat">
    <div class="content">
      <div @click="to">
        <img
          :src="targetUser.appphoto?targetUser.appphoto:require('@/assets/images/personal_head_no@2x.png')"
          alt
        />
      </div>
      <div @click="$router.push('/NewGroupChat')">
        <img src="@/assets/images/groupchat_detail_add@2x.png" alt />
      </div>
    </div>

    <!-- 设置备注 -->
    <router-link to="/SetNotes" tag="div" class="unify">
      设置备注
      <span></span>
    </router-link>
    <!-- 置顶聊天 -->
    <div class="unify">
      置顶聊天
      <img
        @click="isZd=!isZd"
        :src="isZd?require('@/assets/images/setting_btn_sel@2x.png'):require('@/assets/images/setting_btn_nor@2x.png')"
        alt
      />
    </div>
    <!-- 清空聊天记录 -->
    <div class="unify" @click="deleteChat">清空聊天记录</div>

    <!-- 删除好友 -->
    <div class="delete">
      <button @click="show=true">删除好友</button>
    </div>

    <!-- 删除好友提示框 -->
    <van-popup v-model="show" position="bottom">
      <div class="deleteMod">
        <p>将联系人“{{this.$route.query.chatname}}”删除之后，与其的聊天记录将同时删除</p>
        <p @click="deleteFriend">删除联系人</p>
        <p @click="show=false">取消</p>
      </div>
    </van-popup>
  </div>
</template>

<script>
import { DeleteFriend } from "@/api/getChatData";
import { getMyFriendByUserId } from "@/api/localStorageData";
import { Dialog } from "vant";
export default {
  data() {
    return {
      isZd: true, //是否置顶
      //删除好友提示框
      show: false,
      targetUser: {} //好友
    };
  },
  components: {
    [Dialog.Component.name]: Dialog.Component
  },
  mounted() {
    let targetId = this.$route.query.targetId;
    getMyFriendByUserId(
      { ...JSON.parse(sessionStorage.userParam), userId: targetId },
      res => {
        if (res) {
          this.targetUser = res;
        }
      }
    );
  },
  methods: {
    //点击用户头像
    to() {
      //是否是好友 传参?
      this.$router.push("/Information");
    },
    //删除聊天记录
    deleteChat() {
      Dialog.confirm({
        title: "清空聊天记录",
        message: "是否清空聊天记录?"
      })
        .then(() => {
          // 确定
          //Ajax
          this.$toast("清除聊天记录成功");
        })
        .catch(() => {
          // 取消
        });
    },
    deleteFriend() {
      let _this = this;
      DeleteFriend(
        {
          ...JSON.parse(sessionStorage.userParam),
          usercode: _this.targetUser.nodecode
        },
        res => {
          console.log("删除好友结果 ：" + JSON.stringify(res));
          if (res.result > 0) {
            let friendList = localStorage.getItem("MyFriendList");
            if (friendList) {
              let friendListObj = JSON.parse(friendList);
              let newFriendList = friendListObj.filter(function(friend) {
                if (friend.nodecode != _this.targetUser.nodecode) {
                  return friend;
                }
              });
              localStorage.setItem(
                "MyFriendList",
                JSON.stringify(newFriendList)
              );
              _this.$router.replace("/AddressBookHome");
            }
          } else {
            _this.$toast("删除失败!");
          }
        }
      );
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

.delete {
  width: 100%;
  margin-top: 1.2rem;
  box-sizing: border-box;
  padding: 0 0.3rem;

  button {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    height: 0.88rem;
    background-color: #ff1541;
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

.unify {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 0.6rem;
  height: 0.88rem;
  background: #fff;
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;

  img {
    width: 0.7rem;
    height: 0.4rem;
  }

  span {
    width: 0.2rem;
    height: 0.2rem;
    border-top: 0.04rem solid #d2d2d2;
    border-right: 0.04rem solid #d2d2d2;
    transform: rotate(45deg);
  }
}

.content {
  display: flex;
  flex-wrap: wrap;
  width: 100%;
  box-sizing: border-box;
  background: #fff;
  padding: 0.44rem 0.3rem;
  padding-left: 0;
  padding-top: 0;

  & > div {
    margin-top: 0.44rem;
    margin-left: 0.3rem;
    display: flex;
    width: 0.8rem;
    height: 0.8rem;

    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
    }
  }
}

.setchat {
  height: 100%;
  background: #f2f2f2;
}
</style>
