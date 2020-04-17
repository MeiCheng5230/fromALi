<template>
  <div class="renew" >
    <div class="bgImg">
      <img src="@/assets/images/vip_renew_bg.png" alt="">
    </div>
    <div class="list">
      <div class="ops tit">
        <div class="lft">当前级别</div>
        <div class="rgt">{{user.typeiddesc}}</div>
      </div>
      <div class="ops">
        <div class="lft">用户名称</div>
        <div class="rgt">{{user.nodename}}</div>
      </div>
      <div class="ops">
        <div class="lft">到期时间</div>
        <div class="rgt">{{user.endtime }}</div>
      </div>
      <div class="ops">
        <div class="lft">续费时长</div>
        <div class="rgt">一年</div>
      </div>
      <div class="ops">
        <div class="lft">应付费用</div>
        <div class="rgt"><span>{{$route.query.isown?'500':'1500'}}</span>DOS/年</div>
      </div>
    </div>
    <div class="btn" @click='pay'>确定续费，去支付</div>
  </div>
</template>

<script>
  import { GetRenewInfo, Renew } from '@/api/getFbApData';
  export default {
    data() {
      return {
        user: null, //用户信息
      }
    },
    created() {
      this.user = JSON.parse(this.$route.query.userJxs);
      this.GetRenewInfo() ;
    },
    mounted() {
      this.dosPayResult() ;//dos 支付回调
    },
    methods: {
      //dos 支付回调
      dosPayResult(){
        let _this = this;
        window.dosPayResult = function(obj) {
          try {
            var ret = JSON.parse(obj);
            if (ret.result == undefined) {
              throw new Error("解析错误");
            }
            if (ret.result <= 0) {
              _this.Toast(ret.message);
              return;
            } else {
              _this.Toast("续费成功");
              setTimeout(()=>{
                _this.$router.go(-1);
              },1000);
            }
          } catch (e) {
            _this.Toast("支付异常:" + obj);
          }
        }
      },
      async BtsRenew() { //  /api/Bts/Renew续费
        let result;
        if (!this.$route.query.infoid) {
          result = await Renew(JSON.parse(sessionStorage.userParam));
        } else {
          result = await Renew(JSON.parse(sessionStorage.userParam), JSON.parse(this.$route.query.infoid));
          if (result.result > 0) {
            try {
              AppNative.blJsTunedupNativeWithTypeParamSign(1003,result.data.chargestr, result.data.sign);
            } catch (e) {
              this.Toast(e);
              this.Toast("调起码库支付失败");
            }
          } else {
            this.Toast(result.message);
          }
        }
      },
      // /api/Bts/GetRenewInfo获取续费信息
      async GetRenewInfo() {
        let result = await GetRenewInfo(JSON.parse(sessionStorage.userParam), this.user.infoid);
        if (result.result > 0) {
          this.user = result.data;
        } else {
          this.Toast(result.message);
        };
      },
      //点击支付
      async pay() {
        this.BtsRenew();
      }
    },
  }
</script>

<style lang="scss" scoped>
  .renew {
    height: 100%;
    background: rgba(244, 244, 244, 1);

    /deep/ .header {
      background: #fff;
    }

    .bgImg {
      img {
        width: 100%;
        height: auto;
      }
    }

    .list {
      background: #fff;
      margin: 0 0.5rem;
      position: relative;
      top: -0.5rem;
      border-radius: 0.06rem;

      .ops {
        color: #666;
        display: flex;
        padding: 0.3rem 0;
        margin: 0 0.3rem;
        border-bottom: 1px solid #f4f4f4;

        .lft {
          flex: auto;
        }

        .rgt {
          color: #333;

          span {
            color: #FF3030;
          }
        }
      }

      .tit {
        margin: 0;
        padding: 0.3rem;
        box-shadow: 0 0.05rem 0.1rem rgba(224, 224, 224, 1);
        border-radius: 0.06rem;

        .lft {
          color: #333;
        }

        .rgt {
          color: #FF3030;
        }
      }
    }

    .btn {
      margin: 0.5rem;
      padding: 0.24rem 0;
      text-align: center;
      background: #2ea2fa;
      border-radius: 0.06rem;
      color: #fff;
    }
  }
</style>
