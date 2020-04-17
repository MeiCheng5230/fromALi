<template>
    <div class="apply">
        <div class="items">
            <h5>发票类型</h5>
            <div class="item">
                <div class="item-nav">
                    <div :class="typeActive===1?'nav-btn active':'nav-btn'" @click="SetType(1)">增值税普通发票</div>
                    <div :class="typeActive===2?'nav-btn active':'nav-btn'" @click="SetType(2)">增值税专用发票</div>
                </div>
                <!-- <div class="item-info">电子普通发票与纸质发票具备同等法律效力，可支持报销入账</div> -->
            </div>
        </div>
        <div class="items">
            <h5>发票抬头</h5>
            <div class="item">
                <div class="item-nav" v-show="typeActive===1">
                    <div :class="headActive==1?'nav-btn active':'nav-btn'" @click="SetHeadActive(1)">个人及政府事业单位</div>
                    <div :class="headActive==0?'nav-btn active':'nav-btn'" @click="SetHeadActive(0)">企业</div>
                </div>
                <div class="item-input">
                    <van-field v-model="invoiceHead" :readonly="typeActive==2?true:false" placeholder="请输入发票抬头"/>
                    <van-field v-show="typeActive===2||headActive===0" :readonly="typeActive==2?true:false" @input="SetInpt" v-model="numCode"
                    maxlength="20" placeholder="请输入纳税人识别号/统一社会信用代码"/>
                    <van-field v-show="typeActive===2" readonly v-model="companyAddress" placeholder="请输入公司注册地址"/>
                    <van-field v-show="typeActive===2" readonly v-model="companyPhone" placeholder="请输入公司电话"/>
                    <van-field v-show="typeActive===2" readonly v-model="bankName" placeholder="请输入公司开户银行"/>
                    <van-field v-show="typeActive===2" readonly v-model="bankNum" placeholder="请输入公司开户银行账户"/>
                </div>
            </div>
        </div>
        <div class="items info">
            <!-- v-show="typeActive===2" -->
            <div class="addArea"><h5>收票人信息</h5><van-icon name="plus" size="18px" @click="SetAddr"/></div>
            <div class="item">
                <div class="selRes" @click="SetShowArea"><div class="lft">选择发票收件人</div><van-icon name="arrow" /></div>
                <div class="item-input">
                    <!-- <van-field v-show="typeActive===1" v-model="email" placeholder="请输入邮箱，用来接收电子发票"/> -->
                    <van-field readonly v-model="recInfo.name" placeholder="收票人姓名"/>
                    <van-field readonly v-model="recInfo.phone" placeholder="收票人号码"/>
                    <van-field readonly v-model="recInfo.pcraddress" placeholder="所在地区"/>
                    <!-- <van-field readonly v-model="recInfo.address" placeholder="详细地址"/> -->
                    <div style="background: #f7f7fc; font-size: 0.4rem; padding: 12px 16px; white-space: nowrap; overflow: auto;" v-text="recInfo.address"></div>
                </div>
                <!-- <div class="item-info" v-show="typeActive===1">如需下载打印电子普通发票，可进入‘已申请’自行下载打印</div> -->
                <div class="item-info" v-show="typeActive===2">本人同意相信科技使用本人上述信息，以实现开具发票的目的</div>
                
            </div>
        </div>
        <div class="btn" @click="SetSubmit">提交</div>
        <van-action-sheet v-model="showArea" title="请选择一个收票人" :round="false">
            <div class="area_item" v-for="(item, index) in areaList" :key="index" @click="SetArea(item)">
                <van-icon name="success" color="#ffb90f" size="24px" v-if="item.select" />
                <div class="area_name">{{ item.name }} ({{ item.phone }})</div>
                <div class="area_address">{{ item.pcraddress + item.address }}</div>
            </div>
        </van-action-sheet>
    </div>
</template>

