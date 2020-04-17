<!-- 充值V -->
<template>
  <div class="addv">
    <div class="addhead">
      <p>
        <span @click="$router.push('/AddDetail')">充值记录</span>
      </p>
      <div class="desc">
        <p>{{user.v}}</p>
        <p>
          {{$t('m.accountbalance')}}（V）
          <img
            @click="show=!show"
            src="@/assets/images/personal_wallet_p_issue@2x.png"
            alt
          />
        </p>
      </div>
    </div>
    <!-- 充值 -->
    <div class="addcount">
      <div class="addleft">
        <span>充值</span>
        <input v-model="amount" type="text" placeholder="请输入数量" />
      </div>
      <div style="color:#ffae00">V</div>
    </div>
    <!-- 其他选择 -->
    <div class="rests">
      <p>其他选择</p>
      <div class="type">
        <div
          :class="active==index && 'active' "
          @click="ChoiceAmount(index)"
          v-for="(item,index) of amounts"
          :key="index"
        >
          <p>{{item}}V</p>
          <p>需付:{{(item/10).toFixed(1)}}SV</p>
        </div>
      </div>
    </div>
    <!-- 按钮 -->
    <div class="btn">
      <button @click="passwordshow=true">确认支付{{amount.toFixed(1)}}SV</button>
    </div>

    <!-- 支付 -->
    <div class="password" v-if="passwordshow">
      <div class="passwordList" ref="passwordList">
        <p>请输入您的支付密码</p>
        <!-- 支付金额这里 -->
        <!-- <p>xxxxxUV</p> -->
        <div>
          <div v-for="(item,index) of passwordKey" :key="index">{{item.title}}</div>
        </div>
      </div>
      <van-number-keyboard :show="passwordshow" extra-key="关闭" @input="onInput" @delete="onDelete" />
    </div>
    <!-- 弹框 -->
    <van-popup v-model="show">
      <div class="model">
        <div class="modelT">
          <img src alt />
          {{$t('m.whatvp')}}
          <img
            @click="show=false"
            src="@/assets/images/dynamic_delete@2x.png"
            alt
          />
        </div>
        <p>{{$t('m.V')}}</p>
        <p>{{$t('m.P')}}</p>
      </div>
    </van-popup>
  </div>
</template>

<script>
import { GetUserInfo_Fri, ChargeVDian } from "@/api/myData.js";
import { Base64 } from "@/config/utils.js";
import { fail } from "assert";
export default {
  data() {
    return {
      user: [],
      amounts: [10, 20, 30, 50, 100, 200],
      amount: 10,
      active: 0, //默认选中第一个
      // PV是什么? 弹出框
      show: false,
      //密码弹出框
      passwordshow: false,
      //密码值index -1等于没有输入
      passwordIndex: -1,
      //密码值
      passwordKey: [
        //title=* num=数字
        { title: "", num: "" },
        { title: "", num: "" },
        { title: "", num: "" },
        { title: "", num: "" },
        { title: "", num: "" },
        { title: "", num: "" }
      ]
    };
  },
  methods: {
    ChoiceAmount: function(index) {
      this.active = index;
      this.amount = this.amounts[index];
    },
    //密码框 输入触发
    onInput(num) {
      if (num != "关闭") {
        this.passwordIndex++; //密码下标
        this.passwordKey[this.passwordIndex].title = "●";
        this.passwordKey[this.passwordIndex].num = num;
        //密码6位 发送请求
        if (this.passwordIndex == 5) {
          let paypwd = "";
          this.passwordKey.forEach(element => {
            paypwd += "" + element.num;
          });
          ChargeVDian(
            {
              price: this.amount,
              paytype: 1,
              paypwd: Base64.encode(paypwd)
            },
            data => {
              for (let item of this.passwordKey) {
                item.title = "";
                item.num = "";
              }
              this.passwordIndex = -1;
              this.$toast(data.message);
              if (data.result < 1) {
                return;
              }
              this.passwordshow = false;
              this.user.v += this.amount;
            }
          );
        }
      } else {
        //点击关闭
        for (let item of this.passwordKey) {
          item.title = "";
          item.num = "";
        }
        this.passwordIndex = -1;
        this.passwordshow = false;
      }
    },
    //密码框 删除键
    onDelete() {
      if (this.passwordIndex == -1) return;
      this.passwordKey[this.passwordIndex].title = "";
      this.passwordKey[this.passwordIndex].num = "";
      this.passwordIndex--; //密码下标
    }
  },
  created() {
    GetUserInfo_Fri(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      if (data.data.length > 0) {
        this.user = data.data[0];
      }
    });
  },
  updated() {
    //设置密码框高度
    if (document.getElementsByClassName("van-number-keyboard__body")[0]) {
      let height = document.getElementsByClassName(
        "van-number-keyboard__body"
      )[0].offsetHeight;
      this.$refs.passwordList.style.bottom = height + "px";
    }
  },
};
</script>

