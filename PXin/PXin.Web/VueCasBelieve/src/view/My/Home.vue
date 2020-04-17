<!-- 我的 -->
<template>
  <div class="my">
    <div
      class="title"
      :style="{backgroundImage:'url('+require('@/assets/images/personal_bg(1).png')+')'}"
    >
      <div>
        <div>
          <img @click="$router.push('/PersonalInformation')" :src="user.pic" alt />
        </div>
        <div>{{user.nodename}}</div>
      </div>
    </div>
    <!--选项卡-->
    <div class="content">
      <div class="slider">
        <div class="slider-group">
          <div
            v-for="(item,index) of homeMenu"
            v-show="item.isDisplay"
            :key="index"
            class="slider-info"
            @click="Goto(item.url)"
          >
            <p>
              <img :src="item.logo" alt />
            </p>
            <p>{{item.chName}}</p>
          </div>
          <div class="slider-group"></div>
        </div>
      </div>
      <!-- 菜单 -->
      <div class="info">
        <div
          v-for="(item,index) of myMenu"
          :key="index"
          v-show="item.isDisplay"
          @click="MyMenuOnClick(item)"
        >
          <img :src="item.logo" alt />
          <span>{{item.chName}}</span>
          <!-- <span>{{$t('m.'+item.funName)}}</span> -->
        </div>
      </div>
    </div>
    <!-- myfooter -->
    <myfooter></myfooter>
  </div>
</template>

<script>
import myfooter from "@/components/common/footer";
import { GetUserInfo } from "@/api/myData.js";
import { GetConfig, GoWithParam } from "@/api/sysRequest.js";
import { resolve } from "url";
import { Base64 } from "@/config/utils.js";
export default {
  data() {
    return {
      user: {},
      homeMenu: [],
      myMenu: []
    };
  },
  methods: {
    routeMap: function() {
      let obj = {};
      obj["personal_wallet"] = { routeName: "Wallet" };
      obj["personal_erweima"] = { routeName: "" };
      obj["personal_rate"] = { routeName: "" };
      obj["personal_phone"] = {
        routeName: "Contact",
        query: { mobileno: this.user.mobileno }
      };
      obj["personal_about"] = { routeName: "About" };
      return obj;
    },
    MyMenuOnClick: function(item) {
      let routeMap = this.routeMap();
      if (item.funName in routeMap) {
        this.$router.push({
          name: routeMap[item.funName].routeName,
          query: routeMap[item.funName].query
        });
        return;
      }
      this.Goto(item.url);
    },
    Goto: function(url) {
      if (!url || url == "") {
        return;
      }
      GoWithParam(url);
    }
  },
  created() {
    GetUserInfo(null, data => {
      if (data.result < 0) {
        this.$toast("获取用户数据失败");
        return;
      }
      this.user = data.data;
    });
    GetConfig(null, data => {
      if (data.result < 0 || !data.data) {
        this.$toast("数据加载失败");
        return;
      }
      this.homeMenu = JSON.parse(Base64.decode(data.data.home_menu));
      this.myMenu = JSON.parse(Base64.decode(data.data.my_menu));
    });
  },
  components: {
    myfooter
  }
};
</script>

<style scoped lang='scss'>
.my {
  height: 100%;
  overflow-y: scroll;
}
.slider-group::-webkit-scrollbar {
  display: none !important;
}
.info {
  padding: 0 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  // height: 4.3rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  margin: 0.77rem 0;

  & > div {
    display: flex;
    align-items: center;
    height: 0.8rem;
  }

  img {
    margin-right: 0.25rem;

    width: 0.44rem;
    height: 0.4rem;
    vertical-align: middle;
  }
}

.slider-group {
  overflow-x: scroll;
  display: flex;
  padding-right: 0.5rem;
  p {
    margin: 0;
  }

  .slider-info {
    margin-left: 0.7rem;
    width: 1.4rem;
    display: flex;
    flex-direction: column;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    text-align: center;
    white-space: nowrap;

    img {
      width: 0.73rem;
      height: 0.73rem;
    }
  }

  .slider-info:nth-child(1) {
    margin-left: 0.5rem;
  }

  .slider-info:last-child {
    margin-left: 0;
    visibility: hidden;

    p {
      width: 0.5rem;
    }
  }
}

.slider {
  width: 100%;
  height: 1.8rem;
  padding: 0.3rem 0;
  box-sizing: border-box;
  background-color: #ffffff;
  box-shadow: 0rem 0.01rem 0.18rem 0rem rgba(64, 173, 255, 0.35);
  border-radius: 0.1rem;
}

.content {
  padding: 0 0.3rem;
  position: relative;
  top: -0.3rem;
}

.title {
  height: 3.5rem;
  background-size: 100% 100%;
  padding: 0 0.5rem;
  box-sizing: border-box;
  padding-top: 0.8rem;
  font-family: MicrosoftYaHei;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #fefefe;

  & > div {
    display: flex;
    align-items: center;
  }

  img {
    margin-right: 0.35rem;
    width: 1.4rem;
    height: 1.4rem;
    border: solid 0.04rem #ffffff;
    border-radius: 50%;
  }
}

/*  /deep/ .van-nav-bar {
    background: rgba(255, 255, 255, 0);

  } */
</style>
