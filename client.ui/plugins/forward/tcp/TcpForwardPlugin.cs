
using client.ui.events;
using client.ui.plugins.p2pMessage;
using common;
using common.extends;
using server.model;
using server.models;
using server.plugin;

namespace client.ui.plugins.forward.tcp
{
    public class TcpForwardPlugin : IP2PMessagePlugin
    {
        public P2PDataMessageTypes Type => P2PDataMessageTypes.TCP_FORWARD;
        public void Excute(OnP2PTcpMessageArg arg)
        {
            TcpForwardModel data = arg.Data.Data.ProtobufDeserialize<TcpForwardModel>();
            TcpForwardEventHandles.Instance.OnTcpForwardMessage(new OnTcpForwardMessageEventArg
            {
                Packet = arg.Packet,
                Data = data,
            });
        }
    }
}
