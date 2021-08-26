<!--
 * @Author: snltty
 * @Date: 2021-08-19 21:50:16
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-26 09:36:31
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\views\Home.vue
-->
<template>
    <div class="home">
        <el-table :data="clients" border size="mini">
            <el-table-column prop="Name" label="客户端"></el-table-column>
            <el-table-column prop="Mac" label="Mac"></el-table-column>
            <el-table-column prop="UDP" label="UDP" width="80">
                <template #default="scope">
                    <el-switch disabled @click.stop v-model="scope.row.Connected"></el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="TCP" label="TCP" width="80">
                <template #default="scope">
                    <el-switch disabled @click.stop v-model="scope.row.TcpConnected"></el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="todo" label="操作" width="200" fixed="right" class="t-c">
                <template #default="scope">
                    <div class="t-c">
                        <el-button :disabled="scope.row.Connected && scope.row.TcpConnected" :loading="scope.row.Connecting || scope.row.TcpConnected" size="mini" @click="handleConnect(scope.row)">连接</el-button>
                        <el-button :loading="scope.row.Connecting || scope.row.TcpConnecting" size="mini" @click="handleReset(scope.row)">重启它</el-button>
                    </div>
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>

<script>
import { toRefs } from '@vue/reactivity';
import { injectClients } from '../states/clients'
import { sendClientConnect } from '../apis/clients'
import { sendReset } from '../apis/reset'
export default {
    name: 'Home',
    components: {},
    setup () {
        const clientsState = injectClients();

        const handleConnect = (row) => {
            sendClientConnect(row.Id);
        }
        const handleReset = (row) => {
            sendReset(row.Id);
        }

        return {
            ...toRefs(clientsState), handleConnect, handleReset
        }

    }
}
</script>
<style lang="stylus" scoped>
.home
    padding: 2rem;
</style>