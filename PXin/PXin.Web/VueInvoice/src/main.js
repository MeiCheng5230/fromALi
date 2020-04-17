import App from './App.vue'
import router from './router'
import store from './store'
import global  from './global'
import 'amfe-flexible';
if ('addEventListener' in document) {
    document.addEventListener(
        'DOMContentLoaded',
        function () {
            FastClick.attach(document.body)
        },
        false
    )
}

Vue.config.productionTip = false;
Vue.prototype.GLOBAL = global;
new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')
