<!-- 公司认证 -->
<template>
  <div class="IdentCompany">
    <div class="UploadImg">
      <div class="title" v-show="isAuthText.LSText">
        <p>{{isAuthText.LSText}}</p>
        <p>(<span>{{licenseCount}}</span>/1)</p>
      </div>
      <div>
        <van-uploader v-show="dele" :multiple="false" :after-read="onRead">
          <img :src="license" alt />
        </van-uploader>
        <div v-show="!dele" class="van-uploader">
          <img @click="bigImg(license)" :src="license" alt />
        </div>
      </div>
    </div>
    <!-- 上传 -->
    <p @click="ImgIsChecked" class="agre" v-show="isAuthText.btn == '上传'">
      <img
        :src="IsChecked?require('@/assets/images/select_sel.png'):require('@/assets/images/select_nor.png')"
        alt
      />
      <span>
        我已阅读并同意
        <router-link to="/signing">《相信充值商服务协议》</router-link>
      </span>
    </p>
    <div
      v-if="isAuthText.btn"
      :class="['UploadBtn',Status&&'isAuth',nextStep==true?'':'BtnDisabled']"
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
      IsChecked: false, //同意电子签约
      show: false,
      images: [], //大图
      license: require("@/assets/images/image_add_large@2x.png"), //营业执照图片
      licenseCount: 0, //营业执照图片数量
      dele: true, //删除小图标
      isAuthText: {
        LSText: "", //请上传您的营业执照
        btn: ""
      },
      urlContentFiles: [
        { uploadfiles: [{}, {}, {}], authdatatype: 1 },
        { uploadfiles: [{}], authdatatype: 2 }
      ],
      nextStep: false,
      uploadFlag: 1
    };
  },
  created() {
    this.hasStatus() == 1 && this.GetAuthData();
  },
  computed: {
    Status() {           
      //是否禁用提交按钮
      let arr = ["等待审核","冻结","审核通过","审核拒绝"] ;
      return arr.some(item => this.isAuthText.btn == item) ? true : false ;
    }
  },
  watch: {
    uploadFlag: function() {
      if (this.urlContentFiles[1].uploadfiles[0].url) {
        this.nextStep = true;
        let bool = ["等待审核","审核通过"].every( item => this.$route.query.statusdesc !=item ) ;
        if (bool) {
          this.isAuthText.btn = "上传";
        }
      } else {
        this.nextStep = false;
      }
    }
  },
  methods: {
    hasStatus(){
      if (
        this.$route.query.statusdesc == "未上传资料" ||
        (this.$route.query.statusdesc == "审核拒绝" &&
          this.$store.state.IdentCompany)
      ) {
        this.isAuthText.btn = "上传";
        this.isAuthText.LSText = "请上传您的营业执照";
        //返回
        if (this.$store.state.IdentCompany) {
          let countType5 = 0;
          for (let item of JSON.parse(this.$store.state.IdentCompany)) {
            if (item.imageactiontype == 5) {
              countType5++;
              this.licenseCount += countType5;
              this.license = item.fullurl;
              let self = this.urlContentFiles[1].uploadfiles[0] ;
              self.url = item.url;
              self.fullurl = item.fullurl;
              self.imageactiontype = 5;
            }
          }
          this.uploadFlag += 1;
        }
        return -1;
      }else{
        return 1 ;
      }
    },
    //查看大图
    bigImg(item) {
      if (item.length < 90) return;
      this.show = true;
      this.images[0] = item;
    },
    ImgIsChecked() {
      this.IsChecked = !this.IsChecked;
    },
    //营业执照上传
    async onRead(file, detail) {
      this.license = file.content;
      this.isAuthText.btn = "正在上传,请稍等...";
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        content: file.content, //64位图片
        typeid: file.content.split(";")[0].split("/")[1] //文件后缀
      };
      let res = await UploadFile(data);
      if (res.result > 0) {
        this.licenseCount == 1 ? 1 : this.licenseCount++;
        let self = this.urlContentFiles[1].uploadfiles[0] ;
        self.fullurl = res.data.fullurl;
        self.url = res.data.url;
        self.imageactiontype = 5;
      } else {
        this.license = "";
      }
      this.uploadFlag += 1;
    },
    testCon(){
      if (!this.urlContentFiles[1].uploadfiles[0].url) {
        this.Toast("营业执照不能为空！");
        return -1 ;
      }
      if (!!!this.IsChecked) {
        this.Toast("请先同意相信充值商服务协议");
        return -1 ;
      }
      let identityCard = this.$store.state.IdentityCard;
      if (!identityCard) {
        this.Toast("请上传身份证信息");
        this.$router.go(-1);
        return -1 ;
      }
      return 1 ;
    },
    async AsyncUploadFile() {
      if (this.testCon() == -1) return;
      this.urlContentFiles[0].uploadfiles = JSON.parse(this.$store.state.IdentityCard);
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        authdatas: this.urlContentFiles
      };
      let res = await UploadAuthData(data);
      if (res.result > 0) {
        this.dele = false;
        this.Toast("上传成功");
        this.isAuthText.btn = res.data.statusdesc;
        setTimeout(() => {
          this.$router.go(-2);
        }, 500);
      } else {
        this.Toast(res.message);
      }
    },
    //init
    async GetAuthData() {
      let data = {
        authdatatype: 2,
        infoid: this.$route.query.infoid,
        ...JSON.parse(sessionStorage.userParam)
      };
      let res = await GetAuthData(data) ; 
        if (res.result > 0) {
          let licenseCount = 0; //营业执照数量
          for (let item of res.data.imageurls) {
            if (item.imageactiontype == 5 && item.url) {
              licenseCount++;
              this.license =
                item.url.substr(0, item.url.lastIndexOf(".")) +
                "_thumbnail" +
                item.url.substr(item.url.lastIndexOf("."));
              this.licenseCount = licenseCount; //营业执照数量
              this.isAuthText.LSText = "您已上传的营业执照";
              
              let self = this.urlContentFiles[1].uploadfiles[0] ; 
              self.fullurl = item.url;
              self.url = item.url.substr(item.url.indexOf("images") - 1);
              self.imageactiontype = 5;
            }
          }
          if (res.data.statusdesc != "审核拒绝") {
            this.isAuthText.btn = res.data.statusdesc;
            this.dele = false;
          } else {
            this.isAuthText.btn = "上传";
          }
          this.uploadFlag += 1;
        }
    }
  },
  //监听当前页面返回事件
  beforeRouteLeave(to, from, next) {
    this.$store.state.IdentCompany = JSON.stringify(
      this.urlContentFiles[1].uploadfiles
    );
    next(true);
  },
};
</script>

