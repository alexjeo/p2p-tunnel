using System;
using System.Collections.Generic;
using System.Text;

namespace client.service.serverPlugins.request
{
    public interface IRequestExcutePlugin
    {
        RequestExcuteTypes Type { get; }

        byte[] Excute(OnTcpRequestMsessageEventArg arg);
    }
}
