<template>
    <div class="upload">
        <div class="img" v-for="(item, index) in imglist" :key="index">
            <img :src="item" @error.once="SetErro(item, $event)" @click="ShowBigImg(index)" alt="" />
            <div class="del" @click="SetDel(index)"><img src="@/assets/images/detele.png" alt=""></div>
        </div>
        <div class="img" v-if="showUpload">
            <input type="file" @change="SelectImage($event)" accept="image/*"/>
            <img src="@/assets/images/icon_photo.png" alt="">
        </div>
        <div class="imgnum">{{ imglist.length }}/{{ maxnum }}</div>

        <!-- 查看大图 -->
        <div class="popup" v-if="bigImg">
            <div class="top" :style="isiOS?'top: 0.5rem':'top: 0'">
                <van-icon name="arrow-left" color="#fff" size="0.32rem" @click="SetClose"/>
                <div class="imgNum">{{ idx+1 }}/{{ imglist.length }}</div>
                <div class="delImg" @click="SetDel(idx)"><img src="@/assets/images/detele@2x.png" alt=""></div>
            </div>
            <div class="showImg">
                 <v-touch v-on:swipeleft="LeftChangeImg" v-on:swiperight="RightChangeImg">
                    <img :src="imglist[idx]" @click="SetClose" alt="">
                </v-touch>
            </div>
        </div>
    </div>
</template>

<script>
import Vue from 'vue';
import VueTouch from 'vue-touch';
Vue.use(VueTouch, {name: 'v-touch'});
import { UploadFile } from '@/api/getData';
import { canvasDataURL } from '@/config/utils';
import { compressImage } from '@/config/compressImage'
export default {
    props: ['imglist', 'maxnum'],
    data() {
        return {
            showUpload: true,
            idx: 0,     // 
            bigImg: false,
            userInfo: {},
			isiOS: false,
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        this.userInfo = userinfo;
         if(this.imglist.length >= this.maxnum) {
            this.showUpload=false;
        } else {
            this.showUpload=true;
        };
		var u = navigator.userAgent;
		var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1;
		var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); 
		if(isiOS){
			this.isiOS = true;
		}
	},
    watch: {
        imglist(val) {
            if(val.length >= this.maxnum) {
                this.showUpload=false;
            } else {
                this.showUpload=true;
            }
           
            if (val.length==0) {
                this.bigImg = false;
            }
        }
    },
    beforeDestroy() {
        try {
            AppNative.blJsTunedupNativeWithTypeParamSign(1012, '', '');
        } catch (error) {
            this.Toast.fail("请在相信App中打开");
        }
    },
    methods: {
        SelectImage(e) {  
            var target = e.target;
            //检测上传文件的类型 
            // var imgName = document.all.up_file.value;
            var imgName = target.files[0].name;

            var ext,idx;   
            if (imgName == ''){  
                this.Toast("请选择需要上传的文件!");  
                return; 
            } else {   
                idx = imgName.lastIndexOf(".");   
                if (idx != -1){   
                    ext = imgName.substr(idx+1).toUpperCase();   
                    ext = ext.toLowerCase( ); 
                    if (ext != 'jpg' && ext != 'png' && ext != 'gif' && ext != 'jpeg'){  
                        this.Toast("只能上传.jpg  .png  .gif .jpeg类型的文件!"); 
                        return;  
                    }
                } else {  
                    this.Toast("只能上传.jpg  .png  .gif .jpeg类型的文件!"); 
                    return;
                }   
            }
                            
            //检测上传文件的大小        
            var isIE = /msie/i.test(navigator.userAgent) && !window.opera;  
            var fileSize = 0;           
            if (isIE && !target.file){       
                var filePath = target.value;       
                var fileSystem = new ActiveXObject("Scripting.FileSystemObject");          
                var file = fileSystem.GetFile(filePath);       
                fileSize = file.Size;  
            } else {      
                fileSize = target.files[0].size;       
            }
            var size = fileSize / 1024*1024; 
            
            if(size>(1024*1000)) {    
                var imagesize=size/1024/1024;
                var rate=0.95-((imagesize-1)/0.33)/100;
                
                // alert("文件大小不能超过2M"); 

                var file = target.files;
                var _this = this;
                if (!file || !file[0]) {
                    return;
                }
                var reader = new FileReader();
                reader.onload = function (evt) {
                    var newImg = evt.target.result;
                    compressImage(newImg, {
                        maxWidth: 1920, // 限制最大宽度
                        maxHeight: 1080, // 限制最大高度，若宽高都限制了，按原图比例最小边为主
                        // width: 100, // 压缩后图片的宽
                        // height: 200, // 压缩后图片的高，宽高若只传一个，则按图片原比例进行压缩
                        quality: rate// 压缩后图片的清晰度，取值0-1，值越小，所绘制出的图像越模糊
                    }).then(result => {
                        _this.UploadFile(result);
                    })

                }
                reader.readAsDataURL(file[0]);
                e.target.value = '';
            } else {
                this.ImgFn(target);
            }    
        },

        ImgFn(e) {
            var file = e.files;
            var _this = this;
            if (!file || !file[0]) {
                return;
            }
            var reader = new FileReader();
            reader.onload = function (evt) {
                var newImg = evt.target.result;
                _this.UploadFile(newImg)
            }
            reader.readAsDataURL(file[0]);
            e.value = '';
        },
        SetDel(idx) {
        // 删除图片
            if (idx == this.imglist.length-1) {
                this.$emit('delImg', idx);
                this.idx -= 1;
                return
            }
            this.$emit('delImg', idx);
        },
        async pushImg(src) {
            await this.imglist.push(src);
        },
        async UploadFile(content) {
        // 上传图片
            let result = await UploadFile({content: content, typeid: 'jpeg', imageactiontype: 0, ...this.$global.userInfo});
            if(result.result > 0) {
                this.pushImg(result.data.fullurl);
            } else {
                this.Toast.fail(result.message)
            }
        },
        ShowBigImg(idx) {
            try {
                AppNative.blJsTunedupNativeWithTypeParamSign(1011, '', '');
            } catch (error) {
                this.Toast.fail("请在相信App中打开");
            }
            this.idx = idx;
            this.bigImg = true;
        },
        SetClose() {
            this.bigImg = false;
            try {
                AppNative.blJsTunedupNativeWithTypeParamSign(1012, '', '');
            } catch (error) {
                this.Toast.fail("请在相信App中打开");
            }
        },
        LeftChangeImg() {
        // 向左滑动
            if(this.idx < this.imglist.length-1) {
                this.idx += 1;
            }
        },
        RightChangeImg() {
        // 向右滑动
            if(this.idx > 0) {
                this.idx -= 1;
            }
        },
        SetErro(src, e) {
            e.target.src = src;
        }
    }
}
</script>

