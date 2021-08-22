/*
 * @Author: snltty
 * @Date: 2021-08-20 09:12:44
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-22 12:19:30
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\main.js
 */
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import './assets/style.css'
import './extends/index'


import ElementPlus from 'element-plus';
import 'element-plus/lib/theme-chalk/index.css';

createApp(App).use(ElementPlus).use(router).mount('#app');
