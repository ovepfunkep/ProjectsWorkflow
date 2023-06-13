import { createRouter, createWebHistory } from 'vue-router'
import index from '../components/index.vue';

const routes = [
    {
        path: '',
        name: 'index',
        component: index
    },
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    mode: 'history',
    routes,

})

export default router