<!-- 通讯录朋友 -->
<template>
  <div class="AddressBookFriends">
    <vantHeader title="通讯录朋友" :back="true"></vantHeader>
    <search @click.native="$router.push('/AddressSearch')" :placeholder="$t('m.searchphonenumber')"></search>
    <!-- indexBar 索引列表 -->
    <div class="container">
      <van-index-bar :sticky="false" highlight-color="#069">
        <div v-for="(item,index) of contactsList" :key="index">
          <van-index-anchor :index="item.index" class="indexBarTitle" />
          <van-cell v-for="(item,index) of item.arr" :key="index">
            <div class="userInfo">
              <img :src="item.url" alt />
              <span>{{item.name}}</span>
            </div>
            <div>
              <button
                :class="item.status==2 && 'isAdd'"
                @click="addFriend(item)"
              >{{item.status==1?"添加":"已添加"}}</button>
            </div>
          </van-cell>
        </div>
        <!-- 占位 -->
        <div style="height:2rem;background:#f2f2f2"></div>
      </van-index-bar>
    </div>
    
  </div>
</template>

<script>
import vantHeader from "@/components/common/header";
import search from "@/components/common/NewSearch.vue";
export default {
  //data
  data() {
    return {
      //联系人列表
      contactsList: [
        //status 1可以邀请 2 已注册
        {
          index: "A",
          arr: [
            {
              name: "小明小明小明小明小明小明小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明小明小明小明小明小明小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明小明小明小明小明小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "B",
          arr: [
            {
              name: "小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "C",
          arr: [
            {
              name: "小明小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "C",
          arr: [
            {
              name: "小明小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "C",
          arr: [
            {
              name: "小明小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            },
            {
              name: "小明小明小明小明",
              status: 2,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "D",
          arr: [
            {
              name: "小明小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            }
          ]
        },
        {
          index: "E",
          arr: [
            {
              name: "小明小明",
              status: 1,
              url: require("@/assets/images/logo.png")
            }
          ]
        }
      ]
    };
  },
  //props
  //生命周期
  mounted() {
    // 设置右侧A-Z top值
    let vanIndexBarT = document.getElementsByClassName("van-index-bar")[0]
      .offsetTop; //距离顶部
    let vanIndexBarH = document.getElementsByClassName("van-index-bar")[0]
      .offsetHeight; //元素高度
    let sidebarH = document.getElementsByClassName("van-index-bar__sidebar")[0]
      .offsetHeight; //元素高度
    document.getElementsByClassName("van-index-bar__sidebar")[0].style.top =
      vanIndexBarT + vanIndexBarH / 2 + "px";

    //设置滚动区域高度
    document.getElementsByClassName("van-index-bar")[0].style.height =
      document.body.offsetHeight - vanIndexBarT + "px";
  },
  //组件
  components: {
    vantHeader,
    search
  },
  //methods:
  methods: {
    //添加按钮
    addFriend(item) {
      //Ajax 跳转
      if (item.status == 2) return;
      this.$toast("添加成功");
    }
  }
};
</script>

<style scoped lang='scss'>
.AddressBookFriends {
  height: 100%;
  overflow: hidden;
  .container {
    .indexBarTitle {
      background: #f2f2f2;
      height: 0.68rem;

      /deep/ .van-index-anchor {
        line-height: 0.68rem;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
      }
    }

    .van-cell {
      height: 1.09rem;
      padding: 0 15px;

      .van-cell__value {
        display: flex;
        justify-content: space-between;
      }

      .userInfo {
        height: 100%;
        display: flex;
        align-items: center;

        span {
          padding-left: 0.2rem;
          font-family: PingFang-SC-Medium;
          font-size: 0.3rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #333333;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          width: 3rem;
        }

        img {
          border-radius: 50%;

          width: 0.72rem;
          height: 0.72rem;
          background-color: #d1d1d1;
        }
      }

      .userInfo + div {
        padding-right: 0.2rem;
        display: flex;
        align-items: center;

        button {
          border: 0;
          font-family: PingFang-SC-Medium;
          font-size: 0.24rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #ffffff;
          background-color: #2ea2fa;
          border-radius: 0.04rem;
        }
      }

      .isAdd {
        background: #fff !important;
        color: #999 !important;
        padding: 0 !important;
      }
    }

    .van-index-bar {
      height: 11rem;
      overflow: scroll;
      /deep/ .van-index-bar__sidebar {
        padding-right: 0.1rem;
      }
    }
  }

  /deep/ .search {
    background: #fff;

    & > div {
      background: #f2f2f2;
      padding-right: 0.22rem;

      input {
        background: #f2f2f2;
      }
    }
  }
}
</style>
