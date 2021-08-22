using client.ui.events;
using client.ui.plugins.rdpDesktop;
using client.ui.plugins.request;
using common;
using common.extends;
using server.model;
using server.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace client.ui.plugins.rdpDesktop
{
    public class RdpShareEventHandles
    {
        private static readonly Lazy<RdpShareEventHandles> lazy = new Lazy<RdpShareEventHandles>(() => new RdpShareEventHandles());
        public static RdpShareEventHandles Instance => lazy.Value;

        private RdpShareEventHandles()
        {

        }

        //发起请求
        public void GetRdpShareInfo(Socket socket, Action<RdpShareRequestResultMessage> callback, Action<TcpRequestMessageFailModel> failCallback)
        {
            RequestEventHandlers.Instance.SendTcpRequestMsessage(socket, new RdpShareRequestExcuteMessage(),
            (result) =>
            {
                callback(result.Data.ProtobufDeserialize<RdpShareRequestResultMessage>());
            }, failCallback);
        }

        public void SendClientConnectString(Socket socket, string clientConnectString)
        {
            RequestEventHandlers.Instance.SendTcpRequestMsessage(socket, new RdpShareRequestReplyMessage
            {
                ConnectString = clientConnectString
            });
        }
    }
}
