<template>
    <div class="changeInfo">
        <div class="change" v-if="!nodata">
            <div class="lft" >
                <div class="info">{{$t('lang.ChangeInfo_CzsName')}}:{{ userinfo.name }}</div>
                <div class="info">{{$t('lang.ChangeInfo_CzsUserName')}}:{{ nodename }}</div>
                <div class="info">{{$t('lang.ChangeInfo_CzsUserAccount')}}:{{ mobileno }}</div>
            </div>
            <div class="rgt" v-if="showchange">
                <button @click="inpflag = true">{{$t('lang.ChangeInfo_RepCzs')}}</button>
            </div>
        </div>
        <div class="lft emptyData" v-else>
            {{$t('lang.ChangeInfo_empty')}}
        </div>
        <div class="bot" v-show="showbtn">
            <div class="text"><span>*</span><div>{{$t('lang.ChangeInfo_hint')}}</div></div>
            <div class="btn">
                <button @click="setChange($route.query.key, $route.query.infoid)">{{$t('lang.ChangeInfo_revision')}}</button>
            </div>
        </div>
        <BotPopup v-show="inpflag" :title="this.$t('lang.ChangeInfo_hint')" @popupSubmit="setPopupSubmit" @popupFlag="setPopup"/>
    </div>
</template>

<script>
const BotPopup = () => import("@/components/BotPopup");
import merge from 'webpack-merge';
import { GetMyUserCzs, GetFbapInfo, ChangeFbap } from "@/api/getFbApData";
import { NameFilter, phoneFilter } from '@/config/utils' ;
export default {
    data() {
        return {
            inpflag: false,     // 输入框显示
            showbtn: false,     // 更换按钮部分显示
            userinfo: {},       // 用户信息
            nodename: '',       // 充值商用户名称
            mobileno: '',       // 充值商手机号码
            nodata: false,
            showchange: false,
        }
    },
    created() {
        this.GetMyUserCzs(this.$route.query.key);
    },
    watch: {
        showbtn(show) {
            if (show) {
                // document.title = "更改充值商信息"
                document.title = this.$t('lang.ChangeInfo_title');
            }
        },
    },
    methods: {
        setPopupSubmit(num) {
            this.GetFbapInfo(num);
        },
        setPopup(flag) {
            this.inpflag = flag;
        },
        //获取我的充值商
        async GetMyUserCzs() {
            let result = await GetMyUserCzs(
                JSON.parse(sessionStorage.userParam),
                JSON.parse(this.$route.query.infoid)
            );
            if (result.result > 0) {
                if (result.data.mobileno || result.data.nodename || result.data.parentname) {
                    this.userinfo = result.data;
                    if (!result.data.hschanged) this.showchange = true;
                    this.userinfo.name = result.data.parentname;
                    this.nodename = NameFilter(result.data.nodename) ;
                    this.mobileno = phoneFilter(result.data.mobileno) ; 
                } else {
                    this.nodata = true;
                }
            } else {
                this.Toast.fail(result.message)
            }
        },
        async GetFbapInfo(key) {
            let result = await GetFbapInfo({...JSON.parse(sessionStorage.userParam), key});
            if (result.result > 0) {
                this.userinfo = result.data;
                this.nodename = NameFilter(this.userinfo.nodename) ;  
                this.mobileno = phoneFilter(this.userinfo.mobileno) ;
                this.inpflag = false;   // 关闭输入框
                this.showbtn = true;    // 显示按钮
                if (this.$route.query.key != key) {
                    this.$router.push({ 
                        // 改变url中的参数
                        query:merge(this.$route.query,{'key': key})
                    })
                }
                
            } else {
                this.Toast.fail(result.message);
            }
        },
        async setChange(key, infoid) {
            let result = await ChangeFbap({...JSON.parse(sessionStorage.userParam), key, infoid});
            if (result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    // this.$router.push('/fbap');
                    this.$router.go(-2);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
    },
    components: {
        BotPopup
    }
}
</script>

<style lang="scss" scoped>
.emptyData{
    padding-top: 3rem;
    text-align: center;
}
.changeInfo {
    min-height: 100%;
    background: #f7f7fc;
    font-size: 0.3rem;
    .change {
        background: #fff;
        display: flex;
        padding: 0.35rem 0.3rem;
        .lft {
            flex: 1;
            .info {
                margin-bottom: 0.15rem;
                &:last-of-type {
                    margin-bottom: 0;
                }
            }
        }
        .rgt {
            display: flex;
            align-items: center;
            button {
                border: 1px solid #2ea2fa;
                color: #2ea2fa;
                background: none;
                padding: 0.1rem 0.15rem;
                border-radius: 0.04rem;
            }
        }
    }
    .text {
        display: flex;
        padding: 0.3rem;
        span {
            color: #ff3030;
            font-size: 0.48rem;
        }
    }
    .btn {
        padding: 0 0.3rem;
        margin-top: 0.6rem;
        button {
            background: #2ea2fa;
            width: 100%;
            height: 0.88rem;
            line-height: 0.88rem;
            border: none;
            color: #fff;
            border-radius: 0.04rem;
        }
    }
}
</style>