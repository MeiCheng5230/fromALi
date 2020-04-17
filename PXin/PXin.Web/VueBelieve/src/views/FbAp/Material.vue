<!-- 补充资料 -->
<template>
  <div class="PactionPictrue IdentCompany">
    <div class="CompanyImgList">
      <div class="title" v-if="isAuthText.title">
        <p>{{isAuthText.title}}</p>
        <p>(<span>{{imgCount}}</span>/9)</p>
      </div>
      <div class="ImgList">
        <div
          v-for="(item,index) of urlContentFiles[0].uploadfiles"
          class="posting-uploader-item"
          :key="index"
        >
          <!-- image_add_large@2x.png -->
          <img @click="bigImg(item.fullurl)" :src="item.fullurl" />
          <i
            v-show="dele"
            @click="delImg(index)"
            class="delte"
            :style="{backgroundImage:'url('+require('@/assets/images/ic_cancel@2x.png')+')'}"
          ></i>
        </div>
        <van-uploader :after-read="onCompanyImg" v-show="(dele && imgCount<9 )">
          <img src="@/assets/images/image_add_small@2x.png" alt />
        </van-uploader>
      </div>
    </div>
    <!-- 上传 -->
    <div
      class="UploadBtn"
      v-if="isAuthText.btn"
      :class="Status&&'isAuth'"
      @click="AsyncUploadFile"
    >{{isAuthText.btn}}</div>

    <!-- 大图 -->
    <van-image-preview :showIndex="false" v-model="show" :images="images"></van-image-preview>
  </div>
</template>

<script>
import { UploadFile, UploadAuthData ,GetAuthData} from "@/api/getFbApData";
export default {
  data() {
    return {
      show: false,
      images: [],
      imgCount: 0, //补充资料图片上传数量
      CompanyImg: require("@/assets/images/image_add_small@2x.png"), //默认图片
      urlContentFiles: [{ authdatatype: 4, uploadfiles: [] }],
      dele: true, //删除小图标
      isAuthText: {
        title: "",
        btn: "上传"
      }
    };
  },
  created() {
    this.hasStatus() == 1 && this.GetAuthData();
  },
  computed: {
    Status() {
      //是否禁用提交按钮
      let arr = ['等待审核','冻结','审核通过','审核拒绝'] ;
      return arr.some(item => this.isAuthText.btn == item) ? true : false ;
    }
  },
  methods: {
    hasStatus(){
      if (this.$route.query.supplementstatusdesc == "未上传资料") {
        this.isAuthText.btn = "上传";
        this.isAuthText.title = "请上传您的补充资料";
        return -1 ;
      }else{
        return 1 ;
      }
    },
    //大图
    bigImg(item) {
      this.show = true;
      this.images[0] = item;
    },
    //提交上传图片
    async AsyncUploadFile() {
      if (this.imgCount == 0) {
        this.Toast("补充图片至少一张！");
        return;
      }
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        authdatas: this.urlContentFiles
      };
      let res = await UploadAuthData(data);
      if (res.result > 0) {
        this.dele = false;
        this.Toast("上传成功");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
      } else {
        this.Toast(res.message);
      }
    },
    //补充资料上传
    async onCompanyImg(file, detail, item, index) {
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        content: file.content, //64位图片
        typeid: file.content.split(";")[0].split("/")[1] //文件后缀
      };
      let res = await UploadFile(data);
      if (res.result > 0) {
        this.imgCount < 9 ? this.imgCount++ : 9;
        if (this.imgCount <= 9) {
          this.urlContentFiles[0].uploadfiles.push({
            url: res.data.url,
            imageactiontype: 9,
            fullurl: res.data.fullurl
          });
        }
      }
    },
    //删除图片
    delImg(index) {
      this.urlContentFiles[0].uploadfiles.splice(index, 1);
      this.imgCount--;
    },
    //init
    async GetAuthData() {
      let data = {
        authdatatype: 4,
        infoid: this.$route.query.infoid,
        ...JSON.parse(sessionStorage.userParam)
      };
      let res = await GetAuthData(data) ;
        if (res.result > 0) {
          for (let item of res.data.imageurls) {
            this.urlContentFiles[0].uploadfiles.push({
              url: item.url.substr(item.url.indexOf("images") - 1),
              fullurl: item.url,
              imageactiontype: 9
            });
          }
          this.isAuthText.title = "您已上传的补充资料";
          this.imgCount = res.data.imageurls.length;
          if (res.data.supplementstatusdesc != "审核拒绝") {
            this.isAuthText.btn = res.data.supplementstatusdesc;
            this.dele = false;
          } else {
            this.isAuthText.btn = "上传";
          }
        }
    }
  },
};
</script>

<style scoped lang='scss'>
.isAuth {
  background: rgba(205, 205, 205, 1) !important;
  pointer-events: none;
}

