<!-- 我的代理人 -->
<template>
  <div class="myDealer">
    <!-- <div class='hint' v-if='isZys'>支付500DOS开通1个代理人，代理人进货后，您将获得其进货额度10%的DP奖励</div> -->
    <table></table>
    <!-- 代理人才显示  -->
    <div class="dealerInfo" v-if="isZys===false">
      <div style="padding: 0.3rem;color: #666;background-color: #fff;">
        <div class="info" style="padding-bottom: 0.1rem">
          充值商名称：
          <span style="color: #1989fa;font-size: 0.32rem">{{zysInfo.parentname}}</span>
        </div>
        <div
          class="info"
          style="padding-bottom: 0.1rem"
        >充值商用户名称：{{zysInfo.nodename?zysInfo.nodename.slice(0,1)+'*'.repeat(zysInfo.nodename.length-1):''}}</div>
        <div
          class="info"
          style="padding-bottom: 0.1rem"
        >充值商手机号：{{zysInfo.mobileno?zysInfo.mobileno.slice(0, 3)+'****'+zysInfo.mobileno.slice(7, 11):''}}</div>
      </div>
    </div>
    <div class="dealerList" v-if="isZys">
      <table></table>
      <div class="dealerInfo" v-for="(item,index) of userList" :key="index">
        <!--  -->
        <div>
          <p>
            <!--  -->
            <span>{{item.nodename}}</span>
            <button :class="item.statusdesc | status">{{item.statusdesc}}</button>
          </p>
          <p>开始时间：{{item.starttimeformat}}</p>
          <p>结束时间：{{item.endtimeformat}}</p>
        </div>

        <!--  -->
        <div class="inven">
          <p>
            <span>库存：</span>
            <span>{{item.counts}}</span>
          </p>
          <div class="btns">
            <!-- 待审核 -->
            <!--  -->
            <div id="await" v-if="item.statusdesc=='等待审核'">
              <button @click="AuditBtn(item.infoid)">审核</button>
            </div>

            <router-link
              :to="{path:'/Renew',query:{userJxs:JSON.stringify(item),infoid:JSON.stringify(item.infoid),isown:true}}"
              tag="button"
              :disabled="item.statusdesc!=='审核通过'"
              :class="item.statusdesc=='审核通过'&&'yellow'"
            >续费</router-link>
          </div>
        </div>
      </div>
    </div>

    <div class="empty" v-if="isZys && empty ">暂无代理人，请新增代理人吧</div>

    <div class="btn" v-if="isZys">
      <button @click="$router.push({path:'/add'})">新增代理人</button>
    </div>
  </div>
</template>

<script>
// <!-- 我的代理人 -->
import { Dialog } from "vant";
import { GetMyUserJxs, GetMyUserCzs } from "@/api/getFbApData";
import { getStore } from "@/config/utils";

export default {
  data() {
    return {
      //列表
      userList: [],
      //是否代理人
      isJxs: false,
      //代理人信息
      JsxInfos: {},
      //充值商信息
      zysInfo: {},
      //是否为充值商
      isZys: false,
      //
      audit: false,
      radio: "1",
      remarks: "",
      infoid: "",
      empty: false //数据是否为空
    };
  },
  created() {
    this.isJxsOrZys();
  },
  watch: {
    userList(newValue, formreValue) {
      if (!newValue.length && !formreValue.length) {
        this.empty = true;
      }
    }
  },
  filters: {
    status(n) {
      // switch(n){
      //   case "冻结":
      //   case "认证失败":
      //     return "red" ;
      //   case "审核通过":
      //     return "yellow1";
      //   return ''
      // }
      if (n == "冻结" || n == "认证失败") return "red";
      if (n == "审核通过") return "yellow1";
      return " ";
    }
  },
  methods: {
    //获取我的代理人
    async GetMyUserJxs() {
      let result = await GetMyUserJxs(
        JSON.parse(sessionStorage.userParam),
        JSON.parse(this.$route.query.infoid)
      );
      if (result.result > 0) {
        this.userList = result.data;
      }
    },
    //获取我的充值商
    async GetMyUserCzs() {
      let result = await GetMyUserCzs(
        JSON.parse(sessionStorage.userParam),
        JSON.parse(this.$route.query.infoid)
      );
      if (result.result > 0) {
        this.zysInfo = result.data;
      }
    },
    //审核代理人弹窗
    AuditBtn(id) {
      this.$router.push({
        path: "/Audit",
        query: {
          infoid: id
        }
      });
    },
    //代理人||充值商
    isJxsOrZys() {
      let zys = JSON.parse(getStore("FbApInfo"));
      this.isZys = zys.isczs;
      //当用户是充值商---->我的代理人--->我的代理人列表
      if (this.isZys) {
        this.$route.meta.title = "我的代理人";
        this.$route.meta.subTitle = "新增代理人";
        this.GetMyUserJxs();
        return;
      }
      //当用户是代理人 --->我的充值商--->充值商信息
      this.$route.meta.title = "我的充值商";
      this.$route.meta.subTitle = "";
      this.GetMyUserCzs();
    }
  },
};
</script>

