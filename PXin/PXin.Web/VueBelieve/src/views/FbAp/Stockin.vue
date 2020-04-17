<!-- 经销商兑换码 -->
<template>
  <div class="exchangeCode" id='exchangeCode'>
    <div class="userInfo">
      <div>
        <img :src="user.appphoto? user.appphoto:'http://global.ckv-test.sulink.cn/images/err/photo.png'"
             alt />
      </div>
      <div>
        <p>{{user.nodename}}</p>
        <p>
          {{user.identitydesc}}
          <span></span>
        </p>
      </div>
    </div>
    <!-- line -->
    <div style="height:.3rem; background:rgba(244,244,244,1);"></div>
    <!-- 选择类型 -->
    <div class="selectType">
      <p>
        <span>选择类型</span>
        <!-- <span>我的SV库存</span> -->
      </p>
      <div @click="selectTypeIndex=index;ruleid=item.id;payDos=item.dos;subtitle=item.subtitle"
           :class="selectTypeIndex==index && 'active'"
           class="Type"
           v-for="(item,index) of typeList"
           :key="index">
        <p>
          <span>{{item.title}}</span>
        </p>
        <p>{{item.subtitle}}</p>
        <img v-show="selectTypeIndex==index" src="@/assets/images/check.png" alt />
      </div>
      <!-- 九月活动 -->
      <div v-show="isSept"
           class="Type sept"
           :class="isSeptStock && 'septActive'"
           @click="isSeptStock=!isSeptStock;">
        <p>
          <span>{{promotionType.title}}</span>
        </p>
        <p>{{promotionType.subtitle}}</p>
        <img v-show="isSeptStock" src="@/assets/images/check.png" alt />
        <div class="septtext">九月促销活动</div>
      </div>
    </div>
    <!-- <div data-v-6efed451 style="height: 0.3rem; background: rgb(244, 244, 244);"></div> -->
    <!-- 九月促销活动 -->
    <div class="activety" v-show="isactivity">
      <span>参与九月促销活动</span>
      <img @click="isSeptClick"
           :src="isSept?require('@/assets/images/ic_on.png'):require('@/assets/images/ic_off.png')"
           alt />
    </div>
    <!-- 总计 -->
    <div class="toggle">
      <div>
        <div>总计</div>
        <div class="toggleRight">
          <p>{{payDos}}</p>
          <p>已选择兑换{{subtitle}}</p>
        </div>
      </div>
    </div>
    <!-- btn -->
    <div class="btn">
      <button @click="pay">确认兑换</button>
    </div>

    <div class="hint" v-if="user.typeid==4">
      <h3>购买须知：</h3>
      <p>1.充值商使用DOS兑换SVC批发码及零售码为组合套餐，不允许拆分；</p>
      <p>2.兑换成功后您将获得相应额度的批发码和零售码；</p>
      <p>3.兑换成功获得的批发码可批发给代理人；</p>
      <p>4.兑换成功获得的零售码可零售给任何人；</p>
      <p>5.您一旦点击《确认兑换》，会跳转到码库 APP支付页面，请确保您已安装并且登录码库账户；</p>
      <p>6.在码库 APP支付相应DOS后，SVC批发码及零售码会自动到您的账户内，将不能操作退货，请谨慎选择；</p>
    </div>

    <div class="hint" v-if="user.typeid==5">
      <h3>购买须知：</h3>
      <p>1.您只能从您的充值商处操作兑换零售码,如果提示兑换失败,请您联系所属充值商；</p>
      <p>2.您兑换的是"零售码",兑换成功后将获得相应额度的SVC零售码；</p>
      <p>3.兑换成功后,您可以将零售码可零售给任何人；</p>
      <p>4.您一旦点击《确认兑换》,会跳转到码库 APP支付页面,请确保您已安装并且登录码库账户；</p>
      <p>5.在码库 APP支付相应DOS后,SVC零售码会自动到您的账户内,将由您的充值商收取相应的DOS,此操作不允许操作退货，请谨慎选择；</p>
    </div>

    <div ref='model' class='model' v-show="popup">
    <div class="septPopup">
        <h3>
          <span style="width:.36rem;height:.36rem;"></span>
          <span>活动须知</span>
          <img @click='popup=false;isSept=false;isSeptStock=false;' src="@/assets/images/exchange_delete.png" alt />
        </h3>
        <div class="hint">参与9月促销活动，需绑定优谷账号，请仔细阅读《2019相信助力优享汇活动公告》并勾选同意，兑换时将直接从你绑定的优谷账号扣减DP，请保证DP余额充足；</div>
        <div class="popInt">
          <span>优谷账号:</span>
          <input class="needsclick" @blur="onBlue"  ref='user'  placeholder="请输入优谷账号" maxlength="15" type="text" v-model="nodecode" oninput="value=value.replace(/[^\w\.\/]/ig,'')" />
        </div>
        <div class="popInt">
          <span>支付密码:</span>
          <div class='setPwd'>
            <input class="needsclick" @blur='onBlue'  ref='pwd'  placeholder="请输入支付密码" type="text" v-model="pwd" maxlength="6" oninput="value=value.replace(/[^\d\.\/]/ig,'')" />
            <span>{{SetPwdVal}}</span>
          </div>
        </div>
        <div class="btn" >
          <button @click="VerifyPwd">确定</button>
        </div>
        <div class="agreement" @click='agree=!agree'>
          <img :src="agree?require('@/assets/images/select_sel.png'):require('@/assets/images/select_nor.png')"
               alt />
          我已同意并阅读
          <span @click='goAgree'>《2019相信助力优享汇活动公告》</span>
        </div>
    </div>
    </div>

  </div>
