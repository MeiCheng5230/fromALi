<!-- 信友圈 -->
<template>
  <div class="friends">
    <!-- <canvas id="canvas" style="display: none"></canvas>
    <img id='canvasImg' src="" alt=""> -->
    <!-- :style=" backgroundImage:'url('+require('@/assets/images/chat_chat_camera@2x.png')+')'" -->
    <!-- 背景 -->
    <div class="abs">
      <div class="header">
        <div
          class="top"
          :style="user.backpic && 'backgroundImage:url('+user.backpic+');background-color:#fff'"
          @click="changeBackImageShow=true"
        >
          <div class="shadeUp">
            <img src="@/assets/images/believe_xinyou_top_bg_up.png" alt />
          </div>
          <div class="shadeDown">
            <img src="@/assets/images/believe_xinyou_top_bg_down.png" alt />
          </div>

          <div class="titIMG">
            <div @click.stop="$router.push('/Publishs')">
              <img src="@/assets/images/dynamic_publish.png" alt />
            </div>
          </div>
          <!-- <div class="setBG">
            <van-uploader :after-read="AfterRead">{{$t('m.changeback')}}</van-uploader>
          </div>-->
          <!-- 头像 名称 点赞 -->
          <div class="titImg">
            <img :src="user.appphoto" alt @click.stop />
            <span>{{user.nodename}}</span>
          </div>
        </div>
        <!-- 头像 名称 点赞 -->
        <div class="titBott">
          <div>
            <div>
              <img src="@/assets/images/dynamic_zan_icon@2x.png" alt />
              <span>{{user.up}}</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_tan_icon@2x.png" alt />
              <span>{{user.down}}</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_v_sel@2x.png" alt />
              <span>{{user.v}}</span>
            </div>
            <div>
              <img src="@/assets/images/dynamic_p_sel@2x.png" alt />
              <span>{{user.p}}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 内容 -->
    <van-list
      v-if="friends&&friends.length>0"
      class="content"
      v-model="loading"
      :finished="finished"
      finished-text="到底了~"
      @load="onLoad()"
      :offset="20"
    >
      <div v-for="(message,index) of messages" :key="index" class="usertype1">
        <!-- 朋友头像，名称 发布时间 -->
        <div class="usertype1Head">
          <div @click="GoUserInformation(message.nodeid)">
            <img :src="GetShowAppPhoto(message.nodeid)" alt />
          </div>
          <div>
            <p>{{GetShowName(message.nodeid)}}</p>
            <p>{{message.createtime}}</p>
          </div>
        </div>
        <!-- 发布内容 -->
        <div class="IssuedContent">
          <!-- 文字 -->
          <div class="IssuedText">{{message.content}}</div>
          <div class="Issuedimg">
            <div v-for="(image,index) of GetImages(message.picurl)" :key="index">
              <img @click="ClickBigImg(image)" :src="image" alt />
            </div>
          </div>
          <!-- userVideo -->
          <div class="userVideo" v-if="message.video">
            <!-- poster -->
            <video webkit-playsinline="true" x5-video-player-fullscreen="true" :id="'video'+message.infoid" class="video-js vjs-big-play-centered" :poster="GetPoster('video'+message.infoid)">
              <source :src="message.video" type="video/mp4" />
            </video>
            <!-- <canvas :id="'canvas'+message.infoid" style=""></canvas>-->
            <img :id="'img'+message.infoid" class='canvasImg' src="" alt=""> 
            <div class="vjs-big-play-button" @click="PlayVideo('video'+message.infoid)">
              <img src="@/assets/images/dynamic_pay@2x.png" alt />
            </div>
          </div>
          <!-- 点赞，踩 其他 （已支付此消息时，显示此内容）-->
          <div v-if="message.ispay==1" class="IssuedUp">
            <div>
              <div @click="MessageUp(message,index)">
                <img v-if="message.isup!=1" src="../../assets/images/dynamic_zan_nor@2x.png" alt />
                <img v-if="message.isup==1" src="../../assets/images/dynamic_zan_sel@2x.png" alt />
                <span>{{message.up}}</span>
              </div>
              <div @click="MessageDown(message,index)">
                <img v-if="message.isdown!=1" src="../../assets/images/dynamic_tan_nor@2x.png" alt />
                <img v-if="message.isdown==1" src="../../assets/images/dynamic_tan_sel@2x.png" alt />
                <span>{{message.down}}</span>
              </div>
            </div>
            <div>
              <!-- 评论 打赏 举报 -->
              <div class="comment" v-if="message.msgTools">
                <span @click="CommentOnClick(message)">评论</span>
                <i></i>
                <span v-if="message.nodeid!==user.nodeid" @click="GratuityOnClick(message)">打赏</span>
                <i v-if="message.nodeid!==user.nodeid"></i>
                <span v-if="message.nodeid!==user.nodeid" @click="ReportOnClick(message)">举报</span>
                <span v-if="message.nodeid===user.nodeid" @click="Delete(message,index)">删除</span>
              </div>
              <img
                @click="message.msgTools=!message.msgTools"
                src="@/assets/images/dynamic_more@2x.png"
                alt
              />
            </div>
          </div>
          <!--未支付是显示此内容-->
          <div v-if="message.ispay!=1" class="IssuedUp">
            <div class="pay">
              <!-- 只需支付1个V点，可查看全文 -->
              <div>
                <img src="@/assets/images/dynamic_lock@2x.png" alt />
                只需支付{{message.price}}个V点，可查看全文
              </div>
              <div @click="PayForRead(message,index)">
                <button>支付</button>
              </div>
            </div>
          </div>
          <!-- 评论列表 -->
          <div class="commentList" v-if="GetComments(message).length>0">
            <div
              v-for="(comment,index) of GetComments(message)"
              :key="index"
              @click="ReplyOnClick(message,comment)"
            >
              <div v-if="comment.phisid==0">
                <span
                  @click="GoUserInformation(comment.nodeid)"
                  class="color"
                >{{GetShowName(comment.nodeid)}} :</span>
                {{comment.content}}
              </div>
              <div v-else>
                <span
                  @click="GoUserInformation(comment.nodeid)"
                  class="color"
                >{{GetShowName(comment.nodeid)}}</span>
                回复
                <span
                  @click="GoUserInformation(comment.pnodeid)"
                  class="color"
                >{{GetShowName(comment.pnodeid)}} :</span>
                {{comment.content}}
              </div>
            </div>
          </div>
        </div>
      </div>
    </van-list>
    <!-- 打赏 -->
    <van-popup v-model="gratuityShow">
      <div class="rewards">
        <div class="rewardstop">
          <span style="width: .22rem;"></span>
          <span>选择打赏金额</span>
          <img @click="gratuityShow=false" src="@/assets/images/dynamic_delete@2x.png" alt />
        </div>

        <div class="rewardsMiddle">
          <div v-for="(item,index) of gratuityType" :key="index" @click="IsSelected(item,index)">
            <span>{{item.amount}}V</span>
            <img v-show="item.IsSelected" src="@/assets/images/dynamic_choices@2x.png" alt />
          </div>

          <div>
            <input
              v-model="gratuityAmount"
              @click="GratuityTypeClear()"
              ref="ipt"
              placeholder="请输入其他金额"
              type="number"
            />
            <span style="color: #2ea2fa;">V</span>
          </div>
        </div>

        <div class="rewardsBtn">
          <button @click="Gratuity()">确认</button>
        </div>
      </div>
    </van-popup>
    <!-- 评论 -->
    <div class="commentIpt" v-show="commentShow">
      <input
        v-model="commentContent"
        :placeholder="commentPlaceholder"
        type="text"
        ref="comment"
        @keydown="OnTextareaKeyDown"
      />
    </div>
    <!--更换背景-->
    <van-popup v-model="changeBackImageShow" position="bottom">
      <div class="upload">
        <van-uploader :after-read="ChangeBackImage">
          <span style="color: #333333;font-size:0.3rem">更换相册封面</span>
        </van-uploader>
      </div>
      <div class="close" @click="changeBackImageShow=false">取消</div>
    </van-popup>
    <!-- 大图 -->
    <van-image-preview :showIndex="false" v-model="bigImg" :images="images"></van-image-preview>
  </div>
