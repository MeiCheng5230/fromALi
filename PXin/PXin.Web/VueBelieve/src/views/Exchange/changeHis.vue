<template>
    <div class="record" >
                <div class="datalist">
                     <van-list v-model="loading"
                        :finished="finished"
                        :immediate-check=false
                        :finished-text="hisListshow?'':'没有更多了'"
                        :offset=10
                        @load="onLoad">
                    <div class="item" v-for="item in hisList" :key="item.hisid">
                        <div class="top">
                            <div class="point"></div>
                            <div class="num">{{item.productname}}</div>
                            <div class="num rightnum">x{{item.num}}</div>
                        </div>
                        <div class="text">{{item.remarks}}</div>
                         <div class="top">
                              <div class="text">{{item.createtime}}</div>
                                 <div class="num rightnum dos">{{item.amount}}专户dos</div>
                         </div>
                    </div>
                     </van-list>
                     <nodata v-show="hisListshow" text="咦！您目前还没有兑换记录哟"/>
                </div>
    </div>
</template>

<script>
const nodata = () => import("@/components/noData");
import { GetRechargeHisList} from '@/api/getData';
export default {
    data () {
        return {
            hisList:[],
            loading: false,
            finished: false,
            pageNum: 1,
            pagesize:10,
            hisListshow:false,
        }
    },
    mounted (){
      this.gethis();
    },
    methods: {
        async gethis(){
             let result=await GetRechargeHisList(JSON.parse(sessionStorage.userParam),this.pageNum,this.pagesize);
             if(result.result>0){
                this.hisList= this.hisList.concat(result.data);
                this.hisListshow=this.hisList.length>0?false:true;
                if(result.data.length==0){
                    this.finished=true;
                }
             }
             else{
                 this.Toast(result.message);
             }
        },
         onLoad(){
            this.pageNum += 1;
            this.gethis();
            this.loading = false;
         }
    },
    components: {
       nodata
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
                   width: 0.05rem;
                   height: 0.25rem;
                   border-radius: 20%;
                   background: #2ea2fa;

                }
                .num {
                    font-size: 0.28rem;
                    font-weight: bold;
                    flex: auto;
                    padding-left: 0.1rem;

                }
                .rightnum {
                    text-align: right;
                    }
                 .dos{
                    color: orange;
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
    }

}
</style>
