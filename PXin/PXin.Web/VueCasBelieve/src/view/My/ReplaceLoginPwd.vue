<!-- 修改登录密码 -->
<template>
  <div class="RepPwd">
    <div class="ipt">
      <table></table>
      <div class="iptInfo" v-for="(item,index) of pwd" :key="index">
        <input v-model="item.pwd" :type="item.type" :placeholder="item.placeholder" />
        <img
          :src="!item.isShow?require('@/assets/images/icon_hide@2x.png'):require('@/assets/images/icon_visible@2x.png')"
          alt
          @click="toggle(item,index)"
        />
      </div>
    </div>
    <!--  -->
    <div class="hint">
      <p v-if="type==1">登录密码由6-20位字母、数字、字符任意组合,请注意区分大小写</p>
      <p v-if="type==2">支付密码由6位数字组成</p>
      <div>
        <router-link :to="{path:'/ForgottenPwd',query:{type:type}}" tag="span">忘记密码？</router-link>
      </div>
    </div>
    <!-- btn -->
    <div class="btn">
      <button @click="submit" :class="btnColor && 'active'">确定</button>
    </div>
  </div>
</template>

<script>
import { ChangePwd } from "@/api/myData.js";
export default {
  data() {
    return {
      type: 0, //登录密码1，支付密码2
      //登录密码
      pwd: [
        {
          pwd: "",
          marks: "",
          placeholder: "请输入原密码",
          isShow: false,
          type: "password",
          visiblePwd: ""
        },
        {
          pwd: "",
          marks: "",
          placeholder: "请输入新密码",
          isShow: false,
          type: "password",
          visiblePwd: ""
        },
        {
          pwd: "",
          marks: "",
          placeholder: "请再次输入新密码",
          isShow: false,
          type: "password",
          visiblePwd: ""
        }
      ]

      // formerPwd:'',//旧秘密
      // newPwd:'',//新密码
      // twoPwd:'',//再次输入
      // }
    };
  },
  methods: {
    toggle: function(item, index) {
      item.isShow = !item.isShow;
      item.type = item.isShow ? "text" : "password";
      item.visiblePwd = item.isShow ? item.pwd : item.marks;
    },
    submit: function() {
      if (this.pwd[1].pwd != this.pwd[2].pwd) {
        this.$toast("两次新密码不一致");
        return;
      }
      let data = {
        type: this.type,
        oldpwd: btoa(this.pwd[0].pwd),
        newpwd: btoa(this.pwd[1].pwd)
      };
      ChangePwd(data, data => {
        this.$toast(data.message);
        if (data.result < 0) {
          return;
        }
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
      });
    }
  },
  computed: {
    btnColor() {
      for (let item of this.pwd) {
        if (!item.pwd) return false;
      }
      return true;
    }
  },
  created() {
    this.type = this.$route.query.type;
  }
};
</script>

<style scoped lang='scss'>
.van-popup {
  border-radius: 0.12rem;
}

.model {
  width: 5.3rem;
  height: 3rem;
  background-color: #ffffff;

  div:nth-child(1) {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 2.18rem;
    border-bottom: 0.02rem solid #f2f2f2;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #333333;
  }

  div:nth-child(2) {
    height: 0.8rem;
    display: flex;
    align-items: center;
    justify-content: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #2ea2fa;
  }
}

.active {
  background: #2ea2fa !important;
  color: #fff !important;
}

.btn {
  padding: 0 0.3rem;

  button {
    border: 0;
    width: 100%;
    height: 0.88rem;
    background-color: #90caf6;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #ffffff;
  }
}

.hint {
  font-family: PingFang-SC-Medium;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #666666;
  padding-top: 0.3rem;

  div {
    display: flex;
    justify-content: flex-end;
    padding: 0 0.3rem;

    span {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      color: #2ea2fa;
    }
  }

  p {
    margin: 0;
    padding: 0 0.2rem 0 0.3rem;
    /* white-space: nowrap; */
  }
}

.iptInfo {
  height: 0.88rem;
  margin-top: 0.3rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #fff;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #999999;
  padding: 0 0.3rem;

  img {
    width: 0.4rem;
    height: 0.24rem;
  }

  input {
    color: #333;
    height: 80%;
    flex: 1;
    border: 0;
  }

  input::placeholder {
    color: #999999;
  }
}

.iptInfo:nth-of-type(n + 2) {
  margin-top: 0.2rem !important;
}

.RepPwd {
  height: 100%;
  background: #f2f2f2;
}
</style>