</template>

<script>
// 经销商兑换码
import { Dialog, Popup } from "vant";
import {
  GetUserInfo,
  GetExchangeTypeInfo,
  ExChangeRechargeCode,
  VerifyPwd
} from "@/api/getFbApData";
export default {
  data() {
    return {
      show: true,
      user: {}, //用户信息
      typeList: [], //基本类型规则
      promotionType:{},//促销类型
      selectTypeIndex: 0, //选择类型 下标
      ruleid: "", //已选类型id
      payDos: "0DOS",
      subtitle: "",
      disab: false, //是否禁用确认兑换
      //是否勾选
      isSeptStock: false,
      popup:false,
      //是否参与
      isSept: false,
      //是否勾选协议
      agree: false,
      nodecode:'',//优谷帐号
      pwd:'',//优谷密码
      isactivity:'',//是否开启活动
      hidshow:false,
    };
  },
  created() {
    this.GetUserInfo(); //获取头像名称，余额
    this.HasDisab() ;
 
  },
  mounted() {
    this.initFn() ; 
    this.dosPayResult() ; // dos 回调
  },
  computed: {
    SetPwdVal(){
      let str='';
      for(let i=0;i<this.pwd.length;i++){
        str+='*';
      }
      return str;
    },
    total() {
      if (this.typeList.length != 0) {
        return (
          this.typeList[this.selectTypeIndex].cheaper +
          "+" +
          this.typeList[this.selectTypeIndex].dos
        );
      }
    },
    total2() {
      if (this.typeList.length != 0) {
        return "已选择兑换" + this.typeList[this.selectTypeIndex].subtitle;
      }
    }
  },
  components: {
    [Dialog.Component.name]: Dialog.Component
  },
  methods: {
    initFn(){
      //android
      const docmHeight = document.body.clientHeight; // 默认屏幕高度
      window.onresize = () => {
          var nowHeight = document.body.clientHeight; // 实时屏幕高度
          if (docmHeight !== nowHeight) {
            document.getElementById("exchangeCode").classList.add("focusState");
          } else {
            document.getElementById("exchangeCode").classList.remove("focusState");
          }
      };

      if(window.sessionStorage.SeptNodecode){
        this.nodecode = window.sessionStorage.SeptNodecode;
        this.popup=true;
      }
      if(window.sessionStorage.pwd){
        this.pwd = window.sessionStorage.pwd
        this.popup=true;
      }
    },
    dosPayResult(){
      let _this = this;
      window.dosPayResult = function(obj) {
        try {
          var ret = JSON.parse(obj);
          if (ret.result == undefined) {
            throw new Error("解析错误");
          }
          if (ret.result <= 0) {
            _this.Toast(ret.message);
            return;
          } else {
            _this.Toast("支付成功");
            setTimeout(() => {
              _this.$router.go(-1);
            }, 1000);
          }
        } catch (e) {
          _this.Toast("支付异常:" + obj);
        }
      };
    }, 
    HasDisab(){
      let arr = ['未审核', '审核拒绝' ,'冻结'] ;
      if(arr.some(item => this.$route.query.statusdesc == item)){
        this.Toast("您的账号当前是" +JSON.parse(this.$route.query.statusdesc) +"，不可以兑换码，请向所在区域充值商咨询。");
        this.disab = true;
      }
    },
    //ios兼容固定底部元素
    onBlue(){
      window.scrollTo(0,document.body.clientHeight)
    },
    goAgree(){
      window.sessionStorage.SeptNodecode = this.nodecode;
      window.sessionStorage.pwd = this.pwd;
      this.$router.push("/agree");
    },
    //是否参与
    isSeptClick() {
      if (this.isSept) {
        Dialog({
          message: "已绑定的优谷帐号不支持取消绑定"
        });
        return;
      }
      this.isSept = !this.isSept;
      this.popup=!this.popup;
      this.isSeptStock=true;
    },
    async GetUserInfo() {
      let result = await GetUserInfo(
        JSON.parse(sessionStorage.userParam),
        this.$route.query.infoid
      );
      if (result.result > 0) {
        this.isactivity=result.data.isactivity;
        if(this.isactivity){
          this.isSept=result.data.isbind;
          this.isSeptStock=this.isSept;
          this.nodecode=this.isSept?result.data.nodecode:'';

          if(!this.nodecode && window.sessionStorage.SeptNodecode){
            this.nodecode =window.sessionStorage.SeptNodecode;
          }
        }else{
          this.isSept=false;
          this.isSeptStock=false;
        }
        this.user = result.data;
        this.GetExchangeTypeInfo(); //获取兑换类型信息
      } else {
        this.Toast(result.message);
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
      }
    },
    async GetExchangeTypeInfo() {
      let result = await GetExchangeTypeInfo(
        JSON.parse(sessionStorage.userParam),
        this.$route.query.infoid
      );
      if (result.result > 0) {
        result.data.forEach(el => {
          if(el.ispromotion){
            this.promotionType=el;
          }else{
            this.typeList.push(el);
          }
        });
        this.title = result.data[0].title;
        this.payDos = result.data[0].dos;
        this.subtitle = result.data[0].subtitle;
        this.ruleid = result.data[0].id; //默认提交第1个类型id
      } else {
        this.Toast(result.message);
      }
    },
    async VerifyPwd(){
      if (!this.nodecode){
        this.Toast("请输入优谷帐号");
        return;
      }
      if (!this.pwd){
        this.Toast("请输入优谷支付密码");
        return;
      }
      if(!this.agree){
        this.Toast("请同意相关协议");
        return;
      }
      let result = await VerifyPwd(
        {...JSON.parse(sessionStorage.userParam),nodecode:this.nodecode,typeid:2,pwd: btoa(this.pwd)}
      );
      if (result.result > 0) {
        this.popup=false;
      } else {
        this.Toast(result.message);
      }
    },
    //支付
    async pay() {
      if (!this.ruleid) {
        this.Toast("请重新尝试~");
        return;
      }
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        ruleid: this.ruleid, //规则Id,
        ispromotion:this.isSeptStock&&this.isSept,//是否参与促销活动
        nodecode:this.nodecode
      };
      let result = await ExChangeRechargeCode(data, this.ruleid);
      if (result.result > 0) {
        try {
          AppNative.blJsTunedupNativeWithTypeParamSign(
            1003,
            result.data.chargestr,
            result.data.sign
          );
        } catch (e) {
          this.Toast.fail("调起码库支付失败!");
        }
        this.show = false;
      } else {
        this.Toast(result.message);
      }
    }
  },
};
</script>