.PactionPictrue {
  height: 100%;
  background: rgba(247, 247, 252, 1);

  .cause {
    padding: 0 0.3rem;
    background: rgba(255, 185, 15, 1);
    font-size: 0.28rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(255, 255, 255, 1);
    height: 0.7rem;
    line-height: 0.7rem;
  }

  .UploadBtn {
    width: 6.9rem;
    height: 0.88rem;
    background: #2ea2fa;
    border-radius: 0.06rem;
    margin: 0 auto;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.32rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(255, 255, 255, 1);
    margin-top: 1.9rem;

    position: fixed;
    bottom: 0.3rem;
    margin-left: 0.3rem;
  }

  .IdentCompany {
    padding-bottom: 0.3rem;
    background: rgba(247, 247, 252, 1);

    .van-uploader {
      width: 100%;
      height: 4rem;
    }

    .van-uploader img {
      width: 100%;
      height: 100%;
    }

    .UploadImg,
    .CompanyImgList {
      border-top: 0.02rem solid #dedede;
      height: 5.3rem;
      width: 100%;
      padding: 0.3rem;
      padding-top: 0;
      background: #fff;

      .title {
        display: flex;
        justify-content: space-between;
        height: 1rem;
        align-items: center;

        p {
          font-size: 0.28rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(51, 51, 51, 1);
        }

        p:last-child {
          font-size: 0.28rem;
          font-family: PingFang-SC-Medium;
          font-weight: 500;
          color: rgba(153, 153, 153, 1);
        }
      }
    }

    .CompanyImgList {
      padding-top: 0.2rem;
      border-top: none;
      height: auto;
      padding-bottom: 0.3rem;
      background: #fff;

      .ImgList {
        display: flex;
        flex-wrap: wrap;

        & > div:nth-child(1),
        & > div:nth-child(2),
        & > div:nth-child(3) {
          margin-top: 0;
        }

        & > div:nth-child(1),
        & > div:nth-child(4),
        & > div:nth-child(7) {
          margin-left: 0;
        }

        & > div {
          margin-left: 0.15rem;
        }

        & > div {
          margin-top: 0.1rem;
        }
      }

      .van-uploader,
      .posting-uploader-item {
        width: 2.15rem;
        height: 2.15rem;

        img {
          width: 100%;
          height: 100%;
        }
      }
    }
  }
}

.posting-uploader-item {
  position: relative;

  .delte {
    width: 0.3rem;
    height: 0.3rem;
    position: absolute;
    top: 0.1rem;
    right: 0.1rem;
    background-size: 100% 100%;
  }
}

.UploadBtn {
  width: 6.9rem;
  height: 0.88rem;
  background: rgba(255, 185, 15, 1);
  border-radius: 0.06rem;
  margin: 0 auto;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.32rem;
  font-family: PingFang-SC-Medium;
  font-weight: 500;
  color: rgba(255, 255, 255, 1);
  margin-top: 1.9rem;
}

.IdentCompany {
  /* min-height: 13.34rem; */
  /* padding-bottom: .3rem; */
  box-sizing: border-box;
  background: rgba(247, 247, 252, 1);

  .van-uploader {
    width: 100%;
    height: 4rem;
  }

  .van-uploader img {
    width: 100%;
    height: 100%;
  }

  .UploadImg,
  .CompanyImgList {
    border-top: 0.02rem solid #dedede;
    height: 5.3rem;
    width: 100%;
    padding: 0.3rem;
    padding-top: 0;
    background: #fff;
    box-sizing: border-box;

    .title {
      display: flex;
      justify-content: space-between;
      height: 1rem;
      align-items: center;

      p {
        font-size: 0.28rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(51, 51, 51, 1);
      }

      p:last-child {
        font-size: 0.28rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(153, 153, 153, 1);
      }
    }
  }

  .CompanyImgList {
    padding-top: 0.2rem;
    border-top: none;
    height: auto;
    padding-bottom: 0.3rem;

    .ImgList {
      display: flex;
      flex-wrap: wrap;

      & > div:nth-child(1),
      & > div:nth-child(2),
      & > div:nth-child(3) {
        margin-top: 0;
      }

      & > div:nth-child(1),
      & > div:nth-child(4),
      & > div:nth-child(7) {
        margin-left: 0;
      }

      & > div {
        margin-left: 0.15rem;
      }

      & > div {
        margin-top: 0.1rem;
      }
    }

    .van-uploader,
    .posting-uploader-item {
      width: 2.15rem;
      height: 2.15rem;
      display: flex;
      justify-content: center;
      align-items: center;
      background: #fff;
      img {
        height: auto;
        width: auto;
        max-width: 100%;
        max-height: 100%;
      }
    }
  }
}
</style>