</template>

<script>
import videojs from "video.js";
import "video.js/dist/video-js.css";
import { videoPlayer } from "vue-video-player";
import { UploadFile } from "@/api/sysRequest.js";
import { Base64 } from "@/config/utils.js";
import { fail } from "assert";
import { release } from "os";
import {
  GetUserInfo_Fri,
  GetMsgHome,
  MyFriend,
  CreateAttitude,
  PayVDianForRead,
  CreateReward,
  DeleteMsg,
  CreateComment,
  EditBackgImg
} from "@/api/findData.js";



export default {
  data() {
    return {
      bigImg: false,
      images: [], //点击放大
      user: {},
      pageindex: 1,
      pagesize: 20,
      loading: false,
      finished: false,
      friends: [],
      comments: [],
      messages: [],
      operatingMessage: {}, //当前操作的消息
      commentShow: false, //评论框显示控制
      gratuityShow: false, //打赏框显示控制
      gratuityAmount: "", //打赏金额
      selectedAmount: 0, //选中的打赏金额
      commentContent: "",
      phisid: 0, //被回复主键
      pnodeid: 0, //被回复人ID
      commentPlaceholder: "评论", //评论无值时提示内容
      changeBackImageShow: false, //更换背景框显示
      ImageType: { jpg: "", jpeg: "", png: "", gif: "", bmp: "" },
      ImageTypeArray: ["jpg", "jpeg", "png", "gif", "bmp"],
      //打赏选项
      gratuityType: [
        {
          amount: 1,
          IsSelected: true
        },
        {
          amount: 5,
          IsSelected: false
        },
        {
          amount: 10,
          IsSelected: false
        }
      ],
     
    };
  },
  mounted() {
    //绑定滚动事件,
    document
      .getElementsByClassName("friends")[0]
      .addEventListener("Scroll", this.Scroll);
  },
  components: {
    videoPlayer
  },
  created() {
    this.GetUserInfo();
  },
  computed: {

  },
  methods: {
    //获取视频第一帧图片
    GetPoster(id) {
      this.$nextTick(()=>{
        let video = document.getElementById(id);
        try{
          var canvas = document.createElement("canvas");
          var img = new Image();//创建新的图片对象
          var base64 =''; 
          img.src=video.firstElementChild.src;
          //图片跨域
          img.setAttribute("crossOrigin",'Anonymous');
          var  imgId = id.slice(5);
          img.onload=function(){
            canvas.getContext("2d").drawImage(img,0,0,300,200);
            base64 = canvas.toDataURL("image/png");
            document.getElementById('img'+imgId).src=base64;
            return base64;
          }
          img.onerror = err => {
              console.log(err);
          };
        }catch(e){
          console.log(e)
        }
      })
    },

    //点击大图
    ClickBigImg(src) {
      this.bigImg = true;
      this.images[0] = src;
    },
    // 点击播放视频
    PlayVideo(infoid) {
      document.getElementById(infoid).play();
    },
    //更换相册封面
    ChangeBackImage(file) {
      let data = {};
      data["content"] = file.content;
      data["typeid"] = file.file.name.substring(
        file.file.name.lastIndexOf(".") + 1
      );
      if (!(data.typeid in this.ImageType)) {
        this.$toast("请上传：" + this.ImageTypeArray.join("，") + "类型文件");
        return;
      }
      UploadFile(data, data => {
        if (data.result < 1) {
          this.$toast("修改头像失败");
          return;
        }
        let fullurl = data.data.fullurl;
        EditBackgImg({ backimg: fullurl }, data => {
          this.$toast(data.message);
          this.user.backpic = file.content;
          this.changeBackImageShow = false;
        });
      });
      this.headPicBoxShow = false;
    },
    GoUserInformation: function(userid) {
      this.$router.push({
        name: "Information",
        query: { userId: userid }
      });
    },
    GetComments: function(message) {
      let comments = [];
      this.comments.forEach(element => {
        if (message.infoid == element.infoid) {
          comments.push(element);
        }
      });
      return comments;
    },
    //消息点赞
    MessageUp: function(message, index) {
      if (message.nodeid == this.user.nodeid) {
        this.$toast("不能自己给自己点赞或踩");
        return;
      }
      if (message.isup == 1 || message.isdown == 1) {
        this.$toast("一个文章只能点赞或踩一次");
        return;
      }
      CreateAttitude({ infoid: message.infoid, isupdown: 1 }, data => {
        if (data.result < 1) {
          this.$toast(data.message);
          return;
        }
        message.up++;
        message.isup = 1;
      });
    },
    //踩消息
    MessageDown: function(message, index) {
      if (message.nodeid == this.user.nodeid) {
        this.$toast("不能自己给自己点赞或踩");
        return;
      }
      if (message.isup == 1 || message.isdown == 1) {
        this.$toast("一个文章只能点赞或踩一次");
        return;
      }
      CreateAttitude({ infoid: message.infoid, isupdown: -1 }, data => {
        if (data.result < 1) {
          this.$toast(data.message);
          return;
        }
        message.down++;
        message.isdown = 1;
      });
    },
    //支付查看消息内容
    PayForRead: function(message, index) {
      PayVDianForRead({ infoid: message.infoid }, data => {
        if (data.result < 1) {
          this.$toast(data.message);
          return;
        }
        for (let prop in data.data) {
          this.$set(message, prop, data.data[prop]);
        }
      });
    },
    //评论点击事件
    CommentOnClick: function(message) {
      this.commentPlaceholder = "评论";
      this.commentShow = true;
      this.operatingMessage = message;
      message.msgTools = false;
      this.phisid = 0; //评论时无被记录
      this.pnodeid = 0; //评论时无被回复人
    },
    //回复点击事件
    ReplyOnClick: function(message, comment) {
      if (comment.nodeid == this.user.nodeid) {
        return;
      }
      this.commentPlaceholder =
        "回复 " + this.GetShowName(comment.nodeid) + "：";
      this.commentShow = true;
      this.operatingMessage = message;
      this.phisid = comment.hisid;
      this.pnodeid = comment.nodeid;
    },
    CreateComment: function() {
      let infoid = this.operatingMessage.infoid;
      let content = this.commentContent;
      CreateComment(
        { infoid: infoid, content: content, phisid: this.phisid },
        data => {
          this.$toast(data.message);
          if (data.result < 1) {
            return;
          }
          let obj = {};
          obj["content"] = content;
          obj["createtime"] = Date.now;
          obj["hisid"] = data.data.keyid;
          obj["infoid"] = infoid;
          obj["nodeid"] = this.user.nodeid;
          obj["phisid"] = this.phisid;
          obj["pnodeid"] = this.pnodeid;
          this.$nextTick(() => {
            this.comments.push(obj);
          });
          this.commentContent = "";
          this.commentShow = false;
        }
      );
    },
    //打赏按钮点击事件
    GratuityOnClick: function(message) {
      this.gratuityShow = true;
      message.msgTools = false;
      this.operatingMessage = message;
    },
    //打赏
    Gratuity: function() {
      let amount = 0;
      if (
        this.selectedAmount == 0 &&
        (this.gratuityAmount == "" ||
          this.gratuityAmount == null ||
          this.gratuityAmount == 0)
      ) {
        this.$toast("请先选择或输入打赏金额");
        return;
      }
      if (this.selectedAmount != 0) {
        amount = this.selectedAmount;
      } else {
        amount = this.gratuityAmount;
      }
      CreateReward(
        { infoid: this.operatingMessage.infoid, reward: amount },
        data => {
          if (data.result < 1) {
            this.$toast(data.message);
            return;
          }
          this.$toast("打赏成功");
          this.gratuityShow = false;
        }
      );
    },
    //打赏 选择V点
    IsSelected: function(item, index) {
      for (let item of this.gratuityType) {
        item.IsSelected = false;
      }
      item.IsSelected = true;
      this.selectedAmount = item.amount;
      this.gratuityAmount = "";
    },
    //打赏 选择其他金额
    GratuityTypeClear: function() {
      for (let item of this.gratuityType) {
        item.IsSelected = false;
      }
      this.selectedAmount = 0;
    },
    //举报
    ReportOnClick: function(message) {
      let nodename = this.GetShowName(message.nodeid);
      message = Base64.encode(JSON.stringify(message));
      this.$router.push({
        name: "Report",
        query: { nodename: nodename, message: message }
      });
    },
    //删除
    Delete: function(message, index) {
      message.msgTools = false;
      this.$dialog
        .confirm({
          message: "确定删除码?",
          cancelButtonText: "取消",
          confirmButtonText: "删除"
        })
        .then(() => {
          DeleteMsg({ infoid: message.infoid }, data => {
            this.$toast(data.message);
            if (data.result < 1) {
              return;
            }
            this.$nextTick(() => {
              this.messages.splice(index, 1);
            });
          });
        })
        .catch(() => {});
    },
    onLoad() {
      // setTimeout(() => {
      this.GetMsg();
      // }, 500);
    },
    GetImages: function(picurl) {
      if (picurl == null) {
        return [];
      }
      let images = picurl.split(",");
      return images;
    },
    GetShowName: function(nodeid) {
      let user = this.GetFriend(nodeid);
      if (!user) {
        return "";
      }
      if (user.nickname && user.nickname != "") {
        return user.nickname;
      }
      return user.nodename;
    },
    GetShowAppPhoto: function(nodeid) {
      let user = this.GetFriend(nodeid);
      if (!user) {
        return "";
      }
      return user.appphoto;
    },
    GetFriend: function(nodeid) {
      if (nodeid == this.user.nodeid) {
        return this.user;
      }
      for (let index = 0; index < this.friends.length; index++) {
        if (this.friends[index].nodeid == nodeid) {
          return this.friends[index];
        }
      }
    },
    GetMyFriend: function() {
      MyFriend(null, data => {
        if (data.result < 1) {
          this.$toast("获取消息信息失败");
          return;
        }
        this.friends = data.data;
        // this.GetMsg();
      });
    },
    GetUserInfo: function() {
      GetUserInfo_Fri(null, data => {
        if (data.result < 1) {
          this.$toast("用户数据加载失败");
          // setTimeout(() => {
          this.$router.go(-1);
          // }, 500);
          return;
        }
        this.user = data.data[0];
        this.GetMyFriend();
      });
    },
    GetMsg: async function(callback) {
      let data = {};
      data["pageindex"] = this.pageindex;
      data["pagesize"] = this.pagesize;
      data["starttime"] = "2018-01-01T00:00:00Z";
      let resp = await GetMsgHome(data, null);
      if (resp.result < 1) {
        this.$toast("获取消息信息失败");
        return;
      }
      // console.info(resp);
      resp.data.messages.forEach(element => {
        element["msgTools"] = false;
      });
      if (data.pageindex == 1) {
        this.comments = resp.data.comments;
        this.messages = resp.data.messages;
      } else {
        console.info(this.messages);
        this.$nextTick(() => {
          this.comments = this.comments.concat(resp.data.comments);
          this.messages = this.messages.concat(resp.data.messages);
        });
        console.info(this.messages);
      }
      if (resp.data.messages.length < this.pagesize) {
        this.finished = true;
      }
      this.loading = false;
      this.pageindex++;
    },
    //键盘keycode=13
    OnTextareaKeyDown() {
      if (event.keyCode == 13) {
        this.CreateComment();
      }
    },
    //页面滚动事件
    Scroll() {
      this.commentShow = false;
      this.$refs.comment.blur();
    },
  },
  updated() {
    

    //底部评论框出现 自动获取焦点
    if (this.commentShow) {
      this.$refs.comment.focus();
    }
    // !document.getElementsByClassName('van-number-keyboard__body')[0] && return ;
    //设置密码框高度
    if (document.getElementsByClassName("van-number-keyboard__body")[0]) {
      let height = document.getElementsByClassName(
        "van-number-keyboard__body"
      )[0].offsetHeight;
      this.$refs.passwordList.style.bottom = height + "px";
    }
  }
};
</script>
<style scoped lang='scss'>
.canvasImg{
  position: absolute;
    height: 3.86rem;
    width: 2.4rem !important;
  left: 0;
  top: 0;
}
.userVideo {
  margin-left: 1.15rem;
  position: relative;
  .vjs-big-play-button {
    height: 3.86rem;
    width: 2.4rem;
    position: absolute;
    top: 0;
    left: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    img {
      width: auto;
      height: auto;
      max-width: 0.65rem;
      max-height: 0.65rem;
      z-index: 100;
    }
  }
}

