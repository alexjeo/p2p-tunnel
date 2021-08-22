using common.cache;
using common.extends;
using server.model;
using server.plugin;

namespace server.service.plugins
{
    public class HeartPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.HEART;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageHeartModel model = data.Packet.Chunk.ProtobufDeserialize<MessageHeartModel>();
            ClientRegisterCache.Instance.UpdateTime(model.SourceId);

            if (serverType == ServerType.UDP)
            {
                UDPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                {
                    Address = data.SourcePoint,
                    Data = new MessageHeartModel { TargetId = model.SourceId, SourceId = -1 }
                });
            }
            else if (serverType == ServerType.TCP)
            {
                TCPServer.Instance.Send(new MessageRecvQueueModel<IMessageModelBase>
                {
                    Address = data.SourcePoint,
                    TcpCoket = data.TcpSocket,
                    Data = new MessageHeartModel { TargetId = model.SourceId, SourceId = -1 }
                });
            }
        }
    }
}