<style lang="scss" scoped>
.upload {
    width: 100%;
    background: #f7f7fc;
    padding: 0.3rem;
    min-height: 2.5rem;
    position: relative;
    box-sizing: border-box;
    .img {
        width: 1.2rem;
        height: 1.2rem;
        display: inline-block;
        background: #fff;
        margin: 0 0.2rem 0.2rem 0;
        position: relative;
        img {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            max-width: 1.2rem;
            max-height: 1.2rem;
        }
        .del {
            position: absolute;
            top: 0.07rem;
            right: 0.15rem;
            img {
                width: 0.24rem;
                height: 0.24rem;
            }
        }
        input {
            position: absolute;
            width: 100%;
            height: 100%;
            z-index: 2;
            opacity: 0;
        }
    }
    .imgnum {
        position: absolute;
        right: 0.3rem;
        bottom: 0.2rem;
        color: #999;
    }
    .popup {
        background: rgba($color: #000000, $alpha: 1.0);
        position: fixed;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
        .top {
            display: flex;
            padding: 0.2rem;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            box-sizing: border-box;
            z-index: 9;
            .imgNum {
                flex: auto;
                color: #fff;
                text-align: center;
            }
            .delImg {
                img {
                    width: 0.32rem;
                }
            }
        }
        .showImg {
            div {
                position: fixed;
                top: 0;
                width: 100%;
                height: 100%;
                display: flex;
                align-items: center;
                justify-content: center;
                img {
                    width: 100%;
                    max-height: 100%;
                }
            }
            
        }
    }
}
</style>
