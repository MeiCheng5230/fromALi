import Home from '@/views/Index'

import {
  setStore,
  getUrlParams,
  getPhoneType,
  isEqual,
  getStore
} from "@/config/utils";
const Record = () => import(/* webpackChunkName: "Record" */"@/views/Record");
const MyRecord = () => import(/* webpackChunkName: "MyRecord" */"@/views/MyRecord");
const Rule = () => import(/* webpackChunkName: "Rule" */"@/views/Rule");
const MyPointA = () => import(/* webpackChunkName: "MyPointA" */"@/views/MypointA");
const Auction = () => import(/* webpackChunkName: "Auction" */"@/views/BidsAuction");
const MarkUp = () => import(/* webpackChunkName: "markUp" */"@/views/AddPrice");
const Detail = () => import(/* webpackChunkName: "Detail" */"@/views/Charts");
const Result = () => import(/* webpackChunkName: "Result" */"@/components/Result");

const router = new VueRouter({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home,
      meta: {
        title: 'A点竞拍',
      },
    },
    {
      path: '/record',
      name: 'record',
      component: Record,
      meta: {
        title: '拍卖记录'
      }
    },
    {
      path: '/myRecord',
      name: 'myRecord',
      component: MyRecord,
      meta: {
        title: '我的竞拍记录'
      }
    },
    {
      path: '/rule',
      name: 'rule',
      component: Rule,
      meta: {
        title: '竞拍规则'
      }
    },
    {
      path: '/myPointA',
      name: 'myPointA',
      component: MyPointA,
      meta: {
        title: '我的A点'
      }
    },
    {
      path: '/auction',
      name: 'auction',
      component: Auction,
      meta: {
        title: 'A点竞拍'
      }
    },
    {
      path: '/markUp',
      name: 'markUp',
      component: MarkUp,
      meta: {
        title: '竞拍加价'
      }
    },
    {
      path: '/detail',
      name: 'detail',
      component: Detail,
      meta: {
        title: '拍卖记录详情'
      }
    },
    {
      path: '/result',
      name: 'result',
      component: Result,
      meta: {
        title: '竞拍成功'
      }
    }
  ]
});

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title;
  }
  if (to.name) {
    let usreInfo = {
      "nodeid": getUrlParams('nodeid'),
      "sid": getUrlParams('sid'),
      "tm": getUrlParams('tm'),
      "sign": getUrlParams('sign'),
      "client": getPhoneType()
    };
    if (!usreInfo.nodeid || !usreInfo.sid || !usreInfo.tm || !usreInfo.sign) {
      next('/404');
      return;
    }
    let userParam = getStore('userParam');

    if (usreInfo && isEqual(JSON.parse(userParam), usreInfo)) { // 第一次验证通过后，存储，如果url参数不变不发请求
      next();
      return;
    }
    let url = GetBaseurl()+'/api/Sys/CheckSign';
    $.ajax({
      type:'post',
      url,
      data:usreInfo,
      success(res) {
        if (res.result > 0) {
          setStore('userParam', usreInfo); //存储
          next();
        } else {
          next('/404'); //验证不通过
        }
      },
      error(err) {
        vant.Toast('签名不正确');
        next('/404');
      }
    })

    // axios.post(url, usreInfo).then(res => {
    //   if (res.data.result > 0) {
    //     setStore('userParam', usreInfo); //存储
    //     next();
    //   } else {
    //     next('/404'); //验证不通过
    //   }
    // }).catch(err => { //验证不通过 报错
    //   vant.Toast('签名不正确');
    //   next('/404');
    // })

  } else {
    next();
  }
});

export default router
