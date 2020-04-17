<template>
  <div class="IdentityCard">
    <div class="upload">
      <div class="t" v-if="isAuthText.title">{{isAuthText.title}}</div>
      <van-uploader :disabled="isDisabled" class="uploadImg" :after-read="uploadImg1">
        <img
          :class="isDisabled && 'isbig'"
          @click="bigImg(Img1)"
          :src="!Img1?require('@/assets/images/image_card_1@2x.png'):Img1"
          alt
        />
      </van-uploader>

      <van-uploader :disabled="isDisabled" class="uploadImg" :after-read="uploadImg2">
        <img
          :class="isDisabled && 'isbig'"
          @click="bigImg(Img2)"
          :src="!Img2?require('@/assets/images/image_card_2@2x.png'):Img2"
          alt
        />
      </van-uploader>

      <van-uploader :disabled="isDisabled" class="uploadImg" :after-read="uploadImg3">
        <img
          :class="isDisabled && 'isbig'"
          @click="bigImg(Img3)"
          :src="!Img3?require('@/assets/images/image_card_3@2x.png'):Img3"
          alt
        />
      </van-uploader>
    </div>
    <div
      v-show="isAuthText.btn"
      :class="['Btn',Status&&'isAuth',nextStep==true?'':'BtnDisabled']"
      @click="AsyncUploadFile"
    >{{isAuthText.btn}}</div>

    <van-popup v-model="tipshow">
      <div class="tipSon">
        <p>退出后上传的资料将会被清空</p>
        <p>确定退出？</p>
        <div class="footerBut">
          <span class="sure" @click="tipBacks">确定</span>
          <span class="cancle" @click="tipColse">继续上传</span>
        </div>
      </div>
    </van-popup>
    <!-- 大图 -->
    <van-image-preview :showIndex="false" v-model="show" :images="images"></van-image-preview>
  </div>
</template>
<script>
import { Toast } from "vant";
import Vue from "vue";
import { UploadFile, UploadAuthData ,GetAuthData} from "@/api/getFbApData";

