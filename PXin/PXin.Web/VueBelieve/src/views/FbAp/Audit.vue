<!-- 审核 -->
<template>
  <div class="checkout">
    <div style="height: .2rem; background: #f2f2f2;"></div>
    <div class="status">
      <p>{{$t('lang.Audit_auditResults')}}</p>
      <div class="statusInfo">
        <div v-for="(item,index) of checkout" @click="changeStatus(item,index)" :key="index">
          <img
            :src="item.ischecked?require('@/assets/images/pay_success.png'):require('@/assets/images/ic_unselected_pay.png')"
            alt
          />
          <span>{{item.result}}</span>
        </div>
      </div>
    </div>
    <div style="height: .2rem; background: #f2f2f2;"></div>

    <!-- 原因 -->
    <div class="cause status" v-show="checkout[1].ischecked">
      <p>{{$t('lang.Audit_auditReason')}}</p>
      <div>
        <textarea
          maxlength="20"
          :placeholder="checkout[1].ischecked && this.$t('lang.Audit_auditText')"
          v-model="remarks"
          name
          id
          cols="30"
          rows="10"
        ></textarea>
        <p>{{TextLength}}/20</p>
      </div>
    </div>
    <div style="height: .2rem; background: #f2f2f2;"></div>

    <div class="checkoutImg status">
      <div>
        <p>{{$t('lang.Audit_auditData')}}</p>
        <div class="htImg">
          <p>{{$t('lang.Audit_IdentityCard')}}：</p>
          <div>
            <div class="imgsList">
              <img @click="onBigImg(imageactiontype1)" :src="imageactiontype1" alt />
            </div>
            <div class="imgsList">
              <img @click="onBigImg(imageactiontype2)" :src="imageactiontype2" alt />
            </div>
            <div class="imgsList">
              <img @click="onBigImg(imageactiontype3)" :src="imageactiontype3" alt />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- 底部 -->
    <div class="fixed" @click="Audit">{{$t('lang.Audit_submit')}}</div>

    <!-- 大图 -->
    <van-image-preview :showIndex="false" v-model="show" :images="images"></van-image-preview>
  </div>
</template>

<script>
import { Dialog } from "vant";
import { GetAuthData, Audit } from "@/api/getFbApData";
export default {
  data() {
    return {
      //  1：身份证正面图片；2：身份证反面图片;3:手持身份证正面照；4：公司照片;5:营业执照；6：开户许可证;7:租赁合同) = ['1', '2', '3', '4', '5', '6', '7']
      imageactiontype1: "",
      imageactiontype2: "",
      imageactiontype3: "",
      imageactiontype4: [],
      imageactiontype5: "",
      imageactiontype6: "",
      imageactiontype7: "",
      show: false,
      images: [], //点击放大
      TextLength: 0,
      causeText: "",
      checkout: [
        {
          result: "通过",
          ischecked: true
        },
        {
          result: "拒绝",
          ischecked: false
        }
      ],
      remarks: "", //拒绝原因
      status: 1, //审核状态(1：通过，2：拒绝) ,
      typeid:0
    };
  },
  created() {
    this.GetAuthData();
  },
  watch: {
    remarks(newVal) {
      this.TextLength = newVal.length;
    }
  },
  methods: {
    //放大
    onBigImg(item) {
      this.show = true;
      this.images[0] = item;
    },

    changeStatus(item, index) {
      for (let item of this.checkout) {
        item.ischecked = false;
      }
      item.ischecked = true;

      this.status = index + 1; //审核状态(1：通过，2：拒绝) ,
    },

    async change() {
      let data = {
        infoid: this.$route.query.infoid,
        status: this.status,
        remarks: this.remarks,
        ...JSON.parse(sessionStorage.userParam)
      };
      let result = await Audit(data);
      if (result.result > 0) {
        this.Toast(result.message);
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
      } else {
        this.Toast(result.message);
      }
    },

    async Audit() {
      //审核经销商资料
      if (this.status == 2 && this.remarks == "") {
        // this.Toast("请输入审核原因");
        this.Toast(this.$t('lang.hint1'));
        return;
      }
      // let msg = "请仔细查看审核资料,并详细核对资料,确定通过该代理人的申请?";
      let msg = this.$t('lang.hint2');
      if (this.status == 2) {
        // msg = "请仔细查看审核资料,并详细说明拒绝理由,确定拒绝该代理人的申请?";
        msg = this.$t('lang.hint3');
      }
      Dialog.confirm({
        message: msg
      })
        .then(() => {
          // on confirm
          this.change();
        })
        .catch(() => {
          // on cancel
        });
    },
    //组件加载获取数据 /api/Bts/GetAuthData获取认证资料
    async GetAuthData() {
      let data = {
        infoid: this.$route.query.infoid,
        ...JSON.parse(sessionStorage.userParam)
      };
      let result = await GetAuthData(data);
      if (result.result > 0) {
        this.typeid=result.data.typeid;
        for (let item of result.data.imageurls) {
          switch(item.imageactiontype){
            case 0:
              this.imageactiontype6 = [...this.imageactiontype6, item.url];
              break ;
            case 1:
              this.imageactiontype1 = item.url ;
              break ;
            case 2:
              this.imageactiontype2 = item.url ;
              break ;
            case 3:
              this.imageactiontype3 = item.url ;
              break ;
            case 4:
              this.imageactiontype4 = [...this.imageactiontype4, item.url];
              break ;
            case 5:
              this.imageactiontype5 = item.url;
              break ;
            case 7:
              this.imageactiontype7 = [...this.imageactiontype7, item.url];
              break ;
            default:
              break ;
          }
        }
      }
    }
  },
};
</script>

