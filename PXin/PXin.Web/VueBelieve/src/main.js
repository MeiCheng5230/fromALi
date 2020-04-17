// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
// import Vue from 'vue';
import App from './App';
import router from './router';
// import axios from 'axios';
import Vant from 'vant';
// import Vuex from 'vuex';
// import FastClick from 'fastclick'
import global from '@/config/global';
import 'vant/lib/index.css';
import {Toast} from 'vant';
import {Dialog} from 'vant';
import vueKeyboard from 'vue-keyboard-su-link/dist/vueKeyboard'
import VueI18n from 'vue-i18n'




Vue.use(Vant).use(Vuex).use(vueKeyboard).use(Toast).use(VueI18n).use(Dialog);
const i18n = new VueI18n({
  locale: 'zh', // 语言标识 //this.$i18n.locale // 通过切换locale的值来实现语言切换
  messages: {
    'chs': require('./lang/lang-zh'),  //
    'en': require('./lang/lang-en'),
    'cht': require('./lang/lang-zh-hant'),
  }
})



if ('addEventListener' in document) {
  document.addEventListener('DOMContentLoaded', function () {
    FastClick.attach(document.body);
  }, false);
}
Vue.prototype.$global = global;
Vue.prototype.axios = axios;
Vue.prototype.Toast = Toast;
Vue.prototype.Dialog = Dialog;
Vue.prototype.i18n = i18n;
Vue.config.productionTip = false
router.afterEach((to, from, next) => { //解决跳转高度问题
  window.scrollTo(0, 0)
});

let store = new Vuex.Store({
  issale: false
});

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  i18n,
  components: {App},
  template: '<App/>'
})
