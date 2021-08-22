using client.ui.events;
using client.ui.plugins.p2pMessage;
using client.ui.plugins.request;
using common.extends;
using server.model;
using server.models;
using server.plugin;

namespace client.ui.plugins.request
{
    public class RequestPlugin : IP2PMessagePlugin
    {
        public P2PDataMessageTypes Type => P2PDataMessageTypes.REQUEST;

        public void Excute(OnP2PTcpMessageArg arg)
        {
            RequestEventHandlers.Instance.OnTcpRequestMsessage(new OnTcpRequestMsessageEventArg
            {
                Data = arg.Data.Data.ProtobufDeserialize<MessageRequestModel>(),
                Packet = arg.Packet,
            });
        }
    }
}
