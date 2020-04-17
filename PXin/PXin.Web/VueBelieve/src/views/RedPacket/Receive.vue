<template>
  <div class="receive">
    <!--<div @click="!data.iscompletetask?$router.push({ path:'/buycode'}):''" class="topbx">-->
    <div class="topbx">
      <div class="text">上月任务：{{data.iscompletetask1?"已完成":"未完成"}}<span style="margin-left:50px"></span>本月任务：{{data.iscompletetask?"已完成":"未完成"}}</div>
      <img src="@/assets/images/red_packet_bg_top.png" alt="" />
      <router-link class="toexplain" to="/explain" tag="div">活动说明</router-link>
    </div>
    <div class="content" :style="{backgroundImage: 'url('+require('@/assets/images/red_packet_explain_bg_01.png')+')'}">
      <div class="receivebtn" @click="receiveFn" v-if="data.status==0">领取红包</div>
      <div class="receivebtn hasreceive" v-else-if="data.status==1" @click="toaFn">今日红包已领取</div>
      <div class="receivebtn hasreceive" v-else>本月无权限领取</div>
      <div class="txt">本月购买充值码(SVC)，次月方可参与活动哦！</div>
      <div class="auction">
        <div class="lft">拥有A点  {{data.adain}}个</div>
        <router-link to="/auction" tag="button">去竞拍</router-link>
      </div>
      <div class="reward" :style="{backgroundImage: 'url('+require('@/assets/images/red_packet_bg_award.png')+')'}">
        <div class="numBx">
          <div class="opt">
            <div class="tt" style="margin-top: 0.05rem;">SV</div>
            <div class="nn">{{data.sv}}</div>
          </div>
          <div class="opt">
            <div class="tt">充值码(SVC)</div>
            <div class="nn">{{data.svc}}</div>
          </div>
          <div class="opt">
            <div class="tt">专户DOS</div>
            <div class="nn">{{data.dos}}</div>
          </div>
        </div>
        <router-link to="/rewardlist" tag="div" class="todetail">我的红包明细 >></router-link>
      </div>
      <div class="activity">
        <img src="@/assets/images/red_packet_bg_strategy.png" alt="">
      </div>
    </div>
    <div class="popup" v-if="receiveFlag">
      <div class="con">
        <div class="receiveBx" :style="{backgroundImage: 'url('+require('@/assets/images/red_packet_pop_up_bg.png')+')'}">
          <div class="tit">恭喜你获得</div>
          <span>{{redpacket.sv}}SV</span>
          <span>{{redpacket.svc}}SVC</span>
          <span>{{redpacket.dos}}专户DOS</span>
          <button class="clsBtn" @click="receiveFlag = false;">知道了</button>
        </div>
        <div class="close" @click="receiveFlag = false;">
          <img src="@/assets/images/red_packet_pop_up_delete.png" alt="">
        </div>
      </div>

    </div>
  </div>
</template>

<script>
  import { GetRedPacketInfo, ReceiveRedPacket } from '@/api/getData'
  import { setStore } from "@/config/utils";
  import { Dialog } from 'vant';
import axios from 'axios';
export default {
    data () {
        return {
            receiveFlag: false,
          hasreceive: false,
          data: {},
          redpacket: {},
        }
    },
    beforeCreate() {
      let _this = this;
        axios.post('/api/Redpacket/GetRedPacketInfo',JSON.parse(sessionStorage.userParam)).then((res)=>{
           if(res.data.result>0){
              if (res.data.data.isopen == 0) {
                setStore("openInfo", JSON.stringify({
                  nodename: res.data.data.name,
                  nodecode: res.data.data.nodecode,
                  callbackurl: -1
                }))
                _this.$router.push('/Opening');
              }
              _this.data = res.data.data;
              }else{
                  _this.Toast.fail(res.data.data.message);
              }
            }).catch((err)=>{
               _this.Toast.fail('网络繁忙');
            });
    },
    mounted() {
      
    },
    methods: {
      async ReceiveRedPacket(infoid) {
        let result = await ReceiveRedPacket(JSON.parse(sessionStorage.userParam),infoid);
        if (result.result > 0) {
          this.redpacket = result.data;
          this.data.status = 1;
          this.data.sv += result.data.sv;
          this.data.svc += result.data.svc;
          this.data.dos += result.data.dos;
          this.receiveFlag = true;
        } else {
          this.Toast(result.message);
        }
      },
      async GetRedPacketInfo() {
        let result = await GetRedPacketInfo(JSON.parse(sessionStorage.userParam));
        if (result.result > 0) {
          if (result.data.isopen == 0) {
            setStore("openInfo", JSON.stringify({
              nodename: result.data.name,
              nodecode: result.data.nodecode,
              callbackurl: -1
            }))
            this.$router.push('/Opening');
          }
          this.data = result.data;
        } else {
          this.Toast(result.message);
        }
      },
      setdata: function () {
        this.GetRedPacketInfo();

      },
      receiveFn: function () {
        if (this.data.status != 0) {
          this.Toast(this.data.statusdesc);
        } else {
          this.ReceiveRedPacket(this.data.infoid);
        }
      },
      toaFn() {
        Dialog.alert({
          message: '今日已领取，明天再来吧！',
          confirmButtonText: '知道了'
        })
      }
    }
}
</script>