/deep/ .vjs-control-bar {
  opacity: 0;
}

/deep/ .video-js {
  width: 2.4rem;
  height: 3.86rem;
  background: none;
}

/deep/ .vjs-poster {
  background-position: 100% 50% !important;
}

/* 中间的播放箭头 */
/deep/ .vjs-big-play-button .vjs-icon-placeholder {
  font-size: 1.63em;
}

/* 加载圆圈 */
/deep/ .vjs-loading-spinner {
  font-size: 2.5em;
  width: 2em;
  height: 2em;
  border-radius: 1em;
  margin-top: -1em;
  margin-left: -1.5em;
}

/deep/ .video-js.vjs-playing .vjs-tech {
  pointer-events: auto;
}

.van-popup {
  background: #f2f2f2;
}

.upload,
.close {
  height: 0.8rem;
  line-height: 0.8rem;
  text-align: center;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0.01rem;
  color: #333333;
  background: #fff;
}

.close {
  margin-top: 0.2rem;
}

.player {
  margin-top: 0.5rem;
  overflow: hidden;
}

// .video-player {
//   width: 2.41rem !important;
//   width: 100% !important;
//   height: 3.88rem !important;
// }
.shadeUp,
.shadeDown {
  height: 1rem;
  width: 100%;
  position: absolute;

  img {
    width: 100%;
    height: 100%;
  }
}

