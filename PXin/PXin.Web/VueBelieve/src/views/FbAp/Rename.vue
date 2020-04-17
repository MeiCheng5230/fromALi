<template>
  <div class='content' >
    <table></table>
    <div class='Int'>
      <input  @input='change($event)' ref="int" v-model='name' type="text" placeholder="2—12位,不支持特殊字符" minlength='2' maxlength="12">
      <span v-show='name'><img @click='name=""' src="@/assets/images/detele.png" alt="" /></span>
    </div>

    <div style='padding: .2rem 0 0 .3rem; font-size:0.24rem;font-family:PingFang-SC-Medium;font-weight:500;color:rgba(153,153,153,1);'>为保护您的隐私，请避免使用手机号作为昵称</div>

    <div :style='!name && "background:#D1D1D1;"' class='btn' @click='UpdateUserJxsName'>确定</div>

  </div>
</template>

<script>
  import {
    UpdateUserJxsName
  } from '@/api/getFbApData';
  export default {
    data() {
      return {
        name: '',
        ischange: false, //是否改变
        changeName: ''
      }
    },
    created() {
      this.name = this.$route.query.name;
      this.changeName = this.name;
    },
    mounted() {
      this.$refs.int.focus();
    },
    watch: {
      name(newValue, formerValue) {
        newValue != this.changeName ? this.ischange = true : this.ischange = false;
        if (newValue == '') return;
        if (!this.name.match("^[a-zA-Z0-9_\u4e00-\u9fa5]+$")) {
          this.name = formerValue;
          this.Toast('请输入有效字符');
        }
      }
    },
    methods: {
      testCon(){
        if (!this.ischange) {
          this.Toast('请修改昵称~');
          return -1 ;
        }
        if (!this.name) {
          this.Toast('昵称不能为空~');
          return -1 ;
        }
        if (this.name.length>12 || this.name.length<2) {
          this.Toast('请输入2-12位有效字符');
          return -1 ;
        }
        return 1 ;
      },
      async UpdateUserJxsName() {
        if(this.testCon() == -1) return ;

        this.name = this.name.replace(/^\s+|\s+$/g, "");
        let data = {
          ...JSON.parse(sessionStorage.userParam),
          infoid: this.$route.query.infoid,
          jxsname: this.name
        }

        let result = await UpdateUserJxsName(data);
        if (result.result > 0) {
          this.Toast('修改成功！');
          this.$router.go(-1);
        } else {
          this.Toast(result.message);
        }
      }
    }
  }
</script>

<style scoped lang='scss'>
  .content {
    height: 100%;
    background: rgba(244, 244, 244, 1);

    .btn {
      margin: 0 auto;
      margin-top: .7rem;
      width: 7rem;
      height: .88rem;
      display: flex;
      align-items: center;
      justify-content: center;
      background: #2ea2fa;
      border-radius: 0.04rem;
      font-size: 0.3rem;
      font-family: PingFang-SC-Bold;
      font-weight: bold;
      color: rgba(255, 255, 255, 1);
    }

    .Int {
      width: 100%;
      height: .88rem;
      margin-top: .3rem;
      display: flex;
      background: #fff;

      input {
        padding-left: .3rem;
        box-sizing: border-box;
        border: 0;
        width: 80%;
        height: 100%;
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(51, 51, 51, 1);
      }

      span {
        display: block;
        width: 20%;
        background: #fff;
        display: flex;
        align-items: center;
        padding-right: .3rem;
        justify-content: flex-end;

        img {
          width: .36rem;
          height: .36rem;
        }
      }
    }
  }
</style>
