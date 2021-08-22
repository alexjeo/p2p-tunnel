/*
 * @Author: snltty
 * @Date: 2021-08-19 21:50:16
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-22 12:22:26
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\router\index.js
 */
import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: () => import('../views/Home.vue')
    },
    {
        path: '/register.html',
        name: 'Register',
        component: () => import('../views/Register.vue')
    },
    {
        path: '/upnp.html',
        name: 'UPNP',
        component: () => import('../views/UPNP.vue')
    },
    {
        path: '/tcp-forward.html',
        name: 'TcpForward',
        component: () => import('../views/TcpForward.vue')
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