.shadeUp {
  top: 0;
}

.shadeDown {
  bottom: 0;
}

.commentIpt {
  width: 100%;
  box-sizing: border-box;
  position: fixed;
  bottom: 0;
  height: 1rem;
  padding: 0 0.3rem;
  display: flex;
  background: #fff;

  input {
    border: 0;
    width: 100%;
    height: 100%;
    font-family: PingFang-SC-Regular;
    font-size: 0.24rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #666;
  }
}

/deep/ [class*="van-hairline"]::after {
  border: none !important;
}

/deep/ .van-key {
  font-size: 14px !important;
}

.passwordList {
  position: fixed;
  left: 0;
  width: 100%;
  height: 1.5rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: #fff;

  & > div {
    /* margin-top: .1rem; */
    width: 4.8rem;
    height: 0.8rem;
    box-sizing: border-box;
    display: flex;

    & > div {
      background: #f1f1f1;
      display: flex;
      justify-content: center;
      align-items: center;
      width: 0.8rem;
      border: 0.01rem solid #bbb;
      box-sizing: border-box;
      font-size: 0.28rem;
    }

    & > div:nth-of-type(n + 2) {
      border-left: 0;
    }
  }

  p {
    padding-bottom: 0.1rem;
    margin: 0;
    font-family: MicrosoftYaHei;
    font-size: 0.21rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #999999;
  }
}