<style lang="scss" scoped>
.receive {
    .topbx {
        position: relative;
        .text {
            position: absolute;;
            width: 100%;
            background-color: rgba(255, 255, 255, 0.4);
            color: #fff;
            font-size: 0.28rem;
            padding: 0.2rem 0.3rem;
            box-sizing: border-box;
        }
        .toexplain {
          background: #ffe59a;
          position: absolute;
          text-align: center;
          top: 1rem;
          right: 0;
          /* writing-mode:vertical-rl; */
          width: 0.47rem;
          text-align: center;
          font-size: 0.24rem;
          padding: 0.1rem 0;
          color: #c4422b;
        }
        img {
            display: block;
            width: 100%;
            height: auto;
        }
    }
    .content {
        background-size: 100% auto;
        .receivebtn {
            margin: 0 0.9rem;
            text-align: center;
            font-weight: bold;
            color: #ff443f;
            background-image: linear-gradient(180deg,
                        #e96072 0%,
                        #9e8aa1 0%,
                        #53b3d0 0%,
                        #a9d6c5 0%,
                        #fff8b9 0%,
                        #edd03d 100%),
                    linear-gradient(
                        #f6ff94,
                        #f6ff94);
            padding: 0.26rem 0;
            border-radius: 0.44rem;
            position: relative;
            top: -0.22rem;
            &.hasreceive {
                background: #e5e5e5;
            }
        }
        .txt {
            color: #fff;
            font-size: 0.24rem;
            text-align: center;
        }
        .auction {
            background-image: linear-gradient(0deg,
                #ffedcd 0%,
                #ffffff 100%);
            border-radius: 0.04rem;
            display: flex;
            align-items: center;
            margin: 0.3rem;
            padding: 0.26rem 0.3rem;
            .lft {
                flex: auto;
                color: #d1001a;
            }
            button {
                padding: 0.2rem 0.4rem;
                border: none;
                border-radius: 0.04rem;
                background-image: linear-gradient(0deg,
                    #b80011 0%,
                    #dd363b 100%);
                color: #ffd493;
            }
        }
        .reward {
            margin: 0.5rem;
            background-size: 100% 100%;
            padding: 1.5rem 0.4rem 0.6rem 0.4rem;
            text-align: center;
            .numBx {
                display: flex;
                .opt {
                    flex: 1;
                    border-right: 1px solid #eaad64;
                    .tt {
                        margin-bottom: 0.4rem;
                        font-size: 0.28rem;
                    }
                    .nn {
                        color: #e51c27;
                        font-size: 0.36rem;
                        width: 1.6rem;
                        overflow: hidden;
                        text-overflow: ellipsis;
                        white-space: nowrap;
                        margin: 0 auto;
                    }
                    &:last-of-type {
                        border: none;
                    }
                }
            }
            .todetail {
                margin-top: 0.47rem;
                font-size: 0.24rem;
                color: #e51c27;
                font-weight: bold;
            }
        }
        .activity {
            padding: 0 0.5rem 0.5rem 0.5rem;
            img {
                width: 100%;
                height: auto;
            }
        }
    }
    .popup {
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        .con {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            .receiveBx {
                width: 5.28rem;
                height: 5.6rem;
                background-size: 100% 100%;
                display: flex;
                flex-direction: column;
                align-items: center;
                position: relative;
                .tit {
                    color: #f6a830;
                    padding: 1.32rem 0 0.33rem;
                }
                span {
                    line-height: 0.48rem;
                }
                .clsBtn {
                  position: absolute;
                  bottom: 0.3rem;
                  width: 3rem;
                  height: 1rem;
                  opacity: 0;
                }
            }
        }
        .close {
            position: absolute;
            bottom: -1.2rem;
            left: 50%;
            transform: translateX(-50%);
            img {
                width: 0.6rem;
                height: 0.6rem;
            }
        }
    }
}
</style>
