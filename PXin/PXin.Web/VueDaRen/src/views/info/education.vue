<template>
    <div class="education">
        <div class="item" v-for="item in educationList" :key="item.index" @click="ToDet(item)">
            <div class="title">{{ item.schoolname }}</div>
            <div class="time">{{ DateFormat(item.fromtime) }} - {{ DateFormat(item.endtime) }}</div>
            <div class="job"><span>专业：</span><div>{{ item.subject }}</div></div>
        </div>
        <div class="nodata" v-if="noData">还没有添加教育背景，赶紧去添加吧！</div>
        <router-link to="/filleducation" tag="div" class="btn" v-if="status == 0 || status == 2">
            <button>添加</button>
        </router-link>
    </div>
</template>

<script>
import { GetDaRenEdus } from '@/api/getData'
export default {
    data() {
        return {
            educationList: [],
            noData: false,
            status: '',
        }
    },
    created() {
        this.GetDaRenEdus();
        var userinfo = JSON.parse(this.getStore('info'));
        this.status = userinfo.status;
    },
    watch: {
       educationList(val) {
            if(!val.length) {
                this.noData = true;
            } else {
                this.noData = false;
            }
       } 
    },
    methods: {
        async GetDaRenEdus() {
            let result = await GetDaRenEdus(this.$global.userInfo);
            console.log(result)
            if(result.result > 0) {
                this.educationList = result.data;
            } else {
                this.Toast(result.message);
            }
        },
        DateFormat(val) {
            var now = new Date();
            if (val != null) {
                if (new Date(val.replace(/-/g,"/")).getFullYear() > now.getFullYear() && new Date(val.replace(/-/g,"/")).getMonth() == now.getMonth()) {
                    return '至今'
                } else {
                    var date = new Date(val.replace(/-/g,"/"));
                    return date.getFullYear() + '.' + (date.getMonth() + 1);
                }
            }
        },
        ToDet(data) {
            this.$router.push('/filleducation');
            setTimeout(() => {
                this.bus.$emit('educ', data);
            }, 500)
        }
    },
}
</script>

<style lang="scss" scoped>
.education {
    min-height: 100%;
    background: #f7f7fc;
    font-size: 0.3rem;
    padding-bottom: 1.48rem;
    box-sizing: border-box;
    .item {
        padding: 0.3rem 0.36rem;
        border-bottom: 1px solid #f7f7fc;
        background: #fff;
        .title {
            font-weight: bold;
        }
        .time {
            padding: 0.3rem 0;
        }
        .job {
            display: flex;
            span {
                white-space: nowrap;
            }
        }
    }
    .nodata {
        text-align: center;
        color: #999;
        padding: 1rem 0;
    }
    .btn {
        padding: 0.3rem;
        position: fixed;
        width: 100%;
        bottom: 0;
        box-sizing: border-box;
        button {
            width: 100%;
            height: 0.88rem;
            background: #2ea2fa;
            border: none;
            color: #fff;
            border-radius: 0.04rem;
        }
    }
}
</style>
