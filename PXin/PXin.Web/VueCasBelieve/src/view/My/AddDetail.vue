<!-- 充值明细 -->
<template>
  <div class="adddetail">
    <table></table>
    <!-- 列表 -->
    <div v-if="RechargeHisList&&RechargeHisList.length>0" class="detailList">
      <div class="detailInfo" v-for="(item,index) of RechargeHisList" :key="index">
        <div class="detailImg">
          <img :src="GetPurseHisTypeIcon(item.typeid)" alt />
          <span>{{item.remark}}</span>
        </div>
        <div class="detailTime">
          <p>+{{item.price}}V</p>
          <p>{{item.createtime}}</p>
        </div>
      </div>
    </div>
    <!-- 暂时只支持查看30天的记录哟 -->
    <div class="hint">暂时只支持查看30天的记录哟</div>
  </div>
</template>

<script>
import { GetPxinAmountChangeHis, GetPurseHisTypeLogo } from "@/api/myData.js";
export default {
  data() {
    return {
      pageindex: 1,
      pagesize: 30,
      RechargeHisList: [],
      purseHisTypeLogo: []
    };
  },
  methods: {
    GetPurseHisTypeIcon: function(typeid) {
      for (let index = 0; index < this.purseHisTypeLogo.length; index++) {
        if (this.purseHisTypeLogo[index].typeid == typeid) {
          return this.purseHisTypeLogo[index].iconurl;
        }
      }
    }
  },
  created() {
    GetPxinAmountChangeHis(
      {
        pageindex: this.pageindex,
        pagesize: this.pagesize,
        typeid: 1
      },
      data => {
        if (data.data < 1) {
          this.$toast("数据加载失败");
          setTimeout(() => {
            this.$router.go(-1);
          }, 500);
          return;
        }
        this.RechargeHisList = data.data;
      }
    );
    GetPurseHisTypeLogo(null, data => {
      if (data.data < 1) {
        this.$toast("数据加载失败");
        setTimeout(() => {
          this.$router.go(-1);
        }, 500);
        return;
      }
      this.purseHisTypeLogo = data.data;
    });
  }
};
</script>

<style scoped lang='scss'>
.hint {
  width: 100%;
  height: 2.6rem;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #666666;
}

.detailright {
  font-family: PingFang-SC-Medium;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
}

.detailLeft {
  display: flex;
  align-items: center;

  span:first-child {
    display: inline-block;
    width: 0.1rem;
    height: 0.3rem;
    background-color: #2ea2fa;
    border-radius: 0.05rem;
    margin-right: 0.2rem;
  }

  font-family: PingFang-SC-Bold;
  font-size: 0.32rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #333333;
}

.adddetail {
  height: 100%;
  background-color: #f0f0f0;
  padding: 0 0.3rem;
  overflow-y: scroll;
}

.detailList {
  padding-bottom: 1rem;
}
.detailInfo {
  border-bottom: 1px solid #ddd;
  height: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #1a1a1a;
}
.detailImg {
  display: flex;
  align-items: center;
  img {
    width: 0.5rem;
    height: 0.5rem;
    margin-right: 0.25rem;
  }
}
.detailTime {
  justify-content: center;
  display: flex;
  flex-direction: column;
  height: 100%;
  padding-right: 0.3rem;
  p {
    margin: 0;
    color: #000;
    text-align: right;
  }
  p:last-child {
    font-family: PingFang-SC-Regular;
    font-size: 0.2rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}
</style>
