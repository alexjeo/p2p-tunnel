/*
 * @Author: snltty
 * @Date: 2021-08-22 00:28:31
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-22 13:38:10
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\vue.config.js
 */
module.exports = {
    productionSourceMap: process.env.NODE_ENV === 'production' ? false : true,
    outputDir: '../client.service/web',
    publicPath: './',
    parallel: false,
    assetsDir: './'
}