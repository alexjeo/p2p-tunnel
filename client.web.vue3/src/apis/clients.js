/*
 * @Author: snltty
 * @Date: 2021-08-21 14:58:34
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-22 13:31:09
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\apis\clients.js
 */
import { sendWebsocketMsg } from "./request";

export const getClients = () => {
    return sendWebsocketMsg(`clients/list`);
}

export const sendClientConnect = (id) => {
    return sendWebsocketMsg(`clients/connect`, { id: id });
}