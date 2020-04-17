<template>
  <div>
    <mt-index-list>
      <mt-index-section index="常用">
        <div @click="toReg(item)" v-for="(item,index) of commonuse" :key="index">
          <span>{{item.country}}</span>
          <span>+{{item.code}}</span>
        </div>
      </mt-index-section>
      <mt-index-section v-for="(item,index) of List" :key="index" :index="item.index">
        <div @click="toReg(item)" v-for="(item,index) of item.arr" :key="index">
          <span>{{item.country}}</span>
          <span>+{{item.code}}</span>
        </div>
      </mt-index-section>
    </mt-index-list>
  </div>
</template>
<script>
import { IndexList, IndexSection } from "mint-ui";
export default {
  data() {
    return {
      //常用
      commonuse: [],
      //3次循环筛选后的数据列表
      List: []
    };
  },
  mounted() {
    this.axios
      .post("/api/Sys/GetAreaCode", this.$route.query.info)
      .then(res => {
        if (res.data.result > 0) {
          for (let item of res.data.data) {
            if (item.commonuse) {
              if (item.country == "大陆") {
                item.country = "中国";
              }
              this.commonuse.push(item);
            }
          }
          this.regionList = res.data.data;

          for (var i = 0; i < 26; i++) {
            this.List.push({
              index: String.fromCharCode(65 + i),
              arr: []
            });
          }

          for (let item of res.data.data) {
            for (let item2 of this.List) {
              if (item.encountry == item2.index) {
                item2.arr.push(item);
              }
            }
          }
          let spliceIndex = [];

          //splice(i,1)  index:V  删不尽
          for (let i = 0; i < this.List.length; i++) {
            if (this.List[i].arr.length != 0) {
              spliceIndex.push({
                index: this.List[i].index,
                arr: this.List[i].arr
              });
            }
            if (this.List[i].index == "Z") {
              this.List[i].arr.splice(0, 0, {
                country: "中国",
                code: "86"
              });
            }
          }
          this.List = spliceIndex;
        } else {
        }
      });
  },
  methods: {
    toReg(item) {
      this.$router.push({
        path: "/register",
        query: { country: item.country, code: item.code }
      });
    }
  }
};
</script>

<style lang="scss" scoped>
/deep/ .mint-indexlist-navitem {
  color: #2ea2fa;
  line-height: 0.4rem;
}
/deep/ .mint-indexlist-content {
  margin: 0 !important;
  -webkit-overflow-scrolling: touch;
}
/deep/ .mint-indexlist {
  & > ul div {
    font-family: PingFang-SC-Medium;
    font-size: 0.36rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0.01rem;
    color: #333333;
    // padding: 0 .3rem;
    margin-left: 0.3rem;
    padding-right: 1rem;
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: 0.8rem;
    border-bottom: 0.02rem solid #eeeeee;

    span:last-child {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0.01rem;
      color: #999999;
    }
  }
}
ul div:last-child {
  border: 0;
}
/deep/ .mint-indexlist-nav {
  background: transparent;
}
/deep/ .mint-indexsection-index {
  padding: 0;
  height: 0.6rem;
  display: flex;
  align-items: center;
  font-family: PingFang-SC-Medium;
  font-size: 0.28rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  padding: 0 0.3rem;
}
/deep/ .mint-indexlist-nav {
  border: 0;
}
</style>