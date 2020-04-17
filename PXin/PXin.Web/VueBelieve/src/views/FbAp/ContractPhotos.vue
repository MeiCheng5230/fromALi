<!-- 电子签约 -->
<template>
  <div class="PactionPictrue IdentCompany">
    <div class="CompanyImgList">
      <div class="title">
        <p v-show="isAuthText.title">{{isAuthText.title}}</p>
        <p v-show="isAuthText.title">
          (
          <span>{{CompanyImgCount}}</span>/9)
        </p>
      </div>
      <div class="ImgList">
        <div v-for="(item,index) of CompanyImgList" :key="index" class="posting-uploader-item">
          <img @click='bigImg(item.content)' :src="item.content" />
          <i
            v-show="dele"
            @click="delImg(index)"
            class="delte"
            :style="{backgroundImage:'url('+require('@/assets/images/ic_cancel@2x.png')+')'}"
          ></i>
        </div>
        <van-uploader
          :after-read="onCompanyImg"
          multiple="false"
          v-show="(dele && CompanyImgCount<9 )"
        >
          <img src="@/assets/images/image_add_small@2x.png" alt />
        </van-uploader>
      </div>
    </div>
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
export default {
  data() {
    return {
      show:false,
      images:[],//大图
      CompanyImgCount: 0, //合同图片上传数量
      CompanyImg: require("@/assets/images/image_add_small@2x.png"),
      CompanyImgList: [], //合同图片上传数量数组
      urlContentFiles: [], //上传文件集合
      dele: true, //删除小图标
      isAuthText: {
        title: "",
        btn: ""
      },
      remark: "", //审核不通过原因
      uploadfilesCount: 0,
      fileCount: 0
    };
  },
  created() {
    this.hasStatus() == 1 &&  this.GetAuthData();
  },
  computed: {
    Status() {
      //是否禁用提交按钮
      let arr = ["等待审核","冻结","审核通过"] ;
      return arr.some(item => this.isAuthText.btn == item) ? true : false ;
    }
  },
  watch: {
    uploadfilesCount(newValue, oldValue) {
      if (newValue == this.fileCount) {
        let urlUploadAuthData = "/api/FbAp/UploadAuthData";
        let data1 = {
          ...JSON.parse(sessionStorage.userParam),
          authdatas: this.urlContentFiles
        };
        this.axios.post(urlUploadAuthData, data1).then(res => {
          if (res.data.result > 0) {
            this.dele = false;
            this.Toast("上传成功");
            this.isAuthText.btn = res.data.data.statusdesc;
            setTimeout(() => {
              this.$router.push({
                path: "/IdentityIndex",
                query: {
                  infoid: this.$route.query.infoid
                }
              });
            }, 500);
          } else {
            this.Toast(res.data.message);
          }
        });
      }
    }
  },
  methods: {
    hasStatus(){
      if (
        JSON.parse(this.$route.query.statusdesc) == "未上传资料" ||
        (JSON.parse(this.$route.query.statusdesc) == "审核拒绝" &&
          this.$store.state.contractPhotos)
      ) {
        this.isAuthText.btn = "上传";
        this.isAuthText.title = "请上传您的合同照片";
        if (this.$store.state.contractPhotos) {
          for (let item of JSON.parse(this.$store.state.contractPhotos)) {
            this.CompanyImgList = [...this.CompanyImgList, item];
          }
          this.CompanyImgCount = this.CompanyImgList.length;
        }
        return -1 ;
      }else{
        return 1 ;
      }

    },
    //图片放大
    bigImg(item){
      if(item.length<90) return;
      this.show = true;
      this.images[0] = item;
    },
    //公司图片上传
    onCompanyImg(file, detail, item, index) {
      this.CompanyImgCount < 9 ? this.CompanyImgCount++ : 9;
      if (this.CompanyImgCount <= 9) {
        this.CompanyImgList.push({
          content: file.content,
          imageactiontype: 8
        });
      }
    },
    //删除图片
    delImg(index) {
      this.CompanyImgList.splice(index, 1);
      this.CompanyImgCount--;
    },
    AsyncUploadFile() {
      if (this.CompanyImgList.length == 0) {
        this.Toast("合同图片至少一张！");
        return;
      }
      let this_ = this;
      let IdentityCard = {
        //个人资料
        uploadfiles: [...JSON.parse(this.$store.state.IdentityCard)],
        authdatatype: 1 //个人资料
      };
      let IdentCompany = {
        //公司资料
        uploadfiles: [...JSON.parse(this.$store.state.IdentCompany)],
        authdatatype: 2 //公司资料
      };
      let PactionPictrue = {
        //合同资料
        uploadfiles: [...this.CompanyImgList],
        authdatatype: 3
      };
      this.uploadfilesCount = 0;
      this.urlContentFiles = [IdentityCard, IdentCompany, PactionPictrue];

      this.fileCount =
        this.urlContentFiles[0].uploadfiles.length +
        this.urlContentFiles[1].uploadfiles.length +
        this.urlContentFiles[2].uploadfiles.length;
      let url = "/api/Sys/UploadFile";
      let promiseAll;
      for (var i = 0; i < IdentityCard.uploadfiles.length; i++) {
        let content = IdentityCard.uploadfiles[i].content;
        let type = IdentityCard.uploadfiles[i].imageactiontype;
        if (content.indexOf("data:image/") == -1) {
          let url = content.substr(content.indexOf("images") - 1);
          this.urlContentFiles[0].uploadfiles[i].url = url;
          this.urlContentFiles[0].uploadfiles[i].imageactiontype = type;
          this.uploadfilesCount++;
          continue;
        }
        let data = {
          ...JSON.parse(sessionStorage.userParam),
          content: content, //64位图片
          typeid: content.split(";")[0].split("/")[1] //文件后缀
        };
        let ii = i;
        this.axios.post(url, data).then(res => {
          this.urlContentFiles[0].uploadfiles[ii].url = res.data.data.url;
          this.urlContentFiles[0].uploadfiles[ii].imageactiontype = type;
          this.uploadfilesCount++;
        });
      }
      for (var j = 0; j < IdentCompany.uploadfiles.length; j++) {
        let content = IdentCompany.uploadfiles[j].content;
        let type = IdentCompany.uploadfiles[j].imageactiontype;
        if (content.indexOf("data:image/") == -1) {
          let url = content.substr(content.indexOf("images") - 1);
          this.urlContentFiles[1].uploadfiles[j].url = url;
          this.urlContentFiles[1].uploadfiles[j].imageactiontype = type;
          this.uploadfilesCount++;
          continue;
        }
        let data = {
          ...JSON.parse(sessionStorage.userParam),
          content: content, //64位图片
          typeid: content.split(";")[0].split("/")[1] //文件后缀
        };
        let jj = j;
        this.axios.post(url, data).then(res => {
          this.urlContentFiles[1].uploadfiles[jj].url = res.data.data.url;
          this.urlContentFiles[1].uploadfiles[jj].imageactiontype = type;
          this.uploadfilesCount++;
        });
      }
      for (var k = 0; k < PactionPictrue.uploadfiles.length; k++) {
        let content = PactionPictrue.uploadfiles[k].content;
        let type = PactionPictrue.uploadfiles[k].imageactiontype;
        if (content.indexOf("data:image/") == -1) {
          let url = content.substr(content.indexOf("images") - 1);
          this.urlContentFiles[2].uploadfiles[k].url = url;
          this.urlContentFiles[2].uploadfiles[k].imageactiontype = type;
          this.uploadfilesCount++;
          continue;
        }
        let data = {
          ...JSON.parse(sessionStorage.userParam),
          content: content, //64位图片
          typeid: content.split(";")[0].split("/")[1] //文件后缀
        };
        let kk = k;
        this.axios.post(url, data).then(res => {
          this.urlContentFiles[2].uploadfiles[kk].url = res.data.data.url;
          this.urlContentFiles[2].uploadfiles[kk].imageactiontype = type;
          this.uploadfilesCount++;
        });
      }
    },
    //init
    GetAuthData() {
      let url = "/api/FbAp/GetAuthData";
      let data = {
        authdatatype: 3,
        infoid: this.$route.query.infoid,
        ...JSON.parse(sessionStorage.userParam)
      };
      this.axios.post(url, data).then(res => {
        if (res.data.result > 0) {
          for (let item of res.data.data.imageurls) {
            this.CompanyImgList.push({
              content: item.url,
              imageactiontype: 8
            }); //公司图片
          }
          this.isAuthText.title = "您已上传的合同照片";
          this.CompanyImgCount = res.data.data.imageurls.length; //图片张数
          if (res.data.data.statusdesc != "审核拒绝") {
            this.dele = false; //是否可以继续上传
            this.isAuthText.btn = res.data.data.statusdesc; //审核状态
            this.remark = res.data.data.remark; //不通过理由
          } else {
            this.dele = true; //是否可以继续上传
            this.isAuthText.btn = "上传"; //审核状态
            // this.remark = res.data.data.remark; //不通过理由
          }
        }
      });
    }
  }, //监听当前页面返回事件
  beforeRouteLeave(to, from, next) {
    let contractPhotos = [...this.CompanyImgList];
    this.$store.state.contractPhotos = JSON.stringify(contractPhotos);
    next(true);
  },
};
</script>

<style scoped lang='scss'>
/deep/ .van-uploader__input-wrapper,
/deep/ van-uploader__wrapper {
  width: 100%;
  height: 100%;
  background: #fff;
}
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
    // min-height: 13.34rem;
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
      margin-top: 0.2rem;
      border-top: none;
      height: auto;
      // padding-bottom: 0.3rem;
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
        display: flex;
        align-items: center;
        justify-content: center;

        img {
          width: auto;
          height: auto;
          max-width: 100%;
          max-height: 100%;
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
  // min-height: 13.34rem;
  // padding-bottom: 0.3rem;
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
    // margin-top: 0.2rem;
    border-top: none;
    height: auto;
    padding-bottom: 0.3rem;
    box-sizing: border-box;
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
