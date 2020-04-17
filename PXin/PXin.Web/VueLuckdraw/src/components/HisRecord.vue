<template>
    <div class="record">
        <div class="bgBx">
            <img src="@/assets/img/list_bg.png" alt=""/>
            <div class="txt">已获得<span>{{ total }}</span>A点</div>
        </div>
        <div class="list">
            <div class="item" v-for="item in recordlist" :key="item.index">
                <div class="lft">
                    <div class="tit">抽奖活动奖励</div>
                    <div class="time">{{ item.createtime }}</div>
                </div>
                <div class="num"><span>{{ item.amount>0?'+'+item.amount:item.amount }}</span>A点</div>
            </div>
        </div>
        <div class="nodata" v-if="nodata">
            暂无数据
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import utils from "@/utils";
import { Toast, Indicator } from 'mint-ui';
export default {
    data() {
        return {
            recordlist: [],
            nodata: false,
        }
    },
    created() {
        this.getData();
    },
    computed: {
        total() {
            var tnum = 0;
            for (const itm of this.recordlist) {
                tnum += itm.amount;
            }
            return tnum;
        }
    },
    methods: {
        getData() {
            Indicator.open();
            axios.post('/api/Redpacket/GetLuckDrawHis', utils.userInfo).then((res) => {
                console.log(res.data);
                Indicator.close();
                if(res.data.result > 0) {
                    this.recordlist = res.data.data;
                    console.log(this.recordlist);
                    if(this.recordlist.length == 0) {
                        this.nodata = true;  
                    }
                } else {
                    Toast(res.data.message);
                }
            }).catch((err) => {
                Indicator.close();
                console.log(err);
            })
        }
    }
}
</script>

<style scoped>
.record {
    min-height: 100%;
    background: #fff;
}
.record .bgBx {
    position: relative;
}
.record .bgBx img {
    width: 100%;
}
.record .bgBx .txt {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    z-index: 2;
    color: #fff;
}
.record .bgBx .txt span {
    font-size: 1.4rem;
    margin-left: 0.5rem;
    font-weight: bold;
}
.list .item {
    display: flex;
    align-items: center;
    padding: 1rem 0.8rem;
    border-bottom: 1px solid #f9f9f9;
}
.list .item .lft {
    flex: auto;
}
.list .item .num {
    color: #999;
    font-size: 0.8rem;
}
.list .item .num span {
    color: #e77474;
    font-weight: bold;
}
.list .item .lft .time {
    color: #999;
    font-size: 0.7rem;
    margin-top: 0.4rem;
}
.list .item .lft .tit {
    font-size: 0.8rem;
    font-weight: bold;
}
.nodata {
    padding: 2rem 0;
    text-align: center;
    color: #999;
}
</style>