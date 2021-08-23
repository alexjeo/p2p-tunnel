/*
 * @Author: snltty
 * @Date: 2021-08-19 22:39:45
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-23 16:17:54
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\states\register.js
 */
// import { provide, inject, reactive } from 'vue'

import { provide, inject, reactive } from "vue";
import { getRegisterInfo } from '../apis/register'

const provideRegisterKey = Symbol();
export const provideRegister = () => {
    const state = reactive({
        ClientName: '',
        ClientPort: 0,
        AutoReg: false,
        UseMac: false,
        ClientTcpPort: 0,
        Connected: 0,
        TcpConnected: 0,
        Ip: '',
        Mac: '',
        GroupId: '',
        ConnectId: '',
        IsConnecting: false,
        RouteLevel: 0,
        ServerIp: '',
        ServerPort: 0,
        ServerTcpPort: 0,
    });
    provide(provideRegisterKey, state);

    //定时更新一些不可修改的数据
    const fn = () => {
        //console.time('获取注册信息时间');
        getRegisterInfo().then((msg) => {
            let json = JSON.parse(msg);
            state.Connected = json.Connected;
            state.ClientPort = json.ClientPort;
            state.ClientTcpPort = json.ClientTcpPort;
            state.TcpConnected = json.TcpConnected;
            state.Ip = json.Ip;
            state.Mac = json.Mac;
            state.ConnectId = json.ConnectId;
            state.IsConnecting = json.IsConnecting;
            state.RouteLevel = json.RouteLevel;
            state.Connected = json.Connected;
            if (!state.GroupId) {
                state.GroupId = json.GroupId;
            }
            //console.timeEnd('获取注册信息时间');
            setTimeout(fn, 10)
        }).catch(() => {
            //console.timeEnd('获取注册信息时间');
            setTimeout(fn, 1000);
        });
    };
    fn();
}
export const injectRegister = () => {
    return inject(provideRegisterKey);
}