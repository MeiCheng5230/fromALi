<template>
  <div class="AddressBookHome">
    <div class="fixed">
      <div class="AddressBook">
        <!-- 通讯录 -->
        <div>
          <div></div>
          <div>{{$t('m.contacts')}}</div>
          <div>
            <img
              @click="$router.push('/AddFriend')"
              src="@/assets/images/list_add-friends@2x.png"
              alt
            />
            <img
              @click="$router.push('/AddressSearch')"
              src="@/assets/images/list_search@2x.png"
              alt
            />
          </div>
        </div>
      </div>
    </div>
    <!-- 高度 -->
    <div style="height: .87rem;"></div>
    <!-- 新的朋友 群聊 广播号... -->
    <div class="container">
      <div class="options" style="position: relative;">
        <router-link to="/NewFriend" tag="div" class="option">
          <img src="@/assets/images/list_newfriends@2x.png" alt />
          <span class="msg">
            {{$t('m.newfriend')}}
            <i
              class="msgCount"
              v-show="addFriendMsgCount>0"
            >{{addFriendMsgCount}}</i>
          </span>
        </router-link>
        <router-link to="/AddGroupChat" tag="div" class="option">
          <img src="@/assets/images/list_groupchat@2x.png" alt />
          <span>{{$t('m.groupchat')}}</span>
        </router-link>

        <router-link to="/AddressBookFriends" tag="div" class="option">
          <img src="@/assets/images/list_contact@2x.png" alt />
          <span>{{$t('m.addacontack')}}</span>
        </router-link>
        <router-link to="/InviteFriend" tag="div" class="option">
          <img src="@/assets/images/list_friend@2x.png" alt />
          <span>{{$t('m.invite')}}</span>
        </router-link>
      </div>
    </div>
    <!-- 高度 -->
    <div style="height: 1.19rem;"></div>
    <!-- indexBar 索引列表 -->
    <div class="indexBar">
      <div class="indexBarContent">
        <van-index-bar highlight-color="#f00" :sticky="false" :index-list="indexList">
          <div v-for="(item,index) of contactsList" :key="index">
            <van-index-anchor :index="item.index" />
            <van-cell
              v-for="(item, index) of item.arr"
              :key="index"
              @click="$router.push({path:'/Information',query:{userId:item.nodeid}})"
            >
              <div class="userInfo">
                <img :src="item.url==null?'@/assets/images/personal_head_no@2x.png':item.url" alt />
                <span>{{item.name}}</span>
              </div>
            </van-cell>
          </div>
        </van-index-bar>
      </div>
    </div>
    <!-- footer -->
    <myfooter></myfooter>
  </div>
</template>

<script>
import ImService from "@/config/imService";
import myfooter from "@/components/common/footer";
import { MyFriend } from "@/api/localStorageData";
import vPinyin from "@/config/chineseToPinYin";
export default {
  data() {
    return {
      //联系人列表
      contactsList: [],
      indexList: [
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
        "Q",
        "R",
        "S",
        "T",
        "U",
        "V",
        "W",
        "X",
        "Y",
        "Z",
        "#"
      ],
      addFriendMsgCount: 0
    };
  },
  created() {
    MyFriend(null, res => {
      if (res.length > 0) {
        let tempData = [
          "A",
          "B",
          "C",
          "D",
          "E",
          "F",
          "G",
          "H",
          "I",
          "J",
          "K",
          "L",
          "M",
          "N",
          "O",
          "P",
          "Q",
          "R",
          "S",
          "T",
          "U",
          "V",
          "W",
          "X",
          "Y",
          "Z"
        ];
        let contactsList = [];
        res.forEach(ele => {
          let py = vPinyin.chineseToPinYin(ele.nodename).toLocaleUpperCase();
          let index = contactsList.indexOf(py);
          if (index < 0 && tempData.indexOf(py) > -1) {
            contactsList.push(py);
          }
        });
        contactsList.sort();
        contactsList.push("#");
        let showContactList = [];
        contactsList.forEach(ele => {
          showContactList.push({
            index: ele,
            arr: []
          });
        });
        res.forEach(ele => {
          let py = vPinyin.chineseToPinYin(ele.nodename).toLocaleUpperCase();
          for (let i = 0; i < showContactList.length; i++) {
            let index = showContactList[i].index.indexOf(py);
            if (index > -1) {
              showContactList[i].arr.push({
                name: ele.nodename,
                url: ele.appphoto,
                nodeid: ele.nodeid,
                remarks: ele.remarks,
                nodecode: ele.nodecode
              });
              break;
            } else {
              if (i + 1 == showContactList.length) {
                showContactList[showContactList.length - 1].arr.push({
                  name: ele.nodename,
                  url: ele.appphoto,
                  nodeid: ele.nodeid,
                  remarks: ele.remarks,
                  nodecode: ele.nodecode
                });
              }
            }
          }
        });
        this.contactsList = showContactList;
      }
    });
  },
  mounted() {
    // 索引固定右侧
    // let HeaderHeight = document.getElementsByClassName('van-nav-bar')[0].offsetHeight;
    let FixedTop = document.getElementsByClassName("van-index-bar")[0];
    let RightFixed = document.getElementsByClassName(
      "van-index-bar__sidebar"
    )[0];
    RightFixed.style.top = FixedTop.offsetTop + "px"; //元素所在位子距离顶部距离 固定值 +5 间隙
    //      // 设置高度
    // let vanIndexBarT = document.getElementsByClassName('van-index-bar')[0].offsetTop; //距离顶部
    // let vanIndexBarH = document.getElementsByClassName('van-index-bar')[0].offsetHeight; //元素高度
    // let sidebarH = document.getElementsByClassName('van-index-bar__sidebar')[0].offsetHeight; //元素高度
    // document.getElementsByClassName('van-index-bar__sidebar')[0].style.top = vanIndexBarT + vanIndexBarH/2 +'px'
    this.getConversationList();
  },
  methods: {
    //获取会话列表
    getConversationList() {
      let _this = this;
      ImService.getConversationList(list => {
        console.log(list);
        let chatMsgConversations = list.filter(function(item) {
          if (
            item.conversationType == 6 &&
            item.objectName == "RC:ContactNtf" &&
            item.latestMessage.content.operation != "respfriendpass" &&
            item.unreadMessageCount > 0
          ) {
            return item;
          }
        });
        _this.addFriendMsgCount = chatMsgConversations.length;
      });
    }
  },
  components: {
    myfooter
  }
};
</script>

