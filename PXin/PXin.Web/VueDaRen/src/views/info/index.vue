<template>
  <div class="info">
    <van-notice-bar
      wrapable
      v-if="status==2"
      :scrollable="false"
      color="#ff722b"
      background="#ffd7c3"
    >拒绝理由：{{ reason }}</van-notice-bar>
    <div class="conBx">
      <div class="ops img" @click="SetApp(1006)">
        <div class="lft">个人头像</div>
        <div class="rgt">
          <div class="headImg">
            <img :src="appphoto" alt />
          </div>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </div>
      <div class="ops" @click="SetApp(1007)">
        <div class="lft">签名</div>
        <div class="rgt">
          <span class="set" v-if="!autograph">
            <span class="star">*</span>
            <span>去设置</span>
          </span>
          <span class="has" v-else>{{ autograph }}</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </div>
      <div class="ops" @click="SetApp(1008)">
        <div class="lft">手机号</div>
        <div class="rgt">{{ phoneNum }}</div>
        <van-icon name="arrow" size="0.3rem" />
      </div>
    </div>
    <div class="conBx">
      <router-link to="/introduce" tag="div" class="ops img">
        <div class="lft">自我介绍</div>
        <div class="rgt">
          <span class="set" v-if="!selfintroduction">
            <span class="star">*</span>
            <span>请输入自我介绍</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <div class="ops" @click="SetIden()">
        <div class="lft">实名认证</div>
        <div class="rgt">
          <span class="set" v-if="isauth != 1">
            <span class="star">*</span>
            <span>去认证</span>
          </span>
          <span class="has" v-else>已认证</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </div>
      <router-link to="/sort" tag="div" class="ops">
        <div class="lft">专业区域</div>
        <div class="rgt">
          <span class="set" v-if="!majors.length">
            <span class="star">*</span>
            <span>请选择</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/vocation" tag="div" class="ops">
        <div class="lft">职业背景</div>
        <div class="rgt">
          <span class="set" v-if="!isoccupation">
            <span class="star">*</span>
            <span>去填写</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/education" tag="div" class="ops">
        <div class="lft">教育背景</div>
        <div class="rgt">
          <span class="set" v-if="!isedu">去填写</span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/identify" tag="div" class="ops">
        <div class="lft">专业资格认证</div>
        <div class="rgt">
          <span class="set" v-if="!professionalpics.length">去认证</span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/explain" tag="div" class="ops">
        <div class="lft">达人达语</div>
        <div class="rgt">
          <span class="set" v-if="!greetings">
            <span class="star">*</span>
            <span>去设置</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/knowlege" tag="div" class="ops">
        <div class="lft">我的知识库</div>
        <div class="rgt">
          <span class="set" v-if="isknowledge==0">
            <span>去设置</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <router-link to="/video" tag="div" class="ops">
        <div class="lft">我的视频</div>
        <div class="rgt">
          <span class="set" v-if="isvideo==0">
            <span>去上传</span>
          </span>
          <span class="has" v-else>已上传</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
      <div class="ops" v-if="srcstatus==3">
        <div class="lft" style="flex: auto;">倍率保护</div>
        <van-switch :value="protect" @input="SetProtect" />
      </div>
    </div>
    <div class="conBx">
      <router-link to="/remarks" tag="div" class="ops">
        <div class="lft">欢迎语</div>
        <div class="rgt">
          <span class="set" v-if="!welcome">
            <span class="star">*</span>
            <span>去设置</span>
          </span>
          <span class="has" v-else>已保存</span>
        </div>
        <van-icon name="arrow" size="0.3rem" />
      </router-link>
    </div>
    <div class="btnBx">
      <button v-if="status==0" :class="subflag?'subtn':''" @click="SetSub">提交审核</button>
      <button v-if="status==2" :class="subflag?'subtn':''" @click="SetSub">重新提交</button>
      <button v-if="status==1">提交审核中</button>
    </div>
  </div>
</template>

