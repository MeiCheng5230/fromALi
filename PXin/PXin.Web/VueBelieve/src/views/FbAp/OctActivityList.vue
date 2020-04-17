<template>
  <div class="octList" v-if="dataLoad">
    <div class="tabs">
      <van-tabs v-model="tabsIndex" @change="tabsChange">
        <van-tab :title="item" v-for="(item,index) of typeList" :key="index">
          <div class="content">
            <table></table>
            <!-- 暂无记录 -->
            <noData
              v-if="empty && tabsIndex==0 && paylist.length==0"
              text="暂无数据"
              :img="require('@/assets/images/6.png')"
            ></noData>
            <noData
              v-if="empty && tabsIndex==1 && receivelist.length==0"
              text="暂无数据"
              :img="require('@/assets/images/6.png')"
            ></noData>
            <div
              class="infos"
              v-for="(item,index) of tabsIndex==0  ? paylist:receivelist"
              :key="index"
              :class="(payStatus && tabsIndex==1 && item.mystatus!=1) ? 'payStatus':''"
              @click="checked(item,index)"
            >
              <div>
                <div class="title">
                  <div>{{getUserType(item)}}</div>
                  <span :class="item.paystatus && 'pay'">{{item.paystatus==0?"对方未支付":"对方已支付"}}</span>
                </div>
                <div class="name">{{item.nodename}}（{{item.nodecode}}）</div>
                <div>{{item.createtime}}</div>
              </div>
              <!-- 右侧勾选 -->
              <div class="right">
                <div class="logistics" v-if="item.mystatus">
                  <div>{{getPayStatus(item)}}</div>
                  <div>
                    <button
                      v-if="item.mystatus==3"
                      @click="$router.push({path:'/logistics',query:{expressno:item.expressno}})"
                    >查看物流</button>
                  </div>
                </div>

                <div class="rightImg" v-if="!item.mystatus">
                  <div v-show=" tabsIndex==0 || !payStatus">
                    <img
                      v-if="!item.mystatus "
                      :src="item.isChecked? require('@/assets/images/ic_seleted_pay.png'):require('@/assets/images/ic_unseleted_pay.png')"
                      alt
                    />
                  </div>
                </div>
              </div>
            </div>
            <!--  -->
            <div style="height:1.3rem;"></div>
          </div>
        </van-tab>
      </van-tabs>
    </div>
    <!--  -->
    <div class="footer" v-show="(tabsIndex==1 && receivelist.length>0 && payStatus==false) || (this.tabsIndex==0 && paylist.length>0 && condition) ">
      <div v-show="!tabsIndex" class="allChecked" @click="AllChecked">
        <img
          :src="isAllChecked ? require('@/assets/images/ic_seleted_all.png'):require('@/assets/images/ic_unseleted_all.png')"
          alt
        />
        全选
      </div>
      <div class="totails" :class="tabsIndex &&  'status2'">
        <div class="count">
          合计：
          <span>{{totail}}</span>
        </div>
        <div class="payBtn" @click="OctoberActivityDosUEPrepare">去支付</div>
      </div>
    </div>
  </div>
