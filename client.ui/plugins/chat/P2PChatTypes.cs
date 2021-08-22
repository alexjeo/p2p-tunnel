using ProtoBuf;
using System;

namespace client.ui.plugins.chat
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum P2PChatTypes : int
    {
        TEXT,FILE
    }

}