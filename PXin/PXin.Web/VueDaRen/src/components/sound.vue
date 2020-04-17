<template>
<div class="popup" @click="setClose($event)">
    <div class="sound">
        <div class="soundbx" id="soundbx">
            <div class="record" v-show="flag == 1">
                <div class="tit">点击开始录音</div>
                <div class="Img" @click="startRecording"><img src="@/assets/images/record.png" alt=""/></div>
            </div>
            <div class="record" v-show="flag == 2">
                <div class="top">
                    <img src="@/assets/images/lft.gif" alt="" />
                    <div class="time">{{ showTime }}</div>
                    <img src="@/assets/images/rgt.gif" alt="" />
                </div>
                <div class="Img" @click="stopRecording"><img src="@/assets/images/record_ing.png" alt=""/></div>
            </div>
            <div class="record" v-show="flag == 3">
                <div class="top">
                    <img src="@/assets/images/voice_bg.png" alt="" />
                    <div class="time">{{ showTime }}</div>
                    <img src="@/assets/images/voice_bg.png" alt="" />
                </div>
                <div class="Img" @click="playRecording"><img src="@/assets/images/record_pause.png" alt=""/></div>
            </div>
        </div>
        <div class="btnbx" v-show="flag == 3">
            <div class="btn" @click="SetCancel">取消</div>
            <div class="btn" @click="uploadSound">保存</div>
        </div>
        <audio controls autoplay id="downloadRec"></audio>
    </div>
</div>
    
</template>

<script>
import { UploadFile } from '@/api/getData';
import { formatSeconds } from '@/config/utils';
export default {
    props: ["show"],
    data() {
        return {
            flag: 1,    // 录音状态， 1： 未开始， 2： 正在录音中， 3： 录音暂停, 4: 继续录音
            showTime: '0:00',   // 显示录音时间
            timer: null,    // 计时器
            nowTime: 0,    // 开始时间
            timeCha: 0,    // 时间差
            overtime: false,    // 超时3分钟限制
            param: [
                {   // 开始录音
                    jsonStr: 'eyJ0eXBlIjoiMCJ9',
                    sign: '7979ee904bd7bbf820380eaedd641f55'
                },{ // 取消录音
                    jsonStr: 'eyJ0eXBlIjoiMSJ9',
                    sign: '1813a294dd9fbfdb0d712e9dc4c96e99'
                },{ // 录音结束
                    jsonStr: 'eyJ0eXBlIjoiMiJ9',
                    sign: 'ac6493a20fa48d1b884b7e293018d556'
                },{ // 暂停录音
                    jsonStr: 'eyJ0eXBlIjoiMyJ9',
                    sign: 'fb89d4ba90e1c8417ec81617126a3ed2'
                },{ // 继续录音
                    jsonStr: 'eyJ0eXBlIjoiNCJ9',
                    sign: '6ecf50bacbfdc610ca03b260eba6231b'
                }
            ]
        }
    },
    created() {
        let _this = this;
        window.nativeRecordCompletion = function(file, time) {
        // 录音回调（音频base64， 音频时间）
            _this.UploadVoice(file);
        }
    },
    watch: {
        showTime(val) {
            if (val == '1:00') {
                this.Toast("最多可录制一分钟，请保存录音或者重新录制！");
                this.overtime = true;
                window.clearInterval(this.timer);
                this.flag = 3;
                setTimeout(()=> {
                    this.AppFunction(3);
                }, 500)
            }
        }
    },
    methods: {
        setClose(e) {
        // 关闭
            if (e.target.className == 'popup' && this.flag == 1) {
                this.$emit('closePop', false);
                this.SetCancel();
            }
        },
        startRecording() {
        // 开始录音
            this.flag = 2;
            this.nowTime = (new Date()).getTime();
            this.AppFunction(0);
            this.SetTimeStart();
            
        },
        stopRecording() {
        // 暂停录音
            window.clearInterval(this.timer);
            this.timeCha = this.timeCha+((new Date()).getTime()-this.nowTime);
            this.flag = 3;
            this.AppFunction(3);
        },
        playRecording() {
        // 继续录音
            if (this.overtime) {
                this.Toast("最多可录制一分钟，请保存录音或者重新录制！");
                return;
            }
            this.nowTime = (new Date()).getTime();
            this.SetTimeStart();
            this.flag = 2;
            this.AppFunction(4);
        },
        SetCancel() {
        // 取消录音
            this.AppFunction(1);
            this.showTime = "0:00";
            this.$emit('showSound', false);
            this.timeCha = 0;
            this.overtime = false;
        },
        uploadSound() {
            // 点击保存，录音结束
            if (this.time<3*1000) {
                this.showTime = "0:00";
                this.timeCha = 0;
                this.flag = 1;
                this.Toast('您的录音必须超过三秒，请重新录制！');
                this.AppFunction(1);
                return;
            }
            this.AppFunction(2);
            this.overtime = false;
        },
        async UploadVoice(content) {
        // 上传音频，返回地址
            let result = await UploadFile({content, typeid: 'wav', imageactiontype: 0, ...this.$global.userInfo});
            if (result.result > 0) {
                this.showTime = "0:00";
                this.flag = 1;
                this.timeCha = 0;
                this.$emit('audioSrc', result.data.fullurl);
            } else {
                this.Toast.fail(result.message)
            }  
        },
        SetTimeStart() {
        // 时间函数
            this.timer = setInterval(() => {
                let nowTime = (new Date()).getTime();
                let cha = nowTime - this.nowTime + this.timeCha;
                this.showTime = formatSeconds((cha/1000)>60?60:(cha/1000));
            }, 1000);
        },
        AppFunction(i) {
        // 调用APP方法（录音）
            try {
                AppNative.jsTunedupNativeWithTypeParamSign(1020, this.param[i].jsonStr, this.param[i].sign);
            } catch(err) {
                this.Toast.fail(err);
            }
        },
    }
}
</script>

<style lang="scss" scoped>
.popup {
    position: fixed;
    bottom: 0;
    left: 0;
    background: rgba($color: #000000, $alpha: 0.5);
    width: 100%;
    height: 100%;
    z-index: -1;
}
.sound {
    height: 5.2rem;
    background: #fff;
    display: flex;
    flex-direction: column;

    position: absolute;
    bottom: 0;
    width: 100%;

    #downloadRec {
        display: none;
    }
    .soundbx {
        flex: auto;
        position: relative;
        .record {
            position: absolute;
            left: 0;
            right: 0;
            width: 100%;
            height: 100%;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            .tit {
                font-size: 0.3rem;
            }
            .Img {
                width: 1rem;
                height: 1rem;
                margin-top: 0.34rem;
                img {
                    width: 100%;
                    height: 100%;
                }
            }
            .top {
                display: flex;
                align-items: center;
                img {
                    width: 1.8rem;
                }
                .time {
                    font-size: 0.3rem;
                    color: #999;
                    padding: 0 0.1rem;
                }
            }       
        }
    }
    .btnbx {
        display: flex;
        padding: 0.15rem 0;
        border-top: 1px solid #d1d1d1;
        .btn {
            flex: 1;
            text-align: center;
            padding: 0.15rem 0;
            color: #2ea2fa;
            &:first-of-type {
                border-right: 1px solid #d1d1d1;
                color: #999;
            }
        }
    }
}
</style>
