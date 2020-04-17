<!-- 发布 -->
<template>
  <div class="Publishs">
    <div class="content">
      <textarea v-model="content" placeholder="请输入你要发布的内容..." name id cols="30" rows="10"></textarea>
      <div class="imageList">
        <div v-for="(image,index) of imageList" class="posting-uploader-item" :key="index">
          <img :src="image.content" @click="PreviewImage(image)" />
          <img
            @click="imageList.splice(index,1)"
            class="detele"
            src="@/assets/images/detele.png"
            alt
          />
        </div>
        <van-uploader :after-read="ChoosingImg" max-count="9" multiple v-show="imageList.length<9">
          <img src="@/assets/images/dynamic_photo.png" alt />
        </van-uploader>
      </div>
    </div>
    <!-- 设置发布模式 -->
    <p>设置发布模式</p>
    <!--  -->
    <div class="settype">
      <div v-for="(type,index) of publishTypes" :key="index">
        {{type.title}}
        <img
          @click="PublishTypeSelete(type)"
          :src="!type.isSelected?require('@/assets/images/dynamic_radio_nor@2x.png'):require('@/assets/images/dynamic_radio_sel@2x.png')"
          alt
        />
      </div>
    </div>
    <!-- btn -->
    <div class="btn">
      <button @click="Publish()">确认发布</button>
    </div>
    <van-image-preview :showIndex="false" v-model="imagePreviewBoxShow" :images="previewImages"></van-image-preview>
  </div>
</template>

<script>
import { CreateMsg } from "@/api/findData.js";
import { UploadFile } from "@/api/sysRequest.js";
import { constants } from "fs";
import { fail } from "assert";
import { async } from "q";
export default {
  data() {
    return {
      content: "", //输入文本
      imageList: [], //上传图片集合
      imageUrlList: [], //上传后图片URL集合
      videoUrl: "", //视频地址
      previewImages: [], //预览图片集合
      imagePreviewBoxShow: false, //预览框显示状态
      price: 1, //查看此消息需要多少V点
      //发布模式
      publishTypes: [
        {
          title: "免费浏览",
          isSelected: true
        },
        {
          title: "收费浏览(用户查看需支付1V)",
          isSelected: false
        }
      ]
    };
  },
  methods: {
    //预览图片
    PreviewImage: function(image) {
      this.imagePreviewBoxShow = true;
      this.previewImages[0] = image.content;
    },
    //发布状态选择事件
    PublishTypeSelete: function(type) {
      //免费浏览 || 收费浏览
      for (let item of this.publishTypes) {
        item.isSelected = false;
      }
      type.isSelected = true;
    },
    //发布
    Publish: async function() {
      let flag = await this.UploadFile();
      if (!flag) {
        return;
      }
      //图片、文本内容必有其一
      if (
        (this.content == null || this.content == "") &&
        this.imageUrlList.length < 1
      ) {
        this.$toast("请填写发布内容或选择发布图片");
        return;
      }
      let data = {};
      data["price"] = this.price;
      data["content"] = this.content;
      data["video"] = this.videoUrl;
      data["picurl"] = this.imageUrlList.join(",");
      CreateMsg(data, resp => {
        if (resp.result < 1) {
          this.$toast("发布失败");
          return;
        }
        this.$router.go(-1);
      });
    },
    UploadFile: async function() {
      let flag = true;
      this.imageUrlList = [];
      for (let index = 0; index < this.imageList.length; index++) {
        const element = this.imageList[index];
        let data = {};
        let suffix = element.file.name.substring(
          element.file.name.lastIndexOf(".") + 1
        );
        data["content"] = element.content;
        data["typeid"] = suffix;
        let resp = await UploadFile(data, null);
        if (resp.result > 0) {
          if (suffix.toLowerCase() == "mp4") {
            this.videoUrl = resp.data.fullurl;
          } else {
            this.imageUrlList.push(resp.data.fullurl);
          }
        } else {
          this.videoUrl = "";
          this.imageUrlList = [];
          this.$toast("上传文件失败");
          flag = false;
          break;
        }
      }
      return flag;
    },
    //上传图片
    ChoosingImg(file) {
      if (!file.length) file.length = 1;
      if (this.imageList.length + file.length > 9) {
        this.$toast("最大为9张");
        return;
      }
      if (file.length > 1) {
        this.imageList = [...this.imageList, ...file];
      } else {
        this.imageList.push(file);
      }
    }
  }
};
</script>

<style scoped lang='scss'>
.detele {
  width: 0.36rem !important;
  height: 0.36rem !important;
  position: absolute;
  top: -0.1rem;
  right: -0.1rem;
}
// .van-uploader {
//   margin-left: 0.4rem;
// }

// .van-uploader:nth-child(1),
// .van-uploader:nth-child(5),
// .van-uploader:nth-child(9) {
//   margin-left: 0;
// }

.btn {
  box-sizing: border-box;
  padding: 0 0.3rem;
  display: flex;
  padding-top: 2rem;

  button {
    width: 100%;
    font-family: MicrosoftYaHei;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    border: 0;
    height: 0.8rem;
    background-color: #2ca1f9;
    border-radius: 0.1rem;
  }
}

.settype {
  & > div:first-child {
    border-bottom: 0.01rem solid #f8f8f8;
  }

  & > div {
    background: #fff;
    display: flex;
    justify-content: space-between;
    height: 0.88rem;
    align-items: center;
    box-sizing: border-box;
    padding: 0 0.3rem;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #000000;

    img {
      width: 0.35rem;
      height: 0.35rem;
    }
  }
}

p {
  padding-left: 0.3rem;
  margin: 0;
  height: 0.88rem;
  line-height: 0.88rem;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
}

.imageList {
  display: flex;
  flex-wrap: wrap;
  &>div:nth-of-type(n+5){
    margin-top: .3rem;
  }
  .posting-uploader-item,.van-uploader {
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    height: 1.2rem;
    width: 25%;
    padding:0 .3rem;
    box-sizing: border-box;
  }

  img {
    /* margin-left: .4rem; */
    width: auto;
    height: auto;
    max-width: 1.2rem;
    max-height: 1.2rem;
  }
  // .posting-uploader-item:nth-child(1),
  // .posting-uploader-item:nth-child(5),
  // .posting-uploader-item:nth-child(9) {
  //   margin-left: 0;
  // }
}

.Publishs {
  height: 100%;
  background: #f0f0f0;
  overflow-y: scroll;
  /* padding-bottom: .78rem; */
}

.content {
  background: #fff;
  width: 100%;
  box-sizing: border-box;
  padding: 0.48rem 0.3rem 0.2rem 0.3rem;

  textarea::placeholder {
    color: #999;
  }

  textarea {
    height: 2.8rem;
    border: 0;

    width: 100%;
    resize: none;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #999999;
  }
}
</style>
