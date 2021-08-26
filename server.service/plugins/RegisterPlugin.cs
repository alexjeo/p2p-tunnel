using common;
using common.cache;
using common.extends;
using server.model;
using server.plugin;
using server.service.model;
using System;
using System.Net;

namespace server.service.plugins
{
    /// <summary>
    /// 注册插件  添加到缓存，再发送一条注册反馈，失败或者成功
    /// </summary>
    public class RegisterPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_REGISTER;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageRegisterModel model = data.Packet.Chunk.ProtobufDeserialize<MessageRegisterModel>();

            if (serverType == ServerType.UDP)
            {
                if (ClientRegisterCache.Instance.GetBySameGroup(model.GroupId, model.Name) == null)
                {
                    RegisterCacheModel add = new()
                    {
                        Address = data.SourcePoint,
                        Name = model.Name,
                        LastTime = Helper.GetTimeStamp(),
                        TcpSocket = null,
                        TcpPort = 0,
                        GroupId = model.GroupId,
                        LocalIps = model.LocalIps,
                        Mac = model.Mac,
                        LocalPort = model.LocalTcpPort

                    };
                    long id = ClientRegisterCache.Instance.Add(add, 0);

                    UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                    {
                        Address = data.SourcePoint,
                        TcpCoket = null,
                        Data = new MessageRegisterResultModel
                        {
                            Id = id,
                            Ip = data.SourcePoint.Address.ToString(),
                            Port = data.SourcePoint.Port,
                            TcpPort = 0,
                            GroupId = add.GroupId,
                            Mac = add.Mac,
                            LocalTcpPort = add.LocalPort
                        }
                    });
                }
                else
                {
                    UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                    {
                        Address = data.SourcePoint,
                        TcpCoket = null,
                        Data = new MessageRegisterResultModel
                        {
                            Code = -1,
                            Msg = "组中已存在同名客户端"
                        }
                    });
                }
            }
            else if (serverType == ServerType.TCP)
            {
                int tcpPort = 0;
                if (data.TcpSocket != null)
                {
                    tcpPort = IPEndPoint.Parse(data.TcpSocket.RemoteEndPoint.ToString()).Port;
                }
                _ = ClientRegisterCache.Instance.UpdateTcpInfo(model.Id, data.TcpSocket, tcpPort);

                TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                {
                    Address = data.SourcePoint,
                    TcpCoket = data.TcpSocket,
                    Data = new MessageRegisterResultModel
                    {
                        Id = model.Id,
                        Ip = data.SourcePoint.Address.ToString(),
                        Port = data.SourcePoint.Port,
                        TcpPort = tcpPort,
                        GroupId = model.GroupId,
                        Mac = model.Mac,
                        LocalTcpPort = model.LocalTcpPort
                    }
                });
            }
        }
    }
}
