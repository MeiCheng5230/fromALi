import Home from '../views/Home'

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home,
        meta: {
            title: '申请开发票',
        }
    }, {
        path: '/add',
        name: 'nov',
        component: () => import(/* webpackChunkName: "add" */ '../views/Add.vue'),
        meta: {
            title: '增票资质',
        }
    }, {
        path: '/apply',
        name: 'apply',
        component: () => import(/* webpackChunkName: "apply" */ '../views/Apply.vue'),
        meta: {
            title: '申请开发票',
        }
    }, {
        path: '/detail',
        name: 'detail',
        component: () => import(/* webpackChunkName: "detail" */ '../views/Detail.vue'),
        meta: {
            title: '发票详情',
        }
    }, {
        path: '/result',
        name: 'result',
        component: () => import(/* webpackChunkName: "result" */ '../views/Result.vue'),
        meta: {
            title: '提交成功',
        }
    }, 

]

const router = new VueRouter({
    base: '/app/',
    scrollBehavior: () => ({y: 0}),
    routes,
});

router.beforeEach((to, from, next) => {
    if (to.meta.title) {
        document.title = to.meta.title;
    }
    next()
});

export default router
