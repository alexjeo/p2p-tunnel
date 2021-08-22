using common.cache;
using common.extends;
using server.model;
using server.models;
using server.plugin;

namespace server.service.plugins
{
    /// <summary>
    /// 退出插件
    /// </summary>
    public class ExitPlugin : IPlugin
    {
        public MessageTypes MsgType => MessageTypes.SERVER_EXIT;

        public void Excute(PluginExcuteModel data, ServerType serverType)
        {
            MessageExitModel model = data.Packet.Chunk.ProtobufDeserialize<MessageExitModel>();
            ClientRegisterCache.Instance.Remove(model.Id);
        }
    }
}
