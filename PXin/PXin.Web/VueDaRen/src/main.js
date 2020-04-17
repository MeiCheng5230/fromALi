// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import global from '@/config/global.js'
import VConsole from 'vconsole'
import { Toast } from 'vant'
import { FiltersPrice, getStore } from "@/config/utils"

Vue.filter('FiltersPrice', FiltersPrice)
Vue.use(Toast)

var bus = new Vue();

Vue.config.productionTip = false
Vue.prototype.$global = global
Vue.prototype.Toast = Toast
Vue.prototype.bus = bus
Vue.prototype.getStore = getStore

router.afterEach((to, from, next) => { //解决跳转高度问题
  window.scrollTo(0, 0)
});

// if (process.env.NODE_ENV !== 'production') {
//   new VConsole()
// } else {
//   if(process.env.type=="test"){
//     new VConsole()
//   }
// }
window.nativeAppEnterBackground = function() {
  // alert('程序进入后台');
};

window.nativeAppEnterForeground = function() {
  // alert('程序进入前台');
};

window.nativeAppTerminate = function() {
  // alert('销毁');
};

window.SetPlayVideo = function(dom) {
  // 播放视频
  let video = dom.getAttribute('data-videosrc');
  let param = window.btoa('{"videoUrlStr":"'+video+'","firstImageBase":""}');   // 参数base64加密
  let sign = hex_md5(param+'F59E5087-DC84-451A-9B74-C854EE0A952B');   // MD5加密
  try {
    AppNative.jsTunedupNativeWithTypeParamSign(1023, param, sign);
  } catch (error) {
    this.Toast.fail(err);
  }
};

window.nativeCloseAction = function() {
  // 点击顶部关闭
  let audio = document.querySelectorAll("audio");
  for (const item of audio) {
    item.pause();
  }
};

// 按需引入ui组件
import { Icon, NoticeBar, DatetimePicker, Popup, Picker, Uploader, Dialog, Progress, List, Switch } from 'vant';
Vue.use(Icon).use(DatetimePicker).use(Popup).use(Picker).use(Uploader).use(Dialog).use(NoticeBar).use(Progress).use(List).use(Switch);


/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
