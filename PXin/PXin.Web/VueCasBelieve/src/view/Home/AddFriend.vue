<!-- 添加好友 -->
<template>
  <div class="AddFriend">
    <!-- 搜索组件 -->
    <mysearch
      @click.native="$router.push('/AddressSearch')"
      :placeholder="$t('m.searchphonenumber')"
      iptBG="#fff"
      marginLeft=".6rem"
    ></mysearch>
    <!-- 我的二维码 -->
    <div class="myCode">
      <span>{{$t('m.myqrcode')}}</span>
      <img @click="$router.push('/TwoCode')" src="@/assets/images/addfried_erweima@2x.png" alt />
    </div>
    <!-- 邀请好友 -->
    <div
      class="iml"
      style="width: 100%;box-sizing: border-box; padding: 0 .3rem; padding-top: .3rem;"
    >
      <img @click="show=true" style="width: 100%;" src="@/assets/images/addfriend_invite@2x.png" alt />
    </div>
    <!-- 扫一扫 手机联系人 -->
    <div>
      <!-- 扫一扫 -->
      <div class="typeInfo">
        <div>
          <img src="@/assets/images/addfriend_saoyisao@2x.png" alt />
        </div>
        <div>
          <span>{{$t('m.scan')}}</span>
          <span>{{$t('m.scantheqrcode')}}</span>
        </div>
      </div>
      <!-- 手机联系人 -->
      <router-link to="/AddressBookFriends" tag="div" class="typeInfo">
        <div>
          <img src="@/assets/images/addfriend_contact@2x.png" alt />
        </div>
        <div>
          <span>{{$t('m.phonecontact')}}</span>
          <span>{{$t('m.addfriend')}}</span>
        </div>
      </router-link>
    </div>

    <!-- 遮罩 -->

    <div class="popupMod" v-show="show">
      <div class="ModList">
        <div class="ModInfo">
          <div>
            <div>
              <img src="@/assets/images/invite_weichat@2x.png" alt />
            </div>
            <div>{{$t('m.WeChat')}}</div>
          </div>
          <div>
            <div>
              <img src="@/assets/images/invite_friendcircle@2x.png" alt />
            </div>
            <div>{{$t('m.friendcircle')}}</div>
          </div>
          <div>
            <div>
              <img src="@/assets/images/invite_qq@2x.png" alt />
            </div>
            <div>QQ</div>
          </div>
          <div>
            <div>
              <img src="@/assets/images/invite_pcn@2x.png" alt />
            </div>
            <div>PCN</div>
          </div>
          <div>
            <router-link to="/MsgInvite" tag="div">
              <img src="@/assets/images/invite_note@2x.png" alt />
            </router-link>
            <div>{{$t('m.SMS')}}</div>
          </div>
        </div>
        <div class="close">
          <img @click="show=false" src="@/assets/images/invite_delete@2x.png" alt />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import mysearch from "@/components/common/search";
export default {
  data() {
    return {
      //遮罩层弹出框
      show: false
    };
  },
  components: {
    mysearch
  }
};
</script>
<style scoped lang='scss'>
.popupMod {
  position: fixed;
  top: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.8);
  .ModList {
    position: absolute;
    bottom: 0;
    width: 100%;

    .close {
      width: 100%;
      height: 1.87rem;
      display: flex;
      justify-content: center;
      align-items: center;
      img {
        width: 0.5rem;
        height: 0.5rem;
      }
    }
  }
  .ModInfo {
    filter: blur(0px);
    width: 100%;
    height: 3.36rem;

    bottom: 1.87rem;
    display: flex;
    flex-wrap: wrap;

    & > div:nth-of-type(n + 4) {
      align-self: flex-end;
    }

    & > div {
      width: 33.3%;
      height: 1.38rem;
      display: flex;
      justify-content: space-between;
      flex-direction: column;
      align-items: center;
      font-family: AdobeHeitiStd-Regular;
      font-size: 0.29rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #e2e2e2;

      img {
        width: 0.8rem;
        height: 0.8rem;
      }
    }
  }
}

.AddFriend {
  height: 100%;
  background-color: #f2f2f2;

  .typeInfo {
    height: 1.28rem;
    background: #fff;
    margin-top: 0.1rem;
    display: flex;
    padding: 0 0.3rem;
    align-items: center;

    & > div:nth-child(1) {
      width: 0.8rem;
      height: 0.8rem;
      padding-right: 0.23rem;
      display: flex;
      align-items: center;

      img {
        width: 100%;
        height: 100%;
      }
    }

    & > div:nth-child(2) {
      height: 0.8rem;
      display: flex;
      flex-direction: column;
      justify-content: space-between;
    }

    span {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;
    }

    span:nth-child(2) {
      color: #999999;
      font-size: 0.28rem;
    }
  }

  .typeInfo:nth-child(1) {
    margin-top: 0;
  }

  .myCode {
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
    display: flex;
    align-items: center;
    justify-content: center;
    padding-top: 0.3rem;

    img {
      padding-left: 0.2rem;
      width: 0.32rem;
      height: 0.32rem;
    }
  }
}
</style>
