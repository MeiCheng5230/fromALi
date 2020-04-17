<template>
    <div class="additional">
        <div class="inpbx">
            <div class="title ops">
                <div class="tit">主题</div>
                <input type="text" placeholder="请输入你的知识问答题的问题" v-model="title" />
            </div>
            <div class="ops">
                <div class="tit">查看需支付</div>
                <span>(需要先选择支付类型)</span>
                <img class="ico" @click="SetTit" src="@/assets/images/icon_hint.png" alt="">
            </div>
            <div class="ops inpnum">
                <input type="text" v-model="price" @input="price=price.replace(/[^\d]/g,'')" placeholder="选择支付类型后在此处输入数量" />
                <div class="paytype">
                    <div v-for="(item, index) in payTypeList" :key="index" :class="item.select?'item sel':'item'" @click="setPayType(item)">{{ item.tit }}
                    </div>
                </div>
            </div>
            <div class="ops inp">
                <div id="play_radio" style="width: 100%"><playaudio v-for="(item, index) in soundList" flag="additional" :key="index" :sound="item" bg="#f7f7fc" @delAudio="SetDelAudio"/></div>
                <div id="showbx" contentEditable="true" @input="JudgeText" @focus="showSound=false"></div>
                <div v-show="showHodler" class="placeholder" id="placeholder">输入你的回答，点击右下方按钮支持上传图片、视频、语音</div>
            </div>
            
            
        </div>
        <div class="poi_bot" v-if="showBot">
            <div class="bot_f">
                <div class="ico_img" @click="AppUpload(0)">
                    <img src="@/assets/images/icon_image.png" alt=""/>
                </div>
                <div class="ico_img" @click="AppUpload(1)">
                    <img src="@/assets/images/icon_video.png" alt=""/>
                </div>
                <div class="ico_img" @click="showSound=true"><img src="@/assets/images/icon_vioce.png" alt=""/></div>
                <div class="btn">
                    <button :class="fillFlag ? 'clc': ''" @click="SetRelease">发布</button>
                </div>
            </div>
            <sound v-if="showSound" @showSound="SetShowSound" :show="showSound" @audioSrc="GetAudioSrc" />
        </div>
    </div>
</template>

