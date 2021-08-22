using client.ui.plugins.p2pMessage;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace client.ui.plugins.forward.tcp
{
    [ProtoContract]
    public class TcpForwardModel : IP2PMessageBase
    {
        public TcpForwardModel() { }

        [ProtoMember(1)]
        public long RequestId { get; set; } = 0;

        [ProtoMember(2)]
        public byte[] Buffer { get; set; } = new byte[0];

        [ProtoMember(3, IsRequired = true)]
        public TcpForwardType Type { get; set; } = TcpForwardType.REQUEST;

        [ProtoMember(4)]
        public string TargetIp { get; set; } = string.Empty;

        [ProtoMember(5)]
        public int TargetPort { get; set; } = 0;

        [ProtoMember(6, IsRequired = true)]
        public TcpForwardAliveTypes AliveType { get; set; } = TcpForwardAliveTypes.UNALIVE;

        [ProtoMember(7, IsRequired = true)]
        P2PDataMessageTypes IP2PMessageBase.Type { get; } = P2PDataMessageTypes.TCP_FORWARD;
    }


    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum TcpForwardType
    {
        REQUEST, RESPONSE, FAIL
    }
}
