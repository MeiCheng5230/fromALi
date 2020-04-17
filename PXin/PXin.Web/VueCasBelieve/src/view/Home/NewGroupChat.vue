<!-- 新建群组聊天 -->
<template>
  <div class="NewGroupChat">
    <!-- 搜索 -->
    <div style="background: #f2f2f2">
      <div class="search">
        <div v-for="(item,index) of imgList" :key="index">
          <img :src="item" alt />
        </div>

        <span>搜索</span>
      </div>
    </div>
    <!-- 选择一个群 -->
    <div class="flock">
      <span>选择一个群</span>
      <span></span>
    </div>
    <!-- van index bar 索引字符列表 -->
    <div class="ContentList">
      <div v-for="(item,index) of ContactsList" :key="index">
        <div class="item">
          <div>{{item.index}}</div>
          <div>
            <div class="info" v-for="(item2,index) of item.indexArr" :key="index">
              <div @click="CheckedClick(item2)">
                <img
                  :src="item2.isChecked?require('@/assets/images/groupchat_addpeople_sel@2x.png'):require('@/assets/images/groupchat_addpeople_nor@2x.png')"
                  alt
                />
              </div>
              <div>
                <img :src="item2.imgUrl" alt />
                <span>{{item.index}}{{item2.name}}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 底部 -->
    <div class="footer">
      <div>
        <span>已选择</span>
        <span>({{CheckedCount}})</span>
      </div>
      <div @click="submit">完成</div>
    </div>
  </div>
</template>

<script>
import mysearch from "@/components/common/search";
export default {
  data() {
    return {
      //顶部已选图片集合
      imgList: [],
      title: "选择联系人",
      CheckedCount: 0, //已选数量
      ContactsList: [
        //联系人列表
        {
          index: "A",
          indexArr: [
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸1",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/addfriend_saoyisao@2x.png"),
              name: "小狐狸2",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/chat_chat_address@2x.png"),
              name: "小狐狸3",
              isChecked: false
            }
          ]
        },
        {
          index: "B",
          indexArr: [
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸1",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸2",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸3",
              isChecked: false
            }
          ]
        },
        {
          index: "C",
          indexArr: [
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸1",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸1",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸1",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸2",
              isChecked: false
            },
            {
              imgUrl: require("@/assets/images/invite_weichat@2x.png"),
              name: "小狐狸3",
              isChecked: false
            }
          ]
        }
      ]
    };
  },
  methods: {
    //选择联系人
    CheckedClick(item) {
      item.isChecked = !item.isChecked;
      if (item.isChecked) {
        this.CheckedCount++;
        this.imgList.push(item.imgUrl);
      } else {
        this.CheckedCount--;
        for (let i = 0; i < this.imgList.length; i++) {
          if (item.imgUrl == this.imgList[i]) {
            this.imgList.splice(i, 1);
          }
        }
      }
    },
    //右下角完成
    submit() {
      //至少勾选一个
      let bool = false;
      for (let item of this.ContactsList) {
        for (let item2 of item.indexArr) {
          if (item2.isChecked) {
            bool = true;
            break;
          }
        }
      }
      if (!bool) {
        this.$toast("至少勾选一个");
        return;
      }
      //Ajax
      this.$toast("完成");
      this.$router.go(-1);
    }
  },
  components: {
    mysearch
  }
};
</script>

<style scoped lang='scss'>
.search {
  display: flex;

  flex-wrap: wrap;
  align-items: center;
  padding-left: 0.3rem;
  & > div {
    height: 0.98rem;
    display: flex;
    align-items: center;
  }
  img {
    width: 0.6rem;
    height: 0.6rem;
    border-radius: 50%;
  }
  & > div:nth-of-type(n + 2) {
    margin-left: 0.15rem;
  }
  span {
    line-height: 0.98rem;
    height: 0.98rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
    padding-left: 0.2rem;
  }
}
.ContentList > div:last-child .item .info:last-child {
  border-bottom: 0.01rem solid #d1d1d1;
}
.NewGroupChat {
  padding-bottom: 2rem;
  .flock {
    width: 100%;
    box-sizing: border-box;
    padding: 0 0.3rem;
    background-color: #f2f2f2;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    margin-top: 0.1rem;
    height: 0.98rem;
    display: flex;
    align-items: center;
    justify-content: space-between;

    span:nth-child(2) {
      display: block;
      width: 0.2rem;
      height: 0.2rem;
      border-top: 0.04rem solid #999;
      border-right: 0.04rem solid #999;
      transform: rotate(45deg);
    }
  }

  .footer {
    box-sizing: border-box;
    width: 100%;
    height: 0.88rem;
    background-color: #ffffff;
    box-shadow: inset 0rem 0.01rem 0rem 0rem #f1f1f1;
    display: flex;
    align-items: center;
    justify-content: space-between;
    position: fixed;
    bottom: 0;
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    padding-left: 0.3rem;

    div {
      display: flex;
      align-items: center;
    }

    div:nth-child(2) {
      width: 2rem;
      height: 100%;
      justify-content: center;
      background-color: #2ea2fa;
      box-shadow: 0rem -0.01rem 0rem 0rem #f2f2f2;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
    }
  }
  @supports (bottom: env(safe-area-inset-bottom)) {
    .footer {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
  .ContentList {
    padding-top: 0.1rem;
    width: 100%;

    .item {
      width: 100%;

      & > div:nth-child(1) {
        display: flex;
        align-items: center;
        padding: 0 0.3rem;
        height: 0.68rem;
        background-color: #f2f2f2;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #494949;
      }

      & > div:nth-child(2) {
        padding-left: 0.3rem;
      }

      .info {
        height: 1.08rem;
        display: flex;
        align-items: center;
        border-bottom: 0.01rem solid #d1d1d1;

        & > div:nth-child(1) {
          width: 0.3rem;
          height: 0.3rem;
          display: flex;
          padding-right: 0.3rem;

          img {
            width: 100%;
            height: 100%;
          }
        }

        & > div:nth-child(2) {
          width: 100%;
          display: flex;
          align-items: center;

          img {
            width: 0.72rem;
            height: 0.72rem;
            border-radius: 50%;
          }

          span {
            padding-left: 0.2rem;
            font-family: PingFang-SC-Medium;
            font-size: 0.3rem;
            font-weight: normal;
            font-stretch: normal;
            letter-spacing: 0rem;
            color: #494949;
            width: 5rem;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
          }
        }
      }

      .info:last-child {
        border: 0;
      }
    }
  }
  @supports (bottom: env(safe-area-inset-bottom)) {
    .ContentList {
      padding-bottom: env(safe-area-inset-bottom);
    }
  }
}

/deep/ .van-index-bar__sidebar {
  display: none;
}
</style>