<script>
import { Dialog } from 'vant';
const sound = () => import('@/components/sound');
import playaudio from '@/components/audio';
import { UploadFile, DeleteFile, CreateOrUpdateDaRenKnowledge } from '@/api/getData';
import { compressImage } from '@/config/compressImage';
import Vue from 'vue';
export default {
    data() {
        return {
            payTypeList: [{tit: 'V点', select: true}, {tit: 'UV'}],
            showSound: false,   // 录音组件
            showBot: true,      // 底部显示
            dialogComp: null,
            showHodler: true,
            title: '',  // 主题
            paytype: 0, // 支付类型 0=V点 1=UV ,
            price: '',  // 金额
            fillFlag: false,    // 是否填写完成
            soundList: [],   // 添加录音的src
        }
    },
    created() {
        let _this = this;
        window.DelFile = function(dom) {
            let src;
            if(dom.parentNode.querySelector('.Img')) {
                src = dom.parentNode.querySelector('.Img').src;
            } else if(dom.parentNode.querySelector('.VideoCoverImg')) {
                src= dom.parentNode.querySelector('.VideoCoverImg').getAttribute('data-videosrc');
            } else if(dom.parentNode.querySelector('.audio')) {
                src = dom.parentNode.querySelector('.audio').src;
            }
            _this.DeleteFile(src, dom.parentNode);
        };
        window.nativeSelecMediaCompletion = function(type, file, img, time) {
            if (type == 0) {
                _this.UploadImg(file);
            } else {
                _this.SetVideo(file, img);
            }
        };
        window.DelItem = function(src, dom) {
            if (_this.soundList.indexOf(src) < 0) {
                dom.parentNode.removeChild(dom);
                return;
            }
            _this.soundList.splice(_this.soundList.indexOf(src), 1);
        };
    },
    updated() {
        this.JudgeText();
    },
    watch: {
        showSound(val) {
            let bot = this.$el.querySelector(".poi_bot");
            if (val) {
                bot.style.bottom = "5.2rem";
            } else {
                bot.style.bottom = 0;
            }
        }
    },
    methods: {
        AppUpload(i) {
            this.showSound = false;
            let param = [
                {
                    jsonStr: 'eyJ0eXBlIjoiMCIsIm1heFNlbGVjdENvdW50IjoiMSIsIm1heER1cmluZyI6IjAifQ==',
                    sign: '02ee71ba639e5f2213b5661b6370c877'
                    
                }, {
                    jsonStr: 'eyJ0eXBlIjoiMSIsIm1heFNlbGVjdENvdW50IjoiMSIsIm1heER1cmluZyI6IjE1In0=',
                    sign: 'af89d267d466f8f0898fed4c9af9991e'
                }
            ];
            try {
                AppNative.jsTunedupNativeWithTypeParamSign(1021, param[i].jsonStr, param[i].sign);
            } catch(err) {
                this.Toast.fail(err);
            }
        },
        JudgeText() {
        // 判断是否表单是否为空，是否可以提交
            let placeholder = document.querySelector("#placeholder");
            let html = document.querySelector("#play_radio").innerHTML + document.querySelector("#showbx").innerHTML;
            if (html.length > 0) {
                this.showHodler = false;
            } else {
                this.showHodler = true;
            };
            if (this.title && this.price && html.length) {
                this.fillFlag = true;
            } else {
                this.fillFlag = false;
            }
        },
        SetTit() {
            Dialog.alert({
                title: '温馨提示',
                message: '<p style="text-align: left;">支付方式为V点，您和查看者可增加对应数量的P点</p><p style="text-align: left;">支付方式为UV，你可增加设置金额80%的UV，查看者可增加10倍的设置金额的P点</p>',
                confirmButtonText: '知道了'
            })
        },
        setPayType(data) {
            if(data.tit == 'V点') {
                this.SetDialog('支付方式为V点，您和查看者可增加对应数量的P点', data, 0);
            } else if(data.tit == 'UV') {
                this.SetDialog('支付方式为UV，你可增加设置金额80%的UV，查看者可增加10倍的设置金额的P点', data, 1);
            }
        },
        SetDialog(txt, data, num) {
            Dialog.confirm({
                title: '温馨提示',
                message: '<p style="text-align: left;">'+ txt +'</p>',
                confirmButtonText: '确定'
            }).then(() => {
                for (const item of this.payTypeList) {
                    this.$set(item, 'select', false);
                }
                this.$set(data, 'select', true);
                this.paytype = num;
            })
        },
        SetShowSound(data) {
        // 底部录音组件显示隐藏
            this.showSound = data;
        },
        SetDelAudio(src) {
        // 删除语音
            this.soundList.splice(this.soundList.indexOf(src), 1);
        },
        async SetVideo(video, img) {
        // 上传视频文件
            this.showSound = false;
            var text = document.querySelector("#showbx");
            let VideoSrc = await this.UploadVideo(video);
            let imgSrc = await this.SetUploadFile(img);

            var newHtml = "<div style='position: relative; width: 100%; height: 4rem; background: #000; display: flex; justify-content: center;' class='item video_div' contentEditable='false'>";
            newHtml += "<img style='position: absolute; width: 0.36rem; right: 0; top: 0; z-index: 2;' class='del_Ico' src='static/img/delete.png' onclick='DelFile(this)'>";
            newHtml += "<img data-videosrc='"+VideoSrc+"' style='position: absolute; top: 50%; left: 50%; width: 0.8rem; transform: translate(-50%, -50%)' src='static/img/video_pause.png' onclick='SetPlayVideo(this)' class='VideoCoverImg'>";
            newHtml += "<img data-name='img' src='"+imgSrc+"' style='max-width: 100%; height: 100%;' class='cover'>";
            newHtml += "</div><br>";
            text.innerHTML += newHtml;
            this.JudgeText();
            this.keepLastIndex(text);
        },

        async UploadImg(content) {
        // 上传图片
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            console.log(result);
            if(result.result > 0) {
                var text = document.querySelector("#showbx");
                text.innerHTML += "<div style='position: relative;' class='item img_div' contenteditable='false'><img onclick='DelFile(this)' style='width: 0.36rem; position: absolute; right: 0; top: 0;' class='del_Ico' src='static/img/delete.png'/><img data-name='img' src='"+ result.data.fullurl +"' style='width: 100%;' class='Img' /></div><br>"
                this.keepLastIndex(text);
                this.JudgeText();
            } else {
                this.Toast.fail(result.message)
            }
        },
        GetAudioSrc(data) {
        // 添加录音组件
            this.soundList.push(data);
            this.showSound = false;
            this.JudgeText();
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

        async CreateOrUpdateDaRenKnowledge(status) {
        // 发布
            let delIco = document.querySelector("#showbx").querySelectorAll(".del_Ico");
            for (const itm of delIco) {
                itm.style.display = 'none';
            };
            let srcStr = this.soundList.join(',');
            let html = document.querySelector("#showbx").innerHTML;
            let result = await CreateOrUpdateDaRenKnowledge({...this.$global.userInfo, id: 0, title: this.title, paytype: this.paytype, price: this.price*1, content: html, voiceurl: srcStr, status: status });
            if (result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast.fail(result.message);
                for (const ico of delIco) {
                    ico.style.display = 'block';
                };
            }
        },

        async SetRelease() {
        // 发布知识库
            if (!this.title) {
                this.Toast("请输入您的主题！");
                return;
            };
            if (!this.price) {
                this.Toast("请输入您的价格！");
                return;
            };
            let html = document.querySelector("#play_radio").innerHTML + document.querySelector("#showbx").innerHTML;
            if (html <= 0) {
                this.Toast("请输入您的回答内容！");
                return;
            };
            if (this.soundList.length>3) {
                this.Toast("您的语音不能超过三条！");
                return;
            };
            let videoLen = document.querySelector("#showbx").querySelectorAll(".video_div").length;
            if(videoLen > 3) {
                this.Toast("您上传视频不能超过三条！");
                return;
            };
            let imgLen = document.querySelector("#showbx").querySelectorAll(".img_div").length;
            if(imgLen > 9) {
                this.Toast("您上传的图片不能超过九张！");
                return;
            };
            Dialog.confirm({
                title: '温馨提示',
                message: '现在要发布该知识库内容吗？',
                cancelButtonText: '存为草稿',
                confirmButtonText: '发布',
                cancelButtonColor: '#999'
            }).then(() => {
                this.CreateOrUpdateDaRenKnowledge(1);
            }).catch(() => {
                this.CreateOrUpdateDaRenKnowledge(0);
            });
        },
        async UploadVideo(content) {
        // 上传视频
            let result = await UploadFile({content: content, typeid: 'mp4', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                return result.data.fullurl;
            } else {
                this.Toast.fail(result.message)
            }
        },
        keepLastIndex(obj) {
        // 设置光标
            if (window.getSelection) {//ie11 10 9 ff safari
                obj.focus(); //解决ff不获取焦点无法定位问题
                var range = window.getSelection();//创建range
                range.selectAllChildren(obj);//range 选择obj下所有子内容
                range.collapseToEnd();//光标移至最后
            } else if (document.selection) {//ie10 9 8 7 6 5
                var range = document.selection.createRange();//创建选择对象
                //var range = document.body.createTextRange();
                range.moveToElementText(obj);//range定位到obj
                range.collapse(false);//光标移至最后
                range.select();
            }
        },
        async SetUploadFile(content) {
        // 上传第一帧图片
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                return result.data.fullurl;
            } else {
                this.Toast.fail(result.message)
            }
        }
    },
    components: {
        sound, playaudio
    }
}
</script>

<style lang="scss" scoped>
.additional {
    min-height: 100%;
    font-size: 0.3rem;
    box-sizing: border-box;
    padding-bottom: 2rem;
    .inpbx {
        padding: 0 0.3rem;
        .ops {
            padding: 0.4rem 0;
            display: flex;
            align-items: center;
            input {
                border: none;
                flex: auto;
                &::placeholder {
                    color: #999;
                }
            }
            span {
                color: #999;
                margin: 0 0.1rem;
            }
            .ico {
                width: 0.32rem;
                height: 0.32rem;
            }
            .paytype {
                display: flex;
                align-items: center;
                .item {
                    margin-left: 0.3rem;
                    border-radius: 0.04rem;
	                border: solid 1px #2ea2fa;
                    background: #fff;
                    height: 0.54rem;
                    line-height: 0.52rem;
                    padding: 0 0.2rem;
                    box-sizing: border-box;
                    color: #2ea2fa;
                    &.sel {
                        background: #2ea2fa;
                        color: #fff;
                    }
                    &:first-of-type {
                        margin: 0;
                    }
                }
            }
            &.inpnum {
                padding: 0 0 0.16rem 0;
                border-bottom: 1px solid #eaeaea;
            }
            &.title {
                border-bottom: 1px solid #eaeaea;
                .tit {
                    margin-right: 0.6rem;
                }
            }
            &.inp {
                position: relative;
                flex-direction: column;
                align-items: center;
                textarea {
                    position: absolute;
                    top: 0.4rem;
                    box-sizing: border-box;
                    width: 100%;
                    border: none;
                    resize: none;
                    font-size: 0.28rem;
                    &::placeholder {
                        color: #999;
                    }
                }
                .placeholder {
                    position: absolute;
                    top: 0.4rem;
                    color: #999;
                }
                #showbx {
                    // position: absolute;
                    // top: 0.4rem;
                    z-index: 2;
                    width: 100%;
                    min-height: 1rem;
                    outline: none;
                }
            }
        }
        
    }
    .poi_bot {
        position: fixed;
        bottom: 0rem;

        z-index: 99;

        width: 100%;
        z-index: 9;
        .bot_f {
            padding: 0.22rem 0.3rem;
            box-sizing: border-box;
            display: flex;
            align-items: center;
            background: #fff;
            // box-shadow: 0 -0.01rem 0.1rem 0 #eaeaea;
            border-bottom: 1px solid #f7f7fc;
            border-top: 1px solid #f7f7fc;
            .ico_img {
                margin-right: 0.6rem;
                position: relative;
                img {
                    height: 0.48rem;
                }
                input {
                    position: absolute;
                    left: 0;
                    top: 0;
                    width: 100%;
                    height: 100%;
                    opacity: 0;
                }
            }
            .btn {
                flex: auto;
                display: flex;
                justify-content: flex-end;
                button {
                    width: 1.6rem;
                    height: 0.64rem;
                    border: none;
                    background-color: #2ea2fa;
                    border-radius: 0.32rem;
                    opacity: 0.5;
                    color: #fff;
                    &.clc {
                        opacity: 1;
                    }
                }
            }
        }
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .poi_bot {
        padding-bottom: env(safe-area-inset-bottom);
    }
}
</style>
