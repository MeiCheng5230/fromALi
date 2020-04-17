<template>
    <div class="botpopup" >
        <div class="content" id="scbx">
            <div class="top-bx">
                <div :class="titClass || 'title'">{{ title }}</div>
                <div class="close" @click="closeFn"><img src="@/assets/images/ic_cancel.png" alt=""></div>
            </div>
            <div class="mid-bx"><input type="text" maxlength="20" @click="setClc" @keyup="inpNum=inpNum.replace(/\D/g,'')" @focus="onFocus" v-model="inpNum" @blur="onBlur" :placeholder="placeholder"></div>
            <div class="bot-bx">
                <div class="bt-btn" @click="submit">确定</div>
            </div>
            <!-- <div class="keybd" v-show="keybd"></div> -->
        </div>
    </div>
</template>

<script>
export default {
    props: ["title"],
    data() {
        return {
            inpNum: '',                     // input框值
            titClass: "",                   // 标题文字样式 可选 'bold-tit' 与 'title'
            placeholder: "请输入账号/手机号",   // placeholder
            keybd: false,
        }
    },
    mounted() {
        if (this.title == "零售账号") {
            this.placeholder = "请输入账号/手机号";
            this.titClass = "bold-tit";
        } else if (this.title == "卡号充值") {
            this.placeholder = "请输入充值码编号";
            this.titClass = "bold-tit";
        } else if (this.title.indexOf('优谷')>0){
             this.placeholder='请输入优谷账号\\手机号';
             this.titClass = "bold-tit";
        } else if(this.title.indexOf('pcn')>0){
             this.placeholder='请输入pcn账号\\手机号';
             this.titClass = "bold-tit";
        } else if(this.title == "代理人只能更改一次上级充值商，请谨慎操作") {
            this.placeholder='请输入对方账号\\手机号';
            this.titClass = "bold-tit-sm";
        }
    },
    methods: {
        closeFn() {     // 关闭弹窗
            this.$emit("popupFlag" , false);
        },
        submit(){
            if(this.inpNum==''){
                this.Toast(this.placeholder);
                return;
            }
            this.$emit("popupSubmit",this.inpNum);
        },
        onFocus() {
            // var u = navigator.userAgent, app = navigator.appVersion;
            // var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
            // if(!isiOS) {
            //     this.keybd = true;
            //     this.$nextTick(() => {
            //         console.log(document.getElementById("app"))
            //         document.getElementById("app").scrollTop = document.getElementById("app").offsetHeight;
            //     })
            //  }
        },
        onBlur() {
            // var u = navigator.userAgent, app = navigator.appVersion;
            // var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
            // if(!isiOS) {
            //     this.keybd = false;
            //  }
        },
        setClc(e) {
            e.target.focus();
        }
    },
}
</script>

<style lang="scss" scoped>
.botpopup {
    width: 100%;
    height: 100%;
    position: fixed;
    z-index: 10;
    top: 0;
    background: rgba($color: #000000, $alpha: 0.5);
    .keybd {
        width: 100%;
        height: 5rem;
    }
    .content {
        background: #fff;
        position: absolute;
        width: 100%;
        bottom: 0;
        padding: 0.2rem 0.3rem 0.3rem 0.3rem;
        box-sizing: border-box;
        border-radius: 0.1rem 0.1rem 0 0;
        .top-bx {
            display: flex;
            .title {
                font-size: 0.28rem;
                color: #999;
                flex: auto;
            }
            .bold-tit {
                font-size: 0.32rem;
                color: #000;
                flex: auto;
                font-weight: 600;
            }
            .bold-tit-sm {
                font-size: 0.3rem;
                color: #000;
                flex: auto;
                font-weight: 600;
            }
            .close {
                padding: 0 0.15rem;
                img {
                    width: 0.4rem;
                    height: auto;
                }
            }
        }
        .mid-bx {
            padding: 0.3rem 0;
            border-bottom: 1px solid #dedede;
            input {
                width: 100%;
                border: none;
                background: #f7f7fc;
                padding: 0.2rem 0.3rem;
                box-sizing: border-box;
                &::placeholder {
                    color: #c7c7cd;
                }
            }
        }
        .bot-bx {
            padding-top: 0.6rem;
            .bt-btn {
                background: #2ea2fa;
                color: #fff;
                text-align: center;
                padding: 0.21rem 0;
                font-size: 0.32rem;
                border-radius: 0.04rem;
            }
        }
    }
}
@supports (bottom: env(safe-area-inset-bottom)) {
    .content {
       padding-bottom: env(safe-area-inset-bottom); 
    }
}
</style>