<style scoped lang='scss'>
/deep/ .van-uploader__wrapper,
/deep/ .van-uploader__input-wrapper {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fff;
}
.isAuth {
  background: rgba(205, 205, 205, 1) !important;
  pointer-events: none;
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
  background: #2ea2fa;
  border-radius: 0.06rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.32rem;
  font-family: PingFang-SC-Medium;
  font-weight: 500;
  color: rgba(255, 255, 255, 1);
  margin-left: 0.3rem;
  margin-top: 1rem;
}
.BtnDisabled {
  pointer-events: none;
  background-color: #90caf6 !important;
}

.IdentCompany {
  height: 100%;
  padding-bottom: 1.5rem;
  box-sizing: border-box;
  background: rgba(247, 247, 252, 1);

  .van-uploader {
    width: 100%;
    height: 3rem;
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .van-uploader img {
    height: auto;
    width: auto;
    max-width: 100%;
    max-height: 100%;
  }

  .UploadImg,
  .CompanyImgList {
    // border-top: 0.02rem solid #dedede;
    // height: 4.3rem;
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
    margin-top: 0.2rem;
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
      img {
        width: auto;
        height: auto;
        max-width: 100%;
        max-height: 100%;
      }
    }
  }
}

.agre {
  padding-top: 0.3rem;
  display: flex;
  justify-content: center;
  img {
    padding-right: 0.1rem;
    width: 0.28rem;
    height: 0.28rem;
  }

  span {
    font-size: 0.24rem;
    font-family: PingFang-SC-Medium;
    font-weight: 500;
    color: rgba(102, 102, 102, 1);
    color: #666;
  }
}
</style>