.password {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.3);
}

.rewardsBtn {
  display: flex;
  height: 1.3rem;
  align-items: center;
  box-sizing: border-box;
  padding: 0 0.8rem;

  button {
    border: 0;
    width: 100%;
    height: 0.61rem;
    background-color: #2ea2fa;
    border-radius: 0.05rem;
    font-family: MicrosoftYaHei;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
  }
}

.rewardsMiddle {
  & > div {
    border-bottom: 0.01rem solid #ddd;
    display: flex;
    padding-left: 0.45rem;
    padding-right: 0.55rem;
    height: 1rem;
    align-items: center;
    justify-content: space-between;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #1a1a1a;

    img {
      width: 0.39rem;
      height: 0.28rem;
    }

    input {
      padding: 0;
      border: 0;
      width: 80%;
      height: 90%;
    }
  }
}

.rewardstop {
  height: 0.84rem;
  align-items: center;
  display: flex;
  justify-content: space-between;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #999999;
  padding: 0 0.32rem;
  border-bottom: 0.01rem solid #dddddd;

  img {
    width: 0.22rem;
    height: 0.22rem;
  }
}

.van-popup {
  border-radius: 0.1rem;
}

.rewards {
  width: 5.8rem;
  height: 6.24rem;
  background-color: #ffffff;
}

