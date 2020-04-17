<template>
  <div class="invitation">
    <div class="content">
      <div class="title"><img src="@/assets/images/invite_bg_left.png" /><span>我的邀请码</span><img src="@/assets/images/invite_bg_right.png" /></div>
      <div class="mycode">
        <div class="item" v-for="item in mycode" :key="item.index">
          <div class="codenum">{{ item.invitationcode }}</div>
          <div class="copy" @click="copy(item.invitationcode)">复制</div>
        </div>
      </div>
      <div class="nodata" v-if="nodataFlag">无可用邀请码</div>
      <router-link tag="div" :to="{name: 'invitelist', params: {inviteList: inviteList}}" class="myinvitation">
        <span>我邀请的好友</span>
        <van-icon name="arrow" color="#ccc" size="0.3rem" />
      </router-link>
      <div class="line"></div>
      <div class="download">
        <div id="qrcode" ref="qrcode"></div>
        <p>扫码下载相信APP<br>加入相信免费赠送50V点</p>
      </div>
    </div>
  </div>
</template>

<script>
  import QRCode from 'qrcodejs2';
  import { InviteesList } from "@/api/getData.js"
  export default {
    data() {
      return {
        mycode: [],         // 未使用列表
        inviteList: [],     // 已邀请的好友列表
        nodataFlag: false,
      }
    },
    created() {
      this.getData();
    },
    mounted() {
      this.$nextTick(() => {
        this.qrcode()
      })
    },
    methods: {
      qrcode() {
        let qrcode = new QRCode('qrcode', {
          width: 90, // /
          height: 90, // 设置高度，单位像素
          text: 'http://client.xiang-xin.net/home/register'   // 设置二维码内容或跳转地址
        })
      },
      copy(code) {
        var oInput = document.createElement("input");
        oInput.value = code;
        document.body.appendChild(oInput);
        oInput.select(); // 选择对象
        document.execCommand("Copy"); // 执行浏览器复制命令
        oInput.className = "oInput";
        oInput.style.display = "none";
        this.Toast('复制成功');
      },
      async getData() {
        let result = await InviteesList(this.$global.userInfo);
        if (result.result > 0) {
          let arr = result.data;
          this.mycode = [];
          for (const itm of arr) {
            if (!itm.isused) {
              this.mycode.push(itm);
            } else {
              this.inviteList.push(itm);
            };
          }
          if (this.mycode.length <= 0) {
            this.nodataFlag = true;
          }
        } else {
          this.Toast.fail(result.message);
        }

      }
    }
  }
</script>

<style lang="scss" scoped>
  .invitation {
    min-height: 100%;
    background: #2ea2fa;
    padding: 0.4rem 0.5rem 0.9rem 0.5rem;
    font-size: 0.3rem;
    box-sizing: border-box;
    .content

  {
    background: #fff;
    border-radius: 0.12rem;
    padding: 0.45rem 0.54rem;
    .title

  {
    font-size: 0.36rem;
    font-weight: bold;
    text-align: center;
    display: flex;
    justify-content: center;
    align-items: center;
    img

  {
    height: 0.27rem;
  }

  span {
    padding: 0 0.32rem;
  }

  }

  .mycode {
    padding: 0 0.65rem;
    .item

  {
    display: flex;
    align-items: center;
    margin-top: 0.3rem;
    .codenum

  {
    flex: auto;
    font-size: 0.6rem;
    color: #ff5b1b;
  }

  .copy {
    background: #2ea2fa;
    width: 1.2rem;
    height: 0.48rem;
    line-height: 0.48rem;
    text-align: center;
    white-space: nowrap;
    border-radius: 0.24rem;
    color: #fff;
  }

  }
  }

  .nodata {
    font-size: 0.72rem;
    font-weight: bold;
    color: #999;
    text-align: center;
    padding: 1.2rem 0;
  }

  .myinvitation {
    background: #f7f7fc;
    border-radius: 0.04rem;
    padding: 0.26rem 0.24rem;
    margin-top: 0.4rem;
    font-weight: bold;
    display: flex;
    align-items: center;
    span

  {
    flex: auto;
  }

  }

  .line {
    border-top: 2px dashed #f7f7fc;
    margin: 0.5rem 0;
    position: relative;
  }

  .download {
    display: flex;
    flex-direction: column;
    align-items: center;
    text-align: center;
    p

  {
    text-align: center;
    width: 3.42rem;
    margin-top: 0.36rem;
    font-weight: bold;
  }

  }
  }
  }
</style>