<script>
import {
  GetAbovementionedData,
  VerifyDaRen,
  SetProtectRate
} from "@/api/getData";
import { Dialog } from "vant";
import axios from "axios";
import { setStore, clearStore } from "@/config/utils";
export default {
  data() {
    return {
      appphoto: "", // 头像
      autograph: "", // 签名
      phone: "", // 手机号
      majors: "", // 专业领域
      isedu: "", // 是否填写教育背景 0=否 1=是 ,
      isoccupation: "", //  是否填写职业背景 0=否 1=是
      professionalpics: [], // 专业资格认证图片 ,
      selfintroduction: "", // 自我介绍
      pic: [], // 自我介绍图片
      greetings: "", // 达人达语
      name: "", // 姓名
      srcstatus: 0,
      status: 0, // 状态    0=不是达人(未填写资料) 1=申请中(已填写资料,但未审核) 2=申请未通过(已填写资料,审核未通过) 3=是达人(通过审核)
      isauth: "", // 是否实名认证 0=否 1=是 ,
      subflag: false, // 是否可以提交
      welcome: "", // 欢迎语
      reason: "",
      isChange: "",
      isknowledge: "", // 是否填写了我的知识库数据
      isvideo: [], // 我的视频地址
      protect: false // 倍率保护
    };
  },
  created() {
    this.GetAbovementionedData();
    clearStore();
  },
  updated() {
    if (
      this.autograph &&
      this.phone &&
      this.selfintroduction &&
      this.majors &&
      this.isoccupation == 1 &&
      this.greetings &&
      this.isauth == 1
    ) {
      this.subflag = true;
    } else {
      this.subflag = false;
    }
  },
  computed: {
    phoneNum() {
      if (this.phone && this.phone.length > 0) {
        return this.phone.substr(0, 4) + "****" + this.phone.substr(-4);
      }
    }
  },
  methods: {
    async GetAbovementionedData() {
      let result = await GetAbovementionedData(this.$global.userInfo);
      if (result.result > 0) {
        this.srcstatus = result.data.status;
        if (result.data.ischange == 1 && result.data.status) {
          result.data.status = 0;
        }
        let {
          appphoto,
          autograph,
          phone,
          majors,
          isedu,
          isoccupation,
          professionalpics,
          selfintroduction,
          pic,
          greetings,
          isauth,
          name,
          status,
          welcome,
          note,
          ischange,
          isknowledge,
          isvideo,
          isprotectrate
        } = result.data;
        this.appphoto = appphoto;
        this.autograph = autograph;
        this.phone = phone;
        this.majors = majors || [];
        this.isedu = isedu;
        this.isoccupation = isoccupation;
        this.professionalpics = professionalpics || [];
        this.selfintroduction = selfintroduction;
        this.pic = pic;
        this.greetings = greetings;
        this.name = name;
        this.status = status;
        this.welcome = welcome;
        this.isauth = isauth;
        this.reason = note;
        this.isChange = ischange;
        this.isknowledge = isknowledge;
        this.isvideo = isvideo;
        this.protect = Boolean(isprotectrate);
        setStore("info", JSON.stringify(result.data));
      } else {
        this.Toast(result.message);
      }
    },
    SetProtect(checked) {
      let word = "开启";
      if (!checked) {
        word = "关闭";
      }
      Dialog.confirm({
        title: "温馨提示",
        message: "您确定要" + word + "倍率保护？"
      }).then(async () => {
        let result = await SetProtectRate({
          ...this.$global.userInfo,
          status: Number(checked)
        });
        if (result.result > 0) {
          this.protect = checked;
        } else {
          this.Toast.fail(result.message);
        }
      });
    },
    SetSub() {
      if (this.status == 0 && this.subflag) {
        // 提交审核
        this.VerifyDaRen();
      } else if (this.subflag && this.status == 2) {
        this.VerifyDaRen();
      }
    },
    async VerifyDaRen() {
      let result = await VerifyDaRen(this.$global.userInfo);
      if (result.result > 0) {
        this.Toast.success(result.message);
        if (this.ischange == 1) {
          this.status = 3;
        } else {
          this.status = 1;
        }
      } else {
        this.Toast.fail(result.message);
      }
    },
    SetApp(id) {
      try {
        AppNative.blJsTunedupNativeWithTypeParamSign(id, "", "");
      } catch (error) {
        this.Toast.fail("请在相信App中打开");
      }
    },
    SetIden() {
      if (this.isauth == 1) {
        try {
          AppNative.blJsTunedupNativeWithTypeParamSign(1010, "", "");
        } catch (error) {
          this.Toast.fail("请在相信App中打开");
        }
      } else {
        try {
          AppNative.blJsTunedupNativeWithTypeParamSign(1009, "", "");
        } catch (error) {
          this.Toast.fail("请在相信App中打开");
        }
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.info {
  background: #f7f7fc;
  min-height: 100%;
  .conBx {
    background: #fff;
    padding: 0 0.3rem;
    margin-bottom: 0.3rem;
    .ops {
      border-bottom: 1px solid #f7f7fc;
      display: flex;
      align-items: center;
      padding: 0.3rem 0;
      .rgt {
        flex: auto;
        text-align: right;
        margin-right: 0.1rem;
        .set {
          color: #999;
          .star {
            color: #ff1541;
            font-weight: bold;
            font-size: 0.3rem;
            margin-right: 0.1rem;
          }
        }
      }

      &.img {
        padding: 0.2rem 0;
        .rgt {
          .headImg {
            width: 0.9rem;
            height: 0.9rem;
            border-radius: 0.9rem;
            overflow: hidden;
            display: inline-block;
            img {
              width: 100%;
              max-height: 100%;
            }
          }
        }
      }
    }
  }

  .btnBx {
    padding: 0.6rem 0.3rem;
    button {
      width: 100%;
      padding: 0.28rem;
      border: none;
      outline: none;
      background: rgba($color: #2ea2fa, $alpha: 0.5);
      border-radius: 0.04rem;
      color: #fff;
      &.subtn {
        background: #2ea2fa;
      }
    }
  }

  .van-switch {
    font-size: 20px;
  }
}
</style>
