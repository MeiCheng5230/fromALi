<!-- 附近的人 -->
<template>
  <div class="NearbyFriends">
    <!-- 头 -->
    <!-- 附近的人 -->
    <div class="container">
      <div class="item" v-for="(item,index) of NearbyFriendsDataList" :key="index">
        <div @click="GoUserInformation(item)">
          <div>
            <img :src="item.photo" alt />
          </div>
          <div>
            <span>{{item.nickname}}</span>
            <span>{{DistanceShow(item.distance)}}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { GetNearby } from "@/api/findData.js";
export default {
  data() {
    return {
      NearbyFriendsDataList: []
    };
  },
  methods: {
    NativeLocationCompletion: function(latitude, longitude) {
      if (
        (latitude === "0" && longitude === "0") ||
        (latitude === "4.9E-324" && longitude === "4.9E-324")
      ) {
        this.$toast("获取当前位置失败，请打开GPS");
      } else {
        this.GetNearby(latitude, longitude);
      }
    },
    GetNearby: function(latitude, longitude) {
      GetNearby({ latitude: latitude, longitude: longitude }, data => {
        if (data.result < 0) {
          this.$toast("数据加载失败");
          setTimeout(() => {
            this.$router.go(-1);
          }, 500);
        }
        this.NearbyFriendsDataList = data.data;
      });
    },
    GoUserInformation: function(nearUser) {
      this.$router.push({
        name: "Information",
        query: { userid: nearUser.mpdeod }
      });
    },
    DistanceShow: function(distance) {
      if (distance > 500) {
        distance = distance / 1000;
        distance = (Math.ceil(distance * 100) / 100).toFixed(2);
        return distance + "km";
      } else {
        distance = (Math.ceil(distance * 100) / 100).toFixed(2);
        if (distance <= 100) {
          return "100m以内";
        } else {
          return distance + "m";
        }
      }
    }
  },
  mounted() {
    window["nativeLocationCompletion"] = (latitude, longitude) => {
      this.NativeLocationCompletion(latitude, longitude);
    };
  },
  created() {
    try {
      PCNWebInteration.jsTunedupNativeWithTypeParamSign(1006, "", "");
    } catch (e) {
      getDefaultMap(".siteCity", "#siteVal", ".site");
    }
  }
};
</script>

<style scoped lang='scss'>
.NearbyFriends {
  .item {
    height: 1.48rem;
    padding-left: 0.3rem;

    & > div {
      display: flex;
      height: 100%;
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

      div:last-child {
        height: 1rem;
        box-sizing: border-box;
        padding: 0.05rem 0;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding-left: 0.2rem;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #666666;

        span:last-child {
          font-size: 0.24rem;
          color: #999;
        }
      }
    }
  }
}
</style>
