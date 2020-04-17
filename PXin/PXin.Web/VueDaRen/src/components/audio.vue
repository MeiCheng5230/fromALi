<template>
    <div class="audio play_audio" v-show="showAudio" :style="{background: bg ? bg:''}">
        <div class="playIco"><img @click="setPause($event)" v-show="playIng" src="static/img/voice_start.png" alt=""><img v-show="!playIng" @click="setPlay($event)" src="static/img/voice_pause.png" alt=""></div>
        <div class="closeIco" v-show="!showDelIco" onclick="DelFile(this)"><img src="static/img/delete.png" alt=""></div>
        <audio data-name='audio' :src="sound" class="audio" @ended="setEnd" @pause="setPaused"></audio>
        <div class="progress">
            <div class="status"><span>语音</span></div>
            <van-progress :percentage="rate" :show-pivot="false" track-color="#999"/>
            <div class="time" @click="getAudioTime($event)">{{ audioTime }}</div>
        </div>
    </div>
</template>

<script>
import { Progress } from 'vant';
import { DeleteFile } from '@/api/getData';
import { formatSeconds } from '@/config/utils';
export default {
    props: ['sound', 'bg', 'flag', 'showDelIco'],
    data() {
        return {
            audioTime: '0:00',   // 音频时间
            timer: null,    // 定时器
            playIng: false,
            rate: 0,
            showAudio: true,
        }
    },
    created() {
        const _this = this;
        window.DelFile = function(dom) {
            let src;
            if(dom.parentNode.querySelector('.Img')) {
                src = dom.parentNode.querySelector('.Img').src
            } else if(dom.parentNode.querySelector('.VideoCoverImg')) {
                src= dom.parentNode.querySelector('.VideoCoverImg').getAttribute('data-videosrc');
            } else if(dom.parentNode.querySelector('.audio')) {
                window.clearInterval(_this.timer);
                _this.rate = 0;
                _this.audioTime="0:00";
                dom.parentNode.querySelector('.audio');
                    src = dom.parentNode.querySelector('.audio').src;
                }
            _this.DeleteFile(src, dom.parentNode);
        };
        window.nativeAppEnterBackground = function() {
            _this.setPause();
        };
    },
    mounted() {
        // 设置音频时常
        let _this = this;
        this.$nextTick(() => {
            let audio = this.$el.querySelector('.audio');
            audio.load();
            audio.oncanplay = function () {
                _this.audioTime = formatSeconds(Math.floor(audio.duration)>=60?60:Math.floor(audio.duration));
            }
        })
    },
    beforeDestroy() {
        let audio = this.$el.querySelector('.audio');
        audio.pause();
    },
    watch: {
        sound(val) {
            // 当播放src为空的时候隐藏组件
            if (val == '') {
                window.clearInterval(this.timer);
                this.showAudio = false;
                return;
            }
            let _this = this;
            this.$nextTick(() => {
                let audio = this.$el.querySelector('.audio');
                audio.load();
                audio.oncanplay = function () {
                    _this.audioTime = formatSeconds(Math.floor(audio.duration)>=60?60:Math.floor(audio.duration));
                }
            })
        }
    },
    methods: {
        setPlay(e) {
        // 点击播放
            let audio = e.target.parentNode.parentNode.querySelector("audio");
            this.playIng = true;
            let allAudio = document.querySelectorAll("audio");
            console.log(allAudio);
            for (const item of allAudio) {
                item.pause();
            };
            audio.play();
            this.timer = setInterval(() => {
                this.rate += 100/audio.duration;
                if (this.rate >= 100) {
                    this.rate = 100;
                    window.clearInterval(this.timer);
                }
            }, 1000);
        },
        setPause() {
        // 点击暂停
            window.clearInterval(this.timer);
            let audio = this.$el.querySelector(".audio");
            this.playIng = false;
            audio.pause();
        },
        setEnd() {
        // 播放完时
            window.clearInterval(this.timer);
            this.rate = 0;
            this.playIng = false;
        },
        getAudioTime(e) {
        // 点击获取音频的时间
            let audio = e.target.parentNode.parentNode.querySelector("audio");
            this.audioTime = formatSeconds(audio.duration)
        },
        async DeleteFile(src, dom) {
        // 删除文件（图片，视频，录音）
            let result = await DeleteFile({...this.$global.userInfo, pics: src});
            if(result.result > 0) {
                this.Toast.success(result.message);
                if (this.flag == 'introduce') {
                    this.$emit('hideAudio', false);
                } else {
                    window.DelItem(src, dom);
                }
            } else {
                this.Toast(result.message);
            }
        },
        setPaused() {
        // 当前音频暂停时
            window.clearInterval(this.timer);
            this.playIng=false;
        }
    }
}
</script>

<style lang="scss" scoped>
.audio {
    padding: 0.44rem 0.3rem;
    margin-bottom: 0.3rem;
    background: #fff;
    display: flex;
    align-items: center;
    position: relative;
    .playIco {
        img {
            width: 0.6rem;
            height: 0.6rem;
            margin-right: 0.3rem;
        }
    }
    .closeIco {
        position: absolute;
        top: 0;
        right: 0;
        img {
            width: 0.36rem;
            height: 0.36rem;
        }
    }
    .progress {
        flex: auto;
        position: relative;
        .status {
            position: absolute;
            bottom: 0.15rem;
        }
        .time {
            position: absolute;
            right: 0;
            top: 0.15rem;
        }
    }
}
</style>
