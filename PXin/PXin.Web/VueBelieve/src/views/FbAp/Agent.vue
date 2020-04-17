<!-- 代开充值商 -->
<template>
  <div class="addDealer">
    <div class="content">
      <p>
        <input
          :placeholder="$t('lang.add_placeholder')"
          type="text"
          @click="setClc"
          @keyup="serchText=serchText.replace(/\D/g,'')"
          v-model="serchText"
        />
        <button @click="SearchUser(serchText, 3)">{{$t('lang.add_search')}}</button>
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
            <span>SV{{$t('lang.agent_stockbalance')}}</span>
            <span>{{balance}}</span>
          </li>
          <li>
            <span>{{$t('lang.agent_toPay')}}</span>
            <span>
              <div class="payNum">1500DOS+50000SV{{$t('lang.agent_RetailCode')}}</div>
            </span>
          </li>
        </ul>
      </div>
      <div class="area">
        <div class="lft">{{$t('lang.agent_region')}}</div>
        <div class="rgt">
          <div class="city" @click="areashow = true">
            <span v-show="!region.length">{{$t('lang.agent_selectregion')}}</span>
            <span
              v-if="region.length"
              class="areadet"
            >{{region[0].name}} {{region[1].name}} {{region[2].name}}</span>
            <van-icon name="arrow" />
          </div>
        </div>
      </div>
      <div :class="ispass ? 'btn active' : 'btn'" @click="OpenCzs">{{$t('lang.agent_add')}}</div>
    </div>
    <van-popup position="bottom" v-model="areashow">
      <van-area :area-list="areaList" @cancel="areashow=false" @confirm="confirm" />
    </van-popup>
    <div class="tips">
      <div class="tit">{{$t('lang.agent_Reminder')}}：</div>
      <p>1.{{$t('lang.agent_hint1')}}</p>
      <p>2.{{$t('lang.agent_hint4')}}</p>
      <p>3.{{$t('lang.agent_hint5')}}</p>
    </div>
    <keyboard theme="keyboard" :isKeyboard="isPay" @pay="setPay" @close="isPay = false"></keyboard>
  </div>
</template>

