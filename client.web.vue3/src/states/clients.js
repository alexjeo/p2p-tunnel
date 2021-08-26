/*
 * @Author: snltty
 * @Date: 2021-08-21 14:57:33
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-26 09:35:45
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\states\clients.js
 */
import { provide, inject, reactive } from "vue";
import { getClients } from '../apis/clients'

const provideClientsKey = Symbol();
export const provideClients = () => {
    const state = reactive({
        clients: [{}]
    });
    provide(provideClientsKey, state);

    //定时更新一些不可修改的数据
    const fn = () => {
        getClients().then((msg) => {
            //state.clients = JSON.parse(msg);
            setTimeout(fn, 10)
        }).catch(() => {
            setTimeout(fn, 1000);
        });
    };
    fn();
}
export const injectClients = () => {
    return inject(provideClientsKey);
}