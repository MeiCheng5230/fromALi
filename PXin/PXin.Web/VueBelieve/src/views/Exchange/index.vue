<template>
  <div class="exchange">
    <div class="infoBx">
      <div class="userImg"><img :src="userInfo.imgurl" alt="头像" @error='imgError'></div>
      <div class="info">
        <div class="name">{{userInfo.nodename.length>11? userInfo.nodename.slice(0,11)+'...':userInfo.nodename}}</div>
        <div class="dos"><span>专户DOS: {{userInfo.dos}}</span></div>
      </div>
      <div class="btnBx">
        <div class="btn in" @click="transfer(0)">转入</div>
        <div class="btn out" @click="transfer(1)">转出</div>
      </div>
    </div>
    <div class="list">
      <div class="titBx">
        <div class="tit">精品兑换</div>
        <router-link to='changehis'>
          <div class="record">兑换记录
            <van-icon name="arrow"/>
          </div>
        </router-link>
      </div>
      <div class="libx">
        <div class="item" v-for="(p,index) in exchangelist" :key="p.id" @click="exchangeDetails(index)">
          <div class="Img"><img :src="p.pic" alt=""></div>
          <div class="title">{{p.name}}<span v-if="p.typeid==2">({{p.pdtvalue}}面额)</span></div>
          <div class="price">{{p.price}}{{p.priceunit}}</div>
        </div>
      </div>
    </div>
    <!-- <BotPopup /> -->
    <popup :title="popuptitle"
           v-if="popupFlag"
           :popFlag="popupFlag"
           @popupFlag="popupFn" @popupSubmit='popupSubmit'/>
  </div>
</template>

<script>
    import { GetUeUserInfo } from '@/api/getData';
    import { Dialog, Divider } from "vant";
    const popup = () => import("@/components/BotPopup");
    import {GetExChangeDosInfo, GetExChangeList, GetPCNUserByNodeCode} from '@/api/getData';
    import {setStore, getStore} from "@/config/utils";

    export default {
        data() {
            return {
                userInfo: {
                    imgurl: 'http://images2.xiang-xin.net/userphoto/noempty.png',
                    nodename: '',
                },
                exchangelist: [],
                popupFlag: false,  // 弹出框显示
                popuptitle: '',
            }
        },
        mounted() {
            this.getData();
            this.getexchange();
        },
        components: {
            popup,
            [Dialog.Component.name]: Dialog.Component
        },
        methods: {
            popupFn(data) {     // 接受子传值，关闭弹窗
                this.popupFlag = data;
            },
            imgError() {
                this.userInfo.imgurl = 'http://images2.xiang-xin.net/userphoto/noempty.png';
            },
            async getData() {
                let result = await GetExChangeDosInfo(JSON.parse(sessionStorage.userParam));
                if (result.result > 0) {
                    this.userInfo = result.data;
                    if (result.data.isopeninfo != 1) {
                        setStore("openInfo", JSON.stringify({
                            nodecode: this.userInfo.nodecode,
                            nodename: this.userInfo.nodename,
                            callbackurl: -1
                        }));
                        this.$router.push({name: 'Opening'});
                    }
                } else {
                    this.Toast(result.message);
                }
            },
            async getexchange() {
                let result = await GetExChangeList(JSON.parse(sessionStorage.userParam));
                if (result.result > 0) {
                    this.exchangelist = result.data;
                } else {
                    this.Toast(result.message);
                }
            },
            //兑换详情
            exchangeDetails(index) {
                let details = this.exchangelist[index];
                let json = JSON.stringify(details);
                setStore("exchangeDetails", json);
                switch (details.typeid) {
                    case 1:
                        setStore("nextTitle", "SV兑换");
                        this.$router.push({path: '/SVCtransfer'});
                        break;
                    case 2:
                        setStore("nextTitle", "SVC兑换");
                        this.$router.push({path: '/SVCtransfer'});
                        break;
                    case 3:
                        this.popuptitle = '请输入要兑换的优谷账号\\手机号';
                        this.popupFlag = true;
                        setStore("nextTitle", "优谷VIP兑换");
                        break;
                    case 4:
                        this.popuptitle = '请输入要兑换的pcn账号\\手机号';
                        this.popupFlag = true;
                        setStore("nextTitle", "PCN认证码兑换");
                        break;
                    default:
                        this.Toast("选择错误");
                        break;
                }

            },
            //输入账号组件确定按钮回调
            async popupSubmit(nodeCode) {
                let json = getStore("exchangeDetails");
                if (json == null) {
                    this.Toast("获取详情失败");
                    return;
                }
                var data = JSON.parse(json);
                var typeid = data.typeid == 3 ? 5 : 4;
                let result = await GetPCNUserByNodeCode(JSON.parse(sessionStorage.userParam), nodeCode, typeid);
                if (result.result > 0) {
                    setStore("exchangeYGorPcnUserInfo", JSON.stringify(result.data));
                    this.$router.push({name: 'YGtransfer'});
                } else {
                    this.Toast(result.message);
                }
            },
            // 查询绑定状态
            async GetUeUserInfo(typeid){
              let res = await GetUeUserInfo(JSON.parse(sessionStorage.userParam),0);
              if( res.result <= 0 ){
                Dialog.confirm({
                  title: "提示",
                  message: "<p> 未绑定码库账号,将影响您使用相关功能,现在去绑定</p>",
                  confirmButtonText: "立即绑定",
                  cancelButtonText: "取消"
                })
                  .then(() => {
                    this.$router.push('/BindAccount');
                  })
                  .catch(() => {
                    // on cancel
                  });
              }else{
                if(typeid==0){
                  this.$router.push('/transferInto');
                }else{
                  this.$router.push({name: 'transferOut'});
                }
              }
              
            },
            //转入转出
            transfer(typeid) {
                setStore("exchangeUserDos", this.userInfo.dos);
                this.GetUeUserInfo(typeid);
            }
        }
    }
