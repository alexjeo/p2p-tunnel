using ProtoBuf;
using System;

namespace client.service.serverPlugins.p2pMessage
{
    public interface IP2PMessagePlugin
    {
        P2PDataMessageTypes Type { get; }

        void Excute(OnP2PTcpMessageArg arg);
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum P2PDataMessageTypes
    {
        REQUEST, TCP_FORWARD, CHAT
    }

    public interface IP2PMessageBase
    {
        P2PDataMessageTypes Type { get; }
    }
}
