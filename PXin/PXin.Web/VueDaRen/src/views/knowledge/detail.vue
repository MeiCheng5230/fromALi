<template>
    <div class="detail">
        <div class="title" :contentEditable="editShow">
            <div class="tit" v-show="editShow">主题</div>
            <input type="text" v-model="title" :readonly="!editShow"/>
        </div>
        <div class="info">
            <div class="Img"><img :src="photo" alt="" /></div>
            <div class="name">{{ name }}</div>
            <div class="num">{{ num }}人已看过</div>
        </div>
        <div class="paynum" v-show="editShow">
            <div class="ops">
                <div class="tit">查看需支付</div>
                <span>(需要先选择支付类型)</span>
                <img class="ico" @click="SetTit" src="@/assets/images/icon_hint.png" alt="">
            </div>
            <div class="ops inpnum">
                <input type="text" v-model="price" @input="price=price.replace(/[^\d]/g,'')" placeholder="选择支付类型后在此处输入数量" />
                <div class="paytype">
                    <div v-for="(item, index) in payTypeList" :key="index" :class="item.select?'item sel':'item'" @click="setPayType(item)">
                        {{ item.tit }}
                    </div>
                </div>
            </div>
        </div>
        
        <div class="v_playvideo" style="margin-top: 0.3rem;">
            <playaudio v-for="(item, index) in audioList" :key="index" flag="detail" :sound="item" :showDelIco="showDelIco" bg="#f7f7fc" />
        </div>
        <div class="content" id="content" :contentEditable="editShow" v-html="content"></div>

        <div class="edit" v-show="!editShow && ismine==1">
            <div class="Img" @click="SetEdit"><img src="@/assets/images/icon_bianji.png" alt="" /></div>
            <!-- <div class="Img" @click="setDelet"><img src="@/assets/images/icon_delete.png" alt="" /></div> -->
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
                    <button @click="SetUpdate">更新</button>
                </div>
            </div>
            <sound v-if="showSound" @showSound="SetShowSound" :show="showSound" @audioSrc="GetAudioSrc" />
        </div>
    </div>
</template>

