// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Axios from 'axios'
import MintUI from 'mint-ui'
import global from '@/config/global';
import 'mint-ui/lib/style.css'
Vue.use(MintUI);

Vue.prototype.axios=Axios;
Vue.prototype.Mint = MintUI;
Vue.prototype.$global = global;
Vue.config.productionTip = false;


//请求拦截器
Axios.interceptors.request.use((config) => {

  MintUI.Indicator.open('加载中...');
  return config;
}, (err) => {
  return Promise.reject(err)
})
//响应拦截器
Axios.interceptors.response.use((response) => {
  MintUI.Indicator.close(); //关闭loading
  return response;
}, (err) => {
  return Promise.reject(err);
});



/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})
