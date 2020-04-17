<template>
    <div class="home">
        <div class="banner">
            <img src="../../assets/images/banner.png"/>
        </div>
        <div class="content">
            <div class="item">
                <div class="title"><img src="../../assets/images/image_title1.png" alt="" height="25"></div>
                <div class="body">
                    <h5>合伙人</h5>
                    <p>
                        1.1 活动期间新注册的合伙人向代理人、充值商或相信公司兑换1万SVC充值码且兑换成SV；<br><br>
                        1.2 缴费期间合伙人和充值商/代理人各支付200DOS服务费即获得迪拜游名额；
                    </p>
                    <h5>代理人</h5>
                    <p>
                        1.1 活动期间代理人向上级充值商进货SVC零售码库存3次或以上；<br><br>
                        1.2 缴费期间代理人和充值商各支付200DOS 服务费即获得迪拜游名额；
                    </p>
                    <h5>充值商</h5>
                    <p>
                        1.1 活动期间充值商进货1次或以上；<br><br>
                        1.2 缴费期间充值商支付200DOS即获得迪拜游名额；
                    </p>
                </div>
            </div>
            <div class="item">
                <div class="title"><img src="../../assets/images/image_title2.png" alt="" height="25"></div>
                <div class="body">
                    <p style="padding-top: 20px;">
                        1. 活动时间：11月1日 - 11月30日<br><br>
                        2. 缴费时间：11月1日 - 12月10日<br><br>
                        3. 本次活动每个账号只能获得一个迪拜游名额；<br><br>
                        4. 参与迪拜游的账号必须绑定已实名认证的PCN账号；<br><br>
                        5.
                        获得迪拜游名额的用户最多可以邀请两位嘉宾参加迪拜游，每邀请一位嘉宾需支付1000DOS作为参与活动的押金，如在见证之旅结束内有完成兑换10000SVC且兑换SV或以上任务，则全额退回DOS，如没有完成任务则押金被视为直接向相信公司兑换一张一万面值的SVC充值码。<br><br>
                        6. 缴费期间未支付服务费的用户，将视为自动放弃，如其中一方已支付服务费，将在缴费时间结束后退回原支付账号；<br><br>
                        7. 本活动最终解释权归相信所有
                    </p>

                </div>
            </div>
        </div>
        <div class="footer">
            <!--            <div class="fail">开始缴费时间：10月21日</div>-->
            <div class="pass">
                <div class="item">
                    <div class="info">已满足条件：{{satisfactionCount}}
                        <van-icon name="question-o" @click="setDialog(0,)"/>
                    </div>
                    <div class="button serve" @click="getPcnCode(0)">支付服务费</div>
                </div>
                <div class="item">
                    <div class="info">已获得资格：{{qualifyCount}}
                        <van-icon name="question-o" @click="setDialog(1)"/>
                    </div>
                    <div class="button mobile" @click="getPcnCode(1)">迪拜旅游名额</div>
                </div>
                <router-link to="/home/1/2"></router-link>
            </div>
        </div>
    </div>
</template>

