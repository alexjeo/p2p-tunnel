using client.ui.plugins.p2pMessage;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.ui.plugins.request
{
    [ProtoContract]
    public class MessageRequestModel : IP2PMessageBase
    {
        public MessageRequestModel() { }

        [ProtoMember(1, IsRequired = true)]
        public P2PDataMessageTypes Type { get; } = P2PDataMessageTypes.REQUEST;

        [ProtoMember(2)]
        public byte[] Data { get; set; } = Array.Empty<byte>();

        [ProtoMember(3)]
        public long RequestId { get; set; } = 0;

        [ProtoMember(4, IsRequired = true)]
        public RequestExcuteTypes RequestType { get; set; } = RequestExcuteTypes.GET;

        [ProtoMember(5, IsRequired = true)]
        public MessageRequestResultCodes Code { get; set; } = MessageRequestResultCodes.OK;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum MessageRequestResultCodes : short
    {
        OK, NOTFOUND, FAIL
    }
}
