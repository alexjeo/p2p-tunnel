using client.ui.events;
using client.ui.plugins.p2pMessage;
using common;
using common.extends;
using server.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace client.ui.plugins.chat
{
    public class P2PChatEventHandles
    {
        private static readonly Lazy<P2PChatEventHandles> lazy = new Lazy<P2PChatEventHandles>(() => new P2PChatEventHandles());
        public static P2PChatEventHandles Instance => lazy.Value;
        private readonly Dictionary<P2PChatTypes, IP2PChatPlugin[]> plugins = null;

        private P2PChatEventHandles()
        {
            plugins = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(c => c.GetTypes())
                .Where(c => c.GetInterfaces().Contains(typeof(IP2PChatPlugin)))
                .Select(c => (IP2PChatPlugin)Activator.CreateInstance(c)).GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public event EventHandler<P2PChatModel> OnTcpChatMessageHandler;
        public void OnTcpChatMessage(P2PChatModel arg)
        {
            P2PChatTypes type = (P2PChatTypes)arg.ChatType;

            if (plugins.ContainsKey(type))
            {
                IP2PChatPlugin[] plugin = plugins[type];
                if (plugin.Length > 0)
                {
                    for (int i = 0; i < plugin.Length; i++)
                    {
                        plugin[i].Excute(arg);
                    }
                }
            }

            OnTcpChatMessageHandler?.Invoke(this, arg);
        }


        #region 纯文本

        public event EventHandler<P2PChatTextReceiveModel> OnTcpChatTextMessageHandler;
        public void OnTcpChatTextMessage(P2PChatTextReceiveModel arg)
        {
            OnTcpChatTextMessageHandler?.Invoke(this, arg);
        }
        public event EventHandler<SendTcpChatMessageEventArg<P2PChatTextModel>> OnSendTcpChatTextMessageHandler;
        public void SendTcpChatTextMessage(SendTcpChatMessageEventArg<P2PChatTextModel> arg)
        {
            P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
            {
                Socket = arg.Socket,
                Data = new P2PChatModel
                {
                    ChatType = (byte)arg.Data.ChatType,
                    FormId = EventHandlers.Instance.ConnectId,
                    ToId = arg.ToId,
                    Data = arg.Data.ProtobufSerialize()
                }
            });
            OnSendTcpChatTextMessageHandler?.Invoke(this, arg);
        }
        #endregion


        #region 文件

        public event EventHandler<P2PChatFileReceiveModel> OnTcpChatFileMessageHandler;
        public void OnTcpChatFileMessage(P2PChatFileReceiveModel arg)
        {
            OnTcpChatFileMessageHandler?.Invoke(this, arg);
        }
        public event EventHandler<SendTcpChatMessageEventArg<P2PChatFileModel>> OnSendTcpChatFileMessageHandler;
        public void SendTcpChatFileMessage(SendTcpChatMessageEventArg<P2PChatFileModel> arg, FileInfo file = null)
        {
            if (file == null)
            {
                _SendTcpChatFileMessage(arg);
                OnSendTcpChatFileMessageHandler?.Invoke(this, arg);
            }
            else
            {
                //文件分包传输
                Task.Run(() =>
                {
                    try
                    {
                        int packSize = 1024; //每个包大小 

                        int packCount = (int)(arg.Data.Size / packSize);
                        long lastPackSize = arg.Data.Size - packCount * packSize;
                        int index = 0;
                        using FileStream fs = file.OpenRead();
                        for (index = 0; index < packCount; index++)
                        {
                            byte[] data = new byte[packSize];
                            fs.Read(data, 0, packSize);

                            arg.Data.Data = data;
                           _SendTcpChatFileMessage(arg);
                        }
                        if (lastPackSize > 0)
                        {
                            byte[] data = new byte[lastPackSize];
                            fs.Read(data, 0, (int)lastPackSize);
                            arg.Data.Data = data;
                            _SendTcpChatFileMessage(arg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Info(ex + "");
                    }
                    OnSendTcpChatFileMessageHandler?.Invoke(this, arg);
                });
            }
        }
        private void _SendTcpChatFileMessage(SendTcpChatMessageEventArg<P2PChatFileModel> arg)
        {
            P2PMessageEventHandles.Instance.SendTcpMessage(new SendP2PTcpMessageArg
            {
                Socket = arg.Socket,
                Data = new P2PChatModel
                {
                    ChatType = (byte)arg.Data.ChatType,
                    FormId = EventHandlers.Instance.ConnectId,
                    ToId = arg.ToId,
                    Data = arg.Data.ProtobufSerialize()
                }
            });
        }

        #endregion
    }

    #region 聊天
    public class SendTcpChatMessageEventArg<T> : EventArgs
    {
        public Socket Socket { get; set; }
        public T Data { get; set; }
        public long ToId { get; set; } = 0;
    }
    #endregion
}
