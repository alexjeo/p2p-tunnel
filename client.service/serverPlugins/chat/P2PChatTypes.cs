using ProtoBuf;
using System;

namespace client.service.serverPlugins.chat
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [Flags]
    public enum P2PChatTypes : int
    {
        TEXT,FILE
    }

}