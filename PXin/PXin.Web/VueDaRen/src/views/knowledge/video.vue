<template>
    <div class="myvideo">
        <div class="videolist">
            <div class="item" v-for="(item, index) in videoList" :key="index">
                <div class="del_ico" @click="DeleteVideo(item.id)"><img src="@/assets/images/delete.png" alt=""/></div>
                <div class="play_ico" @click="setPlay($event, item)" v-show="!item.play"><img src="@/assets/images/video_pause.png" alt=""/></div>
                <img :src="item.imageurl" class="first">
            </div>
        </div>
        <nodata text='暂未上传视频' v-if="noData"/>
        <div class="btn">
            <button @click="AppUpload">上传视频</button>
        </div>
    </div>
</template>

<script>
import { UploadFile, CreateVideo, DeleteVideo, GetMyVideo } from '@/api/getData';
const nodata = () => import("@/components/nodata");
import axios from 'axios';
export default {
    data() {
        return {
            videoList: [],  // 页面
            noData: false,  // 无数据状态
            userInfo: {},   // 用户信息
        }
    },
    created() {
        this.GetMyVideo();
        let _this = this;
        window.nativeSelecMediaCompletion = function(type, file, img, time) {
            _this.UploadFile(file, img, time);
        }
    },
    methods: {
        AppUpload() {
        // 调用APP视频（单个）
            try {
                AppNative.jsTunedupNativeWithTypeParamSign(1021,'eyJ0eXBlIjoiMSIsIm1heFNlbGVjdENvdW50IjoiMSIsIm1heER1cmluZyI6IjE1In0=','af89d267d466f8f0898fed4c9af9991e');
            } catch(err) {
                this.Toast.fail(err);
            }
        },
        setPlay(e, data) {
        // 点击播放
            let video = data.url;
            let param = window.btoa('{"videoUrlStr":"'+video+'","firstImageBase":""}');
            let sign = hex_md5(param+'F59E5087-DC84-451A-9B74-C854EE0A952B');
            try {
                AppNative.jsTunedupNativeWithTypeParamSign(1023, param, sign);
            } catch (error) {
                this.Toast.fail(err);
            }
        },
        async UploadFile(content, img, time) {
        // 上传文件接口
            let result = await UploadFile({content: content, typeid: 'mp4', imageactiontype: 0, ...this.$global.userInfo});
            let imgSrc = await this.SetUploadFile(img);
            if(result.result > 0) {
                axios.post('/api/DaRen/CreateVideo', {
                    ...this.$global.userInfo,
                    pics: result.data.fullurl,
                    imageurl: imgSrc,
                    duration: Math.ceil(time*1)
                }).then(res => {
                    if(res.data.result > 0) {
                        this.Toast.success(res.data.message);
                        setTimeout(() => {
                            location.reload();
                        }, 500);
                    } else {
                        this.Toast(res.data.message)
                    }
                }).catch(err => {
                    console.log(err);
                })
            } else {
                this.Toast.fail(result.message)
            }
        },
        async SetUploadFile(content) {
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                return result.data.fullurl;
            } else {
                this.Toast.fail(result.message)
            }
            this.keepLastIndex(text);
            this.JudgeText();
        },
        async DeleteVideo(id) {
        // 删除视频-id
            let res = await DeleteVideo({...this.$global.userInfo, id});
            if (res.result > 0) {
                this.Toast.success(res.message);
                setTimeout(() => {
                    location.reload();
                }, 500)
            } else {
                this.Toast.fail(res.message);
            }
        },
        async GetMyVideo() {
        // 获取我的视频列表
            let result = await GetMyVideo(this.$global.userInfo);
            if (result.result > 0) {
                this.videoList = result.data;
                if(!result.data.length) {
                    this.noData = true;
                }
            } else {
                this.Toast.fail(res.message);
            }
        }
    },
    components: {
        nodata
    }
}
</script>

<style lang="scss" scoped>
.myvideo {
    box-sizing: border-box;
    padding-bottom: 0.88rem;
    .videolist {
        padding: 0.3rem;
        .item {
            margin-bottom: 0.3rem;
            position: relative;
            height: 4rem;
            display: flex;
            align-items: center;
            justify-content: center;
            background: #000;
            .del_ico {
                position: absolute;
                top: 0;
                right: 0;
                z-index: 2;
                img {
                    width: 0.36rem;
                    height: 0.36rem;
                }
            }
            .first {
                max-width: 100%;
                height: 100%;
            }
            .play_ico {
                position: absolute;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%);
                z-index: 2;
                img {
                    width: 0.8rem;
                    height: 0.8rem;
                }
            }
            video {
                width: 100%;
                height: 4rem;
            }
        }
    }
    .btn {
        position: fixed;
        bottom: 0;
        width: 100%;
        padding: 0.3rem;
        box-sizing: border-box;
        background: #fff;
        z-index: 3;
        button {
            width: 100%;
            height: 0.88rem;
            background: #2ea2fa;
            border: none;
            color: #fff;
            border-radius: 0.04rem;
        }
        input {
            position: absolute;
            left: 0;
            width: 100%;
            height: 0.88rem;
            opacity: 0;
        }
    }
}
</style>
