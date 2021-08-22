using ProtoBuf;
using System;

namespace client.ui.plugins.request
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum RequestExcuteTypes : short
    {
        GET,
        RESULT,
        SEND,
        FAIL
    }
}
