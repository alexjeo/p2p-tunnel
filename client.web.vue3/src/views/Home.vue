<!--
 * @Author: snltty
 * @Date: 2021-08-19 21:50:16
 * @LastEditors: snltty
 * @LastEditTime: 2021-08-23 15:03:29
 * @version: v1.0.0
 * @Descripttion: 功能说明
 * @FilePath: \client.web.vue3\src\views\Home.vue
-->
<template>
    <div class="home">
        <el-table :data="clients" border size="mini">
            <el-table-column prop="Name" label="客户端"></el-table-column>
            <el-table-column prop="UDP" label="UDP" width="80">
                <template #default="scope">
                    <el-switch @click.stop v-model="scope.row.Connected"></el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="TCP" label="TCP" width="80">
                <template #default="scope">
                    <el-switch @click.stop v-model="scope.row.TcpConnected"></el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="todo" label="操作" width="100" fixed="right" class="t-c">
                <template #default="scope">
                    <el-button :disabled="scope.row.TcpConnected" :loading="scope.row.Connecting" size="mini" @click="handleConnect(scope.row)">连接</el-button>
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>

<script>
import { toRefs } from '@vue/reactivity';
import { injectClients } from '../states/clients'
import { sendClientConnect } from '../apis/clients'
export default {
    name: 'Home',
    components: {},
    setup () {
        const clientsState = injectClients();

        const handleConnect = (row) => {
            sendClientConnect(row.Id);
        }

        return {
            ...toRefs(clientsState), handleConnect
        }

    }
}
</script>
<style lang="stylus" scoped>
.home
    padding: 2rem;
</style>