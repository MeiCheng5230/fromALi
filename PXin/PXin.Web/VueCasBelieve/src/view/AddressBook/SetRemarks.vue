<!-- 设置昵称 -->
<template>
  <div class="SetNotes">
    <div class="container">
      <!-- <div class="Notes">昵称</div> -->
      <div class="ipt">
        <input v-model="remarks" maxlength="12" ref="ipt" type="text" placeholder="请输入昵称" />
      </div>
    </div>
    <div class="btn">
      <button :class="remarks?'active':''" @click="submit">{{$t('m.finish')}}</button>
    </div>
  </div>
</template>

<script>
import { UpdateMyFriendInfo } from "@/api/getChatData";
export default {
  data() {
    return {
      remarks: null //用户输入文本
    };
  },
  mounted() {
    this.$refs.ipt.focus(); //页面加载获取焦点
  },
  watch: {
    remarks(newVal, fVal) {
      if (newVal != "") {
      }
    }
  },
  methods: {
    //底部按钮 完成提交
    submit() {
      if (!this.remarks) {
        this.$toast("昵称不能为空");
        return;
      }
      let data={
          ...JSON.parse(sessionStorage.userParam),
          paramtype:1,
          paramvalue:this.remarks,
          usercode:this.$route.query.usercode
      };
      UpdateMyFriendInfo(data, res => {
        if (res.result > 0) {
            sessionStorage.setItem('newRemarks',this.remarks);
            this.$toast("设置成功");
        } else {
            this.$toast("设置失败");
        }
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
      });
    }
  },
  created() {
    this.remarks = this.$route.query.remarks;
  }
};
</script>

<style lang='scss' scoped>
.SetNotes {
  height: 100%;
  box-sizing: border-box;
  background-color: #f2f2f2;
  .active {
    color: rgba(255, 255, 255, 1) !important;
    background: #2ea2fa !important;
  }
  .btn {
    box-sizing: border-box;
    width: 100%;
    padding: 0 0.3rem;
    margin-top: 0.92rem;
    display: flex;
    button {
      display: inline-block;
      width: 100%;
      height: 0.88rem;
      background-color: rgba(46, 162, 250, 0.5);
      border-radius: 0.04rem;
      border: 0;
      font-family: PingFang-SC-Bold;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: rgba(255, 255, 255, 0.5);
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }

  .container {
    .Notes {
      height: 0.8rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #666666;
      display: flex;
      align-items: center;
      padding-left: 0.3rem;
    }

    .ipt {
      height: 0.88rem;
      display: flex;

      input {
        height: 100%;
        width: 100%;
        box-sizing: border-box;
        border: 0;
        outline: none;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #666666;
        padding-left: 0.3rem;
      }
    }
  }
}
</style>
