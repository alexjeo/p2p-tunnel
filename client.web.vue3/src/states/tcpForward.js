/*
 * @Author: snltty
 * @Date: 2021-08-21 19:46:50
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-21 19:51:03
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\states\tcpForward.js
 */

import { provide, inject, reactive } from "vue";
import { getTcpForwards } from '../apis/tcp-forward'

const provideTcpForwardKey = Symbol();
export const provideTcpForward = () => {
    const state = reactive({
        connected: false
    });
    provide(provideTcpForwardKey, state);

    const fn = () => {
        getTcpForwards().then((msg) => {
            let list = JSON.parse(msg);
            state.connected = list.filter(c => c.Listening == true).length > 0;
            setTimeout(fn, 100)
        }).catch(() => {
            setTimeout(fn, 1000);
        });
    };
    fn();
}
export const injectTcpForward = () => {
    return inject(provideTcpForwardKey);
}