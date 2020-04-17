<template>
  <div class="container">
    <div class="lucky-wheel">
      <!--头部-->
      <div class="lucky-title"></div>
      <!--剩余次数-->
      <div class="lucky-number">
        <div class="ticket"> 剩余次数：<span>{{winningTicket}}</span></div>
        <div class="money">A点数量：<span>{{AMoney}}</span></div>
      </div>
      <!--抽奖转盘-->
      <div class="lottery">
        <div class="lottery-main" :style="{transform:endRotateAngle,transition:rotateTransition}">
          <div class="lottery-items"></div>
        </div>
        <div class="lottery-pointer" @click="startRotating()"></div>
      </div>
      <div class="order">
        <div class="view_order" @click="goHisPage()">查看抽奖历史</div>
      </div>
    </div>
    <!--规则说明-->
    <div class="main">
      <div class="tip">
        <div class="tip-title">活动规则</div>
        <div class="tip-content">
          <p>1.每月最后1天结束竞拍后，前100名出价可参与A点抽奖；</p>
          <p>2.A点抽奖时间为每月最后1天的14:00到24:00，过期不参与的无法再进行抽奖；</p>
          <p>3.每次抽奖所中A点为随机，也可能不中奖，中奖的A点次月1号生效；</p>
        </div>
      </div>
    </div>
    <!--抽奖结果提示框-->
    <div class="toast" v-show="isWinningResult">
      <div class="toast-container">
        <div class="toast-title">
          {{isWinning ? "恭喜您" : "很遗憾"}}
        </div>
        <div class="toast_con">
          {{isWinning ? "获得" + WinningList[winningIdx].name + " 已放入您的账户" : "一不小心红包擦肩而过"}}
        </div>
        <div class="toast-btn">
          <div class="toast-cancel" @click="initRotate">确定</div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
    import {Toast, Indicator} from 'mint-ui';
    import {setTimeout} from 'timers';
    import utils from "@/utils";
    import axios from 'axios'

    export default {
        data() {
            return {
                //（可调）默认中奖
                winningIdx: 0,
                //（可调）旋转多少秒
                rotateTime: 6,
                //(可调)控制转盘旋转几圈
                rotateNum: 5,
                //(可调)控制转盘旋转到对应奖项所需要的度数 第一个度数计算为360/6/2  其他均为前一个数值减去360/6  6为转盘抽奖模块数量
                rotateAngleArr: [330, 270, 210, 150, 90, 30],
                //初始转旋转的角度
                startRotateAngle: 0,
                //最终需要旋转到的角度
                endRotateAngle: 0,
                //旋转过动画属性控制
                rotateTransition: 0,
                //控制结果弹窗展示
                isWinningResult: false,
                //控制抽奖按钮
                isWinningBtn:true,
                //A点余额
                AMoney:0,
                //剩余抽奖次数
                winningTicket:0,
                //是否中奖
                isWinning:false,
                //中奖列表
                WinningList: [
                    {
                        count: 0,// 奖品数量
                        name: "谢谢参与",// 奖品名称
                        isPrize: false // 该奖项是否为奖品
                    },
                    {
                        count: 1,
                        name: "1A点",
                        isPrize: true
                    },
                    {
                        count: 2,
                        name: "2A点",
                        isPrize: true
                    },
                    {
                        count: 3,
                        name: "3A点",
                        isPrize: true
                    },
                    {
                        count: 5,
                        name: "5A点",
                        isPrize: true
                    },
                    {
                        count: 8,
                        name: "8A点",
                        isPrize: true
                    }
                ],

            };
        },
        created() {
            this.getUserInfoAjax();
        },
        methods: {
            //获取用户信息
            getUserInfoAjax(){
                Indicator.open();
                let that = this;
                axios.post('/api/Redpacket/GetLuckDrawInfo', utils.userInfo)
                    .then(function (resp) {
                        resp = resp.data;
                        Indicator.close();
                        if (resp.data.result <= 0) {
                            Toast({message: resp.message, iconClass: 'icon icon-error',});
                            return false;
                        }
                        that.winningTicket = resp.data.num;
                        that.AMoney = resp.data.a;
                        //抽奖次数用完
                        if (that.winningTicket == 0) {
                            Toast({message: resp.message, iconClass: 'icon icon-error'});
                            return false;
                        }
                    })
                    .catch(function (err) {
                        Indicator.close();
                        Toast({message: '失败-' + err, iconClass: 'icon icon-error'});
                    });
            },
            //点击开始抽奖
            startRotating() {
                if(this.winningTicket<1){
                    Toast({message: "抽奖次数已用完", iconClass: 'icon icon-error'});
                    return;
                }
                if (!this.isWinningBtn) return;
                this.isWinningBtn=false;
                this.getWinningAjax();
            },
            //请求中奖结果ajax
            getWinningAjax(){
                Indicator.open();
                let that = this;
                axios.post('/api/Redpacket/LuckDraw', utils.userInfo)
                    .then(function (resp) {
                        resp = resp.data;
                        Indicator.close();
                        if (resp.result <= 0) {
                            Toast({message: resp.message, iconClass: 'icon icon-error'});
                            return false;
                        }
                        that.getWinningResult(resp.data.amount)
                    })
                    .catch(function (err) {
                        that.click_flag = true;
                        Indicator.close();
                        Toast({message: '失败-' + err, iconClass: 'icon icon-error'});
                    });
            },
            //中奖结果
            getWinningResult(id) {
                let that=this;
                for(let i in that.WinningList){
                    if(that.WinningList[i].count==id){
                        that.winningIdx=i;
                        break;
                    }
                }
                this.setRotating();
            },
            //设置旋转
            setRotating(){
                this.rotateTransition = 'transform ' + this.rotateTime + 's ease-in-out';
                this.startRotateAngle = this.startRotateAngle + this.rotateNum * 360 + this.rotateAngleArr[this.winningIdx] - this.startRotateAngle / 360;
                this.endRotateAngle = "rotate(" + this.startRotateAngle + "deg)";
                this.winningTicket = --this.winningTicket;
                this.ShowResultAlert();
            },
            //弹出抽奖结果窗口
            ShowResultAlert() {
                setTimeout(() => {
                    this.isWinningResult = true;
                    this.isWinning=this.WinningList[this.winningIdx].isPrize;
                    this.AMoney=this.AMoney+this.WinningList[this.winningIdx].count;
                }, this.rotateTime * 1000 + 300);
            },
            //关闭中将结果弹窗初始化转盘
            initRotate() {
                this.isWinningBtn = true;
                this.isWinningResult=false;
                this.rotateTransition = 'transform 0s ease-in-out';
                this.startRotateAngle = 0;
                this.endRotateAngle = 'rotate(0deg)';
            },
            // 查看历史
            goHisPage() {
                this.$router.push('/record');
            },

        }
    };