Vue.use(Toast);
export default {
  data() {
    return {
      IsChecked: false, //同意电子签约
      isSucc: false,
      show: false,
      images: [], //点击放大
      license: require("@/assets/images/image_card_1@2x.png"),
      Img1: "", // 身份证正面照
      Img2: "", // 身份证反面照
      Img3: "", // 手持身份证照
      isDisabled: false, //禁止用户上传
      files: [], //返回的相对路径集合
      isAuthText: {
        title: "",
        btn: ""
      },
      tipshow: false, //控制提示弹窗显隐
      backStatu: false, //判断当执行页面回退时是否要执行next(true);
      urlContentFiles: [{ uploadfiles: [{}, {}, {}], authdatatype: 1 }], //上传文件集合
      nextStep: false,
      uploadFlag: 1
    };
  },
  created() {
    this.hasStatus() == 1 && this.GetAuthData();
  },
  watch: {
    uploadFlag: function() {
      let imgs = this.urlContentFiles[0].uploadfiles;
      if (imgs.every(item => { return item.url })) {
        this.nextStep = true;
        if (this.$route.query.statusdesc != "等待审核") {
          this.isAuthText.btn = this.$route.query.typeid == 5 ? "上传" : "下一步";
        }
      } else {
        this.nextStep = false;
      }
    }
  },
  computed: {
    Status() {
      //是否禁用提交按钮
      let arr = ['等待审核','冻结','审核通过'] ;
      return arr.some(item => this.isAuthText.btn==item) ? true : false ;
    }
  },
  methods: {
    hasStatus(){
      if (
        this.$route.query.statusdesc == "未上传资料" ||
        (this.$route.query.statusdesc == "审核拒绝" && this.$store.state.IdentityCard)
      ) {
        this.isAuthText.btn = this.$route.query.typeid == 5 ? "上传" : "下一步";
        this.isAuthText.title = "请上传您的身份证";
        //返回
        if (this.$store.state.IdentityCard) {
          for (let item of JSON.parse(this.$store.state.IdentityCard)) {
            if (item.imageactiontype == 1) {
              this.Img1 = item.fullurl;
              let self = this.urlContentFiles[0].uploadfiles[0] ; 
              self.url = item.url;
              self.fullurl = item.fullurl;
              self.imageactiontype = 1;
            } else if (item.imageactiontype == 2) {
              this.Img2 = item.fullurl;
              let self = this.urlContentFiles[0].uploadfiles[1] ; 
              self.url = item.url;
              self.fullurl = item.fullurl;
              self.imageactiontype = 2;
            } else if (item.imageactiontype == 3) {
              this.Img3 = item.fullurl;
              let self = this.urlContentFiles[0].uploadfiles[2] ; 
              self.url = item.url;
              self.fullurl = item.fullurl;
              self.imageactiontype = 3;
            }
          }
        } else {
        }
        this.uploadFlag += 1;
        return -1 ;
      }else{
        return 1 ;
      }
    },
    //放大图片
    bigImg(item) {
      if (!this.isDisabled) return;
      this.show = true;
      this.images[0] = item;
    },
    ImgIsChecked() {
      this.IsChecked = !this.IsChecked;
    },
    //上传图片
    async uploadImg1(file) {
      this.Img1 = file.content;
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        content: file.content, //64位图片
        typeid: file.content.split(";")[0].split("/")[1] //文件后缀
      };
      let res = await UploadFile(data);
      if (res.result > 0) {
        let self = this.urlContentFiles[0].uploadfiles[0] ;
        self.url = res.data.url;
        self.fullurl = res.data.fullurl;
        self.imageactiontype = 1;
      } else {
        this.Img1 = "";
      }
      this.uploadFlag += 1;
    },
    async uploadImg2(file) {
      this.Img2 = file.content;
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        content: file.content, //64位图片
        typeid: file.content.split(";")[0].split("/")[1] //文件后缀
      };
      let res = await UploadFile(data);
      if (res.result > 0) {
        let self = this.urlContentFiles[0].uploadfiles[1] ;
        self.url = res.data.url;
        self.fullurl = res.data.fullurl;
        self.imageactiontype = 2;
      } else {
        this.Img2 = "";
      }
      this.uploadFlag += 1;
    },
    async uploadImg3(file) {
      this.Img3 = file.content;
      this.isAuthText.btn = "正在上传,请稍等...";
      let data = {
        ...JSON.parse(sessionStorage.userParam),
        content: file.content, //64位图片
        typeid: file.content.split(";")[0].split("/")[1] //文件后缀
      };
      let res = await UploadFile(data);
      if (res.result > 0) {
        let self = this.urlContentFiles[0].uploadfiles[2] ; 
        self.url = res.data.url;
        self.fullurl = res.data.fullurl;
        self.imageactiontype = 3;
      } else {
        this.Img3 = "";
      }
      this.uploadFlag += 1;
    },
    async AsyncUploadFile() {
      let imgs = this.urlContentFiles[0].uploadfiles;
      if (imgs.some(item => { !item.url })) {
        this.Toast("请上传证件!");
        return;
      }
      if (this.$route.query.typeid == 5) {
        let data = {
          ...JSON.parse(sessionStorage.userParam),
          authdatas: this.urlContentFiles
        };
        let res = await UploadAuthData(data);
        if (res.result > 0) {
          this.isSucc = true;
          this.Toast("上传成功");
          this.isAuthText.btn = res.data.statusdesc;
          setTimeout(() => {
            this.$router.go(-1);
          }, 500);
        } else {
          this.Toast(res.message);
        }
      } else {
        this.$store.state.IdentityCard = JSON.stringify(
          this.urlContentFiles[0].uploadfiles
        );
        this.$router.push({
          path: "/IdentityCompany",
          query: {
            infoid: this.$route.query.infoid,
            statusdesc: this.$route.query.statusdesc
          }
        });
      }
    },
    //组件加载获取数据 /api/FbAp/GetAuthData获取认证资料
    async GetAuthData() {
      let data = {
        authdatatype: 1,
        infoid: this.$route.query.infoid,
        ...JSON.parse(sessionStorage.userParam)
      };
      let res = await GetAuthData(data) ;
        if (res.result > 0) {
          for (let item of res.data.imageurls) {
            if (item.url == "") {
              this.Toast("图片不能为空！");
              return;
            }
          }
          this.isAuthText.title = "您已上传的身份证";
          let url1 = res.data.imageurls[0].url;
          let url2 = res.data.imageurls[1].url;
          let url3 = res.data.imageurls[2].url;
          this.Img1 =
            url1.substr(0, url1.lastIndexOf(".")) +
            "_thumbnail" +
            url1.substr(url1.lastIndexOf(".")); //1
          this.Img2 =
            url2.substr(0, url2.lastIndexOf(".")) +
            "_thumbnail" +
            url2.substr(url2.lastIndexOf(".")); //1
          this.Img3 =
            url3.substr(0, url3.lastIndexOf(".")) +
            "_thumbnail" +
            url3.substr(url3.lastIndexOf(".")); //1

          let self1 = this.urlContentFiles[0].uploadfiles[0] ;
          self1.fullurl = url1;
          self1.url = url1.substr(url1.indexOf("images") - 1);
          self1.imageactiontype = 1;

          let self2 = this.urlContentFiles[0].uploadfiles[1] ;
          self2.fullurl = url2;
          self2.url = url2.substr(url2.indexOf("images") - 1);
          self2.imageactiontype = 2;

          let self3 = this.urlContentFiles[0].uploadfiles[2] ;
          self3.fullurl = url3;
          self3.url = url3.substr(url3.indexOf("images") - 1);
          self3.imageactiontype = 3;

          if (res.data.statusdesc != "审核拒绝") {
            this.isDisabled = true;
            this.isAuthText.btn = res.data.statusdesc; //审核状态
          } else {
            this.isAuthText.btn =
              this.$route.query.typeid == 5 ? "上传" : "下一步";
          }
        }
        this.uploadFlag += 1;
    },
    tipColse() {
      //控制提示弹窗显隐
      this.tipshow = !this.tipshow;
    },
    tipBacks() {
      this.backStatu = true;
      this.$router.go(-1);
    }
  },
 //监听当前页面返回事件
  beforeRouteLeave(to, from, next) {
    if (
      to.path == "/IdentityIndex" &&
      !this.isSucc &&
      (this.Img1 || this.Img2 || this.Img3)
    ) {
      //next方法传true或者不传为默认历史返回，传false为不执行历史回退
      if (['未上传资料','审核拒绝'].some(item => this.$route.query.statusdesc==item)) {
        if (this.backStatu) {
          this.$store.state.IdentityCard = "";
          this.$store.state.IdentCompany = "";
          this.$store.state.contractPhotos = "";
          next(true);
        } else {
          next(false);
        }
        this.tipshow = true;
      } else {
        next(true);
      }
    } else {
      next(true);
    }
  },
};
</script>

