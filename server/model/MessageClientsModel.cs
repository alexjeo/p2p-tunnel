using ProtoBuf;
using System.Collections.Generic;

namespace server.model
{

    [ProtoContract]
    public class MessageClientsModel : IMessageModelBase
    {
        public MessageClientsModel() { }

        [ProtoMember(1, IsRequired = true)]
        public MessageTypes MsgType { get; } = MessageTypes.SERVER_SEND_CLIENTS;

        [ProtoMember(2)]
        public IEnumerable<MessageClientsClientModel> Clients { get; set; }

    }

    [ProtoContract]
    public class MessageClientsClientModel
    {
        [ProtoMember(1)]
        public string Address { get; set; } = string.Empty;

        [ProtoMember(2)]
        public int Port { get; set; } = 0;

        [ProtoMember(3)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(4)]
        public long Id { get; set; } = 0;

        [ProtoMember(5)]
        public int TcpPort { get; set; } = 0;
    }
}