<script>
import { ApplyWriteInvioce, GetInvioceQualifica, GetShoppingAddrs } from '@/api/api';
    export default {
        name: "Apply",
        data() {
            return {
                typeActive: 1,   // 发票类型 0-电子普通发票，1-增值税专业发票
                headActive: 1,   // 发票抬头 0-个人及政府事业单位，1-企业
                invoiceHead: '',    // 发票抬头
                email: '',  // 收票人邮箱
                companyAddress: '',     // 公司地址
                companyPhone: '',   // 公司电话
                numCode: '',    // 纳税人识别号/统一社会信用代码
                bankName: '',   // 银行
                bankNum: '',    // 银行卡号
                status: '', // 是否通过增票资质审核 0-未填写， 1-审核中， 2-审核通过， 3-审核拒绝
                idno: '',   // 充值码卡id(逗号分割)
                areaList: [],   // 收件人列表
                showArea: false,    // 选择收件人
                recInfo: {},    // 收件人信息
            }
        },
        created() {
            this.status = this.$route.query.status;
            this.idno = this.$route.query.idno;
            this.invoiceHead = this.$route.query.name;
            this.GetShoppingAddrs();
            let _this = this;
            // 添加地址成功回调
            window.nativeCreatAddressCompletion = function() {
                _this.GetShoppingAddrs();
            }
        },
        methods: {
            SetInpt() {
                this.$set(this, 'numCode', this.numCode.replace(/[\W]/g,''));
            },
            SetShowArea() {
            // 点击收票人信息
                if(!this.areaList.length) {
                    vant.Toast("暂无收件人，请先去添加！");
                    return;
                }
                this.showArea = true;
            },
            SetType(type) {
            // 增值税专用发票
                if (type == 1) {
                    this.typeActive = type;
                    if (this.headActive == 1) {
                        this.invoiceHead = this.$route.query.name;
                    } else if (this.headActive == 0) {
                        this.invoiceHead = '';
                        this.numCode = '';
                    }
                    return;
                }
                if(this.status == 0) {
                    vant.Dialog.confirm({
                        title: '温馨提示',
                        message: '增值税专用发票资质需在开票首页进行添加，资质审核通过后即可申请开票，现在添加？'
                    }).then(() => {
                        this.$router.push('/add');
                    });
                } else if(this.status == 3) {
                    vant.Dialog.confirm({
                        title: '温馨提示',
                        message: '您的资质审核未通过，现在去重新提交申请信息？'
                    }).then(() => {
                        this.$router.push('/add');
                    });
                } else if (this.status == 1) {
                    vant.Dialog.alert({
                        title: '温馨提示',
                        message: '您的增票资质正在审核中'
                    });
                } else {
                    this.typeActive = 2;
                    this.GetInvioceQualifica();
                    // this.GetShoppingAddrs();
                }
            },
            SetHeadActive(type) {
            // 点击设置发票抬头 1-个人及政府； 2-企业
                this.headActive = type;
                if (type == 1) {
                    this.invoiceHead = this.$route.query.name;
                } else if (type == 0) {
                    this.invoiceHead = '';
                    this.numCode = '';
                }
            },
            SetAddr() {
            // 添加地址
                try {
                    AppNative.jsTunedupNativeWithTypeParamSign(1022, '', '');
                } catch(err) {
                    vant.Toast.fail(err);
                }
            },
            SetSubmit() {
            // 点击提交
                const reg = new RegExp("^[a-z0-9A-Z]+[- | a-z0-9A-Z . _]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$");
                if(!this.invoiceHead.length) {
                    vant.Toast('请输入发票抬头！');
                    return;
                }
                if(this.typeActive == 1) {
                    // if(!reg.test(this.email)) {
                    //     vant.Toast('您的邮箱格式不正确');
                    //     return;
                    // }
                    if (!this.recInfo.consigneeid) {
                        vant.Toast('请选择或者去添加收件人!');
                        return;
                    }
                    if(this.headActive == 0) {
                        if(this.numCode.length< 15) {
                            vant.Toast('请输入15-20位纳税人识别号/统一社会信用代码！');
                            return;
                        }
                    }
                    this.ApplyWriteInvioce({
                        idno : this.idno,
                        typeid: this.typeActive,
                        isperson: this.headActive,
                        head: this.invoiceHead,
                        code: this.headActive==0?this.numCode:'0',

                        address: this.recInfo.consigneeid,
                        email: 0,
                        // email: this.email,
                        // address: '0',

                        ...this.GLOBAL.USERINFO
                    });
                } else if(this.typeActive == 2) {
                    if (!this.recInfo.consigneeid) {
                        vant.Toast('请选择或者去添加收件人!')
                    } else {
                        this.ApplyWriteInvioce({
                            idno: this.idno,
                            typeid: 2,
                            address: this.recInfo.consigneeid,
                            isperson: '0',
                            head: '0',
                            code: '0',
                            email: '0',
                            ...this.GLOBAL.USERINFO
                        })
                    }
                }
            },
            SetArea(item) {
            // 选择一条收件人
                for (const itm of this.areaList) {
                    this.$set(itm, 'select', false);
                }
                this.$set(item, 'select', true);
                this.recInfo = item;
                this.showArea = false;
            },
            async ApplyWriteInvioce(data) {
            // 提交开票
                let result = await ApplyWriteInvioce(data);
                if (result.result > 0) {
                    vant.Toast.success(result.message);
                    setTimeout(() => {
                        this.$router.go(-1);
                    }, 1000)
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async GetInvioceQualifica() {
            // 获取增票资质信息
                let result = await GetInvioceQualifica(this.GLOBAL.USERINFO);
                if (result.result > 0) {
                    let { address, bank, cardno, company, mobile, taxnum } = result.data;
                    this.invoiceHead = company;
                    this.numCode = taxnum;
                    this.bankName = bank;
                    this.companyPhone = mobile;
                    this.bankNum = cardno;
                    this.companyAddress = address;
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async GetShoppingAddrs() {
            // 获取收货地址
                let result = await GetShoppingAddrs(this.GLOBAL.USERINFO);
                if (result.result > 0) {
                    this.areaList = result.data;
                    for (const item of result.data) {
                        if (item.isdefault == 1) {
                            this.recInfo = item;
                            this.$set(item, 'select', true);
                        }
                    }
                    if (!this.recInfo.consigneeid && result.data[0]) {
                        this.recInfo = result.data[0]
                        this.$set(result.data[0], 'select', true);
                    }
                } else {
                    vant.Toast.fail(result.message);
                }
            }
        }
    }
</script>

<style scoped lang="scss">
    .apply {
        padding: 15px 15px 65px 15px;

        .items {
            background-color: #fff;
            padding: 15px 15px 20px 15px;
            box-shadow: 0px 1px 5px 0px #eaeaea;
            border-radius: 5px;
            margin-bottom: 15px;

            h5 {
                font-size: 15px;
                margin: 0;
                padding: 0;
            }

            .item {
                padding-top: 30px;

                .item-nav {
                    display: flex;
                    padding-bottom: 30px;
                    .nav-btn {
                        padding: 9px 16px;
                        font-size: 15px;
                        background-color: #f2f2f2;
                        color: #999999;
                        border-radius: 16px;
                        margin-right: 30px;

                        &.active {
                            color: #fff;
                            background-color: #2ea2fa;
                        }

                        &:last-child {
                            margin-right: 0;
                        }
                    }
                }

                .item-input {
                
                    /deep/ .van-cell {
                        background-color: #f7f7fc;
                        margin-bottom: 15px;
                        .van-cell__value {
                            .van-field__body {
                                input {
                                    overflow-x: auto;
                                }
                            }
                        }
                        .van-field__control {
                            font-size: 15px;
                        }
                    }
                }

                .item-info {
                    color: #999;
                    padding-top: 10px;
                    font-size: 12px;
                }
            }

        }

        .btn {
            background-color: #2ea2fa;
            color: #fff;
            padding: 15px 0;
            font-size: 15px;
            position: fixed;
            bottom: 0;
            left: 0;
            right: 0;
            text-align: center;
        }
    }
    .area_item {
        padding: 8px 10px;
        border-bottom: 1px solid #eee;
    }
    .selRes {
        display: flex;
        align-items: center;
        font-size: 15px;
        color: #999;
        background: #f7f7fc;
        margin-bottom: 15px;
        padding: 10px 16px;
        .lft {
            flex: auto;
        }
    }
    .items.info {
        .addArea {
            display: flex;
            align-items: center;
            h5 {
                flex: auto;
            }
        }
    }
    .area_item {
        position: relative;
        .van-icon.van-icon-success {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
        }
    }
</style>
