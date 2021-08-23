<!--
 * @Author: snltty
 * @Date: 2021-08-19 22:30:19
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-23 16:27:09
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\views\Register.vue
-->
<template>
    <div class="register-form">
        <el-form label-width="8rem">
            <el-form-item label="" label-width="0">
                <el-row>
                    <el-col :span="12">
                        <el-form-item label="客户名称">
                            <el-input v-model="ClientName" maxlength="32" show-word-limit></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item label="分组编号">
                            <el-input v-model="GroupId" maxlength="32" show-word-limit></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="服务信息">
                <el-row>
                    <el-col :span="8">
                        <el-form-item label="IP" label-width="50">
                            <el-input v-model="ServerIp"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="UDP端口">
                            <el-input v-model="ServerPort"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="TCP端口">
                            <el-input v-model="ServerTcpPort"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="客户信息">
                <el-row>
                    <el-col :span="8">
                        <el-form-item label="IP" label-width="50">
                            <el-input readonly v-model="Ip"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="UDP端口">
                            <el-input readonly v-model="ClientPort"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="TCP端口">
                            <el-input readonly v-model="ClientTcpPort"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="注册信息">
                <el-row>
                    <el-col :span="8">
                        <el-form-item label="ID" label-width="50">
                            <el-input readonly v-model="ConnectId"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="外网距离">
                            <el-input readonly v-model="RouteLevel"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="Mac地址">
                <el-row>
                    <el-col :span="8">
                        <el-form-item label="地址" label-width="50">
                            <el-input readonly v-model="Mac"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="上报mac">
                            <el-switch v-model="UseMac" />
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="注册状态">
                <el-row>
                    <el-col :span="8">
                        <el-form-item label="UDP">
                            <el-switch disabled v-model="Connected" />
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="TCP">
                            <el-switch disabled v-model="TcpConnected" />
                        </el-form-item>
                    </el-col>
                    <el-col :span="8">
                        <el-form-item label="自动注册">
                            <el-switch v-model="AutoReg" />
                        </el-form-item>
                    </el-col>
                </el-row>
            </el-form-item>
            <el-form-item label="" label-width="0" class="t-c">
                <el-button type="primary" :loading="IsConnecting" @click="handleSubmit">注册</el-button>
            </el-form-item>
            <el-form-item label="" label-width="0">
                <el-alert title="分组编号" type="info" show-icon :closable="false">
                    <p style="line-height:2rem">1、当【分组编号】不为空时，注册才会保存【分组编号】，以便于下次使用相同【分组编号】</p>
                    <p style="line-height:2rem">2、相同【分组编号】直接的客户端可见，请尽量设置一个重复可能性最小的值</p>
                </el-alert>
                <el-alert title="客户名称" type="info" show-icon :closable="false">
                    <p style="line-height:2rem">1、TCP转发时，将【客户名称】作为目标客户端的标识符，请尽量各客户端之间不重名</p>
                </el-alert>
            </el-form-item>
        </el-form>
    </div>
</template>

<script>
import { ref, toRefs } from '@vue/reactivity';
import { injectRegister } from '../states/register'
import { sendRegisterMsg, getRegisterInfo } from '../apis/register'
import { sendConfigMsg } from '../apis/config'

import { ElMessage } from 'element-plus'
export default {
    setup () {
        const formDom = ref(null);
        const registerState = injectRegister();

        //获取一下可修改的数据
        getRegisterInfo().then((msg) => {
            let json = JSON.parse(msg);
            registerState.ClientName = json.ClientName;
            registerState.ServerIp = json.ServerIp;
            registerState.ServerPort = json.ServerPort;
            registerState.ServerTcpPort = json.ServerTcpPort;
            registerState.GroupId = json.GroupId;
            registerState.AutoReg = json.AutoReg;
            registerState.UseMac = json.UseMac;

        }).catch((msg) => {
            ElMessage.error(msg);
        });

        const handleSubmit = () => {
            registerState.IsConnecting = true;
            sendConfigMsg({
                ClientName: registerState.ClientName,
                ServerIp: registerState.ServerIp,
                ServerPort: registerState.ServerPort,
                ServerTcpPort: registerState.ServerTcpPort,
                GroupId: registerState.GroupId,
                AutoReg: registerState.AutoReg,
                UseMac: registerState.UseMac,
            }).then(() => {
                sendRegisterMsg().then((res) => {
                }).catch((msg) => {
                    ElMessage.error(msg);
                });
            }).catch((msg) => {
                ElMessage.error(msg);
            })
        }

        return {
            ...toRefs(registerState), formDom, handleSubmit
        }
    }
}
</script>

<style lang="stylus" scoped>
.register-form
    padding: 5rem;
</style>