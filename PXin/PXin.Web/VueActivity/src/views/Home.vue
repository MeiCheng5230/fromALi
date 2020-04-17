<template>
    <div class="home">
        <van-tabs :sticky="true">
            <van-tab title="活动中">
                <div class="item"
                    v-for="item in goingList"
                     :key="item.id"
                     @click="jumpDetailPage(item.id)"
                     v-show="goingList.length>0"
                >
                    <div class="picture"><img :src="item.cover" alt="" height="140"></div>
                    <div class="body">
                        <div class="title">{{item.activityname}}</div>
                        <div class="time">活动时间：{{item.activitystarttime.substr(0,10)}} 至 {{item.activityendtime.substr(0,10 )}}</div>
                        <div class="time">缴费时间：{{item.paystarttime.substr(0,10) }} 至 {{item.payendtime.substr(0,10)}}</div>
                    </div>
                </div>
                <no-data v-if="goingList.length<1&&isAjaxFinished" text="咦，活动还未开始哟~"></no-data>
            </van-tab>
            <van-tab title="已结束">
                <div class="item"
                     v-for="item in overList"
                     :key="item.id"
                     @click="jumpDetailPage(item.id)"
                     v-show="overList.length>0"
                >
                    <div class="picture"><img :src="item.cover" alt="" height="140"></div>
                    <div class="body">
                        <div class="title">{{item.activityname}}</div>
                        <div class="time">活动时间：{{item.activitystarttime.substr(0,10)}} 至 {{item.activityendtime.substr(0,10 )}}</div>
                        <div class="time">缴费时间：{{item.paystarttime.substr(0,10) }} 至 {{item.payendtime.substr(0,10)}}</div>
                    </div>
                </div>
                <no-data v-if="overList.length<1&&isAjaxFinished" text="咦，还没有已结束的活动哟~"></no-data>
            </van-tab>
        </van-tabs>
    </div>
</template>

<script>
    import {GetActivitys} from '@/api/api'
    import NoData from "../components/NoData";
    export default {
        name: "Home",
        data() {
            return {
                isAjaxFinished:false,
                //活动列表
                list:[],
                overList:[],
                goingList:[],
            }
        },
        components: {
            NoData
        },
        created() {
            this.getListData();
        },
        methods: {
            //获取活动列表
            async getListData() {
                let res = await GetActivitys(this.GLOBAL.USERINFO);
                if(res.result>0){
                    this.list=res.data;
                    this.listArr();
                    this.isAjaxFinished=true;

                }
            },
            //过滤列表1：活动中，0：已结束
            listArr(){
                this.goingList=this.list.filter((item)=>item.status===1);
                this.overList=this.list.filter((item)=>item.status===0);
            },
            //跳转到对应页面
            jumpDetailPage(id){
                if(id==2){
                    this.$router.push({path:'/nov/2'});
                    return;
                }
                location.href='/App/Believe/index.html?activityid='+id+'&nodeid='+this.GLOBAL.USERINFO.nodeid+'&sid='+this.GLOBAL.USERINFO.sid+'&tm='+this.GLOBAL.USERINFO.tm+'&sign='+this.GLOBAL.USERINFO.sign+'#/OctActivity';
            }
        }
    }
</script>

<style scoped lang="scss">
    .home {
        height: 100%;
        background-color: #f2f2f2;

        /deep/ .van-tabs__content {
            padding: 12px 15px;
        }
        /deep/.van-tabs__nav{
            .van-tab--active{
                color: #2ea2fa;
            }
            .van-tabs__line{
                width: 21px;
                background-color: #2ea2fa;
            }

        }

        .item {
            background-color: #ffffff;
            border-radius: 5px;
            margin-bottom: 12px;

            .picture {

                width: 100%;
                height: 140px;

                img {
                    width: 100%;
                    height: 100%;
                }
            }

            .body {
                padding: 12px 15px;
                text-align: left;

                .title {
                    font-size: 14px;
                    padding-bottom: 10px;
                }

                .time {
                    color: #999999;
                    font-size: 12px;
                }
            }
        }
    }

</style>
