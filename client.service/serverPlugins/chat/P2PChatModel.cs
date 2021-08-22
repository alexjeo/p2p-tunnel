using client.service.serverPlugins.p2pMessage;
using client.ui.plugins.p2pMessage;
using ProtoBuf;
using server.model;

namespace client.service.serverPlugins.chat
{
    [ProtoContract]
    public class P2PChatModel : IP2PMessageBase
    {
        public P2PChatModel() { }

        [ProtoMember(1, IsRequired = true)]
        public P2PDataMessageTypes Type { get; } = P2PDataMessageTypes.CHAT;

        [ProtoMember(2)]
        public byte[] Data { get; set; } = System.Array.Empty<byte>();

        [ProtoMember(3)]
        public long FormId { get; set; } = 0;

        [ProtoMember(4)]
        public long ToId { get; set; } = 0;

        [ProtoMember(5)]
        public byte ChatType { get; set; } = 0;
    }
}