<style scoped lang='scss'>
.van-popup {
  border-radius: 0.1rem;
}

.model {
  width: 5.8rem;
  height: 3.55rem;
  background-color: #ffffff;
  border-radius: 0.1rem;
  padding: 0.32rem 0.24rem;
  box-sizing: border-box;
  p {
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;
  }
}
.modelT {
  padding: 0 0.2rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
  display: flex;
  justify-content: space-between;
  align-items: center;

  img {
    width: 0.22rem;
    height: 0.22rem;
  }
}

.passwordList {
  position: fixed;
  left: 0;
  width: 100%;
  height: 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: #fff;

  & > div {
    /* margin-top: .1rem; */
    width: 4.8rem;
    height: 0.8rem;
    box-sizing: border-box;
    display: flex;

    & > div {
      background: #f1f1f1;
      display: flex;
      justify-content: center;
      align-items: center;
      width: 0.8rem;
      border: 0.01rem solid #bbb;
      box-sizing: border-box;
      font-size: 0.28rem;
    }

    & > div:nth-of-type(n + 2) {
      border-left: 0;
    }
  }

  p {
    padding-bottom: 0.1rem;
    margin: 0;
    font-family: MicrosoftYaHei;
    font-size: 0.21rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.password {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.3);
}

/deep/ .van-key {
  font-size: 14px !important;
}

.btn {
  width: 100%;
  position: fixed;
  padding: 0 0.3rem;
  bottom: 0.44rem;
  box-sizing: border-box;
  display: flex;
  button {
    border: 0;
    width: 100%;
    height: 0.8rem;
    line-height: 0.8rem;
    background-color: #ffae00;
    border-radius: 0.05rem;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}
.active {
  background: #2ea2fa !important;
  p {
    color: #ffffff !important;
  }
}
.rests {
  padding: 0 0.3rem;

  .type {
    width: 100%;
    box-sizing: border-box;
    display: flex;
    flex-wrap: wrap;

    & > div {
      padding: 0.2rem 0;
      box-sizing: border-box;
      margin-left: 0.15rem;
      width: 2.2rem;
      height: 1.06rem;
      background-color: #fff;
      border-radius: 0.05rem;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      p {
        margin: 0;
        padding: 0;
        font-family: PingFang-SC-Regular;
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #2ea2fa;
      }
      p:first-child {
        font-size: 0.36rem;
      }
    }
    & > div:nth-child(1),
    & > div:nth-child(4) {
      margin-left: 0;
    }
    & > div:nth-of-type(n + 4) {
      margin-top: 0.3rem;
    }
  }

  & > p {
    margin: 0;
    height: 0.7rem;
    line-height: 0.7rem;
    font-family: PingFang-SC-Regular;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.addv {
  height: 100%;
  background: #f0f0f0;
}

.addleft {
  display: flex;
  align-items: center;
  width: 80%;
  height: 100%;

  input {
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
    border: 0;
    margin: 0;
    padding: 0;
    height: 100%;
    width: 80%;
    padding-left: 0.3rem;
  }
}

.addcount {
  margin-top: 0.24rem;
  border-bottom: 1px solid #dddddd;
  background: #fff;
  width: 100%;
  box-sizing: border-box;
  display: flex;
  justify-content: space-between;
  padding: 0 0.3rem;
  height: 1rem;
  align-items: center;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #1a1a1a;
}

.addhead {
  background: #2ea2fa;
  padding-bottom: 0.75rem;
  padding-top: 0.2rem;
  .desc {
    // height: 1.2rem;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    align-items: center;

    p {
      margin: 0;
      font-family: PingFang-SC-Regular;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
      img {
        position: relative;
        bottom: 0.2rem;
        right: 0.1rem;
        width: 0.3rem;
        height: 0.3rem;
      }
    }

    p:nth-child(1) {
      display: flex;
      font-size: 0.6rem;
    }

    p:nth-child(2) {
      padding-top: 0.2rem;
      padding-left: 0.3rem;
    }
  }

  p {
    margin: 0;
    display: flex;
    justify-content: flex-end;

    span {
      margin-right: 0.45rem;
      height: 0.43rem;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: rgba(255, 255, 255, 0.2);
      border-radius: 0.22rem;
      /* opacity: 0.2; */
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      line-height: 0.48rem;
      letter-spacing: 0rem;
      color: #ffffff;
      padding: 0 0.15rem;
    }
  }
}
</style>
