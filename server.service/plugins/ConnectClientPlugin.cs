using common.cache;
using common.extends;
using server.model;
using server.plugin;
using server.service.model;

namespace server.service.plugins
{
    /// <summary>
    /// 客户端链接插件
    /// </summary>
    public class ConnectClientPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageConnectClientModel model = data.Packet.Chunk.ProtobufDeserialize<MessageConnectClientModel>();

            //A已注册
            RegisterCacheModel source = ClientRegisterCache.Instance.Get(model.Id);
            if (source != null)
            {
                //B已注册
                RegisterCacheModel target = ClientRegisterCache.Instance.Get(model.ToId);
                if (target != null)
                {
                    if (serverType == ServerType.UDP)
                    {
                        //向B发送链接请求，告诉A谁要连接你，你给它回个消息
                        UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep1Model
                            {
                                Ip = source.Address.Address.ToString(),
                                Id = source.Id,
                                Name = source.Name,
                                Port = source.Address.Port,
                                TcpPort = source.TcpPort,
                                LocalIps = source.LocalIps
                            }
                        });
                    }
                    else if (serverType == ServerType.TCP)
                    {
                        //向A发送B的信息，等待A准备好
                        TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = source.Address,
                            TcpCoket = source.TcpSocket,
                            Data = new MessageConnectClientStep1Model
                            {
                                Ip = target.Address.Address.ToString(),
                                Id = target.Id,
                                Name = target.Name,
                                Port = target.Address.Port,
                                TcpPort = target.TcpPort,
                                LocalIps = target.LocalIps
                            }
                        });
                    }
                }
            }

        }
    }

    public class ConnectClientStep1ResultPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P_STEP_1_RESULT;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageConnectClientStep1ResultModel model = data.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep1ResultModel>();

            //已注册
            RegisterCacheModel source = ClientRegisterCache.Instance.Get(model.Id);
            if (source != null)
            {
                //已注册
                RegisterCacheModel target = ClientRegisterCache.Instance.Get(model.ToId);
                if (target != null)
                {
                    if (serverType == ServerType.UDP)
                    {
                        //向A发送信息，B已经准备好了，你去连接一下吧
                        UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2Model
                            {
                                Ip = source.Address.Address.ToString(),
                                Id = source.Id,
                                Name = source.Name,
                                Port = source.Address.Port,
                                TcpPort = source.TcpPort,
                                LocalIps = source.LocalIps

                            }
                        });
                    }
                    else if (serverType == ServerType.TCP)
                    {
                        //向B发送信息，A已经准备好了，你去连接一下吧
                        TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2Model
                            {
                                Ip = source.Address.Address.ToString(),
                                Id = source.Id,
                                Name = source.Name,
                                Port = source.Address.Port,
                                TcpPort = source.TcpPort,
                                LocalIps = source.LocalIps
                            }
                        });
                    }
                }
            }

        }
    }

    public class ConnectClientStep2RetryPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P_STEP_2_RETRY;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageConnectClientStep2RetryModel model = data.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep2RetryModel>();

            //已注册
            RegisterCacheModel source = ClientRegisterCache.Instance.Get(model.Id);
            if (source != null)
            {
                //已注册
                RegisterCacheModel target = ClientRegisterCache.Instance.Get(model.ToId);
                if (target != null)
                {
                    if (serverType == ServerType.UDP)
                    {
                        UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2RetryModel
                            {
                                Id = model.ToId,
                                ToId = model.Id,
                                Ip = source.Address.Address.ToString(),
                                Port = source.Address.Port,
                                TcpPort = source.TcpPort,
                                LocalIps = source.LocalIps
                            }
                        });
                    }
                    else if (serverType == ServerType.TCP)
                    {
                        TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2RetryModel
                            {
                                Id = model.ToId,
                                ToId = model.Id,
                                Ip = source.Address.Address.ToString(),
                                Port = source.Address.Port,
                                TcpPort = source.TcpPort,
                                LocalIps = source.LocalIps
                            }
                        });
                    }
                }
            }
        }
    }

    public class ConnectClientStep2FailPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_P2P_STEP_2_FAIL;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageConnectClientStep2FailModel model = data.Packet.Chunk.ProtobufDeserialize<MessageConnectClientStep2FailModel>();

            //已注册
            RegisterCacheModel source = ClientRegisterCache.Instance.Get(model.Id);
            if (source != null)
            {
                //已注册
                RegisterCacheModel target = ClientRegisterCache.Instance.Get(model.ToId);
                if (target != null)
                {
                    if (serverType == ServerType.UDP)
                    {
                        UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2FailModel
                            {
                                Id = model.Id,
                                ToId = model.ToId
                            }
                        });
                    }
                    else if (serverType == ServerType.TCP)
                    {
                        TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                        {
                            Address = target.Address,
                            TcpCoket = target.TcpSocket,
                            Data = new MessageConnectClientStep2FailModel
                            {
                                Id = model.Id,
                                ToId = model.ToId
                            }
                        });
                    }
                }
            }
        }
    }
}
