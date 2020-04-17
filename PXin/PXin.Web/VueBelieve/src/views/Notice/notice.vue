<template>
  <div class="noticeDetail" id="notice">
    <div class="textBx">
      <div class="title">{{ info.title }}</div>
      <div class="time">{{$t('lang.NOTICE_NOTICE_MEETINGTIME')+':'+info.starttime }}</div>
      <div class v-html="info.detail"></div>
    </div>
    <div class="inpBx">
      <form action>
        <div class="title" v-show="info.status!=-1">{{$t('lang.NOTICE_NOTICE_FORMTITLE')}}</div>
        <div class="inp" v-show="info.status!=-1">
          <span>{{$t('lang.NOTICE_NOTICE_NAME')}}</span>
          <input
            type="text"
            maxlength="10"
            v-model="info.name"
            :placeholder="$t('lang.NOTICE_NOTICE_NAME')"
            :disabled="info.status!=0?true:false"
            @click="setClc"
            @focus="setView"
            @blur="setBlur"
          />
        </div>
        <div class="inp" v-show="info.status!=-1">
          <span>{{$t('lang.NOTICE_NOTICE_PHONE')}}</span>
          <input
            type="text"
            v-model="info.mobileno"
            maxlength="11"
            @keyup="info.mobileno=info.mobileno.replace(/\D/g,'')"
            :placeholder="$t('lang.NOTICE_NOTICE_PHONE')"
            :disabled="info.status!=0?true:false"
            @click="setClc"
            @focus="setView"
            @blur="setBlur"
          />
        </div>
        <h5 v-show="info.status!=-1">{{$t('lang.NOTICE_NOTICE_ATTENDEE')}}</h5>
        <div class="inp" v-show="info.status!=-1">
          <span>
            <input
              type="text"
              maxlength="10"
              v-model="firstJoinMeetingPerson.joinpersonname"
              :disabled="info.status!=0?true:false"
              @click="setClc"
              @blur="setBlur"
              @focus="setView"
              :placeholder="$t('lang.NOTICE_NOTICE_NAME')"
            />
          </span>
          <input
            type="number"
            maxlength="11"
            @keyup="firstJoinMeetingPerson.joinpersonmobileno=firstJoinMeetingPerson.joinpersonmobileno.replace(/\D/g,'')"
            v-model="firstJoinMeetingPerson.joinpersonmobileno"
            :placeholder="$t('lang.NOTICE_NOTICE_PHONE')"
            :disabled="info.status!=0?true:false"
            @click="setClc"
            @focus="setView"
            @blur="setBlur"
          />
        </div>

        <div
          class="inp"
          v-show="info.status!=-1&&joinMeetingPersonList"
          v-for="(item,index) in joinMeetingPersonList"
          :key="index"
        >
          <span>
            <input
              type="text"
              maxlength="10"
              v-model="item.joinpersonname"
              :disabled="info.status!=0?true:false"
              @click="setClc"
              @blur="setBlur"
              @focus="setView"
              placeholder="姓名"
            />
          </span>
          <input
            type="number"
            maxlength="11"
            @keyup="item.joinpersonmobileno=item.joinpersonmobileno.replace(/\D/g,'')"
            v-model="item.joinpersonmobileno"
            :placeholder="$t('lang.NOTICE_NOTICE_PHONE')"
            :disabled="info.status!=0?true:false"
            @click="setClc"
            @focus="setView"
            @blur="setBlur"
          />
          <i class="delInviter" v-show="info.status==0" @click="DelInviter(index)">{{$t('lang.NOTICE_NOTICE_DEL')}}</i>
        </div>
        <div class="addInviter" v-show="info.status==0">
          <span @click="AddInviter">+ {{$t('lang.NOTICE_NOTICE_ADD')}}</span>
        </div>
        <div class="btn" v-if="info.status==0" @click="ValidationForm">{{$t('lang.NOTICE_NOTICE_SUBMIT')}}</div>
        <div class="btn btn1" v-else-if="info.status==1">{{$t('lang.NOTICE_NOTICE_APPLY')}}</div>
        <div class="btn btn-1" v-else-if="info.status==-1">{{$t('lang.NOTICE_NOTICE_OVERDUE')}}</div>
      </form>
    </div>
    <div v-show="keybd" class="keybd"></div>
  </div>
</template>

