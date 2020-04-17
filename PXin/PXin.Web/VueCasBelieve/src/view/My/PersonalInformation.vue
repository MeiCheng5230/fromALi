<!-- 个人信息 -->
<template>
  <div class="PersonalInformation">
    <!-- 个人头像 昵称 手机号-->
    <div class="container">
      <!-- 头像 -->
      <div @click="headPicBoxShow=true">
        <div>个人头像</div>
        <div>
          <img :src="user.pic" alt />
          <span></span>
        </div>
      </div>
      <!-- 昵称 -->
      <div tag="div" @click="SetNodeName()" class="setName">
        <div>昵称</div>
        <div>
          <span>{{user.nodename}}</span>
          <span></span>
        </div>
      </div>
      <!-- 手机号 -->
      <div tag="div" @click="SetMobileno()" class="setName">
        <div>手机号</div>
        <div>
          <span>{{user.mobileno}}</span>
          <span></span>
        </div>
      </div>
      <!-- UE账号 -->
      <div tag="div" @click="UEBindClick()" class="setName">
        <div>UE账号</div>
        <div>
          <span v-if="isBindUE">{{ueUser.nodename}}</span>
          <span v-else>未绑定</span>
          <span></span>
        </div>
      </div>
      <!--账号安全  -->
      <router-link tag="div" to="/UserSafety" class="setName">
        <div>账号安全</div>
        <div></div>
      </router-link>
    </div>
    <!-- 退出登录 -->
    <div class="btn">退出登录</div>

    <van-popup v-model="headPicBoxShow" position="bottom">
      <div class="upload">
        <van-uploader :after-read="ChangeHeadPic">图像</van-uploader>
      </div>
      <div class="close" @click="headPicBoxShow=false">取消</div>
    </van-popup>
  </div>
</template>

<script>
import { GetUserInfo, GetYGPCNUEUserInfo, EditUserInfo } from "@/api/myData";
import { Go, UploadFile } from "@/api/sysRequest.js";
import { fail } from "assert";
export default {
  data() {
    return {
      user: {},
      isBindUE: true,
      ueUser: {},
      headPicBoxShow: false,
      headPicType: { jpg: "", jpeg: "", png: "", gif: "", bmp: "" },
      headPicTypeStrArr: ["jpg", "jpeg", "png", "gif", "bmp"]
    };
  },
  created() {
    GetUserInfo(null, data => {
      if (data.result < 0) {
        this.$toast("获取用户数据失败");
        return;
      }
      this.user = data.data;
    });
    GetYGPCNUEUserInfo({ typeid: 0 }, data => {
      if (data.result < 0 && data.result != -5) {
        this.$toast("获取用户数据失败");
        return;
      }
      if (data.result == -5) {
        this.isBindUE = false;
      }
      this.ueUser = data.data;
    });
  },
  methods: {
    //图片更换
    ChangeHeadPic(file) {
      let data = {};
      data["content"] = file.content;
      data["typeid"] = file.file.name.substring(
        file.file.name.lastIndexOf(".") + 1
      );
      if (!(data.typeid in this.headPicType)) {
        this.$toast(
          "请上传：" + this.headPicTypeStrArr.join("，") + "类型文件"
        );
        return;
      }
      UploadFile(data, data => {
        if (data.result < 1) {
          this.$toast("修改头像失败");
          return;
        }
        let fullurl = data.data.fullurl;
        EditUserInfo(
          {
            type: 3,
            info: data.data.fullurl
          },
          data => {
            if (data.result < 1) {
              this.$toast("修改头像失败");
              return;
            }
            this.user.pic = fullurl;
          }
        );
      });
      this.headPicBoxShow = false;
    },
    UEBindClick: function() {
      if (this.isBindUE) {
        Go(16);
      } else {
        this.$dialog
          .confirm({
            title: "提示",
            message: "未绑定UE账号，将影响您使用相关功\n能，现在去绑定"
          })
          .then(() => {
            Go(15);
          });
      }
    },
    SetNodeName: function() {
      this.$router.push({
        name: "SetNotes",
        query: { nodename: this.user.nodename }
      });
    },
    SetMobileno: function() {
      this.$router.push({
        name: "Contact",
        query: { mobileno: this.user.mobileno }
      });
    }
  }
};
</script>

<style scoped lang='scss'>
.van-uploader {
  width: 100%;
  /deep/ .van-uploader__input-wrapper {
    width: 100%;
  }
}
.van-popup {
  background: #f2f2f2;
}
.upload,
.close {
  height: 0.8rem;
  line-height: 0.8rem;
  text-align: center;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  background: #fff;
}
.close {
  margin-top: 0.2rem;
}
.btn {
  margin-top: 3.2rem;
  width: 100%;
  box-sizing: border-box;
  height: 0.88rem;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  background: #fff;
}

.PersonalInformation {
  height: 100%;
  box-sizing: border-box;
  background: #f2f2f2;
  padding-top: 0.3rem;

  .container {
    width: 100%;
    box-sizing: border-box;
    padding: 0 0.3rem;
    background: #fff;

    .setName {
      border-bottom: 0.01rem solid #c7c7c7;
    }

    .phone {
      border-bottom: 0.01rem solid #c7c7c7;

      span:nth-child(1) {
        color: #666 !important;
      }
    }

    .phone,
    .setName {
      font-family: PingFang-SC-Medium;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      height: 0.89rem;
      align-items: center;
      color: #4f4f4f;
      display: flex;
      justify-content: space-between;

      span:nth-child(1) {
        margin-right: 0.2rem;
        color: #999999;
      }

      span:nth-child(2) {
        display: inline-block;
        width: 0.15rem;
        height: 0.15rem;
        border-top: 0.01rem solid #676767;
        border-right: 0.01rem solid #676767;
        transform: rotate(45deg);
      }
    }

    & > div:nth-child(1) {
      height: 1.1rem;
      box-sizing: border-box;
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 0.01rem solid #c7c7c7;

      div {
        font-family: PingFang-SC-Medium;
        font-size: 0.28rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0.01rem;
        color: #333333;
        display: flex;
        align-items: center;

        img {
          width: 0.9rem;
          height: 0.9rem;
          border-radius: 50%;
          margin-right: 0.34rem;
        }

        span {
          display: inline-block;

          width: 0.15rem;
          height: 0.15rem;
          border-top: 0.01rem solid #676767;
          border-right: 0.01rem solid #676767;
          transform: rotate(45deg);
        }
      }
    }
  }

  .setName:last-child {
    border-bottom: 0;

    div:last-child {
      width: 0.15rem;
      height: 0.15rem;
      border-top: 0.01rem solid #676767;
      border-right: 0.01rem solid #676767;
      transform: rotate(45deg);
    }
  }
}
</style>
