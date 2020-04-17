import Vue from 'vue'
import Router from 'vue-router'
import {
  getUrlParams,
} from "@/config/utils";
Vue.use(Router)
const router = new Router({
  routes: [
    {
      path: '/',
      name: 'register',
      component: () => import(/* webpackChunkName: "reg" */ '@/components/register')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import(/* webpackChunkName: "register" */ '@/components/register')
    },
    {
      path: '/succReg',
      name: 'succReg',
      component: () => import(/* webpackChunkName: "succReg" */ '@/components/succReg')
    },
    {
      path: '/region',
      name: 'region',
      component: () => import(/* webpackChunkName: "region" */ '@/components/region')
    },
  ]
})


export default router;
