<!-- 朋友验证 -->
<template>
  <div class="test">
    <p>你需要发送验证申请，等对方通过</p>
    <div class="ipt">
      <input placeholder="请输入验证申请" v-model="val" type="text" maxlength="30" />
    </div>

    <div class="btn">
      <button @click="submit">发送</button>
    </div>
  </div>
</template>

<script>
import { GetUserInfo} from '@/api/myData';
import { AddFriend} from '@/api/getChatData';
export default {
  data() {
    return {
      //验证信息
      val: "",
      userCode:''
    };
  },
  created(){
    this.userCode=this.$route.query.userCode;
    GetUserInfo({...JSON.parse(sessionStorage.userParam)},res=>{
      if(res.result>0){
        this.val='我是'+res.data.nodename+',添加我为好友吧';
      }
    })
  },
  methods: {
    //发送按钮
    submit() {
      // 不能为空
      if (!this.val) {
        this.$toast("验证申请不能为空");
        return;
      }
      AddFriend({...JSON.parse(sessionStorage.userParam),usercode:this.userCode,remarks:this.val},res=>{
        this.$toast(res.message);
        if(res.result>0){
          setTimeout(() => {
            this.$router.go(-1);
          }, 1000);
        }
      });
      
    }
  }
};
</script>

<style scoped lang='scss'>
.btn {
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  display: flex;
  padding-top: 0.9rem;

  button {
    height: 0.88rem;
    background-color: #2ea2fa;
    border-radius: 0.04rem;
    width: 100%;
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
  }
}

.ipt {
  height: 0.88rem;
  display: flex;

  input {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
    border: 0;
    padding: 0;
    box-sizing: border-box;
    height: 100%;
    width: 100%;
    padding: 0 0.3rem;
  }
}

p {
  margin: 0;
  height: 0.8rem;
  line-height: 0.8rem;
  padding: 0 0.3rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;
}

.test {
  height: 100%;
  background: #f2f2f2;
}
</style>
