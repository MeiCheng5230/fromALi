<!-- 短信邀请 -->
<template>
  <div class="AddressBookFriends">
    <vantHeader title="通讯录朋友" :back="true"></vantHeader>
    <!-- 短信邀请好友 -->
    <div class="fixed">
      <div>
        <div></div>
        <div>短信邀请好友</div>
        <div @click="MultipleChoice">{{MultipleText}}</div>
      </div>
    </div>
    <!-- fixed占高 -->
    <div style="height: .6rem;"></div>
    <!-- 提示 通过短信邀请所产生的资费由运营商收取 -->
    <div class="hint">通过短信邀请所产生的资费由运营商收取</div>
    <!-- 搜索 -->
    <search @click.native="$router.push('/AddressSearch')" :placeholder="$t('m.searchphonenumber')"></search>

    <!-- indexBar 索引列表 -->
    <div class="container">
      <van-index-bar :sticky="false" highlight-color="#069">
        <div v-for="(item,index) of contactsList" :key="index">
          <van-index-anchor :index="item.index" class="indexBarTitle" />
          <van-cell v-for="(item,index) of item.arr" :key="index">
            <div style="display: flex; align-items: center;">
              <div
                v-show="isMultipleChoice"
                style="display:flex;padding-right:.3rem;width:.3rem;height: .3rem; border-radius: 0;"
              >
                <img
                  @click="item.ischecked=!item.ischecked"
                  v-if="item.status==1"
                  style="border-radius: 0;width:100%;height: 100%;"
                  :src="item.ischecked?require('@/assets/images/groupchat_addpeople_sel@2x.png'):require('@/assets/images/groupchat_addpeople_nor@2x.png')"
                  alt
                />
              </div>
              <div class="userInfo">
                <!--  -->
                <img src="@/assets/images/logo.png" alt />
                <span>{{item.name}}</span>
              </div>
            </div>

            <div>
              <button
                @click="invite(item)"
                :class="item.status==2 && 'isAdd'"
              >{{item.status==1?'邀请':'已注册'}}</button>
            </div>
          </van-cell>
        </div>
        <!-- 占位 -->
        <div style="height:2rem;background:#f2f2f2"></div>
      </van-index-bar>
    </div>
    <!-- 底部多选 -->
    <div class="IsHiddenBottom" v-show="isMultipleChoice">
      <div>
        <img
          @click="isCheckAll"
          :src="isAll?require('@/assets/images/groupchat_addpeople_sel@2x.png'):require('@/assets/images/groupchat_addpeople_nor@2x.png')"
          alt
        />
        <span>全选</span>
      </div>
      <div>
        <button @click="inviteAll">邀请</button>
      </div>
    </div>
  </div>
</template>

<script>
import vantHeader from "@/components/common/header";
import search from "@/components/common/NewSearch.vue";

