using client.service.serverPlugins.p2pMessage;
using common.extends;

namespace client.service.serverPlugins.request
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
