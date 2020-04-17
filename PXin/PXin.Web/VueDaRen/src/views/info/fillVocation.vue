<template>
    <div class="fillvoac">
        <div class="ops">
            <div class="lft">头衔职位</div>
            <div class="rgt"><input type="text" v-model="position" :readonly="!showBtn" placeholder="请输入您的职位名称" /></div>
        </div>
        <div class="ops">
            <div class="lft">任职机构</div>
            <div class="rgt"><input type="text" v-model="company" :readonly="!showBtn" placeholder="请输入您的任职机构" /></div>
        </div>
        <div class="ops time">
            <div class="lft">任职时间</div>
            <div class="rgt">
                <div class="start" @click="ShowTime('start')">{{ start || '开始时间' }}</div>
                至
                <div class="end" @click="ShowTime('end')">{{ endTime }}</div>
            </div>
        </div>

        <div class="upload">
            <div class="tit">材料上传</div>
            <div class="txt">请提供职位的名片、工作证、在职证明劳动合同和专业领域的一些照片等，证明材料仅供审核参考，信息严格保密，不会显示在达人首页，请务必提供真实有效的材料否则将会导致审核不通过。</div>
            <upload :imglist="fileList" :maxnum='9' @delImg='DelImg' />
        </div>

        <div class="btn" v-if="showBtn">
            <button @click="SetConfirm" :class="fill?'fillbtn': ''" >保存</button>
        </div>
        <div class="delbtn" v-if="showDel">
            <button @click="SetDelete">删除</button>
        </div>

        <!-- 时间选择 -->
        <van-popup v-model="timeShow" position="bottom">
            <van-datetime-picker
                v-model="currentDate"
                type="year-month"
                title="时间选择"
                :formatter="Formatter"
                @confirm="StartTime"
                @cancel="timeShow=false"
                :max-date="nowDate"
                :minDate="minDate"
            />
        </van-popup>

        
    </div>
</template>

