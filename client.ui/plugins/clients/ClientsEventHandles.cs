using client.ui.events;
using common.extends;
using server.model;
using server.models;
using System;
using System.Net.Sockets;

namespace client.ui.plugins.clients
{
    public class ClientsEventHandles
    {
        private static readonly Lazy<ClientsEventHandles> lazy = new Lazy<ClientsEventHandles>(() => new ClientsEventHandles());
        public static ClientsEventHandles Instance => lazy.Value;

        private ClientsEventHandles()
        {

        }

        /// <summary>
        /// 客户端被移除 下线了
        /// </summary>
        public event EventHandler<ClientInfo> OnCurrentClientChangeHandler;
        /// <summary>
        /// 客户端被移除
        /// </summary>
        /// <param name="arg"></param>
        public void OnCurrentClientChange(ClientInfo client)
        {
            OnCurrentClientChangeHandler?.Invoke(this, client);
        }

        /// <summary>
        /// 客户端被移除 下线了
        /// </summary>
        public event EventHandler<long> OnClientRemoveHandler;
        /// <summary>
        /// 客户端被移除
        /// </summary>
        /// <param name="arg"></param>
        public void OnClientRemove(long id)
        {
            OnClientRemoveHandler?.Invoke(this, id);
        }

        /// <summary>
        /// 服务器发来的客户端列表数据
        /// </summary>
        public event EventHandler<OnServerSendClientsEventArg> OnServerSendClientsHandler;
        /// <summary>
        /// 服务器发来的客户端列表数据
        /// </summary>
        /// <param name="arg"></param>
        public void OnServerSendClients(OnServerSendClientsEventArg arg)
        {
            OnServerSendClientsHandler?.Invoke(this, arg);
        }
    }

    public class OnServerSendClientsEventArg : EventArgs
    {
        public PluginExcuteModel Packet { get; set; }
        public MessageClientsModel Data { get; set; }
    }

}