<script>
    import {GetNovemberActivityCount, HasBindActivityThirdparty, BindActivityThirdparty} from '@/api/api'

    export default {
        name: "Index",
        data() {
            return {
                // 已满足条件数量
                satisfactionCount: 0,
                //已获得资格数量
                qualifyCount: 0,
                //是否绑定Pcn账号
                isBindPNC: false,

            }
        },
        created() {
            this.getCountData();
        },
        methods: {
            //获取是否绑定PCN
            async getBindPcnData(id) {
                let res = await HasBindActivityThirdparty(this.GLOBAL.USERINFO,this.$route.params.id);
                if (res.result > 0) {
                    this.isBindPNC = true;
                    sessionStorage.setItem('OctActivityPcnUser', '');
                    this.jumpListPage(id);
                    return;
                }
                vant.Toast.clear();
                setTimeout(() => {
                    this.jumpPCNPage();
                }, 100)
            },
            //获取数据
            async getCountData() {
                let res = await GetNovemberActivityCount(this.GLOBAL.USERINFO);
                if (res.result > 0) {
                    let {satisfycondicount, qualifycount} = res.data;
                    this.satisfactionCount = satisfycondicount;
                    this.qualifyCount = qualifycount;
                }
            },
            //每月活动绑定pcn帐号
            async postPcnData(code,id) {
                let res = await BindActivityThirdparty(this.GLOBAL.USERINFO, code,this.$route.params.id);
                if (res.result > 0) {
                    this.isBindPNC = true;
                    sessionStorage.setItem('OctActivityPcnUser', '');
                    this.jumpListPage(id);
                }
            },
            //未绑定pcn
            jumpPCNPage() {
                vant.Dialog.confirm({
                    title: "请绑定PCN账户",
                    message: "<p> 缴费前需要先绑定PCN的账户</p>",
                    confirmButtonText: "立即绑定",
                    cancelButtonText: "取消"
                }).then(() => {
                    location.href ='/App/Believe/index.html?nodeid=' + this.GLOBAL.USERINFO.nodeid + '&sid=' + this.GLOBAL.USERINFO.sid + '&tm=' + this.GLOBAL.USERINFO.tm + '&sign=' + this.GLOBAL.USERINFO.sign + '#/bindpcn';
                }).catch(() => {
                });
            },
            //已绑定PCN
            jumpListPage(id) {
                let activityid = this.$route.params.id;
                this.$router.push({path: `/list/${id}/` + activityid});
            },
            getPcnCode(id) {
                let code = sessionStorage.OctActivityPcnUser;
                if (code) {
                    this.postPcnData(code,id);
                } else {
                    this.getBindPcnData(id)
                }
            },
            //弹窗
            setDialog(index) {
                let messageObj = [
                    {
                        title: '满足条件',
                        message: '下级代理人/合伙人已满足迪拜游活动条件，你需支付200DOS服务费，缴费期间双方服务费支付完成， 代理人/合伙人即可获得迪拜游名额。'
                    },
                    {
                        title: '获得资格',
                        message: '当前账号已获得迪拜游活动条件，需支付200DOS服务费，缴费期间双方服务费支付完成，即可获得迪拜游名额。'
                    }
                ]
                vant.Dialog.alert({
                    title: messageObj[index].title,
                    message: messageObj[index].message,
                    confirmButtonText: "知道了"
                }).then(() => {
                });
            }
        }
    }
</script>

<style scoped lang="scss">

    .home {
        height: 100%;
        overflow-y: scroll;
        background-color: #4462ef;


    }

    .banner {
        height: 311px;

        img {
            width: 100%;
            height: 100%;
        }
    }

    .content {

        margin-top: -1px;
        margin-bottom: 100px;

        .item {
            background-color: #fff;
            margin: 0 26px 25px 27px;
            padding: 25px 15px;
            border-radius: 6px;

            &:first-child {
                border-radius: 0 0 6px 6px;
            }

            .body {
                text-align: left;

                h5 {
                    font-size: 14px;
                    color: #4966ef;
                    margin: 0;
                    padding: 20px 0 10px 0;
                }

                p {
                    font-size: 13px;
                    margin: 0;
                    color: #4a4a4a;
                }
            }
        }
    }

    .footer {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        background-image: linear-gradient(180deg,
                #92a4f4 0%,
                #4966ef 100%);
        box-shadow: 0px -3px 8px 0px rgba(48, 41, 187, 0.25);
        padding: 10px 25px;
        border-radius: 20px 20px 0 0;

        .fail {
            font-size: 16px;
            color: #7a1c23;
            padding: 11px 0;
            background-image: linear-gradient(180deg, #ffe8a8 0%, #fed84b 100%);
            box-shadow: 0px 2px 8px 0px #a71d25;
            border-radius: 6px;
            border: solid 2px #ffffdd;
        }

        .pass {
            display: flex;
            justify-content: space-between;

            .item {
                flex: 1;
                font-size: 14px;
                text-align: left;
                padding-right: 17px;

                &:last-child {
                    text-align: right;
                    padding-right: 0;
                    padding-left: 17px;
                }

                .info {
                    text-align: left;
                    color: #ffffff;
                    padding-bottom: 8px;
                    position: relative;


                    .van-icon-question-o {
                        vertical-align: middle;
                        position: absolute;
                        right: 0;
                        top: 2px;
                        font-size: 16px;
                    }
                }

                .button {
                    display: inline-block;
                    font-size: 16px;
                    height: 40px;
                    line-height: 40px;
                    width: 150px;
                    text-align: center;
                    box-sizing: border-box;
                    font-weight: 700;

                    &.serve {
                        background-image: linear-gradient(180deg,
                                #ffe8a8 0%,
                                #fed84b 100%);
                        box-shadow: 0px 4px 15px 0px #312dbc;
                        border-radius: 6px;
                        border: solid 2px #ffffdd;
                        color: #4462ef;
                    }

                    &.mobile {
                        background-color: #4966ef;
                        box-shadow: 0px 4px 15px 0px #312dbc;
                        border-radius: 6px;
                        border: solid 2px #ffffdd;
                        color: #fff0b8;

                    }

                }
            }

        }
    }

</style>
