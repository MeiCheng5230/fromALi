<!-- 邀请好友 -->
<template>
  <div class="InviteFriend">
    <div class="TopBgImg">
      <img src="@/assets/images/invite_bg@2x.png" alt />
      <div class="container">
        <div style="display: flex; justify-content: center;position: relative;top:.26rem">
          <div
            class="BgImg"
            :style="{backgroundImage:'url('+require('@/assets/images/invite_bg_02.png')+')'}"
          >邀请好友&nbsp;&nbsp;&nbsp;来相信</div>
        </div>

        <div>
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
          <router-link to="/MsgInvite" tag="div">
            <div>
              <img src="@/assets/images/invite_note@2x.png" alt />
            </div>
            <div>{{$t('m.SMS')}}</div>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>


<style scoped lang='scss'>
.InviteFriend {
  height: 100%;
  background-color: #f5eefb;

  .BgImg {
    width: 4.45rem;
    height: 0.65rem;
    background-size: 100% 100%;
    font-family: AdobeHeitiStd-Regular;
    font-size: 0.32rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    display: flex;
    justify-content: center;
    align-items: center;
  }

  /deep/ .van-nav-bar {
    background: rgba(200, 200, 200, 0) !important;
  }

  .TopBgImg {
    position: absolute;
    top: 0;
    height: 7.1rem;
    width: 7.5rem;

    img {
      width: 100%;
      height: 100%;
    }

    .container {
      position: relative;
      bottom: 1.75rem;

      & > div:nth-child(2) {
        width: 6.9rem;
        height: 4.8rem;
        box-sizing: border-box;
        padding: 0 0.6rem;
        padding-top: 0.85rem;
        background-color: #ffffff;
        border-radius: 0.1rem;
        margin: 0 auto;
        display: flex;
        flex-wrap: wrap;

        & > div {
          width: 33%;
          height: 1.38rem;
          justify-content: space-between;
          display: flex;
          flex-direction: column;
          align-items: center;
          font-family: AdobeHeitiStd-Regular;
          font-size: 0.29rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #666666;

          div:nth-child(1) {
            width: 0.8rem;
            height: 0.8rem;
            border-radius: 50%;

            img {
              border-radius: 50%;
              width: 100%;
              height: 100%;
            }
          }
        }
      }
    }
  }
}
</style>
