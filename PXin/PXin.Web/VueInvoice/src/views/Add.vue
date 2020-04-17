<template>
    <div class="add">
        <div class="error-info" v-if="status==3||status==1">{{ status==3?'审核拒绝：'+reason:'增票资质审核状态：审核中' }}</div>
        <van-cell-group>
            <van-field
                    v-for="item in listArr"
                    :key="item.id"
                    :rows="item.id==2?2:1"
                    autosize
                    v-model="item.value"
                    :type="item.type"
                    clearable
                    :label="item.name"
                    :placeholder="item.placeholder"
                    :readonly="item.disabled"
                    :maxlength="item.maxlength"
                    @input="SetInp(item)"
            ></van-field>
        </van-cell-group>
        <div class="btn">
            <van-button v-if="status==0||status==3" size="normal" @click="SetSub">{{ status==3?'修改':'提交审核' }}</van-button>
        </div>
    </div>
</template>

<script>
import { ApplyInvioceQualifica, GetInvioceQualifica } from '@/api/api';
    export default {
        name: "Add",
        data() {
            return {
                status: 0,  // 状态 1-审核中； 2-审核通过； 3-审核拒绝
                reason: '', // 拒绝理由
                listArr:[
                    {
                        id:0,
                        name:'名称',
                        value:'',
                        type:"textarea",
                        placeholder:"公司名称",
                        maxlength: 50,
                    },
                    {
                        id:1,
                        name:'税号',
                        type:"text",
                        value:'',
                        placeholder:'15-20位识别号/统一社会信用代码',
                        maxlength: 20,
                        minlength: 15,
                    },
                    {
                        id:2,
                        name:'单位地址',
                        value:'',
                        type:"textarea",
                        placeholder:'公司注册地址',
                        maxlength: 100,
                    },
                    {
                        id:3,
                        name:'电话号码',
                        value:'',
                        type:"text",
                        placeholder:'公司电话',
                        maxlength: 13,
                        minlength: 11,
                    },
                    {
                        id:4,
                        name:'开户银行',
                        value:'',
                        type:"text",
                        placeholder:'开户银行',
                        maxlength: 50,
                    },
                    {
                        id:5,
                        name:'银行账户',
                        value:'',
                        type:"text",
                        placeholder:'银行账户',
                        maxlength: 19,
                        minlength: 13,
                    },
                ]

            }
        },
        created() {
            if (this.$route.query.status==0) {
                this.status = this.$route.query.status;
            } else {
                this.GetInvioceQualifica();
            }
        },
        watch: {
            status(val) {
                if (val == 1 || val == 2) {
                    for (const item of this.listArr) {
                        this.$set(item, 'disabled', true);
                    }
                }
            }
        },
        methods:{
            SetInp(item) {
                if(item.name==="名称") {
                    this.$set(item, 'value', item.value.replace(/[^\w\u4E00-\u9FA5]/g, ''));
                } else if(item.name==='税号') {
                    this.$set(item, 'value', item.value.replace(/[\W]/g,''));
                } else if(item.name==='电话号码') {
                    this.$set(item, 'value', item.value.replace(/[^\d-]/g,''));
                } else if(item.name==='开户银行') {
                    this.$set(item, 'value', item.value.replace(/[^\w\u4E00-\u9FA5]/g, ''));
                } else if(item.name==='银行账户') {
                    this.$set(item, 'value', item.value.replace(/[^\d]/g,''));
                }
            },
            async SetSub() {
            // 点击提交审核
                for (const item of this.listArr) {
                    if(!item.value) {
                        vant.Toast('请输入您的'+item.name);
                        return;
                    }
                    if(item.minlength && item.value.length < item.minlength) {
                        vant.Toast('请输入至少'+item.minlength+'位数的'+item.name);
                        return;
                    }
                };
                let result = await ApplyInvioceQualifica({
                    company: this.listArr[0].value,
                    taxnum: this.listArr[1].value,
                    address: this.listArr[2].value,
                    mobile: this.listArr[3].value,
                    bank: this.listArr[4].value,
                    cardno: this.listArr[5].value,
                    ...this.GLOBAL.USERINFO
                });
                if (result.result > 0) {
                    vant.Toast.success(result.message);
                    setTimeout(() => {
                        this.$router.go(-1);
                    }, 500)
                } else {
                    vant.Toast.fail(result.message);
                }
            },
            async GetInvioceQualifica() {
            // 获取增票资质信息
                let result = await GetInvioceQualifica(this.GLOBAL.USERINFO);
                if (result.result > 0 && result.data) {
                    let { company, taxnum, address, mobile, bank, cardno, status, note } = result.data;
                    this.listArr[0].value = company;
                    this.listArr[1].value = taxnum;
                    this.listArr[2].value = address;
                    this.listArr[3].value = mobile;
                    this.listArr[4].value = bank;
                    this.listArr[5].value = cardno;
                    this.reason = note || '';
                    this.status = status;
                }
            }
        }
    }
</script>

<style scoped lang="scss">
    .add{
        .error-info{
            background-color: #ffd8cd;
            color: #ff1541;
            padding: 14px 17px;
        }
       .van-cell-group{
           padding-left: 15px;
           .van-cell{
               padding-left: 0;
           }
           .van-cell:not(:last-child)::after{
               border-color: #cccccc;
               left: 0;
           }
       }
        .btn{

            margin: 100px 15px 0 15px;
            .van-button{
               width: 100%;
                background-color: #2ea2fa;
                border-radius: 2px;
                font-size: 18px;
                color: #fff;
            }
        }

    }
</style>
