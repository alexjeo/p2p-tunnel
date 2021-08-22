using client.ui.plugins.request;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace client.ui.plugins.rdpDesktop
{
    [ProtoContract]
    public class RdpShareRequestExcuteMessage : IRequestExcuteMessage
    {
        [ProtoMember(1, IsRequired = true)]
        public RequestExcuteTypes Type { get; } = RequestExcuteTypes.GET;
    }

    [ProtoContract]
    public class RdpShareRequestReplyMessage : IRequestExcuteMessage
    {
        [ProtoMember(1, IsRequired = true)]
        public RequestExcuteTypes Type { get; } = RequestExcuteTypes.SEND;
        [ProtoMember(2)]
        public string ConnectString { get; set; }
    }

    [ProtoContract]
    public class RdpShareRequestResultMessage : IRequestExcuteMessage
    {
        [ProtoMember(1, IsRequired = true)]
        public RequestExcuteTypes Type { get; } = RequestExcuteTypes.RESULT;

        [ProtoMember(2)]
        public string ConnectString { get; set; }

        [ProtoMember(3)]
        public string Passeord { get; set; }
    }
}
