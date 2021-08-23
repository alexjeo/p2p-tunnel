/*
 * @Author: snltty
 * @Date: 2021-08-20 16:06:04
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-23 16:52:30
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\apis\wakeup.js
 */
import { sendWebsocketMsg } from "./request";

export const sendWakeUp = (msg) => {
    return sendWebsocketMsg(`wakeup/wakeup`, msg);
}