.empty {
  height: 2rem;
  line-height: 2rem;
  text-align: center;
  background: #f2f2f2;
  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;

  letter-spacing: 0rem;
  color: #999999;
}

.Issuedimg {
  display: flex;
  padding-left: 1.15rem;
  flex-wrap: wrap;
  padding-bottom: 0.2rem;

  & > div {
    margin-left: 0.04rem;
    margin-top: 0.04rem;
    width: 1.8rem;
    height: 1.8rem;

    img {
      width: 100%;
      height: 100%;
    }
  }
}

.commentList {
  margin-top: 0.25rem;
  /* width: 100%; */
  background-color: #f0f0f0;
  box-sizing: border-box;
  padding: 0.3rem;
  margin-left: 1.15rem;
  margin-right: 0.3rem;

  & > div:nth-of-type(n + 2) {
    margin-top: 0.2rem;
  }

  & > div {
    /* display: flex; */
    /* flex-wrap: wrap; */
    .color {
      color: #00297b;
    }
  }

  font-family: PingFang-SC-Regular;
  font-size: 0.24rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #000000;
}

.pay {
  padding-left: 0.3rem;
  width: 100%;
  display: flex;
  justify-content: space-between;
  height: 0.65rem;
  margin-top: 0.25rem;

  & > div {
    margin: 0 !important;
  }

  & > div:nth-child(1) {
    flex: 1;
    height: 100%;
    background-color: #f0f0f0;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.48rem;
    letter-spacing: 0rem;
    color: #999999;

    img {
      margin-left: 0.3rem;
      margin-right: 0.15rem;
    }
  }

  & > div:nth-child(2) {
    width: 1rem;
  }

  button {
    width: 100%;
    border: 0;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.65rem;
    letter-spacing: 0rem;
    color: #ffffff;
    /* width: 1.01rem; */
    height: 0.65rem;
    padding: 0;
    background-color: #2ea2fa;
  }
}

