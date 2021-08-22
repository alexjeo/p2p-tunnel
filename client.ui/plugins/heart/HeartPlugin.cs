
using client.ui.events;
using common.extends;
using server.model;
using server.plugin;

namespace client.ui.plugins.heart
{
    /// <summary>
    /// 心跳包
    /// </summary>
    public class HeartPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.HEART;

        public void Excute(PluginExcuteModel model, ServerType serverType)
        {
            MessageHeartModel data = model.Packet.Chunk.ProtobufDeserialize<MessageHeartModel>();
            HeartEventHandles.Instance.OnHeartMessage(new OnHeartEventArg
            {
                Packet = model,
                Data = data
            });
        }
    }
}