<script>
const upload = () => import('@/components/upload');
import { DeleteDaRenOccOrEdu, CreateDaRenOccupations, UpdateDaRenOccupations } from '@/api/getData';
import { Dialog } from 'vant';
export default {
    data() {
        return {
            currentDate: new Date(),
            timeShow: false,// 选择时间显示
            start: '',  // 开始于
            end: '',    // 结束于
            fileList: [],   // 上传证件照片
            nowShow: '',   // 判断此时选择时间为开始还是结束
            showBtn: false,
            company: '',
            position: '',
            showDel: false,
            id: '',
            change: false,      // 添加/修改    true为修改， false为添加
            fill: false,  // 是否填写完成
            nowDate: new Date(),
            minDate: new Date('January 01,1960'),
        }
    },
    created() {
        this.bus.$on('voac', this.SetBusData);
        var userinfo = JSON.parse(this.getStore('info'));
        if(userinfo.status == 0 || userinfo.status == 2) {
            this.showBtn = true;
        }
    },
    updated() {
        if(this.company && this.position && this.start && this.end) {
            this.fill = true;
        } else {
            this.fill = false;
        }
    },
    computed: {
        endTime() {
            var now = new Date();
            if (new Date(this.end.replace(/-/g,"/")).getFullYear() >= now.getFullYear() && new Date(this.end.replace(/-/g,"/")).getMonth() == now.getMonth()) {
                return '至今';
            } else if (!this.end) {
                return '结束时间';
            } else {
                return this.end;
            }
        }
    },
    methods: {
        async DeleteDaRenOccOrEdu() {
        // 删除
            let result = await DeleteDaRenOccOrEdu({id: this.id, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
        async CreateDaRenOccupations() {
        // 添加
            let imgstr = this.fileList.join(',');
            let params = {position: this.position, fromtime: this.start, endtime: this.JudgeTime(), company: this.company, pics: imgstr }
            let result = await CreateDaRenOccupations({...params, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message)
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000);
                
            } else {
                this.Toast.fail(result.message);
            }
        },

        async UpdateDaRenOccupations(extid) {
        // 修改   
            let imgstr = this.fileList.join(',');
            let params = {extid: extid, position: this.position, fromtime: this.start, endtime: this.JudgeTime(), company: this.company, pics: imgstr }
            let result = await UpdateDaRenOccupations({...params, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1)
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
        SetDelete() {
            Dialog.confirm({
                message: '您确定要删除该条数据吗？',
                confirmButtonText: '确定'
            }).then(() => {
                this.DeleteDaRenOccOrEdu();
            })
        },
        Formatter(type, value) {
            if (type === 'year') {
                return `${value}年`;
            } else if (type === 'month') {
                return `${value}月`
            }
            return value;
        },
        StartTime(date) {
            var time = this.DateFormat(date);
            if(this.nowShow == 'start') {
                this.start = time;
            } else if(this.nowShow == 'end') {
                this.end = time;
            }
            this.timeShow = false;
        },
        DateFormat(val) {
            if (val != null) {
                var date = new Date(val);
                return date.getFullYear() + '-' + (date.getMonth() + 1);
            }
        },
        ShowTime(type) {
            if(this.showBtn) {
                this.timeShow = true;
                this.nowShow = type;
            }
        },
        SetConfirm() {
            if(this.fill) {
                if(this.change) {
                    this.UpdateDaRenOccupations(this.id);
                } else {
                    this.CreateDaRenOccupations();
                }
            }
        },
        DelImg(idx) {
        // 删除图片
            this.fileList.splice(idx, 1);
        },
        SetBusData(data) {
        // 接受传值
            console.log(data);
            if(data) {
                 this.change = true;
                var userinfo = JSON.parse(this.getStore('info'));
                if(userinfo.status == 0 || userinfo.status == 2) {
                    this.showDel = true;
                }
            }
            this.start = this.DateFormat(data.fromtime.replace(/-/g,"/"));
            this.end = this.DateFormat(data.endtime.replace(/-/g,"/"));

            this.position = data.position;
            this.company = data.company;
            this.fileList = data.pics || [];
            this.id = data.id;
        },
        JudgeTime() {
            var now = new Date();
            if (new Date(this.end.replace(/-/g,"/")).getFullYear() >= now.getFullYear() && new Date(this.end.replace(/-/g,"/")).getMonth() == now.getMonth()) {
                return this.end.split('-')[0]*1 + 20 + '-' + this.end.split('-')[1]
            } else {
                return this.end
            }
        }
    },
    beforeDestroy() {
        this.bus.$off('educ', this.SetBusData);
    },
    components: {
        upload
    }
}
</script>

<style lang="scss" scoped>
.fillvoac {
    padding: 0 0.3rem;
    .ops {
        border-bottom: 1px solid #d1d1d1;
        padding: 0.3rem 0;
        display: flex;
        align-items: center;
        .rgt {
            flex: auto;
            text-align: right;
            input {
                border: none;
                text-align: right;
                &::placeholder {
                    color: #999;
                }
            }
        }
        &.time {
            .rgt {
                display: flex;
                justify-content: flex-end;
                align-items: center;
                font-size: 0.24rem;
                .start, .end {
                    background: #f1f1f1;
                    padding: 0.1rem 0.3rem;
                    margin: 0 0.2rem;
                }
            }
        }
    }
    .upload {
        .tit {
            padding-top: 0.6rem;
            font-weight: bold;
        }
        .txt {
            padding: 0.3rem 0 0.3rem;
            color: #999;
            font-size: 0.24rem;
        }
        .img {
            background: #f7f7fc;
            padding: 0.3rem 0.3rem 0.4rem 0.3rem;
            min-height: 3.2rem;
            position: relative;
            .imgnum {
                position: absolute;
                right: 0.3rem;
                bottom: 0.1rem;
                font-size: 0.3rem;
                color: #999;
                padding-bottom: 0.1rem;
            }
        }
    }
    .btn {
        margin: 1rem 0 0.3rem;
        button {
            border: none;
            color: #fff;
            background: rgba($color: #2ea2fa, $alpha: 0.5);
            border-radius: 0.04rem;
            width: 100%;
            height: 0.88rem;
            &.fillbtn {
                background: #2ea2fa;
            }
        }
    }
    .delbtn {
        margin: 0rem 0 0.3rem;
        button {
            border: 1px solid #999;
            color: #999;
            background: #fff;
            border-radius: 0.04rem;
            width: 100%;
            height: 0.88rem;
        }
    }
}
</style>
