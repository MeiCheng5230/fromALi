<template>
    <div class="record" >
        <van-tabs @click="getTabLists" id="myCode" style="">
          <van-tab v-for="(item1,index) in showlist" :title="item1.name" v-if="item1.isshow" :key="index">
            <div class="datalist">
              <div class="item" v-for="item in item1.list" :key="item.index">
                <div class="top">
                  <div :class="'point point'+ item.type"></div>
                  <div class="num">{{item.num}}</div>
                  <div class="time">{{item.time}}</div>
                </div>
                <div class="text" v-if="item.type==1">方式：{{item.pattern}}</div>
                <div class="text" v-else>账号：{{item.nodecode}}</div>
                <div class="text">编号：{{item.noid}}</div>
              </div>
            </div>
          </van-tab>
          <noData v-if="nodata"></noData>
          <van-list v-model="loading"
                    :finished="finished"
                    :finished-text="finishedStr"
                    @load="onLoad" >

          </van-list>
        </van-tabs>
    </div>
</template>

<script>
  const noData = () => import("@/components/noData");
  import { GetMySvchis } from '@/api/getData'
  export default {
      data () {
          return {
            showlist: [
              {
                name: "全部",
                isshow:true,
                list: [],
                num:1,
              },
              {
                name: "获取",
                isshow: true,
                list: [],
                num: 1,
              },
              {
                name: "充值",
                isshow: true,
                list: [],
                num: 1,
              },
              {
                name: "转让",
                isshow: true,
                list: [],
                num: 1,
              },
              {
                name: "零售",
                isshow: false,
                list: [],
                num: 1,
              },
            ],
            finished: false,
            finishedStr: '没有更多了',
            activenum: 0,
            loading: false,
            typeid: -1,
            last: 0,
            nodata:false,
            flag: true
          }
      },
      mounted() {
        this.setdata();
        if (!document.querySelector(".Header")) {
          document.querySelector(".van-tabs__wrap").style.top = "0";
          //document.querySelector(".van-tabs__content").style.paddingTop = "0";
          document.querySelector(".van-tabs--line").style.paddingTop = "0";
        }
      },
      methods: {
        onChange() {

        },
        async GetMySvchis(pagenum, pagesize, id, i) {
          let result = await GetMySvchis(JSON.parse(sessionStorage.userParam), pagenum, pagesize, id);
          if (result.result > 0) {
            if (pagenum == 1) {
              this.showlist[this.activenum].list = [];
              this.showlist[this.activenum].num = 1;
            }
            if (result.data.length > 0) {
              this.showlist[this.activenum].num++;
            }

            if (pagenum === 1 && result.data.length < 1) {
              this.finishedStr = '';
              this.nodata = true;
              this.loading = false;
              this.finished = true;
              return;
            }
            let _this = this;
            result.data.forEach(function (item, index) {
              let data = {
                num: " "+item.amount,//+item.amounttype,
                time: item.createtime,
                pattern: item.note,
                noid: "NO." + item.cardno,
                nodecode:item.nodecode,
              };
              let i = 1;
              if (item.typeid == 0 || item.typeid == 2 || item.typeid == 3 || item.typeid == 4|| item.typeid == 7|| item.typeid == 8|| item.typeid == 9|| item.typeid == 10|| item.typeid == 11) {
                i = 1;
              } else if (item.typeid == 5) {
                i = 2;
              } else if (item.typeid == 6) {
                i = 3;
              } else if (item.typeid == 1) {
                i = 4;
              }

              data.type = i;
              data.num = _this.showlist[i].name + data.num;
              _this.showlist[_this.activenum].list.push(data);
            })
            this.finished = false;
            if (result.data.length < pagesize) {
              this.finishedStr = '没有更多了';
              this.finished = true;
            }

            this.loading = false;
          } else {
            this.Toast(result.message);
          }
          },
        setdata: function () {
          this.showlist[4].isshow = this.$store.issale;
          //this.GetMySvchis(1,6,-1,0);

        },
        getTabLists(index, name) {
          this.flag = false;
          let index1 = 0;
          this.showlist.forEach((c, index) => { if (c.name == name) index1 = index; });
          this.activenum = index1;
          let i = -1;
          switch (index1) {
            case 0:
              i = -1;
              break;
            case 1:
              i = 0;
              break;
            case 2:
              i = 5;
              break;
            case 3:
              i = 6;
              break;
            case 4:
              i = 1;
              break;
            default:
          }
          this.typeid = i;
          this.finished = true;
          this.nodata = false;
          this.finishedStr = '';
          this.loading = true;
          this.toTop();
          this.GetMySvchis(1, 10, i, index1);
          this.flag = true;
        },
        toTop() {
          let initialNode = document.getElementById("myCode");
          initialNode.scrollTop = 0;
        },
        onLoad: function () {
          if (!this.timer && this.flag) {
            this.timer = setTimeout(() => {
              this.GetMySvchis(this.showlist[this.activenum].num, 10, this.typeid, this.activenum);
              this.timer = null;
            }, 500)
          }
        },
      },
      components: {
        noData
      }
  }
</script>

<style lang="scss" scoped>
.record {
    min-height: 100%;
    background: #f7f7fc;
    .datalist {
        padding: 0.4rem;
        .item {
            padding: 0.3rem;
            background-color: #ffffff;
            box-shadow: 0 2px 5px 4px
                rgba(122, 177, 224, 0.05);
            border-radius: 0.12rem;
            margin-bottom: 0.4rem;
            .top {
                display: flex;
                align-items: center;
                padding-bottom: 0.1rem;
                .point {
                    width: 0.14rem;
                    height: 0.14rem;
                    border-radius: 50%;
                    &.point1 {
                        background: #2ea2fa;
                    }
                    &.point2 {
                        background: #ffc424;
                    }
                    &.point3 {
                        background: #cbcbcb;
                    }
                    &.point4 {
                        background: #2df338;
                    }

                }
                .num {
                    font-size: 0.28rem;
                    font-weight: bold;
                    flex: auto;
                    padding-left: 0.1rem;
                }
                .time {
                    color: #999;
                    font-size: 0.24rem;
                }
            }
            .text {
                font-size: 0.24rem;
                color: #999;
                padding-top: 0.1rem;
            }
            &:last-of-type {
                margin: 0;
            }
        }
    }
    /deep/ .van-tabs {
        .van-tabs__wrap {
            position: fixed;
            top: 1rem;
        }
        .van-tabs__line {
            background-color: #2ea2fa;
        }
        .van-tabs__content {
          padding-top: 0.8rem;
        }
        .van-tabs__wrap{
          width:100%;
        }
        .van-tabs__wrap--scrollable .van-tab {
          flex: auto;
        }
    }


}
</style>