</template>
<script>
import {
  GetOctoberActivityList,
  OctoberActivityDosUEPrepare,
} from "@/api/getFbApData";
import { getUrlParams , NameFilter } from '@/config/utils'
import noData from "@/components/noData";
export default {
  data() {
    return {
      condition:false,
      tabsIndex: 0,
      isAllChecked: false,
      //
      typeList: ["已满足条件", "已获得资格"],
      //已满足条件支付服务费列表 ,
      paylist: [],
      //领取手机支付服务费列表
      receivelist: [],
      //是否为空
      empty: false,
      payStatus: false,
      dataLoad: false,
      activityid:getUrlParams('activityid')
    };
  },
  mounted() {

    this.GetOctoberActivityList();

    this.tabsIndex = this.$route.query.tabsIndex;
  },
  computed: {
    totail() {
      let count = 0;
      let amount = 0;
      for (let item of this.tabsIndex == 0 ? this.paylist : this.receivelist) {
        if (item.isChecked) {
          amount = item.amount;
          count++;
        }
      }
      return count * amount + " DOS";
    }
  },
  components: {
    noData
  },
  methods: {

    getPayStatus(item) {
      switch (item.mystatus) {
        case 1:
          return "已支付";
        case 3:
          return "已发货";
        case 4:
          return "已退款";
      }
    },

    getUserType(item) {
      switch (item.typeid) {
        case 1:
          return "代开充值商";
        case 2:
          return "代理人进货";
        case 3:
          return "零售充值码(SVC)并充值SV";
      }
    },
    //UE支付
    async OctoberActivityDosUEPrepare() {
      //缴费时间
      let start = new Date();
      let end = new Date("2019-11-6");
      if (start > end) {
        this.$toast("活动已经结束！");
        return;
      }
      let bool = false;
      let idArr = [];
      for (let item of this.tabsIndex ? this.receivelist : this.paylist) {
        if (item.isChecked && item.mystatus == 0) {
          idArr.push(item.id);
          bool = true;
        }
      }
      // return ;
      if (!bool) {
        this.$toast("请选择支付用户");
        return;
      }

      let price = parseInt(this.totail); //price (number, optional): 支付服务费
      let dataid = idArr.join("_"); //dataid (integer, optional): 选择支付的数据Id ,
      let paytype = this.tabsIndex + 1; //paytype (integer, optional): 1已满足条件支付服务费,2领取手机支付服务费 ,
      let data = {
        price,
        dataid,
        paytype,
        ...this.$global.userInfo,
        activityid:this.activityid
      };
      let res = await OctoberActivityDosUEPrepare(data);
      if (res.result <= 0) {
        this.$toast(res.message);
        return;
      }

      try {
        window.dosPayResult = res => {
          if (res.result <= 0) {
            return;
          }
          this.$toast("支付成功");
          setTimeout(() => {
            this.payStatus = true;

            this.GetOctoberActivityList();
          }, 500);
        };

        this.$global.setAppNative(1003, res.data.charge, res.data.sign);

      } catch (e) {
        this.Toast("调起码库支付失败");
      }
    },
    //获取十月送手机活动的领取手机和支付服务费的列表
    async GetOctoberActivityList() {
      let res = await GetOctoberActivityList(this.$global.userInfo,this.activityid);
      if (res.result < 0) {
        this.$toast("网络异常");
        return ;
      }
      let { paylist, receivelist } = res.data;

      let condition = false ; 
      for (let item of paylist) {
        if (item.mystatus) {
          continue;
        }else{
          condition = true ; 
        }
        item.isChecked = false;
      }
      this.condition = condition;

      for (let item of receivelist) {
        if (item.mystatus >= 1) {
          this.payStatus = true;
          break;
        }
        item.isChecked = false;
      }
      
      for (let item of paylist) {
        item.nodename = NameFilter(item.nodename);
      }

      for (let item of receivelist) {
        item.nodename = NameFilter(item.nodename);
      }
      paylist.sort((a, b) => b.mystatus - a.mystatus);
      receivelist.sort((a, b) => b.mystatus - a.mystatus);
      this.empty = true;
      this.dataLoad = true;
      this.paylist = paylist;
      this.receivelist = receivelist;
      this.$nextTick(() => {
        document.getElementsByClassName("van-tabs__content")[0].style.height =
          document.body.clientHeight -
          document.getElementsByClassName("van-tabs__wrap")[0].offsetHeight +
          "px";
      });
    },
    //单选切换 支持多选
    checked(item, index) {
      item.isChecked ? (item.isChecked = false) : (item.isChecked = true);

      //支持多选
      if (this.tabsIndex == 0) {
        let bool = true;
        for (let item of this.paylist) {
          if (item.mystatus) {
            continue;
          }
          if (!item.isChecked) {
            bool = false;
            break;
          }
        }
        this.isAllChecked = bool;
      } else {
        //单选
        for (let item of this.receivelist) {
          item.isChecked = false;
        }
        item.isChecked ? (item.isChecked = false) : (item.isChecked = true);
      }
    },
    //全选
    AllChecked() {
      this.isAllChecked = !this.isAllChecked;
      for (let item of this.paylist) {
        if (item.mystatus) {
          continue;
        }
        item.isChecked = this.isAllChecked ? true : false;
      }
    },
    tabsChange(index) {
      for (let item of this.paylist) {
        item.isChecked = false;
      }
      for (let item of this.receivelist) {
        item.isChecked = false;
      }
    }
  }
};
</script>
<style lang="scss" scoped>
.payStatus {
  background-color: #eeeef1 !important;
}
/deep/ .text {
  padding: 0 !important;
}
.logistics {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: column;
  align-items: flex-end;
  height: 1.4rem;
  flex: 1;
  & > div:first-child {
    font-family: PingFangSC-Regular;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.28rem;
    letter-spacing: 0rem;
    color: #2ea2fa;
  }
  & > div:last-child {
    height: 0.54rem;

    button {
      width: 100%;
      height: 100%;
      background-color: #2ea2fa;
      border-radius: 0.06rem;
      border: 0;
      font-family: PingFangSC-Regular;
      font-size: 0.26rem;
      font-weight: normal;
      font-stretch: normal;
      line-height: 0.3rem;
      letter-spacing: 0rem;
      color: #ffffff;
    }
  }
}
.status2 {
  width: 100%;
  justify-content: space-between !important;
}
.right {
  height: 100%;
  flex: 1;
  display: flex;
  justify-content: flex-end;
}
.footer {
  width: 100%;
  box-sizing: border-box;
  height: 1rem;
  position: fixed;
  bottom: 0;
  display: flex;
  justify-content: space-between;
  background: #fff;
  align-items: center;
  .totails {
    display: flex;
    align-items: center;
  }
  .allChecked {
    padding-left: 0.4rem;
    font-family: PingFangSC-Regular;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.32rem;
    letter-spacing: 0rem;
    color: #999999;
    display: flex;
    align-items: center;
    img {
      margin-right: 0.1rem;
      width: 0.36rem;
      height: 0.36rem;
      border-radius: 0.03rem;
    }
  }
  .count {
    padding-left: 0.4rem;
    padding-right: 0.3rem;
    font-family: PingFangSC-Regular;
    font-size: 0.28rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.32rem;
    letter-spacing: 0rem;
    color: #333333;
    span {
      vertical-align: middle;
      font-family: PingFangSC-Medium;
      font-size: 0.32rem;
      font-weight: normal;
      font-stretch: normal;
      line-height: 0.36rem;
      letter-spacing: 0rem;
      color: #398cff;
    }
  }
  .payBtn {
    width: 2.4rem;
    height: 100%;
    background-color: #2ea2fa;
    font-family: PingFangSC-Medium;
    font-size: 0.32rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.36rem;
    letter-spacing: 0rem;
    color: #ffffff;
    line-height: 1rem;
    text-align: center;
  }
}
.infos {
  margin-top: 0.4rem;
  box-sizing: border-box;
  padding: 0.25rem 0.3rem;
  background-color: #ffffff;
  box-shadow: 0rem 0.02rem 0.3rem 0rem rgba(141, 171, 196, 0.15);
  border-radius: 0.12rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-family: PingFangSC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.28rem;
  letter-spacing: 0rem;
  color: #999999;
  span {
    margin-left: 0.2rem;
    box-sizing: border-box;
    font-size: 0.24rem;
    font-weight: normal;
    color: #999999;
    border-radius: 0.04rem;
    border: solid 0.02rem #999999;
    padding: 0 0.1rem;
  }
  .pay {
    color: #2ea2fa;
    border: solid 0.02rem #2ea2fa;
  }
  .rightImg {
    width: 0.48rem !important;
    height: 0.48rem !important;
    img {
      width: 100%;
      height: 100%;
    }
  }
  .title {
    display: flex;
    align-items: center;
    font-weight: bold;
    color: #333333;
    font-size: 0.32rem;
    line-height: 0.36rem;
    font-family: PingFangSC-Medium;
    & > div {
      max-width: 3rem;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
  }
  .name {
    font-size: 0.28rem;
    line-height: 0.32rem;
    padding-top: 0.3rem;
    padding-bottom: 0.1rem;
  }
}
/deep/ .van-tabs {
  .van-tabs__content {
    background: #f7f7fc;
    padding: 0 0.3rem;
    overflow: scroll;
    -webkit-overflow-scrolling: touch;
  }
  .van-tabs__content::-webkit-scrollbar {
    display: none;
  }
  .van-tabs__wrap {
    height: 0.8rem;
    .van-tab {
      font-family: PingFangSC-Regular;
      font-size: 0.28rem;
      font-weight: normal;
      font-stretch: normal;
      line-height: 0.32rem;
      letter-spacing: 0rem;
      color: #999999;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    .van-tab--active {
      color: #2ea2fa;
    }
    .van-tabs__line {
      height: 0.05rem;
      background-color: #2ea2fa;
      border-radius: 0.03rem;
    }
  }
  .van-hairline--top-bottom::after,
  .van-hairline-unset--top-bottom::after {
    border: 0;
  }
}
</style>