<template>
    <div class="identify">
        <div class="tit">资格证书</div>
        <div class="remark">上传相关资格证书可加快审核</div>
        <upload :imglist="fileList" :maxnum='9' @delImg='delImg' />
        <button :class="fileList.length?'btn':'nobtn'" v-if="showbtn" @click="UpdateSpecializedPics">保存</button>
    </div>
</template>

<script>
const upload = () => import('@/components/upload')
import { UpdateSpecializedPics } from '@/api/getData';
export default {
    data() {
        return {
            fileList: [],
            showbtn: false,
        }
    },
    created() {
        var userinfo = JSON.parse(this.getStore('info'));
        if (userinfo.professionalpics) {
            this.fileList = userinfo.professionalpics;
        }
        if(userinfo.status == 0 || userinfo.status == 2) {
            this.showbtn = true;
        }
    },
    methods: {
        async UpdateSpecializedPics() {
            // if (!this.fileList.length) {
            //     return false;
            // }
            var pics = this.fileList.join(',');
            let result = await UpdateSpecializedPics({pics: pics, ...this.$global.userInfo});
            console.log(result);
            if (result.result > 0) {
                this.Toast.success(result.message);
                setTimeout(() => {
                    this.$router.go(-1);
                }, 1000)
            } else {
                this.Toast(result.message)
            }
        },
        delImg(idx) {
        // 删除图片
            this.fileList.splice(idx, 1);
        }
    },
    components: {
        upload
    }
}
</script>

<style lang="scss" scoped>
.identify {
    padding: 0.3rem;
    font-size: 0.3rem;
    .tit {
        font-weight: bold;
    }
    .remark {
        font-size: 0.24rem;
        padding: 0.3rem 0 0.6rem;
    }
    .upload {
        background: #f7f7fc;
        padding: 0.3rem;
        min-height: 3.2rem;
        position: relative;
        .imgnum {
            position: absolute;
            bottom: 0.2rem;
            right: 0.3rem;
            color: #999;
        }
    }
    .btn {
        width: 100%;
        background: #2ea2fa;
        border: none;
        border-radius: 0.04rem;
        color: #fff;
        padding: 0.25rem 0;
        margin-top: 2rem;
    }
    .nobtn {
        width: 100%;
        background: rgba($color: #2ea2fa, $alpha: 0.5);
        border: none;
        border-radius: 0.04rem;
        color: #fff;
        padding: 0.25rem 0;
        margin-top: 2rem;
    }
}
</style>