<style scoped lang='scss'>

.focusState {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
}
.model{
  height: 100%;
  position: fixed;
  width: 100%;
  background:rgba(0,0,0,.5);
  z-index: 2000;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  display: flex;
  align-items: flex-end;
}
.exchangeCode::-webkit-scrollbar { display: none }
.agreement {
  padding-top: .2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: PingFang-SC-Medium;
	font-size: 0.24rem;
	font-weight: normal;
	font-stretch: normal;
	line-height: 0.69rem;
	letter-spacing: 0rem;
	color: #666666;
  img {
    width: 0.28rem;
    height: 0.28rem;
    margin-right: .24rem;
  }
  span{
    color: #2ea2fa;
  }
}
.septPopup  {
  // position: absolute;
  // bottom: 0;
  background: #fff;
  border-radius: 0.5rem 0.5rem 0 0;
  padding: 0 0.3rem;
  box-sizing: border-box;
  padding-bottom: 0.6rem;
  z-index: 2001;
  .btn {
    padding: 0;
    padding-top: 0.6rem;
    margin-top: 0;
  }
  h3 {
    height: 0.88rem;
    margin: 0;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    font-weight: bold;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 0.02rem solid #d1d1d1;
    img {
      width: 0.36rem;
      height: 0.36rem;
    }
  }
  .hint {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
    padding: 0 0.15rem;
    padding-top: 0.35rem;
    margin: 0;
  }
  .setPwd{
    position: relative;
    input{
      font-size:.26rem;
    }
    span{
      position: absolute;
      font-size:.335rem !important;
      top: 0;
      left: 0.5rem;
      z-index: 3000;
      // height: 100%;
      background-color: #f7f7fc;
    }
  }
  .popInt {
    margin-top: 0.3rem;
    background-color: #f7f7fc;
    border-radius: 0.04rem;
    padding: 0 0.25rem;
    height: 0.88rem;
    display: flex;
    align-items: center;
    span {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #333333;
    }
    input {
      border: 0;
      flex: 1;
      height: 100%;
      box-sizing: border-box;
      padding-left: 0.5rem;
      background-color: #f7f7fc;
    }
    input::placeholder {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }
  }
}
.van-dialog {
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.48rem;
  letter-spacing: 0.01rem;
  color: #333333;
}
.activety {
  margin-top: 0.3rem;
  height: 1rem;
  padding: 0 0.3rem;
  background-color: #f7f7fc;
  font-family: PingFang-SC-Bold;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #2ea2fa;
  display: flex;
  align-items: center;
  justify-content: space-between;
  img {
    width: 1rem;
    height: auto;
  }
}
.septtext {
  font-family: PingFang-SC-Bold;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #fcaf56;
  position: absolute;
  right: 0.2rem;
  bottom: 0.1rem;
}
.septActive {
  background: #ffead2 !important;
  border: 0.02rem solid #ffb90f !important;
}
.btn {
  width: 100%;
  padding: 0 0.3rem;
  box-sizing: border-box;
  display: flex;
  margin-top: 0.9rem;
  button {
    width: 100%;
    border: 0;
    height: 0.88rem;
    background: #2ea2fa;
    border-radius: 0.06rem;
    font-size: 0.3rem;
    font-family: PingFang-SC-Bold;
    font-weight: bold;
    color: rgba(254, 254, 254, 1);
  }
}