<style lang='scss' scoped>
.van-popup {
  width: 90%;
  height: 5rem;
  border-radius: 0.08rem;

  & > div {
    width: 100%;
    height: 100%;
    overflow: hidden;
  }

  img {
    width: 100%;
    height: 100%;
  }
}

.fixed {
  position: fixed;
  width: 100%;
  bottom: 0;
  box-sizing: content-box;
  height: 0.88rem;
  background-color: #2ea2fa;
  font-family: PingFang-SC-Bold;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #ffffff;
  border: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

@supports (bottom: env(safe-area-inset-bottom)) {
  .fixed {
    padding-bottom: env(safe-area-inset-bottom);
  }
}

.htImg {
  & > p {
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #333333;
    padding: 0.2rem 0;
    padding-bottom: 0;
  }

  & > div {
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;

  
  }
}

.status {
  height: 1.68rem;
  padding: 0.3rem 0.3rem 0 0.3rem;

  & > p {
    font-family: PingFang-SC-Bold;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #333333;
  }

  .statusInfo {
    display: flex;
    padding-top: 0.2rem;

    & > div {
      display: flex;
      align-items: center;

      span {
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0.01rem;
        color: #333333;
      }
    }

    & > div:last-child {
      margin-left: 1.2rem;
    }

    img {
      width: 0.32rem;
      height: 0.32rem;
      margin-right: 0.2rem;
    }
  }
}

.cause {
  height: 2.48rem;

  & > div {
    padding-top: 0.2rem;
    position: relative;

    p {
      position: absolute;
      right: 0.1rem;
      bottom: 0.1rem;
      font-family: PingFang-SC-Medium;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      color: #999999;
    }
  }

  textarea {
    border: 0;
    width: 100%;
    height: 1.28rem;
    background-color: #f6f6f6;
    border-radius: 0.04rem;
    font-family: PingFang-SC-Medium;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #999999;
    padding: 0.2rem 0.3rem;
    box-sizing: border-box;
    overflow: hidden;
    resize: none;
  }
}

.checkoutImg {
  height: auto;
  padding-bottom: 1.5rem;
}
.imgsList {
  margin-left: 0.1rem;
  width: 2.16rem;
  height: 2.16rem;
  display: flex;
  justify-content: center;
  align-items: center;

  img {
    width: auto;
    height: auto;
    max-height: 100%;
    max-width: 100%;
  }
}
</style>
