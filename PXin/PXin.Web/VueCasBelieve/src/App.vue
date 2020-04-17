<template>
  <div id="app" class>
    <router-view />

    <!--  -->
    <div class="moreBox" id="moreBox">
      <div class="moreBox-body">
        <div class="moreBox-list" @click="goAs">关于相信</div>
        <div class="moreBox-list" id="close" @click="closeBox" style>取消</div>
      </div>
    </div>
  </div>
</template>

<script>
import ImService from "@/config/imService";
import { Login } from "@/api/getChatData";
import { setStore } from "@/config/utils.js";
import {
  startConnection,
  getSignalrConnectionStatus,
  executeServerMethod
} from "@/config/signalr.js";

export default {
  data() {
    return {
      nodecode: ""
    };
  },
  methods: {
    closeBox() {
      document.getElementById("moreBox").style.display = "none";
    },
    goAs() {
      //moreId
      location.href =
        "http://global.ckv-test.sulink.cn/Cas/moreIndex.html?moreId=1";
    },
    Login() {
      Login({ ...JSON.parse(sessionStorage.userParam) }, res => {
        if (res.result > 0) {
          let host = window.location.host;
          console.log("host==>" + host);
          var appKey = "25wehl3uwm6bw"; //"4z3hlwrv3n08t";
          if (
            process.env.NODE_ENV == "production" &&
            (host != "client.be.sulink.cn" && host != "pxin.ckv-test.sulink.cn")
          ) {
            appKey = "25wehl3uwm6bw";
          }
          ImService.init({
            appKey: appKey,
            token: res.data.token
          });
          setStore("UserConfigInfo", res);
          this.nodecode = res.data.nodecode;
          startConnection(this.nodecode);
        }
      });
    }
  },
  mounted() {
    let _this = this;
    setInterval(() => {
      if (ImService.ConnectionStatus != 0) {
        console.log("登录检测");
        _this.Login();
      } else if (getSignalrConnectionStatus() != 1) {
        startConnection(_this.nodecode);
      }
    }, 1000);
  },
  beforeDestroy() {
    let data = {
      command_id: 0x00000002,
      sequence_id: GetSequence(),
      reason: 1
    };
    executeServerMethod(data);
  }
};
</script>

<style lang='scss'>
.van-notify {
  height: 0.88rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
.moreBox {
  position: fixed;
  left: 0;
  right: 0;
  bottom: 0;
  top: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: none;
  .moreBox-body {
    background-color: #f7f7fc;
    position: absolute;
    bottom: 0;

    left: 0;
    right: 0;
    font-size: 16px;

    text-align: center;
    /*padding: 0.3rem;*/
    border-radius: 10px 10px 0 0;
    overflow: hidden;
    .moreBox-list {
      padding: 0.3rem;
      background-color: #fff;
      &#close {
        margin-top: 0.2rem;
      }
    }
  }
}

#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

html,
body,
#app {
  box-sizing: border-box;
  width: 100%;
  height: 100%;
  padding: 0;
  margin: 0;
}

/* top  */
.paddingTop {
  padding-top: 1rem !important;
}

/* bottom */
.paddingBottom {
  padding-bottom: 0.98rem;
}

img[src=""],
img:not([src]) {
  opacity: 0;
  border: 0 !important;
  visibility: hidden;
  /* max-width: none; */
  width: 0 !important;
}
.van-notify {
  height: 1rem;
  line-height: 1rem;
  text-align: center;
  padding: 0;
  background: #fff !important;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333 !important;
}
</style>