<script>
    import {GetMeetInfoDetail, JoinMeeting} from "@/api/getFbApData";

    export default {
        data() {
            return {
                info: {},
                keybd: false,
                firstJoinMeetingPerson: {},
                joinMeetingPersonList: [],
                isForm: true
            };
        },
        created() {
            this.$i18n.locale = this.$global.lang;
            var clientHeight = document.documentElement.clientHeight || document.body.clientHeight;
            window.onresize = function () {
                var nowClientHeight = document.documentElement.clientHeight || document.body.clientHeight;
                if (clientHeight - nowClientHeight > 60) {//因为ios有自带的底部高度
                    //键盘弹出的事件处理
                    document.getElementById("notice").classList.add("focusState");
                } else {
                    //键盘收起的事件处理
                    document.getElementById("notice").classList.remove("focusState");
                }
            };
        },
        mounted() {
            this.GetMeetInfoDetail();
        },
        methods: {
            GetMeetInfoDetail() {
                let infoid = this.$route.query.infoid;
                GetMeetInfoDetail(
                    {
                        ...JSON.parse(sessionStorage.userParam),
                        infoid: infoid
                    },
                    res => {
                        if (res.result > 0) {
                            this.info = res.data;
                            for (
                                let index = 0;
                                index < res.data.joinmeetingpersons.length;
                                index++
                            ) {
                                const element = res.data.joinmeetingpersons[index];
                                if (index == 0) {
                                    this.firstJoinMeetingPerson = element;
                                } else {
                                    this.joinMeetingPersonList.push(element);
                                }
                            }
                        }
                    }
                );
            },
            setView(e) {
                var u = navigator.userAgent,
                  app = navigator.appVersion;
                var isAndroid = u.indexOf("Android") > -1 || u.indexOf("Linux") > -1; //android终端或者uc浏览器
                var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
                if (isiOS) {
                  this.keybd = true;
                  this.$nextTick(() => {
                    document.getElementById("notice").scrollTop = document.getElementById(
                      "notice"
                    ).offsetHeight;
                  });
                }
            },
            setBlur() {
                this.keybd = false;
            },
            setClc(e) {
                e.target.focus();
            },
            AddInviter() {
                let inviterObj = {
                    joinpersonname: "",
                    joinpersonmobileno: ""
                };
                this.joinMeetingPersonList.push(inviterObj);
            },
            DelInviter(idx) {
                this.joinMeetingPersonList.splice(idx, 1);
            },
            ValidationForm() {
                if (!this.info.name) {
                    this.Toast("请输入姓名");
                    return;
                }
                if (!this.info.mobileno) {
                    this.Toast("请输入电话");
                    return;
                }
                if (this.info.mobileno.length != 11) {
                    this.Toast("请输入正确的电话号码");
                    return;
                }
                if (!this.firstJoinMeetingPerson) {
                    this.Toast("请输入参会人信息");
                    return;
                }
                if (!this.firstJoinMeetingPerson.joinpersonname) {
                    this.Toast("请输入参会人姓名");
                    return;
                }
                if (!this.firstJoinMeetingPerson.joinpersonmobileno) {
                    this.Toast("请输入参会人电话");
                    return;
                }

                if (this.firstJoinMeetingPerson.joinpersonmobileno.length != 11) {
                    this.Toast("请输入正确的参会人电话号码");
                    return;
                }

                let isValidation = true;
                let isMobileLength = true;
                for (let item of this.joinMeetingPersonList) {
                    if (!item.joinpersonname || !item.joinpersonmobileno) {
                        isValidation = false;
                        break;
                    }
                    if (item.joinpersonmobileno.length != 11) {
                        isMobileLength = false;
                        break;
                    }
                }
                if (!isValidation) {
                    this.Toast("");
                    return;

                }
                if (!isMobileLength) {
                    this.Toast("请输入正确的参会人电话号码");
                    return;
                }
                this.SendFormData();
            },
            SendFormData() {
                let infoid = this.$route.query.infoid;
                let meetinglist = [
                    ...this.joinMeetingPersonList,
                    this.firstJoinMeetingPerson
                ];
                JoinMeeting(
                    {
                        ...JSON.parse(sessionStorage.userParam),
                        ...this.info,
                        infoid: infoid,
                        joinmeetingpersons: meetinglist
                    },
                    res => {
                        if (res.result > 0) {
                            this.Toast("报名成功");
                            setTimeout(() => {
                                this.$router.go(-1);
                            }, 1000);
                        } else {
                            this.Toast(res.message);
                        }
                    }
                );
            }
        }
    };
</script>

<style lang="scss" scoped>
  .focusState {
    position: absolute;
    left: 0;
    right: 0;
  }

  .noticeDetail {
    min-height: 100%;

    .keybd {
      height: 4rem;
      width: 100%;
    }

    .textBx {
      padding: 0.3rem;
      // background: #f7f7fc;
      .title {
        font-size: 0.36rem;
        font-weight: bold;
        margin-bottom: 0.35rem;
      }

      .time {
        text-align: left;
        font-size: 0.24rem;
        margin-bottom: 0.6rem;
      }
    }

    .inpBx {
      padding: 0.5rem;

      h5 {
        font-size: 0.36rem;
        margin: 0 0 0.3rem 0;
        padding: 0 0.25rem;
      }

      .addInviter {
        text-align: center;

        span {
          font-size: 0.3rem;
          color: #2ea2fa;
          padding: 0.3rem 0.6rem;
          display: inline-block;
          text-align: center;
          border: dashed 1px #2ea2fa;
          border-radius: 3px;
        }
      }

      .title {
        font-size: 0.36rem;
        font-weight: bold;
        text-align: center;
        color: #2ea2fa;
        margin-bottom: 0.6rem;
      }

      .inp {
        background: #f7f7fc;
        border-radius: 0.06rem;
        margin-bottom: 0.3rem;
        padding: 0.3rem 0.25rem;
        display: flex;
        align-items: center;
        position: relative;

        .delInviter {
          position: absolute;
          right: 10px;
          color: #ff1541;
          font-style: normal;
          display: inline-block;
        }

        span {
          width: 2.1rem;
          box-sizing: border-box;

          input {
            width: 100%;
            padding-right: 0.2rem;
            box-sizing: border-box;
          }
        }

        input {
          border: none;
          background: #f7f7fc;
          font-weight: bold;
          flex: auto;

          &::placeholder {
            color: #999;
            font-weight: normal;
          }
        }
      }

      .btn {
        background: #2ea2fa;
        text-align: center;
        padding: 0.3rem 0;
        border-radius: 0.06rem;
        color: #fff;
        font-weight: bold;
        margin: 0.7rem 0;

        &.btn1 {
          background: #96d0fc;
        }

        &.btn-1 {
          background: #e70c0c;
          opacity: 0.3;
        }
      }
    }
  }
</style>