export default {
  data() {
    return {
      MultipleText: "多选", // 顶部右侧文字
      isMultipleChoice: false, //多选模块
      //联系人列表
      contactsList: [
        //status 1可以邀请 2 已注册
        {
          index: "A",
          arr: [
            {
              name: "小明小明小明小明小明小明小明小明小明",
              status: 1,
              ischecked: false
            },
            { name: "小明小明小明小明小明小明小明小明小明", status: 2 },
            {
              name: "小明小明小明小明小明小明小明小明小明",
              status: 1,
              ischecked: false
            }
          ]
        },
        {
          index: "B",
          arr: [
            { name: "小明小明", status: 2 },
            { name: "小明小明", status: 1, ischecked: false },
            { name: "小明小明", status: 2 }
          ]
        },
        {
          index: "C",
          arr: [
            { name: "小明小明小明小明", status: 1, ischecked: false },
            { name: "小明小明小明小明", status: 2 }
          ]
        },
        {
          index: "D",
          arr: [{ name: "小明小明小明", status: 1, ischecked: false }]
        },
        { index: "E", arr: [{ name: "小明小明", status: 1, ischecked: false }] }
      ]
    };
  },

  //计算属性
  computed: {
    isAll() {
      for (let item of this.contactsList) {
        for (let item2 of item.arr) {
          if (item2.status == 1 && !item2.ischecked) {
            return false;
          }
        }
      }
      return true;
    }
  },
  methods: {
    //右下角 全部邀请
    inviteAll() {
      let bool = false;
      for (let item of this.contactsList) {
        for (let item2 of item.arr) {
          if (item2.status == 1 && item2.ischecked) {
            bool = true;
          }
        }
      }
      if (!bool) {
        this.$toast("至少邀请一位好友");
        return;
      }
      //Ajax
      this.$toast("全部邀请");
    },
    //邀请按钮
    invite(item) {
      //已注册
      if (item.status == 2) return;

      //Ajax
      this.$toast("邀请成功");
    },
    //点击右上多选按钮
    MultipleChoice() {
      this.isMultipleChoice = !this.isMultipleChoice;
      this.isMultipleChoice
        ? (this.MultipleText = "取消")
        : (this.MultipleText = "多选");
    },
    //左下全选按钮
    isCheckAll() {
      //根据计算属性里的isAll返回 第一次false
      if (this.isAll) {
        for (let item of this.contactsList) {
          for (let item2 of item.arr) {
            if (item2.status == 1) {
              item2.ischecked = false;
            }
          }
        }
      } else {
        for (let item of this.contactsList) {
          for (let item2 of item.arr) {
            if (item2.status == 1) {
              item2.ischecked = true;
            }
          }
        }
      }
    }
  },
  components: {
    vantHeader,
    search
  },
  mounted() {
    // // 设置右侧A-Z 固定顶部高度
    let vanIndexBarT = document.getElementsByClassName("van-index-bar")[0]
      .offsetTop; //距离顶部
    // let vanIndexBarH = document.getElementsByClassName("van-index-bar")[0]
    //   .offsetHeight; //元素高度
    // let sidebarH = document.getElementsByClassName("van-index-bar__sidebar")[0]
    //   .offsetHeight; //元素高度
    // document.getElementsByClassName("van-index-bar__sidebar")[0].style.top =
    //   vanIndexBarT + vanIndexBarH / 2 + "px";

    //设置滚动区域高度
    document.getElementsByClassName("van-index-bar")[0].style.height =
      document.body.offsetHeight - vanIndexBarT + "px";
  }
};
</script>

<style scoped lang='scss'>
.AddressBookFriends {
  height: 100%;
  overflow: hidden;
  background: #f2f2f2;
  .IsHiddenBottom {
    position: fixed;
    width: 100%;
    box-sizing: border-box;
    bottom: 0;
    padding-left: 0.3rem;
    background: #fff;
    height: 0.98rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;

    div {
      height: 100%;
      display: flex;
      align-items: center;
    }

    img {
      margin-right: 0.2rem;
      width: 0.3rem;
      height: 0.3rem;
    }

    button {
      width: 2.52rem;
      height: 100%;
      border: 0;
      background-color: #2ea2fa;
      color: #ffffff;
    }
  }
  @supports (bottom: env(safe-area-inset-bottom)) {
    .IsHiddenBottom {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
  .hint {
    width: 100%;
    padding-left: 0.28rem;
    box-sizing: border-box;
    display: flex;
    align-items: center;
    height: 0.68rem;
    background-color: #ffebd4;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
  }

  .fixed {
    position: fixed;
    width: 100%;
    padding: 0 0.5rem;
    box-sizing: border-box;
    height: 0.6rem;
    background-color: #2ea2fa;

    & > div {
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: space-between;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;

      div:nth-child(1),
      div:nth-child(3) {
        width: 1rem;
        font-family: PingFang-SC-Bold;
        text-align: right;
      }

      div:nth-child(2) {
        font-family: PingFang-SC-Medium;
      }
    }
  }

  .container {
    background: #f2f2f2;
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
        align-items: center;
        padding-right: 0.2rem;
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
          width: 3rem;
          text-overflow: ellipsis;
          white-space: nowrap;
          overflow: hidden;
        }

        img {
          border-radius: 50%;

          width: 0.72rem;
          height: 0.72rem;
          background-color: #d1d1d1;
        }
      }

      .userInfo + div {
        display: flex;
        align-items: center;
      }

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

      .isAdd {
        background: #fff !important;
        color: #999 !important;
        padding: 0 !important;
      }
    }

    .van-index-bar {
      overflow-y: scroll;

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