.comment {
  position: absolute;
  right: 0.65rem;
  // width: 3.35rem;
  height: 0.8rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  box-sizing: border-box;
  // padding: 0 0.3rem;
  background-color: #2c2c2c;
  border-radius: 0.05rem;

  span {
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    letter-spacing: 0rem;
    color: #ffffff;
    width: 0.8rem;
    padding-left: 0.2rem;
  }

  i {
    width: 0.02rem;
    height: 0.25rem;
    background: #000;
  }
}

.IssuedUp {
  // border-bottom: 0.02rem solid #f9f9f9;
  // height: 1rem;
  display: flex;
  padding-left: 0.5rem;
  justify-content: space-between;
  width: 100%;
  box-sizing: border-box;
  padding: 0 0.3rem;
  padding-left: 0.85rem;

  & > div:first-child {
    display: flex;
    align-items: center;
    font-family: PingFang-SC-Regular;
    font-size: 0.3rem;
    font-weight: normal;
    font-stretch: normal;
    line-height: 0.48rem;
    letter-spacing: 0rem;
    color: #999999;

    & > div {
      margin-left: 0.35rem;
      display: flex;
      align-items: center;
    }

    img {
      width: 0.33rem;
      height: 0.33rem;
      vertical-align: middle;
      margin-right: 0.15rem;
    }
  }

  & > div:last-child {
    display: flex;
    align-items: center;
    position: relative;

    & > img {
      width: 0.4rem;
      height: 0.4rem;
    }
  }
}