</script>

<style lang="scss" scoped>
  .exchange {
    font-size: 0.28rem;

    .infoBx {
      box-shadow: 1px 2px 6px 0px #ececec, -1px -2px 6px 0px #ececec;
      border-radius: 0.12rem;
      display: flex;
      align-items: center;
      margin: 0.3rem;
      padding: 0.3rem;
      box-sizing: border-box;
      height: 1.8rem;

      .userImg {
        margin-right: 0.34rem;

        img {
          width: 0.96rem;
          height: 0.96rem;
          border-radius: 50%;
        }

      }

      .info {
        flex: auto;

        .dos {
          margin-top: 0.16rem;
          display: flex;
          align-items: center;
        }

      }

      .btnBx {
        .btn {
          width: 1.2rem;
          text-align: center;
          color: #fff;
          padding: 0.1rem 0;
          border-radius: 0.04rem;

          &.in {
            background: #2ea2fa;
            margin-bottom: 0.2rem;
          }

          &.out {
            background: #ff9000;
          }

        }
      }
    }

    .list {
      padding: 0 0.3rem;

      .titBx {
        display: flex;
        padding: 0.3rem 0;

        .tit {
          flex: auto;
          font-size: 0.36rem;
          font-weight: 600;

          &::before {
            content: "";
            border: 3px solid #ff9000;
            border-bottom: none;
            border-top: none;
            border-radius: 6px;
            margin-right: 0.12rem;
          }

        }

        .record {
          display: flex;
          align-items: center;
          font-size: 0.28rem;
        }

      }

      .libx {
        .item {
          width: 3.3rem;
          display: inline-block;
          margin-bottom: 0.3rem;

          &:nth-child(odd) {
            margin-right: 0.2rem;
          }

          .Img {
            height: 2rem;

            img {
              width: 3.3rem;
              height: auto;
            }

          }

          .title {
            span {
              font-size: 0.24rem;
            }

          }

          .price {
            color: #ff9000;
          }

        }
      }
    }
  }
</style>