<script>
import { Dialog } from 'vant';
const sound = () => import('@/components/sound');
const playaudio = () => import('@/components/audio');
import { UploadFile, DeleteFile, CreateOrUpdateDaRenKnowledge, GetDaRenKnowledgeDetail, DeleteDaRenKnowledge } from '@/api/getData';
export default {
    data() {
        return {
            payTypeList: [{tit: 'V点', select: true}, {tit: 'UV'}],
            payetype: 0, // 支付类型 0=V点 1=UV ,
            editShow: false,    // 编辑
            showSound: false,   // 录音组件
            showBot: false,  // 底部上传组件
            photo: '',  // 用户头像
            content: '',    // html内容
            name: '',   // 作者姓名
            num: '',    // 浏览次数
            title: '',  // 主题
            price: '',  // 价格
            id: '',
            audioList: [],  // 录音
            showDelIco: true,  // 显示audio组件删除按钮
            ismine: 1,    // 是否通过自己账号进来
        }
    },
    created() {
        this.ismine = this.$route.query.ismine || 1;
        this.GetDaRenKnowledgeDetail(this.$route.query.id);
        const _this = this;
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
        window.nativeSelecMediaCompletion = function(type, file, img) {
        // APP图片视频回调
            if (type == 0) {
                _this.UploadImg(file);
            } else {
                _this.SetVideo(file, img);
            }
        };
        window.DelItem = function(src, dom) {
        // 判断时候为音频，删除
            if (_this.audioList.indexOf(src) < 0) {
                dom.remove();
                return;
            }
            _this.audioList.splice(_this.audioList.indexOf(src), 1);
        };
    },
    watch: {
        showSound(val) {
            let bot = this.$el.querySelector(".poi_bot");
            if (val) {
                bot.style.bottom = "5.2rem"
            } else {
                bot.style.bottom = 0;
            }
        },
    },
    methods: {
        AppUpload(i) {
        // 调用APP相册，视频（单个）
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
        async SetVideo(video, img) {
        // 上传视频文件
            this.showSound = false;
            let text = document.querySelector("#content");
            let VideoSrc = await this.UploadVideo(video);
            let imgSrc = await this.SetUploadFile(img);

            let newHtml = "<div style='position: relative; width: 100%; height: 4rem; background: #000; display: flex; justify-content: center;' class='item video_div' contentEditable='false'>";
            newHtml += "<img style='position: absolute; width: 0.36rem; right: 0; top: 0; z-index: 2;' class='del_Ico' src='static/img/delete.png' onclick='DelFile(this)'>";
            newHtml += "<img data-videosrc='"+VideoSrc+"' style='position: absolute; top: 50%; left: 50%; width: 0.8rem; transform: translate(-50%, -50%)' src='static/img/video_pause.png' onclick='SetPlayVideo(this)' class='VideoCoverImg'>";
            newHtml += "<img data-name='img' src='"+imgSrc+"' style='max-width: 100%; height: 100%;' class='cover'>";
            newHtml += "</div><br>";
            text.innerHTML += newHtml;
            this.JudgeText();
        },
        async UploadImg(content) {
        // 上传图片
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            console.log(result);
            if(result.result > 0) {
                var text = document.querySelector("#content");
                text.innerHTML += "<div style='position: relative;' contenteditable='false' class='item img_div'><img onclick='DelFile(this)' style='width: 0.36rem; position: absolute; right: 0; top: 0;' class='del_Ico' src='static/img/delete.png'/><img data-name='img' src='"+ result.data.fullurl +"' style='width: 100%;' class='Img' /></div><br>";
                this.keepLastIndex(text);
            } else {
                this.Toast.fail(result.message);
            }
        },
        async UploadVideo(content) {
        // 上传视频
            let result = await UploadFile({content: content, typeid: 'mp4', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                return result.data.fullurl;
            } else {
                this.Toast.fail(result.message);
            }
        },
        SetEdit() {
        // 点击编辑
            this.showBot = true;
            this.editShow = true;
            this.SetShowDel();
            this.showDelIco = false;
        },
        GetAudioSrc(data) {
        // 添加录音组件
            this.audioList.push(data);
            this.showSound = false;
        },
        async SetUpdate() {
        // 更新接口
            let delIco = document.querySelector("#content").querySelectorAll(".del_Ico");
            for (const itm of delIco) {
                itm.style.display = 'none';
            };
            let content = document.querySelector("#content").innerHTML;
            let urlstr;
            if (this.audioList.length) {
                urlstr = this.audioList.join(',');
            } else {
                urlstr = '';
            };

            if (!this.title) {
                this.Toast("请输入您的主题！");
                return;
            };
            if (!this.price) {
                this.Toast("请输入您的价格！");
                return;
            };
            let html = document.querySelector("#content").innerHTML;
            if (html.length+this.audioList.length<= 0) {
                this.Toast("请输入您的回答内容！");
                return;
            };
            if (this.audioList.length>3) {
                this.Toast("您的语音不能超过三条！");
                return;
            };
            let videoLen = document.querySelector("#content").querySelectorAll(".video_div").length;
            if(videoLen > 3) {
                this.Toast("您上传视频不能超过三条！");
                return;
            };
            let imgLen = document.querySelector("#content").querySelectorAll(".img_div").length;
            if(imgLen > 9) {
                this.Toast("您上传的图片不能超过九张！");
                return;
            };

            Dialog.confirm({
                title: '温馨提示',
                message: '现在要更新该知识库内容吗？',
                cancelButtonText: '存为草稿',
                confirmButtonText: '更新',
                cancelButtonColor: '#999'
            }).then(() => {
                this.CreateOrUpdateDaRenKnowledge(1, urlstr, content);
            }).catch(() => {
                this.CreateOrUpdateDaRenKnowledge(0, urlstr, content);
            })
        },
        setDelet() {
        // 删除知识库/草稿
            Dialog.confirm({
                title: '温馨提示',
                message: '<p style="text-align: left; color: #999; padding: 0 0.3rem;">你确定要删除该条知识库，删除后无法撤回。</p>',
                confirmButtonText: '确定'
            }).then(async() => {
                let result = await DeleteDaRenKnowledge({...this.$global.userInfo, id: this.$route.query.id});
                if (result.result > 0) {
                    this.$router.go(-1);
                } else {
                    this.Toast.fail(result.message);
                }
            })
        },
        SetTit() {
            Dialog.alert({
                title: '温馨提示',
                message: '<p style="text-align: left;">支付方式为V点，您和查看者可增加对应数量的P点</p><p style="text-align: left;">支付方式为UV，你可增加设置金额80%的UV，查看者可增加10倍的设置金额的P点</p>',
                confirmButtonText: '知道了'
            })
        },
        setPayType(data) {
            for (const item of this.payTypeList) {
                this.$set(item, 'select', false);
            }
            this.$set(data, 'select', true);
            if(data.tit == 'V点') {
                this.paytype = 0;
                this.SetDialog('支付方式为V点，您和查看者可增加对应数量的P点');
            } else if(data.tit == 'UV') {
                this.paytype = 1;
                this.SetDialog('支付方式为UV，你可增加设置金额80%的UV，查看者可增加10倍的设置金额的P点');
            }
        },
        SetDialog(txt) {
            Dialog.confirm({
                title: '温馨提示',
                message: '<p style="text-align: left;">'+ txt +'</p>',
                confirmButtonText: '确定'
            })
        },
        async GetDaRenKnowledgeDetail(id) {
        // 获取信息
            let result = await GetDaRenKnowledgeDetail({...this.$global.userInfo, id});
            if (result.result > 0) {
                let { appphoto, content, name, num, price, title, id, voice } = result.data;
                this.photo = appphoto;
                this.content = content;
                this.name = name;
                this.num = num;
                this.price = price;
                this.title = title;
                this.id = id;
                this.voice = voice;
                if (!voice) {
                    this.audioList = [];
                    return;
                }
                this.audioList = voice.split(',');
            } else {
                this.Toast.fail(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            }
        },

        SetShowDel() {
        // 显示删除图标
            let delIco = document.querySelector("#content").querySelectorAll(".del_Ico");
            for (const item of delIco) {
                item.style.display = 'block';
            }
        },
        async DeleteFile(src, dom) {
        // 删除文件（图片，视频，录音）
            let result = await DeleteFile({...this.$global.userInfo, pics: src});
            console.log(result);
            if(result.result > 0) {
                dom.remove();
            } else {
                this.Toast(result.message);
            }
        },
        SetShowSound(data) {
        // 显示隐藏录音组件
            this.showSound = data;
        },
        async SetUploadFile(content) {
        // 上传第一帧图片
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                return result.data.fullurl;
            } else {
                this.Toast.fail(result.message)
            }
        },
        async CreateOrUpdateDaRenKnowledge(status, urlstr, content) {
        // 发布
            let result = await CreateOrUpdateDaRenKnowledge({
                ...this.$global.userInfo,
                id: this.id,
                title: this.title,
                paytype: this.paytype,
                price: this.price,
                content: content,
                status: status,
                voiceurl: urlstr
            });
            if (result.result > 0) {
                this.Toast.success(result.message);
                this.editShow = false;
                this.showBot = false;
                this.showDelIco = true;
                setTimeout(() => {
                    this.$router.go(-1);
                }, 500)
            } else {
                this.Toast.fail(result.message);
            }
        }
    },
    components: {
        sound, playaudio
    }
}
</script>

<style lang="scss" scoped>
video {
    width: 100%;
}
.detail {
    min-height: 100%;
    padding: 0.3rem;
    padding-bottom: 1.5rem;
    box-sizing: border-box;
    .content {
        margin-top: 0.3rem;
        min-height: 2rem;
        padding: 0.2rem;
        outline: none;
    }
    .title {
        font-size: 0.3rem;
        font-weight: bold;
        line-height: 0.36rem;
        display: flex;
        align-items: center;
        .tit {
            margin-right: 0.1rem;
        }
        input {
            flex: auto;
            border: none;
        }
    }
    .info {
        display: flex;
        align-items: center;
        margin: 0.3rem 0;
        color: #999;
        .Img {
            width: 0.6rem;
	        height: 0.6rem;
            border-radius: 50%;
            overflow: hidden;
            img {
                width: 100%;
                height: 100%;
            }
        }
        .name {
            font-size: 0.3rem;
            margin-left: 0.17rem;
        }
        .num {
            flex: auto;
            text-align: right;
            font-size: 0.24rem;
        }
    }
    .edit {
        position: fixed;
        right: 0.3rem;
        bottom: 1rem;
        .Img {
            width: 1rem;
            height: 1rem;
            margin: 0.1rem 0;
            img {
                width: 100%;
                height: 100%;
            }
        }
    }
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
            #showbx {
                width: 100%;
                min-height: 1rem;
                outline: none;
            }
        }
    }
    .poi_bot {
        position: fixed;
        bottom: 0;
        left: 0;
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
                    opacity: 1;
                    color: #fff;
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