<style scoped lang='scss'>
.AddressBookHome {
  height: 100%;
  position: relative;

  .msg {
    position: relative;

    .msgCount {
      position: absolute;
      right: -0.2rem;
      top: -0.07rem;
      width: 0.24rem;
      height: 0.24rem;
      background-color: #ff0000;
      font-family: PingFang-SC-Medium;
      font-size: 0.2rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #fffefe;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      font-style: normal;
    }
  }

  .fixed {
    width: 100%;
    position: fixed;
    z-index: 1;
  }

  .AddressBook {
    width: 100%;
    background-color: #2ea2fa;

    & > div:nth-child(1) {
      width: 100%;
      box-sizing: border-box;
      padding: 0 0.52rem;
      height: 0.87rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      display: flex;
      align-items: center;
      justify-content: space-between;

      img {
        width: 0.4rem;
        height: 0.4rem;
      }

      & > div:nth-child(1),
      & > div:nth-child(3) {
        width: 1rem;
        display: flex;
        justify-content: space-between;
      }
    }
  }

  .container {
    background: #2ea2fa;
    box-sizing: border-box;
    padding: 0 0.3rem;
    height: 1rem;
    border-radius: 0rem 0rem 0.3rem 0.3rem;
  }

  .options {
    width: 100%;
    height: 2.2rem;
    box-sizing: border-box;
    background-color: #ffffff;
    box-shadow: 0rem 0.03rem 0.07rem 0rem #ececec;
    border-radius: 0.1rem;
    margin: 0 auto;
    display: flex;
    padding: 0 0.2rem;
    align-items: center;
    justify-content: space-between;

    .option {
      width: 20%;
      height: 1.18rem;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      align-items: center;

      img {
        width: 0.68rem;
        height: 0.68rem;
        /*          background-image: linear-gradient(0deg,
            #868eff 0%,
            #83c8ff 100%),
            linear-gradient(#2ea2fa,
            #2ea2fa); */
        background-blend-mode: normal, normal;
        border-radius: 0.3rem;
      }

      span {
        font-family: PingFang-SC-Medium;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
        white-space: nowrap;
      }
    }
  }

  .van-cell:not(:last-child)::after {
    border-bottom: 0.02rem solid #d1d1d1;
  }

  .indexBar {
    /deep/ .van-index-bar__sidebar {
      padding-right: 0.1rem;
      transform: translateY(0);
      right: 0;
      top: 0;

      // .van-index-bar__index {
      //   /* line-height: .3rem; */
      // }
    }
    .van-index-bar {
      padding-bottom: 1.5rem;
      margin-top: 0.6rem;
      height: 6.73rem;
      overflow: scroll;
      -webkit-overflow-scrolling: touch;
    }

    .van-cell {
      height: 1.09rem;
      padding: 0 15px;

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
          white-space: nowrap;
          text-overflow: ellipsis;
          overflow: hidden;
        }

        img {
          border-radius: 50%;

          width: 0.72rem;
          height: 0.72rem;
          background-color: #d1d1d1;
        }
      }
    }

    /deep/ .van-index-anchor {
      background-color: #f2f2f2;
    }
  }
}
</style>
