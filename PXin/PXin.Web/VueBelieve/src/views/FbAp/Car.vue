<template>
  <div class="car" :style="{backgroundImage:`url(${HomeBg})`}">
    <div class="top">
      <div><img src="../../assets/images/car_theme_bg.png" alt="" height="113"></div>
      <button type="button" :class="peichestatus>0?'active':''">{{peichestatusshow}}</button>
    </div>
    <div class="content">
      <img class="carImg" src="../../assets/images/car_bg.png" alt="" height="77">
      <div class="car-status" :style="{backgroundImage:`url(${textbox})`}">
        <div>
          <div class="title">审核状态</div>
          <div class="status" @click="getNotPassDialog" v-if="approvalstatus===0||approvalstatus===2">{{approvalstatusshow}}
            <van-icon color="red" name="question-o"/>
          </div>
          <div class="status active" v-if="approvalstatus===1">
            {{approvalstatusshow}}
          </div>
        </div>
        <div class="line"></div>
        <div>
          <div class="title">冻结状态</div>
          <div class="status" @click="getFreezeDialog" v-if="freezestatus!==0"> {{freezestatusshow}}
            <van-icon color="red" name="question-o"/>
          </div>
          <div class="status active" v-if="freezestatus===0">
           {{freezestatusshow}}
          </div>
        </div>
      </div>
      <div class="car-term">
        <div class="term-title">下方二者满足其一即可</div>
        <div class="term-body">
          <div class="body-item">
            <div>进货批发码总计</div>
            <div @click="getWholesaleDialog" v-if="ispfm===false">{{pfm}}<van-icon color="red" name="question-o"/></div>
            <div v-if="ispfm">{{pfm}}</div>
          </div>
          <div class="body-item">
            <div>回收充值码(SVC)总计</div>
            <div @click="getSVCDialog" v-if="issvc===false"> {{svc}}<van-icon color="red" name="question-o"/></div>
            <div v-if="issvc">{{svc}}</div>
          </div>
        </div>
      </div>
    </div>
    <div class="rule">
      <div class="rule-title">
        <img src="../../assets/images/car_activity_bg.png" alt="" height="28">
      </div>
      <p>
        1、只有充值商才有配车资格；<br/><br/>
        2、充值商当前状态为审核通过且未冻结状态；<br/><br/>
        3、从开通充值商起，进货批发码数量大于等于20万(即进货
        总次数大于等于10次)，或者回收充值码(SVC)大于等于50万，
        两个条件满足一个即获得配车资格
      </p>
    </div>
  </div>
</template>

<script>
    import HomeBg from '@/assets/images/car_home_bg_.png'
    import textbox from '@/assets/images/car_textbox.png'
    import { Dialog,Toast} from "vant";
    import {GetJxsPeiche} from '@/api/getFbApData'
    import {separateStr} from '@/config/utils'
    export default {
        name: "Car",
        data() {
            return {
                HomeBg,
                textbox,
                approvalstatus:0,
                approvalstatusshow:'',
                peichestatusshow:'未配车',
                peichestatus:0,
                freezestatus:1,
                freezestatusshow:'',
                pfm:0,
                svc:0,
                issvc:false,
                ispfm:false,
            }
        },
        created(){
            this.getPeiCheInfoData();
        },
        methods: {
            async getPeiCheInfoData(){
                let res=await GetJxsPeiche(this.$global.userInfo,this.$route.params.infoId);
                if(res.result>0&&res.data){
                    this.approvalstatus=res.data.approvalstatus;
                    this.approvalstatusshow=res.data.approvalstatusshow;
                    this.freezestatus=res.data.freezestatus;
                    this.freezestatusshow=res.data.freezestatusshow;
                    this.svc=separateStr(res.data.svc,3,',');
                    this.pfm=separateStr(res.data.pfm,3,',');
                    if(res.data.pfm>=200000||res.data.svc>=500000){
                        this.ispfm=true;
                        this.issvc=true;
                    }
                    this.peichestatusshow=res.data.peichestatusshow;
                    this.peichestatus=res.data.peichestatus;
                    return;
                }
                if(res.result<1){
                    Toast(res.message);
                }

            },
            //未通过弹窗
            getNotPassDialog() {
                Dialog.confirm({
                    message: '未上传认证资料/审核状态已拒绝，不满足配车条件，立即去上传认证资料',
                    confirmButtonText:'上传资料',
                    cancelButtonColor:'#2ea2fa'
                }).then(() => {
                    this.$router.push({path:'/IdentityIndex',query:{infoid: this.$route.query.infoId}})
                }).catch(() => {
                    // on cancel
                });
            },
            //冻结弹窗
            getFreezeDialog() {
                Dialog.alert({
                    message: '当前状态已冻结，不满足配车条件，请联系客服解冻',
                    confirmButtonText:'知道了',
                }).then(() => {
                    // on close
                });
            },
            //批发码弹窗
            getWholesaleDialog() {
                let that=this;
                Dialog.confirm({
                    message: '进货批发码总计小于20万，不满足配车条件，赶紧去进货批发码',
                    confirmButtonText:'去进货',
                    cancelButtonColor:'#2ea2fa'
                }).then(() => {
                    that.$router.push({
                        path: "/Stockin",
                        query: {
                            statusdesc: that.$route.query.statusdesc,
                            infoid: that.$route.query.infoId
                        }
                    });
                }).catch(() => {

                });
            },
            //svc弹窗
            getSVCDialog() {
                Dialog.alert({
                    message: '回收充值码(SVC)总计小于50万，不满足配车条件，联系好友将充值码(SVC)转让给你即完成回收充值码',
                    confirmButtonText:'知道了',
                }).then(() => {
                    // on close
                });
            },

        }
    }
