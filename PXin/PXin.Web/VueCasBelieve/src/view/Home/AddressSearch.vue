<!-- 搜索页面 -->
<template>
  <div class="AddressSearch">
    <!-- 头部导航 -->
    <!-- 搜索输入框 -->
    <div class="searchIpt">
      <div :style="{backgroundImage:'url('+require('@/assets/images/addfriend_search@2x.png')+')'}">
        <input v-model="val" ref="ipt" :placeholder="$t('m.search')" type="text" />
      </div>
      <!-- 取消 -->
      <div @click="$router.go(-1)">{{$t('m.cancel')}}</div>
    </div>
    <!-- 搜寻结果提示 -->
    <div></div>
    <!-- 搜寻结果提示 -->
    <div class="hint" v-show="val.length" @click="search">
      <div>
        <div>
          <div>
            <div>
              <img src="@/assets/images/search_people@2x.png" alt />
            </div>
          </div>
          <div>
            <span>{{$t('m.searchphone')}}</span>
            <span>{{val}}</span>
          </div>
        </div>
      </div>
      <!--  -->
    </div>
  </div>
</template>

<script>
import { QueryFriend } from "@/api/getChatData";
export default {
  data() {
    return {
      val: ""
    };
  },
  mounted() {
    // 加载input获取焦点
    this.$refs.ipt.focus();
  },
  methods:{
    search(){
      let data={
        ...JSON.parse(sessionStorage.userParam),
        key:this.val,
        pageIndex:1,
        pageSize:1
      };
      QueryFriend(data,res=>{
        if (res.result > 0) {
          let data=res.data.item[0];
          this.$router.push({path:'/Information',query:{userId:data.nodeid}});
        } else {
          this.$toast(res.message);
        }
      })
    }
  }
};
</script>

<style scoped lang='scss'>
.AddressSearch {
  height: 100%;
  background-color: #f2f2f2;

  /deep/ .left {
    font-size: 0.48rem !important;
  }

  .hint {
    margin-top: 0.2rem;

    & > div {
      height: 1.68rem;
      background: #fff;
      padding-left: 0.3rem;

      & > div {
        display: flex;
        align-items: center;
        align-items: center;
        /* border-bottom: 0.02rem solid #d1d1d1; */
        height: 100%;
        div:nth-child(2) {
          padding-left: 0.33rem;
          display: flex;
          align-items: center;
        }

        div:nth-child(1) {
          width: 1rem;
          height: 1rem;

          img {
            width: 100%;
            height: 100%;
          }
        }

        span {
          font-family: PingFang-SC-Medium;
          font-size: 0.3rem;
          font-weight: normal;
          font-stretch: normal;
          letter-spacing: 0rem;
          color: #333333;
        }

        span:nth-child(2) {
          color: #2ea2fa;
        }
      }
    }
  }

  .searchIpt {
    box-sizing: border-box;
    width: 100%;
    height: 1.2rem;
    padding: 0 0.3rem;
    background-color: #2ea2fa;
    display: flex;
    align-items: center;

    div:nth-child(1) {
      box-sizing: border-box;
      padding-left: 0.96rem;
      width: 5.8rem;
      height: 0.68rem;
      background-color: #ffffff;
      border-radius: 0.1rem;
      display: flex;
      align-items: center;
      background-size: 0.38rem 0.42rem;
      background-repeat: no-repeat;
      background-position: 0.3rem center;

      input {
        border-radius: 0.1rem;
        padding: 0;
        outline: none;
        width: 100%;
        height: 100%;
        border: 0;
        font-family: PingFang-SC-Medium;
        font-size: 0.3rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
      }
    }

    div:nth-child(2) {
      display: flex;
      flex: 1;
      justify-content: flex-end;
      align-items: center;
      font-family: PingFang-SC-Medium;
      font-size: 0.34rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #ffffff;
    }
  }
}
</style>