</script>
<style scoped>
  .download {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, .5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 199;
  }

  .down-body {
    background-color: #fff;
    padding: 1rem;
    border-radius: 5px;
  }

  .download-title {
    font-size: 0.72rem;
    padding: 0.4rem 0;
    text-align: center;
  }

  .download-content {
    text-align: center;
  }

  .download-content img {
    margin: 1rem 0;
  }

  .download-content p {
    font-size: 0.56rem;
  }

  .download-footer {
    padding: 1rem;
    text-align: center;
  }

  .download-footer button {
    outline: none;
    background-color: #fff;
    color: #999;
    border: 1px solid #999;
    font-size: 0.72rem;
    border-radius: 5px;
    padding: 0.4rem 1rem;
  }

  .download-footer a {
    display: inline-block;
    padding: 0.4rem 0rem;
    background-color: #ffc700;
    color: #fff;
    border: 1px solid #ffc700;
    font-size: 0.72rem;
    border-radius: 5px;
    width: 70px;
  }

  .container {
	min-height: 100%;
    padding-top: 1rem;
    padding-bottom: 1rem;
    background: url("../assets/img/bg.png") no-repeat center bottom;
    -webkit-background-size: 100% 100%;
    background-size: 100% 100%;
  }

  .font-10 {
    font-size: 10px;
  }

  .lucky-title {
    width: 17.03rem;
    height: 4.125rem;
    margin: 0 auto;
    background: url("../assets/img/word.png") no-repeat center top;
    background-size: 100%;
  }
  .lottery {
    text-align: center;
    position: relative;
  }
  .lottery .lottery-main {
    display: flex;
    justify-content: center;
  }

  .lottery .lottery-main .lottery-items {
    width: 15rem;
    height: 15rem;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    background-image: url('../assets/img/award_bg.png');
    background-size: 100%;
    background-repeat: no-repeat;
    position: relative;
    transition: transform 3s ease;


  }

  .lottery .lottery-pointer {
    width: 4rem;
    height: 5rem;
    position: absolute;
    left: 50%;
    top: 50%;
    margin-top: -2.5rem;
    margin-left: -2rem;
    background: url("../assets/img/award_pointer.png") no-repeat;
    background-size: 100%;
  }


  .lucky-number {
    display: -webkit-flex;
    display: flex;
    display: -webkit-box;
    justify-content: space-between;
    -webkit-justify-content: space-between;
    -webkit-box-pack: justify;
    padding: 1rem 1.8rem;
    color: #ff6500;
    font-size: 14px;
  }

  .main {
    position: relative;
    width: 100%;
    min-height: 14.25rem;
    padding-bottom: 1.6875rem;
  }

  .content div {
    text-align: left;
  }

  .tip {
    padding: 1rem 1.8rem;
    font-size: 15px;
    color: #333;
  }

  .order {
    text-align: center;
    margin-top: 1.38rem;
  }

  .order a {
    display: inline-block;
    padding: 0.3rem 1.6rem;
    background-color: #fff;
    border: solid 0.06rem #f99f64;
    border-radius: 0.75rem;
    font-size: 12px;
    color: #ff6500;
  }

  .tip-title {
  }

  .tip-content {
    font-size: 12px;
  }

  .tip-content p {
    padding-top: 0.4rem;
    color: #fff;
  }

  .tip-content p a {
    color: #ff6500;
  }

  .toast-mask {
    position: fixed;
    top: 0;
    left: 0;
    background: rgba(0, 0, 0, 0.6);
    z-index: 10000;
    width: 100%;
    height: 100%;
  }

  .toast {
    position: fixed;
    top: 50%;
    left: 50%;
    z-index: 20000;
    transform: translate(-50%, -50%);
    width: 20.4375rem;
    background: url("../assets/img/popout_bg.png") no-repeat;
    -webkit-background-size: 100% 100%;
    background-size: 100% 100%;
    border-radius: 0.3125rem;
    height: 19.1rem;
    padding: 0 4rem 0 4rem;
  }

  .toast-container {
    position: relative;
    width: 100%;
    height: 100%;
    /*border: 1px dotted #fccc6e;*/
  }


  .toast-title {
    padding-top: 4rem;
    font-size: 24px;
    color: #7f4016;
    text-align: center;
  }

  .toast-btn {
    position: absolute;
    text-align: center;
    bottom: 2rem;
    left: 2rem;
    opacity: 0;
  }

  .toast-btn div {
    background-image: -moz-linear-gradient(-18deg, rgb(242, 148, 85) 0%, rgb(252, 124, 88) 51%, rgb(252, 124, 88) 99%);
    background-image: -ms-linear-gradient(-18deg, rgb(242, 148, 85) 0%, rgb(252, 124, 88) 51%, rgb(252, 124, 88) 99%);
    background-image: -webkit-linear-gradient(-18deg, rgb(242, 148, 85) 0%, rgb(252, 124, 88) 51%, rgb(252, 124, 88) 99%);
    box-shadow: 0px 4px 0px 0px rgba(174, 34, 5, 0.7);
    width: 8.6875rem;
    height: 1.875rem;
    border-radius: 1.875rem;
    text-align: center;
    line-height: 1.875rem;
    color: #fff;
  }

  .toast-ticket {
    padding: 0.5rem 0.2rem;
    background: rgba(0, 0, 0, .5);
    color: #fff;
    text-align: center;
    position: fixed;
    top: 50%;
    left: 50%;
    z-index: 20000;
    transform: translate(-50%, -50%);
    width: 7rem;
    -webkit-background-size: 100% 100%;
    background-size: 100% 100%;
    border-radius: 0.3125rem;
  }

  .toast_con {
    padding: 1rem 2.5rem;
    color: #a27555;
    text-align: center;
  }

  .ticket, .money {
    color: #85bfff;
    font-size: 1rem;
  }

  .ticket span, .money span {
    color: #fff;
    font-size: 1.2rem;
    font-weight: bold;
  }

  .order .view_order {
    background: #f5e800;
    border: none;
    color: #ed4c00;
    display: inline-block;
    display: inline-block;
    padding: 0.3rem 1.6rem;
    border-radius: 0.75rem;
    font-size: 12px;
  }

  .tip-title, .tip-content p {
    color: #fff;
  }

  .tip-content span {
    color: #f5e800;
  }
</style>