</script>

<style scoped lang="scss">
  .car {
    height: 100%;
    background-size: 100% 100%;
    background-repeat: no-repeat;
    padding-top: 6.5rem;
    position: relative;

    .top {
      position: absolute;
      top: 0.8rem;
      left: 0;
      right: 0;
      text-align: center;

      img {
        margin-bottom: 0.6rem;
      }

      button {
        background-color: rgba(255, 255, 255, .5);
        border-radius: 0.44rem;
        font-weight: 700;
        border: none;
        outline: none;
        color: #ffffff;
        font-size: 0.36rem;
        padding: 0.27rem 1.28rem;
        &.active{
          background-color: #fff;
          color: #2ea2fa;
        }
      }
    }

    .content {
      .van-icon {
        vertical-align: middle;
        margin-left: 0.1rem;
      }

      position: relative;
      margin: 0 0.3rem;
      padding: 0.95rem 0.3rem 0.49rem 0.3rem;
      /*height: 6rem;*/
      background-color: #fff;
      box-shadow: 0rem 0.1rem 0.2rem 0rem rgba(70, 75, 203, 0.3);
      border-radius: 0.1rem;

      .carImg {
        position: absolute;
        top: -1.2rem;
        left: 0;
      }

      .car-status {
        background-size: 100% 100%;
        background-repeat: no-repeat;
        height: 1.68rem;
        display: flex;
        justify-content: space-evenly;
        align-items: center;
        font-size: 0.3rem;
        margin-bottom: 0.48rem;

        .title {

          color: #333;
          padding-bottom: 0.4rem;
        }

        .status {
          color: red;
          &.active{
            color: #2ea2fa;
            font-weight: 700;
          }

        }

        .line {
          width: 1px;
          background-color: #fff;
          height: 1rem;
        }
      }

      .car-term {
        background-color: #f7f7fc;
        border-radius: 0.12rem;
        padding-bottom: 0.3rem;

        .term-title {
          width: 2.8rem;
          background-color: #2ea2fa;
          border-radius: 0rem 0.26rem 0.26rem 0rem;
          color: #fff;
          font-size: 0.24rem;
          padding: 0.14rem 0;
          text-align: center;
        }

        .body-item {
          padding: 0.3rem 0.25rem 0.1rem 0.25rem;
          display: flex;
          justify-content: space-between;
        }
      }
    }

    .rule {
      .rule-title {
        text-align: center;
        padding: 0.6rem 0;
      }

      p {
        padding: 0 0.24rem 0 0.34rem;
        color: #fff;
        font-size: 0.28rem;
      }
    }
  }
</style>