.IssuedText {
  padding: 0 0.3rem;
  padding-left: 1.15rem;
  font-family: PingFang-SC-Medium;
  font-size: 0.3rem;
  line-height: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.36rem;
  letter-spacing: 0.01rem;
  color: #333333;
}

.usertype1Head {
  display: flex;
  height: 1.1rem;
  align-items: center;
  padding: 0 0.3rem;

  & > div:first-child {
    width: 0.6rem;
    height: 0.6rem;
    display: flex;

    img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
    }
  }

  & > div:last-child {
    margin-left: 0.25rem;

    p:nth-child(1) {
      font-family: PingFang-SC-Medium;
      font-size: 0.3rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #2ea2fa;
    }

    p:nth-child(2) {
      font-family: PingFang-SC-Regular;
      font-size: 0.24rem;
      font-weight: normal;
      font-stretch: normal;
      letter-spacing: 0rem;
      color: #999999;
    }
  }
}

p {
  margin: 0;
}

.content {
  background: #fff;

  margin-top: 1rem;
}

.titBott {
  box-sizing: border-box;
  padding: 0 0.3rem;
  background: #fff;
  height: 0.86rem;

  & > div {
    display: flex;
    height: 100%;
    padding-left: 1.25rem;

    & > div {
      display: flex;
      align-items: center;
      height: 100%;
      margin-left: 0.3rem;

      span {
        font-size: 0.24rem;
        font-weight: normal;
        font-stretch: normal;
        letter-spacing: 0rem;
        color: #333333;
        padding-left: 0.15rem;
        height: 0.3rem;
        line-height: 0.3rem;
      }

      img {
        vertical-align: middle;
        width: 0.3rem;
        height: 0.3rem;
      }
    }
  }
}

.top {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
  padding-top: 1rem;
  box-sizing: border-box;
  background-size: 100% 100%;
  position: relative;
  // background-color: #fff;
}

.titImg {
  padding: 0 0.3rem;
  height: 0.7rem;
  line-height: 0.7rem;
  font-family: PingFang-SC-Regular;
  font-size: 0.3rem;
  font-weight: normal;
  font-stretch: normal;
  letter-spacing: 0rem;
  color: #ffffff;
  position: relative;

  span {
    margin-left: 1.55rem;
  }

  img {
    position: absolute;
    box-sizing: border-box;
    width: 1.33rem;
    height: 1.33rem;
    border-radius: 50%;
    border: solid 0.05rem rgba(255, 255, 255, 0.3);
    /* opacity: 0.3; */
  }
}

.setBG {
  margin-top: 0.45rem;
  display: flex;
  justify-content: center;
  align-self: center;
}

.titIMG {
  // margin-top: 0.3rem;
  width: 100%;
  box-sizing: border-box;
  display: flex;
  justify-content: flex-end;
  // padding-left: 0 0.47rem;
  // height: 0.43rem;
  display: flex;

  & > div {
    display: flex;
    height: 0.43rem;
    padding: 0.3rem;
  }

  img {
    width: 0.5rem;
    height: 0.43rem;
  }
}

.header {
  // padding-top: 1rem;
  height: 4rem;
  width: 100%;
  box-sizing: border-box;
  background-color: #2c2c2c;
  // position: absolute;
  // position: relative;
  // top: -1rem;
}

/deep/ .van-uploader__wrapper {
  height: 100%;
}

/deep/ .van-uploader__input-wrapper {
  display: flex;
  height: 100%;
  font-family: MicrosoftYaHei;
  font-size: 0.2rem;
  font-weight: normal;
  font-stretch: normal;
  line-height: 0.22rem;
  letter-spacing: 0rem;
  color: rgba(254, 254, 254, 0.6);
  /* opacity: 0.6; */
}

.friends {
  height: 100%;
  overflow-y: Scroll;
  background-color: #f2f2f2;
  -webkit-overflow-scrolling: touch;
}

.friends::-webkit-scrollbar {
  display: none;
}
</style>