<script>
// 新增经销商
import { SearchUser, OpenCzs, GetStockBalance } from "@/api/getFbApData";
import Vue from "vue";
import vueKeyboard from "vue-keyboard-su-link/dist/vueKeyboard";
Vue.use(vueKeyboard);
import area from "@/api/Area";
import { NameFilter, phoneFilter } from '@/config/utils' ;
export default {
  data() {
    return {
      isPay: false, //支付组件开关
      balance: 0, //SV零售码库存余额
      serchText: "", //搜索关键字
      user: "", //用户信息
      nodeid: "", //新增经销商的NodeId ,
      password: "", //支付密码
      ispass: false, // 是否通过查询
      areaList: area, // 地区列表数据
      areashow: false, // 显示地区选择
      region: [] // 选中的地区
    };
  },
  mounted() {
    this.GetStockBalance();
    this.dosPayResult() ; //dos回调
  },
  watch: {
    user(info) { 
      this.ispass =  (info && this.region.length) ? true : false ;
    },
    region(arr) {
      this.ispass = (arr.length && this.user) ? true : false ;
    }
  },
  methods: {
    //dos回调
    dosPayResult(){
      let _this = this;
      window.dosPayResult = function(obj) {
        try {
          var ret = JSON.parse(obj);
          if (ret.result == undefined) {
            throw new Error("解析错误");
            // throw new Error(this.$t("lang.add_Parsingerror"));
          }
          if (ret.result <= 0) {
            _this.Toast(ret.message);
            return;
          } else {
            _this.Toast("开通成功");
            // _this.Toast(this.$t("lang.agent_success"));
            setTimeout(() => {
              _this.$router.go(-1);
            }, 1000);
          }
        } catch (e) {
          // _this.Toast("支付异常:" + obj);
          _this.Toast(this.$t("lang.add_payabnormal") + obj);
        }
      };
    },
    confirm(val) {
      this.region = val;
      this.areashow = false;
    },
    //开通经销商
    addDealer() {
      if (!this.ispass) return;
      this.isPay = true; //打开支付组件
    },
    // 接收子组件数据
    setPay(pwd) {
      this.OpenCzs(
        btoa(pwd),
        this.nodeid,
        this.region[0].name,
        this.region[1].name,
        this.region[2].name
      );
      this.isPay = false;
    },
    async GetStockBalance() {
      let result = await GetStockBalance({
        ...JSON.parse(sessionStorage.userParam),
        type: 2
      });
      if (result.result > 0) {
        this.balance = result.data;
      } else {
        this.Toast(result.message);
      }
    },
    // 开通充值商
    async OpenCzs() {
      let newnodeid = this.nodeid;
      let province = this.region[0].name;
      let city = this.region[1].name;
      let region = this.region[2].name;
      let res = await OpenCzs({
        ...JSON.parse(sessionStorage.userParam),
        newnodeid,
        province,
        city,
        region
      });
      if (res.result > 0) {
        try {
          AppNative.blJsTunedupNativeWithTypeParamSign(
            1003,
            res.data.chargestr,
            res.data.sign
          );
        } catch (e) {
          this.Toast("调起码库支付失败");
        }
      } else {
        this.Toast(res.message);
      }
    },
    ///api/Bts/SearchUser查询用户
    async SearchUser(key, type) {
      if (!this.serchText) {
        // this.Toast("请输入对方帐号或手机号");
        this.Toast(this.$t("lang.add_placeholder"));
        return;
      }
      let result = await SearchUser(
        JSON.parse(sessionStorage.userParam),
        key,
        type
      );
      if (result.result > 0) {
        this.ispass = true;
        this.nodeid = result.data.nodeid;

        result.data.nodename = NameFilter(result.data.nodename) ;
        result.data.parentname = result.data.parentname;
        if (result.data.mobileno) {
          result.data.mobileno = phoneFilter(result.data.mobileno) ;
        }
        this.user = result.data;
      } else {
        this.Toast.fail(result.message);
        this.ispass = false;
        if (this.user) {
          this.user.nodecode = "";
          this.user.nodename = "";
          this.user.mobileno = "";
        }
      }
    },
    setClc(e) {
      e.target.focus();
    }
  }
};
</script>

<style scoped lang='scss'>
/* 新增经销商 */
.addDealer {
  height: 100%;
  background: rgba(244, 244, 244, 1);

  .btn {
    background: rgba($color: #2ea2fa, $alpha: 0.5);
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
    &.active {
      background: #2ea2fa;
    }
  }
  .area {
    background: #fff;
    padding: 0.2rem 0.3rem;
    display: flex;
    margin-top: 0.3rem;
    box-shadow: 0 0.04rem 0.1rem 0 rgba(209, 209, 209, 1);
    .lft {
      flex: auto;
    }
    .rgt {
      .city {
        display: flex;
        align-items: center;
        .areadet {
          width: 4rem;
          text-overflow: ellipsis;
          white-space: nowrap;
          overflow: hidden;
          text-align: right;
        }
      }
    }
  }
  .user {
    margin-top: 0.6rem;
    background: #fff;
    padding: 0 0.3rem;
    box-shadow: 0 0.04rem 0.1rem 0 rgba(209, 209, 209, 1);
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
      span {
        text-align: right;
        .payNum {
          color: #2ea2fa;
        }
        .oriNum {
          color: #999;
          text-decoration: line-through;
        }
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
        &::placeholder {
          color: #999;
        }
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
  .tips {
    letter-spacing: 1px;
    color: #999999;
    font-size: 0.24rem;
    padding: 0 0.3rem 0.3rem 0.56rem;
    line-height: 0.46rem;
    .tit {
      font-size: 0.3rem;
      color: #333;
    }
  }
}
</style>
