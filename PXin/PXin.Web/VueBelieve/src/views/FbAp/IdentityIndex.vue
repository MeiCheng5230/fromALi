<template>
  <div class="ident" >
    <!-- 不通过原因 -->
    <div class='cause' v-if='statusdesc=="审核拒绝"'>
      审核未通过原因 : <span> {{reason}}</span>
    </div>
    <div class="opt">
      <div class="item">
        <div class="lft">
          <div class="tit">个人资料认证</div>
          <div class="txt">身份证正反面、手持身份证照片</div>
        </div>
        <router-link :to='{path:"/IdentityPerson",query:{infoid:$route.query.infoid,statusdesc:statusdesc,typeid:typeid}}'
          tag='div' class="rgt">
          <span class="col">{{statusdesc}}</span><img src="@/assets/images/ic_enter@2x.png" alt="">
        </router-link>
      </div>
      <div class="item" v-show="typeid==4">
        <div class="lft">
          <div class="tit">公司资料认证</div>
          <div class="txt">营业执照原件</div>
        </div>
        <router-link :to='{path:(statusdesc=="未上传资料"||statusdesc=="审核拒绝")?"/IdentityPerson":"/IdentityCompany",query:{infoid:$route.query.infoid,statusdesc:statusdesc,typeid:typeid}}'
          tag='div' class="rgt">
          <span class="col">{{statusdesc}}</span><img src="@/assets/images/ic_enter@2x.png" alt="">
        </router-link>
      </div>
      <div class="item" v-show="statusdesc == '审核通过'">
        <div class="lft">
          <div class="tit">补充资料</div>
          <div class="txt">需要补充的相关资料</div>
        </div>
        <div @click='toSuppl' class="rgt">
          <span class="col">{{supplementstatusdesc}}</span><img src="@/assets/images/ic_enter@2x.png" alt="">
        </div>
      </div>
    </div>
    <div class="text">
      <p>说明：</p>
      <p>1.请上传真实有效的资料，如有虚假将导致账号冻结无法使用；</p>
      <p>2.上传资料前请认真阅读《相信充值商服务协议》；</p>
      <p>3.充值商资料由‘相信官方’审核，代理人资料由上级‘充值商’审核，如有疑问请联系工作人员或上级充值商。</p>
    </div>
  </div>
</template>

<script>
  import { Toast } from 'vant';
  import Vue from 'vue';
  Vue.use(Toast);
  import { GetAuditStatus } from '@/api/getFbApData';
  export default {
    data() {
      return {
        statusdesc: '', //审核状态
        supplementstatusdesc: '', //补充资料审核状态
        reason:'',
        typeid:0,
      }
    },
    created() {
      this.GetAuditStatus(); //获取审核状态
    },
    methods: {
      copyFn: function() {
        var Url2 = document.getElementById("copy").innerText;
        var oInput = document.createElement('input');
        oInput.value = Url2;
        document.body.appendChild(oInput);
        oInput.select(); // 选择对象
        document.execCommand("Copy"); // 执行浏览器复制命令
        oInput.className = 'oInput';
        oInput.style.display = 'none';

        Toast('复制成功');
      },
      //  /api/Bts/GetAuditStatus获取审核状态
      async GetAuditStatus() {
      
        let result = await GetAuditStatus(JSON.parse(sessionStorage.userParam), this.$route.query.infoid);
        if (result.result > 0) {
          this.statusdesc = result.data.statusdesc;
          this.supplementstatusdesc = result.data.supplementstatusdesc;
          this.reason=result.data.reason;
          this.typeid=result.data.typeid;
        } else {
          this.Toast(result.message);
        };
      },
      //跳转补充资料页面
      toSuppl() {
        if (this.statusdesc == '审核通过') {
          this.$router.push({
            path: "/Material",
            query: {
              infoid: this.$route.query.infoid,
              supplementstatusdesc: this.supplementstatusdesc
            }
          });
        } else {
          this.Toast('请等待以上资料审核通过~');
        }
      }
    },
  }
</script>

<style lang="scss" scoped>
  .ident {
    height: 100%;
    padding-top: .2rem;
    box-sizing: border-box;
    background: rgba(247, 247, 252, 1);

    /deep/ .header {
      background: #fff;
    }

    .opt {
      // padding-top: 1rem;
      // border-top: .02rem solid #DEDEDE;

      .item {
        display: flex;
        background: #fff;
        margin-bottom: 0.2rem;
        padding: 0.2rem 0.3rem;

        .lft {
          flex: auto;

          .tit {
            font-size: 0.32rem;
            font-family: PingFang-SC-Medium;
            font-weight: 500;
            color: rgba(51, 51, 51, 1);
          }

          .txt {
            font-size: 0.24rem;
            font-family: PingFang-SC-Medium;
            font-weight: 500;
            color: rgba(153, 153, 153, 1);
            margin-top: 0.1rem;
          }
        }

        .rgt {
          display: flex;
          align-items: center;
          font-size: 0.26rem;

          img {
            width: 0.36rem;
            height: auto;
            margin-left: 0.1rem;
          }

          span {
            color: #999;
          }

          .col {
            color: #2ea2fa;
          }
        }
      }
    }

    .text {
      padding: 0.3rem;
      font-size: 0.24rem;
      color: #999;
      line-height: 0.44rem;
      letter-spacing: 0.02rem;
    }

    .copyBox {
      display: flex;
      justify-content: center;

      .copyCom {
        border-radius: 0.06rem;
        overflow: hidden;
        display: flex;

        .url {
          background: #fff;
          padding: 0.15rem 0.2rem;
          color: #999;
        }

        .copyBtn {
          background: #2ea2fa;
          color: #fff;
          display: flex;
          align-items: center;
          padding: 0 0.35rem;
        }
      }

    }
    .cause {
      padding:0.1rem  .3rem;
      box-sizing: border-box;
      font-size:0.28rem;
      line-height: .4rem;
      background: #FEFCEB;
      font-family: PingFang-SC-Medium;
      font-weight: 0;
      color: #333333;
      // white-space: nowrap;
      // display:flex;
      span{
        // padding-left: .1rem;
        // white-space: wrap;
        // flex: 1;

        color: #FFB90F;
        font-weight: bold;
        // text-overflow: ellipsis;
        // overflow: hidden;
      }
    }
  }
</style>