.toggleRight {
  font-family: PingFang-SC-Bold;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #ff3838;
  font-weight: bold;
  p {
    text-align: right;
  }
  p:last-child {
    color: #333333;
  }
}

.toggle {
  margin-top: 0.02rem;
  height: 1.28rem;
  background-color: #f7f7fc;
  padding: 0 0.3rem;

  font-size: 0.3rem;
  font-family: PingFang-SC-Bold;
  font-weight: bold;
  color: rgba(51, 51, 51, 1);

  & > div {
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
    // border-bottom: 0.01rem solid rgba(209, 209, 209, 1);
  }
}

.active {
  border: 0.02rem solid #ffb90f !important;
  background: #ffead2 !important;
}

.fixed {
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  height: 1rem;
  position: fixed;
  bottom: 0;

  & > div {
    height: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;

    button {
      display: flex;
      align-items: center;
      justify-content: center;
      border: 0;
      height: 0.72rem;
      background-color: #2ea2fa;
      border-radius: 0.04rem;
      font-family: PingFang-SC-Bold;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      color: #fefefe;
    }
  }
}

.info {
  & > div {
    width: 100%;
    /* background-color: #fafafa; */
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #666666;
    padding: 0.47rem 0.28rem 0.98rem 0.38rem;
    padding: 0.2rem 0.3rem;
    box-sizing: border-box;
  }
}
p {
  margin: 0;
}