<style lang="scss" scoped>
.isAndroid {
  transform: scale(0.5);
}

.isbig {
  position: absolute;
  z-index: 1;
}

/deep/ .van-uploader__wrapper {
  width: 100%;
  height: 100%;
  overflow: hidden;

  .van-uploader__input-wrapper {
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    background: #fff;
  }
}

.van-popup {
  width: 80%;
  border-radius: 0.08rem;
  padding: 0.3rem;
  text-align: center;

  .tipSon {
    p {
      margin: 0;
      padding-top: 0.3rem;
      font-weight: normal;
      color: #5a5b5d;
      font-size: 0.28rem;
    }
  }

  .footerBut {
    margin-top: 0.3rem;
    width: 100%;
    height: 0.8rem;
    display: flex;
    align-items: center;

    span {
      width: 50%;
      height: 100%;
      display: flex;
      align-items: center;
      text-align: center;
      justify-content: center;
    }
  }
}

.isAuth {
  background: rgba(205, 205, 205, 1) !important;
  pointer-events: none; //禁止点击
}

.IdentityCard {
  min-height: 100%;
  background: rgba(247, 247, 252, 1);

  /deep/ .header {
    background: #fff;
  }

  .upload {
    padding: 0.3rem 0.65rem 0.25rem 0.65rem;
    // padding-top: 0;

    .t {
      padding-bottom: 0.3rem;
      font-size: 0.24rem;
      color: #999;
    }

    .uploadImg {
      height: 3rem;
      width: 6.2rem;
      margin-bottom: 0.2rem;
      position: relative;
      display: flex;
      justify-items: center;
      justify-content: center;

      img {
        // width: 100%;
        // height: 100%;

        // width: auto;
        // height: auto;
        // max-width: 100%;
        max-height: 100%;
      }

      input {
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        opacity: 0;
      }
    }
  }

  .Btn {
    background: #2ea2fa;
    color: #fff;
    margin: 0 0.3rem;
    text-align: center;
    padding: 0.22rem 0;
    border-radius: 0.06rem;
  }
  .BtnDisabled {
    pointer-events: none;
    background-color: #90caf6 !important;
  }
}

.agre {
  // padding-top: .3rem;
  padding-bottom: 0.2rem;
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
