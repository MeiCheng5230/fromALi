<template>
  <div id="password" class="password" v-show="isPay">
    <div class="showBox">
      <p class="title">支付金额</p>
      <h1>{{payamount}}</h1>
      <div class="passwordBox">
        <ul>
          <li>
            <div class="point hide"></div>
          </li>
          <li>
            <div class="point hide"></div>
          </li>
          <li>
            <div class="point hide"></div>
          </li>
          <li>
            <div class="point hide"></div>
          </li>
          <li>
            <div class="point hide"></div>
          </li>
          <li>
            <div class="point hide"></div>
          </li>
        </ul>
      </div>
    </div>
    <div id="keyBox" class="keyBox">
      <ul @click="handleKeyPress($event)">
        <li>
          <p data-num="1">1</p>
          <p data-num="2">2</p>
          <p data-num="3">3</p>
        </li>
        <li>
          <p data-num="4">4</p>
          <p data-num="5">5</p>
          <p data-num="6">6</p>
        </li>
        <li>
          <p data-num="7">7</p>
          <p data-num="8">8</p>
          <p data-num="9">9</p>
        </li>
        <li>
          <p data-num="10">取消</p>
          <p data-num="0">0</p>
          <p data-num="11">
            <span class="off" data-num="11"></span>
          </p>
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
export default {

  props:['isPay','payamount'],
  data() {
    return {
        password: []
    };
  },

  methods: {
    handleKeyPress: function(e) {
      var point = document.getElementsByClassName("point");
      let num = e.target.dataset.num;
      if (num == "10") {
        //取消键盘
        this.password = []; //清空密码
        //his.isPay = false;
        this.$emit('fatherHide');
        for (let j = 0; j < 6; j++) {
          //密码填充框样式改变
          point[j].className += " hide";
        }
      } else if (num == "11") {
        //密码删除一个

        this.password.splice(-1, 1);
        let j = this.password.length;
        point[j].className += " hide";
      } else {
        //添加密码
        this.password.push(num); //添加密码
        if (this.password.length >= 6) {
          //回调确认
          this.$emit('fatherConfirm',this.password.join(""));
          //密码大于6位时
          this.password = []; //清空密码
          for (let j = 0; j < 6; j++) {
            //密码填充框样式改变
            point[j].className += " hide";
          }
        } else {
          for (let i = 0; i < this.password.length; i++) {
            //密码填充框样式改变
            point[i].className = "point";
          }
        }
      }
      console.log(this.password)
    }
  }
};
</script>

<style lang="scss" scoped>
a, img, button, input, textarea, div {
  -webkit-tap-highlight-color: rgba(255,255,255,0);
}
.password {
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  position: fixed;
  top: 0;
  color: #000;
  z-index: 999;
}
.showBox {
  background: #fff;
}
.keyBox {
  width: 100%;
  background: #fff;
  position: fixed;
  bottom: 0;
}
.keyBox ul {
  padding: 0;
  margin: 0;
}
.keyBox ul li {
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
  border-bottom: 1px solid #f1f1f1;
}
.keyBox ul li:nth-of-type(4) {
  border: none;
}
.keyBox ul li:nth-of-type(4) p {
  width: 33%;
}
.keyBox ul li p {
  height: 100%;
  flex: auto;
  padding: 0.3rem 0;
  margin: 0;
  text-align: center;
  border-right: 1px solid #f1f1f1;
}
.password .keyBox ul li .off {
  display: inline-block;
  width: 0.4rem;
  height: 0.03rem;
  background: #999;
  line-height: 0;
  font-size: 0;
  vertical-align: middle;
  transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  -moz-transform: rotate(45deg);
  -webkit-transform: rotate(45deg);
  -o-transform: rotate(45deg);
}

.off:after {
  content: "/";
  display: block;
  width: 0.4rem;
  height: 0.03rem;
  background: #999;
  transform: rotate(-90deg);
  -ms-transform: rotate(-90deg);
  -moz-transform: rotate(-90deg);
  -webkit-transform: rotate(-90deg);
  -o-transform: rotate(-90deg);
}
.password .showBox {
  width: 7.1rem;
  height: 5rem;
  background: #fff;
  margin: 1.4rem auto 0 auto;
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-flex-direction: column;
  -moz-flex-direction: column;
  -ms-flex-direction: column;
  -o-flex-direction: column;
  flex-direction: column;
  -webkit-box-align: center;
  -webkit-align-items: center;
  -moz-align-items: center;
  -ms-align-items: center;
  -o-align-items: center;
  align-items: center;
}

.password .showBox .passwordBox ul {
  padding: 0;
  margin: 0;
}
.password .showBox .title {
  font-size: 0.22rem;
  text-align: center;
  padding-top: 0.22rem;
  color: #afafaf;
}

.password .showBox h1 {
  font-size: 0.36rem;
  text-align: center;
  font-weight: normal;
}

.password .showBox .surplusBox {
  height: 0.64rem;
  width: 6.18rem;
  font-size: 0.28rem;
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
  -webkit-box-align: center;
  -webkit-align-items: center;
  -moz-align-items: center;
  -ms-align-items: center;
  -o-align-items: center;
  align-items: center;
  position: relative;
  margin-top: 0.78rem;
}

.password .showBox .surplusBox img {
  width: 0.64rem;
  height: 0.64rem;
}

.password .showBox .surplusBox span {
  margin-left: 0.26rem;
}

.password .showBox .surplusBox .choose {
  position: absolute;
  right: 0;
  width: 0.17rem;
  height: 0.29rem;
}

.password .showBox .surplusBox .surplus {
  color: #a3a3a3;
}

.password .showBox .passwordBox {
  height: 0.98rem;
  width: 6.18rem;
  font-size: 0.28rem;
  margin: 0.4rem 0;
  border-bottom: 1px solid #cfcfcf;
  border-right: 1px solid #cfcfcf;
}

.password .showBox .passwordBox ul {
  width: 100%;
  height: 100%;
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
}

.password .showBox .passwordBox ul li {
  height: 100%;
  flex: auto;
  list-style: none;
  border-left: 1px solid #e1e1e1;
  background: #f1f1f1;
  display: -webkit-box;
  display: -moz-box;
  display: -ms-flexbox;
  display: -webkit-flex;
  display: flex;
  -webkit-box-pack: center;
  -webkit-justify-content: center;
  -moz-justify-content: center;
  -ms-justify-content: center;
  -o-justify-content: center;
  justify-content: center;
  -webkit-box-align: center;
  -webkit-align-items: center;
  -moz-align-items: center;
  -ms-align-items: center;
  -o-align-items: center;
  align-items: center;
}

.password .showBox .passwordBox ul li div {
  width: 0.2rem;
  height: 0.2rem;
  background: #000;
  border-radius: 50%;
}
.hide {
  visibility: hidden;
}
</style>
