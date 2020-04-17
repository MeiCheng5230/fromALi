<template>
    <div class="introduce">
        <div class="inp">
            <div class="tit">自我介绍</div>
            <div class="textCont">
                <textarea name="" id="" v-model="text" cols="30" rows="5" maxlength="300" placeholder="请填写真实的简介，向大家展示自己，如擅长领域涉及专业资格，需提交相关资格证书，否则将无法通过审核，简介不少于10个字。"></textarea>

                <!-- 音频播放 -->
                <playaudio v-show="showAudio" :sound="soundSrc" flag="introduce" @hideAudio="HideAudio"/>
                
                <div class="audioIco" @click="soundShow=true"><img src="@/assets/images/icon_vioce.png" alt=""></div>
                <div class="txtnum">{{ text.length }}/300</div>
            </div>
            
        </div>
        <div class="addimg">
            <div class="tit">添加图片(图片会显示在达人主页,可为空)</div>
            <upload :imglist="fileList" :maxnum='9' @delImg='DelImg' />
        </div>
        <div class="btn">
            <button :class="text.length>=10?'': 'nosub'" @click="SetSub">保存</button>
        </div>
        <div class="popup" v-if="soundShow">
            <div class="recordsound">
                <sound :show="soundShow" @audioSrc="GetAudioSrc" @showSound="closePop" @closePop="closePop"/>
            </div>
        </div>
    </div>
</template>

<script>
import upload  from '@/components/upload';
import { UpdateSelfIntroduction } from '@/api/getData';
import { removeStore } from '@/config/utils';
const sound = () => import('@/components/sound');
const playaudio = () => import("@/components/audio");
export default {
    data() {
        return {
            text: '',
            fileList: [],
            soundShow: false,
            soundSrc: '',
            showAudio: false,
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        this.text = userinfo.selfintroduction || '';
        this.fileList = userinfo.pic || [];
        this.soundSrc = userinfo.voiceurl || '';
        if (this.soundSrc.length) {
            this.showAudio = true;
        } else {
            this.showAudio = false;
        }
    },
    methods: {
        closePop(data) {
            this.soundShow = data;
        },
        async UpdateSelfIntroduction() {
        // 点击保存自我介绍
            var pics = this.fileList.join(',');
            var introduce= this.text;
            let result = await UpdateSelfIntroduction({pics, introduce, voiceurl: this.soundSrc, ...this.$global.userInfo});
            if(result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    removeStore('info');
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
            }
        },
        GetAudioSrc(data) {
        // 接受音频url
            this.showAudio = true;
            this.soundSrc = data;
            this.soundShow = false;
        },
        HideAudio(data) {
        // 隐藏播放组件
            this.showAudio = data;
            this.soundSrc = '';
        },
        SetSub() {
        // 点击保存按钮
            if(this.text.length && this.text.length >= 10) {
                this.UpdateSelfIntroduction();
            }
        },
        DelImg(idx) {
        // 删除图片
            this.fileList.splice(idx, 1);
        },
    },
    components: {
        upload, sound, playaudio
    }
}
</script>

<style lang="scss" scoped>
.introduce {
    min-height: 100%;
    padding: 0.3rem;
    box-sizing: border-box;
    .inp {
        .textCont {
            position: relative;
            background: #f7f7fc;
            margin-top: 0.3rem;
            padding: 0.3rem;
            padding-bottom: 1rem;
        }
        .audio {
            padding: 0.44rem 0.3rem;
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
        textarea {
            width: 100%;
            border: none;
            box-sizing: border-box;
            background: #f7f7fc;
            resize: none;
        }
        .txtnum {
            position: absolute;
            right: 0.3rem;
            bottom: 0.2rem;
            color: #999;
        }
        .audioIco {
            position: absolute;
            left: 0.3rem;
            bottom: 0.2rem;
            img {
                width: 0.48rem;
                height: 0.48rem;
            }
        }
    }
    .addimg {
        .tit {
            padding: 0.3rem 0;
        }
    }
    .btn {
        margin-top: 1rem;
        button {
            background: #2ea2fa;
            border-radius: 0.04rem;
            color: #fff;
            width: 100%;
            border: none;
            padding: 0.28rem 0;
            &.nosub {
                background: rgba($color: #2ea2fa, $alpha: 0.5);
            }
        }
    }
    .popup {
        width: 100%;
        height: 100%;
        // background: rgba($color: #000000, $alpha: 0.5);
        position: fixed;
        top: 0;
        left: 0;
    }
    /deep/ .sound {
        // width: 100%;
        // position: absolute;
        // bottom: 0;
        // left: 0;
        border-radius: 0.2rem 0.2rem 0 0;
        overflow: hidden;
    }
}
</style>
