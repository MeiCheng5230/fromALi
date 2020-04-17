<!-- 发现首页 -->
<template>
  <div class="find">
    <!-- 头 -->
    <table></table>
    <!-- 信友圈 -->
    <router-link to="/Friends" tag="div" class="friends">
      <div>
        <img src="@/assets/images/discover_dynamic@2x.png" alt />
        <span>{{$t('m.busin')}}</span>
      </div>
      <div>
        <div v-if="NewMessageUser" class="state">
          <img :src="NewMessageUser.appphoto" alt />
          <span></span>
        </div>
        <div class="arrows"></div>
      </div>
    </router-link>

    <!-- 摇一摇， 附近的人 -->
    <router-link to="/FindRock" tag="div" class="friends" style="margin-bottom: 0;">
      <div>
        <img src="@/assets/images/discover_shake@2x.png" alt />
        <span>{{$t('m.shake')}}</span>
      </div>
      <div class="arrows"></div>
    </router-link>
    <!-- 摇一摇， 附近的人 -->
    <router-link
      to="/NearbyFriends"
      tag="div"
      class="friends"
      style="margin-top: 0; border-top:.01rem solid #F9F9F9;"
    >
      <div>
        <img src="@/assets/images/discover_nearpeople@2x.png" alt />
        <span>{{$t('m.people')}}</span>
      </div>
      <div class="arrows"></div>
    </router-link>
    <!-- myfooter -->
    <myfooter :hasNewMessage="hasNewMessage"></myfooter>
  </div>
</template>

<script>
import myfooter from "@/components/common/footer";
import { IsNewMessage, QueryUserInfo } from "@/api/findData.js";
export default {
  data() {
    return {
      hasNewMessage: false,
      users: []
    };
  },
  computed: {
    NewMessageUser: function() {
      if (this.users.length > 0) {
        return this.users[0];
      } else {
        return null;
      }
    }
  },
  created() {
    IsNewMessage(null, data => {
      if (data.result > 0) {
        QueryUserInfo({ userids: data.data }, data => {
          this.users = data.data;
        });
        this.hasNewMessage = true;
      } else {
        this.hasNewMessage = false;
      }
      this.hasNewMessage = true;
    });
  },
  components: {
    myfooter
  }
};
</script>

<style scoped lang='scss'>
.state {
  position: relative;
  margin-right: 0.1rem;
  span {
    top: 75%;
  }
}
.find {
  height: 100%;
  background: #f2f2f2;

  .arrows {
    width: 0.2rem;
    height: 0.2rem !important;
    border-top: 0.03rem solid #ccc;
    border-right: 0.03rem solid #ccc;
    transform: rotate(45deg);
  }

  .friends {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    padding: 0 0.36rem 0 0.3rem;
    box-sizing: border-box;
    height: 0.88rem;
    margin: 0.32rem 0 0.5rem 0;
    background: #fff;
    div {
      height: 100%;
      display: flex;
      align-items: center;
      img {
        width: 0.48rem;
        height: 0.48rem;
      }
      span {
        padding-left: 0.67rem;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
      }
    }

    & > div:nth-child(2) {
      justify-items: flex-end;
      position: relative;
      height: auto;
      img {
        width: 0.6rem;
        height: 0.6rem;
        border-radius: 50%;
      }
      span {
        padding: 0;
        right: 0rem;
        top: 0rem;
        border-radius: 50%;
        position: absolute;
        background-color: #ff2a2a;
        width: 0.1rem;
        height: 0.1rem;
      }
    }
  }
}
</style>
