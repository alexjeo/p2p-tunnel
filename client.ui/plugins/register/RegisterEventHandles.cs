using client.ui.events;
using common;
using server;
using server.model;
using server.models;
using System;
using System.Net;
using System.Net.Sockets;

namespace client.ui.plugins.register
{
    public class RegisterEventHandles
    {
        private static readonly Lazy<RegisterEventHandles> lazy = new(() => new RegisterEventHandles());
        public static RegisterEventHandles Instance => lazy.Value;
        private RegisterParams requestCache;
        private long requestCacheId = 0;

        private RegisterEventHandles()
        {

        }

        private EventHandlers EventHandlers => EventHandlers.Instance;
        private IPEndPoint UdpServer => EventHandlers.UdpServer;
        private Socket TcpServer => EventHandlers.TcpServer;
        private long ConnectId => EventHandlers.ConnectId;

        public int ClientTcpPort => EventHandlers.ClientTcpPort;
        public int RouteLevel => EventHandlers.RouteLevel;

        /// <summary>
        /// 发送退出消息
        /// </summary>
        public event EventHandler<SendMessageEventArg> OnSendExitMessageHandler;
        /// <summary>
        /// 发送退出消息
        /// </summary>
        /// <param name="arg"></param>
        public void SendExitMessage()
        {
            requestCache = null;
            if (UdpServer != null)
            {
                SendMessageEventArg arg = new()
                {
                    Address = UdpServer,
                    Data = new MessageExitModel
                    {
                        Id = ConnectId,
                    }
                };

                UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                {
                    Address = arg.Address,
                    Data = arg.Data
                });
                UDPServer.Instance.Stop();
                TCPServer.Instance.Stop();

                EventHandlers.UdpServer = null;

                SendRegisterStateChange(new RegisterEventArg
                {
                    State = false
                });
                SendRegisterTcpStateChange(new RegisterTcpEventArg
                {
                    State = false
                });

                OnSendExitMessageHandler?.Invoke(this, arg);
            }
            EventHandlers.Sequence = 0;
        }

        #region 注册

        /// <summary>
        /// 发送注册消息
        /// </summary>
        public event EventHandler<string> OnSendRegisterMessageHandler;
        /// <summary>
        /// 发送注册消息
        /// </summary>
        /// <param name="arg"></param>
        public void SendRegisterMessage(RegisterParams param)
        {
            Helper.CloseTimeout(requestCacheId);
            EventHandlers.SendMessage(new SendMessageEventArg
            {
                Address = UdpServer,
                Data = new MessageRegisterModel
                {
                    Name = param.ClientName,
                    GroupId = param.GroupId,
                    LocalIps = param.LocalIps
                }
            });

            requestCache = param;
            requestCacheId = Helper.SetTimeout(() =>
           {
               if (requestCache != null)
               {
                   Action<RegisterMessageFailModel> callback = requestCache.FailCallback;
                   requestCache = null;
                   callback.Invoke(new RegisterMessageFailModel
                   {
                       Type = RegisterMessageFailType.TIMEOUT,
                       Msg = "注册超时"
                   });
               }
           }, param.Timeout);

            OnSendRegisterMessageHandler?.Invoke(this, param.ClientName);
        }

        /// <summary>
        /// 发送Tcp注册消息
        /// </summary>
        public event EventHandler<string> OnSendTcpRegisterMessageHandler;
        /// <summary>
        /// 发送Tcp注册消息
        /// </summary>
        /// <param name="arg"></param>
        public void SendTcpRegisterMessage(long id, string clientName, string groupId = "")
        {
            EventHandlers.SendTcpMessage(new SendTcpMessageEventArg
            {
                Socket = TcpServer,
                Data = new MessageRegisterModel
                {
                    Id = id,
                    Name = clientName,
                    GroupId = groupId
                }
            });
            OnSendTcpRegisterMessageHandler?.Invoke(this, clientName);
        }

