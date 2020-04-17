<template>
  <div class="notice">
    <div
      class="item"
      v-for="item in noticeList"
      :key="item.index"
      @click="$router.push({path:'/noticeDetail',query:{infoid:item.infoid}})"
    >
      <div class="title">{{ item.title }}</div>
      <div class="cont">
        <div class="lft">
          <div class="time">
            <img src="@/assets/images/icon__time.png" alt />
            {{ item.starttime }}
          </div>
          <div class="address">
            <img src="@/assets/images/icon_address.png" alt />
            {{ item.address }}
          </div>
        </div>
        <div class="rgt">
          <div :class="'btn'+item.status">{{item.statusdesc}}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { GetMeetInfos } from "@/api/getFbApData";
export default {
  data() {
    return {
      noticeList: []
    };
  },
  async mounted() {
    let res = await GetMeetInfos({
      ...JSON.parse(sessionStorage.userParam),
      typeid: 2
    });
    if (res.result > 0) {
      this.noticeList = res.data;
    }
  }
};
</script>

<style lang="scss" scoped>
.notice {
  min-height: 100%;
  background: #f7f7fc;
  box-sizing: border-box;
  padding: 0.3rem;
  font-size: 0.28rem;
  .item {
    background: #fff;
    margin-bottom: 0.3rem;
    padding: 0.35rem 0.24rem 0.24rem 0.24rem;
    .title {
      margin-bottom: 0.4rem;
      font-weight: bold;
      font-size: 0.3rem;
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-line-clamp: 2; // 控制多行的行数
      -webkit-box-orient: vertical;
    }
    .cont {
      display: flex;
      align-items: center;
      .lft {
        flex: auto;
        color: #666;
        font-size: 0.24rem;
        img {
          width: 0.28rem;
          height: 0.28rem;
          margin-right: 0.13rem;
        }
        .time {
          display: flex;
          margin-bottom: 0.3rem;
        }
        .address {
          display: flex;
        }
      }
      .rgt {
        padding-left: 0.8rem;
        div {
          padding: 0.2rem 0;
          width: 2rem;
          text-align: center;
          white-space: nowrap;
          border-radius: 0.06rem;
          font-size: 0.3rem;
          color: #fff;
          &.btn0 {
            background: #2ea2fa;
          }
          &.btn1 {
            background: #96d0fc;
          }
          &.btn-1 {
            background: #e70c0c;
            opacity: 0.3;
          }
        }
      }
    }
  }
}
</style>