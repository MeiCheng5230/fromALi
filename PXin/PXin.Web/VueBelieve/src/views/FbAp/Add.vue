<!-- 新增代理人 -->
<template>
  <div class="addDealer">
    <div class="content">
      <p>
        <input :placeholder="$t('lang.add_placeholder')" type="text" v-model="serchText" />
        <button @click="SearchUser">{{$t('lang.add_search')}}</button>
      </p>
      <div class="user">
        <ul>
          <li>
            <span>{{$t('lang.add_user')}}</span>
            <span>{{user.nodecode }}</span>
          </li>
          <li>
            <span>{{$t('lang.add_name')}}</span>
            <span>{{user.nodename }}</span>
          </li>
          <li>
            <span>{{$t('lang.add_phone')}}</span>
            <span>{{user.mobileno }}</span>
          </li>
          <li>
            <span>DOS{{$t('lang.add_balance')}}</span>
            <span>{{balance}}DOS</span>
          </li>
          <li>
            <span>{{$t('lang.add_Need')}}DOS</span>
            <span>0DOS</span>
          </li>
        </ul>
      </div>
      <router-link
        :style="ispass ? 'background:#2ea2fa' :'background:#D1D1D1'"
        class="btn"
        to
        tag="div"
        @click.native="addDealer"
      >{{$t('lang.add_Openingagent')}}</router-link>
    </div>

  </div>
</template>

<script>
// 新增经销商
import { GetStockBalance, SearchUser, AddUserJxs } from "@/api/getFbApData";
import { NameFilter, phoneFilter } from '@/config/utils' ;
export default {
  data() {
    return {
      balance: 0, //DOS余额
      serchText: "", //搜索关键字
      user: "", //用户信息
      nodeid: "", //新增经销商的NodeId ,
      ispass: false // 是否通过查询
    };
  },
  created() {
    this.GetStockBalance(); //获取DOS余额
  },
  mounted() {
    this.dosPayResult() ;//dos回调
  },
  watch: {
    serchText(n) {
      this.ispass = false;
    },
  },
  methods: {
    //dos回调
    dosPayResult(){
      let _this = this;
      window.dosPayResult = function(obj) {
        try {
          var ret = JSON.parse(obj);
          if (ret.result == undefined) {
            // throw new Error("解析错误");
            throw new Error(this.$t('lang.add_Parsingerror'));
          }
          if (ret.result <= 0) {
            _this.Toast(ret.message);
            return;
          } else {
            // _this.Toast("新增成功");
            _this.Toast(this.$t('lang.add_Parsingerror'));
            setTimeout(() => {
              _this.$router.go(-1);
            }, 1000);
          }
        } catch (e) {
          // _this.Toast("支付异常:" + obj);
          _this.Toast(this.$t('lang.add_payabnormal') + obj);
        }
      };
    },
    //开通经销商
    addDealer() {
      if (!this.ispass) return;
      this.AddUserJxs();
    },
    //验证条件
    testCon(){
      if (!this.serchText) {
        // this.Toast("请输入代理人账号");
        this.Toast(this.$t('lang.add_hint'));
        return -1;
      }
      if (!!!this.ispass) {
        // this.Toast("请查询代理人账号并核对信息");
        this.Toast(this.$t('lang.add_hint2'));
        return -1;
      }
      return 1 ;
    },
    //  /api/Bts/AddUserJxs 开通经销商
    async AddUserJxs() {
      if(this.testCon() == -1 ) return ;

      let result = await AddUserJxs(
        JSON.parse(sessionStorage.userParam),
        this.nodeid,
        this.$route.query.infoid
      );
      if (result.result > 0) {
        if (!result.data.chargestr  && !result.data.sign  && !result.data.orderno) {
          // this.Toast("添加成功,请耐心等待该用户同意结果");
          this.Toast(this.$t('lang.add_hint3'));
          setTimeout(() => {
            this.$router.go(-1);
          }, 1000);
        } else {
          try {
            AppNative.blJsTunedupNativeWithTypeParamSign(
              1003,
              result.data.chargestr,
              result.data.sign
            );
          } catch (e) {
            this.Toast(e);
            // this.Toast("调起码库支付失败");
            this.Toast(this.$t('lang.add_ueError'));
          }
        }
      } else {
        this.Toast(result.message);
      }
    },
    ///api/Bts/SearchUser查询用户
    async SearchUser() {
      if (!this.serchText) {
        // this.Toast("请输入代理人帐号");
        this.Toast(this.$t('lang.add_hint'));
        return;
      }
      let result = await SearchUser(
        JSON.parse(sessionStorage.userParam),
        this.serchText,
        1
      );
      if (result.result > 0) {
        this.ispass = true;
        this.nodeid = result.data.nodeid;
        result.data.nodename =  NameFilter(result.data.nodename);
        if (result.data.mobileno) {
          result.data.mobileno = phoneFilter(result.data.mobileno) ;
        }

        this.user = result.data;
      } else {
        this.Toast(result.message);
        this.ispass = false;
        if (this.user) {
          this.user.nodecode = "";
          this.user.nodename = "";
          this.user.mobileno = "";
        }
        this.balance = 0;
      }
    },
    async GetStockBalance() {
      let result = await GetStockBalance({
        ...JSON.parse(sessionStorage.userParam),
        type: 1
      });
      if (result.result > 0) {
        this.balance = result.data;
      } else {
        this.Toast(result.message);
      }
    }
  },

};
</script>

<style scoped lang='scss'>
/* 新增经销商 */
.addDealer {
  height: 100%;
  background: rgba(244, 244, 244, 1);

  .btn {
    background: #2ea2fa;
    border-radius: 0.06rem;
    height: 0.88rem;
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 1rem;
    font-size: 0.3rem;
    font-family: PingFang-SC-Bold;
    font-weight: bold;
    color: rgba(255, 255, 255, 1);
  }

  .user {
    margin-top: 0.6rem;
    background: #fff;
    padding: 0 0.3rem;
    box-shadow: 4px 4px 10px 0px rgba(209, 209, 209, 1);
    border-radius: 6px;

    li {
      height: 0.98rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 0.01rem solid rgba(209, 209, 209, 1);
      font-size: 0.3rem;
      font-family: PingFang-SC-Medium;
      font-weight: 500;
      color: rgba(51, 51, 51, 1);
    }

    li:last-child {
      border-bottom: 0;

      span:nth-child(2) {
        color: #ffb90f;
      }
    }
  }

  .content {
    padding: 0.3rem 0.2rem;

    & > p:nth-child(1) {
      display: flex;
      align-items: center;
      height: 0.88rem;
      box-shadow: 4px 4px 10px 0px rgba(209, 209, 209, 1);

      input {
        height: 100%;
        width: 5.8rem;
        border: 0;
        padding-left: 0.3rem;
        border-radius: 0.06rem 0 0 0.06rem;
      }

      button {
        height: 100%;
        width: 1.3rem;
        border: 0;
        border-radius: 0.06rem;
        background: rgba(81, 148, 231, 1);
        font-size: 0.3rem;
        font-family: PingFang-SC-Medium;
        font-weight: 500;
        color: rgba(255, 255, 255, 1);
      }
    }
  }
}
</style>