        /// <summary>
        /// 注册状态发生变化
        /// </summary>
        public event EventHandler<RegisterEventArg> OnSendRegisterStateChangeHandler;
        /// <summary>
        /// 发布注册状态变化消息
        /// </summary>
        /// <param name="arg"></param>
        public void SendRegisterStateChange(RegisterEventArg arg)
        {
            OnSendRegisterStateChangeHandler?.Invoke(this, arg);
        }

        /// <summary>
        /// 注册Tcp状态发生变化
        /// </summary>
        public event EventHandler<RegisterTcpEventArg> OnSendRegisterTcpStateChangeHandler;
        /// <summary>
        /// 发布注册Tcp状态变化消息
        /// </summary>
        /// <param name="arg"></param>
        public void SendRegisterTcpStateChange(RegisterTcpEventArg arg)
        {
            OnSendRegisterTcpStateChangeHandler?.Invoke(this, arg);
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        public event EventHandler<OnRegisterResultEventArg> OnRegisterResultHandler;
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="arg"></param>
        public void OnRegisterResult(OnRegisterResultEventArg arg)
        {
            if (requestCache != null)
            {
                SendTcpRegisterMessage(arg.Data.Id, requestCache.ClientName, arg.Data.GroupId);
                SendRegisterStateChange(new RegisterEventArg
                {
                    ServerAddress = arg.Packet.SourcePoint,
                    ClientAddress = new IPEndPoint(IPAddress.Parse(arg.Data.Ip), arg.Data.Port),
                    State = true,
                    Id = arg.Data.Id,
                    ClientName = requestCache.ClientName
                });
                OnRegisterResultHandler?.Invoke(this, arg);
            }

        }

        /// <summary>
        /// 注册Tcp消息
        /// </summary>
        public event EventHandler<OnRegisterResultEventArg> OnRegisterTcpResultHandler;
        /// <summary>
        /// 注册Tcp消息
        /// </summary>
        /// <param name="arg"></param>
        public void OnRegisterTcpResult(OnRegisterResultEventArg arg)
        {
            if (requestCache != null)
            {
                requestCache.Callback?.Invoke(arg.Data);
                SendRegisterTcpStateChange(new RegisterTcpEventArg
                {
                    State = true,
                    Id = arg.Data.Id,
                    ClientName = requestCache.ClientName,
                    Ip = arg.Data.Ip,
                });
                requestCache = null;
                OnRegisterTcpResultHandler?.Invoke(this, arg);
            }

        }
        #endregion
    }

    #region 注册model
    public class OnRegisterResultEventArg : EventArgs
    {
        public PluginExcuteModel Packet { get; set; }
        public MessageRegisterResultModel Data { get; set; }
    }

    public class RegisterEventArg : EventArgs
    {
        public IPEndPoint ServerAddress { get; set; }
        public IPEndPoint ClientAddress { get; set; }
        public long Id { get; set; }
        public string ClientName { get; set; }
        public bool State { get; set; }
    }
    public class RegisterTcpEventArg : EventArgs
    {
        public string Ip { get; set; }
        public long Id { get; set; }
        public string ClientName { get; set; }
        public bool State { get; set; }
    }
    #endregion

    public class RegisterParams
    {
        public string GroupId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string LocalIps { get; set; } = string.Empty;
        public int Timeout { get; set; } = 15 * 1000;
        public Action<MessageRegisterResultModel> Callback { get; set; } = null;
        public Action<RegisterMessageFailModel> FailCallback { get; set; } = null;
    }

    public class RegisterMessageCache
    {
        public long Time { get; set; } = 0;
        public int Timeout { get; set; } = 15 * 60 * 1000;
        public Action<MessageRegisterResultModel> Callback { get; set; } = null;
        public Action<RegisterMessageFailModel> FailCallback { get; set; } = null;
    }

    public class RegisterMessageFailModel
    {
        public RegisterMessageFailType Type { get; set; } = RegisterMessageFailType.ERROR;
        public string Msg { get; set; } = string.Empty;
    }

    public enum RegisterMessageFailType
    {
        ERROR, TIMEOUT
    }

}
