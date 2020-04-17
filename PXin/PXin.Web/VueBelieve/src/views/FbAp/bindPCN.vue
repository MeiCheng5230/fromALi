<template>
  <div class="bindpcn">
    <table></table>
    <div>
      <span>账号</span>
      <input ref='user' maxlength='20'  type="text" v-model='user' placeholder="请输入本人已实名认证的PCN账号" />
    </div>
    <div>
      <span>密码</span>
      <div class="password">
        <input maxlength='20' placeholder='请输入密码' :type="type" v-model='pwd'  />
        <img
          @click='SwitchPwd'
          :src="isHide?require('@/assets/images/showpwd.png'):require('@/assets/images/hidepwd.png')"
          alt
        />
      </div>
    </div>
    <!-- 提交 -->
    <div class="submit">
      <button :class='user.length<6 && "disabled"' @click='BindPcnAcount'>提交</button>
    </div>
  </div>
</template>
<script>
import { BindPcnAcount } from '@/api/getFbApData';
export default {
  data() {
    return {
      isHide: false,
      type:'password',
      user:'',
      pwd:'',
      isSend:false,
    };
  },
  methods:{
     SwitchPwd(){
        this.isHide = !this.isHide;
        this.type = this.type== 'password' ? this.type = 'text' : this.type = 'password' ; 
     },
     async BindPcnAcount(){
        if(!this.pwd){
          this.$toast('密码不可为空');
          return ;
        }
        let data = {
           ...this.$global.userInfo,
            nodecode : this.user,
            pwd:this.pwd
        }
        let res = await BindPcnAcount(data);
        if(res.result == -2 || res.result > 0 ){
          this.$toast('绑定成功');
          //10月活动 存储PCN绑定的账号
          sessionStorage.OctActivityPcnUser =  this.user ;
          setTimeout(() => {
            this.$router.go(-1);
          }, 1000);
        }else{
           this.$toast(res.message);
           this.user ='';
           this.pwd = '';
           this.$refs.user.focus();
           return ;
        }
     }
  }
};
</script>
<style lang="scss" scoped>
.disabled{
   pointer-events:none;
   background-color: #90CAF6 !important;
}
.bindpcn {
  height: 100%;
  background: #f7f7f7;
  .submit {
    margin-top: 2rem;
    background: none;
    button{
       width: 100%;
       height: .8rem;
       border: 0;
       border-radius: .06rem;
      background-color: #2ea2fa;
      font-family: PingFang-SC-Bold;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      color: #ffffff;
    }
  }
  input {
    margin-left: 0.3rem;
    flex: 1;
    height: 80%;
    border: 0;
    padding: 0;
  }
  input::placeholder {
    color: #999;
  }
  & > div {
    margin-top: 0.3rem;
    display: flex;
    align-items: center;
    height: 0.8rem;
    background: #fff;
    padding: 0 0.3rem;
  }
  .password {
    display: flex;
    flex: 1;
    height: 100%;
    justify-content: space-between;
    align-items: center;
    img {
      max-width: 0.36rem;
      max-height: 0.36rem;
    }
  }
}
</style>