import App from './App'
import router from './router'
import global from './global';

const vueKeyboard=()=>import('vue-keyboard-su-link/dist/vueKeyboard');
Vue.use(vueKeyboard)
FastClick.attach(document.body)


Vue.config.productionTip = false;
Vue.prototype.$global = global;

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,

  render: h => h(App)
});
