using client.service.serverPlugins.p2pMessage;
using client.ui.plugins.p2pMessage;
using common.extends;

namespace client.service.serverPlugins.chat
{
    public class P2PTcpChatPlugin : IP2PMessagePlugin
    {
        public P2PDataMessageTypes Type => P2PDataMessageTypes.CHAT;

        public void Excute(OnP2PTcpMessageArg arg)
        {
            P2PChatModel data = arg.Data.Data.ProtobufDeserialize<P2PChatModel>();
            P2PChatEventHandles.Instance.OnTcpChatMessage(data);
        }
    }
}
