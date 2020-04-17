// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from "vue";
import App from "./App";
import router from "./router";
import { Toast, Dialog } from "vant";
import global from '@/config/global';
import "signalr";
Vue.config.productionTip = false;

import Vant from "vant";
import VConsole from "vconsole";
import "vant/lib/index.css";
Vue.use(Vant);
Vue.prototype.$toast = Toast;
Vue.prototype.$dialog = Dialog;
Vue.prototype.$global = global;
import VueI18n from "vue-i18n";
Vue.use(VueI18n);

//事件总线
Vue.prototype.$eventBus = new Vue();

var vconsole = new VConsole();

import FastClick from "fastclick";
import Video from "video.js";
import "video.js/dist/video-js.css";
Vue.prototype.$video = Video;

FastClick.prototype.focus = function(targetElement) {
  var length;

  var deviceIsIOS = !!navigator.userAgent.match(
    /\(i[^;]+;( U;)? CPU.+Mac OS X/
  ); //ios终端
  // Issue #160: on iOS 7, some input elements (e.g. date datetime month) throw a vague TypeError on setSelectionRange. These elements don't have an integer value for the selectionStart and selectionEnd properties, but unfortunately that can't be used for detection because accessing the properties also throws a TypeError. Just check the type instead. Filed as Apple bug #15122724.

  if (
    deviceIsIOS &&
    targetElement.setSelectionRange &&
    targetElement.type.indexOf("date") !== 0 &&
    targetElement.type !== "time" &&
    targetElement.type !== "month"
  ) {
    length = targetElement.value.length;

    targetElement.focus();

    targetElement.setSelectionRange(length, length);
  } else {
    targetElement.focus();
  }
};

FastClick.attach(document.body);

import vueKeyboard from "vue-keyboard-su-link/dist/vueKeyboard";
Vue.use(vueKeyboard);

const i18n = new VueI18n({
  //locale: 'en-US',
  locale: "zh-CN",
  messages: {
    "zh-CN": require("@/components/lang/zh"), // 中文语言包
    "zh-F": require("@/components/lang/F"), // 繁体语言包
    "en-US": require("@/components/lang/en") // 英文语言包
  }
});

/* eslint-disable no-new */
new Vue({
  el: "#app",
  i18n,
  router,
  components: { App },
  template: "<App/>"
});
