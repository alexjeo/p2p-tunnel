/*
 * @Author: snltty
 * @Date: 2021-08-21 14:58:34
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-24 15:47:14
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\apis\reset.js
 */
import { sendWebsocketMsg } from "./request";
export const sendReset = (id) => {
    return sendWebsocketMsg(`reset/reset`, { id: id });
}