<style scoped lang='scss'>
/* <!-- 我的代理人 --> */
.btns {
  display: flex;
  button{
    white-space: nowrap;
  }
  #await {
    margin-right: 0.3rem;
    button {
      background: #ff9000;
      color: rgba(255, 255, 255, 1);
    }
  }
}

.btn {
  height: 0.88rem;
  position: fixed;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  bottom: 0;

  button {
    border: 0;
    width: 100%;
    height: 100%;
    background-color: #2ea2fa;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
  }
}

.yellow1 {
  background-color: #5194e7 !important;
}

.empty {
  font-size: 0.3rem;
  font-family: PingFang-SC-Medium;
  font-weight: 500;
  color: #666666;
  text-align: center;
}

.audit {
  position: fixed;
  left: 0;
  right: 0;
  bottom: 0;
  top: 0;
  background-color: rgba(0, 0, 0, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;

  .audit-body {
    border-radius: 6px;
    overflow: hidden;
    background-color: #fff;
    padding: 0.3rem;

    .audit-title {
      text-align: center;
      padding: 0 0 0.2rem 0;
    }

    .audit-bottom {
      padding: 0.2rem 0;
      text-align: center;
      display: flex;
      justify-content: space-around;
    }
  }
}

.red {
  background: rgba(255, 48, 48, 1);
}

.yellow {
  background: #2ea2fa !important;
}

.myDealer {
  min-height: 100%;
  background: rgba(244, 244, 244, 1);

  .dealerList {
    padding-bottom: 2rem;
  }

  .hint {
    line-height: 0.44rem;
    padding: 0.1rem 0.22rem;
    background: rgba(255, 234, 210, 1);
    font-size: 0.24rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(255, 48, 48, 1);
  }

  .dealerInfo {
    display: flex;
    justify-content: space-between;
    align-items: flex-end;
    padding: 0 0.2rem;
    width: 100%;
    box-sizing: border-box;
    padding-bottom: 0.2rem;
    background: #fff;
    margin-top: 0.3rem;

    & > div:nth-child(1) {
      & > p:nth-child(1) {
        padding-top: 0.25rem;
        font-size: 0.3rem;
        font-family: PingFang-SC-Bold;
        font-weight: bold;
        color: rgba(81, 148, 231, 1);
        display: flex;
        span {
          max-width: 2.5rem;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }
        button {
          border: 0;
          font-size: 0.24rem;
          width: 1.6rem;
          height: 0.4rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: white;
          margin-left: 0.25rem;
          padding: 0;
        }
      }

      p {
        font-size: 0.24rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(102, 102, 102, 1);
        padding-top: 0.15rem;
      }
    }

    & > div:nth-child(2) {
      button {
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        color: white;
        border: 0;
        border-radius: 0.06rem;
        width: 1.2rem;
        height: 0.58rem;
        background: #d1d1d1;
        font-size: 0.3rem;
        background: #ff9000;
      }
    }

    .inven {
      // padding-top: 0.2rem;
      text-align: right;
      font-size: 0.5rem;
      display: flex;
      flex-direction: column;
      align-items: flex-end;

      p,
      div {
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(102, 102, 102, 1);
        text-align: right;

        button {
          color: rgba(255, 255, 255, 1);
          border: 0;
          border-radius: 0.06rem;
          width: 1.2rem;
          height: 0.58rem;
          background: rgba(209, 209, 209, 1);
          margin-top: 0.23rem;
          font-size: 0.3rem;
        }
      }
    }
  }

  .dealerInfo {
    margin-top: 0.1rem;
  }

  .dealerInfo:nth-child(2) {
    margin-top: 0;
  }
}
</style>
