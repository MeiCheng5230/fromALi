import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router);

const router = new Router({
    routes: [
        {
            path: '/',
            component: () => import('@/components/redLottery'),
        },{
            path: '/record',
            name: 'record',
            component: () => import('@/components/HisRecord'),
        }
    ]
});

export default router;