.disab {
  pointer-events: none;
  cursor: default;
  background: #ccc !important;
}

.van-popup {
  border-radius: 0.1rem;
}
.hint {
  padding: 0 0.3rem;
  margin-top: 0.6rem;
  padding-bottom: 1rem;
  h3 {
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #333333;
    margin: 0;
    font-weight: bold;
  }

  p {
    padding-top: 0.2rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #999999;
  }
}

/* 经销商兑换码 */
.exchangeCode {
  height: 100%;
  background: #fff;
  overflow: scroll;
  -webkit-overflow-scrolling: touch;
  position: relative;
  .convert {
    padding-left: 0.2rem;
    /* height: 3rem; */
    padding-bottom: 0.2rem;

    & > div {
      height: 0.88rem;
      display: flex;
      justify-content: space-between;
      border-bottom: 0.01rem solid #d1d1d1;
      align-items: center;

      font-size: 0.3rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;
      color: rgba(51, 51, 51, 1);

      span:nth-child(2) {
        color: #ff3030;
      }

      img {
        padding-right: 0.2rem;
        width: 0.36rem;
        height: 0.36rem;
      }
    }

    & > div.sum {
      height: 1.08rem;

      p {
        font-size: 0.3rem;
        font-family: PingFang-SC-Bold;
        font-weight: bold;
        color: rgba(51, 51, 51, 1);
      }

      div {
        padding-right: 0.2rem;

        p {
          font-size: 0.24rem;
        }

        & > p:nth-child(1) {
          color: rgba(255, 48, 48, 1);
        }

        & > p:nth-child(2) {
          color: rgba(255, 185, 15, 1);
        }
      }
    }
  }

  .selectType {
    background: rgba(255, 255, 255, 1);
    padding: 0.3rem 0.3rem;
    img {
      width: 0.6rem;
      height: 0.6rem;
      position: absolute;
      right: 0rem;
      top: 0rem;
    }
    .Type {
      background-color: #fff;
      border-radius: 0.04rem;
      border: 0.02rem solid transparent;
      height: 1.48rem;
      padding: 0.3rem 0;
      width: 100%;
      box-sizing: border-box;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: space-between;
      position: relative;
      border: solid 0.02rem #d1d1d1;
      p:nth-child(1) {
        font-family: PingFang-SC-Bold;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #9c8054;
      }

      p:nth-child(2) {
        font-family: PingFang-SC-Bold;
        font-size: 0.28rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
      }
    }

    .Type:nth-of-type(n + 2) {
      margin-top: 0.2rem;
    }

    & > p {
      font-size: 0.3rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;
      color: rgba(51, 51, 51, 1);
      padding-bottom: 0.2rem;
      display: flex;
      justify-content: space-between;
    }

    & > div:nth-child(2) {
      margin-top: 0;
    }
  }

  .userInfo {
    height: 1.8rem;
    /* background: rgba(156, 128, 84, 1); */
    background: #fff;
    padding-left: 0.2rem;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    div {
      img {
        height: 1rem;
        width: 1rem;
        border-radius: 50%;
      }

      p {
        font-size: 0.28rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(51, 51, 51, 1);
        padding: 0.05rem 0;
        display: flex;
        align-items: center;

        // span {
        //   margin-left: .1rem;
        //   display: inline-block;
        //   width: .15rem;
        //   height: .15rem;
        //   border-right: .01rem solid #333;
        //   border-top: .01rem solid #333;
        //   transform: rotate(45deg);
        // }
      }
    }

    div:nth-child(2) {
      padding-left: 0.2rem;
    }
  }
}
</style>
