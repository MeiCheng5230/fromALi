<!-- 创建普通群聊 -->
<template>
  <div class="addchat">
    <!-- 点击跟换群头像 -->
    <div class="uploadHead">
      <div>
        <van-uploader :after-read="afterRead">
          <img :src="!uploadImg?require('@/assets/images/groupchat_head@2x.png'):uploadImg" alt />
        </van-uploader>
        <span>点击上传群头像</span>
      </div>
    </div>
    <!-- 群名称 -->
    <div class="groupname">
      <input v-model="groupName" type="text" placeholder="请填写群名称" />
    </div>
    <!-- 群描述 -->
    <div class="chatdesc">
      <p>填写群描述（可不填）</p>
      <div>
        <textarea v-model="descript" placeholder="群描述内容" name id cols="30" rows="10"></textarea>
      </div>
    </div>
    <!-- 提交 -->
    <div class="btn">
      <button @click="submit" :class="groupName && 'active'">确定</button>
    </div>
  </div>
</template>

<script>
import { UploadFile, CreateGroup } from "@/api/getChatData";
export default {
  //数据data
  data() {
    return {
      //群名称
      groupName: "",
      descript: "",
      //默认图片
      uploadImg: ""
    };
  },
  //方法 methods
  methods: {
    //上传头像
    afterRead(file) {
      this.uploadImg = file.content;
    },
    //按钮提交
    submit() {
      if (!this.groupName) {
        // this.$toast("请填写群名称");
        return;
      }
      if (!this.uploadImg) {
        this.$toast("请选择群聊头像");
        return;
      }
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        descript:
          this.descript == "" ? "该群主很懒，什么都没留下~" : this.descript,
        groupname: this.groupName,
        grouppic: this.uploadImg
      };
      CreateGroup(data, res => {
        if (res.result > 0) {
          this.$toast("成功");
          setTimeout(() => {
            this.$router.go(-1);
          }, 500);
        } else {
          this.$toast(res.message);
        }
      });
    }
  }
};
</script>

<style scoped lang='scss'>
.active {
  color: #fff !important;
  background: #2ea2fa !important;
}
.btn {
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  display: flex;
  padding-top: 1.46rem;

  button {
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
  }
}

.chatdesc {
  background: #f2f2f2;
  margin-top: 0.1rem;
  padding: 0 0.3rem;

  textarea {
    border: 0;
    width: 100%;
    height: 1.24rem;
    line-height: 0.41rem;
    background-color: #f2f2f2;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #999999;
    box-sizing: border-box;
    overflow: hidden;
    resize: none;
  }

  p {
    margin: 0;
    height: 0.86rem;
    line-height: 0.86rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #333333;
  }
}

.groupname {
  width: 100%;
  height: 0.88rem;
  display: flex;
  padding: 0 0.3rem;
  box-sizing: border-box;
  background: #f2f2f2;

  input {
    border: 0;
    background: #f2f2f2;
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    font-family: PingFang-SC-Medium;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666666;
  }
}

.uploadHead {
  height: 2.8rem;
  display: flex;
  justify-content: center;
  align-items: center;
  background: #fff;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;

  /deep/ .van-uploader__wrapper {
    display: flex;
    justify-content: center;

    img {
      width: 1rem;
      height: 1rem;
      border-radius: 0.04rem;
    }
  }

  & > div {
    display: flex;
    flex-direction: column;

    span {
      margin-top: 0.1rem;
    }
  }
}
</style>
