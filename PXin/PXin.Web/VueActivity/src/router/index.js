import Home from '../views/Home'

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home,
        meta: {
            title: '每月活动',
        },
    },
    {
        path: '/nov/:id',
        name: 'nov',
        component: () => import(/* webpackChunkName: "list" */ '../views/Nov/Index.vue'),
        meta: {
            title: '迪拜见证之旅',
        }
    },
    {
        path: '/list/:id/:activityid',
        name: 'list',
        component: () => import(/* webpackChunkName: "list" */ '../views/Nov/List.vue'),
        meta: {
            title: '迪拜见证之旅',
        }
    }
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
