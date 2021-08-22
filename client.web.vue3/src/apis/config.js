/*
 * @Author: snltty
 * @Date: 2021-08-20 11:00:08
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-20 11:01:34
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\apis\config.js
 */
import { sendWebsocketMsg } from "./request";

export const sendConfigMsg = (config) => {
    return sendWebsocketMsg(`config/update`, config);
}