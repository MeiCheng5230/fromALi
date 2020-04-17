import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

const router = new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/open/home.vue'),
      meta: {
        title: '相信达人'
      }
    },{
      path: '/info',
      name: 'info',
      component: () => import('@/views/info/index.vue'),
      meta: {
        title: '填写资料'
      }
    },{
      path: '/open',
      name: 'open',
      component: () => import('@/views/open/index.vue'),
      meta: {
        title: '成为达人'
      }
    },{
      path: '/agreement',
      name: 'agreement',
      component: () => import('@/views/open/agreement.vue'),
      meta: {
        title: '相信达人协议'
      }
    },{
      path: '/signature',
      name: 'signature',
      component: () => import('@/views/info/signature.vue'),
      meta: {
        title: '签名'
      }
    },{
      path: '/introduce',
      name: 'introduce',
      component: () => import('@/views/info/introduce.vue'),
      meta: {
        title: '自我介绍'
      }
    },{
      path: '/remarks',
      name: 'remarks',
      component: () => import('@/views/info/remarks.vue'),
      meta: {
        title: '欢迎语设置'
      }
    },{
      path: '/education',
      name: 'education',
      component: () => import('@/views/info/education.vue'),
      meta: {
        title: '教育背景'
      }
    },{
      path: '/vocation',
      name: 'vocation',
      component: () => import('@/views/info/vocation.vue'),
      meta: {
        title: '职业背景'
      }
    },{
      path: '/filleducation',
      name: 'filleducation',
      component: () => import('@/views/info/fillEducation.vue'),
      meta: {
        title: '教育背景'
      }
    },{
      path: '/fillvocation',
      name: 'fillvocation',
      component: () => import('@/views/info/fillVocation.vue'),
      meta: {
        title: '职业背景'
      }
    },{
      path: '/explain',
      name: 'explain',
      component: () => import('@/views/info/explain.vue'),
      meta: {
        title: '达人达语'
      }
    },{
      path: '/sort',
      name: 'sort',
      component: () => import('@/views/info/sort.vue'),
      meta: {
        title: '专业领域'
      }
    },{
      path: '/identify',
      name: 'identify',
      component: () => import('@/views/info/identify.vue'),
      meta: {
        title: '专业资格认证'
      }
    },{
      path: '/hasidentify',
      name: 'hasidentify',
      component: () => import('@/views/info/hasIdentify.vue'),
      meta: {
        title: '实名认证信息'
      }
    },


    {
      path: '/knowlege',
      name: 'knowlege',
      component: () => import('@/views/knowledge/index.vue'),
      meta: {
        title: '知识库'
      }
    },{
      path: '/detail',
      name: 'detail',
      component: () => import('@/views/knowledge/detail.vue'),
      meta: {
        title: '问答详情'
      }
    },{
      path: '/additional',
      name: 'additional',
      component: () => import('@/views/knowledge/additional.vue'),
      meta: {
        title: '添加问答'
      }
    },{
      path: '/video',
      name: 'video',
      component: () => import('@/views/knowledge/video.vue'),
      meta: {
        title: '我的视频'
      }
    }
    
    
  ]
});

export default router;

router.beforeEach((to, from, next) => {
  if(to.meta.title){
    document.title=to.meta.title;
  };
  if(to.name=="home" && from.name=="info") {
    try {
      AppNative.blJsTunedupNativeWithTypeParamSign(1001, '', '');
    } catch (error) {
      this.Toast.fail("请在相信App中关闭");
    }
  } else if(to.name=="home" && from.name=="open") {
    try {
      AppNative.blJsTunedupNativeWithTypeParamSign(1001, '', '');
    } catch (error) {
      this.Toast.fail("请在相信App中关闭");
    }
  } 
  next();
})
