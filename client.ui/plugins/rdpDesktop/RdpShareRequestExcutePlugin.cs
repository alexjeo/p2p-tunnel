using client.ui.plugins.request;
using common.extends;

namespace client.ui.plugins.rdpDesktop
{
    public class RdpShareRequestExcutePlugin : IRequestExcutePlugin
    {
        public RequestExcuteTypes Type => RequestExcuteTypes.GET;

        public byte[] Excute(OnTcpRequestMsessageEventArg arg)
        {
            return new RdpShareRequestResultMessage
            {
                ConnectString = RdpSahreHelper.Instance.ConnectString,
                Passeord = RdpSahreHelper.Instance.ViewModel.Password
            }.ProtobufSerialize();
        }
    }

    public class RdpShareRequestReplyPlugin : IRequestExcutePlugin
    {
        public RequestExcuteTypes Type => RequestExcuteTypes.SEND;

        public byte[] Excute(OnTcpRequestMsessageEventArg arg)
        {
            RdpShareRequestReplyMessage msg = arg.Data.Data.ProtobufDeserialize<RdpShareRequestReplyMessage>();
            RdpSahreHelper.Instance.ConnectClient(msg.ConnectString);
            return new byte[0];
        }
    }
}
